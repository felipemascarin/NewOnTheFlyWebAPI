using DomainAPI.Models.Airport;
using DomainAPI.Models.Flight;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using DomainAPI.Database.Flight.Interface;
using Newtonsoft.Json;
using DomainAPI.Utils.FlightUtils;
using DomainAPI.Models.Aircraft;
using Nancy.Json;
using System.Text;

namespace DomainAPI.Services.Flight
{
    public class FlightsServices
    {
        private readonly IMongoCollection<Flights> _flightsServices;

        public FlightsServices(IDatabaseSettings settings)
        {
            var flight = new MongoClient(settings.ConnectionString);
            var database = flight.GetDatabase(settings.DatabaseName);
            _flightsServices = database.GetCollection<Flights>(settings.FlightsCollectionName);
        }

        public async Task<Flights> CreateFlightAsync(Flights flight)
        {
            await _flightsServices.InsertOneAsync(flight);
            return flight;
        }

        public async Task<List<Flights>> GetAllAsync() => await _flightsServices.Find(flight => true).ToListAsync();

        public async Task<Flights> GetOneAsync(string id) => await _flightsServices.Find(flight => flight.Id == id).FirstOrDefaultAsync();


        //Filtra um voo pela data e aeronave
        public async Task<Flights> GetOneAsync(DateTime date, string rab)
        {
            var flightsRabList = await _flightsServices.Find(flight => flight.Plane.RAB.ToUpper() == rab.ToUpper()).ToListAsync();
            foreach (var flight in flightsRabList)
            {
                if (flight.Departure.ToString("dd/MM/yyyy") == date.ToString("dd/MM/yyyy")) return flight;
            }
            return null;
        }

        //Filtra pela data do voo e retorna uma lista de voos ativos
        public async Task<List<Flights>> GetByDateAsync(DateTime date)
        {
            var flightsDateList = await _flightsServices.Find(flight => true).ToListAsync();
            List<Flights> Listflight = new();
            foreach (var flight in flightsDateList)
            {
                if (flight.Departure.ToString("dd/MM/yyyy") == date.ToString("dd/MM/yyyy") && flight.Status == true)
                    Listflight.Add(flight);
            }
            return Listflight;
        }

        //Filtra por intervalo de data e retorna uma lista de voos ativos
        public async Task<List<Flights>> GetByDateRangeAsync(DateTime initialdate, DateTime finaldate)
        {
            var flightsDateList = await _flightsServices.Find(flight => true).ToListAsync();
            List<Flights> Listflight = new();
            foreach (var flight in flightsDateList)
            {
                var compare1 = DateTime.Compare(initialdate, flight.Departure);
                var compare2 = DateTime.Compare(finaldate, flight.Departure);

                if (compare1 <= 0 && compare2 >= 0 && flight.Status == true)
                    Listflight.Add(flight);
            }
            return Listflight;
        }

        public async Task UpdateCancelFlightAsync(string id, Flights flightIn) => await _flightsServices.ReplaceOneAsync(flight => flight.Id == id, flightIn);

        public async Task UpdateAsync(string id, Flights flightIn) => await _flightsServices.ReplaceOneAsync(flight => flight.Id == id, flightIn);


        //Busca um aeroporto existente no banco de dados da API Airport e AirportAPI
        public async Task<Airports> GetAirportAPIAsync(string iata)
        {
            var httpclient = new HttpClient();
            var airportresponse = await httpclient.GetAsync(FlightUtils.GetAPIUri("ApiGetAirportUri") + iata);
            var JsonString = await airportresponse.Content.ReadAsStringAsync();
            var airport = JsonConvert.DeserializeObject<Airports>(JsonString);
            return airport;
        }

        //Busca uma aeronave existente
        public async Task<Aircrafts> GetAircraftAPIAsync(string rab)
        {
            var httpclient = new HttpClient();
            var airportresponse = await httpclient.GetAsync(FlightUtils.GetAPIUri("ApiGetAircraftUri") + rab);
            var JsonString = await airportresponse.Content.ReadAsStringAsync();

            JavaScriptSerializer ser = new JavaScriptSerializer();

            var aircraft = ser.Deserialize<Aircrafts>(JsonString);

            return aircraft;
        }

        //Altera a data de último voo da aeronave no banco de dados da API Aircraft
        public async Task<bool> PutDateAircraftAPIAsync(string rab)
        {
            var httpclient = new HttpClient();
            string jsonString = new JavaScriptSerializer().Serialize(rab);

            var http = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var aircraftresponse = await httpclient.PutAsync(FlightUtils.GetAPIUri("ApiPutAircrafUri") + rab, http);

            if (aircraftresponse.IsSuccessStatusCode) return true;
            return false;
        }
    }
}
