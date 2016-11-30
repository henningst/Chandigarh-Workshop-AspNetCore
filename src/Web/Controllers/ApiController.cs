using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Models;

namespace Web.Controllers
{
    [Route("api/candidates")]
    public class ApiController : Controller
    {
        private readonly IRepository _repository;

        public ApiController(IRepository repository)
        {
            _repository = repository;
        }

        // GET: api/candidates
        [HttpGet]
        public async Task<IEnumerable<Candidate>> Get()
        {
            var candidates = await _repository.GetCandidates();

            return candidates;
        }

        // GET api/candidates/<guid>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var candidate = await _repository.GetCandidate(id);
            if (candidate == null)
            {
                return NotFound();
            }
            return new ObjectResult(candidate);
        }

        // POST api/candidates
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/candidates/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/candidates/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
