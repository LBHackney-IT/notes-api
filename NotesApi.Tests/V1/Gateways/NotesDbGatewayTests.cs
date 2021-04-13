using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using AutoFixture;
using FluentAssertions;
using Moq;
using NotesApi.Tests.V1.Helper;
using NotesApi.V1.Domain;
using NotesApi.V1.Gateways;
using NotesApi.V1.Infrastructure;
using System;
using System.Threading.Tasks;
using Xunit;

// The DynamoDbGateway.GetByTargetIdAsync() method is untestable, because due to the need to implement pagination
// we are forced to use GetTargetTable and then perform the necessary operation on the Table object.
// However the Table class is a concrete one and does not have a public default constructor, meaning that
// it cannot be mocked or used in isloation, without connecting to a real database instance.
// See here: https://github.com/aws/aws-sdk-net/issues/1310

// This class can be kept here commented out until the time other gateway methods are implemented that can be unit tested.

//namespace NotesApi.Tests.V1.Gateways
//{
//    public class NotesDbGatewayTests
//    {
//        //private readonly Fixture _fixture = new Fixture();
//        private readonly Mock<IDynamoDBContext> _dynamoDb;
//        private readonly Mock<Table> _mockTable;
//        private readonly DynamoDbGateway _classUnderTest;

//        public DynamoDbGatewayTests()
//        {
//            _dynamoDb = new Mock<IDynamoDBContext>();
//            _mockTable = new Mock<Table>();
//            _dynamoDb.Setup(x => x.GetTargetTable<NoteDb>(null)).Returns(_mockTable.Object);
//            _classUnderTest = new DynamoDbGateway(_dynamoDb.Object);
//        }

//        [Fact]
//        public async Task GetByTargetIdReturnsEmptyIfNoRecords()
//        {
//            var response = await _classUnderTest.GetByTargetIdAsync(Guid.NewGuid()).ConfigureAwait(false);
//            response.Should().BeEmpty();
//        }

//    }
//}
