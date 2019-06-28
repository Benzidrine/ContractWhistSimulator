using System;
using System.Collections.Generic;
using System.Text;

namespace ContractWhist
{
    public class Suit
    {
        public Suit (int IDinput)
        {
            //convert suit ID from card ID
            ID = Convert.ToInt32(Math.Floor((IDinput - 0.01M) / 13M));
            SuitName = ((SuitNameEnum)ID).ToString();
        }
        public bool IsTrump { get; set; }
        public int ID { get; set; }
        public string SuitName { get; set; }
        public enum SuitNameEnum { Diamonds, Clubs, Hearts, Spades }
    }
}
