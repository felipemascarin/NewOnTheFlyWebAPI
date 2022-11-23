using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainAPI.Dto.Company
{
    public class AddressDto // Classe Dto para transformar o endereço da companhia em um objeto
    {
        [Required]
        [StringLength(9)]
        [JsonProperty("zipCode")]

        public string ZipCode { get; set; }
        [StringLength(100)]
        [JsonProperty("street")]
        public string Street { get; set; }
        public int Number { get; set; }
        [StringLength(10)]
        public string? Complement { get; set; }

        [StringLength(30)]
        [JsonProperty("city")]
        public string City { get; set; }
        [StringLength(2)]
        [JsonProperty("state")]
        public string State { get; set; }
    }
}
