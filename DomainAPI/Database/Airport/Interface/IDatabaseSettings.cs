namespace DomainAPI.Database.Airport.Interface
{
    public interface IDatabaseSettings
    {
        public string AirportsCollectionName { get; set; }
        public string AirportsTrashCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
