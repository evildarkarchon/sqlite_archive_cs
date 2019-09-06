using System.Collections.Generic;
using System.IO;
namespace sqlite_archive_cs
{
    class InferTable
    {
        public string Clean(string input)
        {
            string Out = input.Replace(".", "_").Replace(" ", "_").Replace(@"'", "_").Replace(",", "").Replace("/", "_").Replace(@"\", "_").Replace(",", "_").Replace("#", "");
            return Out;
        }
        public string Clean(string input, bool lowercase)
        {
            string Out = input.Replace(".", "_").Replace(" ", "_").Replace(@"'", "_").Replace(",", "").Replace("/", "_").Replace(@"\", "_").Replace(",", "_").Replace("#", "");
            if (lowercase)
            {
                return Out.ToLower();
            }
            else
            {
                return Out;
            }
        }

        public string InferTableAdd(ref List<string> files, bool lower)
        {
            string f = default;
            bool _dirorfile = (bool)FileInfo.IsDirOrFile(files[0]);
            if (!_dirorfile)
            {
                if (File.Exists(files[0]))
                {
                    f = Path.GetFileName(files[0]);
                }
            }
            else if (_dirorfile)
            {
                if (Directory.Exists(files[0])) {
                    f = Path.GetDirectoryName(files[0]);
                }
            }

            if (!string.IsNullOrEmpty(f))
            {
                return Clean(f, lower);
            }
            else
            {
                new InvalidDataException("No table name returned");
            }

            return null;
        }
        public string InferTableAdd(ref List<string> files)
        {
            string f = default;
            bool _dirorfile = (bool)FileInfo.IsDirOrFile(files[0]);
            if (!_dirorfile)
            {
                if (File.Exists(files[0]))
                {
                    f = Path.GetFileName(files[0]);
                }
            }
            else if (_dirorfile)
            {
                if (Directory.Exists(files[0]))
                {
                    f = Path.GetDirectoryName(files[0]);
                }
            }

            if (!string.IsNullOrEmpty(f))
            {
                return Clean(f, false);
            }
            else
            {
                new InvalidDataException("No table name returned");
            }

            return null;
        }

        public string InferTableExtract(ref List<string> files, string outdir, bool lower, bool popfile)
        {
            if (!string.IsNullOrEmpty(files[0]))
            {
                string f;
                if (string.IsNullOrEmpty(outdir))
                {
                    f = Clean(Path.GetFileNameWithoutExtension(files[0]), lower);
                }
                else
                {
                    f = Clean(outdir);
                }

                if (popfile)
                {
                    files.Remove(files[0]);
                }
                return f;
            }
            return null;
        }
        public string InferTableExtract(ref List<string> files, bool lower, bool popfile)
        {
            if (!string.IsNullOrEmpty(files[0]))
            {
                string f = Clean(Path.GetFileNameWithoutExtension(files[0]), lower);
                if (popfile)
                {
                    files.Remove(files[0]);
                }
                return f;
            }
            return null;
        }
        public string InferTableExtract(ref List<string> files, bool popfile)
        {
            if (!string.IsNullOrEmpty(files[0]))
            {
                string f = Clean(Path.GetFileNameWithoutExtension(files[0]), false);
                if (popfile)
                {
                    files.Remove(files[0]);
                }
                return f;
            }
            return null;
        }
        public string InferTableExtract(ref List<string> files)
        {
            if (!string.IsNullOrEmpty(files[0]))
            {
                string f = Clean(Path.GetFileNameWithoutExtension(files[0]), false);
                return f;
            }
            return null;
        }
        public string InferTableExtract(ref List<string> files, string outdir, bool lower)
        {
            if (!string.IsNullOrEmpty(files[0]))
            {
                string f;
                if (string.IsNullOrEmpty(outdir))
                {
                    f = Clean(Path.GetFileNameWithoutExtension(files[0]), lower);
                }
                else
                {
                    f = Clean(outdir);
                }
                return f;
            }
            return null;
        }
        public string InferTableExtract(ref List<string> files, string outdir)
        {
            if (!string.IsNullOrEmpty(files[0]))
            {
                string f;
                if (string.IsNullOrEmpty(outdir))
                {
                    f = Clean(Path.GetFileNameWithoutExtension(files[0]), false);
                }
                else
                {
                    f = Clean(outdir);
                }
                return f;
            }
            return null;
        }
    }
}
