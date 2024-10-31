using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Personalfinance;

public class Functions
{
    private static string saveToFilePath = @"c:\Skolan\C#\Individuell uppgift\SaveToFile\SavedTransactions.csv";
    public Bankaccount GetBankAccount(string name, decimal amount)
    {
        Bankaccount bankAccount = new()
        {
            Name = name,
            Amount = amount,
            Date = RandomDate(),
            BankType = "",
        };
        return bankAccount;
    }
    private DateOnly RandomDate()
    {
        DateOnly startDate = new DateOnly(2024, 1, 1);
        DateOnly endDate = new DateOnly(2024, 11, 10);

        Random random = new Random();
        int range = endDate.DayNumber - startDate.DayNumber;
        int randomDays = random.Next(0, range + 1);
        DateOnly randomDate = startDate.AddDays(randomDays);

        return randomDate;
    }
    public bool SetAmount(string input)
    {
        if (!decimal.TryParse(input, out decimal amount) || amount <= 0)
        {
            Console.WriteLine("Ogiltigt belopp. Ange ett positivt tal.");
            return false;
        }
        return true;
    }
    public string SaveToFile(Bankaccount bankAccount)
    {
        string transactionToFile = $"{bankAccount.Name},{bankAccount.Amount},{bankAccount.Date},{bankAccount.BankType}";

        File.AppendAllLines(saveToFilePath, new[] { transactionToFile });

        return transactionToFile;
    }

    public List<Bankaccount> LoadTransactionsFromFile()
    {
        var transactions = new List<Bankaccount>();
        if (File.Exists(saveToFilePath))
        {
            var lines = File.ReadAllLines(saveToFilePath);
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length == 4)
                {
                    transactions.Add(new Bankaccount
                    {
                        Name = parts[0],
                        Amount = decimal.Parse(parts[1]),
                        Date = DateOnly.Parse(parts[2]),
                        BankType = parts[3]
                    });
                }
            }
            Console.WriteLine("Transaktion hämtad från filen.");

        }
        else
        {
            Console.WriteLine("Ingen transaktion går att hitta.");
        }

        return transactions;
    }
}
