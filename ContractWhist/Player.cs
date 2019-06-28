using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ContractWhist
{
    public class Player
    {
        public Player (int IDinput)
        {
            ID = IDinput;
            Wins = 0;
        }
        public string HandSeperatedString()
        {
            List<int> CardIDs = this.Hand.OrderBy(x => x.ID).Select(x => x.ID).ToList();
            return string.Join("_", CardIDs);
        }
        public int SumValue()
        {
            List<int> Values = this.Hand.Select(x => x.Value).ToList();
            return Values.Sum();
        }
        public int NumberOfTrumpCards()
        {
            return this.Hand.Where(x => x.suit.IsTrump == true).ToList().Count();
        }
        public int NumberOfValueCard(int ValueToCheck)
        {
            return this.Hand.Where(x => x.Value == ValueToCheck).ToList().Count();
        }
        public int ID { get; set; }
        public int Bid { get; set; }
        public int Wins { get; set; }
        public Card PlayedCard { get; set; } //Card just played in Trick 
        public List<Card> Hand { get; set; }
        public bool Leading { get; set; } //Will play first card in next trick
        public bool SuitedCardFound { get; set; }
    }
}
