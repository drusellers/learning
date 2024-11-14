// See https://aka.ms/new-console-template for more information

using System.Threading.Channels;

Console.WriteLine("Starting!");


var channel = Channel.CreateBounded<int>(new BoundedChannelOptions(7));

Task.Run(() =>
{
    while (true)
    {
        channel.Writer.WriteAsync(Random.Shared.Next(10));
        Thread.Sleep(500);
    }
});

await foreach (var output in channel.Reader.ReadAllAsync())
{
    Console.WriteLine(output);
}
