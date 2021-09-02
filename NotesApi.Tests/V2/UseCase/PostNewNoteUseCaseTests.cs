using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NotesApi.V2.Boundary.Request;
using NotesApi.V2.Boundary.Response;
using NotesApi.V2.Domain;
using NotesApi.V2.Gateways;
using NotesApi.V2.UseCase;
using Xunit;

namespace NotesApi.Tests.V2.UseCase
{
    [Collection("LogCall collection")]
    public class PostNewNoteUseCaseTests
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<INotesGateway> _mockGateway;
        private readonly PostNewNoteUseCase _classUnderTest;

        public PostNewNoteUseCaseTests()
        {
            _mockGateway = new Mock<INotesGateway>();
            _classUnderTest = new PostNewNoteUseCase(_mockGateway.Object);
        }

        [Fact]
        public async Task PostNewNotCaseShouldCallNoteGateway()
        {
            // Arrange
            _mockGateway.Setup(x => x.PostNewNoteAsync(It.IsAny<CreateNoteRequest>()))
                .ReturnsAsync(_fixture.Create<Note>());

            // Act
            await _classUnderTest.ExecuteAsync(new CreateNoteRequest()).ConfigureAwait(false);

            // Assert
            _mockGateway.Verify(x => x.PostNewNoteAsync(It.IsAny<CreateNoteRequest>()));
        }

        [Fact]
        public void PostNewNoteUseCaseExceptionIsThrown()
        {
            // Arrange
            var exception = new ApplicationException("Test exception");
            _mockGateway.Setup(x => x.PostNewNoteAsync(It.IsAny<CreateNoteRequest>())).ThrowsAsync(exception);

            // Act
            Func<Task<NoteResponseObject>> func = async () =>
                await _classUnderTest.ExecuteAsync(new CreateNoteRequest()).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }
    }
}
