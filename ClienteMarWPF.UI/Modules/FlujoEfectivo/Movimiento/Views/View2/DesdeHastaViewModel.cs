using System;
using System.Linq;
using System.Collections;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.ObjectModel;

using ClienteMarWPF.UI.State.Authenticators;

using ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPF.Domain.Services.TieService;
using ClienteMarWPF.Domain.Services.CajaService;


using ClienteMarWPF.UI.ViewModels;
using ClienteMarWPF.UI.ViewModels.Base;
using ClienteMarWPF.UI.ViewModels.Helpers;
using ClienteMarWPF.UI.ViewModels.Errors;
using ClienteMarWPF.UI.ViewModels.ModelObservable.ComboboxModels;
using ClienteMarWPF.UI.ViewModels.Commands.FlujoEfectivo.Movimiento.EnCajaView;
using ClienteMarWPF.UI.Modules.FlujoEfectivo.Movimiento.Views.View1.ViewModels;



namespace ClienteMarWPF.UI.Modules.FlujoEfectivo.Movimiento.Views.View2
{
    public class DesdeHastaViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        #region Fields
        private readonly ToastViewModel _toast;
        private readonly ErroresViewModel _errores;
        
        private ComboboxTransferir _comboTransferirSeleccion;
        private ObservableCollection<ComboboxTransferir> _comboTransferirOpciones; 

        private string _comentario; 
        private string _monto;

        private readonly IAuthenticator _aut;
        private readonly ICajaService _cajaService;
        #endregion

        #region Propiedades
        public ToastViewModel Toast => _toast;

        #region ComboTransferir
        public ComboboxTransferir ComboTransferirSeleccion
        {
            get => _comboTransferirSeleccion;
            set
            {
                _comboTransferirSeleccion = value; 
                NotifyPropertyChanged(nameof(ComboTransferirSeleccion),
                                      nameof(EsEntradaParaLaBanca),
                                      nameof(EsSalidaParaLaBanca));
            }
        }
        public ObservableCollection<ComboboxTransferir> ComboTransferirOpciones
        {
            get => _comboTransferirOpciones;
            set
            {
                _comboTransferirOpciones = value; NotifyPropertyChanged(nameof(ComboTransferirOpciones));
            }
        }
        #endregion  
        public bool EsEntradaParaLaBanca => ComboTransferirSeleccion.Key == 1 ? true : false;
        public bool EsSalidaParaLaBanca => !EsEntradaParaLaBanca;


        public string Comentario
        {
            get => _comentario;
            set
            {
                _comentario = value; NotifyPropertyChanged(nameof(Comentario));
            }
        }         
        public string Monto
        {
            get => _monto;
            set
            {
                _monto = value; NotifyPropertyChanged(nameof(Monto));
            }
        } 

        #endregion

        #region Propiedades Errors Handling 

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public ErroresViewModel Errores => _errores;
        public bool HasErrors => _errores?.HasErrors ?? false;
        public bool CanCreate => !HasErrors;
        
        #endregion

        #region Comandos

        public ICommand RestablecerCommand { get; }

        #endregion

        public DesdeHastaViewModel(IAuthenticator aut , ICajaService cajaService)
        {
            _toast = new ToastViewModel();
            _errores = new ErroresViewModel();
            _errores.ErrorsChanged += EnCambioDeErrores;

            _aut = aut; 
            _cajaService = cajaService;

            ComboTransferirOpciones = ComboboxTransferirHelper.LeerDefaults();
            ComboTransferirSeleccion = ComboTransferirOpciones.FirstOrDefault();

          
 


            _comentario = string.Empty; 

            _monto = string.Empty;




        }


        #region Metodos Errors Handling
        private void EnCambioDeErrores(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            NotifyPropertyChanged(nameof(CanCreate));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return _errores.GetErrors(propertyName);
        }
        #endregion



    } // fin de clase
}
