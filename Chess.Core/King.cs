namespace Chess.Core
{
    public class King : Piece
    {
        List<MoveStrategy> moveStrategies = new List<MoveStrategy>();

        public King(Square initialSquare, Player player) : base(initialSquare, player, "K")
        {
            this.moveStrategies.Add(new VerticalAndHorizontalMoveStrategy(1));
            this.moveStrategies.Add(new DiagonallMoveStrategy(1));
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

            var maybeToPiece = board.GetPieceOn(toSquare);
            if (maybeToPiece.HasNoValue)
            {
                return true;
            }

            var toPiece = maybeToPiece.Value;
            if (toPiece.Player == this.Player)
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

            //todo: castling
            //squareList.Add(new Square(this.Square.FileNumber - 2, this.Square.RankNumber));
            //squareList.Add(new Square(this.Square.FileNumber - 2, this.Square.RankNumber));

            return squareList;
        }
    }
}