using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankAppWebApi.Models
{
    public class TransactionRequestTemplate
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public decimal Amount { get; set; }
    }
}