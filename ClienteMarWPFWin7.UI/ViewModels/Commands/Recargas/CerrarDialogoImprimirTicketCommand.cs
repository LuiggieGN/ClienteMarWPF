using ClienteMarWPFWin7.UI.Modules.Recargas.Modal;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPFWin7.UI.ViewModels.Commands.Recargas
{
    public class CerrarDialogoImprimirTicketCommand : ActionCommand
    {
        private readonly DialogImprimirTicketViewModel _viewModel;
        public CerrarDialogoImprimirTicketCommand(DialogImprimirTicketViewModel vieModel)
        {
            _viewModel = vieModel; SetAction(new Action<object>(CerrarDialogo));

        }



        public void CerrarDialogo(object parametro)
        {
            if(_viewModel != null)
            {
                _viewModel.Ocultar();
            }
           
        }
    }
}
