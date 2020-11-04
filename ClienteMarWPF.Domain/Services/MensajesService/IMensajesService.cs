using MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.Domain.Services.MensajesService
{
    public interface IMensajesService
    {
        public int SendMessage(MAR_Session session, string mensajes);
        public MAR_Mensajes GetMessages(MAR_Session session);

    }
}
