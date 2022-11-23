using DomainAPI.Models.Passenger;
using DomainAPI.Utils.Passenger;
using System;
using System.ComponentModel.DataAnnotations;

namespace DomainAPI.Dto.Passenger
{
    public class PassengerDTO
    {

        [Required(ErrorMessage = "CPF Precisa de 11 Digitos...")]
        [StringLength(14)]
        public string Cpf { get; set; }
        [Required(ErrorMessage = "Nome é Campo Obrigatorio!")]
        [StringLength(30)]
        public string Name { get; set; }
        [StringLength(1)]
        public string Gender { get; set; }
        [StringLength(14)]
        public string Phone { get; set; }
        public DateTime DtBirth { get; set; }
        public DateTime DtRegister { get; set; }
        public bool Status { get; set; }
        public PassengerAddress Address { get; set; }

        public PassengerDTO(string cpf, string phone, bool status, string gender)
        {
            Cpf = PassengerUtil.MaskCPF(cpf);
            Phone = PassengerUtil.MaskPhone(phone);
            Gender = gender.ToUpper();
            Status = status;
        }
    }
}
