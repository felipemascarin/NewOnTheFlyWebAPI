namespace DomainAPI.Database.Flight.Interface
{
    public interface IDatabaseSettings
    {
        public string FlightsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
