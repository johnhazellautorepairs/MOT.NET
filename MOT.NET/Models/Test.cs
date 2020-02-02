using System;
using System.Collections.Generic;
using MOT.NET.Json;
using Newtonsoft.Json;

namespace MOT.NET.Models {
    public class Test {
        public DateTime CompletedDate { get; set; }
        public string TestResult { get; set; }
        public DateTime ExpiryDate { get; set; }
        [JsonConverter(typeof(StringIntJsonConverter))]
        public int OdometerValue { get; set; }
        [JsonConverter(typeof(StringLongJsonConverter))]
        public long MOTTestNumber { get; set; }
        public string OdometerResultType { get; set; }
        public List<ReasonForRejectionAndComments> RfRAndComments { get; set; }
    }
}