using DomainAPI.Database.Company.Interface;
using DomainAPI.Models.Company;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainAPI.Services.Company
{
    public class CompanyServices
    {
        private readonly IMongoCollection<Companys> _company;

        public CompanyServices(IDatabaseSettings settings)
        {
            var company = new MongoClient(settings.ConnectionString);
            var database = company.GetDatabase(settings.DatabaseName);
            _company = database.GetCollection<Companys>(settings.CompanyCollectionName);
        }

        public async Task<List<Companys>> Get() => await _company.Find(company => true).ToListAsync();

        public async Task<Companys> Get(string cnpj) => await _company.Find(company => company.CNPJ == cnpj).FirstOrDefaultAsync();

        public async Task Create(Companys company) => await _company.InsertOneAsync(company);

        public async Task Put(string cnpj, Companys companyIn) => await _company.ReplaceOneAsync(companyIn => companyIn.CNPJ == cnpj, companyIn);

        public async Task Remove(string cnpj) => await _company.DeleteOneAsync(company => company.CNPJ == cnpj);


    }
}
