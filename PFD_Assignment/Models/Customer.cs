namespace PFD_Assignment.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string EmailAddress { get; set; }
        public string AccountNum { get; set; }

        // Navigation property for the related Account
        public virtual ICollection<Account> Accounts { get; set; }
    }

}
