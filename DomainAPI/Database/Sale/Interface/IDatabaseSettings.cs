namespace DomainAPI.Database.Sale.Interface
{
    public interface IDatabaseSettings
    {
        public string SalesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DataBaseName { get; set; }
    }
}
