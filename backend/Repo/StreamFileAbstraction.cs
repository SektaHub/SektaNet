namespace backend.Repo
{
    public class StreamFileAbstraction : TagLib.File.IFileAbstraction
    {
        private readonly Stream stream;
        public string Name { get; }

        public StreamFileAbstraction(string name, Stream readStream, Stream writeStream)
        {
            Name = name;
            stream = readStream;  // Assuming readStream and writeStream are the same in this context
            WriteStream = writeStream;
            ReadStream = readStream;
        }

        public Stream ReadStream { get; }
        public Stream WriteStream { get; }

        public void CloseStream(Stream stream)
        {
            stream.Flush();
            // Optionally close the stream if required
            // stream.Close();
        }
    }
}
