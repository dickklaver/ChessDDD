using System.Collections.ObjectModel;

namespace Chess.Core
{
    public class Board : Entity
    {
        protected List<Piece> __pieces = new List<Piece>();

        public Board()
        {
            this.IsWhiteToMove = true;
        }

        public bool IsWhiteToMove { get; set; }

        public Player PlayerToMove
        {
            get
            {
                return this.IsWhiteToMove ? Player.White : Player.Black;
            }
        }

        public IReadOnlyCollection<Piece> __Pieces
        {
            get
            {
                return new ReadOnlyCollection<Piece>(this.__pieces);
            }
        }

        public void InitializeBoard()
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
            var maybePiece = this.GetPieceOn(fromSquare);
            if (maybePiece.HasNoValue)
                throw new InvalidMoveException($"no piece on {fromSquare}");

            Piece fromPiece = maybePiece.Value;
            if (fromPiece.Player != this.PlayerToMove)
                throw new InvalidMoveException($"no {this.PlayerToMove} piece on {fromSquare}");

            if (!fromPiece.CanMoveTo(toSquare, this))
                throw new InvalidMoveException($"Cannot move from {fromSquare} to {toSquare}");


            if (IsCapture(fromPiece, toSquare))
            {
                this.RemovePieceOn(toSquare);
            }

            fromPiece.MoveTo(toSquare);

            this.IsWhiteToMove = !this.IsWhiteToMove;

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
                maybePiece = this.GetPieceOn(currentSquare);
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
            var maybeToPiece = this.GetPieceOn(toSquare);
            if (maybeToPiece.HasNoValue)
                return false;

            Piece toPiece = maybeToPiece.Value;
            if (fromPiece.Player == toPiece.Player)
                return false;

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
            var maybePiece = this.GetPieceOn(square);
            if (maybePiece.HasNoValue)
                return;

            Piece piece = maybePiece.Value;
            this.__pieces.Remove(piece);
        }

        private void InitializeWhiteBackrank()
        {
            __pieces.Add(new King(new Square(5, 1), Player.White));
            __pieces.Add(new Queen(new Square(4, 1), Player.White));
            InitializeWhiteRooks();
            InitializeWhiteBishops();
            InitializeWhiteKnights();
        }

        private void InitializeWhitePawns()
        {
            for (int fileNumber = 1; fileNumber <= 8; fileNumber++)
            {
                __pieces.Add(new Pawn(new Square(fileNumber, 2), Player.White));
            }
        }

        private void InitializeBlackPawns()
        {
            for (int fileNumber = 1; fileNumber <= 8; fileNumber++)
            {
                __pieces.Add(new Pawn(new Square(fileNumber, 7), Player.Black));
            }
        }

        private void InitializeBlackBackrank()
        {
            __pieces.Add(new King(new Square(5, 8), Player.Black));
            __pieces.Add(new Queen(new Square(4, 8), Player.Black));
            InitializeBlackRooks();
            InitializeBlackBishops();
            InitializeBlackKnights();
        }

        private void InitializeWhiteRooks()
        {
            __pieces.Add(new Rook(new Square(1, 1), Player.White));
            __pieces.Add(new Rook(new Square(8, 1), Player.White));
        }

        private void InitializeWhiteBishops()
        {
            __pieces.Add(new Bishop(new Square(3, 1), Player.White));
            __pieces.Add(new Bishop(new Square(6, 1), Player.White));
        }

        private void InitializeWhiteKnights()
        {
            __pieces.Add(new Knight(new Square(2, 1), Player.White));
            __pieces.Add(new Knight(new Square(7, 1), Player.White));
        }

        private void InitializeBlackRooks()
        {
            __pieces.Add(new Rook(new Square(1, 8), Player.Black));
            __pieces.Add(new Rook(new Square(8, 8), Player.Black));
        }

        private void InitializeBlackBishops()
        {
            __pieces.Add(new Bishop(new Square(3, 8), Player.Black));
            __pieces.Add(new Bishop(new Square(6, 8), Player.Black));
        }

        private void InitializeBlackKnights()
        {
            __pieces.Add(new Knight(new Square(2, 8), Player.Black));
            __pieces.Add(new Knight(new Square(7, 8), Player.Black));
        }
    }
}