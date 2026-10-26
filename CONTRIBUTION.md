# Contributing to Notes API

## Build and run

Install the .NET 10 SDK. `NuGet.Config` restores `Hackney.Core` packages from the LBHackney-IT GitHub feed, and that feed reads `LBHPACKAGESTOKEN` from the environment.

```powershell
dotnet restore NotesApi.sln
dotnet build NotesApi.sln
```

Local DynamoDB listens on port 8000 and LocalStack SNS listens on port 4566. Start them from the repository root:

```powershell
docker compose up dynamodb-database localstack
```

The `notes_api` launch profile sets `ASPNETCORE_ENVIRONMENT` to `LocalDevelopment` and points DynamoDB and SNS at those local addresses. The API then listens on `http://localhost:5000`, and the health check is `http://localhost:5000/api/v1/healthcheck/ping`.

```powershell
dotnet run --project NotesApi --launch-profile notes_api
```

`docker compose up notes-api` builds `NotesApi/Dockerfile` and runs the API in a container. The image build also needs `LBHPACKAGESTOKEN`.

`NotesApi/build.sh` installs Amazon.Lambda.Tools 4.0.0 and writes `NotesApi/bin/release/net10.0/notes-api.zip`. On Windows, `NotesApi/build.cmd` runs the same package command and expects `dotnet lambda` to already be on the path. Serverless deploys that zip. The runtime id is `dotnet10`.

## Testing

```powershell
dotnet test NotesApi.sln
```

Gateway tests and end-to-end tests use the `AppTest` collection. That fixture creates the Notes table in DynamoDB Local and a `notes.fifo` topic in LocalStack. Those tests need the containers from the previous section, plus `AWS_ACCESS_KEY_ID` and `AWS_SECRET_ACCESS_KEY`. Docker Compose sets both to `local` for the test container.

The CircleCI test image is `NotesApi.Tests/Dockerfile`. The pipeline builds it and runs:

```text
docker-compose run -T notes-api-test dotnet test --logger "console;verbosity=normal" -tl:off
```

`-T` and `-tl:off` keep the test step from going silent long enough for CircleCI to stop it.

## Development

Branch from `master`. Pull requests merge back to `master`. CircleCI runs the formatter, SonarCloud, and the tests on that branch. Terraform apply and the Lambda deploy to development run only on `master`.

`release` is the branch that deploys staging and, after approval, production. Pre-production terraform and code deploys also run from `release`, each behind its own approval. Update `release` by merging `master`. Do not branch feature work from `release`.

Use the pull request template. It asks for the Jira ticket, the problem, the change, and any follow-up after merge.
