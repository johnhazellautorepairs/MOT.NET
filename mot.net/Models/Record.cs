using System;
using System.Collections.Generic;

namespace MOT.NET.Models {
    public class Record {
        public string Registration { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public DateTime FirstUsedDate { get; set; }
        public string FuelType { get; set; }
        public string PrimaryColour { get; set; }
        public string VehicleId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime ManufactureDate { get; set; }
        public int EngineSize { get; set; }
        public List<MOTTest> MOTTests { get; set; }
        public List<ReasonForRejectionAndComments> RfRAndComments { get; set; }
    }
}