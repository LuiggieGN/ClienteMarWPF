using ClienteMarWPFWin7.Domain.Services.RecargaService;
using ClienteMarWPFWin7.UI.Modules.Recargas.Modal;
using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.UI.State.Navigators;
using ClienteMarWPFWin7.UI.ViewModels;
using ClienteMarWPFWin7.UI.ViewModels.Base;
using ClienteMarWPFWin7.UI.ViewModels.Commands.Recargas;
using ClienteMarWPFWin7.UI.ViewModels.Factories;
using ClienteMarWPFWin7.UI.ViewModels.ModelObservable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace ClienteMarWPFWin7.UI.Modules.Recargas
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
                var valor = value;
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

        public void SetProveedorFromkeyDown(int proveedorID)
        {
            for (int i = 0; i < Proveedores.Count; i++)
            {
                if(proveedorID == Proveedores[i].OperadorID)
                {
                    Proveedores[i].IsSelected = true;
                }
                else
                {
                    Proveedores[i].IsSelected = false;
                }

                

            }

            NotifyPropertyChanged(nameof(Proveedores));
        }


      
        



    }



}
