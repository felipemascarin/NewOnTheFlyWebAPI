using DomainAPI.Models.Aircraft;
using DomainAPI.Models.Airport;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace DomainAPI.Models.Flight
{
    public class Flights
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        public Airports Destiny { get; set; }

        [Required]
        public Aircrafts Plane { get; set; }

        [Required]
        public int Sales { get; set; }

        [Required]
        public DateTime Departure { get; set; }

        [Required]
        public bool Status { get; set; }
    }
}
