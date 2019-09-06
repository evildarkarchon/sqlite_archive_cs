using System.IO;
using System.Security.Cryptography;

namespace sqlite_archive_cs
{
    public class FileInfo
    {
        public string Name { get; set; }
        public byte[] Data { get; set; }
        public string Digest { get; set; }

        public FileInfo() : base() { }
        public FileInfo(string filename)
        {
            Name = filename;
            if (Directory.Exists(Name) || File.Exists(Name))
            {
                Name = Path.GetFullPath(Name);
            }
            bool? _isdirfile = IsDirOrFile(Name);
            if (_isdirfile == false)
            {
                Data = File.ReadAllBytes(Name);
            }
            else if (_isdirfile == null)
            {
                new FileNotFoundException("Could not find the file:", Name);
            }
            if (Data.Length >= 1)
            {
                Digest = GetHashSha512(Name);
            }
        }

        public FileInfo(string filename, byte[] filedata)
        {
            Name = filename;
            if (Directory.Exists(Name) || File.Exists(Name))
            {
                Name = Path.GetFullPath(Name);
            }
            Data = filedata;

            if (Data.Length >= 1)
            {
                Digest = GetHashSha512(Name);
            }
        }

        public FileInfo(string filename, byte[] filedata, string filedigest)
        {
            Name = filename;
            if (Directory.Exists(Name) || File.Exists(Name))
            {
                Name = Path.GetFullPath(Name);
            }
            Data = filedata;
            Digest = filedigest;
        }

        public static bool? IsDirOrFile(string path)
        {
            // Returns true if the path is a dir, false if it's a file and null if it's neither or doesn't exist.
            bool? result = null;
            if (Directory.Exists(path) || File.Exists(path))
            { // get the file attributes for file or directory
                var fileAttr = File.GetAttributes(path);
                if (fileAttr.HasFlag(FileAttributes.Directory)) result = true;
                else result = false;
            }
            return result;
        }
        
        /*public static string GetHashSha256(string filename)
        {
            using (FileStream stream = File.OpenRead(filename))
            {
                using (SHA256 Sha256 = SHA256.Create())
                {
                    string output = Sha256.ComputeHash(stream).ToString();
                    return output;
                }
            }
        }*/

        public static string GetHashSha512(string filename)
        {
            using (FileStream stream = File.OpenRead(filename))
            {
                using (SHA512 Sha512 = SHA512.Create())
                {
                    string output = Sha512.ComputeHash(stream).ToString();
                    return output;
                }
            }
        }
    }
}
