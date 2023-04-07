using Chess.Core;

namespace Chess.ApplicationServices
{
    public class PieceOnSquareDto
    {
        private PieceOnSquareDto(PieceOnSquare pieceOnSquare)
        {
            this.NotationLetter = pieceOnSquare.Piece.NotationLetter;
            this.Player = pieceOnSquare.Piece.Player == Core.Player.White ? "White" : "Black";
            this.Square = pieceOnSquare.Square.ToString();
        }

        public string NotationLetter { get; private set; }
        public string Player { get; private set; }

        public string Square { get; private set; }

        internal static PieceOnSquareDto CreateFrom(PieceOnSquare pieceOnSquare)
        {
            return new PieceOnSquareDto(pieceOnSquare);
        }
    }
}