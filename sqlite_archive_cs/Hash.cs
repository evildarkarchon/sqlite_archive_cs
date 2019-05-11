using System.IO;
using System.Security.Cryptography;

namespace sqlite_archive_cs
{
    public class Hash {
        private SHA256 Sha256 = SHA256.Create();
        private SHA512 Sha512 = SHA512.Create();

        public byte[] GetHashSha256(string filename)
        {
            using (FileStream stream = File.OpenRead(filename))
            {
                return Sha256.ComputeHash(stream);
            }
        }

        public byte[] GetHashSha512(string filename)
        {
            using (FileStream stream = File.OpenRead(filename))
            {
                return Sha512.ComputeHash(stream);
            }
        }
    }
}