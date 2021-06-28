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
using System.Windows;
using ClienteMarWPFWin7.Domain.Services.BancaService;
using ClienteMarWPFWin7.Domain.Models.Dtos;

namespace ClienteMarWPFWin7.UI.ViewModels.Commands.Sorteos
{
    public class GetListadoTicketsCommand: ActionCommand
    {
        private readonly SorteosViewModel ViewModel;
        private readonly ValidarPagoTicketViewModel ViewModelPago;
       private readonly ISorteosService SorteosService;
        private readonly IBancaService BancaService;
        private readonly IAuthenticator Autenticador;
        private int BusquedaTickets=0;

        public GetListadoTicketsCommand(SorteosViewModel viewModel, IAuthenticator autenticador, ISorteosService sorteosService,IBancaService bancaService, ValidarPagoTicketViewModel viewModelValidar)
        {
            ViewModel = viewModel;
            Autenticador = autenticador;
            SorteosService = sorteosService;
            ViewModelPago = viewModelValidar;
            BancaService = bancaService;
           
            Action<object> comando = new Action<object>(GetListadoTickets);
            base.SetAction(comando);
        }


        private void GetListadoTickets(object parametro)
        {
            ViewModel.listaTicketsJugados.Clear();
            try
            {
                var sorteos = SessionGlobals.LoteriasTodas;

                if (ViewModel.BuscarTicketsInService == false)
                {
                    ViewModel.ListadoTicketsPrecargados.Clear();
                    /*foreach (var item in sorteos)
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
                    }*/
                    var result = BancaService.LeerTicketsHoy(Autenticador.BancaConfiguracion.BancaDto.BancaID);

                    ViewModelPago.listaTicketsJugados = (ObservableCollection<TicketDTO>)ViewModelPago.listaTicketsJugados.OrderBy(x => x.Ticket);
                    ViewModel.ListadoTicketsPrecargados =(ObservableCollection<TicketDTO>) ViewModel.ListadoTicketsPrecargados.OrderBy(x => x.Ticket);
                    string total = ViewModelPago.listaTicketsJugados.Where(x => x.Nulo==false).Sum(x => x.Costo) .ToString("C", CultureInfo.CurrentCulture);
                    ViewModelPago.TotalVentas = total;
                    ViewModel.BuscarTicketsInService = true;
                }else if (ViewModel.ListadoTicketsPrecargados.Count == 0  && ViewModel.BuscarTicketsInService == true) {
                    
                    ViewModel.ListadoTicketsPrecargados.Clear();
                    foreach (var item in sorteos)
                    {
                        var result = BancaService.LeerTicketsHoy( Autenticador.BancaConfiguracion.BancaDto.BancaID);
                        if (result != null)
                        {
                            
                            var data = result.OfType<TicketDTO>();
                            
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
                    ViewModelPago.listaTicketsJugados = (ObservableCollection<TicketDTO>)ViewModelPago.listaTicketsJugados.OrderBy(x => x.Ticket);
                    ViewModel.ListadoTicketsPrecargados = (ObservableCollection<TicketDTO>)ViewModel.ListadoTicketsPrecargados.OrderBy(x => x.Ticket);
                    string total = ViewModelPago.listaTicketsJugados.Where(x => x.Nulo==false).Sum(x => x.Costo).ToString("C", CultureInfo.CurrentCulture);
                    ViewModelPago.TotalVentas = total;
                }
                else if (ViewModel.ListadoTicketsPrecargados.Count > 0 && ViewModel.BuscarTicketsInService == true)
                {
                    
                    ViewModelPago.listaTicketsJugados = (ObservableCollection<TicketDTO>) ViewModel.ListadoTicketsPrecargados.OrderBy(x => x.Ticket);
                    string total = ViewModel.ListadoTicketsPrecargados.Where(x => x.Nulo==false).Sum(x => x.Costo).ToString("C", CultureInfo.CurrentCulture);
                    ViewModelPago.TotalVentas = total;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }


    }
}
