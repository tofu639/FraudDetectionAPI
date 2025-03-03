using System;

namespace FraudDetectionAPI.Models;

public class Transaction
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public double Amount { get; set; }
    public DateTime Timestamp { get; set; }
    public string Location { get; set; } = string.Empty;
}
