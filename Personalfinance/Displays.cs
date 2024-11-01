using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;
using System.Transactions;

namespace Personalfinance;

public class Displays
{
    Functions functions = new Functions();
    List<Bankaccount> bankAccounts = new List<Bankaccount>();
    static TransactionManager transactionManager = new TransactionManager();
    public void DisplayMenu()
    {
        var readFromFile = functions.LoadTransactionsFromFile();

        if (readFromFile != null)
        {
            foreach (Bankaccount bankAccount in readFromFile)
            {
                bankAccounts.Add(bankAccount);
            }
        }

        Console.WriteLine("-------------------");
        Console.WriteLine("Personal Finance");
        Console.WriteLine("-------------------");
        Console.WriteLine("1. Vart vill du spara transaktionerna?");
        Console.WriteLine("2. Lägg till transaktion");
        Console.WriteLine("3. Visa kontosaldo");
        Console.WriteLine("4. Visa transaktion över tid");
        Console.WriteLine("5. Radera en transaktion");
        Console.WriteLine("6. Spara och avsluta");
        Console.WriteLine("-------------------");
    }
    public void DisplayChangeFilePaths()
    {
        functions.ChangeFilePaths();
    }
    public void DisplayAddTrancations()
    {
        string transaction;
        decimal amount;
        string inputAmount;

        Console.WriteLine("Välj typ av transaktion:");
        Console.WriteLine("1. Inkomst");
        Console.WriteLine("2. Utgift");
        int choice = int.Parse(Console.ReadLine());

        if (choice == 1)
        {
            Console.WriteLine("Namn på inkomst:");
            transaction = Console.ReadLine();

            Console.WriteLine("Skriv in summan:");
            inputAmount = Console.ReadLine();

            bool checkAmount = functions.SetAmount(inputAmount);

            if (checkAmount)
            {
                amount = decimal.Parse(inputAmount);
                Bankaccount newIncome = functions.GetBankAccount(transaction, amount);
                bankAccounts.Add(newIncome);
                var savedIncome = functions.SaveToFile(newIncome);

                Console.WriteLine($"{savedIncome} har lagts till och sparats!");
            }
            else
            {
                Console.WriteLine("Ogiltigt belopp, försök igen.");
            }
        }
        else if (choice == 2)
        {
            Console.WriteLine("Namn på utgift:");
            transaction = Console.ReadLine();
            Console.WriteLine("Skriv in summan:");
            inputAmount = Console.ReadLine();

            bool checkAmount = functions.SetAmount(inputAmount);

            if (checkAmount)
            {
                amount = decimal.Parse(inputAmount) * -1;
                Bankaccount newExpense = functions.GetBankAccount(transaction, amount);
                bankAccounts.Add(newExpense);
                var savedExpense = functions.SaveToFile(newExpense);

                Console.WriteLine($"{savedExpense} har lagts till och sparats!");
            }
        }
        else
        {
            Console.WriteLine("Ogiltigt val.");

        }

    }
    public void DisplayShowBalance()
    {
        if (bankAccounts.Count == 0)
        {
            Console.WriteLine("Inga transaktioner har lagts till ännu.");
        }
        else
        {
            decimal balance = 0;
            foreach (Bankaccount t in bankAccounts)
            {
                Console.WriteLine($"{t.BankType}: {t.Name} - Summa: {t.Amount} - Datum: {t.Date}");
                balance += t.Amount;
            }

            Console.WriteLine("--------------------");
            Console.WriteLine($"Nuvarande kontobalans: {balance:C}");
            Console.WriteLine("--------------------");

        }
    }
    public void DisplayReports()
    {

        bool running = true;
        while (running)
        {
            Console.WriteLine("\n--- Välj: ---");
            Console.WriteLine("1. Visa transaktioner dagsvis");
            Console.WriteLine("2. Visa transaktioner veckovis");
            Console.WriteLine("3. Visa transaktioner månadsvis");
            Console.WriteLine("4. Visa transaktioner årsvis");
            Console.WriteLine("5. Avsluta");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        ShowDailyTransactions();
                        break;
                    case 2:
                        ShowWeeklyTransactions();
                        break;
                    case 3:
                        ShowMonthlyTransactions();
                        break;
                    case 4:
                        ShowYearlyTransactions();
                        break;
                    case 5:
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val, försök igen.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Ogiltigt val, försök igen.");
            }
        }
    }

    public void ShowDailyTransactions()
    {
        Console.Write("Ange år (yyyy): ");
        if (int.TryParse(Console.ReadLine(), out int year) && year > 0)
        {
            Console.Write("Ange månad (1-12): ");
            if (int.TryParse(Console.ReadLine(), out int month) && month >= 1 && month <= 12)
            {
                Console.Write("Ange dag (1-31): ");
                if (int.TryParse(Console.ReadLine(), out int day) && day >= 1 && day <= 31)
                {
                    try
                    {
                        DateOnly date = new DateOnly(year, month, day);

                        List<Bankaccount> dailyTransactions = bankAccounts
                            .Where(t => t.Date.Day == date.Day)
                            .ToList();

                        Console.WriteLine($"\nTransaktioner för {date:yyyy-MM-dd}:");
                        PrintTransactions(dailyTransactions);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        Console.WriteLine("Ogiltigt datum. Kontrollera att dagen är giltig för den angivna månaden.");
                    }
                }
                else
                {
                    Console.WriteLine("Ogiltig dag. Vänligen ange ett giltigt värde mellan 1 och 31.");
                }
            }
            else
            {
                Console.WriteLine("Ogiltig månad. Vänligen ange ett giltigt värde mellan 1 och 12.");
            }
        }
        else
        {
            Console.WriteLine("Ogiltigt år. Vänligen ange ett giltigt år.");
        }
    }


    public void ShowWeeklyTransactions()
    {
        Console.Write("Ange år (yyyy): ");
        if (int.TryParse(Console.ReadLine(), out int year) && year > 0)
        {
            Console.Write("Ange vecka (1-53): ");
            if (int.TryParse(Console.ReadLine(), out int weekNumber) && weekNumber >= 1 && weekNumber <= 53)
            {
                DateOnly jan1 = new DateOnly(year, 1, 1);
                DateOnly startOfWeek = jan1.AddDays((weekNumber - 1) * 7 - (int)jan1.DayOfWeek + (int)DayOfWeek.Monday);
                DateOnly endOfWeek = startOfWeek.AddDays(7);

                List<Bankaccount> weeklyTransactions = bankAccounts
                    .Where(t => t.Date >= startOfWeek && t.Date < endOfWeek)
                    .ToList();

                if (weeklyTransactions.Count > 0)
                {
                    Console.WriteLine($"\nTransaktioner för vecka {weekNumber} av {year}:");
                    PrintTransactions(weeklyTransactions);
                }
                else
                {
                    Console.WriteLine($"Inga transaktioner hittades för vecka {weekNumber} av {year}.");
                }
            }
            else
            {
                Console.WriteLine("Ogiltig vecka. Vänligen ange ett värde mellan 1 och 53.");
            }
        }
        else
        {
            Console.WriteLine("Ogiltigt år. Vänligen ange ett giltigt år.");
        }
    }


    public void ShowMonthlyTransactions()
    {
        Console.Write("Ange år (yyyy): ");
        if (int.TryParse(Console.ReadLine(), out int year) && year > 0)
        {
            Console.Write("Ange månad (1-12): ");
            if (int.TryParse(Console.ReadLine(), out int month) && month >= 1 && month <= 12)
            {
                List<Bankaccount> monthlyTransactions = bankAccounts
                    .Where(t => t.Date.Year == year && t.Date.Month == month)
                    .ToList();

                if (monthlyTransactions.Count > 0)
                {
                    Console.WriteLine($"\nTransaktioner för {year}-{month:D2}:");
                    PrintTransactions(monthlyTransactions);
                }
                else
                {
                    Console.WriteLine($"Inga transaktioner hittades för {year}-{month:D2}.");
                }
            }
            else
            {
                Console.WriteLine("Ogiltig månad. Vänligen ange ett värde mellan 1 och 12.");
            }
        }
        else
        {
            Console.WriteLine("Ogiltigt år. Vänligen ange ett giltigt år.");
        }
    }


    public void ShowYearlyTransactions()
    {
        Console.Write("Välj år (yyyy): ");
        if (DateTime.TryParseExact(Console.ReadLine(), "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
        {
            List<Bankaccount> yearlyTransactions = bankAccounts.Where(t => t.Date.Year == date.Year).ToList();

            Console.WriteLine($"\nTransaktioner för år {date.Year}:");
            PrintTransactions(yearlyTransactions);
        }
        else
        {
            Console.WriteLine("Ogiltigt datum.");
        }
    }

    public void PrintTransactions(List<Bankaccount> bankAccounts)
    {
        if (bankAccounts.Count == 0)
        {
            Console.WriteLine("Inga transaktioner hittades.");
        }
        else
        {
            foreach (Bankaccount transaction in bankAccounts)
            {
                Console.WriteLine($"{transaction.Date:yyyy-MM-dd}: {transaction.Name} - {transaction.Amount:C} {transaction.BankType}");
            }
        }
    }
    public void DisplayRemoveTransaction()
    {
        Console.WriteLine("Ange namnet på transaktionen du vill ta bort:");
        string name = Console.ReadLine();

        Console.WriteLine("Ange datumet för transaktionen (format: YYYY-MM-DD):");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
        {
            var transactionToRemove = bankAccounts
                .FirstOrDefault(t => t.Name == name && t.Date == DateOnly.FromDateTime(date));

            if (transactionToRemove != null)
            {
                bankAccounts.Remove(transactionToRemove);

                transactionManager.UpdateFile(bankAccounts);

                Console.WriteLine("Transaktionen har tagits bort från både listan och filen.");
            }
            else
            {
                Console.WriteLine("Transaktionen hittades inte.");
            }
        }
        else
        {
            Console.WriteLine("Ogiltigt datumformat.");
        }
    }



}

