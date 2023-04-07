namespace Chess.Core
{
    public class PieceOnSquare : ValueObject
    {
        
        public PieceOnSquare(Piece piece, Square square)
        {
            Piece = piece;
            Square = square;
        }

        public Piece Piece { get; }
        public Square Square { get; }

        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return Piece;
            yield return Square;
        }
    }
}
