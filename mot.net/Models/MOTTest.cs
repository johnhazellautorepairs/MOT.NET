using System;
using MOT.NET.Json;
using Newtonsoft.Json;

namespace MOT.NET.Models {
    public class MOTTest {
        public DateTime CompletedDate { get; set; }
        public string TestResult { get; set; }
        public DateTime ExpiryDate { get; set; }
        [JsonConverter(typeof(StringIntJsonConverter))]
        public int OdometerValue { get; set; }
        [JsonConverter(typeof(StringIntJsonConverter))]
        public int MOTTestNumber { get; set; }
        public string OdometerResultType { get; set; }
    }
}