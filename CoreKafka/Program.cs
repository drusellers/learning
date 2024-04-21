using Confluent.Kafka;

var config = new ProducerConfig
{
    BootstrapServers = "localhost:19092",
};

using (var producer = new ProducerBuilder<Null, string>(config).Build())
{
    var msg = new Message<Null, string>();
    msg.Value = "HI";
    producer.Produce("test", msg);

    producer.Flush();
}
