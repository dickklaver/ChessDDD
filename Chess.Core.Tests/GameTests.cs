namespace Chess.Core.Tests
{
    
    public class GameTests
    {

        [Fact]
        public void InitializeBoardReturnsAInitializedBoard()
        {
            Game game = new Game();
            game.Board.Should().NotBeNull();
        }

        [Fact]
        public void NieuweGameIsNietAfgelopen()
        {
            Game game = new Game();
            game.IsPartijAfgelopen.Should().BeFalse();
        }

        [Fact]
        public void BedenkEenZetReturnsAZet()
        {
            Game game = new Game();
            var zet = game.BedenkEenZet();

            game.Board.Should().NotBeNull();
        }

        [Fact]
        public void AccepteerEenZetReturnsAZet()
        {
            Game game = new Game();
            var zet = game.AccepteerEenZet();

            game.Board.Should().NotBeNull();
        }
    }
}