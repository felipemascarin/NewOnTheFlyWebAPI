using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace DomainAPI.Dto.Company
{
    public class AddressDtoTwo // Classe necessária para criação de um endereço
    {
        [Required]
        [StringLength(9)]
        [JsonProperty("zipCode")]
        public string ZipCode { get; set; }
        public int Number { get; set; }
        [StringLength(10)]
        public string? Complement { get; set; }
    }
}
