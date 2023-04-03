using Chess.Core;

using CSharpFunctionalExtensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.ApplicationServices
{
    public class GameService
    {
        private readonly IGameRepository repository;
        Game game;

        public GameService(IGameRepository repository)
        {

            this.game = new Game();
            this.repository = repository;
        }

        public BoardDto GetBoard()
        {
            Board board = this.game.Board;
            return BoardDto.CreateFrom(board);
        }

        public Result<bool> MakeMove(string from, string to)
        {
            return this.game.MakeMove(from, to);
        }

        public void SaveGame()
        {
            repository.SaveGame(game);
        }
    }
}
