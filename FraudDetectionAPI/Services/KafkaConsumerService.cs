using System;
using System.Threading;
using Confluent.Kafka;

namespace FraudDetectionAPI.Services;

public class KafkaConsumerService
{
    private readonly string _bootstrapServers = "localhost:9092";
    private readonly string _topic = "fraud-transactions";
    private readonly string _groupId = "fraud-detector-group";

    public void Consume()
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _bootstrapServers,
            GroupId = _groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe(_topic);

        while (true)
        {
            var consumeResult = consumer.Consume(CancellationToken.None);
            Console.WriteLine($"Consumed: {consumeResult.Message.Value}");
        }
    }
}
