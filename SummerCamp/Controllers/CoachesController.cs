using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SummerCamp.DataAccessLayer.Interfaces;
using SummerCamp.DataAccessLayer.Repositories;
using SummerCamp.DataModels.Models;
using SummerCamp.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SummerCamp.Controllers
{
    public class CoachesController : Controller
    {
        // GET: /<controller>/

        private readonly IMapper _mapper;
        private readonly ICoachRepository _coachRepository;
        private readonly ITeamRepository _teamRepository;

        public CoachesController(IMapper mapper, ICoachRepository coachRepository, ITeamRepository teamRepository)
        {
            _mapper = mapper;
            _coachRepository = coachRepository;
            _teamRepository = teamRepository;
        }
        public IActionResult Index()
        {
            var coaches = _coachRepository.GetAll();
            var coachModels = _mapper.Map<List<CoachViewModel>>(coaches);
            return View(coachModels);
        }

        public IActionResult Add() {
            return View();
        }

        [HttpPost]
        public IActionResult Add(CoachViewModel coachViewModel)
        {
            if (ModelState.IsValid) {
                _coachRepository.Add(_mapper.Map<Coach>(coachViewModel));
                _coachRepository.Save();
                return RedirectToAction("Index");
            }
            return View(coachViewModel);
        }

        public IActionResult Edit(int CoachId) {
            var coach = _coachRepository.GetById(CoachId);
            return View(_mapper.Map<CoachViewModel>(coach));
        }

        [HttpPost]
        public IActionResult Edit(CoachViewModel? coachViewModel) {
            if (ModelState.IsValid) {
                _coachRepository.Update(_mapper.Map<Coach>(coachViewModel));
                _coachRepository.Save();
                return RedirectToAction("Index");
            }
            return View(coachViewModel);
        }
        public IActionResult Delete(int CoachId)
        {
            var coach = _coachRepository.GetById(CoachId);
            var coaches = _coachRepository.GetAll();
            var teams = _teamRepository.GetAll();
            var team = from t in teams where t.CoachId == CoachId select t;

            foreach (var each in team)
            {
                each.CoachId = null;
                each.Coach = null;
            }
        
            _coachRepository.Delete(coach);
            _coachRepository.Save();
            return RedirectToAction("Index");
        }
    }
}

