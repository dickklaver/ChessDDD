namespace Chess.Core.BusinessRules.ChessRules
{
    internal class HasPieceOnSquare : BusinessRule
    {
        private readonly Square square;
        private readonly Board board;

        public HasPieceOnSquare(Square square, Board board)
        {
            this.square = square;
            this.board = board;
        }
        public override IEnumerable<BusinessRuleViolation> CheckRule()
        {
            List<BusinessRuleViolation>? violations = new List<BusinessRuleViolation>();
            Maybe<Piece> maybePiece = this.board.GetPieceOn(this.square);
            if (maybePiece.HasNoValue)
            {
                violations.Add(new BusinessRuleViolation($"no piece on {this.square}"));
            }

            return violations;
        }
    }
}
