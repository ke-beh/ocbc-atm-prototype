namespace PFD_Assignment.Models
{
    public class FeatureUsage
    {
        public int FeatureUsageID { get; set; }
        public string FeatureName { get; set; }
        public int UsageCount { get; set; }
        public int CustomerID { get; set; }
        public DateTime LastUsed { get; set; }
    }

}
