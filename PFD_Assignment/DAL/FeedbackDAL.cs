using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace PFD_Assignment.DAL
{
    public class FeedbackDAL 
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public FeedbackDAL()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
            "FeedbackConnectionString");
            conn = new SqlConnection(strConn);
        }
        public void updatestarfeedback(int feedbackval,int transactionid)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(this.conn.ConnectionString))
                {
                    SqlCommand cmd = conn.CreateCommand();
			        cmd.CommandText = @"UPDATE dbo.[Transaction] SET FeedbackStarRating = @FeedbackStarRating where TransactionID = @TransactionID";
                    cmd.Parameters.AddWithValue("@FeedbackStarRating",feedbackval);
                    cmd.Parameters.AddWithValue("@TransactionID", transactionid);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.ToString());
            }

        }

        public void updatecategoryfeedback(string feedbackval, int transactionid)
        {


            try
            {
                using (SqlConnection conn = new SqlConnection(this.conn.ConnectionString))
                {
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = @"UPDATE dbo.[Transaction] SET FeedbackCategory = @FeedbackCategory where TransactionID = @TransactionID";
                    cmd.Parameters.AddWithValue("@FeedbackCategory", feedbackval);
                    cmd.Parameters.AddWithValue("@TransactionID", transactionid);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.ToString());
            }

        }


    }
}
