using System.IO;
namespace sqlite_archive_cs
{
    public class InferTable
    {
        public string Clear(ref string input)
        {
            return input.ToLower();
        }
        public string Clear(ref string input, bool lowercase)
        {
            string _Out = input.Replace('.', '_').Replace(' ', '_').Replace('\'', '_').Replace(',', '"').Replace('/', '_').Replace('\\', '_');

            if (lowercase)
            {
                return _Out.ToLower();
            }
            else
            {
                return _Out;
            }
        }
        public string Infer(ref string input, bool extract)
        {
            return ""; //Temporary to shut intellisense up.
        }
    }
}