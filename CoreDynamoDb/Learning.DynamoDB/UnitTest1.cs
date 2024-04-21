namespace Learning.DynamoDB;

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;

public class Tests
{
    const string TableName = "testing";
    AmazonDynamoDBClient _dc;


    [SetUp]
    public void Setup()
    {
        var creds = new BasicAWSCredentials("a", "a");
        _dc = new AmazonDynamoDBClient(creds, new AmazonDynamoDBConfig
        {

            ServiceURL = "http://localhost:8000"
        });
    }

    [TearDown]
    public void TearDown()
    {
        _dc.Dispose();
    }



    [Test]
    public async Task CreateTable()
    {
        // await _dc.DeleteTableAsync(_tableName);

        var request = new CreateTableRequest
        {
            TableName = TableName,
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
        await _dc.CreateTableAsync(request);
    }

    [Test]
    public async Task Test1()
    {
        var pk = new AttributeValue("PACKAGE#gateway");
        var sk = new AttributeValue("META#gateway");

        var packageName = new AttributeValue("gateway");

        var attr = new Dictionary<string, AttributeValue>
        {
            { "PK", pk },
            { "SK", sk },
            { "name", packageName },
        };
        var req = new PutItemRequest(TableName, attr);
        await _dc.PutItemAsync(req);
    }

    [Test]
    [TestCase("stable")]
    [TestCase("nightly")]
    public async Task AddChannel(string channel)
    {
        var pk = new AttributeValue("PACKAGE#gateway");
        var sk = new AttributeValue($"CHANNEL#{channel}");

        var channelName = new AttributeValue(channel);
        var current = new AttributeValue("1.0.0");

        var attr = new Dictionary<string, AttributeValue>
        {
            { "PK", pk },
            { "SK", sk },
            { "name", channelName },
            { "current", current }
        };
        var req = new PutItemRequest(TableName, attr);
        await _dc.PutItemAsync(req);
    }

    [Test]
    [TestCase("1.0.0")]
    [TestCase("1.0.1")]
    [TestCase("1.0.2")]
    [TestCase("1.1.0")]
    public async Task AddRelease(string release)
    {
        var pk = new AttributeValue("PACKAGE#gateway");
        var sk = new AttributeValue($"RELEASE#{release}");

        var releaseName = new AttributeValue(release);

        var attr = new Dictionary<string, AttributeValue>
        {
            { "PK", pk },
            { "SK", sk },
            { "name", releaseName },
        };
        var req = new PutItemRequest(TableName, attr);
        await _dc.PutItemAsync(req);
    }

    [Test]
    [TestCase("prod", "", null, null)]
    [TestCase("prod", "us", null, "stable")]
    [TestCase("prod", "us/gateway", "stable", null)]
    public async Task Tracking(string geo, string path, string? gatewayChannel, string? vectorChannel)
    {
        var pk = new AttributeValue($"TRACKING#{geo}");
        var sk = new AttributeValue($"PATH#{path}");

        var attr = new Dictionary<string, AttributeValue>
        {
            { "PK", pk },
            { "SK", sk },
        };

        if (gatewayChannel != null)
        {
            attr.Add("gateway", new AttributeValue(gatewayChannel));
        }

        if (vectorChannel != null)
        {
            attr.Add("vector", new AttributeValue(vectorChannel));
        }

        var req = new PutItemRequest(TableName, attr);
        await _dc.PutItemAsync(req);
    }

    [Test]
    public async Task TrackQuery()
    {
        var resp = await _dc.QueryAsync(new QueryRequest
        {
            TableName = TableName,
            KeyConditions = new Dictionary<string, Condition>
            {
                {
                    "PK", new Condition
                    {
                        ComparisonOperator = ComparisonOperator.EQ,
                        AttributeValueList = new List<AttributeValue>
                        {
                            new("TRACKING#prod")
                        }
                    }
                }

            }
        });

        Assert.That(resp.Count, Is.EqualTo(3));

        var a1 = resp.Items.Select(FlipIt).ToList();

        var b1 = a1[0];
        var b2 = a1[0];
        var b3 = a1[0];
    }

    Dictionary<string, string> FlipIt(Dictionary<string, AttributeValue> a2)
    {
        return a2.Select((k) => (k.Key, k.Value.S))
            // PK doesn't really matter
            .Where( o => o.Key != "PK")
            .ToDictionary(ShapeKey, ShapeValue);
    }
    string ShapeKey((string, string) key)
    {
        return key.Item1 switch
        {
            "SK" => "path",
            _ => key.Item1
        };
    }

    string ShapeValue((string, string) key)
    {
        return key.Item1 switch
        {
            "SK" => key.Item2[5..],
                _ => key.Item2
        };
    }

    [Test]
    public async Task Abc()
    {
        var dbc = new DynamoDBContext(_dc);


    }
}

// public class Document
// {
//     public string PK { get; set; } = "";
//     public string SK { get; set; } = "";
//     public Dictionary<string, string> Values { get; set; } = new();
// }
//
// public class TrackingDocument
// {
//     // PK
//     // SK
//     // ... other values
//
//
//     public static TrackingDocument FromDocument(string pk, string sk, Dictionary<string, string> values)
//     {
//
//     }
//
//     public static object ToDocument()
//     {
//
//     }
// }
