using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContractWhist
{
    public class Deal
    {
        public static List<Card> CreateDeck()
        {
            List<Card> Deck = new List<Card>();
            var numbers = new List<int>(Enumerable.Range(1, 52));
            foreach (int number in numbers)
            {
                Deck.Add(new Card(number));
            }
            Deck.Shuffle();
            return Deck;
        }

        public static void DealToPlayers(List<Player> players, List<Card> Deck)
        {
            int Page = 0;
            foreach (Player player in players)
            {
                player.Hand = Deck.Skip((Page * 10)).Take(10).ToList();
                Page++;
            }
        }
    }
}
