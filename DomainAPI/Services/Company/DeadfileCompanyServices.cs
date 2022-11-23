using DomainAPI.Database.Company.Interface;
using DomainAPI.Models.Company;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainAPI.Services.Company
{
    public class DeadfileCompanyServices
    {
        private readonly IMongoCollection<DeadfileCompany> _deadfile;

        public DeadfileCompanyServices(IDatabaseSettings settings)
        {
            var deadfile = new MongoClient(settings.ConnectionString);
            var database = deadfile.GetDatabase(settings.DatabaseName);
            _deadfile = database.GetCollection<DeadfileCompany>(settings.DeadfileCollectionName);
        }

        public async Task<List<DeadfileCompany>> Get() => await _deadfile.Find(deadfile => true).ToListAsync();

        public async Task<DeadfileCompany> Get(string cnpj) => await _deadfile.Find(deadfile => deadfile.FileCompany.CNPJ == cnpj).FirstOrDefaultAsync();

        public async Task Create(DeadfileCompany deadfile) => await _deadfile.InsertOneAsync(deadfile);

        public async Task Put(string cnpj, DeadfileCompany deadfile) => await _deadfile.ReplaceOneAsync(deadfil => deadfil.FileCompany.CNPJ == cnpj, deadfile);

        public async Task Remove(string cnpj) => await _deadfile.DeleteOneAsync(deadfile => deadfile.FileCompany.CNPJ == cnpj);
    }
}
