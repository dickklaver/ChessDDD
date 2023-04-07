namespace Chess.Core.Tests
{
    public partial class BoardTests
    {
        public class TestableBoard : Board 
        {
            public void AddPiece(PieceOnSquare pieceOnSquare)
            {
                this.AddPieceToBoard(pieceOnSquare, false);
            }
        }
    }
}
