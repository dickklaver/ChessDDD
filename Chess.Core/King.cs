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

        public override List<Square> GetSquaresPieceCanTheoreticallyCapture()
        {
            return this.GetSquaresPieceCanTheoreticallyMoveTo();
        }

        public override List<Square> GetSquaresPieceCanTheoreticallyMoveTo()
        {
            List<Square> squareList = new List<Square>();

            foreach (var moveStrategy in this.moveStrategies)
            {
                squareList.AddRange(moveStrategy.GetSquaresPieceCanTheoreticallyMoveTo(this.Square));
            }

            //squareList.Add(new Square(this.Square.FileNumber - 2, this.Square.RankNumber));
            //squareList.Add(new Square(this.Square.FileNumber - 2, this.Square.RankNumber));

            return squareList;
        }
    }
}