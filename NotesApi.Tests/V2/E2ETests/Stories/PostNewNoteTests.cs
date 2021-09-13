using System;
using NotesApi.Tests.V2.E2ETests.Fixtures;
using NotesApi.Tests.V2.E2ETests.Steps;
using NotesApi.V2.Boundary.Request.Validation;
using TestStack.BDDfy;
using Xunit;

namespace NotesApi.Tests.V2.E2ETests.Stories
{
    [Story(
        AsA = "Internal Hackney user (such as a Housing Officer or Area housing Manager)",
        IWant = "to add a new note against a Person",
        SoThat = "I can add/track important information against a person in one place")]
    [Collection("DynamoDb collection")]
    public class PostNewNoteTests : IDisposable
    {
        private readonly DynamoDbIntegrationTests<Startup> _dbFixture;
        private readonly NotesFixture _notesFixture;
        private readonly PostNoteSteps _steps;

        public PostNewNoteTests(DynamoDbIntegrationTests<Startup> dbFixture)
        {
            _dbFixture = dbFixture;
            _notesFixture = new NotesFixture(_dbFixture.DynamoDbContext);
            _steps = new PostNoteSteps(_dbFixture.Client);
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
        public void PostingANoteTestNoteCreated()
        {
            this.Given(g => _notesFixture.GivenANewNoteIsCreated())
                .When(w => _steps.WhenPostingANote(_notesFixture))
                .Then(t => _steps.ThenTheNoteHasBeenPersisted(_notesFixture))
                .BDDfy();
        }

        [Fact]
        public void PostingANoteTestInvalidPayloadReturns400()
        {
            this.Given(g => _notesFixture.GivenAnInvalidNewNotePayload())
                .When(w => _steps.WhenPostingTheInvalidPayload(_notesFixture))
                .Then(t => _steps.ThenBadRequestIsReturned())
                .BDDfy();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void PostingANoteTestNoDescriptionReturns400(string desc)
        {
            this.Given(g => _notesFixture.GivenANewNotePayloadWithDescription(desc))
                .When(w => _steps.WhenPostingANote(_notesFixture))
                .Then(t => _steps.ThenBadRequestValidationErrorResultIsReturned("Description", ErrorCodes.DescriptionMandatory))
                .BDDfy();
        }

        [Fact]
        public void PostingANoteTestDescriptionTooLongReturns400()
        {
            this.Given(g => _notesFixture.GivenANewNotePayloadWithTooLongDescription())
                .When(w => _steps.WhenPostingANote(_notesFixture))
                .Then(t => _steps.ThenBadRequestValidationErrorResultIsReturned("Description", ErrorCodes.DescriptionTooLong,
                                    "'Description' must be between 1 and 500 characters."))
                .BDDfy();
        }

        [Fact]
        public void PostingANoteTestNoTargetIdReturns400()
        {
            this.Given(g => _notesFixture.GivenANewNotePayloadWithNoTargetId())
                .When(w => _steps.WhenPostingANote(_notesFixture))
                .Then(t => _steps.ThenBadRequestValidationErrorResultIsReturned("TargetId"))
                .BDDfy();
        }

        [Fact]
        public void PostingANoteTestNoTargetTypeReturns400()
        {
            this.Given(g => _notesFixture.GivenANewNotePayloadWithNoTargetType())
                .When(w => _steps.WhenPostingANote(_notesFixture))
                .Then(t => _steps.ThenBadRequestValidationErrorResultIsReturned("TargetType"))
                .BDDfy();
        }

        [Fact]
        public void PostingANoteTestNoCreatedAtReturns400()
        {
            this.Given(g => _notesFixture.GivenANewNotePayloadWithNoCreatedAt())
                .When(w => _steps.WhenPostingANote(_notesFixture))
                .Then(t => _steps.ThenBadRequestValidationErrorResultIsReturned("CreatedAt"))
                .BDDfy();
        }
    }
}
