namespace Chess.Core
{
    public class Knight : Piece
    {
        public Knight(Square initialSquare, Player player) : base(initialSquare, player, "N")
        {
        }

        public override List<Square> GetSquaresPieceCanTheoreticallyCapture()
        {
            return this.GetSquaresPieceCanTheoreticallyMoveTo();
        }

        public override List<Square> GetSquaresPieceCanTheoreticallyMoveTo()
        {
            List<Square> squareList = new List<Square>();

            var currentFile = this.Square.FileNumber;
            var currentRank = this.Square.RankNumber;
            //NNE
            if (currentFile + 1 <= 8 && currentRank + 2 <= 8)
                squareList.Add(new Square(currentFile + 1, currentRank + 2));

            //ENE
            if (currentFile + 2 <= 8 && currentRank + 1 <= 8)
                squareList.Add(new Square(currentFile + 2, currentRank + 1));

            //ESE
            if (currentFile + 2 <= 8 && currentRank - 1 >= 1)
                squareList.Add(new Square(currentFile + 2, currentRank - 1));

            //SSE
            if (currentFile + 1 <= 8 && currentRank - 2 >= 1)
                squareList.Add(new Square(currentFile + 1, currentRank - 2));

            //SSW
            if (currentFile - 1 >= 1 && currentRank - 2 >= 1)
                squareList.Add(new Square(currentFile - 1, currentRank - 2));

            //WSW
            if (currentFile - 2 >= 1 && currentRank - 1 >= 1)
                squareList.Add(new Square(currentFile - 2, currentRank - 1));

            //WNW
            if (currentFile - 2 >= 1 && currentRank + 1 <= 8)
                squareList.Add(new Square(currentFile - 2, currentRank + 1));

            //NNW
            if (currentFile - 1 >= 1 && currentRank + 2 <= 8)
                squareList.Add(new Square(currentFile - 1, currentRank + 2));

            return squareList;
        }
    }
}