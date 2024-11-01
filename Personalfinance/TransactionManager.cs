namespace Personalfinance
{
    public class TransactionManager
    {
        public string SaveToFilePath { get; set; } = @"c:\Skolan\C#\Individuell uppgift\SaveToFile\SavedTransactions.csv";
        public void UpdateFile(List<Bankaccount> bankAccounts)
        {
            var lines = bankAccounts
                .Select(t => $"{t.Name},{t.Amount},{t.Date},{t.BankType}")
                .ToArray();

            File.WriteAllLines(SaveToFilePath, lines);
        }
    }
}
