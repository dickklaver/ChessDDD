namespace Chess.Core
{
    internal class Square
    {
        private const string validFiles = "ABCDEFGH";
        private const string validRanks = "12345678";

        private string rankString;
        private string fileString;

        public Square(string squareString) // A1 - H8
        {
            if (squareString.Length != 2)
                throw new ArgumentException("invalid square");


            var fileString = squareString.Substring(0, 1);
            var isValidFile = validFiles.Contains(fileString);
            if (!isValidFile)
                throw new ArgumentException("invalid file");
            this.fileString = fileString;

            var rankString = squareString.Substring(1, 1);
            var isValidRank = validRanks.Contains(rankString);
            if (!isValidRank)
                throw new ArgumentException("invalid rank");
            this.rankString = rankString;
        }

        private int SquareNumber
        {
            get
            {
                return FileNumber + (RankNumber - 1) * 8;
            }
        }

        private int FileNumber
        {
            get
            {
                return validFiles.IndexOf(this.fileString) + 1;
            }
        }

        private int RankNumber
        {
            get
            {
                return validRanks.IndexOf(this.rankString) + 1;
            }
        }
    }
}