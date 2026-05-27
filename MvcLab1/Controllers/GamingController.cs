using Microsoft.AspNetCore.Mvc;

namespace MvcLab1.Controllers
{
    public class GamingController : Controller
    {
        // Действие 1: Games - список игр
        public IActionResult Games()
        {
            // Список игр (название, жанр, рейтинг, год)
            var games = new List<dynamic>
            {
                new { Title = "Cyberpunk 2077", Genre = "RPG", Rating = 8.5, Year = 2020 },
                new { Title = "The Witcher 3", Genre = "RPG", Rating = 9.5, Year = 2015 },
                new { Title = "God of War", Genre = "Action", Rating = 9.8, Year = 2018 },
                new { Title = "Minecraft", Genre = "Sandbox", Rating = 9.0, Year = 2011 },
                new { Title = "GTA V", Genre = "Action", Rating = 9.3, Year = 2013 }
            };

            ViewBag.Games = games;
            ViewData["Title"] = "Каталог игр";
            ViewData["TotalGames"] = games.Count;

            return View();
        }

        // Действие 2: Game - информация об игре по названию
        public IActionResult Game(string gameTitle)
        {
            if (string.IsNullOrEmpty(gameTitle))
            {
                ViewBag.ErrorMessage = "Название игры не указано!";
                return View("Error");
            }

            // База данных игр
            var gamesDb = new Dictionary<string, dynamic>
            {
                ["cyberpunk 2077"] = new
                {
                    Title = "Cyberpunk 2077",
                    Genre = "RPG",
                    Rating = 8.5,
                    Developer = "CD Projekt Red",
                    Year = 2020,
                    Description = "Ролевая игра в открытом мире, действие которой происходит в Найт-Сити",
                    Platforms = new List<string> { "PC", "PS5", "Xbox Series X" }
                },
                ["the witcher 3"] = new
                {
                    Title = "The Witcher 3: Wild Hunt",
                    Genre = "RPG",
                    Rating = 9.5,
                    Developer = "CD Projekt Red",
                    Year = 2015,
                    Description = "Эпическая ролевая игра о ведьмаке Геральте из Ривии",
                    Platforms = new List<string> { "PC", "PS4", "Xbox One", "Switch" }
                },
                ["god of war"] = new
                {
                    Title = "God of War",
                    Genre = "Action",
                    Rating = 9.8,
                    Developer = "Santa Monica Studio",
                    Year = 2018,
                    Description = "Кратос и его сын Атрей отправляются в путешествие",
                    Platforms = new List<string> { "PS4", "PC" }
                },
                ["minecraft"] = new
                {
                    Title = "Minecraft",
                    Genre = "Sandbox",
                    Rating = 9.0,
                    Developer = "Mojang Studios",
                    Year = 2011,
                    Description = "Игра о строительстве и выживании в кубическом мире",
                    Platforms = new List<string> { "PC", "Mobile", "PS4", "Xbox One", "Switch" }
                },
                ["gta v"] = new
                {
                    Title = "Grand Theft Auto V",
                    Genre = "Action",
                    Rating = 9.3,
                    Developer = "Rockstar North",
                    Year = 2013,
                    Description = "Криминальный экшен в открытом мире Лос-Сантоса",
                    Platforms = new List<string> { "PC", "PS4", "PS5", "Xbox One", "Xbox Series X" }
                }
            };

            string key = gameTitle.ToLower().Trim();
            if (gamesDb.ContainsKey(key))
            {
                var game = gamesDb[key];
                ViewBag.Title = game.Title;
                ViewBag.Genre = game.Genre;
                ViewBag.Rating = game.Rating;
                ViewBag.Developer = game.Developer;
                ViewBag.Year = game.Year;
                ViewBag.Description = game.Description;
                ViewBag.Platforms = game.Platforms;
                ViewData["Title"] = $"{game.Title} - информация об игре";
            }
            else
            {
                ViewBag.ErrorMessage = $"Игра '{gameTitle}' не найдена";
                return View("Error");
            }

            return View();
        }

        // Действие 3: Platform - игры на платформе
        public IActionResult Platform(string platform)
        {
            if (string.IsNullOrEmpty(platform))
            {
                ViewBag.ErrorMessage = "Платформа не указана!";
                return View("Error");
            }

            ViewBag.Platform = platform.ToUpper();

            // База данных игр с платформами
            var allGames = new List<dynamic>
            {
                new { Title = "Cyberpunk 2077", Platform = "PC", Genre = "RPG", Rating = 8.5 },
                new { Title = "Cyberpunk 2077", Platform = "PS5", Genre = "RPG", Rating = 8.5 },
                new { Title = "The Witcher 3", Platform = "PC", Genre = "RPG", Rating = 9.5 },
                new { Title = "The Witcher 3", Platform = "PS4", Genre = "RPG", Rating = 9.5 },
                new { Title = "God of War", Platform = "PS4", Genre = "Action", Rating = 9.8 },
                new { Title = "God of War", Platform = "PC", Genre = "Action", Rating = 9.8 },
                new { Title = "Minecraft", Platform = "PC", Genre = "Sandbox", Rating = 9.0 },
                new { Title = "Minecraft", Platform = "Mobile", Genre = "Sandbox", Rating = 9.0 },
                new { Title = "GTA V", Platform = "PC", Genre = "Action", Rating = 9.3 },
                new { Title = "GTA V", Platform = "PS4", Genre = "Action", Rating = 9.3 }
            };

            // Фильтруем игры по платформе
            var platformGames = allGames
                .Where(g => g.Platform.ToLower() == platform.ToLower())
                .Select(g => new { g.Title, g.Genre, g.Rating })
                .Distinct()
                .ToList();

            if (platformGames.Any())
            {
                ViewBag.Games = platformGames;
                ViewData["Title"] = $"Игры на платформе {platform.ToUpper()}";
                ViewBag.GamesCount = platformGames.Count;
            }
            else
            {
                ViewBag.ErrorMessage = $"Игры для платформы '{platform}' не найдены";
                return View("Error");
            }

            return View();
        }

        // Действие для отображения системных требований
        public IActionResult SystemRequirements(string gameTitle)
        {
            if (string.IsNullOrEmpty(gameTitle))
            {
                ViewBag.ErrorMessage = "Название игры не указано!";
                return View("Error");
            }

            ViewBag.GameTitle = gameTitle;

            // Системные требования
            var requirements = new Dictionary<string, dynamic>
            {
                ["cyberpunk 2077"] = new
                {
                    Minimum = new
                    {
                        OS = "Windows 10",
                        CPU = "Intel Core i5-3570K",
                        RAM = "8 GB",
                        GPU = "GTX 780",
                        Storage = "70 GB"
                    },
                    Recommended = new
                    {
                        OS = "Windows 10",
                        CPU = "Intel Core i7-4790",
                        RAM = "12 GB",
                        GPU = "GTX 1060",
                        Storage = "70 GB SSD"
                    }
                },
                ["the witcher 3"] = new
                {
                    Minimum = new
                    {
                        OS = "Windows 7/8/10",
                        CPU = "Intel Core i5-2500K",
                        RAM = "6 GB",
                        GPU = "GTX 660",
                        Storage = "35 GB"
                    },
                    Recommended = new
                    {
                        OS = "Windows 7/8/10",
                        CPU = "Intel Core i7-3770",
                        RAM = "8 GB",
                        GPU = "GTX 770",
                        Storage = "35 GB"
                    }
                }
            };

            string key = gameTitle.ToLower().Trim();
            if (requirements.ContainsKey(key))
            {
                ViewBag.Requirements = requirements[key];
                ViewData["Title"] = $"Системные требования - {gameTitle}";
            }

            return View();
        }

        // Действие для ошибок
        public IActionResult Error()
        {
            return View();
        }
    }
}