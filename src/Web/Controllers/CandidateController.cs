using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Web.Controllers
{
    public class CandidateController : Controller
    {
        private readonly IRepository _repository;

        public CandidateController(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var candidates = await _repository.GetCandidates();
            return View(candidates);
        }
    }
}
