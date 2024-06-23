namespace Application.IntegrationTests;

using static Testing;

[TestFixture]
public abstract class BaseTestFixture
{
    [SetUp]
    public async Task TaskSetUp()
    {
        await ResetState();
    }
}
