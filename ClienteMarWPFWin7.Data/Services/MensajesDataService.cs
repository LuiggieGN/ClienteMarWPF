using ClienteMarWPFWin7.Data.Services.Helpers;
using ClienteMarWPFWin7.Domain.Services.MensajesService;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
 

namespace ClienteMarWPFWin7.Data.Services
{

    public class MensajesDataService: IMensajesService
    {
        private static SoapClientRepository SoapClientesRepository;
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
