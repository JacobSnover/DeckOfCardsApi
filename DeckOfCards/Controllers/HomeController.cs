using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DeckOfCards.Models;
using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace DeckOfCards.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        // make a global card controller object, that allows me to call
        // the deck of cards api
        CardController cardController = new CardController();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var deckid = await cardController.GetDeck();

            // we use sessions to hold data as a key value pair
            HttpContext.Session.SetString("deckId", deckid);

            //var testSession = HttpContext.Session.GetString("deckId");

            return View();
        }

        // we make an async method that will call the cardController
        // and return to us a number of cards
        public async Task<IActionResult> GetCards()
        {
            Hand hand = await cardController
                .GetCards(HttpContext.Session.GetString("deckId"));
            return View(hand);
        }

        public async Task<Hand> GetHand()
        {
            Hand hand = await cardController
                .GetCards(HttpContext.Session.GetString("deckId"));
            return hand;
        }



        ////we make the method async, so that we can await async method calls
        ////inside this method
        //// Task keyword is saying that the return process will be an async process
        //public async Task<IActionResult> Index()
        //{
        //    //using blocks in our code, assure that the objects resources/memory 
        //    //are, released to be used again

        //    //using this string to hold response before it gets released
        //    string apiResponse = "";

        //    //make an new HttpClient object in a using block
        //    using (var httpClient = new HttpClient())
        //    {
        //        //make another using block for these internal processes
        //        //call the GetAsync method, and await the response
        //        using (var response = await httpClient.GetAsync("https://deckofcardsapi.com/api/deck/new/shuffle/?deck_count=1"))
        //        {
        //            //we then read the response with the ReadAsStringAsync method
        //            //which we will also await
        //            apiResponse = await response.Content.ReadAsStringAsync();

        //            //parses our Json String into a JsonDocument object
        //            var jsonDocument = JsonDocument.Parse(apiResponse);

        //            // we can then GetProperty or "Index" through our JsonDocument
        //            //object, and then we call the GetString method,
        //            //to retrieve the value from that index
        //            //we call GetString because the value is a string,
        //            //if the value is another type, you have to call
        //            //the method that represents that type
        //            var jsonProperty = jsonDocument.RootElement
        //                .GetProperty("deck_id").GetString();

        //            var cardModel = JsonSerializer.Deserialize<Card>(apiResponse);

        //        }// this releases the resource used to call the api

        //    }// this releases the memory used to hold the HttpClient

        //        return View();
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
