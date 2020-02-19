using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeckOfCards.Models
{
    // this class represents the array of cards our deck of cards api returns to us
    public class Hand
    {
        // this list of cards, is what the array or cards will desearialize to
        public Card[] cards { get; set; }
    }
}    