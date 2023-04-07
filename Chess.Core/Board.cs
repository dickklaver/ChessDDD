using Chess.Core.BusinessRules;
using Chess.Core.BusinessRules.ChessRules;

using System.Collections.ObjectModel;

namespace Chess.Core
{
    public class Board : Entity
    {
        private readonly List<Square> unmovedSquares = new List<Square>();
        private readonly Dictionary<Square, Piece> piecesOnSquares = new Dictionary<Square, Piece>();

        public Board()
        {
            IsWhiteToMove = true;
        }

        public bool IsWhiteToMove { get; set; }

        public Player PlayerToMove
        {
            get
            {
                return IsWhiteToMove ? Player.White : Player.Black;
            }
        }

        public IReadOnlyCollection<Piece> __Pieces
        {
            get
            {
                var pieces = new Collection<Piece>();
                foreach (var item in this.piecesOnSquares)
                {
                    Piece piece = item.Value;
                    pieces.Add(piece);
                }

                return new ReadOnlyCollection<Piece>(pieces);
            }
        }

        public bool HasPieceOnSquareMoved(Square square)
        {
            Square? foundSquare = this.unmovedSquares.Where(s => s == square).SingleOrDefault();
            return foundSquare == null;
        }

        public void Initialize()
        {
            InitializeWhiteBackrank();
            InitializeWhitePawns();
            InitializeBlackPawns();
            InitializeBlackBackrank();
        }

        public Maybe<Piece> GetPieceOn(Square square)
        {
            if (!this.piecesOnSquares.ContainsKey(square))
            {
                return Maybe<Piece>.None;
            }

            Piece piece = this.piecesOnSquares[square];
            return piece.AsMaybe();
        }

        public void MakeMove(Square fromSquare, Square toSquare)
        {
            // TODO Add Move within which a string representation of the move to moves collection
            Maybe<Piece> maybeFromPiece = GetPieceOn(fromSquare);

            BusinessRule.ThrowIfNotSatisfied(
                new HasPieceOnSquare(fromSquare, this) &&
                new HasPieceOfColorOnSquare(fromSquare, PlayerToMove, this) &&
                new PieceCanMoveToSquare(maybeFromPiece, fromSquare, toSquare, this));

            Piece fromPiece = maybeFromPiece.Value;

            if (IsCapture(fromPiece, toSquare))
            {
                RemovePieceOn(toSquare);
            }

            MoveTo(fromPiece, toSquare);

            Square? sq = this.unmovedSquares.Where(s => s == fromSquare).SingleOrDefault();
            if (sq != null)
            {
                this.unmovedSquares.Remove(sq);
            }

            IsWhiteToMove = !IsWhiteToMove;

            return;
        }

        private void MoveTo(Piece fromPiece, Square toSquare)
        {
            if (!this.piecesOnSquares.ContainsKey(fromPiece.Square))
                return;

            fromPiece.MoveTo(toSquare);

            this.piecesOnSquares.Remove(fromPiece.Square);
            this.piecesOnSquares.Add(toSquare, fromPiece);
        }

        public ForsythEdwardsNotation ToForsythEdwardsNotation()
        {
            return ForsythEdwardsNotation.CreateFrom(this);
        }

        public bool AreSquaresInBetweenEmpty(Piece fromPiece, Square toSquare)
        {
            int rankIncrement, fileIncrement;
            (fileIncrement, rankIncrement) = DetermineDirection(fromPiece, toSquare);

            Maybe<Piece> maybePiece;
            Square currentSquare = new Square(fromPiece.Square.FileNumber + fileIncrement, fromPiece.Square.RankNumber + rankIncrement);
            while (currentSquare != toSquare)
            {
                maybePiece = GetPieceOn(currentSquare);
                if (maybePiece.HasValue)
                {
                    return false;
                }

                currentSquare = new Square(currentSquare.FileNumber + fileIncrement, currentSquare.RankNumber + rankIncrement);
            }

            return true;
        }

        private bool IsCapture(Piece fromPiece, Square toSquare)
        {
            Maybe<Piece> maybeToPiece = GetPieceOn(toSquare);
            if (maybeToPiece.HasNoValue)
            {
                return false;
            }

            Piece toPiece = maybeToPiece.Value;
            if (fromPiece.Player == toPiece.Player)
            {
                return false;
            }

            return true;
        }

        private (int fileIncrement, int rankIncrement) DetermineDirection(Piece fromPiece, Square toSquare)
        {
            int rankIncrement = 0;
            int fileIncrement = 0;

            if (fromPiece.Square.FileNumber == toSquare.FileNumber && fromPiece.Square.RankNumber < toSquare.RankNumber)
            {
                // North
                rankIncrement = 1;
                fileIncrement = 0;
            }
            else if (fromPiece.Square.FileNumber < toSquare.FileNumber && fromPiece.Square.RankNumber < toSquare.RankNumber)
            {
                // North East
                rankIncrement = 1;
                fileIncrement = 1;
            }
            else if (fromPiece.Square.FileNumber < toSquare.FileNumber && fromPiece.Square.RankNumber == toSquare.RankNumber)
            {
                // East
                rankIncrement = 0;
                fileIncrement = 1;
            }
            else if (fromPiece.Square.FileNumber < toSquare.FileNumber && fromPiece.Square.RankNumber > toSquare.RankNumber)
            {
                // South East
                rankIncrement = -1;
                fileIncrement = 1;
            }
            else if (fromPiece.Square.FileNumber == toSquare.FileNumber && fromPiece.Square.RankNumber > toSquare.RankNumber)
            {
                // South
                rankIncrement = -1;
                fileIncrement = 0;
            }
            else if (fromPiece.Square.FileNumber > toSquare.FileNumber && fromPiece.Square.RankNumber > toSquare.RankNumber)
            {
                // South West
                rankIncrement = -1;
                fileIncrement = -1;
            }
            else if (fromPiece.Square.FileNumber > toSquare.FileNumber && fromPiece.Square.RankNumber == toSquare.RankNumber)
            {
                // West
                rankIncrement = 0;
                fileIncrement = -1;
            }
            else if (fromPiece.Square.FileNumber > toSquare.FileNumber && fromPiece.Square.RankNumber < toSquare.RankNumber)
            {
                // North West
                rankIncrement = 1;
                fileIncrement = -1;
            }

            return (fileIncrement, rankIncrement);
        }

        private void RemovePieceOn(Square square)
        {
            Maybe<Piece> maybePiece = GetPieceOn(square);
            if (maybePiece.HasNoValue)
            {
                return;
            }

            Piece piece = maybePiece.Value;
            this.piecesOnSquares.Remove(square);
        }

        private void InitializeWhiteBackrank()
        {
            Square? whiteKingSquare = new Square(5, 1);
            AddPieceToBoard(new King(whiteKingSquare, Player.White), whiteKingSquare, false);

            Square whiteQueenSquare = new Square(4, 1);
            AddPieceToBoard(new Queen(whiteQueenSquare, Player.White), whiteQueenSquare, false);

            InitializeWhiteRooks();
            InitializeWhiteBishops();
            InitializeWhiteKnights();
        }

        protected void AddPieceToBoard(Piece piece, Square square, bool hasMoved)
        {
            this.piecesOnSquares.Add(square, piece);
            if (!hasMoved)
            {
                this.unmovedSquares.Add(square);
            }
        }

        private void InitializeWhitePawns()
        {
            for (int fileNumber = 1; fileNumber <= 8; fileNumber++)
            {
                Square? pawnSquare = new Square(fileNumber, 2);
                AddPieceToBoard(new Pawn(pawnSquare, Player.White), pawnSquare, false);
            }
        }

        private void InitializeBlackPawns()
        {
            for (int fileNumber = 1; fileNumber <= 8; fileNumber++)
            {
                Square? pawnSquare = new Square(fileNumber, 7);
                AddPieceToBoard(new Pawn(pawnSquare, Player.Black), pawnSquare, false);
            }
        }

        private void InitializeBlackBackrank()
        {
            Square? blackKingSquare = new Square(5, 8);
            AddPieceToBoard(new King(blackKingSquare, Player.Black), blackKingSquare, false);

            Square? blackQueenSquare = new Square(4, 8);
            AddPieceToBoard(new Queen(blackQueenSquare, Player.Black), blackQueenSquare, false);

            InitializeBlackRooks();
            InitializeBlackBishops();
            InitializeBlackKnights();
        }

        private void InitializeWhiteRooks()
        {
            Square initialSquare = new Square(1, 1);
            AddPieceToBoard(new Rook(initialSquare, Player.White), initialSquare, false);

            initialSquare = new Square(8, 1);
            AddPieceToBoard(new Rook(initialSquare, Player.White), initialSquare, false);
        }

        private void InitializeWhiteBishops()
        {
            Square initialSquare = new Square(3, 1);
            AddPieceToBoard(new Bishop(initialSquare, Player.White), initialSquare, false);

            initialSquare = new Square(6, 1);
            AddPieceToBoard(new Bishop(initialSquare, Player.White), initialSquare, false);
        }

        private void InitializeWhiteKnights()
        {
            Square initialSquare = new Square(2, 1);
            AddPieceToBoard(new Knight(initialSquare, Player.White), initialSquare, false);

            initialSquare = new Square(7, 1);
            AddPieceToBoard(new Knight(initialSquare, Player.White), initialSquare, false);
        }

        private void InitializeBlackRooks()
        {
            Square initialSquare = new Square(1, 8);
            AddPieceToBoard(new Rook(initialSquare, Player.Black), initialSquare, false);

            initialSquare = new Square(8, 8);
            AddPieceToBoard(new Rook(initialSquare, Player.Black), initialSquare, false);
        }

        private void InitializeBlackBishops()
        {
            Square initialSquare = new Square(3, 8);
            AddPieceToBoard(new Bishop(initialSquare, Player.Black), initialSquare, false);

            initialSquare = new Square(6, 8);
            AddPieceToBoard(new Bishop(initialSquare, Player.Black), initialSquare, false);
        }

        private void InitializeBlackKnights()
        {
            Square initialSquare = new Square(2, 8);
            AddPieceToBoard(new Knight(initialSquare, Player.Black), initialSquare, false);

            initialSquare = new Square(7, 8);
            AddPieceToBoard(new Knight(initialSquare, Player.Black), initialSquare, false);
        }
    }
}