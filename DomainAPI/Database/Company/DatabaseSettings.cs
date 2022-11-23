using DomainAPI.Database.Company.Interface;

namespace DomainAPI.Database.Company
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string CompanyCollectionName { get; set; }
        public string RestrictedCollectionName { get; set; }
        public string DeadfileCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
