namespace Dexter;

using System.Text.Json;

public static class Thing
{
    public static string ToJson<T>(T obj)
    {
        return JsonSerializer.Serialize(obj, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }
}

public record Message(string Payload, string? Error);

public record Resolved(Message Message, string[] Resolutions);
