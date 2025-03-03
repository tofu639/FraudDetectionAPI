using System;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using FraudDetectionAPI.Models;

namespace FraudDetectionAPI.Services;

public class KafkaProducerService
{
    private readonly string _bootstrapServers = "localhost:9092";
    private readonly string _topic = "fraud-transactions";

    public async Task ProduceAsync(Transaction transaction)
    {
        var config = new ProducerConfig { BootstrapServers = _bootstrapServers };
        
        using var producer = new ProducerBuilder<Null, string>(config).Build();
        var message = JsonSerializer.Serialize(transaction);
        await producer.ProduceAsync(_topic, new Message<Null, string> { Value = message });
        Console.WriteLine($"Produced: {message}");
    }
}
