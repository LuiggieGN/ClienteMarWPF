using System; 

namespace ClienteMarWPFWin7.Domain.Exceptions
{
    public class MarFileReadException : Exception
    {
        public string FileName { get; set; }

        public MarFileReadException(string filename)
        {
            FileName = filename;
        }

        public MarFileReadException(string message, string filename) : base(message)
        {
            FileName = filename;
        }

        public MarFileReadException(string message, Exception innerException, string filename) : base(message, innerException)
        {
            FileName = filename;
        }
    }
}
