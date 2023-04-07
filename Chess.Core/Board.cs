using Chess.Core.BusinessRules;
using Chess.Core.BusinessRules.ChessRules;

using System.Collections.ObjectModel;

namespace Chess.Core
{
    public class Board : Entity
    {
        private List<Square> unmovedSquares = new List<Square>();
        private Dictionary<Square, Piece> piecesOnSquares = new Dictionary<Square, Piece>();

        protected List<Piece> __pieces = new List<Piece>();

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
                return new ReadOnlyCollection<Piece>(this.__pieces);
            }
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
            return this.__pieces.Where(p => p.Square.RankNumber == square.RankNumber && p.Square.FileNumber == square.FileNumber).SingleOrDefault().AsMaybe();
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

            fromPiece.MoveTo(toSquare);

            IsWhiteToMove = !IsWhiteToMove;

            return;
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
            this.__pieces.Remove(piece);
        }

        private void InitializeWhiteBackrank()
        {
            var whiteKingSquare = new Square(5, 1);
            this.AddPieceToBoard(new King(whiteKingSquare, Player.White), whiteKingSquare, false);

            Square whiteQueenSquare = new Square(4, 1);
            this.AddPieceToBoard(new Queen(whiteQueenSquare, Player.White), whiteQueenSquare, false);

            InitializeWhiteRooks();
            InitializeWhiteBishops();
            InitializeWhiteKnights();
        }

        private void AddPieceToBoard(Piece piece, Square square, bool hasMoved)
        {
            this.__pieces.Add(piece);
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
                var pawnSquare = new Square(fileNumber, 2);
                this.AddPieceToBoard(new Pawn(pawnSquare, Player.White), pawnSquare, false);
            }
        }

        private void InitializeBlackPawns()
        {
            for (int fileNumber = 1; fileNumber <= 8; fileNumber++)
            {
                var pawnSquare = new Square(fileNumber, 7);
                this.AddPieceToBoard(new Pawn(pawnSquare, Player.Black), pawnSquare, false);
            }
        }

        private void InitializeBlackBackrank()
        {
            var blackKingSquare = new Square(5, 8);
            this.AddPieceToBoard(new King(blackKingSquare, Player.Black), blackKingSquare, false);

            var blackQueenSquare = new Square(4, 8);
            this.AddPieceToBoard(new Queen(blackQueenSquare, Player.Black), blackQueenSquare, false);

            InitializeBlackRooks();
            InitializeBlackBishops();
            InitializeBlackKnights();
        }

        private void InitializeWhiteRooks()
        {
            Square initialSquare = new Square(1, 1);
            this.AddPieceToBoard(new Rook(initialSquare, Player.White), initialSquare, false);

            initialSquare = new Square(8, 1);
            this.AddPieceToBoard(new Rook(initialSquare, Player.White), initialSquare, false);
        }

        private void InitializeWhiteBishops()
        {
            Square initialSquare = new Square(3, 1);
            this.AddPieceToBoard(new Bishop(initialSquare, Player.White), initialSquare, false);

            initialSquare = new Square(6, 1);
            this.AddPieceToBoard(new Bishop(initialSquare, Player.White), initialSquare, false);
        }

        private void InitializeWhiteKnights()
        {
            Square initialSquare = new Square(2, 1);
            this.AddPieceToBoard(new Knight(initialSquare, Player.White), initialSquare, false);

            initialSquare = new Square(7, 1);
            this.AddPieceToBoard(new Knight(initialSquare, Player.White), initialSquare, false);
        }

        private void InitializeBlackRooks()
        {
            Square initialSquare = new Square(1, 8);
            this.AddPieceToBoard(new Rook(initialSquare, Player.Black), initialSquare, false);

            initialSquare = new Square(8, 8);
            this.AddPieceToBoard(new Rook(initialSquare, Player.Black), initialSquare, false);
        }

        private void InitializeBlackBishops()
        {
            Square initialSquare = new Square(3, 8);
            this.AddPieceToBoard(new Bishop(initialSquare, Player.Black), initialSquare, false);

            initialSquare = new Square(6, 8);
            this.AddPieceToBoard(new Bishop(initialSquare, Player.Black), initialSquare, false);
        }

        private void InitializeBlackKnights()
        {
            Square initialSquare = new Square(2, 8);
            this.AddPieceToBoard(new Knight(initialSquare, Player.Black), initialSquare, false);

            initialSquare = new Square(7, 8);
            this.AddPieceToBoard(new Knight(initialSquare, Player.Black), initialSquare, false);
        }
    }
}