using System.Collections.Generic;
using System.Threading.Tasks;
using GuestbookWebApi.Model;

namespace GuestbookWebApi.Interfaces
{
    public interface IGuestRepository
    {
        Task<IEnumerable<Guest>> GetAllGuests();

        // add new note document
        Task AddGuest(Guest guest);

        // should be used with high cautious, only in relation with demo setup
        Task<bool> RemoveAllGuests();
        
    }
}
