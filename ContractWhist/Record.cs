using System;
using System.Collections.Generic;
using System.Text;

namespace ContractWhist
{
    public class Record
    {
        public static HandRecord RecordHandInit(Player player)
        {
            HandRecord hr = new HandRecord();
            hr.Bid = player.Bid;
            hr.CardsInHand = player.HandSeperatedString();
            hr.TotalValue = player.SumValue();
            hr.Mean = player.Mean();
            hr.NumberOfTrumpCards = player.NumberOfTrumpCards();
            hr.NumberOfAces = player.NumberOfValueCard(14);
            hr.NumberOfTwos = player.NumberOfValueCard(2);
            hr.ValueOfTrumpCards = player.SumValueConsideringTrump(true);
            hr.ValueOfNonTrumpCards = player.SumValueConsideringTrump(false);
            hr.StdDevOfNonTrump = player.StdDevConsideringTrump(false);
            hr.MeanNonTrump = player.MeanConsideringTrump(false);
            hr.MeanTrump = player.MeanConsideringTrump(true);
            hr.GameWon = 0;
            return hr;
        }
    }
}
