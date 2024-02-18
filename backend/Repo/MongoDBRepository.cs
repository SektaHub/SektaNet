using MongoDB.Driver.GridFS;
using MongoDB.Driver;
using System.IO;
using System.Threading.Tasks;
using MongoDB.Bson;
using backend.Models.Entity;

namespace backend.Repo
{
    public class MongoDBRepository
    {
        //private readonly IMongoCollection<Reel> reelCollection;
        private readonly IGridFSBucket gridFS;

        public MongoDBRepository(string connectionString, string databaseName)
        {
            var clientSettings = MongoClientSettings.FromConnectionString(connectionString);
            clientSettings.MaxConnectionPoolSize = 5000; // Adjust the size as needed
            var client = new MongoClient(clientSettings);
            var database = client.GetDatabase(databaseName);
            gridFS = new GridFSBucket(database);
        }

        public async Task<ObjectId> UploadFileAsync(Stream fileStream, string fileName)
        {
            return await gridFS.UploadFromStreamAsync(fileName, fileStream);

        }

        public async Task<Stream> DownloadFileAsync(string id)
        {
            return await gridFS.OpenDownloadStreamAsync(new ObjectId(id));
        }

        public async Task DeleteFileAsync(string id)
        {
            await gridFS.DeleteAsync(new ObjectId(id));
        }

        public async Task<Stream> GetFileStreamAsync(string id)
        {
            var stream = new MemoryStream();
            await gridFS.DownloadToStreamAsync(new ObjectId(id), stream);
            stream.Position = 0; // Reset the stream position to the start
            return stream;
        }




    }
}
