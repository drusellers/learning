namespace Id.Host.Identity;

using Amazon.DynamoDBv2.Model;

public class User
{
    public string ProviderType { get; set; }
    public string ProviderId { get; set; }

    public string? AvatarUrl { get; set; }
    public string? EmailAddress { get; set; }

    public byte[]? Password { get; set; }
    public byte[]? PasswordIv { get; set; }
}

public class UserMapping
{
    public Dictionary<string, AttributeValue> Convert(User user)
    {
        var pk = $"USER#{user.ProviderType}#{user.ProviderId}";
        var sk = $"USER#META";

        var attrs = new Dictionary<string, AttributeValue>
        {
            { "PK", new(pk) },
            { "SK", new(sk) },
        };

        if (user.AvatarUrl != null)
        {
            attrs.Add("AvatarUrl", new(user.AvatarUrl));
        }

        if (user.EmailAddress != null)
        {
            attrs.Add("EmailAddress", new(user.EmailAddress));
        }

        if (user.Password != null)
        {
            attrs.Add("Password", new(""));
        }

        if (user.PasswordIv != null)
        {
            attrs.Add("PasswordIv", new (""));
        }

        return attrs;
    }
}
