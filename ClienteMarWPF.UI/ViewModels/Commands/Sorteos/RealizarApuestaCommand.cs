using ClienteMarWPF.Domain.Services.SorteosService;
using ClienteMarWPF.UI.Modules.Sorteos;
using ClienteMarWPF.UI.State.Authenticators;
using MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.UI.ViewModels.Commands.Sorteos
{
    public class RealizarApuestaCommand: ActionCommand
    {
        private readonly SorteosViewModel ViewModel;
        private readonly ISorteosService SorteosService;
        private readonly IAuthenticator Autenticador;
        public RealizarApuestaCommand(SorteosViewModel viewModel, IAuthenticator autenticador, ISorteosService sorteosService)
        {
            ViewModel = viewModel;
            Autenticador = autenticador;
            SorteosService = sorteosService;

            Action<object> comando = new Action<object>(RealizarApuesta);
            base.SetAction(comando);
        }

        private void RealizarApuesta(object parametro)
        {
            var multi = new MAR_MultiBet();
            var headers = new MAR_BetHeader();
            var item = new MAR_BetItem();
           
            var prueba = SorteosService.RealizarApuesta(Autenticador.CurrentAccount.MAR_Setting2.Sesion, multi);

        }
    }
}
