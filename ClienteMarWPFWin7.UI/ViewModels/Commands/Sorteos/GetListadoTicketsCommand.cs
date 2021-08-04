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

            base.SetAction(new Action<object>(GetListadoTickets));
        }


        private void GetListadoTickets(object parametro)
        {
            ViewModel.listaTicketsJugados.Clear();
            try
            {
                var result = BancaService.LeerTicketsHoy(Autenticador.BancaConfiguracion.BancaDto.BancaID);

                if (result != null && result.Count > 0)
                {
                    ViewModelPago.listaTicketsJugados = result.OrderBy(x => x.Ticket).Reverse().ToList().ToObservableTicketDTO();

                    ViewModel.ListadoTicketsPrecargados = result.OrderBy(x => x.Ticket).Reverse().ToList().ToObservableTicketDTO();

                    string total = ViewModelPago.listaTicketsJugados.Where(x => x.Nulo == false).Sum(x => x.Costo).ToString("C", new CultureInfo("EN-US"));

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
