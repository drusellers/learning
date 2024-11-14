namespace CoreChannels;

using System.Threading.Channels;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task Test1()
    {
        var channel = Channel.CreateBounded<Message>(7);

        Task.Run(async () =>
        {
            // Queue up data
            foreach (var i in Enumerable.Range(0, 9))
            {
                // do i need to await?
                await channel.Writer.WriteAsync(new Message(i));
                await Task.Delay(10);
                TestContext.WriteLine($"Write {i}");
            }
            channel.Writer.Complete();
        });


        while (channel.Reader.TryRead(out var m))
        {
            TestContext.WriteLine($"Read {m.I}");
        }

        await channel.Reader.Completion;

        TestContext.WriteLine("Done");
    }
}

public record Message(int I);
