using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SummerCamp.DataAccessLayer.Interfaces;
using SummerCamp.DataAccessLayer.Repositories;
using SummerCamp.DataModels.Models;
using SummerCamp.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SummerCamp.Controllers
{
    public class CompetitionsController : Controller
    {
        // GET: /<controller>/

        private readonly IMapper _mapper;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly ISponsorRepository _sponsorRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ICompetitionMatchRepository _competitionMatchRepository;
        private readonly ICompetitionTeamRepository _competitionTeamRepository;



        public CompetitionsController(IMapper mapper,
                                      ICompetitionRepository competitionRepository,
                                      ISponsorRepository sponsorRepository,
                                      ITeamRepository teamRepository,
                                      ICompetitionMatchRepository competitionMatchRepository,
                                      ICompetitionTeamRepository competitionTeamRepository
                                      )
        {
            _mapper = mapper;
            _competitionRepository = competitionRepository;
            _sponsorRepository = sponsorRepository;
            _teamRepository = teamRepository;
            _competitionMatchRepository = competitionMatchRepository;
            _competitionTeamRepository = competitionTeamRepository;

        }
        public IActionResult Index()
        {
            var competitions = _competitionRepository.GetAll();
            var competitionModels = _mapper.Map<List<CompetitionViewModel>>(competitions);
            return View(competitionModels);
        }

        public IActionResult Add()
        {
            string user = HttpContext.Session.GetString("Username");
            if(!string.IsNullOrEmpty(user))
            {
                var sponsors = _sponsorRepository.GetAll();
                var sponsorsSelectList = new SelectList(sponsors, "Id", "Name");
                ViewData["Sponsors"] = sponsorsSelectList;
                var teams = _teamRepository.GetAll();
                var competitionViewModel = new CompetitionViewModel { Teams = _mapper.Map<List<TeamViewModel>>(teams) };
                return View(competitionViewModel);
            }
            return View("LoginError");
        }

        [HttpPost]
        public IActionResult Add(CompetitionViewModel CompetitionViewModel)
        {
            var sponsors = _sponsorRepository.GetAll();
            var sponsorsSelectList = new SelectList(sponsors, "Id", "Name");
            ViewData["Sponsors"] = sponsorsSelectList;
            if (ModelState.IsValid)
            {
                _competitionRepository.Add(_mapper.Map<Competition>(CompetitionViewModel));
                _competitionRepository.Save();
                return RedirectToAction("Index");
            }
    
            return View(CompetitionViewModel);
        }

        public IActionResult Edit(int CompetitionId)
        {
            string user = HttpContext.Session.GetString("Username");
            if (!string.IsNullOrEmpty(user))
            {
                var competition = _competitionRepository.GetById(CompetitionId);
                var sponsors = _sponsorRepository.GetAll();
                var sponsorsSelectList = new SelectList(sponsors, "Id", "Name");
                ViewData["Sponsors"] = sponsorsSelectList;
                return View(_mapper.Map<CompetitionViewModel>(competition));
            }
            return View("LoginError");
        }

        [HttpPost]
        public IActionResult Edit(CompetitionViewModel? competitionViewModel)
        {
            if (ModelState.IsValid)
            {
                _competitionRepository.Update(_mapper.Map<Competition>(competitionViewModel));
                _competitionRepository.Save();
                return RedirectToAction("Index");
            }
            var sponsors = _sponsorRepository.GetAll();
            var sponsorsSelectList = new SelectList(sponsors, "Id", "Name");
            ViewData["Sponsors"] = sponsorsSelectList;
            return View(competitionViewModel);
        }
        public IActionResult Delete(int CompetitionId)
        {
            string user = HttpContext.Session.GetString("Username");
            if (!string.IsNullOrEmpty(user))
            {
                var competition = _competitionRepository.GetById(CompetitionId);
                var competitionTeams = _competitionTeamRepository.Get(cT => cT.CompetitionId == CompetitionId).ToList();
                foreach (var cTeam in competitionTeams)
                {
                    _competitionTeamRepository.Delete(cTeam);
                }
                var competitionMatches = _competitionMatchRepository.Get(cM => cM.CompetitionId == CompetitionId);
                foreach (var cMatches in competitionMatches)
                {
                    _competitionMatchRepository.Delete(cMatches);
                }
                _competitionRepository.Delete(competition);
                _competitionRepository.Save();
            }
            return RedirectToAction("Index");
        }
    }
}

