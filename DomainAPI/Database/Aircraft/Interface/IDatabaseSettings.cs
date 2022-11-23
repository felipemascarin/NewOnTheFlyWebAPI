namespace DomainAPI.Database.Aircraft.Interface
{
    public interface IDatabaseSettings
    {
        public string AircraftCollectionName { get; set; }
        public string DeadfileCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
