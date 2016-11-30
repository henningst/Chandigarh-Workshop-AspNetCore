namespace Services.Configuration
{
    /// <summary>
    /// Strongly typed configuration file for MongoDbRepository
    /// </summary>
    public class MongoDbRepositoryConfiguration
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
