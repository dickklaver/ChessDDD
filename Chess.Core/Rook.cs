namespace Chess.Core
{
    public class Rook : Piece
    {
        List<MoveStrategy> moveStrategies = new List<MoveStrategy>();

        public Rook(Square initialSquare, Player player) : base(initialSquare, player, "R")
        {
            this.moveStrategies.Add(new VerticalAndHorizontalMoveStrategy(8));
        }

        public override bool Attacks(Square toSquare, Board board)
        {
            if (!this.CanMoveTo(toSquare, board))
            {
                return false;
            }

            return true;
        }

        public override bool CanMoveTo(Square toSquare, Board board)
        {
            List<Square> squares = this.GetSquaresPieceCanTheoreticallyMoveTo(board);
            if (!squares.Contains(toSquare))
            {
                return false;
            }

            var squaresInbetweenAreEmpty = board.AreSquaresInBetweenEmpty(this, toSquare);
            if (!squaresInbetweenAreEmpty)
            {
                return false;
            }

            var maybePiece = board.GetPieceOn(toSquare);
            if (maybePiece.HasNoValue)
            {
                return true;
            }

            var piece = maybePiece.Value;
            if (piece.Player == this.Player)
            {
                return false;
            }

            return true;
        }

        private List<Square> GetSquaresPieceCanTheoreticallyMoveTo(Board board)
        {
            List<Square> squareList = new List<Square>();

            foreach (var moveStrategy in this.moveStrategies)
            {
                squareList.AddRange(moveStrategy.GetSquaresPieceCanTheoreticallyMoveTo(this.Square));
            }

            return squareList;
        }
    }
}