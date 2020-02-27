using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using DeckOfCards.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeckOfCards.Controllers
{
    public class CardController : Controller
    {
        // this JsoNDocument will eventually hold our cards
        //after we draw from the deck
        private JsonDocument jDoc;

        // make a method that will draw a new deck
        // this will give us access to a deck id
        public async Task<string> GetDeck()
        {
            // make using block to hold our HttpClient object
            using (var httpClient = new HttpClient())
            {
                // we make another using block to make an async call to the api
                using (var response = await httpClient
                    .GetAsync("https://deckofcardsapi.com/api/deck/new/shuffle/?deck_count=1"))
                {
                    // read our api response as a string
                    var stringResponse = await response.Content.ReadAsStringAsync();
                    // parse the string and return a JsonDocument
                    jDoc = JsonDocument.Parse(stringResponse);

                    //call get property off the root element, but dont try to call the type yet
                    // look at ValueKinf property to see what the type is
                    //var testString = jDoc.RootElement.GetProperty("deck_id");

                    //or get the ValueKind property value to see what the type is
                    //var testString = jDoc.RootElement.GetProperty("deck_id").ValueKind;

                     // then once you know the type you can call the respective Get method, to get the value from the Json Property
                    //var testString = jDoc.RootElement.GetProperty("deck_id").GetString();
                }
            }

            // we return the deck id as a string,
            //that we will get from a property of our JsonDocument
            return jDoc.RootElement.GetProperty("deck_id").GetString();
        }

        // we make an async method that will call the deck of cards api
        // and return to use a number of cards
        // we will put those in a List of cards, and return them
        public async Task<Hand> GetCards(string deckId)
        {
            // make a blank list of cards to add cards to later
            List<Card> cardList = new List<Card>();
            // We use a Hand object, which will hold our list of cards
            Hand hand = new Hand();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient
                    .GetAsync("https://deckofcardsapi.com/api/deck/" + deckId + "/draw/?count=1"))
                {
                    var stringResponse = await response.Content.ReadAsStringAsync();

                    // we have built a Card class to map our card data to
                    // as well as a Hand class that will hold our array of Cards
                    // we can now use the JsonSerializer.Deserialize<Hand>() method,
                    // to the Card array that is called cards in our Hand object
                    hand = JsonSerializer.Deserialize<Hand>(stringResponse);

                    var temp = JsonSerializer.Serialize(hand);


                    jDoc = JsonDocument.Parse(stringResponse);

                    //this grabbed the data the formed our card array in Json format
                    var jsonList = jDoc.RootElement.GetProperty("cards");


                    //we will use a for loop to iterate through that card array

                   //and build our List of cards to return
                    for (int i = 0; i < jsonList.GetArrayLength(); i++)
                    {
                        // we parse through the data while building a new Card
                        // and then add that Card to our List of cards
                        cardList.Add(new Card()
                        {
                            // we now map the properties from the Json
                            // to the new Card and add to the List
                            image = jsonList[i].GetProperty("image").GetString(),
                            suit = jsonList[i].GetProperty("suit").GetString(),
                            value = jsonList[i].GetProperty("value").GetString(),
                            code = jsonList[i].GetProperty("code").GetString()
                        });
                    }



                }
            }
            return hand;
        }


    }
}