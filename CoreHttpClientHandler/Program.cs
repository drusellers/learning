// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var http = new HttpClient(new LoggingHandler(new HttpClientHandler()), true);


var resp = await http.GetAsync("https://httpbin.org/get");
Console.WriteLine(resp.StatusCode);

public class LoggingHandler : DelegatingHandler
{
    public LoggingHandler(HttpMessageHandler innerHandler) : base(innerHandler)
    {
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // pre
        Console.WriteLine("BEFORE");

        var response = await base.SendAsync(request, cancellationToken);

        // post
        Console.WriteLine("AFTER");

        return response;
    }
}
