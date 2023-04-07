using Chess.Core.BusinessRules;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Chess.Core.Tests.BoardTests;

namespace Chess.Core.Tests
{
    public class MovementTests
    {
        public class PawnMovementTests
        {
            private TestableBoard board;

            public PawnMovementTests()
            {
                board = new TestableBoard();
                // Square initialSquare, Player player
                board.AddPiece(new PieceOnSquare(new Pawn(Player.White), new Square("e2")));
                board.AddPiece(new PieceOnSquare(new Pawn(Player.Black), new Square("a7")));
            }

            [Fact]
            public void CanMove2FromInitialPosition()
            {
                board.MakeMove(new Square("e2"), new Square("e4"));
            }

            [Fact]
            public void CannotMove3FromInitialPosition()
            {
                board.Invoking(y => y.MakeMove(new Square("e2"), new Square("e5")))
                    .Should().Throw<BusinessRuleViolationException>()
                    .Where(x => x.Violations.Contains(new BusinessRuleViolation("Cannot move from e2 to e5")));
            }

            [Fact]
            public void CannotMove2AfterItHasMoved()
            {
                board.MakeMove(new Square("e2"), new Square("e3"));
                board.MakeMove(new Square("a7"), new Square("a6"));
                board.Invoking(y => y.MakeMove(new Square("e3"), new Square("e5")))
                    .Should().Throw<BusinessRuleViolationException>()
                    .Where(x => x.Violations.Contains(new BusinessRuleViolation("Cannot move from e3 to e5")));
            }

            [Fact]
            public void CannotMoveIfTheresAPieceInTheWay()
            {
                board.AddPiece(new PieceOnSquare(new Knight(Player.White), new Square("e3")));
                board.Invoking(y => y.MakeMove(new Square("e2"), new Square("e3")))
                    .Should().Throw<BusinessRuleViolationException>()
                    .Where(x => x.Violations.Contains(new BusinessRuleViolation("Cannot move from e2 to e3")));
            }
        }

        public class KnightMovementTests
        {
            private TestableBoard board;

            public KnightMovementTests()
            {
                board = new TestableBoard();
                // Square initialSquare, Player player
                board.AddPiece(new PieceOnSquare(new Knight(Player.White), new Square("e4")));
            }

            [Fact]
            public void CanMoveUpUpRight()
            {
                board.MakeMove(new Square("e4"), new Square("f6"));
            }

            [Fact]
            public void CanMoveUpRightRight()
            {
                board.MakeMove(new Square("e4"), new Square("g5"));
            }

            [Fact]
            public void CanMoveDownRightRight()
            {
                board.MakeMove(new Square("e4"), new Square("g3"));
            }

            [Fact]
            public void CanMoveDownDownRight()
            {
                board.MakeMove(new Square("e4"), new Square("f2"));
            }

            [Fact]
            public void CanMoveDownDownLeft()
            {
                board.MakeMove(new Square("e4"), new Square("d2"));
            }

            [Fact]
            public void CanMoveDownLeftLeft()
            {
                board.MakeMove(new Square("e4"), new Square("c3"));
            }

            [Fact]
            public void CanMoveUpLeftLeft()
            {
                board.MakeMove(new Square("e4"), new Square("c5"));
            }

            [Fact]
            public void CanMoveUpUpLeft()
            {
                board.MakeMove(new Square("e4"), new Square("d6"));
            }
        }

        public class BishopMovementTests
        {
            private TestableBoard board;

            public BishopMovementTests()
            {
                board = new TestableBoard();
            }

            [Fact]
            public void CanMoveDiagonallyUpRight1()
            {
                board.AddPiece(new PieceOnSquare(new Bishop(Player.White), new Square("a1")));
                board.MakeMove(new Square("a1"), new Square("b2"));
            }

            [Fact]
            public void CanMoveDiagonallyUpRight7()
            {
                board.AddPiece(new PieceOnSquare(new Bishop(Player.White), new Square("a1")));
                board.MakeMove(new Square("a1"), new Square("h8"));
            }

            [Fact]
            public void CanMoveDiagonallyUpLeft1()
            {
                board.AddPiece(new PieceOnSquare(new Bishop(Player.White), new Square("h1")));
                board.MakeMove(new Square("h1"), new Square("g2"));
            }

            [Fact]
            public void CanMoveDiagonallyUpLeft7()
            {
                board.AddPiece(new PieceOnSquare(new Bishop(Player.White), new Square("h1")));
                board.MakeMove(new Square("h1"), new Square("a8"));
            }

            [Fact]
            public void CanMoveDiagonallyDownRight1()
            {
                board.AddPiece(new PieceOnSquare(new Bishop(Player.White), new Square("a8")));
                board.MakeMove(new Square("a8"), new Square("b7"));
            }

            [Fact]
            public void CanMoveDiagonallyDownRight7()
            {
                board.AddPiece(new PieceOnSquare(new Bishop(Player.White), new Square("a8")));
                board.MakeMove(new Square("a8"), new Square("h1"));
            }

            [Fact]
            public void CanMoveDiagonallyDownLeft1()
            {
                board.AddPiece(new PieceOnSquare(new Bishop(Player.White), new Square("h8")));
                board.MakeMove(new Square("h8"), new Square("g7"));
            }

            [Fact]
            public void CanMoveDiagonallyDownLeft7()
            {
                board.AddPiece(new PieceOnSquare(new Bishop(Player.White), new Square("h8")));
                board.MakeMove(new Square("h8"), new Square("a1"));
            }

            [Fact]
            public void CannotMoveIfTheresAPieceInTheWay()
            {
                board.AddPiece(new PieceOnSquare(new Bishop(Player.White), new Square("h8")));
                board.AddPiece(new PieceOnSquare(new Knight(Player.White), new Square("g7")));
                board.Invoking(y => y.MakeMove(new Square("h8"), new Square("g7")))
                    .Should().Throw<BusinessRuleViolationException>()
                    .Where(x => x.Violations.Contains(new BusinessRuleViolation("Cannot move from h8 to g7")));
            }
        }

        public class RookMovementTests
        {
            private TestableBoard board;

            public RookMovementTests()
            {
                board = new TestableBoard();
            }

            [Fact]
            public void CanMoveUp1()
            {
                board.AddPiece(new PieceOnSquare(new Rook(Player.White), new Square("a1")));
                board.MakeMove(new Square("a1"), new Square("a2"));
            }

            [Fact]
            public void CanMoveUp7()
            {
                board.AddPiece(new PieceOnSquare(new Rook(Player.White), new Square("a1")));
                board.MakeMove(new Square("a1"), new Square("a8"));
            }

            [Fact]
            public void CanMoveRight1()
            {
                board.AddPiece(new PieceOnSquare(new Rook(Player.White), new Square("a1")));
                board.MakeMove(new Square("a1"), new Square("b1"));
            }

            [Fact]
            public void CanMoveRight7()
            {
                board.AddPiece(new PieceOnSquare(new Rook(Player.White), new Square("a1")));
                board.MakeMove(new Square("a1"), new Square("h1"));
            }

            [Fact]
            public void CanMoveDown1()
            {
                board.AddPiece(new PieceOnSquare(new Rook(Player.White), new Square("a8")));
                board.MakeMove(new Square("a8"), new Square("a7"));
            }

            [Fact]
            public void CanMoveDown7()
            {
                board.AddPiece(new PieceOnSquare(new Rook(Player.White), new Square("a8")));
                board.MakeMove(new Square("a8"), new Square("a1"));
            }

            [Fact]
            public void CanMoveLeft1()
            {
                board.AddPiece(new PieceOnSquare(new Rook(Player.White), new Square("h8")));
                board.MakeMove(new Square("h8"), new Square("g8"));
            }

            [Fact]
            public void CanMoveLeft7()
            {
                board.AddPiece(new PieceOnSquare(new Rook(Player.White), new Square("h8")));
                board.MakeMove(new Square("h8"), new Square("a8"));
            }

            [Fact]
            public void CannotMoveIfTheresAPieceInTheWay()
            {
                board.AddPiece(new PieceOnSquare(new Rook(Player.White), new Square("h8")));
                board.AddPiece(new PieceOnSquare(new Knight(Player.White), new Square("g8")));
                board.Invoking(y => y.MakeMove(new Square("h8"), new Square("a8")))
                    .Should().Throw<BusinessRuleViolationException>()
                    .Where(x => x.Violations.Contains(new BusinessRuleViolation("Cannot move from h8 to a8")));
            }
        }

        public class QueenMovementTests
        {
            private TestableBoard board;

            public QueenMovementTests()
            {
                board = new TestableBoard();
            }

            [Fact]
            public void CanMoveDiagonallyUpRight1()
            {
                board.AddPiece(new PieceOnSquare(new Queen(Player.White), new Square("a1")));
                board.MakeMove(new Square("a1"), new Square("b2"));
            }

            [Fact]
            public void CanMoveDiagonallyUpRight7()
            {
                board.AddPiece(new PieceOnSquare(new Queen(Player.White), new Square("a1")));
                board.MakeMove(new Square("a1"), new Square("h8"));
            }

            [Fact]
            public void CanMoveDiagonallyUpLeft1()
            {
                board.AddPiece(new PieceOnSquare(new Queen(Player.White), new Square("h1")));
                board.MakeMove(new Square("h1"), new Square("g2"));
            }

            [Fact]
            public void CanMoveDiagonallyUpLeft7()
            {
                board.AddPiece(new PieceOnSquare(new Queen(Player.White), new Square("h1")));
                board.MakeMove(new Square("h1"), new Square("a8"));
            }

            [Fact]
            public void CanMoveDiagonallyDownRight1()
            {
                board.AddPiece(new PieceOnSquare(new Queen(Player.White), new Square("a8")));
                board.MakeMove(new Square("a8"), new Square("b7"));
            }

            [Fact]
            public void CanMoveDiagonallyDownRight7()
            {
                board.AddPiece(new PieceOnSquare(new Queen(Player.White), new Square("a8")));
                board.MakeMove(new Square("a8"), new Square("h1"));
            }

            [Fact]
            public void CanMoveDiagonallyDownLeft1()
            {
                board.AddPiece(new PieceOnSquare(new Queen(Player.White), new Square("h8")));
                board.MakeMove(new Square("h8"), new Square("g7"));
            }

            [Fact]
            public void CanMoveDiagonallyDownLeft7()
            {
                board.AddPiece(new PieceOnSquare(new Queen(Player.White), new Square("h8")));
                board.MakeMove(new Square("h8"), new Square("a1"));
            }

            [Fact]
            public void CanMoveUp1()
            {
                board.AddPiece(new PieceOnSquare(new Queen(Player.White), new Square("a1")));
                board.MakeMove(new Square("a1"), new Square("a2"));
            }

            [Fact]
            public void CanMoveUp7()
            {
                board.AddPiece(new PieceOnSquare(new Queen(Player.White), new Square("a1")));
                board.MakeMove(new Square("a1"), new Square("a8"));
            }

            [Fact]
            public void CanMoveRight1()
            {
                board.AddPiece(new PieceOnSquare(new Queen(Player.White), new Square("a1")));
                board.MakeMove(new Square("a1"), new Square("b1"));
            }

            [Fact]
            public void CanMoveRight7()
            {
                board.AddPiece(new PieceOnSquare(new Queen(Player.White), new Square("a1")));
                board.MakeMove(new Square("a1"), new Square("h1"));
            }

            [Fact]
            public void CanMoveDown1()
            {
                board.AddPiece(new PieceOnSquare(new Queen(Player.White), new Square("a8")));
                board.MakeMove(new Square("a8"), new Square("a7"));
            }

            [Fact]
            public void CanMoveDown7()
            {
                board.AddPiece(new PieceOnSquare(new Queen(Player.White), new Square("a8")));
                board.MakeMove(new Square("a8"), new Square("a1"));
            }

            [Fact]
            public void CanMoveLeft1()
            {
                board.AddPiece(new PieceOnSquare(new Queen(Player.White), new Square("h8")));
                board.MakeMove(new Square("h8"), new Square("g8"));
            }

            [Fact]
            public void CanMoveLeft7()
            {
                board.AddPiece(new PieceOnSquare(new Rook(Player.White), new Square("h8")));
                board.MakeMove(new Square("h8"), new Square("a8"));
            }
        }

        public class KingMovementTests
        {
            private TestableBoard board;

            public KingMovementTests()
            {
                board = new TestableBoard();
            }

            // TODO Add castling tests

            [Fact]
            public void CanMoveDiagonallyUpRight1()
            {
                board.AddPiece(new PieceOnSquare(new King(Player.White), new Square("a1")));
                board.MakeMove(new Square("a1"), new Square("b2"));
            }

            [Fact]
            public void CanMoveDiagonallyUpLeft1()
            {
                board.AddPiece(new PieceOnSquare(new King(Player.White), new Square("h1")));
                board.MakeMove(new Square("h1"), new Square("g2"));
            }

            [Fact]
            public void CanMoveDiagonallyDownRight1()
            {
                board.AddPiece(new PieceOnSquare(new King(Player.White), new Square("a8")));
                board.MakeMove(new Square("a8"), new Square("b7"));
            }

            [Fact]
            public void CanMoveDiagonallyDownLeft1()
            {
                board.AddPiece(new PieceOnSquare(new King(Player.White), new Square("h8")));
                board.MakeMove(new Square("h8"), new Square("g7"));
            }

            [Fact]
            public void CanMoveUp1()
            {
                board.AddPiece(new PieceOnSquare(new King(Player.White), new Square("a1")));
                board.MakeMove(new Square("a1"), new Square("a2"));
            }

            [Fact]
            public void CanMoveRight1()
            {
                board.AddPiece(new PieceOnSquare(new King(Player.White), new Square("a1")));
                board.MakeMove(new Square("a1"), new Square("b1"));
            }

            [Fact]
            public void CanMoveDown1()
            {
                board.AddPiece(new PieceOnSquare(new King(Player.White), new Square("a8")));
                board.MakeMove(new Square("a8"), new Square("a7"));
            }

            [Fact]
            public void CanMoveLeft1()
            {
                board.AddPiece(new PieceOnSquare(new King(Player.White), new Square("h8")));
                board.MakeMove(new Square("h8"), new Square("g8"));
            }
        }
    }
}
