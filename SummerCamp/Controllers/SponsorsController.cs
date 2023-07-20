using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SummerCamp.DataAccessLayer.Interfaces;
using SummerCamp.DataModels.Models;
using SummerCamp.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SummerCamp.Controllers
{
    public class SponsorsController : Controller
    {
        // GET: /<controller>/

        private readonly IMapper _mapper;
        private readonly ISponsorRepository _sponsorRepository;
        private readonly ITeamSponsorRepository _teamSponsorRepository;
        private readonly ICompetitionRepository _competitionRepository;

        public SponsorsController(IMapper mapper,
            ISponsorRepository sponsorRepository,
            ITeamSponsorRepository teamSponsorRepository,
            ICompetitionRepository competitionRepository)
        {
            _mapper = mapper;
            _sponsorRepository = sponsorRepository;
            _teamSponsorRepository = teamSponsorRepository;
            _competitionRepository = competitionRepository;
        }
        public IActionResult Index()
        {
           var sponsors = _sponsorRepository.GetAll();
           var sponsorModels = _mapper.Map<List<SponsorViewModel>>(sponsors);
           return View(sponsorModels);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(SponsorViewModel sponsorViewModel)
        {
            if (ModelState.IsValid)
            {
                _sponsorRepository.Add(_mapper.Map<Sponsor>(sponsorViewModel));
                _sponsorRepository.Save();
                return RedirectToAction("Index");
            }
            return View(sponsorViewModel);
        }

        public IActionResult Edit(int SponsorId)
        {
            var sponsor = _sponsorRepository.GetById(SponsorId);
            return View(_mapper.Map<SponsorViewModel>(sponsor));
        }

        [HttpPost]
        public IActionResult Edit(SponsorViewModel? sponsorViewModel)
        {
            if (ModelState.IsValid)
            {
                _sponsorRepository.Update(_mapper.Map<Sponsor>(sponsorViewModel));
                _sponsorRepository.Save();
                return RedirectToAction("Index");
            }
            return View(sponsorViewModel);
        }
        public IActionResult Delete(int SponsorId)
        {
            var sponsor = _sponsorRepository.GetById(SponsorId);
            var teamSponsors = _teamSponsorRepository.Get(tS => tS.SponsorId == SponsorId);
            var competitions = _competitionRepository.Get(c => c.SponsorId == SponsorId);
            foreach (var teamSponsor in teamSponsors)
            {
                teamSponsor.SponsorId = null;
                teamSponsor.Sponsor = null;
            }

            foreach (var competition in competitions) {
                competition.SponsorId = null;
                competition.Sponsor = null;
            }
            _sponsorRepository.Delete(sponsor);
            _sponsorRepository.Save();
            return RedirectToAction("Index");
        }
    }
}

