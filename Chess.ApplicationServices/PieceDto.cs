using Chess.Core;

namespace Chess.ApplicationServices
{
    public class PieceDto
    {
        private PieceDto(Piece piece)
        {
            this.HasMoved = piece.HasMoved;
            this.NotationLetter = piece.NotationLetter;
            this.Player = piece.Player == Chess.Core.Player.White ? "White" : "Black";
            this.Square = piece.Square.ToString();
        }

        public bool HasMoved { get; private set; }
        public string NotationLetter { get; private set; }
        public string Player { get; private set; }
        public string Square { get; private set; }

        internal static PieceDto CreateFrom(Piece piece)
        {
            return new PieceDto(piece);
        }
    }
}