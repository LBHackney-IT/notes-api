using Xunit;

namespace NotesApi.Tests
{
    [CollectionDefinition("AppTest collection", DisableParallelization = true)]
    public class AppTestCollection : ICollectionFixture<AwsMockWebApplicationFactory<Startup>>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}

