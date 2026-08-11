#!/usr/bin/env bash

set -euo pipefail

SKILL_ROOT=$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)
readonly SKILL_ROOT
readonly SCRIPT_PATH="${SKILL_ROOT}/assets/github/actions/resolve-bepinex-version/resolve-version.sh"
FIXTURE_ROOT=''

fail() {
  printf 'test failure: %s\n' "$1" >&2
  return 1
}

assert_contains() {
  local path=$1 expected=$2
  grep -Fqx "$expected" "$path" || fail "${path} does not contain ${expected}"
}

create_fixture() {
  local root=$1 version=$2
  mkdir -p "${root}/assets" "${root}/runner-temp"
  cat >"${root}/Project.csproj" <<EOF
<Project>
  <!-- APP_VERSION_MARKER -->
  <Version>${version}</Version>
</Project>
EOF
  printf '{"version_number":"0.0.0"}\n' >"${root}/assets/manifest.json"
  git -C "$root" init --quiet
}

run_guard() {
  local root=$1 write_files=$2
  (
    cd "$root"
    VERSION_CSPROJ_FILE=Project.csproj \
      VERSION_RELEASE_MODE='' \
      VERSION_APP_VERSION='' \
      VERSION_MANIFEST_VERSION='' \
      VERSION_WRITE_FILES=$write_files \
      GITHUB_OUTPUT="${root}/outputs" \
      GITHUB_SHA=0123456789abcdef \
      RUNNER_TEMP="${root}/runner-temp" \
      bash "$SCRIPT_PATH"
  )
}

test_stable_version_updates_outputs_and_files() {
  local root=$1
  create_fixture "$root" 1.2.3
  run_guard "$root" true

  assert_contains "${root}/outputs" release_mode=latest
  assert_contains "${root}/outputs" app_version=1.2.3
  assert_contains "${root}/outputs" git_version=v1.2.3
  assert_contains "${root}/outputs" manifest_version=1.2.3
  grep -Fq '<Version>1.2.3</Version>' "${root}/Project.csproj"
  grep -Fq '"version_number": "1.2.3"' "${root}/assets/manifest.json"
}

test_invalid_write_flag_fails() {
  local root=$1
  create_fixture "$root" 1.2.3
  if run_guard "$root" invalid 2>/dev/null; then
    fail 'invalid write flag succeeded'
  fi
}

main() {
  FIXTURE_ROOT=$(mktemp -d)
  trap 'rm -rf -- "$FIXTURE_ROOT"' EXIT

  test_stable_version_updates_outputs_and_files "${FIXTURE_ROOT}/stable"
  test_invalid_write_flag_fails "${FIXTURE_ROOT}/invalid"
}

main "$@"
