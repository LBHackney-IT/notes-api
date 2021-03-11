using AutoFixture;
using NotesApi.Tests.V1.Helper;
using NotesApi.V1.Domain;
using NotesApi.V1.Gateways;
using FluentAssertions;
using NUnit.Framework;

namespace NotesApi.Tests.V1.Gateways
{
    //For instruction on how to run tests please see the wiki: https://github.com/LBHackney-IT/lbh-base-api/wiki/Running-the-test-suite.
    [TestFixture]//, Ignore("Database type not confirmed yet")]
    public class NotesApiGatewayTests : DatabaseTests
    {
        private readonly Fixture _fixture = new Fixture();
        private NotesApiGateway _classUnderTest;

        [SetUp]
        public void Setup()
        {
            _classUnderTest = new NotesApiGateway(DatabaseContext);
        }

        [Test]
        public void GetEntityByIdReturnsNullIfEntityDoesntExist()
        {
            var response = _classUnderTest.GetEntityById(123);

            response.Should().BeNull();
        }

        [Test]
        public void GetEntityByIdReturnsTheEntityIfItExists()
        {
            var entity = _fixture.Create<Entity>();
            var databaseEntity = DatabaseEntityHelper.CreateDatabaseEntityFrom(entity);

            DatabaseContext.DatabaseEntities.Add(databaseEntity);
            DatabaseContext.SaveChanges();

            var response = _classUnderTest.GetEntityById(databaseEntity.Id);

            databaseEntity.Id.Should().Be(response.Id);
            databaseEntity.CreatedAt.Should().BeSameDateAs(response.CreatedAt);
        }
    }
}
