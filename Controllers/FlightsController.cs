using AmadeusIntegration.API;
using AmadeusIntegration.Models;
using AmadeusIntegration.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Amadeus_Test
{
    [RoutePrefix("api/flights")]
    public class FlightsController : ApiController
    {
        [HttpPost]
        [Route("init")]
        public IHttpActionResult Initialize( AmadeusSettings settings)
        {
            if (settings==null||string.IsNullOrEmpty(settings.ClientId) || string.IsNullOrEmpty(settings.ClientSecret))
                return BadRequest("Missing Settings(Id/Secret)");
            AmadeusFlightApi.Initialization(settings);
            return Ok("Amadeus API initialized.");
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> SearchFlights([FromUri] FlightSearchGetRequest request)
        {
            if(request == null)
                return BadRequest("Search request cannot be null.");
            try
            {
            var searchResult = await AmadeusFlightApi.FindAllFlightsGet_Async(request);
            return Ok(searchResult);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("price")]
        public async Task<IHttpActionResult> GetPrice([FromBody] List<FlightOffer> selectedOffers)
        {
            var priceResult = await AmadeusFlightApi.GetFlightPrice_Async(new FlightPriceRequest(selectedOffers));
            return Ok(priceResult);
        }

        [HttpPost]
        [Route("book")]
        public async Task<IHttpActionResult> BookFlight([FromBody] FlightOrderRequest bookingRequest)
        {
            var bookingResult = await AmadeusFlightApi.BookFlight_Async(bookingRequest);
            return Ok(bookingResult);
        }
    }
}