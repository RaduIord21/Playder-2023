using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SummerCamp.DataAccessLayer.Interfaces;
using SummerCamp.DataAccessLayer.Repositories;
using SummerCamp.DataModels.Models;
using SummerCamp.Models;

namespace SummerCamp.Controllers
{
    public class CompetitionTeamsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICompetitionTeamRepository _competitionTeamRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly ICompetitionMatchRepository _competitionMatchRepository;

        public CompetitionTeamsController(
            IMapper mapper,
            ICompetitionTeamRepository competitionTeamRepository,
            ITeamRepository teamRepository,
            ICompetitionRepository competitionRepository,
            ICompetitionMatchRepository competitionMatchRepository) {

            _mapper = mapper;
            _competitionTeamRepository = competitionTeamRepository;
            _teamRepository = teamRepository;
            _competitionRepository = competitionRepository;
            _competitionMatchRepository = competitionMatchRepository;
        }

        // GET: /<controller>/
        public IActionResult Index(int competitionId)
        {
            var competitionTeams = _competitionTeamRepository.GetAll();
            ViewBag.CompetitionId = competitionId;
            var allCompetitionMatches = _competitionMatchRepository.Get(m => m.CompetitionId == competitionId);
            foreach (var competitionTeam in competitionTeams)
            {
                competitionTeam.Team = _teamRepository.GetById((int)competitionTeam.TeamId);
                competitionTeam.TotalPoints = 0;
                foreach (var match in allCompetitionMatches) {
                    if (match.AwayTeamId == competitionTeam.TeamId && match.AwayTeamGoals > match.HomeTeamGoals) {
                        competitionTeam.TotalPoints += 3;
                    }
                    if (match.HomeTeamId == competitionTeam.TeamId && match.HomeTeamGoals > match.AwayTeamGoals) {
                        competitionTeam.TotalPoints += 3;
                    }
                    if (match.AwayTeamGoals == match.HomeTeamGoals && (match.HomeTeamId == competitionTeam.TeamId || match.AwayTeamId == competitionTeam.TeamId)) {
                        competitionTeam.TotalPoints++;
                    }
                }
            }
            var competitionTeamViewModels = _mapper.Map<List<CompetitionTeamViewModel>>(competitionTeams);
            return View(competitionTeamViewModels);
        }

        public IActionResult Add(int competitionId) {
            var teams = _teamRepository.GetAll();
            ViewBag.Teams = teams;
            var competitionTeamViewModel = new CompetitionTeamViewModel
            {
                Competition = _competitionRepository.GetById(competitionId)
            };
            competitionTeamViewModel.SelectedTeamIds = _competitionTeamRepository.Get(t => t.CompetitionId == competitionId).Select(t => (int)t.TeamId).ToList();
            ViewBag.CompetitionId = competitionId;
            return View(competitionTeamViewModel);
        }

        [HttpPost]
        public IActionResult Add(CompetitionTeamViewModel competitionTeamViewModel) {
            if (ModelState.IsValid)
            {
                competitionTeamViewModel.Competition = _competitionRepository.GetById(competitionTeamViewModel.CompetitionId);
                var formerSelectedTeamIds = _competitionTeamRepository.Get(t => t.CompetitionId == competitionTeamViewModel.CompetitionId).Select(t => (int)t.TeamId).ToList();
                var allTeamIds = _teamRepository.GetAll().Select(t => t.Id).ToList();
                foreach (var team in allTeamIds) {
                    if ((formerSelectedTeamIds != null && formerSelectedTeamIds.Contains(team)) && (competitionTeamViewModel.SelectedTeamIds == null || !competitionTeamViewModel.SelectedTeamIds.Contains(team))) {
                        var competitionTeam = _competitionTeamRepository.Get(t => t.CompetitionId == competitionTeamViewModel.CompetitionId && t.TeamId == team).FirstOrDefault();
                        _competitionTeamRepository.Delete(competitionTeam);
                    }
                    if ((formerSelectedTeamIds == null || !formerSelectedTeamIds.Contains(team)) && (competitionTeamViewModel.SelectedTeamIds != null && competitionTeamViewModel.SelectedTeamIds.Contains(team) ))
                    {
                        competitionTeamViewModel.TeamId = team;
                        competitionTeamViewModel.Team = _teamRepository.GetById(team);
                        _competitionTeamRepository.Add(_mapper.Map<CompetitonTeam>(competitionTeamViewModel));
                    }
                }
                _competitionTeamRepository.Save();
                ViewBag.CompetitionId = competitionTeamViewModel.CompetitionId;
                return RedirectToAction("Index", new { competitionId = competitionTeamViewModel.CompetitionId });
            }
            return View(competitionTeamViewModel);
        }
    }
}