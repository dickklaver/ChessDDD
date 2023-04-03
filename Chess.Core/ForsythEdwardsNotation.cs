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
            var maybeKing = board.GetPieceOn(new Square("e1"));
            var maybeQueensideRook = board.GetPieceOn(new Square("a1"));
            var maybeKingsideRook = board.GetPieceOn(new Square("h1"));
            var whiteCanKingsideCastle = maybeKing.HasValue && !maybeKing.Value.HasMoved && maybeKingsideRook.HasValue && !maybeKingsideRook.Value.HasMoved;
            var whiteCanQueensideCastle = maybeKing.HasValue && !maybeKing.Value.HasMoved && maybeQueensideRook.HasValue && !maybeQueensideRook.Value.HasMoved;

            maybeKing = board.GetPieceOn(new Square("e8"));
            maybeQueensideRook = board.GetPieceOn(new Square("a8"));
            maybeKingsideRook = board.GetPieceOn(new Square("h8"));
            var blackCanKingsideCastle = maybeKing.HasValue && !maybeKing.Value.HasMoved && maybeKingsideRook.HasValue && !maybeKingsideRook.Value.HasMoved;
            var blackCanQueensideCastle = maybeKing.HasValue && !maybeKing.Value.HasMoved && maybeQueensideRook.HasValue && !maybeQueensideRook.Value.HasMoved;

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