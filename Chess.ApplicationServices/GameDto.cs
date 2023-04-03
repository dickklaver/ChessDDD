using Chess.Core;

namespace Chess.ApplicationServices
{
    public class GameDto
    {
        private GameDto(Game game)
        {
            this.Id = game.Id;
            this.Board = BoardDto.CreateFrom(game.Board);
        }

        public Guid Id { get; private set; }

        public BoardDto Board { get; private set; }

        public GameDto CreateFrom(Game game)
        {
            return new GameDto(game);
        }
    }
}