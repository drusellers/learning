namespace Id.Host.Identity.Queries;

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

public class AllUsers
{
    public async Task<List<User>> Execute(DynamoContext cxt)
    {
        var query = new QueryRequest(cxt.TableName);
        query.KeyConditions = new Dictionary<string, Condition>
        {
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

        var results = await cxt.Query(query);

        return new List<User>();
    }
}
