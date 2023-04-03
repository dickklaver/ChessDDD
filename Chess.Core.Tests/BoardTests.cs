using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core.Tests
{
    public partial class BoardTests
    {
        private TestableBoard board;

        public BoardTests()
        {
            board = new TestableBoard();
        }

        [Fact]
        public void PiecesCollectionIsReadonly()
        {
            board.InitializeBoard();
            board.Pieces.Should().BeOfType<ReadOnlyCollection<Piece>>();
        }

        [Fact]
        public void WhiteIsTheFirstPlayerToMove()
        {
            board.IsWhiteToMove.Should().BeTrue();
            board.PlayerToMove.Should().Be(Player.White);
        }

        [Fact]
        public void AfterMoveBlackIsThePlayerToMove()
        {
            board.InitializeBoard();
            board.MakeMove(new Square("e2"), new Square("e4"));
            board.IsWhiteToMove.Should().BeFalse();
            board.PlayerToMove.Should().Be(Player.Black);
        }

        [Fact]
        public void CanCallGetPieceOn_OnEmptySquare()
        {
            var result = board.GetPieceOn(new Square("e4"));
            result.HasNoValue.Should().BeTrue();
        }

        [Fact]
        public void CanCallGetPieceOn_OnSquareWithPieceOnIt()
        {
            this.board.AddPiece(new Pawn(new Square("e4"), Player.White));
            var result = board.GetPieceOn(new Square("e4"));
            result.HasValue.Should().BeTrue();
            result.Value.GetType().Should().Be(typeof(Pawn));
        }

        [Fact]
        public void InitializeBoardSetsUpInitialChessPosition()
        {
            this.board.InitializeBoard();

            var result = board.GetPieceOn(new Square("a1")).Value;
            result.GetType().Should().Be(typeof(Rook));
            result.Player.Should().Be(Player.White);

            result = board.GetPieceOn(new Square("b1")).Value;
            result.GetType().Should().Be(typeof(Knight));
            result.Player.Should().Be(Player.White);

            result = board.GetPieceOn(new Square("c1")).Value;
            result.GetType().Should().Be(typeof(Bishop));
            result.Player.Should().Be(Player.White);

            result = board.GetPieceOn(new Square("d1")).Value;
            result.GetType().Should().Be(typeof(Queen));
            result.Player.Should().Be(Player.White);

            result = board.GetPieceOn(new Square("e1")).Value;
            result.GetType().Should().Be(typeof(King));
            result.Player.Should().Be(Player.White);

            result = board.GetPieceOn(new Square("f1")).Value;
            result.GetType().Should().Be(typeof(Bishop));
            result.Player.Should().Be(Player.White);

            result = board.GetPieceOn(new Square("g1")).Value;
            result.GetType().Should().Be(typeof(Knight));
            result.Player.Should().Be(Player.White);

            result = board.GetPieceOn(new Square("h1")).Value;
            result.GetType().Should().Be(typeof(Rook));
            result.Player.Should().Be(Player.White);

            result = board.GetPieceOn(new Square("a8")).Value;
            result.GetType().Should().Be(typeof(Rook));
            result.Player.Should().Be(Player.Black);

            result = board.GetPieceOn(new Square("b8")).Value;
            result.GetType().Should().Be(typeof(Knight));
            result.Player.Should().Be(Player.Black);

            result = board.GetPieceOn(new Square("c8")).Value;
            result.GetType().Should().Be(typeof(Bishop));
            result.Player.Should().Be(Player.Black);

            result = board.GetPieceOn(new Square("d8")).Value;
            result.GetType().Should().Be(typeof(Queen));
            result.Player.Should().Be(Player.Black);

            result = board.GetPieceOn(new Square("e8")).Value;
            result.GetType().Should().Be(typeof(King));
            result.Player.Should().Be(Player.Black);

            result = board.GetPieceOn(new Square("f8")).Value;
            result.GetType().Should().Be(typeof(Bishop));
            result.Player.Should().Be(Player.Black);

            result = board.GetPieceOn(new Square("g8")).Value;
            result.GetType().Should().Be(typeof(Knight));
            result.Player.Should().Be(Player.Black);

            result = board.GetPieceOn(new Square("h8")).Value;
            result.GetType().Should().Be(typeof(Rook));
            result.Player.Should().Be(Player.Black);

            for (int rank = 2; rank <= 7; rank+= 5)
            {
                for (int file = 1; file <= 8; file++)
                {
                    result = board.GetPieceOn(new Square(file, rank)).Value;
                    result.GetType().Should().Be(typeof(Pawn));
                    var expectedPlayer = rank == 2 ? Player.White : Player.Black;
                    result.Player.Should().Be(expectedPlayer);
                }

            }
        }

        [Fact]
        public void InitialBoardFenNotationShouldBeCorrect()
        {
            board.InitializeBoard();
            var actualFenNotation = board.ToForsythEdwardsNotation();
            actualFenNotation.Value.Should().Be("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        }
    }
}
