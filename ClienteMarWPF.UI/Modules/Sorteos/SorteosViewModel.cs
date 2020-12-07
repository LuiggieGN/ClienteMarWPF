using ClienteMarWPF.Domain.Services.SorteosService;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.ViewModels.Base;
using ClienteMarWPF.UI.ViewModels.Commands.Sorteos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ClienteMarWPF.UI.Modules.Sorteos
{
    public class SorteosViewModel: BaseViewModel
    {
        public ICommand RealizarApuestaCommand { get; }

        public SorteosViewModel(IAuthenticator autenticador, ISorteosService sorteosService)
        {
            RealizarApuestaCommand = new RealizarApuestaCommand(this, autenticador, sorteosService);

        }


    }
}
