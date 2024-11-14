namespace Dexter.FromAlt.Providers.OpenAi;

using System.Diagnostics;
using Microsoft.Extensions.Logging;
using OpenAI;
using OpenAI.Chat;

/// <inheritdoc />
[DebuggerDisplay("{_originalQuestion}")]
public class OpenAiQuestion : IQuestion
{
    readonly OpenAIClient _client;
    readonly ILogger _logger;
    readonly List<Message> _messages = new();
    readonly string _originalQuestion;

    readonly string _systemPrompt = """
                                    You are an assistant that helps a company's accounting department search and manage their expenses related to SaaS applications.

                                    Please respond using markdown
                                    """;

    readonly List<Tool> _tools = new();

    /// <summary>
    /// ctor
    /// </summary>
    public OpenAiQuestion(string question, OpenAiOptions options, ILogger logger, Action<IQuestion>? action = null)
    {
        _logger = logger;
        _originalQuestion = question;
        _client = new OpenAIClient(options.ApiKey);

        _messages.Add(new Message(Role.System, _systemPrompt));
        _messages.Add(new Message(Role.User, question));

        action?.Invoke(this);
    }


    /// <inheritdoc />
    public void RegisterTool(object tool, string methodName, string description)
    {
        _tools.Add(Tool.GetOrCreateTool(tool, methodName, description));
    }

    /// <summary>
    /// Tokens Remaining
    /// </summary>
    public int? RemainingTokens { get; private set; }

    /// <summary>
    /// Tokens Used
    /// </summary>
    public int? TotalTokens { get; private set; }

    /// <inheritdoc />
    public async IAsyncEnumerator<AssistantMessage> GetAsyncEnumerator(CancellationToken ct = default)
    {
        // OpenAI API https://platform.openai.com/docs/api-reference/chat/create
        // Claude: https://docs.anthropic.com/en/api/messages

        // return original messages
        foreach (var m in _messages)
        {
            LogOMessage(m);
            yield return ToAssistantMessage(m);
        }

        // initial prompt
        var chatRequest = new ChatRequest(_messages, _tools, "auto");
        var response = await _client.ChatEndpoint.GetCompletionAsync(chatRequest, ct);

        while (response.FirstChoice.FinishReason != "stop")
        {
            // We don't do multiple choices
            var choice = response.FirstChoice;

            // add first messages back
            LogChoice(choice);
            _messages.Add(choice.Message);
            if (choice.FinishReason == "tool_calls")
            {
                // iterate over all tool calls and invoke them
                // response.FirstChoice.FinishReason == "tool_calls";
                foreach (var toolCall in response.FirstChoice.Message.ToolCalls)
                {
                    // Invokes function to get a generic json result to return for tool call.
                    var functionResult = await toolCall.InvokeFunctionAsync(ct);
                    // If you know the return type and do additional processing you can use generic overload
                    // var functionResult = await toolCall.InvokeFunctionAsync<string>();
                    yield return ToAssistantMessage(toolCall, functionResult);
                    _messages.Add(new Message(toolCall, functionResult));
                    LogToolCall(toolCall, functionResult);
                }
            }
            else
            {
                yield return ToAssistantMessage(response.FirstChoice);
            }

            chatRequest = new ChatRequest(_messages, _tools, "auto");
            response = await _client.ChatEndpoint.GetCompletionAsync(chatRequest, ct);
        }

        yield return ToAssistantMessage(response.FirstChoice);
        LogChoice(response.FirstChoice);

        // Context Windows
        RemainingTokens = response.RemainingTokens;
        TotalTokens = response.Usage.TotalTokens;
    }

    void LogToolCall(Tool toolCall, string result)
    {
        _logger.LogDebug("{Role}: {Tool}({Args}) {FunctionResult}", Role.Tool, toolCall.Function.Name,
            toolCall.Function.Arguments, result);
    }

    void LogChoice(Choice choice)
    {
        var fr = choice.FinishReason;
        var message = choice.Message;
        var toolCallCount = (message.ToolCalls ?? []).Count;
        object o = message.Content;
        _logger.LogDebug("{Role}: {Content} f:{A} (tools: {Tools}) .. {Fr}", message.Role, o, choice.FinishReason,
            toolCallCount, fr);
    }

    void LogOMessage(Message message)
    {
        var toolCallCount = (message.ToolCalls ?? []).Count;
        object o = message.Content;
        _logger.LogDebug("{Role}: {Content} (tools: {Tools})", message.Role, o, toolCallCount);
    }

    AssistantMessage ToAssistantMessage(Message message)
    {
        var role = message.Role switch
        {
            Role.Assistant => MessageRole.Assistant,
            Role.Tool => MessageRole.Tool,
            Role.User => MessageRole.User,
            Role.System => MessageRole.System,
            _ => MessageRole.System
        };

        return new AssistantMessage(role, $"{message.Content}");
    }

    AssistantMessage ToAssistantMessage(Choice choice)
    {
        var role = choice.Message.Role switch
        {
            Role.Assistant => MessageRole.Assistant,
            Role.Tool => MessageRole.Tool,
            Role.User => MessageRole.User,
            Role.System => MessageRole.System,
            _ => MessageRole.System
        };

        return new AssistantMessage(role, $"{choice.Message.Content}");
    }

    AssistantMessage ToAssistantMessage(Tool toolCall, string result)
    {
        return new AssistantMessage(MessageRole.Tool,
            $"{toolCall.Function.Name}({toolCall.Function.Arguments})={result}");
    }
}
