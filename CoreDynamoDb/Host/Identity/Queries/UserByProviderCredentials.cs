namespace Id.Host.Identity.Queries;

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

public class UserByProviderCredentials
{
    readonly string _providerType;
    readonly string _providerId;

    public UserByProviderCredentials(string providerType, string providerId)
    {
        _providerType = providerType;
        _providerId = providerId;
    }

    public async Task Execute(DynamoContext cxt)
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
                        new ($"USER#{_providerType}#{_providerId}")
                    }
                }
            },
            {
                "SK", new Condition
                {
                    ComparisonOperator = ComparisonOperator.EQ,
                    AttributeValueList = new List<AttributeValue>
                    {
                        new ($"USER#META")
                    }
                }
            }
        };

        var result = await cxt.Query(query);
    }
}
