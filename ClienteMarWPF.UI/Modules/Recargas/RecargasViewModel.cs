using ClienteMarWPF.Domain.Services.RecargaService;
using ClienteMarWPF.UI.Modules.Recargas.Modal;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.State.Navigators;
using ClienteMarWPF.UI.ViewModels;
using ClienteMarWPF.UI.ViewModels.Base;
using ClienteMarWPF.UI.ViewModels.Commands.Recargas;
using ClienteMarWPF.UI.ViewModels.Factories;
using ClienteMarWPF.UI.ViewModels.ModelObservable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace ClienteMarWPF.UI.Modules.Recargas
{
    public class RecargasViewModel : BaseViewModel
    {


        public ICommand GetSuplidoresCommand { get; }
        public ICommand SendRecargaCommand { get; }



        private string monto;
        private string telefono;
        private int cheked;
        private DialogImprimirTicketViewModel _dialog;

        public MessageViewModel ErrorMessageViewModel { get; }

        public string ErrorMessage
        {
            set => ErrorMessageViewModel.Message = value;
        }

        public List<ProveedorRecargasObservable> ProveedorRecargasObservable = new List<ProveedorRecargasObservable>();




        public RecargasViewModel(IAuthenticator autenticador, IRecargaService recargaService,INavigator nav, IViewModelFactory vistas)
        {
            ErrorMessageViewModel = new MessageViewModel();
            GetSuplidoresCommand = new GetSuplidoresCommand(this, autenticador, recargaService);
            SendRecargaCommand = new SendRecargaCommand(this, autenticador, nav, vistas, recargaService);
            GetSuplidoresCommand.Execute(null);
            //ProveedorRecargasObservable = new List<ProveedorRecargasObservable>();
        }

        public List<ProveedorRecargasObservable> _proveedorRecargasObservable;


        public DialogImprimirTicketViewModel Dialog { 
            get 
            {
                return _dialog;
            } 
            set 
            {
                _dialog = value; NotifyPropertyChanged(nameof(Dialog));
            } 
        }


        public List<ProveedorRecargasObservable> Proveedores
        {
            get
            {
                return ProveedorRecargasObservable;
            }

        }


        private ProveedorRecargasObservable _provedor;
        public ProveedorRecargasObservable Provedor
        {
            get => _provedor;
            set
            {
                _provedor = value; NotifyPropertyChanged(nameof(Provedor));
            }
        }

        public string Monto {
            get 
            {
                return monto;
            }

            set 
            {
                monto = value;
                NotifyPropertyChanged(nameof(Monto));
            }
        }


        public string Telefono
        {
            get
            {

                return telefono;
            }

            set
            {
                telefono = value;
                NotifyPropertyChanged(nameof(Telefono));
            }
        }


      
        



    }



}
