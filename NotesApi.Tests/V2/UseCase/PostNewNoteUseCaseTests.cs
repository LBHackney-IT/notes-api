using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Hackney.Core.JWT;
using Hackney.Core.Sns;
using Moq;
using NotesApi.V2.Boundary.Request;
using NotesApi.V2.Boundary.Response;
using NotesApi.V2.Domain;
using NotesApi.V2.Factories;
using NotesApi.V2.Gateways;
using NotesApi.V2.UseCase;
using Xunit;

namespace NotesApi.Tests.V2.UseCase
{
    [Collection("LogCall collection")]
    public class PostNewNoteUseCaseTests
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly PostNewNoteUseCase _classUnderTest;
        private readonly Mock<INotesGateway> _mockGateway;
        private readonly ISnsFactory _snsFactory;
        private readonly Mock<ISnsGateway> _mockSnsGateway;

        public PostNewNoteUseCaseTests()
        {
            _mockGateway = new Mock<INotesGateway>();
            _snsFactory = new SnsFactory();
            _mockSnsGateway = new Mock<ISnsGateway>();

            _classUnderTest = new PostNewNoteUseCase(_mockGateway.Object, _snsFactory, _mockSnsGateway.Object);
        }

        [Fact]
        public async Task PostNewNotCaseShouldCallNoteGatewayAndSnsGateway()
        {
            // Arrange
            var request = _fixture.Create<CreateNoteRequest>();
            var note = _fixture.Create<Note>();

            _mockGateway.Setup(x => x.PostNewNoteAsync(request)).ReturnsAsync(note);

            // Act
            await _classUnderTest.ExecuteAsync(request, new Token()).ConfigureAwait(false);

            // Assert
            _mockGateway.Verify(x => x.PostNewNoteAsync(request));
            _mockSnsGateway.Verify(x => x.Publish(It.IsAny<EntityEventSns>(), It.IsAny<String>(), "fake"));
        }

        [Fact]
        public async Task PostNewNoteUseCaseExceptionIsThrown()
        {
            // Arrange
            var exception = new ApplicationException("Test exception");
            _mockGateway.Setup(x => x.PostNewNoteAsync(It.IsAny<CreateNoteRequest>())).ThrowsAsync(exception);

            // Act
            Func<Task<NoteResponseObject>> func = async () =>
                await _classUnderTest.ExecuteAsync(new CreateNoteRequest(), new Token()).ConfigureAwait(false);

            // Assert
            (await func.Should().ThrowAsync<ApplicationException>()).WithMessage(exception.Message);
        }
    }
}
