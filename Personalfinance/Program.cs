using System.Diagnostics;
using System.Globalization;
using System.Transactions;

namespace Personalfinance;


public class Program
{
    static Displays displays = new Displays();
    
    static void Main()
    {
        bool running = true;
        while (running)
        {
            displays.DisplayMenu();

            Console.Write("Välj ett alternativ: ");
            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Ogiltigt val, försök igen.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    AddSalary();
                    break;
                case 2:
                    ShowBalance();
                    break;
                case 3:
                    SumTransactions();
                    break;
                case 4:
                    RemoveTransaction();
                    break;
                case 5:
                    running = false;
                    break;
                default:
                    Console.WriteLine("Ogiltigt val, försök igen.");
                    break;
            }
        }
    }
    static void AddSalary()
    {
        displays.DisplayAddTrancations();
    }
    static void ShowBalance()
    {
        displays.DisplayShowBalance();
    }
    static void SumTransactions()
    {
        displays.DisplayReports();
    }
    static void RemoveTransaction()
    {
        displays.DisplayRemoveTrancations();
    }
}

