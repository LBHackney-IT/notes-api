using Amazon.DynamoDBv2.DataModel;
using AutoFixture;
using NotesApi.V1.Boundary.Response;
using NotesApi.V1.Domain;
using NotesApi.V1.Domain.Queries;
using NotesApi.V1.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NotesApi.Tests.V1.E2ETests.Fixtures
{
    public class NotesFixture : IDisposable
    {
        private readonly Fixture _fixture = new Fixture();

        private readonly IDynamoDBContext _dbContext;

        public List<NoteDb> Notes { get; private set; } = new List<NoteDb>();

        public Guid TargetId { get; private set; }

        public string InvalidTargetId { get; private set; }

        public const int MAXNOTES = 10;

        public CreateNoteRequest NoteRequest { get; private set; }

        public NoteResponseObject NoteResponse { get; set; }

        public string InvalidPayload { get; private set; }

        public NotesFixture(IDynamoDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                if (Notes.Any())
                    foreach (var note in Notes)
                        _dbContext.DeleteAsync(note).GetAwaiter().GetResult();

                _disposed = true;
            }
        }

        private static CreateNoteRequest CreateNote()
        {
            var note = new Fixture().Create<CreateNoteRequest>();

            note.TargetId = Guid.NewGuid();
            note.CreatedAt = DateTime.Now;
            note.Author.Email = "something@somewhere.com";
            return note;
        }

        public void GivenANewNoteIsCreated()
        {
            NoteRequest = CreateNote();
        }

        public void GivenTargetNotesAlreadyExist()
        {
            GivenTargetNotesAlreadyExist(MAXNOTES);
        }

        public void GivenTargetNotesAlreadyExist(int count)
        {
            if (!Notes.Any())
            {
                var random = new Random();
                TargetId = Guid.NewGuid();
                Func<DateTime> funcDT = () => DateTime.UtcNow.AddDays(0 - random.Next(100));
                Notes.AddRange(_fixture.Build<NoteDb>()
                                       .With(x => x.CreatedAt, funcDT)
                                       .With(x => x.TargetType, TargetType.person)
                                       .With(x => x.TargetId, TargetId)
                                       .CreateMany(count));
                foreach (var note in Notes)
                    _dbContext.SaveAsync(note).GetAwaiter().GetResult();
            }
        }

        public void GivenTargetNotesWithMultiplePagesAlreadyExist()
        {
            GivenTargetNotesAlreadyExist(35);
        }

        public void GivenATargetIdHasNoNotes()
        {
            TargetId = Guid.NewGuid();
        }

        public void GivenAnInvalidTargetId()
        {
            InvalidTargetId = "12345667890";
        }

        public void GivenAnInvalidNewNotePayload()
        {
            InvalidPayload = "This is invalid json";
        }

        public void GivenANewNotePayloadWithTooLongDescription()
        {
            NoteRequest = CreateNote();
            var msgToRepeat = "This description is to long. ";
            string description = "";
            while (description.Length <= 500)
                description += msgToRepeat;
            NoteRequest.Description = description;
        }

        public void GivenANewNotePayloadWithNoTargetId()
        {
            NoteRequest = CreateNote();
            NoteRequest.TargetId = null;
        }

        public void GivenANewNotePayloadWithNoTargetType()
        {
            NoteRequest = CreateNote();
            NoteRequest.TargetType = null;
        }

        public void GivenANewNotePayloadWithNoCreatedAt()
        {
            NoteRequest = CreateNote();
            NoteRequest.CreatedAt = null;
        }
    }
}
