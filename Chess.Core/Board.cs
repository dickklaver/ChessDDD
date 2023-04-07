using Chess.Core.BusinessRules;
using Chess.Core.BusinessRules.ChessRules;

using System.Collections.ObjectModel;

namespace Chess.Core
{
    public class Board : Entity
    {
        private readonly List<Square> unmovedSquares = new();
        private readonly Dictionary<Square, PieceOnSquare> piecesOnSquares = new();

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
                    Piece piece = item.Value.Piece;
                    pieces.Add(piece);
                }

                return new ReadOnlyCollection<Piece>(pieces);
            }
        }

        public IReadOnlyCollection<PieceOnSquare> PiecesOnSquares
        {
            get
            {
                var piecesOnSquares = new Collection<PieceOnSquare>();
                foreach (var item in this.piecesOnSquares)
                {
                    var pieceOnSquare = item.Value;
                    piecesOnSquares.Add(pieceOnSquare);
                }

                return new ReadOnlyCollection<PieceOnSquare>(piecesOnSquares);
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

            Piece piece = this.piecesOnSquares[square].Piece;
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

            MoveTo(fromPiece, fromSquare, toSquare);

            Square? sq = this.unmovedSquares.Where(s => s == fromSquare).SingleOrDefault();
            if (sq != null)
            {
                this.unmovedSquares.Remove(sq);
            }

            IsWhiteToMove = !IsWhiteToMove;

            return;
        }

        private void MoveTo(Piece fromPiece, Square fromSquare, Square toSquare)
        {
            if (!this.piecesOnSquares.ContainsKey(fromSquare))
                return;

            this.piecesOnSquares.Remove(fromSquare);
            this.piecesOnSquares.Add(toSquare, new PieceOnSquare(fromPiece, toSquare));
        }

        public ForsythEdwardsNotation ToForsythEdwardsNotation()
        {
            return ForsythEdwardsNotation.CreateFrom(this);
        }

        public bool AreSquaresInBetweenEmpty(Piece fromPiece, Square fromSquare, Square toSquare)
        {
            int rankIncrement, fileIncrement;
            (fileIncrement, rankIncrement) = DetermineDirection(fromSquare, toSquare);

            Maybe<Piece> maybePiece;
            Square currentSquare = new(fromSquare.FileNumber + fileIncrement, fromSquare.RankNumber + rankIncrement);
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

        private (int fileIncrement, int rankIncrement) DetermineDirection(Square fromSquare, Square toSquare)
        {
            int rankIncrement = 0;
            int fileIncrement = 0;

            if (fromSquare.FileNumber == toSquare.FileNumber && fromSquare.RankNumber < toSquare.RankNumber)
            {
                // North
                rankIncrement = 1;
                fileIncrement = 0;
            }
            else if (fromSquare.FileNumber < toSquare.FileNumber && fromSquare.RankNumber < toSquare.RankNumber)
            {
                // North East
                rankIncrement = 1;
                fileIncrement = 1;
            }
            else if (fromSquare.FileNumber < toSquare.FileNumber && fromSquare.RankNumber == toSquare.RankNumber)
            {
                // East
                rankIncrement = 0;
                fileIncrement = 1;
            }
            else if (fromSquare.FileNumber < toSquare.FileNumber && fromSquare.RankNumber > toSquare.RankNumber)
            {
                // South East
                rankIncrement = -1;
                fileIncrement = 1;
            }
            else if (fromSquare.FileNumber == toSquare.FileNumber && fromSquare.RankNumber > toSquare.RankNumber)
            {
                // South
                rankIncrement = -1;
                fileIncrement = 0;
            }
            else if (fromSquare.FileNumber > toSquare.FileNumber && fromSquare.RankNumber > toSquare.RankNumber)
            {
                // South West
                rankIncrement = -1;
                fileIncrement = -1;
            }
            else if (fromSquare.FileNumber > toSquare.FileNumber && fromSquare.RankNumber == toSquare.RankNumber)
            {
                // West
                rankIncrement = 0;
                fileIncrement = -1;
            }
            else if (fromSquare.FileNumber > toSquare.FileNumber && fromSquare.RankNumber < toSquare.RankNumber)
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
            Square? whiteKingSquare = new(5, 1);
            PieceOnSquare pieceOnSquare = new(new King(Player.White), whiteKingSquare);
            AddPieceToBoard(pieceOnSquare, false);

            Square whiteQueenSquare = new(4, 1);
            pieceOnSquare = new PieceOnSquare(new Queen(Player.White), whiteQueenSquare);
            AddPieceToBoard(pieceOnSquare, false);

            InitializeWhiteRooks();
            InitializeWhiteBishops();
            InitializeWhiteKnights();
        }

        protected void AddPieceToBoard(PieceOnSquare pieceOnSquare, bool hasMoved)
        {
            this.piecesOnSquares.Add(pieceOnSquare.Square, pieceOnSquare);
            if (!hasMoved)
            {
                this.unmovedSquares.Add(pieceOnSquare.Square);
            }
        }

        private void InitializeWhitePawns()
        {
            for (int fileNumber = 1; fileNumber <= 8; fileNumber++)
            {
                Square? pawnSquare = new(fileNumber, 2);
                AddPieceToBoard(new PieceOnSquare(new Pawn(Player.White), pawnSquare), false);
            }
        }

        private void InitializeBlackPawns()
        {
            for (int fileNumber = 1; fileNumber <= 8; fileNumber++)
            {
                Square? pawnSquare = new(fileNumber, 7);
                AddPieceToBoard(new PieceOnSquare(new Pawn(Player.Black), pawnSquare), false);
            }
        }

        private void InitializeBlackBackrank()
        {
            Square? blackKingSquare = new(5, 8);
            AddPieceToBoard(new PieceOnSquare(new King(Player.Black), blackKingSquare), false);

            Square? blackQueenSquare = new(4, 8);
            AddPieceToBoard(new PieceOnSquare(new Queen(Player.Black), blackQueenSquare), false);

            InitializeBlackRooks();
            InitializeBlackBishops();
            InitializeBlackKnights();
        }

        private void InitializeWhiteRooks()
        {
            Square initialSquare = new(1, 1);
            AddPieceToBoard(new PieceOnSquare(new Rook(Player.White), initialSquare), false);

            initialSquare = new Square(8, 1);
            AddPieceToBoard(new PieceOnSquare(new Rook(Player.White), initialSquare), false);
        }

        private void InitializeWhiteBishops()
        {
            Square initialSquare = new(3, 1);
            AddPieceToBoard(new PieceOnSquare(new Bishop(Player.White), initialSquare), false);

            initialSquare = new Square(6, 1);
            AddPieceToBoard(new PieceOnSquare(new Bishop(Player.White), initialSquare), false);
        }

        private void InitializeWhiteKnights()
        {
            Square initialSquare = new(2, 1);
            AddPieceToBoard(new PieceOnSquare(new Knight(Player.White), initialSquare), false);

            initialSquare = new Square(7, 1);
            AddPieceToBoard(new PieceOnSquare(new Knight(Player.White), initialSquare), false);
        }

        private void InitializeBlackRooks()
        {
            Square initialSquare = new(1, 8);
            AddPieceToBoard(new PieceOnSquare(new Rook(Player.Black), initialSquare), false);

            initialSquare = new Square(8, 8);
            AddPieceToBoard(new PieceOnSquare(new Rook(Player.Black), initialSquare), false);
        }

        private void InitializeBlackBishops()
        {
            Square initialSquare = new(3, 8);
            AddPieceToBoard(new PieceOnSquare(new Bishop(Player.Black), initialSquare), false);

            initialSquare = new Square(6, 8);
            AddPieceToBoard(new PieceOnSquare(new Bishop(Player.Black), initialSquare), false);
        }

        private void InitializeBlackKnights()
        {
            Square initialSquare = new(2, 8);
            AddPieceToBoard(new PieceOnSquare(new Knight(Player.Black), initialSquare), false);

            initialSquare = new Square(7, 8);
            AddPieceToBoard(new PieceOnSquare(new Knight(Player.Black), initialSquare), false);
        }
    }
}