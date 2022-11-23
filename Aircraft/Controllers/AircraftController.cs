using DomainAPI.Models.Aircraft;
using DomainAPI.Services.Aircraft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DomainAPI.Dto.Aircraft;

namespace Aircraft.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AircraftController : ControllerBase
    {
        private readonly AircraftServices _aircraftServices;
        private readonly DeadfileAircraftServices _deadfileAircraftServices;

        public AircraftController(AircraftServices aircraftServices, DeadfileAircraftServices deadfileAircraftServices)
        {
            _aircraftServices = aircraftServices;
            _deadfileAircraftServices = deadfileAircraftServices;
        }

        //Endpoint get para obter todas aeronaves cadastradas
        [HttpGet]
        public async Task<ActionResult<List<Aircrafts>>> Get() => await _aircraftServices.Get();

        //Endpoint get para obter uma aeronave especifica pelo RAB
        [HttpGet("{rab}", Name = "GetAircraft")]
        public async Task<ActionResult<Aircrafts>> Get(string rab) => await _aircraftServices.Get(rab);

        //Endpoint de criar Aeronave e vincular com a companhia responsável.
        [HttpPost("{cnpjIn}")]
        public async Task<ActionResult<Aircrafts>> Create(string cnpjIn, AircraftDto aircraftIn)
        {
            var cnpj = cnpjIn.Replace(".", "").Replace("-", "").Replace("/", "").Replace("%2F", "");

            if (!IsCnpj(cnpj)) return BadRequest("CNPJ Digitado não é válido!");

            var company = await _aircraftServices.GetCompany(cnpj);

            if (company is null) return BadRequest("Companhia não possue cadastro!");

            var rab = aircraftIn.RAB.ToUpper();

            var rabValidation = rab.Substring(0, 2);

            if (rabValidation != "PT" && rabValidation != "PP" && rabValidation != "PR" && rabValidation != "PS" && rabValidation != "PU" && rabValidation != "PH")
                return BadRequest("Prefixo da inscrição da Aeronave é inválido!");

            var aircraftVerificated = await _aircraftServices.Get(aircraftIn.RAB);

            if (aircraftVerificated is null)
            {
                var aircraft = new Aircrafts()
                {
                    RAB = rab,
                    Capacity = aircraftIn.Capacity,
                    DtRegistry = DateTime.Now,
                    DtLastFlight = DateTime.Now,
                    Company = company
                };                

                await _aircraftServices.Create(aircraft);

                return Created("GetAircraft", aircraft);
            }

            return BadRequest("Já existe uma Aeronave cadastrada com está inscrição!");
        }

        //Endpoint de atualizar data do ultimo voo
        [HttpPut("UpdateFlight/{rabIn}")]
        public async Task<IActionResult> Put(string rabIn)
        {
            var rab = rabIn.ToUpper();

            var aircraft = await _aircraftServices.Get(rab);

            if (aircraft is null) return BadRequest("Aeronave não possue cadastro!");

            aircraft.DtLastFlight = DateTime.Now;

            await _aircraftServices.Put(rab, aircraft);

            return NoContent();
        }

        //Endpoint de atualizar a capacidade da aeronave
        [HttpPut("UpdateCapacity/{rabIn}/{capacity}")]
        public async Task<IActionResult> PutCapacity(string rabIn, int capacity)
        {
            var rab = rabIn.ToUpper();

            var aircraft = await _aircraftServices.Get(rab);

            if (aircraft is null) return BadRequest("Aeronave não possue cadastro!");

            aircraft.Capacity = capacity;

            await _aircraftServices.Put(rab, aircraft);

            return NoContent();
        }

        [HttpPut("UpdateCompany/")]
        public async Task PutCompany(Aircrafts aircraft)
        {
            await _aircraftServices.Put(aircraft.RAB, aircraft);

            return;
        }

        //Endpoint de remover companhia
        [HttpDelete]
        public async Task<IActionResult> Remove(string rabIn)
        {
            var rab = rabIn.ToUpper();

            var aircraft = await _aircraftServices.Get(rab);

            if (aircraft is null) return NotFound();

            var deadfile = new DeadfileAircrafts();

            deadfile.DeadfilesAircrafts = aircraft;

            await _deadfileAircraftServices.Create(deadfile);

            await _aircraftServices.Remove(rab);

            return NoContent();
        }

        //Método de validação de cnpj
        public static bool IsCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Replace("%2F", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }
    }
}
