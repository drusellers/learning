namespace Id.Host.Identity;

using Amazon.DynamoDBv2.Model;

public class Membership
{
    public User User { get; set; }
    public string Account { get; set; }
    public string Role { get; set; }
}

public class MembershipMapping
{
    public Dictionary<string, AttributeValue> Convert(Membership membership)
    {
        var pk = $"USER#{membership.User.ProviderType}#{membership.User.ProviderId}";
        var sk = $"MEMBERSHIP#{membership.Account}";

        var attrs = new Dictionary<string, AttributeValue>
        {
            { "PK", new(pk) },
            { "SK", new(sk) },
            { "Role", new(membership.Role) }
        };



        return attrs;
    }
}
