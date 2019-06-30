using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NeuralNetworkLib;

namespace ContractWhist
{
    class Program
    {

        public decimal TotalGames = 0;
        private decimal TotalWins = 0;
        private float RollingAverage = 0;
        private float RollingMax = 0;

        static void Main(string[] args)
        {
            Program prog = new Program();
            prog.Parser();
        }

        void Parser()
        {
            Console.WriteLine("Command:");
            String input = Console.ReadLine();
            if (input.ToLower() == "neural network init") // Neural Network
            {
                Console.WriteLine("Certain:");
                input = Console.ReadLine();

                if (input.ToLower() == "yes") // Neural Network
                {
                    //New Neural Network
                    List<int> NNLayers = new List<int>();
                    NNLayers.Add(52);
                    NNLayers.Add(52);
                    NNLayers.Add(52);
                    NeuralNetwork NewNeuralNetwork = new NeuralNetwork(NNLayers.ToArray());
                    NewNeuralNetwork.Save(NewNeuralNetwork);
                }
            }
            else if (input.ToLower() == "test random") // Neural Network
            {
                TotalWins = 0;
                TotalGames = 0;
                int NumberOfRuns = 20;
                //Existing Neural Network Test
                NeuralNetwork neuralNetwork = new NeuralNetwork();
                neuralNetwork = neuralNetwork.Load();
                Program prog = new Program();
                for (int i = 0; i < NumberOfRuns; i++)
                {
                    List<Card> deck = Deal.CreateDeck();
                    TotalWins += prog.GameNNTrainToWin(neuralNetwork, deck);
                    TotalGames += 10;
                };
                Console.WriteLine("");
                Console.WriteLine("Total Wins: " + TotalWins + " Total Games: " + TotalGames);
                neuralNetwork.fitness = (float)(TotalWins / TotalGames);
            }
            else if (input.ToLower() == "test neural network" || input.ToLower() == "test nn") // Neural Network
            {
                TotalWins = 0;
                TotalGames = 0;
                int NumberOfRuns = 20;
                //Random Neural Network Test
                List<int> NNLayers = new List<int>();
                NNLayers.Add(52);
                NeuralNetwork NewNeuralNetwork = new NeuralNetwork(NNLayers.ToArray());
                Program prog = new Program();
                for (int i = 0; i < NumberOfRuns; i++)
                {
                    List<Card> deck = Deal.CreateDeck();
                    TotalWins += prog.GameNNTrainToWin(NewNeuralNetwork, deck);
                    TotalGames += 10;
                };
                Console.WriteLine("");
                Console.WriteLine("Total Wins: " + TotalWins + " Total Games: " + TotalGames);
            }
            else if (input.ToLower() == "neural network engage" || input.ToLower() == "run nn") // Neural Network
            {
                Console.WriteLine("Number of Generations");
                input = Console.ReadLine();
                int NumberOfGenerations = 0;
                Console.WriteLine("Number of Runs");
                string inputTwo = Console.ReadLine();
                int NumberOfRuns = 0;
                if (int.TryParse(input, out NumberOfGenerations) && int.TryParse(inputTwo, out NumberOfRuns))
                {
                    for (int j = 0; j < NumberOfGenerations; j++)
                    {
                        List<List<Card>> decks = new List<List<Card>>();
                        List<List<Card>> decksTwo = new List<List<Card>>();
                        //Create Deck List for NN to compete with
                        for (int i = 0; i < NumberOfRuns; i++)
                        {
                            List<Card> deck = Deal.CreateDeckNN();
                            decks.Add(deck);
                            List<Card> deckCopy = new List<Card>();
                            foreach (Card card in deck)
                            {
                                deckCopy.Add(card.Clone());
                            }
                            decksTwo.Add(deckCopy);
                        }
                        TotalWins = 0;
                        TotalGames = 0;
                        //New Neural Network
                        List<int> NNLayers = new List<int>();
                        NNLayers.Add(52);
                        NNLayers.Add(52);
                        NNLayers.Add(52);
                        NNLayers.Add(52);
                        NeuralNetwork NewNeuralNetwork = new NeuralNetwork(NNLayers.ToArray());
                        //Existing Neural Network Test
                        NeuralNetwork neuralNetwork = new NeuralNetwork();
                        neuralNetwork = neuralNetwork.Load();
                        CompareNN(neuralNetwork, NewNeuralNetwork, NumberOfRuns, decks, decksTwo);
                        if (NewNeuralNetwork.fitness > neuralNetwork.fitness)
                        {
                            Console.WriteLine("Old Fitness: " + neuralNetwork.fitness + " New Fitness: " + NewNeuralNetwork.fitness);
                            Console.WriteLine("Comparing");
                            NewNeuralNetwork.Save(NewNeuralNetwork);
                            Console.WriteLine("OVERWRITE");
                        }
                    }
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

        public void CompareNN(NeuralNetwork neuralNetwork, NeuralNetwork newNeuralNetwork, int NumberOfRuns, List<List<Card>> decks, List<List<Card>> decksTwo)
        {
            Program prog = new Program();
            for (int i = 0; i < NumberOfRuns; i++)
            {
                TotalWins += prog.GameNNTrainToWin(neuralNetwork, decks[i]);
                TotalGames += 10;
            };
            Console.WriteLine("");
            Console.WriteLine("Total Wins: " + TotalWins + " Total Games: " + TotalGames);
            neuralNetwork.fitness = (float)(TotalWins / TotalGames);

            //Remove Won
            //New Neural Network Test 
            TotalWins = 0;
            TotalGames = 0;
            for (int i = 0; i < NumberOfRuns; i++)
            {
                TotalWins += prog.GameNNTrainToWin(newNeuralNetwork, decksTwo[i]);
                TotalGames += 10;
            };
            Console.WriteLine("");
            Console.WriteLine("Total Wins: " + TotalWins + " Total Games: " + TotalGames);
            newNeuralNetwork.fitness = (float)(TotalWins / TotalGames);
        }

        int GameNNTrainToWin(NeuralNetwork neuralNetwork, List<Card> deck)
        {
            List<Player> Players = new List<Player>();
            Players.Add(new Player(1, true));
            Players.Add(new Player(2, false));
            Players[0].Leading = true; //Set Player One to lead trick
            //Apply Neural Network Weights to Deck
            NNDecideCard.ApplyWeightsToCards(neuralNetwork, deck);
            Decision.SetTrumps(deck, 3); //Set Spades as Trump
            Deal.DealToPlayersNN(Players, deck);
            Decision.DecideBids(Players);
            //Train To Win Set Bid to 10
            Players[0].Bid = 10;
            //Record Player 1 Hand
            HandRecord handRecord = Record.RecordHandInit(Players.FirstOrDefault(x => x.ID == 1));
            //State that player 1 should use Neural Network
            Players.FirstOrDefault(x => x.ID == 1).UseNN = true;
            //Write out bids
            //WriteBids(Players);
            //Write out Hands
            //WriteHands(Players);
            for (int i = 1; i < 11; i++)
            {
                //Console.WriteLine("Game " + i);
                //Set NN player to always lead for better training
                Players.FirstOrDefault(p => p.UseNN == true).Leading = true;
                Decision.DecideCardToPlayNN(Players, neuralNetwork);
                Decision.PlayCardNN(Players, false);
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
                Decision.PlayCard(Players, true);
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
