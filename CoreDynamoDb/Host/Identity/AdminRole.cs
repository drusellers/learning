namespace Id.Host.Identity;

using Amazon.DynamoDBv2.Model;

public class AdminRole
{
    public User User { get; set; }
    public string Role { get; set; }
}

public class AdminRoleConverter
{
    public Dictionary<string, AttributeValue> Convert(AdminRole role)
    {
        var pk = $"ADMIN#{role.User.ProviderType}#{role.User.ProviderId}";
        var sk = $"ADMIN";

        var attrs = new Dictionary<string, AttributeValue>
        {
            { "PK", new(pk) },
            { "SK", new(sk) },
            { "Role", new(role.Role) }
        };



        return attrs;
    }
}
