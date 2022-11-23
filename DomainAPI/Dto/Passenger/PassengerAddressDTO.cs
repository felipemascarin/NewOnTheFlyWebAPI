using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainAPI.Dto.Passenger {
    public class PassengerAddressDTO {
        [Required]
        [StringLength(9)]
        [JsonProperty("cep")]
        public string ZipCode { get; set; }

        [StringLength(30)]
        public string Complement { get; set; }

        public int Number { get; set; }
    }
}
