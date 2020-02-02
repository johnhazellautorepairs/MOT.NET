using System;
using System.Collections.Generic;
using MOT.NET.Json;
using Newtonsoft.Json;

namespace MOT.NET.Models {
    public class Vehicle {
        public string Registration { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public DateTime FirstUsedDate { get; set; }
        public string FuelType { get; set; }
        public string PrimaryColour { get; set; }
        [JsonConverter(typeof(Base64GuidJsonConverter))]
        public Guid VehicleId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime ManufactureDate { get; set; }
        [JsonConverter(typeof(StringIntJsonConverter))]
        public int EngineSize { get; set; }
        public List<Test> MOTTests { get; set; }
    }
}