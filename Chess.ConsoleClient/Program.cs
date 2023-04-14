using Chess.ApplicationServices;
using Chess.Core;
using Chess.Infrastructure;

using CSharpFunctionalExtensions;

using System.Text;
using System.Runtime.InteropServices;


namespace Chess
{
    internal class Program
    {
        static void Main(string[] args)
        {
            InitializDisplay();

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
            Console.Clear();
            showFiles();

            var fieldIsDark = true;
            var originalBackgroundColor = Console.BackgroundColor;

            for (int rank = 8; rank >= 1; rank--)
            {
                Console.BackgroundColor = originalBackgroundColor;
                Console.Write("|" + rank + "|");

                for (int file = 1; file <= 8; file++)
                {
                    fieldIsDark = (rank + file) % 2 == 0;
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
                }

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


            Console.OutputEncoding = Encoding.UTF8;
            Console.Write(NotationLetterToUnicode(pieceOnSquare.NotationLetter.ToString()) + " ");
            Console.ForegroundColor = originalColor;
        }

        private static string NotationLetterToUnicode(string notationLetter)
        {
            notationLetter = notationLetter.ToLower();
            var result = " ";
            switch (notationLetter)
            {
                case "k":
                    return "\u2654";
                case "q":
                    return "\u2655";
                case "r":
                    return "\u2656";
                case "b":
                    return "\u2657";
                case "n":
                    return "\u2658";
                case "p":
                    return "\u2659";
                default:
                    return " ";
            }
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

        private static void InitializDisplay()
        {
            CONSOLE_FONT_INFO_EX ConsoleFontInfo = new CONSOLE_FONT_INFO_EX();
            ConsoleFontInfo.cbSize = (uint)Marshal.SizeOf(ConsoleFontInfo);
            ConsoleFontInfo.FaceName = "MS Gothic";
            ConsoleFontInfo.dwFontSize.X = 32;
            ConsoleFontInfo.dwFontSize.Y = 32;

            SetCurrentConsoleFontEx(GetStdHandle(StdHandle.OutputHandle), false, ref ConsoleFontInfo);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern Int32 SetCurrentConsoleFontEx(
        IntPtr ConsoleOutput,
        bool MaximumWindow,
        ref CONSOLE_FONT_INFO_EX ConsoleCurrentFontEx);

        private enum StdHandle
        {
            OutputHandle = -11
        }

        [DllImport("kernel32")]
        private static extern IntPtr GetStdHandle(StdHandle index);

        private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct COORD
    {
        public short X;
        public short Y;

        public COORD(short X, short Y)
        {
            this.X = X;
            this.Y = Y;
        }
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct CONSOLE_FONT_INFO_EX
    {
        public uint cbSize;
        public uint nFont;
        public COORD dwFontSize;
        public int FontFamily;
        public int FontWeight;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] // Edit sizeconst if the font name is too big
        public string FaceName;
    }
}