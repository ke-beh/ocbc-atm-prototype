namespace PFD_Assignment.Models
{
    public class Account
    {
        public string AccountNum { get; set; }
        public string CardNumber { get; set; }
        public byte[] PIN { get; set; } // Stored as VARBINARY
        public DateTime DateOpened { get; set; }
        public decimal Amount { get; set; } // MONEY type in SQL Server maps to decimal in C#
        public string AccountStatus { get; set; }
        public int AccountTypeID { get; set; }

        // Navigation property for related Customer(s)
        public virtual Customer Customer { get; set; }
    }

}
