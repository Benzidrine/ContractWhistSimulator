using System;
using System.Collections.Generic;
using System.Text;

namespace ContractWhist
{
    public class HandRecord
    {
        public string CardsInHand { get; set; }
        public int Bid { get; set; }
        public int GameWon { get; set; }
        public int TotalValue { get; set; } //Total Sum value of cards in hand
        public int NumberOfTrumpCards { get; set; }
        public int NumberOfAces { get; set; }
        public int NumberOfTwos { get; set; }
    }
}
