using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainAPI.Dto.Flight
{
    public class FlightsDto
    {
        [Required(ErrorMessage = "A IATA é obrigatória!", AllowEmptyStrings = false)]
        public string Iata { get; set; }

        [Required(ErrorMessage = "O Número de inscrição da Aeronave é obrigatório!", AllowEmptyStrings = false)]
        public string Rab { get; set; }

        [Required(ErrorMessage = "A Data de partida do voo é obrigatória!", AllowEmptyStrings = false)]
        public DateTime Departure { get; set; }
    }
}
