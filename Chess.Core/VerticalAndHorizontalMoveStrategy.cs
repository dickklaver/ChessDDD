namespace Chess.Core
{
    public class VerticalAndHorizontalMoveStrategy : MoveStrategy
    {
        private readonly int maxSteps;

        public VerticalAndHorizontalMoveStrategy(int maxSteps)
        {
            this.maxSteps = maxSteps;
        }

        public override List<Square> GetSquaresPieceCanTheoreticallyCapture(Square fromSquare)
        {
            return this.GetSquaresPieceCanTheoreticallyMoveTo(fromSquare);
        }

        public override List<Square> GetSquaresPieceCanTheoreticallyMoveTo(Square fromSquare)
        {
            List<Square> squareList = new List<Square>();

            // North
            int steps = 0;
            for (int rank = fromSquare.RankNumber + 1; rank <= 8; rank++)
            {
                squareList.Add(new Square(fromSquare.FileNumber, rank));
                if (++steps >= maxSteps)
                {
                    break;
                }
            }

            // East
            steps = 0;
            for (int file = fromSquare.FileNumber + 1; file <= 8; file++)
            {
                squareList.Add(new Square(file, fromSquare.RankNumber));
                if (++steps >= maxSteps)
                {
                    break;
                }
            }

            // South
            steps = 0;
            for (int rank = fromSquare.RankNumber - 1; rank >= 1; rank--)
            {
                squareList.Add(new Square(fromSquare.FileNumber, rank));
                if (++steps >= maxSteps)
                {
                    break;
                }
            }

            // West
            steps = 0;
            for (int file = fromSquare.FileNumber - 1; file >= 1; file--)
            {
                squareList.Add(new Square(file, fromSquare.RankNumber));
                if (++steps >= maxSteps)
                {
                    break;
                }
            }

            return squareList;
        }
    }
}