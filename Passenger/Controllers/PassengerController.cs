using DomainAPI.Dto.Passenger;
using DomainAPI.Models.Passenger;
using DomainAPI.Services.Passenger;
using DomainAPI.Utils.Passenger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Passenger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengerController : ControllerBase {

        private readonly PassengerServices _passengerService;
        private readonly PassengerServices _removeService;
        private readonly PassengerServices _restrictService;


        public PassengerController(PassengerServices passengerService, PassengerServices removeService,
                                   PassengerServices restrictService) {

            _passengerService = passengerService;
            _removeService = removeService;
            _restrictService = restrictService;


        }

        [HttpGet("Records", Name = "GetAll")]
        public ActionResult<List<Passengers>> Get() => _passengerService.Get();

        [HttpGet("Registration/Cpf", Name = "GetPassenger")]
        public async Task<ActionResult<Passengers>> GetPassenger(string cpf) {


            if (PassengerUtil.ValidateCpf(cpf) == true) {

                var passenger = _passengerService.GetPassenger(PassengerUtil.MaskCPF(cpf));
                if (passenger == null) {
                    return BadRequest("Cadastro de Passageiro Não Encontrado!"); ;
                } else {
                    return Ok(passenger);
                }
            } else {
                return BadRequest("CPF Informado Não é valido!");
            }
        }

        [HttpPost("Register", Name = "PassengerRegister")]
        public async Task<ActionResult<Passengers>> CreateDTOAsync(PassengerDTO passengerDTO) {


            if (PassengerUtil.ValidateCpf(passengerDTO.Cpf) == true) {

                var passengerIn = _passengerService.GetPassenger(passengerDTO.Cpf);


                if (passengerIn == null) {

                    var complement = passengerDTO.Address.Complement;
                    var number = passengerDTO.Address.Number;
                    passengerDTO.Address = new AddressServices().MainAsync(passengerDTO.Address.ZipCode).Result;
                    passengerDTO.Address.Complement = complement;
                    passengerDTO.Address.Number = number;
                    _passengerService.CreateDTO(passengerDTO);

                } else {

                    return BadRequest("CPF Já está Cadastrado!");
                }



                return CreatedAtRoute("GetPassenger", new { cpf = passengerDTO.Cpf.ToString() }, passengerDTO);
            } else {
                return BadRequest("CPF Informado Não é valido!");
            }
        }

        [HttpPut("Alter/Registration", Name = "AlterPassenger")]
        public async Task<ActionResult> UpdateAsync(string cpf, PassengerUpdateDTO passengerIn) {

            if (PassengerUtil.ValidateCpf(cpf)) {
                var passenger = _passengerService.GetPassenger(PassengerUtil.MaskCPF(cpf));
               
                if (passenger == null) {
                    return BadRequest("Cadastro de Passageiro Não Encontrado!");
                } else {
                    passenger.Address = new AddressServices().MainAsync(passengerIn.Address.ZipCode).Result;
                    _passengerService.Update(PassengerUtil.MaskCPF(cpf), passenger,passengerIn);
                    return NoContent();
                }

            } else {
                return BadRequest("CPF Informado Não é valido!");
            }
        }


        [HttpDelete("Remove/cpf", Name = "RemovePassenger")]
        public async Task<ActionResult> DeleteAsync(string cpf) {

            if (PassengerUtil.ValidateCpf(cpf)) {
                var passenger = _passengerService.GetPassenger(PassengerUtil.MaskCPF(cpf));

                if (passenger == null) {
                    return BadRequest("Cadastro de Passageiro Não Encontrado!");
                } else {

                    _removeService.Create(passenger);
                    _passengerService.Remove(passenger);
                    return NoContent();
                }

            } else {
                return BadRequest("CPF Informado Não é valido!");
            }
        }

        [HttpGet("Restrict/cpf", Name = "GetPassengerRestrict")]
        public async Task<ActionResult> RestrictPassengerAsync(string cpf) {

            if (PassengerUtil.ValidateCpf(cpf)) {
                var passenger = _passengerService.GetPassenger(PassengerUtil.MaskCPF(cpf));
                if (passenger == null) {
                    return BadRequest("Cadastro de Passageiro Não Encontrado!"); ;
                } else {

                   passenger.Status = false;
                  _passengerService.UpdateRestrict(PassengerUtil.MaskCPF(cpf), passenger);
                  _restrictService.CreateRestrict(passenger);
                }
                return NoContent();
            } else {
                return BadRequest("CPF Informado Não é valido!");
            }
        }

        [HttpDelete("Remove/Restrict/cpf", Name = "RemoveRestrictPassenger")]
        public async Task<ActionResult> RemoveRestrictAsync(string cpf) {

            if (PassengerUtil.ValidateCpf(cpf)) {
                var passenger = _removeService.GetPassenger(PassengerUtil.MaskCPF(cpf));

                if (passenger == null) {
                    return BadRequest("Cadastro de Passageiro Não Encontrado!"); ;
                } else {
                    if (passenger.Status == false) {
                        passenger.Status = true;
                        _passengerService.UpdateRestrict(PassengerUtil.MaskCPF(cpf), passenger);
                        _restrictService.RemoveRestrict(passenger);

                    }
                }
                return NoContent();
            } else {
                return BadRequest("CPF Informado Não é valido!");
            }
        }

        [HttpGet("StatusValids/Cpf", Name = "GetValidsPassenger")]
        public async Task<ActionResult<Passengers>> ValidPassengers(string cpf) {
            if (PassengerUtil.ValidateCpf(cpf) == true) {
                var passenger = _passengerService.GetValids(PassengerUtil.MaskCPF(cpf));
                if (passenger == null) {
                    return BadRequest("Cadastro de Passageiro Não Encontrado!"); 
                } else {
                    return Ok(passenger);
                }
            } else {
                return BadRequest("CPF Informado Não é valido!");
            }
        }

    }
}
