using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using NotesApi.V1;
using Xunit;

namespace NotesApi.Tests.V1
{
    public class PaginationDetailsTests
    {
        [Fact]
        public void DefaultConstructorTest()
        {
            var sut = new PaginationDetails();
            sut.MoreToken.Should().BeNull();
            sut.HasMore.Should().BeFalse();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("{}")]
        public void CustomConstructorTestEmptyToken(string token)
        {
            var sut = new PaginationDetails(token);
            sut.MoreToken.Should().BeNull();
            sut.HasMore.Should().BeFalse();
        }

        [Theory]
        [InlineData("some value")]
        [InlineData("{ \"id\": \"123\", \"name\": \"some name\"  }")]
        public void CustomConstructorTestWithTokenValue(string token)
        {
            var sut = new PaginationDetails(token);
            sut.MoreToken.Should().Be(Base64UrlEncoder.Encode(token));
            sut.HasMore.Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("{}")]
        public void EncodeMoreTokenTestEmptyToken(string token)
        {
            var sut = new PaginationDetails();
            sut.EncodeMoreToken(token);
            sut.MoreToken.Should().BeNull();
            sut.HasMore.Should().BeFalse();
        }

        [Theory]
        [InlineData("some value")]
        [InlineData("{ \"id\": \"123\", \"name\": \"some name\"  }")]
        public void EncodeMoreTokenTestWithTokenValue(string token)
        {
            var sut = new PaginationDetails();
            sut.EncodeMoreToken(token);
            sut.MoreToken.Should().Be(Base64UrlEncoder.Encode(token));
            sut.HasMore.Should().BeTrue();
        }

        [Fact]
        public void DecodeMoreTokenTestEmptyToken()
        {
            var sut = new PaginationDetails();
            sut.DecodeMoreToken().Should().BeNull();
        }

        [Theory]
        [InlineData("some value")]
        [InlineData("{ \"id\": \"123\", \"name\": \"some name\"  }")]
        public void DecodeMoreTokenTestWithTokenValue(string token)
        {
            var sut = new PaginationDetails(token);
            sut.DecodeMoreToken().Should().Be(token);
        }
    }
}
