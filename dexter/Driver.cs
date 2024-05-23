namespace Dexter;

using OpenAI;
using OpenAI.Chat;
using Serilog;

public class Driver: IAsyncEnumerable<OpenAI.Chat.Message>
{
    readonly OpenAIClient _client;
    readonly ILogger _logger;
    readonly string _request;
    public List<OpenAI.Chat.Message> Messages { get; set; } = new();
    public List<Tool> Tools { get; } = new();
    public int? RemainingTokens { get; private set; }
    public int? TotalTokens { get; private set; }

    public Driver(OpenAiOptions options, string request)
    {
        _logger = Log.ForContext<Driver>();

        _client = new OpenAIClient(options.ApiKey);
        _request = request;

        Messages.Add(new (Role.System, "You are a helpful user assistent."));
        Messages.Add(new (Role.User, _request));

        Tools.Add(Tool.FromFunc("users_name", () => Task.FromResult("Dru"), "Gives you the user's name"));
    }


    public async IAsyncEnumerator<OpenAI.Chat.Message> GetAsyncEnumerator(CancellationToken ct = new CancellationToken())
    {
        foreach (var m in Messages)
            yield return m;

        // initial prompt
        var chatRequest = new ChatRequest(Messages, tools: Tools, toolChoice: "auto");
        var response = await _client.ChatEndpoint.GetCompletionAsync(chatRequest, ct);
        // add first messages back
        Messages.Add(response.FirstChoice.Message);
        yield return response.FirstChoice.Message;

        // iterate over all tool calls and invoke them
        foreach (var toolCall in response.FirstChoice.Message.ToolCalls)
        {
            _logger.Debug("{Role}: {FunctionName} | Finish Reason: {FinishReason}", response.FirstChoice.Message.Role, toolCall.Function.Name, response.FirstChoice.FinishReason);
            _logger.Debug(" args: {Args}", toolCall.Function.Arguments);
            // Invokes function to get a generic json result to return for tool call.
            var functionResult = await toolCall.InvokeFunctionAsync(ct);
            // If you know the return type and do additional processing you can use generic overload
            // var functionResult = await toolCall.InvokeFunctionAsync<string>();
            yield return new OpenAI.Chat.Message(toolCall, functionResult);
            Messages.Add(new OpenAI.Chat.Message(toolCall, functionResult));
            _logger.Debug("{Role}: {FunctionResult}", Role.Tool, functionResult);
        }

        // call with tool data
        chatRequest = new ChatRequest(Messages, tools: Tools, toolChoice: "auto");
        response = await _client.ChatEndpoint.GetCompletionAsync(chatRequest, ct);

        _logger.Debug(".. {Role}: {M} | Finish Reason: {FinishReason}",
            response.FirstChoice.Message.Role,
            response.FirstChoice,
            response.FirstChoice.FinishReason);

        yield return response.FirstChoice.Message;
        Messages.Add(response.FirstChoice.Message);

        // Context Windows
        RemainingTokens = response.RemainingTokens;
        TotalTokens = response.Usage.TotalTokens;
    }


}
