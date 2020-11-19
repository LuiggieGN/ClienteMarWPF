using ClienteMarWPF.Domain.Services.RecargaService;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.ViewModels.Base;
using ClienteMarWPF.UI.ViewModels.Commands.Recargas;
using ClienteMarWPF.UI.ViewModels.ModelObservable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace ClienteMarWPF.UI.Modules.Recargas
{
    public class RecargasViewModel: BaseViewModel
    {


        public ICommand GetSuplidoresCommand { get; }

        public List<ProveedorRecargasObservable> ProveedorRecargasObservable = new List<ProveedorRecargasObservable>();
        public RecargasViewModel(IAuthenticator autenticador, IRecargaService  recargaService)
        {
            GetSuplidoresCommand = new GetSuplidoresCommand(this, autenticador, recargaService);
             GetSuplidoresCommand.Execute(null);
            //ProveedorRecargasObservable = new List<ProveedorRecargasObservable>();
        }
    }
}
