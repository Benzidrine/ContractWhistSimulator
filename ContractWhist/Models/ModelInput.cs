using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML.Data;

namespace ContractWhist.Models
{
    public class ModelInput
    {
        [ColumnName("Bid"), LoadColumn(0)]
        public float Bid { get; set; }


        [ColumnName("NumberOfTrumpCards"), LoadColumn(1)]
        public float NumberOfTrumpCards { get; set; }


        [ColumnName("ValueOfTrumpCards"), LoadColumn(2)]
        public float ValueOfTrumpCards { get; set; }


        [ColumnName("ValueOfNonTrumpCards"), LoadColumn(3)]
        public float ValueOfNonTrumpCards { get; set; }


        [ColumnName("NumberOfAces"), LoadColumn(4)]
        public float NumberOfAces { get; set; }


        [ColumnName("NumberOfTwos"), LoadColumn(5)]
        public float NumberOfTwos { get; set; }


        [ColumnName("StdDevOfNonTrump"), LoadColumn(6)]
        public float StdDevOfNonTrump { get; set; }


        [ColumnName("Mean"), LoadColumn(7)]
        public float Mean { get; set; }


        [ColumnName("MeanNonTrump"), LoadColumn(8)]
        public float MeanNonTrump { get; set; }


        [ColumnName("MeanTrump"), LoadColumn(9)]
        public float MeanTrump { get; set; }


        [ColumnName("CardsInHand"), LoadColumn(10)]
        public string CardsInHand { get; set; }
    }
}
