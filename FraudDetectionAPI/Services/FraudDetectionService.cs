using System;
using System.Collections.Generic;
using System.Linq;
using FraudDetectionAPI.Models;

namespace FraudDetectionAPI.Services;

public class FraudDetectionService
{
    public List<Transaction> DetectFraud(List<Transaction> transactions)
    {
        return transactions.Where(t => t.Amount > 5000 || IsSuspicious(t, transactions)).ToList();
    }

    private bool IsSuspicious(Transaction transaction, List<Transaction> transactions)
    {
        var userTransactions = transactions
            .Where(t => t.UserId == transaction.UserId)
            .OrderByDescending(t => t.Timestamp)
            .ToList();

        for (int i = 0; i < userTransactions.Count - 1; i++)
        {
            var timeDiff = (userTransactions[i].Timestamp - userTransactions[i + 1].Timestamp).TotalMinutes;
            if (userTransactions[i].Location != userTransactions[i + 1].Location && timeDiff < 30)
            {
                return true;
            }
        }
        return false;
    }
}
