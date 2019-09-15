using System.IO;

namespace sqlite_archive_cs
{
    public class FileInfo
    {
        public string Name { get; set; }

        private byte[] data;

        public byte[] GetData()
        {
            return data;
        }

        public void SetData(byte[] value)
        {
            data = value;
        }

        public string Digest { get; set; }

        public FileInfo() : base() { }
        public FileInfo(string filename)
        {
            Name = filename;
            if (Directory.Exists(Name) || File.Exists(Name))
            {
                Name = Path.GetFullPath(Name);
            }

            var _isdirfile = Utility.IsDirOrFile(Name);
            /*
            if (_isdirfile == false)
            {
                Data = File.ReadAllBytes(Name);
            }
            else if (_isdirfile == null)
            {
                throw new FileNotFoundException("Could not find the file:", Name);
            }
            */

            switch(_isdirfile)
            {
                case true:
                    throw new InvalidDataException($"{filename} pointed to a directory.");
                case false:
                    SetData(File.ReadAllBytes(Name));
                    break;
                case null:
                    throw new FileNotFoundException($"{filename} could not be found.");
            }

            if (GetData().Length >= 1)
            {
                Digest = Utility.GetHash(Name);
            }
        }

        public FileInfo(string filename, byte[] filedata)
        {
            Name = filename;
            if (Directory.Exists(Name) || File.Exists(Name))
            {
                Name = Path.GetFullPath(Name);
            }
            SetData(filedata);

            if (GetData().Length >= 1)
            {
                Digest = Utility.GetHash(Name);
            }
        }

        public FileInfo(string filename, byte[] filedata, string filedigest)
        {
            Name = filename;
            if (Directory.Exists(Name) || File.Exists(Name))
            {
                Name = Path.GetFullPath(Name);
            }
            SetData(filedata);
            Digest = filedigest;
        }
    }
}
