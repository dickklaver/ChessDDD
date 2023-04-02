//namespace Chess.Core
//{
//    public class Move : ValueObject
//    {
//        private Square vanVeld;
//        private Square naarVeld;
//        private Piece? promotieStuk;

//        public Move(string vanVeldString, string naarVeldString)
//        {
//            this.vanVeld = new Square(vanVeldString);
//            this.naarVeld = new Square(naarVeldString);
//        }

//        public void SetPromotieStuk(Piece stuk)
//        {
//            this.promotieStuk = stuk;
//        }

//        protected override IEnumerable<IComparable> GetEqualityComponents()
//        {
//            //TODO equality componenten nog maken
//            throw new NotImplementedException();
//        }

//        internal bool IsPromotie()
//        {
//            //TODO bepalen of het een promotie van een pion betreft
//            return false;
//        }
//    }
//}