using DomainAPI.Models.Flight;
using DomainAPI.Models.Passenger;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using System.Net;
using Nancy.Json;
using Microsoft.AspNetCore.Mvc;
using DomainAPI.Models.Aircraft;
using System.Text;

namespace Saler.Controllers
{
    public class ConsumerController
    {
        private readonly string _consumerGetFligth = "https://localhost:44330/api/Flights/GetOneFlight/";
        private readonly string _consumerPutFligth = "https://localhost:44330/api/Flights/";
        private readonly string _consumerGetPassenger = "https://localhost:44388/api/Passenger/StatusValids/Cpf?cpf=";
        public async Task<Flights> GetFlightAsync(DateTime date, string rab)
        {
            using (HttpClient _adressClient = new())
            {
                HttpResponseMessage response = await _adressClient.GetAsync(_consumerGetFligth + date.ToString("yyyy-MM-ddZ") + "/" + rab);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return new JavaScriptSerializer().Deserialize<Flights>(json);
            }
        }

        public async Task<bool> PutFlightAsync(Flights flight)
        {
            using (HttpClient _flightClient = new HttpClient())
            {
                string jsonString = new JavaScriptSerializer().Serialize(flight);

                HttpContent http = new StringContent(jsonString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _flightClient.PutAsync(_consumerPutFligth + flight.Id, http);
                var teste = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                    return true;
                return false;
            }
        }

        public async Task<Passengers> GetPassengerAsync(string cpf)
        {
            using (HttpClient _adressClient = new())
            {
                HttpResponseMessage response = await _adressClient.GetAsync(_consumerGetPassenger + cpf);
                var json = await response.Content.ReadAsStringAsync();
                return new JavaScriptSerializer().Deserialize<Passengers>(json);
            }
        }
    }
}
