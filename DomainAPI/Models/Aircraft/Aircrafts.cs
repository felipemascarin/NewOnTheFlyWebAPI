using DomainAPI.Models.Company;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace DomainAPI.Models.Aircraft
{
    public class Aircrafts
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        [Required]
        [StringLength(6)]
        public string RAB { get; set; }
        [Required]
        public int Capacity { get; set; }
        public DateTime DtRegistry { get; set; }
        public DateTime? DtLastFlight { get; set; }
        [Required]
        public Companys Company { get; set; }
    }
}
