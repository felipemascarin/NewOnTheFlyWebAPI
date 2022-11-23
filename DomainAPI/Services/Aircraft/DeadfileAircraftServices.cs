using DomainAPI.Database.Aircraft.Interface;
using DomainAPI.Models.Aircraft;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainAPI.Services.Aircraft
{
    public class DeadfileAircraftServices
    {
        private readonly IMongoCollection<DeadfileAircrafts> _deadfiles;

        public DeadfileAircraftServices(IDatabaseSettings settings)
        {
            var deadfiles = new MongoClient(settings.ConnectionString);
            var database = deadfiles.GetDatabase(settings.DatabaseName);
            _deadfiles = database.GetCollection<DeadfileAircrafts>(settings.DeadfileCollectionName);
        }

        public async Task<List<DeadfileAircrafts>> Get() => await _deadfiles.Find(deadfiles => true).ToListAsync();

        public async Task<DeadfileAircrafts> Get(string rab) => await _deadfiles.Find(deadfiles => deadfiles.DeadfilesAircrafts.RAB == rab).FirstOrDefaultAsync();

        public async Task Create(DeadfileAircrafts deadfile) => await _deadfiles.InsertOneAsync(deadfile);

        public async Task Put(string rab, DeadfileAircrafts deadfileIn) => await _deadfiles.ReplaceOneAsync(deadfile => deadfile.DeadfilesAircrafts.RAB == rab, deadfileIn);

        public async Task Remove(string rab) => await _deadfiles.DeleteOneAsync(deadfile => deadfile.DeadfilesAircrafts.RAB == rab);
    }
}
