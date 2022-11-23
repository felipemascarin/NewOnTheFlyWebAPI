using DomainAPI.Database.Aircraft.Interface;
using DomainAPI.Dto.Company;
using DomainAPI.Models.Aircraft;
using DomainAPI.Models.Company;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DomainAPI.Services.Aircraft
{
    public class AircraftServices
    {
        private readonly IMongoCollection<Aircrafts> _aircraft;

        public AircraftServices(IDatabaseSettings settings)
        {
            var aircraft = new MongoClient(settings.ConnectionString);
            var database = aircraft.GetDatabase(settings.DatabaseName);
            _aircraft = database.GetCollection<Aircrafts>(settings.AircraftCollectionName);
        }

        public async Task<List<Aircrafts>> Get() => await _aircraft.Find(aircraft => true).ToListAsync();

        public async Task<Aircrafts> Get(string rab) => await _aircraft.Find(aircraft => aircraft.RAB == rab).FirstOrDefaultAsync();

        public async Task Create(Aircrafts aircraft) => await _aircraft.InsertOneAsync(aircraft);

        public async Task Put(string rab, Aircrafts aircraftIn) => await _aircraft.ReplaceOneAsync(aircraft => aircraft.RAB == rab, aircraftIn);

        public async Task Remove(string rab) => await _aircraft.DeleteOneAsync(Aircraft => Aircraft.RAB == rab);

        //Chamada do endpoint para obter um objeto companhia, passando como parametro o cnpj da mesma. 
        public async Task<Companys> GetCompany(string cnpj)
        {
            CompanyDto companyDto;
            using (HttpClient _companyClient = new HttpClient())
            {
                HttpResponseMessage response = await _companyClient.GetAsync($"https://localhost:44379/api/Company/{cnpj}");

                var companyJson = await response.Content.ReadAsStringAsync();

                //se o retorno do método for verdadeiro, significa que o objeto foi localizado
                if (response.IsSuccessStatusCode)
                {
                    //Desserialização para transformar o retorno em objeto.
                    companyDto = JsonConvert.DeserializeObject<CompanyDto>(companyJson);

                    var company = new Companys()
                    {
                        Id = companyDto.Id,
                        CNPJ = companyDto.CNPJ,
                        Name = companyDto.Name,
                        NameOpt = companyDto.NameOpt,
                        DtOpen = companyDto.DtOpen,
                        Status = companyDto.Status,
                        Address = new()
                        {
                            ZipCode = companyDto.Address.ZipCode,
                            Street = companyDto.Address.Street,
                            Number = companyDto.Address.Number,
                            Complement = companyDto.Address.Complement,
                            City = companyDto.Address.City,
                            State = companyDto.Address.State
                        }
                    };

                    return company; // retorna uma companhia.
                }
                return null;
            }
        }
    }
}
