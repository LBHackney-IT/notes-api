resource "aws_dynamodb_table" "notesapi_dynamodb_table" {
    name                  = "Notes"
    billing_mode          = "PROVISIONED"
    read_capacity         = 10
    write_capacity        = 10
    hash_key              = "id"

    attribute {
        name              = "id"
        type              = "S"
    }

    tags = {
        Name              = "notes-api-${var.environment_name}"
        Environment       = var.environment_name
        terraform-managed = true
        project_name      = var.project_name
    }
}

resource "aws_iam_policy" "notesapi_dynamodb_table_policy" {
    name                  = "lambda-dynamodb-notes-api"
    description           = "A policy allowing read/write operations on notes dynamoDB for the notes API"
    path                  = "/notes-api/"

    policy                = <<EOF
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Effect": "Allow",
            "Action": [
                        "dynamodb:BatchGetItem",
                        "dynamodb:GetItem",
                        "dynamodb:Query",
                        "dynamodb:Scan",
                        "dynamodb:BatchWriteItem",
                        "dynamodb:PutItem",
                        "dynamodb:UpdateItem"
                     ],
            "Resource": "${aws_dynamodb_table.notesapi_dynamodb_table.arn}"
        }
    ]
}
EOF
}