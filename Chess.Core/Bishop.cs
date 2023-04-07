namespace Chess.Core
{
    public class Bishop : Piece
    {
        List<MoveStrategy> moveStrategies = new List<MoveStrategy>();

        public Bishop(Player player) : base(player, "B")
        {
            this.moveStrategies.Add(new DiagonallMoveStrategy(8));
        }

        public override bool Attacks(Square fromSquare, Square toSquare, Board board)
        {
            if (!this.CanMoveTo(fromSquare, toSquare, board))
            {
                return false;
            }

            return true;
        }

        public override bool CanMoveTo(Square fromSquare, Square toSquare, Board board)
        {
            List<Square> squares = this.GetSquaresPieceCanTheoreticallyMoveTo(fromSquare, board);
            if (!squares.Contains(toSquare))
            {
                return false;
            }

            var squaresInbetweenAreEmpty = board.AreSquaresInBetweenEmpty(this, fromSquare, toSquare);
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

        private List<Square> GetSquaresPieceCanTheoreticallyMoveTo(Square fromSquare, Board board)
        {
            List<Square> squareList = new List<Square>();

            foreach (var moveStrategy in this.moveStrategies)
            {
                squareList.AddRange(moveStrategy.GetSquaresPieceCanTheoreticallyMoveTo(fromSquare));
            }

            return squareList;
        }
    }
}