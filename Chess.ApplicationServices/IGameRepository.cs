using Chess.Core;

namespace Chess.ApplicationServices
{
    public interface IGameRepository
    {
        void SaveGame(Game game);
        GameDto InstantiateGameFromDatabase(Guid key);

        List<GameDto> GetGameList();
    }
}