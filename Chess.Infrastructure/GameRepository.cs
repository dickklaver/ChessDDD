using Chess.ApplicationServices;
using Chess.Core;

namespace Chess.Infrastructure
{
    public class GameRepository : IGameRepository
    {

        public GameRepository()
        {

        }

        public List<GameDto> GetGameList()
        {
            //TODO
            Console.WriteLine("Reading game from database");
            return null;
        }

        public GameDto InstantiateGameFromDatabase(Guid key)
        {
            //TODO
            Console.WriteLine("Reading game from database");
            return null;
        }

        public void SaveGame(Game game)
        {
            //TODO
            Console.WriteLine("Saving game to database");
        }
    }
}