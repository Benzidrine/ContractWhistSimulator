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


        [ColumnName("TotalValue"), LoadColumn(2)]
        public float TotalValue { get; set; }


        [ColumnName("NumberOfAces"), LoadColumn(3)]
        public float NumberOfAces { get; set; }


        [ColumnName("NumberOfTwos"), LoadColumn(4)]
        public float NumberOfTwos { get; set; }

    }
}
