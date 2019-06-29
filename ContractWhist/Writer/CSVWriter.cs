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
            var newLine = string.Format("{0},{1},{2},{3},{4}", handRecord.Bid, handRecord.NumberOfTrumpCards,  handRecord.TotalValue, handRecord.NumberOfAces, handRecord.NumberOfTwos);
            csv.AppendLine(newLine);

            //after your loop
            if (handRecord.GameWon == 1)
                File.AppendAllText("Z:\\Repos\\ContractWhist\\ContractWhist\\Writer\\HandRecord.csv", csv.ToString());
        }
    }

    //mlnet auto-train --task regression --dataset "HandRecord.csv" --label-column-index 0 --has-header true --max-exploration-time 30
}
