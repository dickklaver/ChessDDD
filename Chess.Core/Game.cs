using Chess.Core.BusinessRules;

namespace Chess.Core
{
    public class Game : AggregateRoot
    {
        //TODO implement: wether or not current player will be in check after move (self-check)
        //TODO implement: board.ToFenNotation()
        //TODO implement: pawn capture en passant
        //TODO implement: pawn promotion
        //TODO implement: king's side castling
        //TODO implement: queen's side castling
        //TODO implement: check detection
        //TODO implement: checkmate detection
        //TODO implement: stalemate detection

        public Game()
        {
            this.Board = new Board();
            this.Board.Initialize();
        }

        public Board Board { get; set; }

        public Result<bool> MakeMove(string from, string to)
        {
            //todo return a Result<Move> within which a string representation of the move... instead of bool
            try
            {
                var fromSquare = new Square(from);
                var toSquare = new Square(to);
                this.Board.MakeMove(fromSquare, toSquare);
                return Result.Success(true);
            }
            catch (BusinessRuleViolationException brve)
            {
                var messages = brve.Violations.Select(v => v.ViolationMessage);
                var overallMessage = messages.Aggregate((partialPhrase, msg) => $"{partialPhrase} {msg}");
                

                return Result.Failure<bool>(overallMessage);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>(ex.Message);
            }
        }

    }
}