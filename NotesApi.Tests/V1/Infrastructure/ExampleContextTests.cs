using System.Linq;
using NotesApi.Tests.V1.Helper;
using NotesApi.V1.Infrastructure;
using NUnit.Framework;

namespace NotesApi.Tests.V1.Infrastructure
{
    [TestFixture]//, Ignore("Database type not confirmed yet")]
    public class DatabaseContextTest : DatabaseTests
    {
        [Test]
        public void CanGetADatabaseEntity()
        {
            var databaseEntity = DatabaseEntityHelper.CreateDatabaseEntity();

            DatabaseContext.Add(databaseEntity);
            DatabaseContext.SaveChanges();

            var result = DatabaseContext.DatabaseEntities.FirstOrDefault();

            Assert.AreEqual(result, databaseEntity);
        }
    }
}
