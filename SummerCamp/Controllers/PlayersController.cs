using System.Numerics;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SummerCamp.DataAccessLayer.Interfaces;
using SummerCamp.DataAccessLayer.Repositories;
using SummerCamp.DataModels.Enums;
using SummerCamp.DataModels.Models;
using SummerCamp.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SummerCamp.Controllers
{
    public class PlayersController : Controller
    {
        // GET: /<controller>/

        private readonly IMapper _mapper;
        private readonly IPlayerRepository _playerRepository;
        private readonly ITeamRepository _teamRepository;

        public PlayersController(IMapper mapper, IPlayerRepository playerRepository, ITeamRepository teamRepository)
        {
            _mapper = mapper;
            _playerRepository = playerRepository;
            _teamRepository = teamRepository;
        }
        public IActionResult Index()
        {
            string user = HttpContext.Session.GetString("Username");
            var players = _playerRepository.GetAll();
            if (!string.IsNullOrEmpty(user))
            {
                foreach (var player in players)
                {
                    if (player.TeamId != null)
                    {
                        player.Team = _teamRepository.GetById((int)player.TeamId);
                    }
                }
                var playerModels = _mapper.Map<List<PlayerViewModel>>(players);
                return View(playerModels);
            }
            return View("LoginError");
        }

            public IActionResult Add(){
            var playerModel = new PlayerViewModel
            {
                PositionList = Enum.GetValues(typeof(PositionEnum)).Cast<PositionEnum>().Select(
                    p => new SelectListItem
                    {
                        Text = p.ToString(),
                        Value = ((int)p).ToString()
                    })
                    .ToList()
            };
            return View(playerModel);
        }

        [HttpPost]
        public IActionResult Add(PlayerViewModel PlayerViewModel)
        {
            if (ModelState.IsValid)
            {
                _playerRepository.Add(_mapper.Map<Player>(PlayerViewModel));
                _playerRepository.Save();
                return RedirectToAction("Index");
            }
            PlayerViewModel.PositionList = Enum.GetValues(typeof(PositionEnum)).Cast<PositionEnum>().Select(
                    p => new SelectListItem
                    {
                        Text = p.ToString(),
                        Value = ((int)p).ToString()
                    })
                    .ToList();
            return View(PlayerViewModel);
        }

        public IActionResult Edit(int PlayerId)
        {
            var player = _playerRepository.GetById(PlayerId);
            var teams = _teamRepository.GetAll();
            var teamsList = new SelectList(teams, "Id", "Name").ToList();
            ViewData["Teams"] = teamsList;
            return View(_mapper.Map<PlayerViewModel>(player));
        }

        [HttpPost]
        public IActionResult Edit(PlayerViewModel? playerViewModel)
        {
            if (ModelState.IsValid)
            {
                _playerRepository.Update(_mapper.Map<Player>(playerViewModel));
                _playerRepository.Save();
                return RedirectToAction("Index");
            }
            var teams = _teamRepository.GetAll();
            var teamsList = new SelectList(teams, "Id", "Name").ToList();
            ViewData["Teams"] = teamsList;

            return View(playerViewModel);
        }
        public IActionResult Delete(int PlayerId)
        {
            var player = _playerRepository.GetById(PlayerId);
            _playerRepository.Delete(player);
            _playerRepository.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Details(int PlayerId) {
            var player = _playerRepository.GetById(PlayerId);
            if (player.TeamId != null)
            {
                player.Team = _teamRepository.GetById((int)player.TeamId);
            }
            return View(_mapper.Map<PlayerViewModel>(player));
        }
    }
}

