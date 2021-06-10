using ClienteMarWPFWin7.Data.Services.Helpers;
using ClienteMarWPFWin7.Domain.Services.SorteosService;
using ClienteMarWPFWin7.Domain.HaciendaService;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPFWin7.Data.Services
{
    public class SorteosDataService: ISorteosService
    {
        private static SoapClientRepository SoapClientesRepository;
        private static mar_haciendaSoapClient haciendacliente;
        private static PtoVtaSoapClient clientePuntoDeVenta;

        static SorteosDataService()
        {
            SoapClientesRepository = new SoapClientRepository();
            haciendacliente = SoapClientesRepository.GetHaciendaServiceClient(false);
            clientePuntoDeVenta = SoapClientesRepository.GetMarServiceClient(false);
        }

        public MAR_HaciendaResponse GetSorteosDisponibles(ClienteMarWPFWin7.Domain.HaciendaService.MAR_Session session)
        {

            try
            {
                ArrayOfAnyType Params = new ArrayOfAnyType();
                return haciendacliente.CallHaciendaFuncion(21, session, Params);
            }
            catch (Exception)
            {
                var errResponse = new MAR_HaciendaResponse();
                errResponse.OK = false;
                return errResponse;
            }

        }
        public MAR_MultiBet RealizarMultiApuesta(ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference.MAR_Session session, MAR_MultiBet Apuestas)
        {
            try
            {

                var response = clientePuntoDeVenta.PlaceMultiBet(session, Apuestas);
                return response;

            }
            catch (Exception)
            {
                return new MAR_MultiBet();
            }
        }
        public void ConfirmarMultiApuesta(ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference.MAR_Session session, ArrayOfInt tickets)
        {

            try
            {
                clientePuntoDeVenta.ConfirmMultiTck(session, tickets);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public MAR_Bet RealizarApuesta(ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference.MAR_Session session, MAR_Bet Apuesta, double Solicitud, bool ParaPasar = false)
        {
            try
            {
                var response = clientePuntoDeVenta.PlaceBet(session, Apuesta, Solicitud, ParaPasar);
                return response;

            }
            catch (Exception)
            {
                return new MAR_Bet();
            }
        }
        public void ConfirmarApuesta(ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference.MAR_Session session)
        {
            try
            {
                clientePuntoDeVenta.ConfirmTck(session);
            }
            catch (Exception)
            {
                throw;
            }
            
        }
        public MAR_Ganadores ListaDeTicket(ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference.MAR_Session session, int LoteriaID, string Fecha)
        {
            try
            {
                return clientePuntoDeVenta.ListaTickets(session, LoteriaID, Fecha);
            }
            catch (Exception)
            {

                return new MAR_Ganadores();
            }
        }
        public MAR_Ganadores Ganadores3(ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference.MAR_Session session, int LoteriaID, string Fecha)
        {
            try
            {
                return clientePuntoDeVenta.Ganadores3(session, LoteriaID, Fecha);
            }
            catch (Exception)
            {
                return new MAR_Ganadores();
            }
            
        }
        public MAR_ValWiner ConsultarTicket(ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference.MAR_Session session, string TicketNumero, string TicketPin, bool Pagar = false)
        {
            try
            {
                return clientePuntoDeVenta.ValidWinner(session, TicketNumero, TicketPin, Pagar);
            }
            catch (Exception)
            {
                var invalid = new MAR_ValWiner();

                invalid.Aprobado = -1;
                invalid.Mensaje = "Ha ocurrido un error al procesar la operaciòn";
                return invalid;
            }
        }
        public MAR_Bet ConsultarTicketSinPin(ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference.MAR_Session session, string TicketNumero)
        {
            try
            {
                return clientePuntoDeVenta.GetBet(session,TicketNumero);
            }
            catch (Exception e)
            {
                var invalid = new MAR_Bet();

                invalid.Err = e.Message;
                return invalid;
            }
        }

        public string AnularTicket(ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference.MAR_Session session, string TicketNumero, string TicketPin)
        {
            try
            {
                return clientePuntoDeVenta.Anula2(session, TicketNumero, TicketPin);
            }
            catch (Exception)
            {
                return string.Empty;
                
            }
        }

         public MAR_Bet ReimprimirTicket(ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference.MAR_Session session, int TicketID)
        {
            try
            {
                return clientePuntoDeVenta.RePrint(session, TicketID);

            }
            catch( Exception)
            {
                return new MAR_Bet();
            }
        }
    }
}
