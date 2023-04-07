namespace Chess.Core
{
    public abstract class Piece : ValueObject
    {
        protected Piece(Player player, string notationLetter)
        {
            this.Player = player;
            this.NotationLetter = player == Player.White ? notationLetter.ToUpper() : notationLetter.ToLower();
        }

        public string NotationLetter { get; protected set; }

        public Player Player { get; protected set; }

        public override string ToString()
        {
            return this.NotationLetter;
        }

        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return this.Player;
            yield return this.NotationLetter;
        }

        public abstract bool CanMoveTo(Square fromSquare, Square toSquare, Board board);

        public abstract bool Attacks(Square fromSquare, Square toSquare, Board board);
    }
}