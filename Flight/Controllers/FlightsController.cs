using DomainAPI.Dto.Flight;
using DomainAPI.Models.Flight;
using DomainAPI.Services.Flight;
using DomainAPI.Utils.FlightUtils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flight.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly FlightsServices _flightsServices;

        public FlightsController(FlightsServices flightsServices)
        {
            _flightsServices = flightsServices;
        }


        [HttpPost("CreateFlight/")]
        public async Task<ActionResult<Flights>> CreateFlightAsync(FlightsDto flightDto)
        {
            var destiny = await _flightsServices.GetAirportAPIAsync(flightDto.Iata);

            if (destiny == null) return NotFound("Aeroporto não encontrado!");

            if (destiny.Country == null) return BadRequest("Não foi possível carregar informação do País de destino!");

            if (destiny.Country.ToUpper() != "BR") return BadRequest("Só é possível voo Nacional!");

            var plane = await _flightsServices.GetAircraftAPIAsync(flightDto.Rab);

            if (plane == null) return NotFound("Aeronave não encontrada!");

            if (FlightUtils.DepartureValidator(flightDto.Departure) == false) return BadRequest("Não é possível cadastrar voo com data passada!");

            if (plane.Company.Status == false) return BadRequest("Não pode ser cadastrado voos para essa companhia!");

            if (FlightUtils.DateOpenCompanyValidator(plane.Company.DtOpen) == false) return BadRequest("Companhia com data de criação menor que 6 meses!");

            var flightday = await _flightsServices.GetOneAsync(flightDto.Departure, plane.RAB);

            if (flightday != null) return BadRequest("Aeronave já possui voo nesse dia!");

            if (await _flightsServices.PutDateAircraftAPIAsync(flightDto.Rab) == false) return BadRequest("Não foi possível alterar atributo Data Último Voo da aeronave!");

            var flight = new Flights()
            {
                Departure = flightDto.Departure,
                Destiny = destiny,
                Plane = plane,
                Sales = 0,
                Status = true
            };

            await _flightsServices.CreateFlightAsync(flight);

            return CreatedAtRoute("GetFlight", new { id = flight.Id }, flight);
        }


        [HttpGet]
        public async Task<ActionResult<List<Flights>>> GetAllAsync() => await _flightsServices.GetAllAsync();

        //Filtra pela data do voo e retorna uma lista de voos ativos
        [HttpGet("Date/{date}")]
        public async Task<ActionResult<List<Flights>>> GetByDateAsync(DateTime date) => await _flightsServices.GetByDateAsync(date);

        //Filtra por intervalo de data e retorna uma lista de voos ativos
        [HttpGet("ByDateRange/{initialdate}/{finaldate}")]
        public async Task<ActionResult<List<Flights>>> GetByDateRangeAsync(DateTime initialdate, DateTime finaldate) => await _flightsServices.GetByDateRangeAsync(initialdate, finaldate);

        [HttpGet("{id:length(24)}", Name = "GetFlight")]
        public async Task<ActionResult<Flights>> GetOneAsync(string id)
        {
            var cliente = await _flightsServices.GetOneAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        //Filtra um voo pela data e aeronave
        [HttpGet("GetOneFlight/{date}/{rab:length(6)}")]
        public async Task<ActionResult<Flights>> GetOneAsync(DateTime date, string rab)
        {
            var flight = await _flightsServices.GetOneAsync(date, rab);

            if (flight == null || flight.Status == false)
            {
                return NotFound();
            }

            return flight;
        }


        [HttpPut("cancelflight/{id:length(24)}")]
        public async Task<ActionResult<Flights>> UpdateCancelFlightAsync(string id)
        {
            var flight = await _flightsServices.GetOneAsync(id);

            if (flight == null || flight.Status == false)
            {
                return NotFound();
            }

            flight.Status = false;

            await _flightsServices.UpdateCancelFlightAsync(id, flight);

            return CreatedAtRoute("GetFlight", new { id = flight.Id }, flight);
        }


        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult<Flights>> UpdateAsync(string id, Flights flightIn)
        {
            var flight = await _flightsServices.GetOneAsync(id);

            if (flight == null || flight.Status == false)
            {
                return NotFound();
            }

            await _flightsServices.UpdateAsync(id, flightIn);

            return CreatedAtRoute("GetFlight", new { id = flight.Id }, flightIn);
        }
    }
}
