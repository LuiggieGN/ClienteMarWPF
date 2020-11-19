using System; 

namespace ClienteMarWPF.Domain.Exceptions
{
    public class MarFileWriteException : Exception
    {
        public string FileName { get; set; }

        public MarFileWriteException(string filename)
        {
            FileName = filename;
        }

        public MarFileWriteException(string message, string filename) : base(message)
        {
            FileName = filename;
        }

        public MarFileWriteException(string message, Exception innerException, string filename) : base(message, innerException)
        {
            FileName = filename;
        }
    }
}
