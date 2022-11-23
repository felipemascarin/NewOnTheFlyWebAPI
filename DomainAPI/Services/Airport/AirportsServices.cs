using DomainAPI.Database.Airport.Interface;
using DomainAPI.Dto.Airport;
using DomainAPI.Models.Airport;
using DomainAPI.Utils.Airport;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DomainAPI.Services.Airport
{
    public class AirportsServices
    {
        private readonly IMongoCollection<Airports> _airportsServices;
        private readonly IMongoCollection<Airports> _airportsTrashServices;

        public AirportsServices(IDatabaseSettings settings)
        {
            var airport = new MongoClient(settings.ConnectionString);
            var database = airport.GetDatabase(settings.DatabaseName);
            _airportsServices = database.GetCollection<Airports>(settings.AirportsCollectionName);
            _airportsTrashServices = database.GetCollection<Airports>(settings.AirportsTrashCollectionName);
        }

        //Insere um aeroporto na collection Airports
        public async Task<Airports> CreateAirportAsync(Airports airport)
        {
            await _airportsServices.InsertOneAsync(airport);
            return airport;
        }

        //Insere um aeroporto na collection de apagados
        public async Task<Airports> CreateTrashAsync(Airports airportIn)
        {
            var airportdeleted = await _airportsTrashServices.Find(airport => airport.IATA.ToUpper() == airportIn.IATA.ToUpper()).FirstOrDefaultAsync();

            if (airportdeleted == null)
            {
                await _airportsTrashServices.InsertOneAsync(airportIn);
                return airportIn;
            }

            return airportIn;
        }

        public async Task<List<Airports>> GetAllAsync() => await _airportsServices.Find(airport => true).ToListAsync();

        public async Task<List<Airports>> GetByCityAsync(string city) => await _airportsServices.Find(airport => airport.City.ToUpper() == city.ToUpper()).ToListAsync();

        public async Task<List<Airports>> GetDeletedAsync() => await _airportsTrashServices.Find(airport => true).ToListAsync();

        public async Task<Airports> GetOneIataAsync(string iata) => await _airportsServices.Find(airport => airport.IATA.ToUpper() == iata.ToUpper()).FirstOrDefaultAsync();

        public async Task UpdateAsync(string iata, Airports airportIn) => await _airportsServices.ReplaceOneAsync(airport => airport.IATA.ToUpper() == iata.ToUpper(), airportIn);

        public async Task RemoveOneAsync(Airports airportRemove) => await _airportsServices.DeleteOneAsync(airport => airport.Id == airportRemove.Id);

        //Busca um aeroporto na AirportAPI pela IATA
        public async Task<Airports> GetAirportWEBAPIAsync(string iata)
        {
            var httpclient = new HttpClient();
            var airportresponse = await httpclient.GetAsync(AirportUtils.GetAPIUri("WebApiGetAirportUri") + iata);
            var JsonString = await airportresponse.Content.ReadAsStringAsync();

            var airportdto = JsonConvert.DeserializeObject<AirportsDto>(JsonString);

            var airport = new Airports()
            {
                IATA = airportdto.iata,
                City = airportdto.city,
                State = airportdto.state,
                Country = airportdto.country_id
            };

            return airport;
        }
    }
}
