using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainAPI.Models.Company
{
    public class CompanyAddress
    {
        [Required]
        [StringLength(9)]
        [JsonProperty("cep")]
        public string ZipCode { get; set; }
        [StringLength(100)]
        [JsonProperty("logradouro")]
        public string Street { get; set; }
        public int Number { get; set; }
        [StringLength(10)]
        public string? Complement { get; set; }

        [StringLength(30)]
        [JsonProperty("localidade")]
        public string City { get; set; }
        [StringLength(2)]
        [JsonProperty("uf")]
        public string State { get; set; }
    }
}
