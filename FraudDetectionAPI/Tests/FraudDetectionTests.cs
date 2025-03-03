using System;
using System.Collections.Generic;
using Xunit;
using FraudDetectionAPI.Models;
using FraudDetectionAPI.Services;

namespace FraudDetectionAPI.Tests;

public class FraudDetectionTests
{
    private readonly FraudDetectionService _fraudDetectionService = new();

    [Fact]
    public void DetectFraud_ShouldIdentifyLargeTransactions()
    {
        var transactions = new List<Transaction>
        {
            new Transaction { Id = 1, UserId = 101, Amount = 10000, Timestamp = DateTime.UtcNow, Location = "Bangkok" }
        };

        var result = _fraudDetectionService.DetectFraud(transactions);
        Assert.Single(result);
        Assert.Equal(10000, result[0].Amount);
    }
}
