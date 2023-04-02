namespace Chess.Core
{
    public class Rook : Piece
    {
        List<MoveStrategy> moveStrategies = new List<MoveStrategy>();

        public Rook(Square initialSquare, Player player) : base(initialSquare, player, "R")
        {
            this.moveStrategies.Add(new VerticalAndHorizontalMoveStrategy(8));
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

            return squareList;
        }
    }
}