﻿namespace Chess.Core
{
    public class Queen : Piece
    {
        List<MoveStrategy> moveStrategies = new List<MoveStrategy>();

        public Queen(Square initialSquare, Player player) : base(initialSquare, player, "Q")
        {
            this.moveStrategies.Add(new VerticalAndHorizontalMoveStrategy(8));
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