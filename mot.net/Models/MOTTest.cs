using System;

namespace MOT.NET.Models {
    public class MOTTest {
        public DateTime CompletedDate { get; set; }
        public string TestResult { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int OdometerValue { get; set; }
        public int MOTTestNumber { get; set; }
        public string OdometerResultType { get; set; }
    }
}