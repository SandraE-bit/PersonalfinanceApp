using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personalfinance
{
    public class Bankaccount
    {
        public string? Name { get; set; }
        public decimal Amount { get; set; }
        public DateOnly Date { get; set; }
        public string? BankType { get; set; }
    }
}
