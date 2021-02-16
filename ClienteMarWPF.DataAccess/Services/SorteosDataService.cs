using ClienteMarWPF.DataAccess.Services.Helpers;
using ClienteMarWPF.Domain.Services.SorteosService;
using HaciendaService;
using MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.DataAccess.Services
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

        public MAR_HaciendaResponse GetSorteosDisponibles(HaciendaService.MAR_Session session)
        {

            try
            {
                ArrayOfAnyType Params = new ArrayOfAnyType();
                return haciendacliente.CallHaciendaFuncion(21, session, Params);
            }
            catch (Exception)
            {
                return new MAR_HaciendaResponse();
            }

        }
        public MAR_MultiBet RealizarMultiApuesta(MarPuntoVentaServiceReference.MAR_Session session, MAR_MultiBet Apuestas)
        {
            try
            {
                return clientePuntoDeVenta.PlaceMultiBet(session, Apuestas);

            }
            catch (Exception)
            {
                return new MAR_MultiBet();
            }
        }
        public void ConfirmarMultiApuesta(MarPuntoVentaServiceReference.MAR_Session session, ArrayOfInt tickets)
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
        public MAR_Bet RealizarApuesta(MarPuntoVentaServiceReference.MAR_Session session, MAR_Bet Apuesta, double Solicitud, bool ParaPasar = false)
        {
            try
            {
                return clientePuntoDeVenta.PlaceBet(session, Apuesta, Solicitud, ParaPasar);
            }
            catch (Exception)
            {
                return new MAR_Bet();
            }
        }
        public void ConfirmarApuesta(MarPuntoVentaServiceReference.MAR_Session session)
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
        public MAR_Ganadores ListaDeTicket(MarPuntoVentaServiceReference.MAR_Session session, int LoteriaID, string Fecha)
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
        public MAR_Ganadores GetUltimosSorteos(MarPuntoVentaServiceReference.MAR_Session session, int LoteriaID, string Fecha)
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
        public MAR_ValWiner ConsultarTicket(MarPuntoVentaServiceReference.MAR_Session session, string TicketNumero, string TicketPin, bool Pagar = false)
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
        public string AnularTicket(MarPuntoVentaServiceReference.MAR_Session session, string TicketNumero, string TicketPin)
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
    }
}
