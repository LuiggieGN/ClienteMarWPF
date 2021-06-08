using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPFWin7.Domain.Services.MensajesService
{
    public interface IMensajesService
    {
        int SendMessage(MAR_Session session, string mensajes);
        MAR_Mensajes2 GetMessages(MAR_Session session);

        MAR_Mensajes GetMessagesNotificacion(MAR_Session session);
    }
}
