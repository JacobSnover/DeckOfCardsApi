using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeckOfCards.Models
{
    public class Card
    {
        //public string deck_id { get; set; }

        // we need to make properties in our model to reflect the data
        //that represents a card from our Json
        public string image { get; set; }
        public string value { get; set; }
        public string code { get; set; }
        public string suit { get; set; }

    }
}
