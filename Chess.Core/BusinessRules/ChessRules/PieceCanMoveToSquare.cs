namespace Chess.Core.BusinessRules.ChessRules
{
    internal class PieceCanMoveToSquare : BusinessRule
    {
        private readonly Maybe<Piece> maybePiece;
        private readonly Board board;
        private readonly Square toSquare;
        private readonly Square fromSquare;

        public PieceCanMoveToSquare(Maybe<Piece> maybePiece, Square fromSquare, Square toSquare, Board board)
        {
            this.maybePiece = maybePiece;
            this.board = board;
            this.toSquare = toSquare;
            this.fromSquare = fromSquare;
        }
        public override IEnumerable<BusinessRuleViolation> CheckRule()
        {
            if (this.maybePiece.HasNoValue)
            {
                yield return new BusinessRuleViolation($"Cannot move from {this.fromSquare} to {this.toSquare}");
            }

            Piece piece = this.maybePiece.Value;
            if (!piece.CanMoveTo(fromSquare, toSquare, this.board))
            {
                yield return new BusinessRuleViolation($"Cannot move from {this.fromSquare} to {this.toSquare}");
            }

            yield break;
        }
    }
}
