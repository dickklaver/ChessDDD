namespace Chess.Core.Tests
{
    public partial class BoardTests
    {
        public class TestableBoard : Board 
        {
            public void AddPiece(Piece piece)
            {
                this.pieces.Add(piece);
            }
        }
    }


}
