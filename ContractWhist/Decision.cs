using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NeuralNetworkLib;

namespace ContractWhist
{
    public class Decision
    {
        public static void PlayCard(List<Player> players)
        {
            foreach (Player player in players)
            {
                //play random card
                Random rng = new Random();
                player.Hand.Shuffle();
            }
            Card WinningCard = HighestCard(players);
            //Set all players leading to false
            players.ForEach(x => { x.Leading = false; });
            //Set who won and leads next round
            players.ForEach(p => {
                if (p.PlayedCard.ID == WinningCard.ID)
                {
                    p.Leading = true;
                    p.Wins++;
                }
            });

            int PlayerCount = 1;
            //Write Result
            foreach (Player player in players)
            {
                Console.WriteLine("Player " + PlayerCount + " Played " + player.PlayedCard.ValueName + " of " + player.PlayedCard.suit.SuitName + " " + (player.PlayedCard.Won ? "Winner" : "") + " Card ID: " + player.PlayedCard.ID);
                PlayerCount++;
            }

            //Remove played card 
            foreach (Player player in players)
            {
                player.Hand.Remove(player.PlayedCard);
            }
        }

        public static void DecideBids(List<Player> players)
        {
            foreach(Player player in players)
            {
                if (player.UseML)
                {
                    player.Bid = MLBid.DecideBid(player);
                }
                else
                {
                    Random rng = new Random();
                    int lowerBound = 0 + player.NumberOfValueCard(14); //Lower bound of bids number of aces in hand
                    int upperBound = 10 - player.NumberOfValueCard(2); //Lower bound of bids number of twos in hand
                    player.Bid = rng.Next(lowerBound, upperBound);
                }
            }
        }

        public static void DecideCardToPlayNN(List<Player> players, NeuralNetwork NN)
        {
            //Find lead player
            Player playerOne = players.FirstOrDefault(x => x.Leading == true);
            //If Neural Network Player
            if (playerOne.UseNN)
            {
                playerOne.Hand.OrderByDescending(x => x.NNValue);
                playerOne.PlayedCard = playerOne.Hand[0];
            }
            else
            {
                //Player One plays random card
                playerOne.Hand.Shuffle();
                if (playerOne.Wins == playerOne.Bid)
                {
                    //Go low if want to lose
                    playerOne.Hand = playerOne.Hand.OrderBy(x => x.Value).ToList();
                }
                else if (playerOne.Wins < playerOne.Bid)
                {
                    //If need wins then go high
                    playerOne.Hand = playerOne.Hand.OrderByDescending(x => x.Value).ToList();
                    playerOne.Hand = playerOne.Hand.OrderByDescending(x => x.suit.IsTrump).ToList();
                }
                playerOne.PlayedCard = playerOne.Hand[0];
            }

            //Set players to follow suit if able
            foreach (Player player in players)
            {
                //Set to false to wipe record of last run
                player.SuitedCardFound = false;
                //Set players other than player one to play card
                if (playerOne.ID != player.ID)
                {

                    player.Hand.Shuffle();
                    foreach (Card c in player.Hand)
                    {
                        if (c.suit.ID == playerOne.PlayedCard.suit.ID)
                        {
                            player.SuitedCardFound = true;
                            break;
                        }
                    }
                }
            }

            //Basic AI
            foreach (Player player in players)
            {
                //Set players other than player one to play card
                if (playerOne.ID != player.ID)
                {
                    if (player.SuitedCardFound)
                    {
                        if (player.Wins == player.Bid) //Try to lose 
                        {
                            player.Hand = player.Hand.OrderBy(x => x.Value).ToList();
                            player.PlayedCard = player.Hand.FirstOrDefault(x => x.suit.ID == playerOne.PlayedCard.suit.ID);
                            continue;
                        }
                        //if need to win play high or try to stop other player winning if already lost
                        else if (player.Wins < player.Bid || player.Wins > player.Bid)
                        {
                            player.Hand = player.Hand.OrderByDescending(x => x.Value).ToList();
                            player.PlayedCard = player.Hand.FirstOrDefault(x => x.suit.ID == playerOne.PlayedCard.suit.ID);
                            continue;
                        }
                    }
                    else if (player.NumberOfTrumpCards() > 0 && player.Wins < player.Bid) //Try to win with Trump
                    {
                        player.Hand = player.Hand.OrderBy(x => x.Value).ToList();
                        player.PlayedCard = player.Hand.FirstOrDefault(x => x.suit.IsTrump);
                        continue;
                    }
                    else if (player.Wins == player.Bid && player.NumberOfTrumpCards() != player.Hand.Count()) // Dump High
                    {
                        player.Hand = player.Hand.OrderByDescending(x => x.Value).ToList();
                        player.PlayedCard = player.Hand.FirstOrDefault(x => x.suit.IsTrump == false);
                        continue;
                    }
                    else if (player.Wins < player.Bid) //Dump Low
                    {
                        player.Hand = player.Hand.OrderBy(x => x.Value).ToList();
                        player.PlayedCard = player.Hand[0];
                        continue;
                    }
                    else //Random
                    {
                        player.Hand.Shuffle();
                        player.PlayedCard = player.Hand[0];
                        continue;
                    }
                }
            }
        }

        public static void DecideCardToPlay(List<Player> players)
        {
            //Find lead player
            Player playerOne = players.FirstOrDefault(x => x.Leading == true);
            //Player One plays random card
            playerOne.Hand.Shuffle();
            if (playerOne.Wins == playerOne.Bid)
            {
                //Go low if want to lose
                playerOne.Hand = playerOne.Hand.OrderBy(x => x.Value).ToList(); 
            }
            else if (playerOne.Wins < playerOne.Bid)
            {
                //If need wins then go high
                playerOne.Hand = playerOne.Hand.OrderByDescending(x => x.Value).ToList();
                playerOne.Hand = playerOne.Hand.OrderByDescending(x => x.suit.IsTrump).ToList();
            }
            playerOne.PlayedCard = playerOne.Hand[0];

            //Set players to follow suit if able
            foreach (Player player in players)
            {
                //Set to false to wipe record of last run
                player.SuitedCardFound = false;
                //Set players other than player one to play card
                if (playerOne.ID != player.ID)
                {

                    player.Hand.Shuffle();
                    foreach(Card c in player.Hand)
                    {
                        if (c.suit.ID == playerOne.PlayedCard.suit.ID)
                        {
                            player.SuitedCardFound = true;
                            break;
                        }
                    }
                }
            }

            //Basic AI
            foreach (Player player in players)
            {
                //Set players other than player one to play card
                if (playerOne.ID != player.ID)
                {
                    if (player.SuitedCardFound)
                    {
                        if (player.Wins == player.Bid) //Try to lose 
                        {
                            player.Hand = player.Hand.OrderBy(x => x.Value).ToList();
                            player.PlayedCard = player.Hand.FirstOrDefault(x => x.suit.ID == playerOne.PlayedCard.suit.ID);
                            continue;
                        }
                        //if need to win play high or try to stop other player winning if already lost
                        else if (player.Wins < player.Bid || player.Wins > player.Bid)
                        {
                            player.Hand = player.Hand.OrderByDescending(x => x.Value).ToList();
                            player.PlayedCard = player.Hand.FirstOrDefault(x => x.suit.ID == playerOne.PlayedCard.suit.ID);
                            continue;
                        }
                    }
                    else if (player.NumberOfTrumpCards() > 0 && player.Wins < player.Bid) //Try to win with Trump
                    {
                        player.Hand = player.Hand.OrderBy(x => x.Value).ToList();
                        player.PlayedCard = player.Hand.FirstOrDefault(x => x.suit.IsTrump);
                        continue;
                    }
                    else if (player.Wins == player.Bid && player.NumberOfTrumpCards() != player.Hand.Count()) // Dump High
                    {
                        player.Hand = player.Hand.OrderByDescending(x => x.Value).ToList();
                        player.PlayedCard = player.Hand.FirstOrDefault(x => x.suit.IsTrump == false);
                        continue;
                    }
                    else if (player.Wins < player.Bid) //Dump Low
                    {
                        player.Hand = player.Hand.OrderBy(x => x.Value).ToList();
                        player.PlayedCard = player.Hand[0];
                        continue;
                    }
                    else //Random
                    {
                        player.Hand.Shuffle();
                        player.PlayedCard = player.Hand[0];
                        continue;
                    }
                }
            }

        }


        public static void SetTrumps(List<Card> Deck, int TrumpSuit)
        {
            Deck.Where(x => x.suit.ID == TrumpSuit).ToList().ForEach(x => { x.suit.IsTrump = true; });
        }


        //Find highest card played in trick
        public static Card HighestCard(List<Player> players)
        {
            List<Card> PlayedCards = new List<Card>();
            List<Card> TempCards = new List<Card>();
            //Fill PlayerCards
            players.ForEach(x => { PlayedCards.Add(x.PlayedCard); });
            //Is Card Trump
            TempCards = PlayedCards.Where(x => x.suit.IsTrump == true).ToList();
            if (TempCards.Count > 0)
            {
                TempCards = TempCards.OrderByDescending(x => x.Value).ToList();
                players.ForEach(p => {
                    if (p.PlayedCard.ID == TempCards[0].ID) p.PlayedCard.Won = true;
                });
                return PlayedCards.FirstOrDefault(c => c.Won == true);
            }

            //Find Leading Card
            Card LeadingCard = players.FirstOrDefault(p => p.Leading == true).PlayedCard;

            //Remove Cards that are off suit
            PlayedCards.RemoveAll(c => c.suit.ID != LeadingCard.suit.ID);
            PlayedCards = PlayedCards.OrderByDescending(x => x.Value).ToList();
            return PlayedCards.FirstOrDefault(c => c.Won = true);
        }

        //CardOne is lead card - Compare two cards
        public static bool IsCardHigher(Card cardOne, Card cardTwo)
        {
            //Is Card Trump
            if (cardOne.suit.IsTrump && !cardTwo.suit.IsTrump)
            {
                return true;
            }
            else if (!cardOne.suit.IsTrump && cardTwo.suit.IsTrump)
            {
                return false;
            }

            //Is card off suit
            if (cardOne.suit.ID != cardTwo.suit.ID)
                return true;

            //Is card ace
            if (cardOne.Value == 1)
                return true;

            //Is card more valuable
            if (cardOne.Value > cardTwo.Value)
                return true;

            return false;
        }
        
    }
}
