namespace CoreTesting;

using NUnit.Framework;

[SetUpFixture]
public class EnvironmentSetUp
{
    [OneTimeSetUp]
    public async Task OneTime()
    {
        await Task.Delay(1);
        TestContext.WriteLine("Run once per");
    }

    // NOT ALLOWED
    // [SetUp]
    // public void SetUp()
    // {
    //     Console.WriteLine("HUH2");
    // }
}

public class ATest
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        TestContext.WriteLine("OneTimeSetUp");
    }

    [SetUp]
    public void SetUp()
    {
        TestContext.WriteLine("  SetUp");
    }

    [Test]
    public void Test1()
    {
        TestContext.WriteLine("    Test1");
    }

    [Test]
    public void Test2()
    {
        TestContext.WriteLine("    Test2");
    }
}

public class BTest
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        TestContext.WriteLine("OneTimeSetUp");
    }

    [SetUp]
    public void SetUp()
    {
        TestContext.WriteLine("  SetUp");
    }

    [Test]
    public void Test1()
    {
        TestContext.WriteLine("    Test1");
    }

    [Test]
    public void Test2()
    {
        TestContext.WriteLine("    Test2");
    }
}
