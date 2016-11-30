using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Models;

namespace Services
{
    public interface IRepository
    {
        Task SaveCandidate(Candidate candidate);
        Task<IEnumerable<Candidate>> GetCandidates();
        Task<Candidate> GetCandidate(Guid id);
    }
}
