namespace Chess.Core.Tests
{

    public class GameTests
    {
        public class PawnTests
        {
            private Game game;

            public PawnTests()
            {
                game = new Game();
            }

            [Fact]
            public void InitializeBoardReturnsAnInitializedBoard()
            {
                game.Board.Should().NotBeNull();
            }

            [Fact]
            public void NewGameHas32PiecesOnBoard()
            {
                game.Board.__Pieces.Count.Should().Be(32);
            }

            [Fact]
            public void PawnsCanMove2FromInitialPosition()
            {
                var result = game.MakeMove("e2", "e4");
                result.Value.Should().BeTrue();
            }

            [Fact]
            public void PawnsCanMove1FromInitialPosition()
            {
                var result = game.MakeMove("e2", "e3");
                result.Value.Should().BeTrue();
            }

            [Fact]
            public void PawnsCannotMove3FromInitialPosition()
            {
                var result = game.MakeMove("e2", "e5");
                result.IsFailure.Should().BeTrue();
                result.Error.Should().Be("Cannot move from e2 to e5");
            }

            [Fact]
            public void PawnsCannotMove2AfterInitialMove()
            {
                _ = game.MakeMove("e2", "e3");
                _ = game.MakeMove("h7", "h6");
                var result = game.MakeMove("e3", "e5");
                result.IsFailure.Should().BeTrue();
                result.Error.Should().Be("Cannot move from e3 to e5");
            }

            [Fact]
            public void WhitePawnsCanCapturePawnToLeft()
            {
                _ = game.MakeMove("e2", "e4");
                _ = game.MakeMove("d7", "d5");
                var result = game.MakeMove("e4", "d5");
                result.IsSuccess.Should().BeTrue();
                var maybePiece = game.Board.GetPieceOn(new Square("d5"));
                maybePiece.HasValue.Should().BeTrue();
                Piece piece = maybePiece.Value;
                piece.Player.Should().Be(Player.White);
            }

            [Fact]
            public void BlackPawnsCanCapturePawnToLeft()
            {
                _ = game.MakeMove("e2", "e4");
                _ = game.MakeMove("d7", "d5");
                _ = game.MakeMove("a2", "a3");
                var result = game.MakeMove("d5", "e4");
                result.IsSuccess.Should().BeTrue();
                var maybePiece = game.Board.GetPieceOn(new Square("e4"));
                maybePiece.HasValue.Should().BeTrue();
                Piece piece = maybePiece.Value;
                piece.Player.Should().Be(Player.Black);
            }

            [Fact]
            public void WhitePawnsCanCapturePawnToRight()
            {
                _ = game.MakeMove("d2", "d4");
                _ = game.MakeMove("e7", "e5");
                var result = game.MakeMove("d4", "e5");
                result.IsSuccess.Should().BeTrue();
            }

            [Fact]
            public void BlackPawnsCanCapturePawnToRight()
            {
                _ = game.MakeMove("d2", "d4");
                _ = game.MakeMove("e7", "e5");
                _ = game.MakeMove("a2", "a3");
                var result = game.MakeMove("e5", "d4");
                result.IsSuccess.Should().BeTrue();
            }

            [Fact]
            public void WhitePawnsCanCapturePieceToLeft()
            {
                _ = game.MakeMove("d2", "d4");
                _ = game.MakeMove("b8", "c6");
                _ = game.MakeMove("d4", "d5");
                _ = game.MakeMove("a7", "a6");
                var result = game.MakeMove("d5", "c6");
                result.IsSuccess.Should().BeTrue();
            }

            [Fact]
            public void BlackPawnsCanCapturePieceToLeft()
            {
                _ = game.MakeMove("g1", "f3");
                _ = game.MakeMove("e7", "e5");
                _ = game.MakeMove("a2", "a3");
                _ = game.MakeMove("e5", "e4");
                _ = game.MakeMove("a3", "a4");
                var result = game.MakeMove("e4", "f3");
                result.IsSuccess.Should().BeTrue();
            }

            [Fact]
            public void WhitePawnsCanCapturePieceToRight()
            {
                _ = game.MakeMove("e2", "e4");
                _ = game.MakeMove("g8", "f6");
                _ = game.MakeMove("e4", "e5");
                _ = game.MakeMove("a7", "a6");
                var result = game.MakeMove("e5", "f6");
                result.IsSuccess.Should().BeTrue();
            }

            [Fact]
            public void BlackPawnsCanCapturePieceToRight()
            {
                _ = game.MakeMove("b1", "c3");
                _ = game.MakeMove("d7", "d5");
                _ = game.MakeMove("a2", "a3");
                _ = game.MakeMove("d5", "d4");
                _ = game.MakeMove("a3", "a4");
                var result = game.MakeMove("d4", "c3");
                result.IsSuccess.Should().BeTrue();
            }

            // TODO pawn takes pawn en passant left
            // TODO pawn takes pawn en passant right
        }

        public class KnightTests
        {

            public class KnightMovementTests
            {
                private Game game;

                public KnightMovementTests()
                {
                    game = new Game();
                }

                [Fact]
                public void KnightShouldBeAbleToMoveInDirectionUpUpLeft()
                {
                    var result = game.MakeMove("g1", "f3");
                    result.IsSuccess.Should().BeTrue();
                }

                [Fact]
                public void KnightShouldBeAbleToMoveInDirectionUpUpRight()
                {
                    _ = game.MakeMove("g1", "f3");
                    _ = game.MakeMove("a7", "a6");
                    var result = game.MakeMove("f3", "g5");
                    result.IsSuccess.Should().BeTrue();
                }

                [Fact]
                public void KnightShouldBeAbleToMoveInDirectionLeftLeftDown()
                {
                    _ = game.MakeMove("g1", "f3");
                    _ = game.MakeMove("a7", "a6");
                    _ = game.MakeMove("f3", "g5");
                    _ = game.MakeMove("a6", "a5");
                    var result = game.MakeMove("g5", "e4");
                    result.IsSuccess.Should().BeTrue();
                }

                [Fact]
                public void KnightShouldBeAbleToMoveInDirectionRightRightDown()
                {
                    _ = game.MakeMove("g1", "f3");
                    _ = game.MakeMove("a7", "a6");
                    _ = game.MakeMove("f3", "g5");
                    _ = game.MakeMove("a6", "a5");
                    _ = game.MakeMove("g5", "e4");
                    _ = game.MakeMove("a5", "a4");
                    var result = game.MakeMove("e4", "g3");
                    result.IsSuccess.Should().BeTrue();
                }

                [Fact]
                public void KnightShouldBeAbleToMoveInDirectionLeftLeftUp()
                {
                    _ = game.MakeMove("g1", "f3");
                    _ = game.MakeMove("a7", "a6");
                    _ = game.MakeMove("f3", "g5");
                    _ = game.MakeMove("a6", "a5");
                    _ = game.MakeMove("g5", "e4");
                    _ = game.MakeMove("a5", "a4");
                    _ = game.MakeMove("e4", "g3");
                    _ = game.MakeMove("a4", "a3");
                    var result = game.MakeMove("g3", "e4");
                    result.IsSuccess.Should().BeTrue();
                }

                [Fact]
                public void KnightShouldBeAbleToMoveInDirectionRightRightUp()
                {
                    _ = game.MakeMove("g1", "f3");
                    _ = game.MakeMove("a7", "a6");
                    _ = game.MakeMove("f3", "g5");
                    _ = game.MakeMove("a6", "a5");
                    _ = game.MakeMove("g5", "e4");
                    _ = game.MakeMove("a5", "a4");
                    _ = game.MakeMove("e4", "g3");
                    _ = game.MakeMove("a4", "a3");
                    _ = game.MakeMove("g3", "e4");
                    _ = game.MakeMove("b7", "b6");
                    var result = game.MakeMove("e4", "g5");
                    result.IsSuccess.Should().BeTrue();
                }

                [Fact]
                public void KnightShouldBeAbleToMoveInDirectionDownDownRight()
                {
                    _ = game.MakeMove("g1", "f3");
                    _ = game.MakeMove("a7", "a6");
                    _ = game.MakeMove("f3", "g5");
                    _ = game.MakeMove("a6", "a5");
                    _ = game.MakeMove("g5", "e4");
                    _ = game.MakeMove("a5", "a4");
                    _ = game.MakeMove("e4", "g3");
                    _ = game.MakeMove("a4", "a3");
                    _ = game.MakeMove("g3", "e4");
                    _ = game.MakeMove("b7", "b6");
                    _ = game.MakeMove("e4", "g5");
                    _ = game.MakeMove("b6", "b5");
                    var result = game.MakeMove("g5", "h3");
                    result.IsSuccess.Should().BeTrue();
                }

                [Fact]
                public void KnightShouldBeAbleToMoveInDirectionDownDownLeft()
                {
                    _ = game.MakeMove("g1", "f3");
                    _ = game.MakeMove("a7", "a6");
                    _ = game.MakeMove("f3", "g5");
                    _ = game.MakeMove("a6", "a5");
                    _ = game.MakeMove("g5", "e4");
                    _ = game.MakeMove("a5", "a4");
                    _ = game.MakeMove("e4", "g3");
                    _ = game.MakeMove("a4", "a3");
                    _ = game.MakeMove("g3", "e4");
                    _ = game.MakeMove("b7", "b6");
                    _ = game.MakeMove("e4", "g5");
                    _ = game.MakeMove("b6", "b5");
                    _ = game.MakeMove("g5", "h3");
                    _ = game.MakeMove("b5", "b4");
                    var result = game.MakeMove("h3", "g1");
                    result.IsSuccess.Should().BeTrue();
                }
            }

            public class KnightCaptureTests
            { 
            }
        }
    }
}