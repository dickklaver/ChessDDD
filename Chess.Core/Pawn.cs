namespace Chess.Core
{
    public class Pawn : Piece
    {
        int rankIncrement;
        public Pawn(Player player) : base(player, "p")
        {
            // white moves up the board, black moves down
            this.rankIncrement = this.Player == Player.White ? 1 : -1;
        }

        public override bool CanMoveTo(Square fromSquare, Square toSquare, Board board)
        {
            List<Square> squares = this.GetSquaresPieceCanTheoreticallyMoveTo(fromSquare, board);
            if (!squares.Contains(toSquare))
            {
                return this.CanCapture(fromSquare, toSquare, board);
            }

            var squaresInbetweenAreEmpty = board.AreSquaresInBetweenEmpty(this, fromSquare, toSquare);
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

        public override bool Attacks(Square fromSquare, Square toSquare, Board board)
        {
            var squares = this.GetSquaresPieceCanTheoreticallyCapture(fromSquare, board);
            if (!squares.Contains(toSquare))
                return false;

            return true;
        }

        private bool CanCapture(Square fromSquare, Square toSquare, Board board)
        {
            var squares = this.GetSquaresPieceCanTheoreticallyCapture(fromSquare, board);
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

        private List<Square> GetSquaresPieceCanTheoreticallyCapture(Square fromSquare, Board board)
        {
            List<Square> squareList = new List<Square>();

            if (fromSquare.FileNumber > 1)
            {
                squareList.Add(new Square(fromSquare.FileNumber - 1, fromSquare.RankNumber + this.rankIncrement));
            }

            if (fromSquare.FileNumber < 8)
            {
                squareList.Add(new Square(fromSquare.FileNumber + 1, fromSquare.RankNumber + this.rankIncrement));
            }

            return squareList;
        }

        private List<Square> GetSquaresPieceCanTheoreticallyMoveTo(Square fromSquare, Board board)
        {
            List<Square> squareList = new List<Square>();
            if (!board.HasPieceOnSquareMoved(fromSquare))
            {
                squareList.Add(new Square(fromSquare.FileNumber, fromSquare.RankNumber + 2 * this.rankIncrement));
            }

            squareList.Add(new Square(fromSquare.FileNumber, fromSquare.RankNumber + this.rankIncrement));

            return squareList;
        }        
    }
}