using ClienteMarWPF.DataAccess.Services.Helpers;
using ClienteMarWPF.Domain.Services.MensajesService;
using MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.DataAccess.Services
{

    public class MensajesDataService: IMensajesService
    {
        public static SoapClientRepository SoapClientesRepository;
        private static PtoVtaSoapClient clientePuntoDeVenta;
        static MensajesDataService()
        {
            SoapClientesRepository = new SoapClientRepository();
            clientePuntoDeVenta = SoapClientesRepository.GetMarServiceClient(false);
        }

        public int SendMessage(MAR_Session session, string mensajes)
        {
            return clientePuntoDeVenta.PlaceMensaje(session, mensajes);
        }

        public MAR_Mensajes GetMessages(MAR_Session session)
        {
            var msj =  clientePuntoDeVenta.GetMensaje(session);
           // var msj2 =  clientePuntoDeVenta.GetMensaje2(session, true, session.Banca, true);
            return msj;
        }

    }
}
