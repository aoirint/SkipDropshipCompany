#!/usr/bin/env bash

set -euo pipefail

install_archive() {
  local name=$1 version=$2 archive=$3 url=$4 sha256=$5
  shift 5
  local archive_path="${workflow_tools_directory}/${archive}"
  local tool_dir="${workflow_tools_directory}/${name}-${version}"

  curl \
    --fail \
    --location \
    --retry 3 \
    --retry-connrefused \
    --retry-delay 2 \
    --show-error \
    --silent \
    --output "$archive_path" \
    "$url"
  printf '%s  %s\n' "$sha256" "$archive_path" | sha256sum --check --strict -
  mkdir -p "$tool_dir"
  tar -xzf "$archive_path" -C "$tool_dir" "$@"
  printf '%s\n' "$tool_dir" >>"$GITHUB_PATH"
}

main() {
  workflow_tools_directory=$(mktemp -d "${RUNNER_TEMP}/workflow-tools.XXXXXX")
  trap 'rm -rf -- "$workflow_tools_directory"' ERR

  install_archive \
    shellcheck 0.11.0 \
    shellcheck-v0.11.0.linux.x86_64.tar.gz \
    https://github.com/koalaman/shellcheck/releases/download/v0.11.0/shellcheck-v0.11.0.linux.x86_64.tar.gz \
    b7af85e41cc99489dcc21d66c6d5f3685138f06d34651e6d34b42ec6d54fe6f6 \
    --strip-components=1 shellcheck-v0.11.0/shellcheck
  install_archive \
    actionlint 1.7.12 \
    actionlint_1.7.12_linux_amd64.tar.gz \
    https://github.com/rhysd/actionlint/releases/download/v1.7.12/actionlint_1.7.12_linux_amd64.tar.gz \
    8aca8db96f1b94770f1b0d72b6dddcb1ebb8123cb3712530b08cc387b349a3d8 \
    actionlint
  install_archive \
    pinact 4.1.1 \
    pinact_linux_amd64.tar.gz \
    https://github.com/suzuki-shunsuke/pinact/releases/download/v4.1.1/pinact_linux_amd64.tar.gz \
    d1cffebe5704b74e2e5f8a864efb9f7e54768972dc686188c008033fb1797841 \
    pinact

  printf 'installation-directory=%s\n' "$workflow_tools_directory" >>"$GITHUB_OUTPUT"
  trap - ERR
}

main "$@"
