using MvcLab1.Models;

namespace MvcLab1.Repositories
{
    public class InMemoryGameRepository : IGameRepository
    {
        private readonly List<Game> _games;
        private int _nextId = 1;

        public InMemoryGameRepository()
        {
            _games = new List<Game>();
            SeedData();
        }

        private void SeedData()
        {
            // Стартовые данные
            Add(new Game
            {
                Title = "Cyberpunk 2077",
                Genre = "RPG",
                Platform = "PC",
                ReleaseYear = 2020,
                Developer = "CD Projekt Red",
                Rating = 8.5,
                Description = "Ролевая игра в открытом мире",
                IsMultiplayer = false,
                Price = 1999,
                CreatedDate = DateTime.Now
            });

            Add(new Game
            {
                Title = "The Witcher 3",
                Genre = "RPG",
                Platform = "PC",
                ReleaseYear = 2015,
                Developer = "CD Projekt Red",
                Rating = 9.5,
                Description = "Эпическая ролевая игра",
                IsMultiplayer = false,
                Price = 1499,
                CreatedDate = DateTime.Now
            });

            Add(new Game
            {
                Title = "God of War",
                Genre = "Action",
                Platform = "PS4",
                ReleaseYear = 2018,
                Developer = "Santa Monica Studio",
                Rating = 9.8,
                Description = "Кратос и его сын Атрей",
                IsMultiplayer = false,
                Price = 2499,
                CreatedDate = DateTime.Now
            });
        }

        public IEnumerable<Game> GetAll()
        {
            return _games;
        }

        public Game? GetById(int id)
        {
            return _games.FirstOrDefault(g => g.Id == id);
        }

        public void Add(Game game)
        {
            game.Id = _nextId++;
            game.CreatedDate = DateTime.Now;
            _games.Add(game);
        }

        public void Update(Game game)
        {
            var existing = GetById(game.Id);
            if (existing != null)
            {
                existing.Title = game.Title;
                existing.Genre = game.Genre;
                existing.Platform = game.Platform;
                existing.ReleaseYear = game.ReleaseYear;
                existing.Developer = game.Developer;
                existing.Rating = game.Rating;
                existing.Description = game.Description;
                existing.IsMultiplayer = game.IsMultiplayer;
                existing.Price = game.Price;
            }
        }

        public void Delete(int id)
        {
            var game = GetById(id);
            if (game != null)
                _games.Remove(game);
        }

        public IEnumerable<Game> GetByPlatform(string platform)
        {
            return _games.Where(g => g.Platform.Equals(platform, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Game> GetByGenre(string genre)
        {
            return _games.Where(g => g.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Game> GetMultiplayer()
        {
            return _games.Where(g => g.IsMultiplayer);
        }
    }
}