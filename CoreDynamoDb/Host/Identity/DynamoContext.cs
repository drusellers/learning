namespace Id.Host.Identity;

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Microsoft.Extensions.Options;

public class DynamoOptions
{
    public static readonly string ConfigKey = "Dynamo";

    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public string TableName { get; set; }
}

public interface IDynamoContext
{
    Task PutUser(User user);
    Task PutAccount(Account account);
}

public class DynamoContext : IDynamoContext
{
    readonly AmazonDynamoDBClient _client;
    readonly IOptions<DynamoOptions> _options;

    public DynamoContext(IOptions<DynamoOptions> options)
    {
        var credentials = new BasicAWSCredentials(options.Value.AccessKey, options.Value.SecretKey);
        _client = new AmazonDynamoDBClient(credentials, new AmazonDynamoDBConfig()
        {
            ServiceURL = "http://localhost:8000"
        });
        _options = options;
    }

    public string TableName => _options.Value.TableName;

    public Task<QueryResponse> Query(QueryRequest request)
    {
        return _client.QueryAsync(request);
    }
    public async Task ResetTable()
    {
        try
        {
            await _client.DeleteTableAsync(_options.Value.TableName);
        }
        catch
        {
            // ignore
        }

        var request = new CreateTableRequest
        {
            TableName = _options.Value.TableName,
            BillingMode = BillingMode.PAY_PER_REQUEST,
            KeySchema = new List<KeySchemaElement>
            {
                new("PK", KeyType.HASH),
                new("SK", KeyType.RANGE)
            },
            AttributeDefinitions = new List<AttributeDefinition>
            {
                new("PK", ScalarAttributeType.S),
                new("SK", ScalarAttributeType.S)
            }
        };
        await _client.CreateTableAsync(request);
    }

    public async Task PutUser(User user)
    {
        var attrs = new UserMapping().Convert(user);
        var req = new PutItemRequest(_options.Value.TableName, attrs)
        {

        };
        await _client.PutItemAsync(req);
    }

    public async Task PutAccount(Account account)
    {
        var attrs = new AccountMapping().Convert(account);
        var req = new PutItemRequest(_options.Value.TableName, attrs);
        await _client.PutItemAsync(req);
    }

    public async Task PutMembership(Membership membership)
    {
        var attrs = new MembershipMapping().Convert(membership);
        var req = new PutItemRequest(_options.Value.TableName, attrs);
        await _client.PutItemAsync(req);
    }

    public async Task PutAdmin(AdminRole role)
    {
        var attrs = new AdminRoleConverter().Convert(role);
        var req = new PutItemRequest(_options.Value.TableName, attrs);
        await _client.PutItemAsync(req);
    }
}
