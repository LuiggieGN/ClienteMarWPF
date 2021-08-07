using ClienteMarWPFWin7.Domain.Services.RecargaService;
using ClienteMarWPFWin7.UI.Modules.Recargas.Modal;
using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.UI.State.PinterConfig;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ClienteMarWPFWin7.UI.ViewModels.Commands.Recargas
{
    public class DialogConfirmTicketCommand : ActionCommand
    {
        private readonly DialogImprimirTicketViewModel _viewModel;
        private IRecargaService _recargaService;
        private IAuthenticator _autenticador;
        private RecargasIndexRecarga datosRecargas;
        public DialogConfirmTicketCommand(DialogImprimirTicketViewModel viewModel, IAuthenticator autenticador,  IRecargaService recargaService,RecargasIndexRecarga dataRecargas):base()
        {
            _viewModel = viewModel;
            _recargaService = recargaService;
            _autenticador = autenticador;
            datosRecargas = dataRecargas;

            var comando = new Action<object>(CrearTicket);
            base.SetAction(comando);
        }

        public void CrearTicket(object parametro)
        {
            try {
                _recargaService.ConfirmRecarga(_autenticador.CurrentAccount.MAR_Setting2.Sesion);
                List<string[]> impresionRecargas = PrintJobs.FromImprimirRecarga(datosRecargas, _autenticador);
                PrintOutHelper.SendToPrinter(impresionRecargas);
                //TicketTemplateHelper.PrintTicket(impresionRecargas);
                new CerrarDialogoImprimirTicketCommand(_viewModel);
                Thread.Sleep(700);
                _viewModel.CerrarDialogoInicioCommand.Execute(null);
               
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }
    }
}
