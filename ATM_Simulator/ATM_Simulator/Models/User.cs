using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_Simulator.Models
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public CardDetails CardDetails { get; set; }
        public string PinCode { get; set; }
        public decimal BalanceGEL { get; set; }
        public decimal BalanceUSD { get; set; }
        public decimal BalanceEUR { get; set; }
        public List<Transaction> TransactionHistory { get; set; }
    }
}
