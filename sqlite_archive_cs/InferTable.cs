using System.Collections.Generic;
using System.IO;
namespace sqlite_archive_cs
{
    public abstract class InferTableAbstract
    {
        public abstract string Clean(string input);
        public abstract string Clean(string input, bool lowercase);
        public abstract string InferTableAdd(ref List<string> files, bool lower);
        public abstract string InferTableAdd(ref List<string> files);
        public abstract string InferTableExtract(ref List<string> files, string outdir, bool lower, bool popfile);
        public abstract string InferTableExtract(ref List<string> files, bool lower, bool popfile);
        public abstract string InferTableExtract(ref List<string> files, string outdir, bool lower);
        public abstract string InferTableExtract(ref List<string> files, bool popfile);
        public abstract string InferTableExtract(ref List<string> files, string outdir);
        public abstract string InferTableExtract(ref List<string> files);
    }

    public class InferTable : InferTableAbstract
    {
        public override string Clean(string input)
        {
            string Out = input.Replace(".", "_").Replace(" ", "_").Replace(@"'", "_").Replace(",", "").Replace("/", "_").Replace(@"\", "_").Replace(",", "_").Replace("#", "");
            return Out;
        }
        public override string Clean(string input, bool lowercase)
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

        public override string InferTableAdd(ref List<string> files, bool lower)
        {
            string f = default;
            var _dirorfile = Utility.IsDirOrFile(files[0]);
            /*
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
            */

            switch (_dirorfile)
            {
                case true:
                    if (Directory.Exists(files[0]))
                    {
                        f = Path.GetDirectoryName(files[0]);
                    }
                    break;
                case false:
                    if (File.Exists(files[0]))
                    {
                        f = Path.GetFileNameWithoutExtension(files[0]);
                    }
                    break;
                case null:
                default:
                    throw new FileNotFoundException($"{files[0]} was not found.");
            }

            if (!string.IsNullOrEmpty(f))
            {
                return Clean(f, lower);
            }
            else
            {
                throw new InvalidDataException("No table name returned");
            }
        }
        public override string InferTableAdd(ref List<string> files)
        {
            string f = default;
            var _dirorfile = Utility.IsDirOrFile(files[0]);
            /*
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
            */

            switch (_dirorfile)
            {
                case true:
                    if (Directory.Exists(files[0]))
                    {
                        f = Path.GetDirectoryName(files[0]);
                    }
                    break;
                case false:
                    if (File.Exists(files[0]))
                    {
                        f = Path.GetFileNameWithoutExtension(files[0]);
                    }
                    break;
                case null:
                default:
                    throw new FileNotFoundException($"{files[0]} was not found.");
            }

            if (!string.IsNullOrEmpty(f))
            {
                return Clean(f, false);
            }
            else
            {
                throw new InvalidDataException("No table name returned");
            }
        }

        public override string InferTableExtract(ref List<string> files, string outdir, bool lower, bool popfile)
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
        public override string InferTableExtract(ref List<string> files, bool lower, bool popfile)
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
        public override string InferTableExtract(ref List<string> files, bool popfile)
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
        public override string InferTableExtract(ref List<string> files)
        {
            if (!string.IsNullOrEmpty(files[0]))
            {
                string f = Clean(Path.GetFileNameWithoutExtension(files[0]), false);
                return f;
            }
            return null;
        }
        public override string InferTableExtract(ref List<string> files, string outdir, bool lower)
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
        public override string InferTableExtract(ref List<string> files, string outdir)
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
