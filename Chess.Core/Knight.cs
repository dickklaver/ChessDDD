namespace Chess.Core
{
    public class Knight : Piece
    {
        public Knight(Square initialSquare, Player player) : base(initialSquare, player, "N")
        {
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

            return true;
        }

        private List<Square> GetSquaresPieceCanTheoreticallyMoveTo(Board board)
        {
            List<Square> squareList = new List<Square>();

            var currentFile = this.Square.FileNumber;
            var currentRank = this.Square.RankNumber;
            if (currentFile + 1 <= 8 && currentRank + 2 <= 8)
                squareList.Add(new Square(currentFile + 1, currentRank + 2));

            if (currentFile + 2 <= 8 && currentRank + 1 <= 8)
                squareList.Add(new Square(currentFile + 2, currentRank + 1));

            if (currentFile + 2 <= 8 && currentRank - 1 >= 1)
                squareList.Add(new Square(currentFile + 2, currentRank - 1));

            if (currentFile + 1 <= 8 && currentRank - 2 >= 1)
                squareList.Add(new Square(currentFile + 1, currentRank - 2));

            if (currentFile - 1 >= 1 && currentRank - 2 >= 1)
                squareList.Add(new Square(currentFile - 1, currentRank - 2));

            if (currentFile - 2 >= 1 && currentRank - 1 >= 1)
                squareList.Add(new Square(currentFile - 2, currentRank - 1));

            if (currentFile - 2 >= 1 && currentRank + 1 <= 8)
                squareList.Add(new Square(currentFile - 2, currentRank + 1));

            if (currentFile - 1 >= 1 && currentRank + 2 <= 8)
                squareList.Add(new Square(currentFile - 1, currentRank + 2));

            return squareList;
        }
    }
}