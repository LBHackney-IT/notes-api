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
            sut.NextToken.Should().BeNull();
            sut.HasNext.Should().BeFalse();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("{}")]
        public void CustomConstructorTestEmptyToken(string token)
        {
            var sut = new PaginationDetails(token, token);
            sut.NextToken.Should().BeNull();
            sut.HasNext.Should().BeFalse();
            sut.PreviousToken.Should().BeNull();
            sut.HasPrevious.Should().BeFalse();
        }

        [Theory]
        [InlineData("some value")]
        [InlineData("{ \"id\": \"123\", \"name\": \"some name\"  }")]
        public void CustomConstructorTestWithTokenValue(string token)
        {
            var sut = new PaginationDetails(token, token);
            sut.NextToken.Should().Be(Base64UrlEncoder.Encode(token));
            sut.HasNext.Should().BeTrue();
            sut.PreviousToken.Should().Be(Base64UrlEncoder.Encode(token));
            sut.HasPrevious.Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("{}")]
        public void EncodeNextTokenTestEmptyToken(string token)
        {
            var sut = new PaginationDetails();
            sut.EncodeNextToken(token);
            sut.NextToken.Should().BeNull();
            sut.HasNext.Should().BeFalse();
        }

        [Theory]
        [InlineData("some value")]
        [InlineData("{ \"id\": \"123\", \"name\": \"some name\"  }")]
        public void EncodePreviousTokenTestWithTokenValue(string token)
        {
            var sut = new PaginationDetails();
            sut.EncodePreviousToken(token);
            sut.PreviousToken.Should().Be(Base64UrlEncoder.Encode(token));
            sut.HasPrevious.Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("{}")]
        public void EncodePreviousTokenTestEmptyToken(string token)
        {
            var sut = new PaginationDetails();
            sut.EncodePreviousToken(token);
            sut.PreviousToken.Should().BeNull();
            sut.HasPrevious.Should().BeFalse();
        }

        [Theory]
        [InlineData("some value")]
        [InlineData("{ \"id\": \"123\", \"name\": \"some name\"  }")]
        public void EncodeNextTokenTestWithTokenValue(string token)
        {
            var sut = new PaginationDetails();
            sut.EncodeNextToken(token);
            sut.NextToken.Should().Be(Base64UrlEncoder.Encode(token));
            sut.HasNext.Should().BeTrue();
        }

        [Fact]
        public void DecodeTokensTestEmptyToken()
        {
            var sut = new PaginationDetails();
            sut.DecodeNextToken().Should().BeNull();
            sut.DecodePreviousToken().Should().BeNull();
        }

        [Theory]
        [InlineData("some value")]
        [InlineData("{ \"id\": \"123\", \"name\": \"some name\"  }")]
        public void DecodeTokensTestWithTokenValue(string token)
        {
            var sut = new PaginationDetails(token, token);
            sut.DecodeNextToken().Should().Be(token);
            sut.DecodePreviousToken().Should().Be(token);
        }
    }
}
