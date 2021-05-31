
 
using ClienteMarWPFWin7.Domain.Services.ServerService;
 

namespace ClienteMarWPFWin7.Data.Services
{

    public class ServerDataService : IServerService
    {
        public string LeerDominioPorDefecto()
        {
            string nombredeDominioPorDefecto = (LeerNombresDeDominios())[0];

            if (nombredeDominioPorDefecto == string.Empty)
            {
                return nombredeDominioPorDefecto;
            }

            try
            {
                string[] dominioYPuerto = MAR.AppLogic.Encryption.Encryptor.DecryptConfig(nombredeDominioPorDefecto).Split(':');
                return dominioYPuerto[0];
            }
            catch 
            {
                return string.Empty;
            }
        }

        public string[] LeerNombresDeDominios()
        {
            string[] hosts = MAR.Config.Reader.ReadStringArray(MAR.Config.ConfigEnums.AllowedWebHosts);

            if (hosts == null || hosts.Length == 0)
            {
                return new string[] { string.Empty };
            }

            return hosts;
        }






    }//fin de clase
}
