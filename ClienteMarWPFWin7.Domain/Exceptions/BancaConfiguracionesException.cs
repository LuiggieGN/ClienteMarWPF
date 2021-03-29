using System;
 

namespace ClienteMarWPFWin7.Domain.Exceptions
{
    public class BancaConfiguracionesException : Exception
    {
        public string ConfigurationName { get; set; }
 
        public BancaConfiguracionesException(string configuration_name )
        {
            ConfigurationName = configuration_name;
        }

        public BancaConfiguracionesException(string message, string configuration_name) : base(message)
        {
            ConfigurationName = configuration_name;
        }

        public BancaConfiguracionesException(string message, Exception innerException, string configuration_name) : base(message, innerException)
        {
            ConfigurationName = configuration_name;
        }
    }
}
