namespace Id.Host.Identity;

using Amazon.DynamoDBv2.Model;

public class Account
{
    public string AccountId { get; set; }
}

public class AccountMapping
{
    public Dictionary<string, AttributeValue> Convert(Account account)
    {
        var pk = $"ACCOUNT#{account.AccountId}";
        var sk = $"ACCOUNT";

        return new Dictionary<string, AttributeValue>
        {
            { "PK", new(pk) },
            { "SK", new(sk) },
        };
    }
}
