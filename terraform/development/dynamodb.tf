resource "aws_dynamodb_table" "notesapi_dynamodb_table" {
    name                  = "Notes"
    billing_mode          = "PROVISIONED"
    read_capacity         = 10
    write_capacity        = 10
    hash_key              = "targetId"
    range_key             = "id"

    attribute {
        name              = "id"
        type              = "S"
    }

    attribute {
        name              = "targetId"
        type              = "S"
    }

    attribute {
        name              = "dateTime"
        type              = "S"
    }

    local_secondary_index {
        name              = "NotesByDate"
        range_key         = "createdAt"
        projection_type   = "ALL"
    }

    tags = {
        Name              = "notes-api-${var.environment_name}"
        Environment       = var.environment_name
        terraform-managed = true
        project_name      = var.project_name
    }
}
