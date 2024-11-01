using System.Diagnostics;
using System.Globalization;
using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

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
                    ChangeFilePath();
                    break;
                case 2:
                    AddSalary();
                    break;
                case 3:
                    ShowBalance();
                    break;
                case 4:
                    SumTransactions();
                    break;
                case 5:
                    RemoveTransaction();
                    break;
                case 6:
                    running = false;
                    break;
                default:
                    Console.WriteLine("Ogiltigt val, försök igen.");
                    break;
            }
        }
    }
    static void ChangeFilePath()
    {
        displays.DisplayChangeFilePaths();
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
        displays.DisplayRemoveTransaction();
    }
   
}

