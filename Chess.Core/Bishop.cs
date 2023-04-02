namespace Chess.Core
{
    public class Bishop : Piece
    {
        List<MoveStrategy> moveStrategies = new List<MoveStrategy>();

        public Bishop(Square initialSquare, Player player) : base(initialSquare, player, "B")
        {
            this.moveStrategies.Add(new DiagonallMoveStrategy(8));
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