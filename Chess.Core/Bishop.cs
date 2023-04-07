namespace Chess.Core
{
    public class Bishop : Piece
    {
        readonly List<MoveStrategy> moveStrategies = new();

        public Bishop(Player player) : base(player, "B")
        {
            this.moveStrategies.Add(new DiagonallMoveStrategy(8));
        }

        public override bool Attacks(Square fromSquare, Square toSquare, Board board)
        {
            return this.CanMoveTo(fromSquare, toSquare, board);
        }

        public override bool CanMoveTo(Square fromSquare, Square toSquare, Board board)
        {
            List<Square> squares = this.GetSquaresPieceCanTheoreticallyMoveTo(fromSquare);
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

            return piece.Player != this.Player;
        }

        private List<Square> GetSquaresPieceCanTheoreticallyMoveTo(Square fromSquare)
        {
            List<Square> squareList = new();

            foreach (var moveStrategy in this.moveStrategies)
            {
                squareList.AddRange(moveStrategy.GetSquaresPieceCanTheoreticallyMoveTo(fromSquare));
            }

            return squareList;
        }
    }
}