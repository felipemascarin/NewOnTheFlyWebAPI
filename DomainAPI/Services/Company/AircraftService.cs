using DomainAPI.Dto.Company;
using DomainAPI.Models.Aircraft;
using DomainAPI.Models.Company;
using Nancy.Json;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DomainAPI.Services.Company
{
    public class AircraftService
    {
        public AircraftService()
        {
        }

        public async Task<Aircrafts> GetAddress(string cnpj)
        {
            Aircrafts aircraft;
            using (HttpClient _aircraftClient = new HttpClient())
            {
                HttpResponseMessage response = await _aircraftClient.GetAsync($"https://viacep.com.br/ws/{cnpj}/json/");
                var adressJson = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode) return aircraft = JsonConvert.DeserializeObject<Aircrafts>(adressJson);
                else return null;
            }
        }

        public async Task<bool> UpdateAircraft(Aircrafts aircraft)  /// tentativa de dar update com o objeto completo ja atualizado
        {
            using (HttpClient _aircraftClient = new HttpClient())
            {
                //string jsonString = JsonSerializer.Serialize(aircraft);
                string jsonString = new JavaScriptSerializer().Serialize(aircraft);

                HttpContent http = new StringContent(jsonString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _aircraftClient.PutAsync($"https://localhost:44375/api/Aircraft/UpdateCompany/", http); // alterar endpoint!

                if (response.IsSuccessStatusCode) return true;
                return false;
            }
        }

        public async Task<List<Aircrafts>> GetAircraft(string cnpj)
        {
            List<Aircrafts> aircraft = new();
            using (HttpClient _aircraftClient = new HttpClient())
            {
                HttpResponseMessage response = await _aircraftClient.GetAsync($"https://localhost:44375/api/Aircraft/");
                var adressJson = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var aircraftIn = JsonConvert.DeserializeObject<List<Aircrafts>>(adressJson);

                    foreach (var item in aircraftIn)
                    {
                        aircraft.Add(new Aircrafts
                        {
                            Id = item.Id,
                            RAB = item.RAB,
                            Capacity = item.Capacity,
                            DtRegistry = item.DtRegistry,
                            DtLastFlight = item.DtLastFlight,
                            Company = item.Company
                        });
                    }

                    return aircraft;
                }
                
                else return null;
            }
        }
    }
}
