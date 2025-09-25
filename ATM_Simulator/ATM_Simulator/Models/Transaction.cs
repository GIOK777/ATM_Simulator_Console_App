using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_Simulator.Models
{
    public class Transaction
    {
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public decimal AmountGEL { get; set; }
        public decimal AmountUSD { get; set; }
        public decimal AmountEUR { get; set; }
    }
}
