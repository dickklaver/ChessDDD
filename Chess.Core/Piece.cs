namespace Chess.Core
{
    public abstract class Piece : ValueObject
    {
        protected Piece(Square initialSquare, Player player, string notationLetter)
        {
            this.Square = initialSquare;
            this.Player = player;
            this.NotationLetter = player == Player.White ? notationLetter.ToUpper() : notationLetter.ToLower();
            this.HasMoved = false;
        }

        protected bool HasMoved { get; set; }

        public string NotationLetter { get; protected set; }

        public Player Player { get; protected set; }
        
        public Square Square { get; protected set; }

        public override string ToString()
        {
            return this.NotationLetter;
        }

        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return this.Square;
            yield return this.Player;
            yield return this.NotationLetter;
        }

        internal void MoveTo(Square toSquare/*, Board board*/)
        {
            this.Square = toSquare;
            this.HasMoved = true;
        }

        public abstract List<Square> GetSquaresPieceCanTheoreticallyMoveTo();

        public abstract List<Square> GetSquaresPieceCanTheoreticallyCapture();
    }
}