namespace Dexter.FromAlt.Providers.Claudia;

using System.Diagnostics;
using System.Text;
using Anthropic.SDK;
using Anthropic.SDK.Common;
using Anthropic.SDK.Constants;
using Anthropic.SDK.Messaging;
using Microsoft.Extensions.Logging;
using Tool = Anthropic.SDK.Common.Tool;

/// <inheritdoc />
[DebuggerDisplay("{_originalQuestion}")]
public class ClaudiaQuestion : IQuestion
{
    readonly AnthropicClient _client;
    readonly ILogger _logger;
    readonly List<Message> _messages = new();
    readonly string _originalQuestion;
    readonly string _systemPrompt;
    readonly List<Tool> _tools = new();


    /// <summary>
    /// ctor
    /// </summary>
    public ClaudiaQuestion(AnthropicOptions options, ILogger logger, string originalQuestion, Action<IQuestion>? action = null)
    {
        _logger = logger;
        _originalQuestion = originalQuestion;
        _client = new AnthropicClient
        {
            Auth = new APIAuthentication(options.ApiKey),
            // supports tool use
            AnthropicBetaVersion = "tools-2024-04-04"
        };

        _systemPrompt = """
                        You are an assistant that helps a company's accounting department search and manage their expenses related to SaaS applications.

                        Please respond using markdown
                        """;
        _messages.Add(new Message(RoleType.User, _originalQuestion));

        action?.Invoke(this);
    }

    /// <inheritdoc />
    public void RegisterTool(object tool, string methodName, string description)
    {
        _tools.Add(Tool.GetOrCreateTool(tool, methodName, description));
    }

    /// <inheritdoc />
    public async IAsyncEnumerator<AssistantMessage> GetAsyncEnumerator(CancellationToken ct = default)
    {
        LogMessage(new Message(RoleType.Assistant, _systemPrompt));
        yield return PromptMessage(_systemPrompt);

        // return original messages
        foreach (var m in _messages)
        {
            LogMessage(m);
            yield return ToAssistantMessage(m);
        }

        var messageRequest = new MessageParameters
        {
            Model = AnthropicModels.Claude35Sonnet,
            MaxTokens = 1024,
            Messages = _messages,
            System = [new(_systemPrompt)],
            Tools = _tools,
            Stream = false,
            Temperature = 1.0m,
        };

        var response = await _client.Messages.GetClaudeMessageAsync(messageRequest, ct);

        while (response.StopReason != "end_turn")
        {
            _messages.Add(response.Message);
            if (response.StopReason == "tool_use")
            {
                foreach (var toolCall in response.ToolCalls)
                {
                    var toolResponse = await toolCall.InvokeAsync(ct);
                    _messages.Add(new Message(toolCall, toolResponse));
                }
            }

            messageRequest = new MessageParameters
            {
                Model = AnthropicModels.Claude35Sonnet,
                MaxTokens = 1024,
                Messages = _messages,
                System = [new(_systemPrompt)],
                Tools = _tools,
                Stream = false
            };
            response = await _client.Messages.GetClaudeMessageAsync(messageRequest, ct);
        }

        yield return ToAssistantMessage(response.Message);
        LogMessage(response.Message);
    }

    void LogMessage(Message message)
    {
        var toolCallCount = 0;
        object o = message.Content;
        _logger.LogDebug("{Role}: {Content} (tools: {Tools})", message.Role, o, toolCallCount);
    }

    AssistantMessage PromptMessage(string prompt)
    {
        var role = MessageRole.System;
        return new AssistantMessage(role, prompt);
    }

    AssistantMessage ToAssistantMessage(Message message)
    {
        var role = message.Role switch
        {
            RoleType.User => MessageRole.User,
            RoleType.Assistant => MessageRole.Assistant,
            _ => MessageRole.System
        };

        var sb = new StringBuilder();
        foreach (var x in message.Content)
        {
            sb.AppendLine(x.ToString());
        }

        return new AssistantMessage(role, sb.ToString());
    }


    /// <summary>
    /// simple examples
    /// </summary>
    [Function("This function returns the current user's name.")]
    public static Task<string> GetCurrentUser(
        [FunctionParameter("not needed", false)]
        string guess)
    {
        return Task.FromResult("Dru Sellers");
    }
}
