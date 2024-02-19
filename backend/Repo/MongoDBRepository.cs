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

            // Extract generic file info such as creation time, last modification, etc.
            // Since we're working with a stream, we assume the file is being uploaded and these system-specific metadata might not be available directly from the stream.
            metadata["UploadedAt"] = DateTime.UtcNow;

            // Decide how to extract metadata based on file extension (simplistic approach)
            string extension = Path.GetExtension(fileName).ToLower();
            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
            {
                // For images
                fileStream.Position = 0; // Reset position to ensure we read from the beginning
                var imageMetadata = ImageMetadataReader.ReadMetadata(fileStream);
                foreach (var directory in imageMetadata)
                    foreach (var tag in directory.Tags)
                        metadata[$"{directory.Name}:{tag.Name}"] = tag.Description;
            }
            else if (extension == ".mp3" || extension == ".wav" || extension == ".mp4")
            {
                // For audio and video, TagLib supports both but you might need special handling for video
                fileStream.Position = 0;  // Reset position
                var file = TagLib.File.Create(new StreamFileAbstraction(fileName, fileStream, fileStream));
                // Now extract different metadata based on the file type, e.g., title, album for audio
                metadata["Duration"] = file.Properties.Duration.ToString();
                // Add more metadata as needed
            }
            // Can add more conditions for other file types

            var options = new GridFSUploadOptions
            {
                Metadata = metadata
            };

            fileStream.Position = 0; // Ensure we're at the beginning of the stream for upload
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
