using System.Collections.ObjectModel;

namespace Chess.Core
{
    public class Board : Entity
    {
        protected List<Piece> pieces = new List<Piece>();

        public Board()
        {
            this.BoardId = new BoardId();
            this.IsWhiteToMove = true;
        }

        public BoardId BoardId { get; private set; }

        public bool IsWhiteToMove { get; set; }

        public Player PlayerToMove
        {
            get
            {
                return this.IsWhiteToMove ? Player.White : Player.Black;
            }
        }

        public IReadOnlyCollection<Piece> Pieces
        {
            get
            {
                return new ReadOnlyCollection<Piece>(this.pieces);
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
            return this.pieces.Where(p => p.Square.RankNumber == square.RankNumber && p.Square.FileNumber == square.FileNumber).SingleOrDefault().AsMaybe();
        }

        public void MakeMove(Square fromSquare, Square toSquare)
        {
            // todo return Move within which a string representation of the move
            var maybePiece = this.GetPieceOn(fromSquare);
            if (maybePiece.HasNoValue)
                throw new InvalidMoveException($"no piece on {fromSquare}");

            Piece fromPiece = maybePiece.Value;
            if (fromPiece.Player != this.PlayerToMove)
                throw new InvalidMoveException($"no {this.PlayerToMove} piece on {fromSquare}");

            if (!CanMoveTo(fromPiece, toSquare))
                throw new InvalidMoveException($"Cannot move from {fromSquare} to {toSquare}");


            if (IsCapture(fromPiece, toSquare))
            {
                this.RemovePieceOn(toSquare);
            }

            fromPiece.MoveTo(toSquare/*, this*/);

            this.IsWhiteToMove = !this.IsWhiteToMove;

            return;
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

        private bool CanMoveTo(Piece fromPiece, Square toSquare)
        {
            List<Square> squares = fromPiece.GetSquaresPieceCanTheoreticallyMoveTo();
            Maybe<Piece> maybePiece;
            Piece piece;

            if (!squares.Contains(toSquare))
            {
                if (fromPiece is Pawn)
                {
                    squares = fromPiece.GetSquaresPieceCanTheoreticallyCapture();
                    if (!squares.Contains(toSquare))
                        return false;

                    maybePiece = this.GetPieceOn(toSquare);
                    if (maybePiece.HasNoValue)
                        return false;

                    piece = maybePiece.Value;
                    if (fromPiece.Player == piece.Player)
                        return false;

                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (fromPiece is Knight)
                return true;

            var squaresInbetweenAreEmpty = this.AreSquaresInBetweenEmpty(fromPiece, toSquare);
            if (!squaresInbetweenAreEmpty)
            {
                return false;
            }

            maybePiece = this.GetPieceOn(toSquare);
            if (maybePiece.HasNoValue)
            {
                return true;
            }

            piece = maybePiece.Value;
            if (piece.Player == fromPiece.Player)
            {
                return false;
            }

            return true;
        }

        private bool AreSquaresInBetweenEmpty(Piece fromPiece, Square toSquare)
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
            this.pieces.Remove(piece);
        }

        private void InitializeWhiteBackrank()
        {
            pieces.Add(new King(new Square(5, 1), Player.White));
            pieces.Add(new Queen(new Square(4, 1), Player.White));
            InitializeWhiteRooks();
            InitializeWhiteBishops();
            InitializeWhiteKnights();
        }

        private void InitializeWhitePawns()
        {
            for (int fileNumber = 1; fileNumber <= 8; fileNumber++)
            {
                pieces.Add(new Pawn(new Square(fileNumber, 2), Player.White));
            }
        }

        private void InitializeBlackPawns()
        {
            for (int fileNumber = 1; fileNumber <= 8; fileNumber++)
            {
                pieces.Add(new Pawn(new Square(fileNumber, 7), Player.Black));
            }
        }

        private void InitializeBlackBackrank()
        {
            pieces.Add(new King(new Square(5, 8), Player.Black));
            pieces.Add(new Queen(new Square(4, 8), Player.Black));
            InitializeBlackRooks();
            InitializeBlackBishops();
            InitializeBlackKnights();
        }

        private void InitializeWhiteRooks()
        {
            pieces.Add(new Rook(new Square(1, 1), Player.White));
            pieces.Add(new Rook(new Square(8, 1), Player.White));
        }

        private void InitializeWhiteBishops()
        {
            pieces.Add(new Bishop(new Square(3, 1), Player.White));
            pieces.Add(new Bishop(new Square(6, 1), Player.White));
        }

        private void InitializeWhiteKnights()
        {
            pieces.Add(new Knight(new Square(2, 1), Player.White));
            pieces.Add(new Knight(new Square(7, 1), Player.White));
        }

        private void InitializeBlackRooks()
        {
            pieces.Add(new Rook(new Square(1, 8), Player.Black));
            pieces.Add(new Rook(new Square(8, 8), Player.Black));
        }

        private void InitializeBlackBishops()
        {
            pieces.Add(new Bishop(new Square(3, 8), Player.Black));
            pieces.Add(new Bishop(new Square(6, 8), Player.Black));
        }

        private void InitializeBlackKnights()
        {
            pieces.Add(new Knight(new Square(2, 8), Player.Black));
            pieces.Add(new Knight(new Square(7, 8), Player.Black));
        }
    }
}