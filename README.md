# Notes API

The Notes API stores and returns notes for housing records in the MTFH application. A note is attached to a person, asset, tenure, repair, or process.

Version 1 list results leave out the Anti-social behaviour category. Version 2 returns every category, and creating a note in version 2 publishes an event.

Build, test, and delivery steps are in [CONTRIBUTION.md](CONTRIBUTION.md).

## Endpoints

v1 request and response models are in `NotesApi.V1.Boundary`. v2 models are in `NotesApi.V2.Boundary`. `PagedResult<T>` is `Hackney.Core.DynamoDb.PagedResult`. `HealthCheckResponse` is `Hackney.Core.HealthCheck.HealthCheckResponse`.

| Verb | Route | Request | Response | Summary |
| --- | --- | --- | --- | --- |
| GET | `/api/v1/notes` | `GetNotesByTargetIdQuery` | `PagedResult<NoteResponseObject>` | Returns notes for `targetId`. Accepts `paginationToken` and `pageSize`. Omits the Anti-social behaviour category. Returns 404 when nothing remains. |
| POST | `/api/v1/notes` | `CreateNoteRequest` | `NoteResponseObject` | Creates a note and returns it. Does not publish an event. |
| GET | `/api/v2/notes` | `GetNotesByTargetIdQuery` | `PagedResult<NoteResponseObject>` | Returns notes for `targetId`, including every category. Accepts `paginationToken` and `pageSize`. Returns 404 when nothing is found. |
| POST | `/api/v2/notes` | `CreateNoteRequest` | `NoteResponseObject` | Creates a note, returns it, and publishes a note-created event. The user on the event is taken from the bearer token. |
| GET | `/api/v1/healthcheck/ping` | — | `HealthCheckResponse` | Checks that the Notes DynamoDB table can be reached. |

Creating a note in v2 publishes one of these event types, chosen from the note's target:

| Event type | Summary |
| --- | --- |
| `NoteCreatedAgainstPersonEvent` | A note was created against a person. |
| `NoteCreatedAgainstAssetEvent` | A note was created against an asset. |
| `NoteCreatedAgainstTenureEvent` | A note was created against a tenure. |
| `NoteCreatedAgainstRepairEvent` | A note was created against a repair. |
| `NoteCreatedAgainstProcessEvent` | A note was created against a process. |

The message is an `EntityEventSns` with source domain `Notes`, source system `NotesAPI`, and version `V2`.

## Linked repositories

Public GitHub has no `notes-shared`, `notes-api-shared`, `notes-listener`, or `notes-api-listener` repository. Terraform for the Notes table lives in this repository.

| Repository | Summary |
| --- | --- |
| [notes-api](https://github.com/LBHackney-IT/notes-api) | This API. |
| [lbh-core](https://github.com/LBHackney-IT/lbh-core) | Shared `Hackney.Core` packages for DynamoDB, SNS, JWT, request context, logging, middleware, validation, and health checks. |

## Data sources

The API does not call another service over HTTP. `Hackney.Core.Http` reads the incoming request.

| Source | Summary |
| --- | --- |
| DynamoDB table `Notes` | Stores notes. Hash key `targetId`, range key `id`, local secondary index `NotesByCreated`. Read and written by `NotesDbGateway`. |
| Amazon SNS (`NOTES_SNS_ARN`) | Receives the v2 note-created event through `ISnsGateway`. |

## Tech stack

- .NET 10 (`net10.0`) ASP.NET Core, hosted on AWS Lambda (`dotnet10`) behind API Gateway.
- API Gateway Lambda authorizer on the note routes. Swagger UI is unauthenticated.
- DynamoDB for storage. SNS for v2 note-created events. AWS X-Ray for tracing.
- Serverless Framework packages and deploys the Lambda. Terraform in this repository manages the Notes table and related configuration for development, staging, pre-production, and production.
- The Lambda runs in the environment VPC. CircleCI builds, analyses, tests, and deploys.
- xUnit for tests. Swashbuckle serves Swagger UI. Docker Compose runs the API with DynamoDB Local and LocalStack.

## Contacts

**Housing product team** at [mmh-project-team@hackney.gov.uk](mailto:mmh-project-team@hackney.gov.uk)
