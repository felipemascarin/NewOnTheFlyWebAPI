using DomainAPI.Models.Airport;
using DomainAPI.Services.Airport;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airport.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AirportsController : ControllerBase
    {
        private readonly AirportsServices _airportsServices;

        public AirportsController(AirportsServices airportsServices)
        {
            _airportsServices = airportsServices;
        }

        //Insere um aeroporto na collection Airports
        [HttpPost]
        public async Task<ActionResult<Airports>> CreateAirportAsync(Airports airportIn)
        {
            var airport = await _airportsServices.GetOneIataAsync(airportIn.IATA);

            if (airport == null)
            {
                await _airportsServices.CreateAirportAsync(airportIn);
                return CreatedAtRoute("GetAirport", new { id = airportIn.IATA }, airportIn);
            }
            return Ok(airport);
        }

        [HttpGet]
        public async Task<ActionResult<List<Airports>>> GetAllAsync() => await _airportsServices.GetAllAsync();

        [HttpGet("AllDeleted")]
        public async Task<ActionResult<List<Airports>>> GetDeletedAsync() => await _airportsServices.GetDeletedAsync();

        [HttpGet("{iata:length(3)}", Name = "GetAirport")]
        public async Task<ActionResult<Airports>> GetOneIataAsync(string iata)
        {
            var airport = await _airportsServices.GetOneIataAsync(iata);

            if (airport == null)
            {
                airport = await _airportsServices.GetAirportWEBAPIAsync(iata);
                if (airport == null)
                {
                    return NotFound("Aeroporto não encontrado!");
                }
                else
                {
                    await _airportsServices.CreateAirportAsync(airport);
                    return Ok(airport);
                }
            }
            return Ok(airport);
        }


        [HttpPut]
        public async Task<ActionResult<Airports>> UpdateAsync(string iata, Airports airportIn)
        {
            var airport = await _airportsServices.GetOneIataAsync(iata);

            if (airport == null)
            {
                return NotFound();
            }

            await _airportsServices.UpdateAsync(iata, airportIn);

            return CreatedAtRoute("GetAirport", new { id = airportIn.IATA }, airportIn);
        }

        //Insere um aeroporto na collection de apagados e remove da Collection Airports
        [HttpDelete]
        public async Task<ActionResult> RemoveOneAsync(string iata)
        {
            var airport = await _airportsServices.GetOneIataAsync(iata);

            if (airport == null)
            {
                return NotFound();
            }

            await _airportsServices.CreateTrashAsync(airport);

            await _airportsServices.RemoveOneAsync(airport);

            return NoContent();
        }
    }
}
