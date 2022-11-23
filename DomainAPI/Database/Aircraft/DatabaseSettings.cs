using DomainAPI.Database.Aircraft.Interface;

namespace DomainAPI.Database.Aircraft
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string AircraftCollectionName { get; set; }
        public string DeadfileCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
