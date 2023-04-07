namespace Chess.Core
{
    public class King : Piece
    {
        List<MoveStrategy> moveStrategies = new List<MoveStrategy>();

        public King(Player player) : base(player, "K")
        {
            this.moveStrategies.Add(new VerticalAndHorizontalMoveStrategy(1));
            this.moveStrategies.Add(new DiagonallMoveStrategy(1));
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

        private List<Square> GetSquaresPieceCanTheoreticallyMoveTo(Square fromSquare, Board board)
        {
            List<Square> squareList = new List<Square>();

            foreach (var moveStrategy in this.moveStrategies)
            {
                squareList.AddRange(moveStrategy.GetSquaresPieceCanTheoreticallyMoveTo(fromSquare));
            }

            //todo: castling
            //squareList.Add(new Square(this.Square.FileNumber - 2, this.Square.RankNumber));
            //squareList.Add(new Square(this.Square.FileNumber - 2, this.Square.RankNumber));

            return squareList;
        }
    }
}