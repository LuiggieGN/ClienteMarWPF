using ClienteMarWPF.UI.Modules.Sorteos.Modal;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.UI.ViewModels.Commands.Sorteos
{
    public class CerrarValidarPagoTicketCommand: ActionCommand
    {
        private readonly ValidarPagoTicketViewModel _viewModel;
        public CerrarValidarPagoTicketCommand(ValidarPagoTicketViewModel vieModel)
        {
            _viewModel = vieModel; SetAction(new Action<object>(CerrarDialogo));

        }



        public void CerrarDialogo(object parametro)
        {
            if (_viewModel != null)
            {
                _viewModel.Ocultar();
            }

        }
    }
}
