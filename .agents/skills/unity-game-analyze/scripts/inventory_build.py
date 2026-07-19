# /// script
# requires-python = ">=3.11,<3.14"
# dependencies = []
# [tool.uv]
# exclude-newer = "P7D"
# ///
"""Create a read-only SHA-256 inventory for build-local evidence roots."""

from __future__ import annotations

import argparse
import csv
import hashlib
import json
from pathlib import Path


def sha256(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as stream:
        for block in iter(lambda: stream.read(1024 * 1024), b""):
            digest.update(block)
    return digest.hexdigest()


def main() -> None:
    parser = argparse.ArgumentParser()
    parser.add_argument("--root", action="append", required=True, help="Evidence root; repeat as needed")
    parser.add_argument("--output", required=True, type=Path)
    args = parser.parse_args()
    output = args.output.resolve()
    if output.exists():
        raise SystemExit(f"no-clobber output exists: {output}")
    roots = [Path(value).resolve() for value in args.root]
    for root in roots:
        if not root.is_dir():
            raise SystemExit(f"not a directory: {root}")
        if output == root or root in output.parents:
            raise SystemExit("output must not be inside an inventoried root")
    rows = []
    for root_id, root in enumerate(roots, 1):
        for path in sorted((value for value in root.rglob("*") if value.is_file()), key=lambda value: value.as_posix().casefold()):
            stat = path.stat()
            rows.append({"root_id": root_id, "root": str(root), "relative_path": path.relative_to(root).as_posix(),
                         "bytes": stat.st_size, "mtime_ns": stat.st_mtime_ns, "sha256": sha256(path)})
    output.parent.mkdir(parents=True, exist_ok=True)
    with output.open("w", encoding="utf-8", newline="") as stream:
        writer = csv.DictWriter(stream, fieldnames=["root_id", "root", "relative_path", "bytes", "mtime_ns", "sha256"])
        writer.writeheader()
        writer.writerows(rows)
    print(json.dumps({"status": "PASS", "roots": len(roots), "files": len(rows), "output": str(output), "sha256": sha256(output)}))


if __name__ == "__main__":
    main()
