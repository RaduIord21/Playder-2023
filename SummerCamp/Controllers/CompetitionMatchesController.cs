
using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SummerCamp.DataAccessLayer.Interfaces;
using SummerCamp.DataModels.Models;
using SummerCamp.Models;

namespace SummerCamp.Controllers
{
	public class CompetitionMatchesController : Controller
	{
		private readonly IMapper _mapper;
		private readonly ICompetitionMatchRepository _competitionMatchRepository;
		private readonly ITeamRepository _teamRepository;
        private readonly ICompetitionRepository _competitonRepository;

		public CompetitionMatchesController(IMapper mapper,
			ICompetitionMatchRepository competitionMatchRepository,
			ITeamRepository teamRepository,
			ICompetitionRepository competitionRepository
			)
		{
			_mapper = mapper;
			_competitionMatchRepository = competitionMatchRepository;
			_teamRepository = teamRepository;
			_competitonRepository = competitionRepository;
		}

		public IActionResult Index(int competitionId) {
			var competitonMatches = _competitionMatchRepository.GetAll();
			ViewData["CompetitionId"] = competitionId;
			foreach (var competitionMatch in competitonMatches) {	
				competitionMatch.AwayTeam = _teamRepository.GetById((int)competitionMatch.AwayTeamId);
				competitionMatch.HomeTeam = _teamRepository.GetById((int)competitionMatch.HomeTeamId);
			}
			return View(_mapper.Map<List<CompetitonMatchViewModel>>(competitonMatches));
		}

		public IActionResult Add(int CompetitionId)
		{
            string user = HttpContext.Session.GetString("Username");
			if (!string.IsNullOrEmpty(user)) {
				var awayTeams = _teamRepository.GetAll();
				var homeTeams = _teamRepository.GetAll();
				var awayTeamsList = new SelectList(awayTeams, "Id", "Name").ToList();
				var homeTeamsList = new SelectList(homeTeams, "Id", "Name").ToList();
				ViewData["AwayTeams"] = awayTeamsList;
				ViewData["HomeTeams"] = homeTeamsList;
				ViewData["Competion"] = CompetitionId;
				var competitionMatchViewModel = new CompetitonMatchViewModel { Competition = _competitonRepository.GetById(CompetitionId) };
				return View(competitionMatchViewModel);
			}
			return View("LoginError");
		}

		[HttpPost]
		public IActionResult Add(CompetitonMatchViewModel competitionMatchViewModel) {
            var awayTeams = _teamRepository.GetAll();
            var homeTeams = _teamRepository.GetAll();
            var awayTeamsList = new SelectList(awayTeams, "Id", "Name").ToList();
            var homeTeamsList = new SelectList(homeTeams, "Id", "Name").ToList();
            ViewData["AwayTeams"] = awayTeamsList;
            ViewData["HomeTeams"] = homeTeamsList;
            ViewData["Competion"] = competitionMatchViewModel.CompetitionId;
			if (ModelState.IsValid)
			{
				if (competitionMatchViewModel.AwayTeamId == competitionMatchViewModel.HomeTeamId)
				{
					ModelState.AddModelError("", "Echipele trebuie sa fie diferite");

					return View(competitionMatchViewModel);
				}
				if (_competitionMatchRepository.Get(m=>m.AwayTeamId == competitionMatchViewModel.AwayTeamId && m.HomeTeamId == competitionMatchViewModel.HomeTeamId).Count() > 0) {
                    ModelState.AddModelError("", "Meciul dintre aceste doua echipe a fost deja introdus");

                    return View(competitionMatchViewModel);
                }
				competitionMatchViewModel.AwayTeam = _teamRepository.GetById((int)competitionMatchViewModel.AwayTeamId);
				competitionMatchViewModel.HomeTeam = _teamRepository.GetById((int)competitionMatchViewModel.HomeTeamId);
				competitionMatchViewModel.Competition = _competitonRepository.GetById((int)competitionMatchViewModel.CompetitionId);
				var map = _mapper.Map<CompetitionMatch>(competitionMatchViewModel);
				_competitionMatchRepository.Add(map);
				_competitionMatchRepository.Save();

				return RedirectToAction("Index", new { competitionId = competitionMatchViewModel.CompetitionId });
			}
            return View(competitionMatchViewModel);
        }

		public IActionResult Edit(int competitionMatchId) {
            string user = HttpContext.Session.GetString("Username");
			if (!string.IsNullOrEmpty(user)) {
				var competitionMatch = _competitionMatchRepository.GetById(competitionMatchId);
				var awayTeams = _teamRepository.GetAll();
				var homeTeams = _teamRepository.GetAll();
				var awayTeamsList = new SelectList(awayTeams, "Id", "Name").ToList();
				var homeTeamsList = new SelectList(homeTeams, "Id", "Name").ToList();
				ViewData["AwayTeams"] = awayTeamsList;
				ViewData["HomeTeams"] = homeTeamsList;
				var competitionViewModel = _mapper.Map<CompetitonMatchViewModel>(competitionMatch);
				return View(competitionViewModel);
			}
			return View("LoginError");
        }
		[HttpPost]
		public IActionResult Edit(CompetitonMatchViewModel competitionMatchViewModel) {
            var awayTeams = _teamRepository.GetAll();
            var homeTeams = _teamRepository.GetAll();
            var awayTeamsList = new SelectList(awayTeams, "Id", "Name").ToList();
            var homeTeamsList = new SelectList(homeTeams, "Id", "Name").ToList();
            ViewData["AwayTeams"] = awayTeamsList;
            ViewData["HomeTeams"] = homeTeamsList;
            if (ModelState.IsValid)
			{
				competitionMatchViewModel.AwayTeam = _teamRepository.GetById((int)competitionMatchViewModel.AwayTeamId);
				competitionMatchViewModel.HomeTeam = _teamRepository.GetById((int)competitionMatchViewModel.HomeTeamId);
				competitionMatchViewModel.Competition = _competitonRepository.GetById((int)competitionMatchViewModel.CompetitionId);
				_competitionMatchRepository.Update(_mapper.Map<CompetitionMatch>(competitionMatchViewModel));
				_competitionMatchRepository.Save();
				return RedirectToAction("Index", new { competitionId = competitionMatchViewModel.CompetitionId }) ;

			}
			return View(competitionMatchViewModel);
		}
		public IActionResult Delete(int competitionMatchId) {
            string user = HttpContext.Session.GetString("Username");
			if (!string.IsNullOrEmpty(user))
			{
				var competitionMatchViewModel = _competitionMatchRepository.GetById(competitionMatchId);
				competitionMatchViewModel.AwayTeam = _teamRepository.GetById((int)competitionMatchViewModel.AwayTeamId);
				competitionMatchViewModel.HomeTeam = _teamRepository.GetById((int)competitionMatchViewModel.HomeTeamId);
				competitionMatchViewModel.Competition = _competitonRepository.GetById(competitionMatchViewModel.CompetitionId);
				_competitionMatchRepository.Delete(_mapper.Map<CompetitionMatch>(competitionMatchViewModel));
				_competitionMatchRepository.Save();
			}
			return RedirectToAction("Index");
		}
		
	} 
}

