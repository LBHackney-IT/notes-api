#!/bin/sh
# Remove a leftover pre-production Terraform lock after a killed apply.
# A lock younger than STALE_LOCK_SECONDS is left in place.
set -eu

lock_table="${TF_LOCK_TABLE:-housing-pre-production-terraform-state-lock}"
lock_path="${TF_LOCK_PATH:-housing-pre-production-terraform-state/services/notes-api/state}"
stale_after_seconds="${STALE_LOCK_SECONDS:-900}"

export AWS_DEFAULT_REGION="${AWS_DEFAULT_REGION:-eu-west-2}"
export AWS_PAGER=""

log() {
  printf '%s\n' "$*"
}

item=$(aws dynamodb get-item \
  --table-name "$lock_table" \
  --key "{\"LockID\":{\"S\":\"${lock_path}\"}}" \
  --output json)
info=$(printf '%s\n' "$item" | jq -r '.Item.Info.S // empty')

if [ -z "$info" ]; then
  log "No Terraform state lock is held."
  exit 0
fi

lock_id=$(printf '%s\n' "$info" | jq -r '.ID // empty')
created=$(printf '%s\n' "$info" | jq -r '.Created // empty')

if [ -z "$lock_id" ] || [ -z "$created" ]; then
  log "Terraform lock record is missing an ID or creation time."
  exit 1
fi

age_seconds=$(python3 -c '
import datetime, re, sys
raw = sys.argv[1].strip().replace("Z", "+00:00")
raw = re.sub(r"\.(\d+)", lambda match: "." + (match.group(1) + "000000")[:6], raw, count=1)
created = datetime.datetime.fromisoformat(raw)
now = datetime.datetime.now(datetime.timezone.utc)
print(int((now - created).total_seconds()))
' "$created")

log "Found a Terraform lock aged ${age_seconds}s."

if [ "$age_seconds" -lt "$stale_after_seconds" ]; then
  log "Lock is newer than ${stale_after_seconds}s, so it was left in place."
  exit 1
fi

log "Releasing stale Terraform lock."
terraform force-unlock -force "$lock_id"
