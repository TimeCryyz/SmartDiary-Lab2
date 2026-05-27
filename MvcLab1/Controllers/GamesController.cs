using Microsoft.AspNetCore.Mvc;
using MvcLab1.Models;
using MvcLab1.Repositories;

namespace MvcLab1.Controllers
{
    public class GamesController : Controller
    {
        private readonly IGameRepository _repository;

        public GamesController(IGameRepository repository)
        {
            _repository = repository;
        }

        // GET: /Games
        public IActionResult Index()
        {
            var games = _repository.GetAll();
            return View(games);
        }

        // GET: /Games/Details/5
        public IActionResult Details(int id)
        {
            var game = _repository.GetById(id);
            if (game == null)
                return NotFound();
            return View(game);
        }

        // GET: /Games/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Games/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Game game)
        {
            if (ModelState.IsValid)
            {
                _repository.Add(game);
                TempData["SuccessMessage"] = "Игра успешно добавлена!";
                return RedirectToAction(nameof(Index));
            }
            return View(game);
        }

        // GET: /Games/Edit/5
        public IActionResult Edit(int id)
        {
            var game = _repository.GetById(id);
            if (game == null)
                return NotFound();
            return View(game);
        }

        // POST: /Games/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Game game)
        {
            if (id != game.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                _repository.Update(game);
                TempData["SuccessMessage"] = "Игра успешно обновлена!";
                return RedirectToAction(nameof(Index));
            }
            return View(game);
        }

        // GET: /Games/Delete/5
        public IActionResult Delete(int id)
        {
            var game = _repository.GetById(id);
            if (game == null)
                return NotFound();
            return View(game);
        }

        // POST: /Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _repository.Delete(id);
            TempData["SuccessMessage"] = "Игра удалена!";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Games/Platform/PC
        public IActionResult Platform(string platform)
        {
            var games = _repository.GetByPlatform(platform);
            ViewBag.Platform = platform;
            return View(games);
        }

        // GET: /Games/Genre/RPG
        public IActionResult Genre(string genre)
        {
            var games = _repository.GetByGenre(genre);
            ViewBag.Genre = genre;
            return View(games);
        }

        // GET: /Games/Multiplayer
        public IActionResult Multiplayer()
        {
            var games = _repository.GetMultiplayer();
            return View("Index", games);
        }
    }
}