using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NeuralNetworkLib;

namespace ContractWhist
{
    public class NNDecideCard
    {
        public static void ApplyWeightsToCards(NeuralNetwork NN, List<Card> cards)
        {
            List<float> IDs = cards.Select(x => (float)x.ID).ToList();
            List<float> Weights = NN.FeedForward(IDs.ToArray()).ToList();
            for (int i = 0; i < cards.Count(); i++)
            {
                cards[i].NNValue = Weights[i];
            }
        }
    }
}
