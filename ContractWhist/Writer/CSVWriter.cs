using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ContractWhist
{
    public class CSVWriter
    {
        public static void WriteHandResult(HandRecord handRecord)
        {
            var csv = new StringBuilder();
            var newLine = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", handRecord.Bid, handRecord.NumberOfTrumpCards, handRecord.ValueOfTrumpCards, handRecord.ValueOfNonTrumpCards, handRecord.NumberOfAces, handRecord.NumberOfTwos, Math.Round(handRecord.StdDevOfNonTrump,3).ToString(),handRecord.Mean,handRecord.MeanNonTrump,handRecord.MeanTrump, handRecord.CardsInHand);
            csv.AppendLine(newLine);

            //after your loop
            if (handRecord.GameWon == 1)
                File.AppendAllText("Z:\\Repos\\ContractWhist\\ContractWhist\\Writer\\HandRecord.csv", csv.ToString());
        }
    }

    //mlnet auto-train --task regression --dataset "HandRecord.csv" --label-column-index 0 --has-header true --max-exploration-time 30
}
