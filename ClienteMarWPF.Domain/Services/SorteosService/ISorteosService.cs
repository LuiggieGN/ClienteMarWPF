using HaciendaService;
using MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.Domain.Services.SorteosService
{
    public interface ISorteosService
    {
        MAR_HaciendaResponse GetSorteosDisponibles(HaciendaService.MAR_Session session);
        MAR_MultiBet RealizarMultiApuesta(MarPuntoVentaServiceReference.MAR_Session session, MAR_MultiBet Apuestas);
        void ConfirmarMultiApuesta(MarPuntoVentaServiceReference.MAR_Session session, ArrayOfInt tickets);
        MAR_Bet RealizarApuesta(MarPuntoVentaServiceReference.MAR_Session session, MAR_Bet Apuesta, double Solicitud, bool ParaPasar);
        void ConfirmarApuesta(MarPuntoVentaServiceReference.MAR_Session session);
    }
}
