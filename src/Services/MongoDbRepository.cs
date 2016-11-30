using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Services.Configuration;
using Services.Models;

namespace Services
{
    public class MongoDbRepository : IRepository
    {
        private readonly MongoDbRepositoryConfiguration _configuration;
        private readonly IMongoDatabase _db;

        public MongoDbRepository(IOptions<MongoDbRepositoryConfiguration> configuration)
        {
            _configuration = configuration.Value;

            var server = new MongoClient(_configuration.ConnectionString);
            _db = server.GetDatabase(_configuration.DatabaseName);

        }
        public async Task SaveCandidate(Candidate candidate)
        {
            var collection = _db.GetCollection<Candidate>("Candidates");
            await collection.InsertOneAsync(candidate);
        }

        public async Task<IEnumerable<Candidate>>  GetCandidates()
        {
            var collection = _db.GetCollection<Candidate>("Candidates");

            var result = await collection.FindAsync(_ => true);
            return result.ToEnumerable();
        }

        public Task<Candidate> GetCandidate(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
