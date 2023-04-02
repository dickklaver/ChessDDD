namespace Chess.Core
{
    public class DiagonallMoveStrategy : MoveStrategy
    {
        private readonly int maxSteps;

        public DiagonallMoveStrategy(int maxSteps)
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
            // North East
            int steps = 0;
            int rank = fromSquare.RankNumber + 1;
            int file = fromSquare.FileNumber + 1;
            while (rank <= 8 && file <= 8)
            {
                squareList.Add(new Square(file, rank));
                rank = rank + 1;
                file = file + 1;
                if (++steps >= maxSteps)
                {
                    break;
                }
            }

            // South East
            steps = 0;
            rank = fromSquare.RankNumber - 1;
            file = fromSquare.FileNumber + 1;
            while (rank >= 1 && file <= 8)
            {
                squareList.Add(new Square(file, rank));
                rank = rank - 1;
                file = file + 1;
                if (++steps >= maxSteps)
                {
                    break;
                }
            }

            // South West
            steps = 0;
            rank = fromSquare.RankNumber - 1;
            file = fromSquare.FileNumber - 1;
            while (rank >= 1 && file >= 1)
            {
                squareList.Add(new Square(file, rank));
                rank = rank - 1;
                file = file - 1;
                if (++steps >= maxSteps)
                {
                    break;
                }
            }

            // North West
            steps = 0;
            rank = fromSquare.RankNumber + 1;
            file = fromSquare.FileNumber - 1;
            while (rank <= 8 && file >= 1)
            {
                squareList.Add(new Square(file, rank));
                rank = rank + 1;
                file = file - 1;
                if (++steps >= maxSteps)
                {
                    break;
                }
            }

            return squareList;
        }
    }
}