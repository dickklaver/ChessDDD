namespace Chess.Core
{
    public class BoardId : ValueObject
    {
        public Guid Value { get; private set; }

        public BoardId()
        {
            this.Value = Guid.NewGuid();
        }
        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}