using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using Services;
using Services.Models;
using Web;
using Xunit;

namespace WebTests
{
    public class ApiControllerTests
    {
        private readonly TestServer _server;
        private readonly Mock<IRepository> _repository;

        public ApiControllerTests()
        {
            // Create a Mock version of IRepository that we can use instead of calling to our
            // actual MongoDB backend.
            _repository = new Mock<IRepository>();

            // Create a test server that will host our API and tell it to use the Mock
            _server = new TestServer(new WebHostBuilder()
                .ConfigureServices(services => services.AddSingleton<IRepository>(sp => _repository.Object))
                .UseStartup<Startup>());       
        }

        [Fact]
        public async void Get_WhenCalled_ShouldReturnMultipleCandidates()
        {
            //
            // ARRANGE
            //


            // Create som mock data to return from our mock repository
            var c = new List<Candidate>()
            {
                new Candidate() { FirstName = "Richard", LastName = "Hendricks"},
                new Candidate() { FirstName = "Bertram", LastName = "Gilfoyle"}
            };

            // Setup our mock repository to return the mock data when GetCandidates() is called
            _repository.Setup(x => x.GetCandidates()).Returns(Task.FromResult(c.AsEnumerable()));

            // Create an HttpClient and call our actual API method
            using (var client = _server.CreateClient())
            {

                //
                // ACT
                //

                var response = await client.GetAsync("/api/candidates");
                var result = await response.Content.ReadAsStringAsync();
                var candidates = JsonConvert.DeserializeObject<IEnumerable<Candidate>>(result).ToArray();

                // 
                // ASSERT
                //

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.NotEmpty(candidates);
                Assert.True(candidates.Count() > 1);
            }
        }

        [Fact]
        public async void GetById_WhenCalledWithExistingId_ShouldReturnCandidate()
        {
            //
            // ARRANGE
            //

            // Create som mock data to return from our mock repository
            var candidateId = new Guid("93E88C21-BC9A-4826-8663-0C576068DDA0");
            var c = new Candidate() {Id = candidateId, FirstName = "Bertram", LastName = "Gilfoyle"};

            // Setup our mock repository to return the mock data when GetCandidates() is called
            _repository.Setup(x => x.GetCandidate(candidateId)).Returns(Task.FromResult(c));

            // Create an HttpClient and call our actual API method
            using (var client = _server.CreateClient())
            {
                //
                // ACT
                //

                var response = await client.GetAsync($"/api/candidates/{candidateId}");
                var result = await response.Content.ReadAsStringAsync();
                var candidate = JsonConvert.DeserializeObject<Candidate>(result);

                // 
                // ASSERT
                //

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.NotNull(candidate);
                Assert.Equal(candidateId, candidate.Id);
                Assert.Equal("Bertram", candidate.FirstName);
                Assert.Equal("Gilfoyle", candidate.LastName);
            }
        }

        [Fact]
        public async void GetById_WhenCalledWithNonExistingId_ShouldReturn404NotFound()
        {
            //
            // ARRANGE
            //

            // Setup our mock repository to return the mock data when GetCandidates() is called
            _repository.Setup(x => x.GetCandidate(It.IsAny<Guid>())).Returns(Task.FromResult<Candidate>(null));

            // Create an HttpClient and call our actual API method
            using (var client = _server.CreateClient())
            {
                //
                // ACT
                //
                var nonExistingId = new Guid("F1977DB5-9906-4EEA-8091-720AAAA324E7");
                var response = await client.GetAsync($"/api/candidates/{nonExistingId}");

                // 
                // ASSERT
                //

                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

    }

    /// <summary>
    /// Fake implementation of IRepository allowing us to use an in-memory
    /// version of the repository instead of connecting to an actual database.
    /// </summary>
    public class FakeRepository : IRepository
    {
        public Task SaveCandidate(Candidate candidate)
        {
            return Task.FromResult(0);
        }

        public Task<IEnumerable<Candidate>> GetCandidates()
        {
            var candidates = new List<Candidate>()
            {
                new Candidate() { FirstName = "Richard", LastName = "Hendricks"},
                new Candidate() { FirstName = "Bertram", LastName = "Gilfoyle"}
            };

            return Task.FromResult(candidates.AsEnumerable());
        }

        public Task<Candidate> GetCandidate(Guid id)
        {
            return Task.FromResult(new Candidate {Id = id, FirstName = "Dinesh", LastName = "Chugtai"});
        }
    }
}
