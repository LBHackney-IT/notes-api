using Amazon.DynamoDBv2.DataModel;
using AutoFixture;
using NotesApi.V1.Domain;
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
                                       .With(x => x.DateTime, funcDT)
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
    }
}
