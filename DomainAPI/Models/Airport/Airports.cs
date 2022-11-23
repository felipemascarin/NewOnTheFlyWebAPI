using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DomainAPI.Models.Airport
{
    public class Airports
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "A IATA é obrigatória", AllowEmptyStrings = false)]
        [StringLength(3)]
        public string IATA { get; set; }

        [Required(ErrorMessage = "O nome da cidade é obrigatório", AllowEmptyStrings = false)]
        public string City { get; set; }

        [Required(ErrorMessage = "O nome do Estado é obrigatório", AllowEmptyStrings = false)]
        public string State { get; set; }

        [Required(ErrorMessage = "O nome do País é obrigatório", AllowEmptyStrings = false)]
        [StringLength(2)]
        public string Country { get; set; }
    }
}
