using Chess.ApplicationServices;
using Chess.Core;
using Chess.Infrastructure;

using CSharpFunctionalExtensions;

namespace Chess
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GameService gameService = new GameService(new GameRepository());
            Show(gameService.GetBoard());
            var gameHasEnded = false;

            // TODO detect when game is over
            while (!gameHasEnded)
            {
                var from = AcceptSquare("From field (e.g. e2): ");
                var to = AcceptSquare("To   field (e.g. e4): ");
                var moveMadeResult = gameService.MakeMove(from, to);
                if (moveMadeResult.IsFailure)
                {
                    var originalForegroundColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(moveMadeResult.Error);
                    Console.ForegroundColor = originalForegroundColor;
                }

                Show(gameService.GetBoard());
                gameService.SaveGame();
            }

        }

        private static string AcceptSquare(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var result = Console.ReadLine();
                if (result == null)
                    continue;

                result = result.ToLower();
                if (result.Length != 2)
                    continue;

                if (!"abcdefgh".Contains(result.Substring(0, 1)))
                    continue;

                if (!"12345678".Contains(result.Substring(1, 1)))
                    continue;

                return result;
            }
        }

        private static void Show(BoardDto board)
        {
            Console.WriteLine();
            Console.WriteLine();

            showFiles();

            var fieldIsDark = true;
            var originalBackgroundColor = Console.BackgroundColor;

            for (int rank = 8; rank >= 1; rank--)
            {
                Console.BackgroundColor = originalBackgroundColor;
                Console.Write("|" + rank + "|");

                for (int file = 1; file <= 8; file++)
                {
                    Console.BackgroundColor = fieldIsDark ? ConsoleColor.DarkGreen : ConsoleColor.White;
                    Console.BackgroundColor = fieldIsDark ? ConsoleColor.DarkRed : ConsoleColor.DarkYellow;
                    var maybePieceOnSquare = GetPieceOn(board, file, rank);
                    if (maybePieceOnSquare.HasValue)
                    {
                        var pieceOnSquare = maybePieceOnSquare.Value;
                        ShowPiece(pieceOnSquare);
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                    fieldIsDark = !fieldIsDark;
                }
                fieldIsDark = !fieldIsDark;

                Console.BackgroundColor = originalBackgroundColor;
                Console.Write("|" + rank + "|");
                Console.WriteLine();
            }

            showFiles();
            Console.WriteLine("|====================|");
            Console.WriteLine(board.IsWhiteToMove ? "White to move" : "Black to move");
        }

        private static Maybe<PieceOnSquareDto> GetPieceOn(BoardDto board, int file, int rank)
        {
            var fileLetters = "abcdefgh";
            var maybePiece = board.PiecesOnSquares.Where(p => p.Square == fileLetters.Substring(file - 1, 1) + rank.ToString()).SingleOrDefault().AsMaybe();
            return maybePiece;
        }

        private static void ShowPiece(PieceOnSquareDto pieceOnSquare)
        {
            var originalColor = Console.ForegroundColor;
            if (pieceOnSquare.Player == "White")
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Black;
            }

            Console.Write(pieceOnSquare.NotationLetter.ToString() + " ");
            Console.ForegroundColor = originalColor;
        }

        private static void showFiles()
        {
            Console.Write("| |");
            for (int file = 1; file <= 8; file++)
            {
                var sq = new Square(file, 1);
                Console.Write(sq.FileString + " ");
            }
            Console.Write("| |");
            Console.WriteLine();
        }
    }
}