using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MAR.BusinessLogic.Code.PrintJobs;
using MAR.DataAccess.Tables.DTOs;
using MAR.DataAccess.Tables.Enums;
using MAR.DataAccess.Tables.ViewModels;
using MAR.DataAccess.UnitOfWork;
using Newtonsoft.Json;
using MAR.DataAccess.ViewModels;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;



namespace MAR.BusinessLogic.Code
{
    public class SeguridadLogic
    {
        public static dynamic GetSession_ParaWinBrowser(int bancaId, int sessionId, string llave)
        {
            if (llave != null && llave.Length == 10 && bancaId > 0 && sessionId > 0)
            {
                var seed1 = llave.Substring(0, 5);
                var seed2 = (((int.Parse(seed1) + sessionId) * bancaId) % 100000).ToString().PadLeft(5, '0');
                if (llave == seed1 + seed2)
                {
                    var banca = DataAccess.EFRepositories.SessionRepository.GetSesionValue(bancaId, sessionId);
                    if (banca != null)
                    {
                        dynamic info = new System.Dynamic.ExpandoObject();
                        info.LastPin = banca.LastPin;
                        info.LastTck = banca.LastTicket;
                        info.UsuarioActual = banca.UsuarioActual;
                        return info;
                    }
                }
            }
            return null;
        }
        public static object GetServerInfo()
        {
            //Get app server id
            String idAppServer = "";
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                idAppServer = host.HostName + " " + host.AddressList[1];
                return idAppServer;
            }
            catch(Exception e)
            {
                idAppServer = e.Message;
                return "Fallo GetServerInfo()";
            }
        }
    }

}
