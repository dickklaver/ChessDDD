namespace Chess.Core
{
    public abstract class Player : ValueObject
    {
        public Player(string notationLetter)
        {
            this.NotationLetter = notationLetter;
        }

        public string NotationLetter { get; protected set; }

        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return NotationLetter;
        }

        public override string ToString()
        {
            return this.NotationLetter;
        }

        public static Player White
        {
            get
            {
                return new White();
            }
        }
        public static Player Black
        {
            get
            {
                return new Black();
            }
        }
    }
}