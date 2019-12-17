using System;
using System.Collections.Generic;
using System.Text;

namespace BCTestDemo
{

    public class PendingTransaction
    {
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string OwnerName { get; set; }
        public int Amount { get; set; }
        public string transid { get; set; }
        public object transactions { get; set; }
    }

    public class Chain
    {
        public int Index { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        public List<Transaction> Transactions { get; set; }
        public int Nonce { get; set; }
    }

    public class RootObject
    {
        public List<Transaction> PendingTransactions { get; set; }
        public int CurrentPropTaxOwed { get; set; }
        public List<Chain> Chain { get; set; }
        public int Difficulty { get; set; }
    }

    public class Transaction
    {
        private string v1;
        private string v2;
        private string v3;
        private string v4;
        private string v5;
        private int v6;
        private string v7;
        private string v8;

        public string clip { get; set; }
        public string streetAddress { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public int propertyValue { get; set; }
        public string lender { get; set; }
        public string customer { get; set; }


        public Transaction(string fromAddress, string toAddress, int amount, string owerName)
        {
          //
        }

        public Transaction()
        {
        }

        public Transaction(string v1, string clip, string v2, string streetAddress, string v3, string city, string v4, string state, string v5, string zip, int v6, int propertyValue, string v7, string lender, string v8, string customer)
        {
            this.v1 = v1;
            this.clip = clip;
            this.v2 = v2;
            this.streetAddress = streetAddress;
            this.v3 = v3;
            this.city = city;
            this.v4 = v4;
            this.state = state;
            this.v5 = v5;
            this.zip = zip;
            this.v6 = v6;
            this.propertyValue = propertyValue;
            this.v7 = v7;
            this.lender = lender;
            this.v8 = v8;
            this.customer = customer;
        }

        public Transaction(string clip, string streetAddress, string city, string state, string zip, int propertyValue, string lender, string customer)
        {
            this.clip = clip;
            this.streetAddress = streetAddress;
            this.city = city;
            this.state = state;
            this.zip = zip;
            this.propertyValue = propertyValue;
            this.lender = lender;
            this.customer = customer;
        }
    }
}
