namespace Id.Host.Identity.Queries;

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

public class MembershipsByUser
{
    readonly User _user;

    public MembershipsByUser(User user)
    {
        _user = user;
    }

    public async Task<List<Membership>> Execute(DynamoContext cxt)
    {
        var query = new QueryRequest(cxt.TableName);
        query.KeyConditions = new Dictionary<string, Condition>
        {
            {
                "PK", new Condition
                {
                    ComparisonOperator = ComparisonOperator.EQ,
                    AttributeValueList = new List<AttributeValue>
                    {
                        new ($"USER#{_user.ProviderType}#{_user.ProviderId}")
                    }
                }
            }
        };

        var results = await cxt.Query(query);

        var items = results.Items;

        var result = new List<Membership>();
        var user = new User();

        foreach (var item in results.Items)
        {
            if (item["SK"].S.StartsWith("USER#"))
            {
                user.ProviderType = "";
                user.ProviderId = "";
                // password
                // email
                // etc
                continue;
            }

            var membership = new Membership();
            membership.User = user;
            membership.Role = item["Role"].S;
            result.Add(membership);
        }

        return result;
    }
}
