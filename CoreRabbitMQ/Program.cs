using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var cf = new ConnectionFactory();
cf.UserName = "guest";
cf.Password = "guest";
cf.Port = 5672;
using var conn = cf.CreateConnection("localhost");
using var model = conn.CreateModel();

// declare the queue
model.QueueDeclare(queue: "hello",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);

var consumer = new EventingBasicConsumer(model);
consumer.Received += (m, ea) =>
{
    var headers = ea.BasicProperties.Headers;

    // var abc = headers["Content-Type"];
    // if (abc.ToString() != "application/json")
    // {
    //     return;
    // }
    //
    // var targetType = headers[""];
    var body = ea.Body.ToArray();

    // deserialize
    // pipeline of my own

    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received {message}");
};

model.BasicConsume(queue: "hello",
    autoAck: true,
    consumer: consumer);

const string message = "Hello World!";
var body = Encoding.UTF8.GetBytes(message);

model.BasicPublish(exchange: string.Empty,
    routingKey: "hello",
    basicProperties: null,
    body: body);

Console.WriteLine("...");
Console.ReadLine();
