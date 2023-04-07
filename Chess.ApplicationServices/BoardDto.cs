using Chess.Core;

namespace Chess.ApplicationServices
{
    public class BoardDto
    {
        private BoardDto(Board board)
        {
            this.Id = board.Id;
            this.IsWhiteToMove = board.IsWhiteToMove;
            this.PiecesOnSquares = new List<PieceOnSquareDto>();
            foreach (var pieceOnSquare in board.PiecesOnSquares)
            {
                this.PiecesOnSquares.Add(PieceOnSquareDto.CreateFrom(pieceOnSquare));
            }
        }

        public Guid Id { get; private set; }
        public bool IsWhiteToMove { get; private set; }
        public List<PieceOnSquareDto> PiecesOnSquares { get; private set; }

        internal static BoardDto CreateFrom(Board board)
        {
            return new BoardDto(board);
        }
    }
}