using DomainAPI.Models.Flight;
using DomainAPI.Models.Passenger;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DomainAPI.Models.Sale {
    public class Sales {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        public Flights Flight { get; set; }
        public List<Passengers> Passengers { get; set; }

        public bool Reserved { get; set; }
        public bool Sold { get; set; }
    }
}
