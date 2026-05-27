using MvcLab1.Models;

namespace MvcLab1.Repositories
{
    public interface IGameRepository
    {
        IEnumerable<Game> GetAll();
        Game? GetById(int id);
        void Add(Game game);
        void Update(Game game);
        void Delete(int id);
        IEnumerable<Game> GetByPlatform(string platform);
        IEnumerable<Game> GetByGenre(string genre);
        IEnumerable<Game> GetMultiplayer();
    }
}