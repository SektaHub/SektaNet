namespace backend.Util
{
    using System.Security.Cryptography;

    public static class RandomBytesGenerator
    {
        public static byte[] GenerateRandomBytes(int length)
        {
            byte[] bytes = new byte[length];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            return bytes;
        }

        public static Guid GenerateGuid()
        {
            byte[] bytes = new byte[16];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            return new Guid(bytes);
        }
    }


}
