namespace Dexter;

using Dexter.FromAlt.Providers.OpenAi;
using OpenAI;
using static Dexter.Thing;

public class MessageAid
{
    public static async Task Main(OpenAiOptions config)
    {
        var openAi = new OpenAIClient(config.ApiKey);


        List<string> actions = [
            "EditMessage",
            "ResubmitToQueue",
            "DeleteMessage",
            "PurgeQueue"
        ];

        var systemContext = "You are an intelligent operations system that is here to help resolve troubleshooting issues.";

        List<Resolved> resolved =
        [
            new Resolved(new Message("abaoeuc", "SystemTimeoutException"), ["ResubmitToQueue"]),
            new Resolved(new Message("a,.bc", "SystemTimeoutException"), ["ResubmitToQueue"]),
            new Resolved(new Message("achbc", "NullReferenceException"), ["EditMessage","ResubmitToQueue"])
        ];

        var newMessage = new Message("abc", "NullReferenceException");

        var first = $$"""
                      I'm going to give you a list of messages that have had errors and resolutions. 
                      Each JSON message will have an error property that identifies the error in the message
                      and a resolutions property that has the actions taken to fix the issue.

                      The total available list of actions that can be taken to resolve the error
                      are: {{string.Join(", ", actions)}}.

                      After that I'm going to give you a new message, and its error. 

                      Out of the available list of actions can you give me the steps to take 
                      to resolve the error.

                      Please explain your thought process step-by-step and then return your answer in a JSON format:

                      ```json
                      {
                          "steps": [ { "type": "DeleteMessage" } ]
                      }
                      ```


                      Previous Resolutions
                      1. '{{ToJson(resolved[0])}}'
                      2. '{{ToJson(resolved[1])}}'

                      Today I have
                      Here is message C as '{{ToJson(newMessage)}}', what should I do?
                      """;

// Console.WriteLine(first);
    }
}
