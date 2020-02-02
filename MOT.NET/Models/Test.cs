using System;
using System.Collections.Generic;
using MOT.NET.Json;
using Newtonsoft.Json;

namespace MOT.NET.Models {
    /// <summary>
    /// Represents an MOT test record.
    /// </summary>
    public class Test {
        /// <summary>
        /// Gets the date on which the current Test was completed.
        /// </summary>
        /// <value>The date on which the current Test was completed.</value>
        public DateTime CompletedDate { get; set; }

        /// <summary>
        /// Gets the result of the current Test.
        /// </summary>
        /// <value>The result of the current Test.</value>
        public string TestResult { get; set; }

        /// <summary>
        /// Gets the expiry date of the current Test.
        /// </summary>
        /// <value>The expiry date of the current Test.</value>
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// Gets the odometer reading of the vehicle during the current test.
        /// </summary>
        /// <value>The odometer reading of the vehicle during the current test.</value>
        [JsonConverter(typeof(StringIntJsonConverter))]
        public int OdometerValue { get; set; }

        /// <summary>
        /// Gets the units in which the odometer reading is measured in.
        /// </summary>
        /// <value>The units in which the odometer reading is measured in.</value>
        public string OdometerUnit { get; set; }

        /// <summary>
        /// Gets the test number of the current Test.
        /// </summary>
        /// <value>The test number of the current Test.</value>
        [JsonConverter(typeof(StringLongJsonConverter))]
        public long MOTTestNumber { get; set; }

        /// <summary>
        /// Gets the type (method of reading) of the odometer reading.
        /// </summary>
        /// <value>The type of the odometer reading.</value>
        public string OdometerResultType { get; set; }

        /// <summary>
        /// Gets the list of reason for rejection / test comments.
        /// </summary>
        /// <value>The list of reason for rejection / test comments.</value>
        public List<ReasonForRejectionAndComments> RfRAndComments { get; set; }
    }
}