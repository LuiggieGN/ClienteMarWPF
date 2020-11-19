using ClienteMarWPF.Domain.Services.SorteosService;
using ClienteMarWPF.UI.Modules.Sorteos;
using ClienteMarWPF.UI.State.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.UI.ViewModels.Commands.Sorteos
{
    public class GetSorteosCommand: ActionCommand
    {
        private readonly SorteosViewModel ViewModel;
        private readonly ISorteosService SorteosService;
        private readonly IAuthenticator Autenticador;
        public GetSorteosCommand(SorteosViewModel viewModel, IAuthenticator autenticador, ISorteosService sorteosService)
        {
            ViewModel = viewModel;
            Autenticador = autenticador;
            SorteosService = sorteosService;

            Action<object> comando = new Action<object>(getSorteos);
            base.SetAction(comando);
        }

        private void getSorteos(object parametro)
        {
            HaciendaService.MAR_Session sessionHacienda = new HaciendaService.MAR_Session();
            MarPuntoVentaServiceReference.MAR_Session sessionPuntoVenta = Autenticador.CurrentAccount.MAR_Setting2.Sesion;
            sessionHacienda.Banca = sessionPuntoVenta.Banca;
            sessionHacienda.Usuario = sessionPuntoVenta.Usuario;
            sessionHacienda.Sesion = sessionPuntoVenta.Sesion;
            sessionHacienda.Err = sessionPuntoVenta.Err;
            sessionHacienda.LastTck = sessionPuntoVenta.LastTck;
            sessionHacienda.LastPin = sessionPuntoVenta.LastPin;
            sessionHacienda.PrinterSize = sessionPuntoVenta.PrinterSize;
            sessionHacienda.PrinterHeader = sessionPuntoVenta.PrinterHeader;
            sessionHacienda.PrinterFooter = sessionPuntoVenta.PrinterFooter;

            var sorteos = SorteosService.GetSorteosDisponibles(sessionHacienda);

        }
    }
}
