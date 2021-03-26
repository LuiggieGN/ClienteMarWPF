using ClienteMarWPFWin7.Domain.HaciendaService;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPFWin7.Domain.Services.SorteosService
{
    public interface ISorteosService
    {
        MAR_HaciendaResponse GetSorteosDisponibles(HaciendaService.MAR_Session session);
        MAR_MultiBet RealizarMultiApuesta(ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference.MAR_Session session, MAR_MultiBet Apuestas);
        void ConfirmarMultiApuesta(ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference.MAR_Session session, ArrayOfInt tickets);
        MAR_Bet RealizarApuesta(ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference.MAR_Session session, MAR_Bet Apuesta, double Solicitud, bool ParaPasar);
        void ConfirmarApuesta(ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference.MAR_Session session);
        MAR_Ganadores ListaDeTicket(ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference.MAR_Session session, int LoteriaID, string Fecha);
        MAR_Ganadores GetUltimosSorteos(ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference.MAR_Session session, int LoteriaID, string Fecha);
        MAR_ValWiner ConsultarTicket(ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference.MAR_Session session, string TicketNumero, string TicketPin, bool Pagar = false);
        string AnularTicket(ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference.MAR_Session session, string TicketNumero, string TicketPin);
        MAR_Bet ConsultarTicketSinPin(ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference.MAR_Session session, string TicketNumero);
        MAR_Bet ReimprimirTicket(ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference.MAR_Session session, int TicketPin);

    }
}
