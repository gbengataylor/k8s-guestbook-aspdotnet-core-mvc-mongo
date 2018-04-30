using System;
using System.Collections;
using Microsoft.AspNetCore.Mvc;

using GuestbookWebApi.Interfaces;
using GuestbookWebApi.Model;

namespace GuestbookWebApi.Controllers
{
    [Route("api/[controller]")]
    public class SystemController : Controller
    {
        private readonly IGuestRepository _guestRepository;

        public SystemController(IGuestRepository guestRepository)
        {
            _guestRepository = guestRepository;
        }

        // Call an initialization - api/system/init
        [HttpGet("{setting}")]
        public string Get(string setting)
        {
            if (setting == "init")
            {
                _guestRepository.RemoveAllGuests();

                _guestRepository.AddGuest(new Guest() { Id = "1", Name = "John Osborne", CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now});
                _guestRepository.AddGuest(new Guest() { Id = "2", Name = "Harold Wong", CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now});
                
                return "Database GuestDb was created, and collection 'Guests' was filled with 2 sample items";
            }
            else if(setting == "env"){
                var sb = new System.Text.StringBuilder();
                sb.AppendLine("{");
                string replaceWith = "";
                bool foundenv = false;

                foreach (DictionaryEntry de in Environment.GetEnvironmentVariables()) 
                {
                    foundenv = true;
                    var key = de.Key.ToString().Replace("\r\n", replaceWith).Replace("\n", replaceWith).Replace("\r", replaceWith).Replace("\"", replaceWith).Replace("\'", replaceWith);
                    var value = de.Value.ToString().Replace("\r\n", replaceWith).Replace("\n", replaceWith).Replace("\r", replaceWith).Replace("\"", replaceWith).Replace("\'", replaceWith);
                    var clean = $"\"{key}\" : \"{value}\",";
                    sb.AppendLine(clean);
                }             
                //Remove the extra comma
                if(foundenv){
                    //Have to go back 2 chars to remove the comma
                    sb.Length -= 2;
                    //Re-add the new line char
                    sb.AppendLine();
                }                

                sb.AppendLine("}");
                return sb.ToString();
            }
            
            return "Unknown";
        }
    }
}
