namespace Chess.Core
{
    public class Pawn : Piece
    {
        int rankIncrement;
        public Pawn(Square initialSquare, Player player) : base(initialSquare, player, "p")
        {
            // white moves up the board, black moves down
            this.rankIncrement = this.Player == Player.White ? 1 : -1;
        }

        public override List<Square> GetSquaresPieceCanTheoreticallyCapture()
        {
            List<Square> squareList = new List<Square>();

            if (this.Square.FileNumber > 1)
            {
                squareList.Add(new Square(this.Square.FileNumber - 1, this.Square.RankNumber + this.rankIncrement));
            }

            if (this.Square.FileNumber < 8)
            {
                squareList.Add(new Square(this.Square.FileNumber + 1, this.Square.RankNumber + this.rankIncrement));
            }

            return squareList;
        }

        public override List<Square> GetSquaresPieceCanTheoreticallyMoveTo()
        {
            List<Square> squareList = new List<Square>();
            if (!this.HasMoved)
            {
                squareList.Add(new Square(this.Square.FileNumber, this.Square.RankNumber + 2 * this.rankIncrement));
            }

            squareList.Add(new Square(this.Square.FileNumber, this.Square.RankNumber + this.rankIncrement));

            return squareList;
        }
    }
}