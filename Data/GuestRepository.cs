using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using GuestbookWebApi.Interfaces;
using GuestbookWebApi.Model;


namespace GuestbookWebApi.Data
{
    public class GuestRepository : IGuestRepository
    {
        private readonly GuestContext _context = null;

        public GuestRepository(IOptions<Settings> settings)
        {
            _context = new GuestContext(settings);
        }

        public async Task<IEnumerable<Guest>> GetAllGuests()
        {
            try
            {
                return await _context.Guest.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddGuest(Guest guest)
        {
            try
            {
                await _context.Guest.InsertOneAsync(guest);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveAllGuests()
        {
            try
            {
                DeleteResult actionResult = await _context.Guest.DeleteManyAsync(new BsonDocument());

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;

            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }
    }
}
