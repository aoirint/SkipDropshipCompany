#!/usr/bin/env bash
# SPDX-License-Identifier: MIT

set -euo pipefail

artifact_pattern="${1:?Usage: publish-thunderstore.sh <package.zip>}"

: "${THUNDERSTORE_TOKEN:?THUNDERSTORE_TOKEN is required}"
: "${THUNDERSTORE_NAMESPACE:?THUNDERSTORE_NAMESPACE is required}"
: "${THUNDERSTORE_COMMUNITY:?THUNDERSTORE_COMMUNITY is required}"
: "${THUNDERSTORE_COMMUNITY_CATEGORIES:?THUNDERSTORE_COMMUNITY_CATEGORIES is required}"

thunderstore_repo="${THUNDERSTORE_REPO:-https://thunderstore.io}"

if [ -f "${artifact_pattern}" ]; then
  artifact_path="${artifact_pattern}"
else
  # Release publishing must fail if packaging ever creates zero or multiple
  # zips; Thunderstore submissions should map to exactly one package version.
  mapfile -t artifact_matches < <(compgen -G "${artifact_pattern}")

  if [ "${#artifact_matches[@]}" -ne 1 ]; then
    echo "Expected exactly one Thunderstore artifact, found ${#artifact_matches[@]} for: ${artifact_pattern}" >&2
    exit 1
  fi

  artifact_path="${artifact_matches[0]}"
fi

artifact_name=$(basename "${artifact_path}")
artifact_size=$(wc -c < "${artifact_path}" | tr -d '[:space:]')

api_post() {
  local url=$1
  curl --fail-with-body --silent --show-error \
    --request POST \
    --header "Accept: application/json" \
    --header "Authorization: Bearer ${THUNDERSTORE_TOKEN}" \
    --header "Content-Type: application/json" \
    --data @- \
    "${url}"
}

upload_response=$(
  jq --null-input --compact-output \
    --arg filename "${artifact_name}" \
    --argjson file_size_bytes "${artifact_size}" \
    '{filename: $filename, file_size_bytes: $file_size_bytes}' \
    | api_post "${thunderstore_repo}/api/experimental/usermedia/initiate-upload/"
)

upload_uuid=$(jq --raw-output '.user_media.uuid' <<< "${upload_response}")
parts_file=$(mktemp)
trap 'rm -f "${parts_file}"' EXIT

# Thunderstore returns presigned upload parts for larger files; upload each
# byte range exactly as requested and collect the ETags required to finalize.
jq --compact-output '.upload_urls[]' <<< "${upload_response}" | while read -r upload_part; do
  part_number=$(jq --raw-output '.part_number' <<< "${upload_part}")
  part_offset=$(jq --raw-output '.offset' <<< "${upload_part}")
  part_length=$(jq --raw-output '.length' <<< "${upload_part}")
  part_url=$(jq --raw-output '.url' <<< "${upload_part}")
  headers_file=$(mktemp)

  dd if="${artifact_path}" bs=1 skip="${part_offset}" count="${part_length}" status=none \
    | curl --fail-with-body --silent --show-error \
      --request PUT \
      --dump-header "${headers_file}" \
      --output /dev/null \
      --data-binary @- \
      "${part_url}"

  etag=$(
    awk 'tolower($1) == "etag:" { value = $2; sub(/\r$/, "", value); print value }' "${headers_file}" \
      | tail -n 1
  )
  rm -f "${headers_file}"

  if [ -z "${etag}" ]; then
    echo "Thunderstore upload part ${part_number} did not return an ETag header." >&2
    exit 1
  fi

  jq --null-input --compact-output \
    --arg etag "${etag}" \
    --argjson part_number "${part_number}" \
    '{"ETag": $etag, "PartNumber": $part_number}' \
    >> "${parts_file}"
done

jq --slurp --compact-output '{parts: .}' "${parts_file}" \
  | api_post "${thunderstore_repo}/api/experimental/usermedia/${upload_uuid}/finish-upload/" \
  > /dev/null

community_categories=$(
  # Workflow YAML keeps categories readable as lines; Thunderstore requires a
  # compact JSON array under the community-specific category map.
  printf '%s\n' "${THUNDERSTORE_COMMUNITY_CATEGORIES}" \
    | tr '[:space:]' '\n' \
    | sed '/^$/d' \
    | jq --raw-input . \
    | jq --slurp --compact-output .
)

submission_response=$(
  jq --null-input --compact-output \
    --arg author_name "${THUNDERSTORE_NAMESPACE}" \
    --arg community "${THUNDERSTORE_COMMUNITY}" \
    --arg upload_uuid "${upload_uuid}" \
    --argjson community_categories "${community_categories}" \
    '{
      author_name: $author_name,
      categories: [],
      community_categories: {($community): $community_categories},
      communities: [$community],
      has_nsfw_content: false,
      upload_uuid: $upload_uuid
    }' \
    | api_post "${thunderstore_repo}/api/experimental/submission/submit/"
)

download_url=$(jq --raw-output '.package_version.download_url // empty' <<< "${submission_response}")
community_url=$(jq --raw-output '.available_communities[0].url // empty' <<< "${submission_response}")

{
  echo "thunderstore_download_url=${download_url}"
  echo "thunderstore_community_url=${community_url}"
} >> "${GITHUB_OUTPUT}"
