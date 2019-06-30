using System;
using System.Collections.Generic;
using System.Linq;
using NeuralNetworkLib;

namespace ContractWhist
{
    class Program
    {

        public decimal TotalGames = 0;
        private decimal TotalWins = 0;

        static void Main(string[] args)
        {
            Program prog = new Program();
            prog.Parser();
        }

        void Parser()
        {
            Console.WriteLine("Command:");
            String input = Console.ReadLine();
            if (input.ToLower() == "neural network engage" || input.ToLower() == "run nn") // Neural Network
            {
                TotalWins = 0;
                TotalGames = 0;
                //New Neural Network
                List<int> NNLayers = new List<int>();
                NNLayers.Add(10);
                NNLayers.Add(10);
                NNLayers.Add(10);
                NeuralNetwork NewNeuralNetwork = new NeuralNetwork(NNLayers.ToArray());
                //Existing Neural Network Test
                NeuralNetwork neuralNetwork = new NeuralNetwork();
                neuralNetwork = neuralNetwork.Load();
                Program prog = new Program();
                for (int i = 0; i < 20; i++)
                {
                    TotalWins += prog.GameNNTrainToWin(neuralNetwork);
                    TotalGames += 10;
                }
                Console.WriteLine("");
                Console.WriteLine("Total Wins: " + TotalWins + " Total Games: " + TotalGames);
                neuralNetwork.fitness = (float)(TotalWins / TotalGames);

                //New Neural Network Test 
                TotalWins = 0;
                TotalGames = 0;
                for (int i = 0; i < 20; i++)
                {
                    TotalWins += prog.GameNNTrainToWin(NewNeuralNetwork);
                    TotalGames += 10;
                }
                Console.WriteLine("");
                Console.WriteLine("Total Wins: " + TotalWins + " Total Games: " + TotalGames);
                NewNeuralNetwork.fitness = (float)(TotalWins / TotalGames);
                if (NewNeuralNetwork.fitness > neuralNetwork.fitness)
                {
                    Console.WriteLine("Old Fitness: " + neuralNetwork.fitness + " New Fitness: " + NewNeuralNetwork.fitness);
                    neuralNetwork.Save(neuralNetwork);
                }
            }
            if (input.ToLower() == "run")
            {
                TotalWins = 0;
                TotalGames = 0;
                Console.WriteLine("How many runs:");
                input = Console.ReadLine();
                int NumberOfRuns = 0;
                if (int.TryParse(input, out NumberOfRuns))
                {
                    for (int i = 0; i < NumberOfRuns; i++)
                    {
                        Program prog = new Program();
                        TotalWins += prog.Game();
                        TotalGames++;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input");
                }

                Console.WriteLine("");
                Console.WriteLine("Win Rate:" + Math.Round(((TotalWins / TotalGames) * 100),2).ToString() + "%");
            }
            else if (input.ToLower() == "exit" || input.ToLower() == "quit")
            {
                System.Environment.Exit(1);
            }

            Parser();
        }

        int GameNNTrainToWin(NeuralNetwork neuralNetwork)
        {
            List<Player> Players = new List<Player>();
            Players.Add(new Player(1, true));
            Players.Add(new Player(2, false));
            Players[0].Leading = true; //Set Player One to lead trick
            List<Card> deck = Deal.CreateDeck();
            Decision.SetTrumps(deck, 3); //Set Spades as Trump
            Deal.DealToPlayers(Players, deck);
            Decision.DecideBids(Players);
            //Train To Win Set Bid to 10
            Players[0].Bid = 10;
            //Record Player 1 Hand
            HandRecord handRecord = Record.RecordHandInit(Players.FirstOrDefault(x => x.ID == 1));
            //State that player 1 should use Neural Network
            Players.FirstOrDefault(x => x.ID == 1).UseNN = true;
            //Apply Neural Network Weights to Player 1 Hand
            NNDecideCard.ApplyWeightsToCards(neuralNetwork, Players.FirstOrDefault(x => x.UseNN).Hand);
            //Write out bids
            WriteBids(Players);
            //Write out Hands
            WriteHands(Players);
            for (int i = 1; i < 11; i++)
            {
                Console.WriteLine("Game " + i);
                Decision.DecideCardToPlayNN(Players, neuralNetwork);
                Decision.PlayCard(Players);
            }
            return Players.FirstOrDefault(x => x.ID == 1).Wins;
        }

        int Game()
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
            WriteBids(Players);
            //Write out Hands
            WriteHands(Players);
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
            {
                handRecord.GameWon = 1;
            }
            CSVWriter.WriteHandResult(handRecord);
            return handRecord.GameWon;
        }

        void WriteBids(List<Player> Players)
        {
            foreach (Player player in Players)
            {
                Console.WriteLine("Player: " + player.ID + " Bid: " + player.Bid);
            }
        }

        void WriteHands(List<Player> Players)
        {
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
        }
    }
}
