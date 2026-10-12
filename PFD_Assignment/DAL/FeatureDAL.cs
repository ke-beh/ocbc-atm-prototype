using System;
using System.Data.SqlClient;
using PFD_Assignment.Models;
using System.Collections.Generic;

namespace PFD_Assignment.DAL
{
    public class FeatureDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public FeatureDAL()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString("OCBCATMConnectionString");
            conn = new SqlConnection(strConn);
        }

        public void UpdateFeatureUsage(string featureName, int customerId)
        {
            string query = @"
                UPDATE dbo.FeatureUsage 
                SET UsageCount = UsageCount + 1, LastUsed = @LastUsed 
                WHERE FeatureName = @FeatureName AND CustomerID = @CustomerID;

                IF @@ROWCOUNT = 0
                BEGIN
                    INSERT INTO dbo.FeatureUsage (FeatureName, UsageCount, CustomerID, LastUsed)
                    VALUES (@FeatureName, 1, @CustomerID, @LastUsed);
                END
            ";

            using (SqlConnection connection = new SqlConnection(conn.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@FeatureName", featureName);
                command.Parameters.AddWithValue("@CustomerID", customerId);
                command.Parameters.AddWithValue("@LastUsed", DateTime.Now);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public FeatureUsage GetMostRecentFeatureUsageByCustomerId(int customerId)
        {
            FeatureUsage featureUsage = null;
            string query = @"
                SELECT TOP 1 * FROM dbo.FeatureUsage
                WHERE CustomerID = @CustomerID
                ORDER BY LastUsed DESC
            ";

            using (SqlConnection connection = new SqlConnection(conn.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CustomerID", customerId);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        featureUsage = new FeatureUsage
                        {
                            FeatureUsageID = int.Parse(reader["FeatureUsageID"].ToString()),
                            FeatureName = reader["FeatureName"].ToString(),
                            UsageCount = int.Parse(reader["UsageCount"].ToString()),
                            CustomerID = customerId,
                            LastUsed = DateTime.Parse(reader["LastUsed"].ToString())
                        };
                    }
                }
            }

            return featureUsage;
        }

        public FeatureUsage GetFavoriteFeatureByCustomerId(int customerId)
        {
            FeatureUsage favoriteFeature = null;
            string query = @"
        SELECT TOP 1 FeatureName, MAX(UsageCount) as MaxUsage
        FROM dbo.FeatureUsage
        WHERE CustomerID = @CustomerID
        GROUP BY FeatureName
        ORDER BY MaxUsage DESC
    ";

            using (SqlConnection connection = new SqlConnection(conn.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CustomerID", customerId);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        favoriteFeature = new FeatureUsage
                        {
                            FeatureName = reader["FeatureName"].ToString(),
                            UsageCount = Convert.ToInt32(reader["MaxUsage"])
                            // Note: You can't retrieve LastUsed or FeatureUsageID without including them in the GROUP BY,
                            // which would change the logic of what you're retrieving.
                        };
                    }
                }
            }

            return favoriteFeature;
        }

    }
}
