using Hackney.Core.Testing.Shared;
using Xunit;

namespace NotesApi.Tests
{
    [CollectionDefinition("LogCall collection")]
    public class LogCallAspectFixtureCollection : ICollectionFixture<LogCallAspectFixture>
    { }
}
