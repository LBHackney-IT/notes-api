resource "aws_ssm_parameter" "aspnetcore_environment" {
  name  = "/housing-tl/pre-production/aspnetcore-environment"
  type  = "String"
  value = "to_be_set_manually"

  lifecycle {
    ignore_changes = [
      value,
    ]
  }
}
