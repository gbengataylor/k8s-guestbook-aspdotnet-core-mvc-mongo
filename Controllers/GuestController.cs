using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GuestbookWebApi.Interfaces;
using GuestbookWebApi.Model;
using GuestbookWebApi.Infrastructure;
using System;
using System.Collections.Generic;

namespace GuestbookWebApi.Controllers
{

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class GuestController : Controller
    {
        private readonly IGuestRepository _guestRepository;
        private static readonly Random getrandom = new Random();

        public GuestController(IGuestRepository guestRepository)
        {
            _guestRepository = guestRepository;
        }

        [NoCache]
        [HttpGet]
        public async Task<IEnumerable<Guest>> Get()
        {
            return await _guestRepository.GetAllGuests();
        }

        // POST api/guest
        [HttpPost]
        public void Post([FromBody] GuestParam newGuest)
        {
            _guestRepository.AddGuest(new Guest
                                        {
                                            Id = getrandom.Next(3, 10000).ToString(),
                                            Name = newGuest.Name,
                                            CreatedOn = DateTime.Now,
                                            UpdatedOn = DateTime.Now
                                        });
        }

    }
}
