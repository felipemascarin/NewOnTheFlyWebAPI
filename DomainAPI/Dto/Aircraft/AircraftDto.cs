using System.ComponentModel.DataAnnotations;

namespace DomainAPI.Dto.Aircraft
{
    public class AircraftDto // Classe para obter dados para criação da aeronave
    {
        [Required]
        public string RAB { get; set; }
        [Required]
        public int Capacity { get; set; }
    }
}
