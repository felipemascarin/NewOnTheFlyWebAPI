using DomainAPI.Database.Flight.Interface;

namespace DomainAPI.Database.Flight
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string FlightsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
