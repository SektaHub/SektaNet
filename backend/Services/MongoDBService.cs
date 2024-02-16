using MongoDB.Driver.GridFS;
using MongoDB.Driver;
using System.IO;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace backend.Services
{
    public class MongoDBService
    {
        private readonly IGridFSBucket gridFS;

        public MongoDBService(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            gridFS = new GridFSBucket(database);
        }

        public async Task<ObjectId> UploadFileAsync(Stream fileStream, string fileName)
        {
            return await gridFS.UploadFromStreamAsync(fileName, fileStream);
        }
        
        public async Task<Stream> DownloadFileAsync(string id)
        {
            return await gridFS.OpenDownloadStreamAsync(new MongoDB.Bson.ObjectId(id));
        }

        public async Task DeleteFileAsync(string id)
        {
            await gridFS.DeleteAsync(new MongoDB.Bson.ObjectId(id));
        }
    }
}
