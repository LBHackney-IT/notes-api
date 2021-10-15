resource "aws_dynamodb_table" "notesapi_dynamodb_table" {
  name           = "Notes"
  billing_mode   = "PROVISIONED"
  read_capacity  = 10
  write_capacity = 10
  hash_key       = "targetId"
  range_key      = "id"

  attribute {
    name = "id"
    type = "S"
  }

  attribute {
    name = "targetId"
    type = "S"
  }

  attribute {
    name = "createdAt"
    type = "S"
  }

  local_secondary_index {
    name            = "NotesByCreated"
    range_key       = "createdAt"
    projection_type = "ALL"
  }

  tags = merge(
    local.default_tags,
    { BackupPolicy = "Stg" }
  )

  point_in_time_recovery {
    enabled = true
  }
}

