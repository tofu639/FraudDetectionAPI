using FraudDetectionAPI.Models;
using FraudDetectionAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<FraudDetectionService>();
builder.Services.AddSingleton<KafkaProducerService>();
builder.Services.AddSingleton<KafkaConsumerService>();
var app = builder.Build();

app.MapPost("/api/transactions/process", async ([FromBody] Transaction transaction, FraudDetectionService fraudService, KafkaProducerService kafkaService) =>
{
    var suspiciousTransactions = fraudService.DetectFraud(new List<Transaction> { transaction });

    if (suspiciousTransactions.Count > 0)
    {
        await kafkaService.ProduceAsync(transaction);
        return Results.Ok(new { message = "Transaction processed", isFraud = true });
    }

    return Results.Ok(new { message = "Transaction processed", isFraud = false });
});

app.MapGet("/", () => "Fraud Detection API is running on .NET 8 🚀");

var kafkaConsumer = app.Services.GetRequiredService<KafkaConsumerService>();
_ = Task.Run(() => kafkaConsumer.Consume()); 

app.Run();
