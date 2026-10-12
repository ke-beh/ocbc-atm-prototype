using System.Data.SqlClient;
using PFD_Assignment.Models;

namespace PFD_Assignment.DAL
{
    public class LoginDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public LoginDAL()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
            "OCBCATMConnectionString");
            conn = new SqlConnection(strConn);
        }


        // Used for testing
        public void InsertOrUpdateFaceImage(string accountNum, byte[] faceEncoding)
        {
            string query = "UPDATE dbo.Account SET FaceImage = @FaceImage WHERE AccountNum = @AccountNum";

            using (SqlConnection connection = new SqlConnection(conn.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@AccountNum", accountNum);
                command.Parameters.AddWithValue("@FaceImage", faceEncoding);

                connection.Open();
                int result = command.ExecuteNonQuery();

                // If no rows were updated, it means the account number doesn't exist, so you might want to handle that case.
            }
        }

        // Used for testing
        public byte[] GetFaceImage(string accountNum)
        {
            byte[] faceImage = null;
            string query = "SELECT FaceImage FROM dbo.Account WHERE AccountNum = @AccountNum";

            using (SqlConnection connection = new SqlConnection(conn.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@AccountNum", accountNum);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        faceImage = (byte[])reader["FaceImage"];
                    }
                }
            }

            return faceImage;
        }

        public IEnumerable<(string AccountNum, byte[] FaceImage)> GetAllFaceImages()
        {
            var facesList = new List<(string AccountNum, byte[] FaceImage)>();
            string query = "SELECT AccountNum, FaceImage FROM dbo.Account";

            using (SqlConnection connection = new SqlConnection(conn.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var accountNum = reader["AccountNum"].ToString();
                        var faceImage = (byte[])reader["FaceImage"];
                        facesList.Add((accountNum, faceImage));
                    }
                }
            }

            return facesList;
        }

        public Account GetAccountByNumber(string accountNum)
        {
            Account account = null;
            string query = "SELECT * FROM dbo.Account WHERE AccountNum = @AccountNum";

            using (SqlConnection connection = new SqlConnection(conn.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@AccountNum", accountNum);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        account = new Account
                        {
                            AccountNum = reader["AccountNum"].ToString(),
                            CardNumber = reader["CardNumber"].ToString(),
                            // Handle other fields like PIN, DateOpened, etc.
                            Amount = decimal.Parse(reader["Amount"].ToString()),
                            AccountStatus = reader["AccountStatus"].ToString(),
                            AccountTypeID = int.Parse(reader["AccountTypeID"].ToString()),
                            // Exclude FaceImage and Biometrics if not needed
                        };
                    }
                }
            }

            return account;
        }

        public Customer GetCustomerByAccountNumber(string accountNum)
        {
            Customer customer = null;
            string query = "SELECT * FROM dbo.Customer WHERE AccountNum = @AccountNum";

            using (SqlConnection connection = new SqlConnection(conn.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@AccountNum", accountNum);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        customer = new Customer
                        {
                            CustomerID = int.Parse(reader["CustomerID"].ToString()),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Address = reader["Address"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            EmailAddress = reader["EmailAddress"].ToString(),
                            AccountNum = reader["AccountNum"].ToString()
                        };
                    }
                }
            }

            return customer;
        }
        public string GetAccountNumByPIN(string enteredPIN)
        {
            string accountNumber = null;
            string query = "SELECT AccountNum FROM dbo.Account WHERE PIN = HASHBYTES('SHA2_256', @EnteredPIN)";

            using (SqlConnection connection = new SqlConnection(conn.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@EnteredPIN", enteredPIN);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        accountNumber = reader["AccountNum"].ToString();
                    }
                }
            }

            return accountNumber;
        }



    }
}
