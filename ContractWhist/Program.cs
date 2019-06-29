using System;
using System.Collections.Generic;
using System.Linq;

namespace ContractWhist
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser();
        }

        static void Parser()
        {
            Console.WriteLine("Command:");
            String input = Console.ReadLine();
            if (input.ToLower() == "run")
            {
                Console.WriteLine("How many runs:");
                input = Console.ReadLine();
                int NumberOfRuns = 0;
                if (int.TryParse(input, out NumberOfRuns))
                {
                    for (int i = 1; i < NumberOfRuns; i++)
                    {
                        Game();
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input");
                }
            }
            else if (input.ToLower() == "exit" || input.ToLower() == "quit")
            {
                System.Environment.Exit(1);
            }

                Parser();
        }
        
        static void Game()
        {
            List<Player> Players = new List<Player>();
            Players.Add(new Player(1,true));
            Players.Add(new Player(2,false));
            Players[0].Leading = true; //Set Player One to lead trick
            List<Card> deck = Deal.CreateDeck();
            Decision.SetTrumps(deck, 3); //Set Spades as Trump
            Deal.DealToPlayers(Players, deck);
            Decision.DecideBids(Players);
            //Record Player 1 Hand
            HandRecord handRecord = Record.RecordHandInit(Players.FirstOrDefault(x => x.ID == 1));
            //Write out bids
            foreach (Player player in Players)
            {
                Console.WriteLine("Player: " + player.ID + " Bid: " + player.Bid);
            }
            //Write out Hands
            foreach (Player player in Players)
            {
                Console.WriteLine("Player " + player.ID + " Hand:");
                List<Card> OrderedHand = player.Hand.OrderBy(x => x.Value).OrderBy(x => x.suit.ID).ToList();
                foreach (Card card in OrderedHand)
                {
                    Console.WriteLine("Card ID: " + card.ID + " Card Value: " + card.ValueName + " of " + card.suit.SuitName);
                }
                Console.WriteLine("");
            }
            for (int i = 1; i < 11; i++)
            {
                Console.WriteLine("Game " + i);
                Decision.DecideCardToPlay(Players);
                Decision.PlayCard(Players);
            }
            //Work out wins
            foreach (Player player in Players)
            {
                Console.WriteLine("Player Wins: " + player.Wins + " Bid: " + player.Bid);
            }
            //Record if PlayerOne was successful
            if (Players.FirstOrDefault(x => x.ID == 1).Wins == Players.FirstOrDefault(x => x.ID == 1).Bid)
                handRecord.GameWon = 1;
            CSVWriter.WriteHandResult(handRecord);
        }
    }
}
