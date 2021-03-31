using ClienteMarWPFWin7.Data;
using ClienteMarWPFWin7.Domain.Services.SorteosService;
using ClienteMarWPFWin7.UI.Modules.Sorteos;
using ClienteMarWPFWin7.UI.Modules.Sorteos.Modal;
using ClienteMarWPFWin7.UI.State.Authenticators;
using MAR.AppLogic.MARHelpers;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using ClienteMarWPFWin7.UI.Extensions;

namespace ClienteMarWPFWin7.UI.ViewModels.Commands.Sorteos
{
    public class GetListadoTicketsCommand: ActionCommand
    {
        private readonly SorteosViewModel ViewModel;
        private readonly ValidarPagoTicketViewModel ViewModelPago;
       private readonly ISorteosService SorteosService;
        private readonly IAuthenticator Autenticador;

        public GetListadoTicketsCommand(SorteosViewModel viewModel, IAuthenticator autenticador, ISorteosService sorteosService,ValidarPagoTicketViewModel viewModelValidar)
        {
            ViewModel = viewModel;
            Autenticador = autenticador;
            SorteosService = sorteosService;
            ViewModelPago = viewModelValidar;
           
            Action<object> comando = new Action<object>(GetListadoTickets);
            base.SetAction(comando);
        }


        private void GetListadoTickets(object parametro)
        {
            ViewModel.listaTicketsJugados.Clear();
            try
            {
                var sorteos = SessionGlobals.LoteriasTodas;

                if (ViewModel.ListadoTicketsPrecargados.Count == 0) { 



                    foreach (var item in sorteos)
                    {
                    
                        var result = SorteosService.ListaDeTicket(Autenticador.CurrentAccount.MAR_Setting2.Sesion, item.Numero, FechaHelper.FormatFecha(DateTime.Today, FechaHelper.FormatoEnum.FechaBasico));
                        if (result.Tickets != null)
                        {

                            var data = result.Tickets.OfType<MAR_Bet>();
                            

                            foreach (var ticket in data)
                            {
                                ViewModelPago.listaTicketsJugados.Add(ticket);
                                ViewModel.ListadoTicketsPrecargados.Add(ticket);
                            }
                        
                        }

                        //var data = result.Tickets.OfType<MAR_Bet>().ToList();

                        //foreach (var ticket in data)
                        //{
                        //    ViewModel.listaTicketsJugados.Add(ticket);
                        //}

                    }

                    ViewModelPago.listaTicketsJugados = ViewModelPago.listaTicketsJugados.OrderBy(x => x.Ticket).Reverse().ToList().ToObservableMarBet();
                    ViewModel.ListadoTicketsPrecargados =  ViewModel.ListadoTicketsPrecargados.OrderBy(x => x.Ticket).Reverse().ToList().ToObservableMarBet();
                    string total = ViewModelPago.listaTicketsJugados.Sum(x => x.Costo).ToString("C", CultureInfo.CurrentCulture);
                    ViewModelPago.TotalVentas = total;
                }
                else if (ViewModel.ListadoTicketsPrecargados.Count > 0)
                {
                    //var observable = new ObservableCollection<MAR_Bet>();
                    //foreach (var item in ViewModel.ListadoTicketsPrecargados.OrderBy(x => x.Ticket).Reverse())
                    //{
                    //    observable.Add(new MAR_Bet() {

                    //    });
                    //}
                    ViewModelPago.listaTicketsJugados = ViewModel.ListadoTicketsPrecargados.OrderBy(x => x.Ticket).Reverse().ToList().ToObservableMarBet();
                    string total = ViewModel.ListadoTicketsPrecargados.Sum(x => x.Costo).ToString("C", CultureInfo.CurrentCulture);
                    ViewModelPago.TotalVentas = total;



                }
            }
            catch (Exception)
            {

            }
        }


    }
}
