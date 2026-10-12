using System.Data.SqlClient;
using PFD_Assignment.Models;

namespace PFD_Assignment.DAL
{
    public class TransactionDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        private SqlConnection conn2;

        public TransactionDAL()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
            "OCBCATMConnectionString");
            string strConn2 = Configuration.GetConnectionString(
            "FeedbackConnectionString");
            conn2 = new SqlConnection(strConn2);
            conn = new SqlConnection(strConn);
        }
        public string GetAccountNumberByPhoneNumber(string phoneNumber)
        {
            string accountNumber = null;

            string query = "SELECT AccountNum FROM dbo.Customer WHERE Phone = @PhoneNumber";
            using (SqlConnection connection = new SqlConnection(conn.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
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

        public bool UpdateAccountBalance(string accountNumber, decimal newBalance)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this.conn.ConnectionString))
                {
                    conn.Open();
                    string sql = "UPDATE dbo.Account SET Amount = @NewBalance WHERE AccountNum = @AccountNumber";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@NewBalance", newBalance);
                    cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return false;
            }
        }

        public bool TransferFunds(string fromAccountNumber, string toAccountNumber, decimal amount)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this.conn.ConnectionString))
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        // Deduct from sender's account
                        string updateSenderSql = "UPDATE dbo.Account SET Amount = Amount - @Amount WHERE AccountNum = @FromAccountNumber";
                        SqlCommand cmd1 = new SqlCommand(updateSenderSql, conn, transaction);
                        cmd1.Parameters.AddWithValue("@Amount", amount);
                        cmd1.Parameters.AddWithValue("@FromAccountNumber", fromAccountNumber);
                        cmd1.ExecuteNonQuery();

                        // Add to recipient's account
                        string updateRecipientSql = "UPDATE dbo.Account SET Amount = Amount + @Amount WHERE AccountNum = @ToAccountNumber";
                        SqlCommand cmd2 = new SqlCommand(updateRecipientSql, conn, transaction);
                        cmd2.Parameters.AddWithValue("@Amount", amount);
                        cmd2.Parameters.AddWithValue("@ToAccountNumber", toAccountNumber);
                        cmd2.ExecuteNonQuery();

                        // Commit the transaction
                        transaction.Commit();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception here
                Console.WriteLine("Error during fund transfer: " + ex.Message); // Replace with proper logging
                return false;
            }
        }

        public int writeTransaction(string accountNumber, decimal balance, decimal amount, string description)
        {
            try
            {
                int transactionID = -1;
                using (SqlConnection conn2 = new SqlConnection(this.conn2.ConnectionString))
                {
                    conn2.Open();
                    string sql = "INSERT INTO dbo.[Transaction] (Description, Amount, Balance, Location, TransactionDate, TransactionStatus, AccountNum, TransactionTypeID) VALUES (@Description, @Amount, @Balance, '153 Jalan Besiah Rd #09-111', '2022-03-14', 'Successful', @AccountNumber, 2)";
                    SqlCommand cmd = new SqlCommand(sql, conn2);
                    cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                    cmd.Parameters.AddWithValue("@Balance", balance);
                    cmd.Parameters.AddWithValue("@Amount", amount);
                    cmd.Parameters.AddWithValue("@Description", description);


                    cmd.ExecuteNonQuery();

                    string sql2 = "SELECT TOP 1 * FROM dbo.[Transaction] ORDER BY TransactionID DESC ";
                    SqlCommand cmd2 = new SqlCommand(sql2, conn2);

                    SqlDataReader reader = cmd2.ExecuteReader();
                    while (reader.Read())
                    {
                        transactionID = reader.GetInt32(0);
                    }


                    reader.Close();

                    conn2.Close();


                    return transactionID;
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return -1;
            }
        }



    }
}
