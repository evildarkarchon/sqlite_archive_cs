using System;
using System.IO;
using System.Security.Cryptography;

namespace sqlite_archive_cs
{
    class Utility
    {
        public static bool IsNetworkPath(string path)
        {
            if (!path.StartsWith(@"/") && !path.StartsWith(@"\"))
            {
                string rootPath = Path.GetPathRoot(path); // get drive's letter
                DriveInfo driveInfo = new DriveInfo(rootPath); // get info about the drive
                return driveInfo.DriveType == DriveType.Network; // return true if a network drive
            }

            return true; // is a UNC path
        }
        public static string GetHash(string filename, string hashalg)
        {
            using (FileStream stream = File.OpenRead(filename))
            {
                switch (hashalg)
                {
                    case "SHA512":
                    case "sha512":
                    case "Sha512":
                    case "default":
                        using (SHA512 Sha512 = SHA512.Create())
                        {
                            string output = Sha512.ComputeHash(stream).ToString();
                            return output;
                        }
                    case "SHA256":
                    case "sha256":
                    case "Sha256":
                        using (SHA256 Sha256 = SHA256.Create())
                        {
                            string output = Sha256.ComputeHash(stream).ToString();
                            return output;
                        }
                    case "SHA384":
                    case "sha384":
                    case "Sha384":
                        using (SHA384 Sha384 = SHA384.Create())
                        {
                            string output = Sha384.ComputeHash(stream).ToString();
                            return output;
                        }
                    case "SHA1":
                    case "sha1":
                    case "Sha1":
                        using (SHA1 Sha1 = SHA1.Create())
                        {
                            string output = Sha1.ComputeHash(stream).ToString();
                            return output;
                        }
                }
            }
            throw new NotSupportedException("This exception should not be able to be reached, if it is, something is wrong.");
        }
        public static string GetHash(string filename)
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
    }
}
