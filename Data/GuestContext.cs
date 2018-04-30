using Microsoft.Extensions.Options;
using MongoDB.Driver;
using GuestbookWebApi.Model;

namespace GuestbookWebApi.Data
{
    public class GuestContext
    {
        private readonly IMongoDatabase _database = null;

        public GuestContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Guest> Guest
        {
            get
            {
                return _database.GetCollection<Guest>("Guest");
            }
        }
    }
}
