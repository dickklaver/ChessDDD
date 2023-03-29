namespace Chess.Core
{
    public class Game : AggregateRoot
    {
        public Game()
        {
            this.Board = new Board();
            this.InitializeBoard();
            this.IsPartijAfgelopen = false;
        }

        public Board Board { get; set; }

        public bool HeeftComputertWit { get; set; }

        public bool IsPartijAfgelopen { get; set; }

        private void InitializeBoard()
        {
            // TODO Initialiseer het board
            this.Board = new Board();
        }

        private void VraagWieWitHeeft()
        {
            //TODO vragen wie wit heeft; zet HeeftComputertWit
        }


        public Move BedenkEenZet()
        {
            //TODO mbv Board de beste zet bedenken
            return new Move("E2", "E4");
        }

        public Move AccepteerEenZet()
        {
            Move result;

            //TODO op basis van gebruikersinvoer een legale zet maken
            var isLegaal = false;
            while (!isLegaal)
            {
                var vanVeldString = VraagVanVeld();
                var naarVeldString = VraagNaarVeld();
                result = new Move(vanVeldString, naarVeldString);
                // TODO tijdelijk true 
                isLegaal = true;
                //TODO checken of zet legaal is
                if (!isLegaal)
                    throw new Exception("Ongeldige zet");

                if (result.IsPromotie())
                {
                    // TODO vraag promotieStuk voorlopig standaar Queen
                    Piece stuk = new Queen();
                    result.SetPromotieStuk(stuk);
                }
            }

            return result;
        }

        private string VraagVanVeld()
        {
            // TODO implementeren
            return "E2";
        }

        private string VraagNaarVeld()
        {
            // TODO implementeren
            return "E4";
        }

        public void Speel(Move zet)
        {
            //TODO zet uitvoeren op het Board
        }

        public void ToonStelling()
        {
            //TODO Toon de stelling mbv Board
        }
    }
}