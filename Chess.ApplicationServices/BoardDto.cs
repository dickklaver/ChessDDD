using Chess.Core;

namespace Chess.ApplicationServices
{
    public class BoardDto
    {
        private BoardDto(Board board)
        {
            this.Id = board.Id;
            this.IsWhiteToMove = board.IsWhiteToMove;
            this.Pieces = new List<PieceDto>();
            foreach (var piece in board.Pieces)
            {
                this.Pieces.Add(PieceDto.CreateFrom(piece));
            }
        }

        public Guid Id { get; private set; }
        public bool IsWhiteToMove { get; private set; }
        public List<PieceDto> Pieces { get; private set; }

        internal static BoardDto CreateFrom(Board board)
        {
            return new BoardDto(board);
        }
    }
}