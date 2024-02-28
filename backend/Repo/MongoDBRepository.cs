using MongoDB.Driver.GridFS;
using MongoDB.Driver;
using System.IO;
using System.Threading.Tasks;
using MongoDB.Bson;
using backend.Models.Entity;
using MetadataExtractor;

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
            var metadata = new BsonDocument();

            metadata["UploadedAt"] = DateTime.UtcNow;

            string extension = Path.GetExtension(fileName).ToLower();
            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
            {
                // Image metadata extraction
                fileStream.Position = 0;
                var imageMetadata = ImageMetadataReader.ReadMetadata(fileStream);
                foreach (var directory in imageMetadata)
                {
                    foreach (var tag in directory.Tags)
                    {
                        // Convert potentially complex objects to a string representation
                        metadata[$"{directory.Name}:{tag.Name}"] = tag.Description.ToString();
                    }
                }
            }
            else if (extension == ".mp3" || extension == ".wav" || extension == ".mp4")
            {
                try
                {
                    fileStream.Position = 0;
                    var file = TagLib.File.Create(new StreamFileAbstraction(fileName, fileStream, fileStream));
                    metadata["Duration"] = file.Properties.Duration.TotalSeconds.ToString();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to extract metadata for {fileName}: {ex.Message}");
                }
            }
            // More conditions for different file types could be added here

            var options = new GridFSUploadOptions
            {
                Metadata = metadata
            };

            fileStream.Position = 0; // Ensure the stream is at the beginning for upload
            return await gridFS.UploadFromStreamAsync(fileName, fileStream, options);
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
