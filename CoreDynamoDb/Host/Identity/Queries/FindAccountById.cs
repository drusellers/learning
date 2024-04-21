namespace Id.Host.Identity.Queries;

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

public class FindAccountById
{
    readonly string _accountId;

    public FindAccountById(string accountId)
    {
        _accountId = accountId;
    }

    public async Task<List<User>> Execute(DynamoContext cxt)
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
                        new ($"ACCOUNT#{_accountId}")
                    }
                }
            }
        };

        var results = await cxt.Query(query);

        return new List<User>();
    }
}
