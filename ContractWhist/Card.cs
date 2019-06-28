using System;
using System.Collections.Generic;
using System.Text;

namespace ContractWhist
{
    public class Card
    {
        //ID is intended to define value and suit
        public Card(int inputID)
        {
            ID = inputID;
            suit = new Suit(inputID);
            Value = inputID - (suit.ID * 13);
            if (Value == 0) Value = 13; //If king then set to 13 to avoid being set to zero above 
            ValueName = ((ValueNameEnum)Value).ToString();
            if (Value == 1) Value = 14; //Set ace high
        }
        public int ID { get; set; }
        public int Value { get; set; }
        public string ValueName { get; set; }
        public enum ValueNameEnum { None, Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King }
        public Suit suit { get; set; }
        public bool Won { get; set; } //if card won trick
    }
}
