namespace Chess.Core
{
    public class ForsythEdwardsNotation : ValueObject
    {
        public ForsythEdwardsNotation(string fenNotation)
        {
            Value = fenNotation;
        }

        public string Value { get; }

        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return Value;
        }

        public static ForsythEdwardsNotation CreateFrom(Board board)
        {
            string result = "";

            // pieces
            for (int rank = 8; rank >= 1; rank--)
            {
                var numberOfEmptySquares = 0;
                for (int file = 1; file <= 8; file++)
                {
                    var maybePiece = board.GetPieceOn(new Square(file, rank));
                    if (maybePiece.HasValue)
                    {
                        if (numberOfEmptySquares > 0)
                        {
                            result += numberOfEmptySquares.ToString();
                        }

                        Piece piece = maybePiece.Value;
                        result += piece.ToString();
                        numberOfEmptySquares = 0;
                    }
                    else
                    {
                        numberOfEmptySquares++;
                    }
                }

                if (numberOfEmptySquares > 0)
                {
                    result += numberOfEmptySquares.ToString();
                }

                if (rank != 1)
                {
                    result += "/";
                }
            }

            // whose move is it?  w/b
            result += board.IsWhiteToMove ? " w" : " b";

            // castling possibilities
            var whiteKingInitialSquare = new Square("e1");
            var maybeKing = board.GetPieceOn(whiteKingInitialSquare);

            var whiteQueensideRookInitialSquare = new Square("a1");
            var maybeQueensideRook = board.GetPieceOn(whiteQueensideRookInitialSquare);

            Square whiteKingsideRookInitialSquare = new Square("h1");
            var maybeKingsideRook = board.GetPieceOn(whiteKingsideRookInitialSquare);

            var whiteCanKingsideCastle = maybeKing.HasValue && !board.HasPieceOnSquareMoved(whiteKingInitialSquare)  && maybeKingsideRook.HasValue && !board.HasPieceOnSquareMoved(whiteKingsideRookInitialSquare);
            var whiteCanQueensideCastle = maybeKing.HasValue && !board.HasPieceOnSquareMoved(whiteKingInitialSquare) && maybeQueensideRook.HasValue && !board.HasPieceOnSquareMoved(whiteQueensideRookInitialSquare);


            Square blackKingInitialSquare = new Square("e8");
            maybeKing = board.GetPieceOn(blackKingInitialSquare);

            Square blackQueensideRookInitialSquare = new Square("a8");
            maybeQueensideRook = board.GetPieceOn(blackQueensideRookInitialSquare);

            Square blackKingsideRookInitialSquare = new Square("h8");
            maybeKingsideRook = board.GetPieceOn(blackKingsideRookInitialSquare);



            var blackCanKingsideCastle = maybeKing.HasValue && !board.HasPieceOnSquareMoved(blackKingInitialSquare) && maybeKingsideRook.HasValue && !board.HasPieceOnSquareMoved(blackKingsideRookInitialSquare);
            var blackCanQueensideCastle = maybeKing.HasValue && !board.HasPieceOnSquareMoved(blackKingInitialSquare) && maybeQueensideRook.HasValue && !board.HasPieceOnSquareMoved(blackQueensideRookInitialSquare);

            if (!whiteCanKingsideCastle && !whiteCanQueensideCastle && !blackCanKingsideCastle && !blackCanQueensideCastle)
            {
                result += " -";
            }
            else
            {
                result += " ";
                if (whiteCanKingsideCastle)
                {
                    result += "K";
                }
                if (whiteCanQueensideCastle)
                {
                    result += "Q";
                }
                if (blackCanKingsideCastle)
                {
                    result += "k";
                }
                if (blackCanQueensideCastle)
                {
                    result += "q";
                }
            }

            // TODO enpassant
            // en passant square. E.g. when last move is e2-e4 then the enpassant field is e3
            // otherwise "-"
            // for now...
            result += " -";

            // TODO number of ply since last pawn move (used for 50-moves rule)
            // for now ...
            result += " 0";

            // TODO number of full moves
            // for now ...
            result += " 1";

            return new ForsythEdwardsNotation(result);
        }
    }
}