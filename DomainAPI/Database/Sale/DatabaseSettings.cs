using DomainAPI.Database.Sale.Interface;

namespace DomainAPI.Database.Sale
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string SalesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DataBaseName { get; set; }
    }
}
