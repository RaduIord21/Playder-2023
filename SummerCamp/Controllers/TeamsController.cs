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
    public class TeamsController : Controller
    {
        // GET: /<controller>/

        private readonly IMapper _mapper;
        private readonly ITeamRepository _teamRepository;
        private readonly ICoachRepository _coachRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly ITeamSponsorRepository _teamSponsorRepository;
        private readonly ISponsorRepository _sponsorRepository;
        private readonly ICompetitionTeamRepository _competitionTeamRepository;


        public TeamsController(
            IMapper mapper,
            ITeamRepository TeamRepository,
            ICoachRepository coachRepository,
            IPlayerRepository playerRepository,
            ITeamSponsorRepository teamSponsorRepository,
            ISponsorRepository sponsorRepository,
            ICompetitionTeamRepository competitionTeamRepository
            )
        {
            _mapper = mapper;
            _teamRepository = TeamRepository;
            _coachRepository = coachRepository;
            _playerRepository = playerRepository;
            _teamSponsorRepository = teamSponsorRepository;
            _sponsorRepository = sponsorRepository;
            _competitionTeamRepository = competitionTeamRepository; 
        }
        public IActionResult Index()
        {
            string user = HttpContext.Session.GetString("Username");

            if (!string.IsNullOrEmpty(user))
            {
                var teams = _teamRepository.GetAll();
                var TeamModels = _mapper.Map<List<TeamViewModel>>(teams);
                return View(TeamModels);
            }
            return View("LoginError");
        }

        public IActionResult Add()
        {
            var coaches = _coachRepository.GetAll();
            var freeCoaches = from coach in coaches
                              join team in _teamRepository.GetAll() on coach.Id equals team.CoachId into teams
                              from t in teams.DefaultIfEmpty()
                              where t == null
                              select coach;
            var coachesList = new SelectList(freeCoaches, "Id", "Name").ToList();
            ViewData["Coaches"] = coachesList;
            var players = _playerRepository.Get(p => p.TeamId == null);
            var sponsors = _sponsorRepository.GetAll();
            ViewBag.Sponsors = sponsors;
            var teamViewModel = new TeamViewModel
            {
                Players = _mapper.Map<List<PlayerViewModel>>(players)
            };
                
            return View(teamViewModel);
        }

        [HttpPost]
        public IActionResult Add(TeamViewModel teamViewModel)
        {
            var players = _playerRepository.Get(p => p.TeamId == null);
            var sponsors = _sponsorRepository.GetAll();
            if (ModelState.IsValid)
            {
                var team = _mapper.Map<Team>(teamViewModel);
                _teamRepository.Add(team);
                _teamRepository.Save();
                foreach (var player in players)
                {
                    if (teamViewModel.SelectedPlayerIds != null && teamViewModel.SelectedPlayerIds.Contains(player.Id))
                    {
                        player.TeamId = team.Id;
                    }
                    else
                    {
                        player.TeamId = null;
                    }
                    _playerRepository.Update(player);
                    _playerRepository.Save();
                }
                foreach (var sponsor in sponsors)
                {
                    if (teamViewModel.SelectedSponsorIds != null && teamViewModel.SelectedSponsorIds.Contains(sponsor.Id))
                    {
                        var teamSponsor = new TeamSponsor { SponsorId = sponsor.Id, TeamId = team.Id,
                            Sponsor =sponsor, Team =team };
                        _teamSponsorRepository.Add(teamSponsor);
                        _teamSponsorRepository.Save();
                    }
                }
                return RedirectToAction("Index");
            }
            var coaches = _coachRepository.GetAll();
            var coachesList = new SelectList(coaches, "Id", "Name").ToList();
            ViewData["Coaches"] = coachesList;
            ViewBag.Sponsors = sponsors;
            teamViewModel.Players = _mapper.Map<List<PlayerViewModel>>(players);
            return View(teamViewModel);
        }

        public IActionResult Edit(int teamId)
        {
            var team = _teamRepository.GetById(teamId);
            var sponsors = _sponsorRepository.GetAll();
            ViewBag.Sponsors = sponsors;
            var freeCoaches = _coachRepository.Get(c=>c.Teams == null || c.Teams.Contains(team));
            var coachesList = new SelectList(freeCoaches, "Id", "Name").ToList();
            ViewData["Coaches"] = coachesList;
            var players = _playerRepository.Get(p => p.TeamId == null || p.TeamId == teamId);
            var teamViewModel = _mapper.Map<TeamViewModel>(team);
            teamViewModel.Players = _mapper.Map<List<PlayerViewModel>>(players) ;
            teamViewModel.SelectedPlayerIds = players.Where(p => p.TeamId == teamId).Select(p => p.Id).ToList();
            teamViewModel.SelectedSponsorIds = _teamSponsorRepository.Get(tS => tS.TeamId == teamId).Select(tS => (int)tS.SponsorId).ToList();
            return View(teamViewModel);
        }

        [HttpPost]
        public IActionResult Edit(TeamViewModel teamViewModel)
        {
            var sponsors = _sponsorRepository.GetAll();
            ViewBag.Sponsors = sponsors;
            var existingSponsors = _teamSponsorRepository.Get(tS => tS.TeamId == teamViewModel.Id).Select(tS => tS.SponsorId).ToList();
            var players = _playerRepository.Get(p => p.TeamId == null || p.TeamId == teamViewModel.Id);
            if (ModelState.IsValid)
            {
                foreach (var player in players)
                {
                    if (teamViewModel.SelectedPlayerIds != null && teamViewModel.SelectedPlayerIds.Contains(player.Id))
                    {
                        player.TeamId = teamViewModel.Id;
                    }
                    else
                    {
                        player.TeamId = null;
                    }
                    _playerRepository.Update(player);
                    _playerRepository.Save();
                }
                var team = _mapper.Map<Team>(teamViewModel);
                _teamRepository.Update(team);
                _teamRepository.Save();
                foreach (var sponsor in sponsors)
                {
                    if (teamViewModel.SelectedSponsorIds != null && teamViewModel.SelectedSponsorIds.Contains(sponsor.Id) && existingSponsors!=null && !existingSponsors.Contains(sponsor.Id))
                    {
                        var teamSponsor = new TeamSponsor
                        {
                            SponsorId = sponsor.Id,
                            TeamId = team.Id,
                            Sponsor = sponsor,
                            Team = team
                        };
                        _teamSponsorRepository.Add(teamSponsor);
                        _teamSponsorRepository.Save();
                    }
                    if ((teamViewModel.SelectedSponsorIds == null || !teamViewModel.SelectedSponsorIds.Contains(sponsor.Id)) && existingSponsors != null && existingSponsors.Contains(sponsor.Id))
                    {
                        var teamSponsor = _teamSponsorRepository.Get(tS => tS.SponsorId == sponsor.Id && tS.TeamId == team.Id);
                        foreach (var each in teamSponsor) {
                            _teamSponsorRepository.Delete(each);
                            _teamSponsorRepository.Save();
                        }
                    }
                    
                }
                return RedirectToAction("Index");
            }
            var coaches = _coachRepository.GetAll(); 
            var coachesList = new SelectList(coaches, "Id", "Name").ToList();
            ViewData["Coaches"] = coachesList;
            teamViewModel.Players = _mapper.Map<List<PlayerViewModel>>(players);
            return View(teamViewModel);
        }
        public IActionResult Delete(int TeamId)
        {
            var Team = _teamRepository.GetById(TeamId);
            var asignedPlayers = _playerRepository.Get(p => p.TeamId == TeamId);
            foreach (var player in asignedPlayers) {
                player.TeamId = null;
                _playerRepository.Update(player);
                _playerRepository.Save();
            }
            var teamSponsors = _teamSponsorRepository.Get(tS => tS.TeamId == TeamId);
            foreach (var teamSponsor in teamSponsors) {
                teamSponsor.TeamId = null;
                teamSponsor.Team = null;
            }
            var competitionTeams = _competitionTeamRepository.Get(cT => cT.TeamId == TeamId);
            foreach (var competitionTeam in competitionTeams) {
                competitionTeam.Team = null;
                competitionTeam.TeamId = null;
            }
            _teamRepository.Delete(Team);
            _teamRepository.Save();
            return RedirectToAction("Index");
        }
    }
}


