using DomainAPI.Database.Airport.Interface;

namespace DomainAPI.Database.Airport
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string AirportsCollectionName { get; set; }
        public string AirportsTrashCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}