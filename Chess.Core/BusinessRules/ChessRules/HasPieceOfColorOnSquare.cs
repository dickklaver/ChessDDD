namespace Chess.Core.BusinessRules.ChessRules
{
    internal class HasPieceOfColorOnSquare : BusinessRule
    {
        private readonly Square square;
        private readonly Player player;
        private readonly Board board;

        public HasPieceOfColorOnSquare(Square square, Player player, Board board)
        {
            this.square = square;
            this.player = player;
            this.board = board;
        }

        public override IEnumerable<BusinessRuleViolation> CheckRule()
        {
            List<BusinessRuleViolation> result = new();
            Maybe<Piece> maybePiece = this.board.GetPieceOn(this.square);
            Piece fromPiece = maybePiece.Value;
            if (this.player != fromPiece.Player)
            {
                result.Add(new BusinessRuleViolation($"no {this.player} piece on {this.square}"));
            }

            return result;
        }
    }
}
