namespace Chess.Core
{
    public abstract class MoveStrategy
    {
        public abstract List<Square> GetSquaresPieceCanTheoreticallyCapture(Square fromSquare);

        public abstract List<Square> GetSquaresPieceCanTheoreticallyMoveTo(Square fromSquare);
    }
}