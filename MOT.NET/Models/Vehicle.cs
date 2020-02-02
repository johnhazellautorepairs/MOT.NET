using System;
using System.Collections.Generic;
using MOT.NET.Json;
using Newtonsoft.Json;

namespace MOT.NET.Models {
    public class Vehicle {
        /// <summary>
        /// Gets the registration of the current Vehicle.
        /// </summary>
        /// <value>The registration of the current Vehicle.</value>
        public string Registration { get; set; }

        /// <summary>
        /// Gets the make of the current Vehicle.
        /// </summary>
        /// <value>The make of the current Vehicle.</value>
        public string Make { get; set; }

        /// <summary>
        /// Gets the model of the current Vehicle.
        /// </summary>
        /// <value>The model of the current Vehicle.</value>
        public string Model { get; set; }

        /// <summary>
        /// Gets the date on which the current Vehicle was first used.
        /// </summary>
        /// <value>The date on which the current Vehicle was first used.</value>
        public DateTime FirstUsedDate { get; set; }

        /// <summary>
        /// Gets the fuel type of the current Vehicle.
        /// </summary>
        /// <remarks>May be Petrol, Diesel, Electric or LPG.</remarks>
        /// <value>The fuel type of the current Vehicle.</value>
        public string FuelType { get; set; }

        /// <summary>
        /// Gets the primary colour of the current Vehicle.
        /// </summary>
        /// <value>The primary colour of the current Vehicle.</value>
        public string PrimaryColour { get; set; }
        [JsonConverter(typeof(Base64GuidJsonConverter))]

        /// <summary>
        /// Gets the unique identifier of the current Vehicle.
        /// </summary>
        /// <value>The unique identifier of the current Vehicle.</value>
        public Guid VehicleId { get; set; }

        /// <summary>
        /// Gets the date on which the current Vehicle was registered.
        /// </summary>
        /// <value>The date on which the current Vehicle was registered.</value>
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// Gets the date on which the current Vehicle was manufactured.
        /// </summary>
        /// <value>The date on which the current Vehicle was manufactured.</value>
        public DateTime ManufactureDate { get; set; }

        /// <summary>
        /// Gets the size of the current Vehicle's engine, in cubic centimeters.
        /// </summary>
        /// <value>The size of the current Vehicle's engine, in cubic centimeters.</value>
        [JsonConverter(typeof(StringIntJsonConverter))]
        public int EngineSize { get; set; }

        /// <summary>
        /// Gets a List of the MOT tests previously performed on the current Vehicle.
        /// </summary>
        /// <value>A List of the MOT tests previously performed on the current Vehicle.</value>
        public List<Test> MOTTests { get; set; }
    }
}