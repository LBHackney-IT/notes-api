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

        [Fact]
        public void PagedResultConstructorSetPaginationToken()
        {
            var paginationDetails = new PaginationDetails();
            var sut = new PagedResult<string>(null, paginationDetails);
            sut.PaginationDetails.Should().Be(paginationDetails);
        }
    }
}
