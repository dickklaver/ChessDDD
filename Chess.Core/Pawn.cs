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

        public override bool CanMoveTo(Square toSquare, Board board)
        {
            List<Square> squares = this.GetSquaresPieceCanTheoreticallyMoveTo(board);
            if (!squares.Contains(toSquare))
            {
                return this.CanCapture(toSquare, board);
            }

            var squaresInbetweenAreEmpty = board.AreSquaresInBetweenEmpty(this, toSquare);
            if (!squaresInbetweenAreEmpty)
            {
                return false;
            }

            var maybePiece = board.GetPieceOn(toSquare);
            if (maybePiece.HasValue)
            {
                return false;
            }

            return true;
        }

        public override bool Attacks(Square toSquare, Board board)
        {
            var squares = this.GetSquaresPieceCanTheoreticallyCapture(board);
            if (!squares.Contains(toSquare))
                return false;

            return true;
        }

        private bool CanCapture(Square toSquare, Board board)
        {
            var squares = this.GetSquaresPieceCanTheoreticallyCapture(board);
            if (!squares.Contains(toSquare))
                return false;

            var maybePiece = board.GetPieceOn(toSquare);
            if (maybePiece.HasNoValue)
                return false;

            var piece = maybePiece.Value;
            if (this.Player == piece.Player)
                return false;

            return true;
        }

        private List<Square> GetSquaresPieceCanTheoreticallyCapture(Board board)
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

        private List<Square> GetSquaresPieceCanTheoreticallyMoveTo(Board board)
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