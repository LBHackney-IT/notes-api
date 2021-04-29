using NotesApi.Tests.V1.E2ETests.Fixtures;
using NotesApi.Tests.V1.E2ETests.Steps;
using System;
using System.Diagnostics.CodeAnalysis;
using TestStack.BDDfy;
using Xunit;

namespace NotesApi.Tests.V1.E2ETests.Stories
{
    [Story(
        AsA = "Internal Hackney user (such as a Housing Officer or Area housing Manager)",
        IWant = "to be able to view notes against a person",
        SoThat = "all the relevant commentary with regards to that person can be stored and viewed in one place")]
    [Collection("DynamoDb collection")]
    public class GetNotesByTargetIdTests : IDisposable
    {
        private readonly DynamoDbIntegrationTests<Startup> _dbFixture;
        private readonly NotesFixture _notesFixture;
        private readonly GetNotesSteps _steps;

        public GetNotesByTargetIdTests(DynamoDbIntegrationTests<Startup> dbFixture)
        {
            _dbFixture = dbFixture;
            _notesFixture = new NotesFixture(_dbFixture.DynamoDbContext);
            _steps = new GetNotesSteps(_dbFixture.Client);
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
                if (null != _notesFixture)
                    _notesFixture.Dispose();

                _disposed = true;
            }
        }

        [Fact]
        [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "BDDfy")]
        public void ServiceReturnsTheRequestedNotes()
        {
            this.Given(g => _notesFixture.GivenTargetNotesAlreadyExist())
                .When(w => _steps.WhenTheTargetNotesAreRequested(_notesFixture.TargetId.ToString()))
                .Then(t => _steps.ThenTheTargetNotesAreReturned(_notesFixture.Notes))
                .BDDfy();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(5)]
        [InlineData(15)]
        [InlineData(100)]
        [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "BDDfy")]
        public void ServiceReturnsTheRequestedNotesByPageSize(int? pageSize)
        {
            this.Given(g => _notesFixture.GivenTargetNotesAlreadyExist(30))
                .When(w => _steps.WhenTheTargetNotesAreRequestedWithPageSize(_notesFixture.TargetId.ToString(), pageSize))
                .Then(t => _steps.ThenTheTargetNotesAreReturnedByPageSize(_notesFixture.Notes, pageSize))
                .BDDfy();
        }

        [Fact]
        [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "BDDfy")]
        public void ServiceReturnsFirstPageOfRequestedNotesWithPaginationToken()
        {
            this.Given(g => _notesFixture.GivenTargetNotesWithMultiplePagesAlreadyExist())
                .When(w => _steps.WhenTheTargetNotesAreRequested(_notesFixture.TargetId.ToString()))
                .Then(t => _steps.ThenTheFirstPageOfTargetNotesAreReturned(_notesFixture.Notes))
                .BDDfy();
        }

        [Fact]
        [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "BDDfy")]
        public void ServiceReturnsAllPagesNotesUsingPaginationToken()
        {
            this.Given(g => _notesFixture.GivenTargetNotesWithMultiplePagesAlreadyExist())
                .When(w => _steps.WhenAllTheTargetNotesAreRequested(_notesFixture.TargetId.ToString()))
                .Then(t => _steps.ThenAllTheTargetNotesAreReturned(_notesFixture.Notes))
                .BDDfy();
        }

        [Fact]
        [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "BDDfy")]
        public void ServiceReturnsNotFoundIfNoNotesExist()
        {
            this.Given(g => _notesFixture.GivenATargetIdHasNoNotes())
                .When(w => _steps.WhenTheTargetNotesAreRequested(_notesFixture.TargetId.ToString()))
                .Then(t => _steps.ThenNotFoundIsReturned())
                .BDDfy();
        }

        [Fact]
        [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "BDDfy")]
        public void ServiceReturnsBadRequestIfIdInvalid()
        {
            this.Given(g => _notesFixture.GivenAnInvalidTargetId())
                .When(w => _steps.WhenTheTargetNotesAreRequested(_notesFixture.InvalidTargetId))
                .Then(t => _steps.ThenBadRequestIsReturned())
                .BDDfy();
        }

        [Fact]
        [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "BDDfy")]
        public void PostingANoteTest()
        {
            this.Given(g => _notesFixture.GivenANewNoteIsCreated())
                .When(w => _steps.WhenPostingANote(_notesFixture.NoteRequest, _notesFixture))
                .Then(t => _steps.ThenTheNoteHasBeenPersisted(_notesFixture.NoteResponse))
                .BDDfy();
        }
    }
}
