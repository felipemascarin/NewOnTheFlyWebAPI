using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainAPI.Dto.Company
{
    public class CompanyDtoTwo // Classe Dto para pegar apenas os dados necessários para criação de uma company
    {
        public string CNPJ { get; set; }
        [Required]
        [StringLength(30)]
        [JsonProperty("name")]
        public string Name { get; set; }
        [StringLength(30)]
        [JsonProperty("nameOpt")]
        public string NameOpt { get; set; }
        [JsonProperty("dtOpen")]
        public DateTime DtOpen { get; set; }
        [JsonProperty("address")]
        public AddressDtoTwo Address { get; set; }
    }
}
