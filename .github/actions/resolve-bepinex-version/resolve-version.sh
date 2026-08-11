#!/usr/bin/env bash

set -euo pipefail

require_environment() {
  local name
  for name in "$@"; do
    if [[ ! -v "$name" ]]; then
      printf 'Missing required environment variable: %s\n' "$name" >&2
      return 1
    fi
  done
}

read_project_version() {
  local project_file=$1
  sed -n '/APP_VERSION_MARKER/{n; s|<Version>\(.*\)</Version>|\1|; p;}' \
    "$project_file" | tr -d '\r' | xargs
}

resolve_release_mode() {
  local project_version=$1 override=$2
  if [[ -n "$override" ]]; then
    printf '%s\n' "$override"
  elif [[ "$(git rev-parse --is-shallow-repository)" != "false" ]]; then
    printf 'Version planning requires a full checkout with tags.\n' >&2
    return 1
  elif git rev-parse "refs/tags/v${project_version}" >/dev/null 2>&1; then
    printf 'edge\n'
  elif [[ "$project_version" == "0.0.0" ]]; then
    printf 'edge\n'
  elif [[ "$project_version" == *-* ]]; then
    printf 'prerelease\n'
  else
    printf 'latest\n'
  fi
}

resolve_app_version() {
  local project_version=$1 release_mode=$2 override=$3
  if [[ -n "$override" ]]; then
    printf '%s\n' "$override"
  elif [[ "$release_mode" == "edge" ]]; then
    printf '%s-edge.d%s.g%s\n' \
      "$project_version" "$(date -u '+%Y%m%d%H%M%S')" "${GITHUB_SHA:0:7}"
  else
    printf '%s\n' "$project_version"
  fi
}

resolve_manifest_version() {
  local app_version=$1 release_mode=$2 override=$3
  if [[ -n "$override" ]]; then
    printf '%s\n' "$override"
  elif [[ "$release_mode" == "latest" ]]; then
    printf '%s\n' "$app_version"
  else
    printf '0.0.0\n'
  fi
}

write_package_versions() {
  local project_file=$1 app_version=$2 manifest_version=$3
  local manifest_path=assets/manifest.json
  local manifest_temp
  manifest_temp=$(mktemp "${RUNNER_TEMP}/manifest.XXXXXX.json")
  trap 'rm -f "$manifest_temp"' RETURN

  sed -i \
    "/APP_VERSION_MARKER/{n; s|<Version>.*</Version>|<Version>${app_version}</Version>|;}" \
    "$project_file"
  jq --arg version "$manifest_version" \
    '.version_number = $version' "$manifest_path" >"$manifest_temp"
  mv "$manifest_temp" "$manifest_path"
  trap - RETURN
}

write_outputs() {
  local release_mode=$1 app_version=$2 manifest_version=$3
  {
    printf 'release_mode=%s\n' "$release_mode"
    printf 'app_version=%s\n' "$app_version"
    printf 'git_version=v%s\n' "$app_version"
    printf 'manifest_version=%s\n' "$manifest_version"
  } >>"$GITHUB_OUTPUT"
}

main() {
  require_environment \
    VERSION_CSPROJ_FILE VERSION_RELEASE_MODE VERSION_APP_VERSION \
    VERSION_MANIFEST_VERSION VERSION_WRITE_FILES GITHUB_OUTPUT GITHUB_SHA \
    RUNNER_TEMP

  if [[ "$VERSION_WRITE_FILES" != "true" && "$VERSION_WRITE_FILES" != "false" ]]; then
    printf 'VERSION_WRITE_FILES must be true or false.\n' >&2
    return 1
  fi

  local project_version release_mode app_version manifest_version
  project_version=$(read_project_version "$VERSION_CSPROJ_FILE")
  if [[ -z "$project_version" ]]; then
    printf 'Could not find a Version element after APP_VERSION_MARKER in %s.\n' \
      "$VERSION_CSPROJ_FILE" >&2
    return 1
  fi

  release_mode=$(resolve_release_mode "$project_version" "$VERSION_RELEASE_MODE")
  app_version=$(
    resolve_app_version "$project_version" "$release_mode" "$VERSION_APP_VERSION"
  )
  manifest_version=$(
    resolve_manifest_version \
      "$app_version" "$release_mode" "$VERSION_MANIFEST_VERSION"
  )

  if [[ "$VERSION_WRITE_FILES" == "true" ]]; then
    write_package_versions \
      "$VERSION_CSPROJ_FILE" "$app_version" "$manifest_version"
  fi
  write_outputs "$release_mode" "$app_version" "$manifest_version"
}

main "$@"
