using AutoFixture;
using FluentAssertions;
using NotesApi.V1;
using System.Linq;
using Xunit;

namespace NotesApi.Tests.V1
{
    public class PagedResultTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public void PagedResultConstructorNullResultsIsEmpty()
        {
            var sut = new PagedResult<string>(null, null);
            sut.Results.Should().BeEmpty();
        }

        [Fact]
        public void PagedResultConstructorEmptyResultsIsEmpty()
        {
            var sut = new PagedResult<string>(Enumerable.Empty<string>(), null);
            sut.Results.Should().BeEmpty();
        }

        [Fact]
        public void PagedResultConstructorSetResults()
        {
            var list = _fixture.CreateMany<string>(10);
            var sut = new PagedResult<string>(list, null);
            sut.Results.Should().BeEquivalentTo(list);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("{}")]
        public void PagedResultConstructorSetEmptyPaginationToken(string token)
        {
            var sut = new PagedResult<string>(null, token);
            sut.PaginationToken.Should().BeNull();
        }

        [Theory]
        [InlineData("some value")]
        [InlineData("{ \"id\": \"123\", \"name\": \"some name\"  }")]
        public void PagedResultConstructorSetPaginationTokenValue(string token)
        {
            var sut = new PagedResult<string>(null, token);
            sut.PaginationToken.Should().Be(token.Replace("\"", "'"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("{}")]
        public void PagedResultPaginationTokenSetsEmptyValue(string token)
        {
            var sut = new PagedResult<string>();
            sut.PaginationToken = token;
            sut.PaginationToken.Should().BeNull();
        }

        [Theory]
        [InlineData("some value")]
        [InlineData("{ \"id\": \"123\", \"name\": \"some name\"  }")]
        public void PagedResultPaginationTokenSetsValue(string token)
        {
            var sut = new PagedResult<string>();
            sut.PaginationToken = token;
            sut.PaginationToken.Should().Be(token.Replace("\"", "'"));
        }
    }
}
