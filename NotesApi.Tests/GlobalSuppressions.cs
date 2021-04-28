// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>", Scope = "member", Target = "~M:NotesApi.Tests.V1.E2ETests.Steps.GetNotesSteps.PostToApi(NotesApi.V1.Domain.Queries.CreateNoteRequest)~System.Threading.Tasks.Task{System.Net.Http.HttpResponseMessage}")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>", Scope = "member", Target = "~M:NotesApi.Tests.V1.E2ETests.Steps.GetNotesSteps.ThenTheNoteHasBeenPersisted(NotesApi.V1.Boundary.Response.NoteResponseObject,Amazon.DynamoDBv2.DataModel.IDynamoDBContext)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>", Scope = "member", Target = "~M:NotesApi.Tests.V1.E2ETests.Fixtures.NotesFixture.GivenANewNoteIsCreated")]
