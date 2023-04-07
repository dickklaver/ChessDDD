namespace Chess.Core
{
    public class Square : ValueObject
    {
        private const string validFiles = "abcdefgh";
        private const string validRanks = "12345678";

        public string RankString { get; private set; }
        public string FileString { get; private set; }

        public Square(string squareString) // A1 - H8
        {
            squareString = squareString.ToLower();

            if (squareString.Length != 2)
                throw new ArgumentException("invalid square");

            var fileString = squareString.Substring(0, 1);
            var isValidFile = validFiles.Contains(fileString);
            if (!isValidFile)
                throw new ArgumentException("invalid file");
            this.FileString = fileString;

            var rankString = squareString.Substring(1, 1);
            var isValidRank = validRanks.Contains(rankString);
            if (!isValidRank)
                throw new ArgumentException("invalid rank");
            this.RankString = rankString;
        }

        public Square(int fileNumber, int rankNumber) // a1 - h8
        {
            if (fileNumber < 1 || fileNumber > 8)
                throw new ArgumentException("invalid filenumber");

            if (rankNumber < 1 || rankNumber > 8)
                throw new ArgumentException("invalid ranknumber");

            
            this.FileString = validFiles.Substring(fileNumber - 1, 1);
            this.RankString = rankNumber.ToString();
        }

        public int FileNumber
        {
            get
            {
                return validFiles.IndexOf(this.FileString) + 1;
            }
        }

        public int RankNumber
        {
            get
            {
                return validRanks.IndexOf(this.RankString) + 1;
            }
        }

        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return this.RankNumber;
            yield return this.FileNumber;
        }

        public override string ToString()
        {
            return this.FileString + this.RankString;
        }
    }
}