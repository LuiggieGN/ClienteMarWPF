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



namespace ClienteMarWPF.UI.Modules.FlujoEfectivo.Movimiento.Views.View1
{
    public class EnCajaViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        #region Fields

        private readonly ToastViewModel _toast;
        private readonly ErroresViewModel _errores;

        private string _nombreBanca;
        private string _comentario; 
        private string _monto;

        private InputConceptoViewModel _inputConcepto;
        private InputCajeraViewModel _inputCajera;

        private ComboboxQueHaras _comboESSeleccion;
        private ObservableCollection<ComboboxQueHaras> _comboESOpciones;

        private ComboboxConcepto _comboConceptoSeleccion;
        private ObservableCollection<ComboboxConcepto> _comboConceptoOpciones;

        private readonly IAuthenticator _aut;
        private readonly ITieService _tieService;
        private readonly ICajaService _cajaService;
        private readonly TieDTO _anonimos;

        private bool _muestroSeccionCajera;

        #endregion

        #region Propiedades

        public TieDTO Anonimos => _anonimos;
        public string NombreBanca
        {
            get => _nombreBanca;
            private set
            {
                _nombreBanca = value; NotifyPropertyChanged(nameof(NombreBanca));
            }
        }

        #region ComboES
        public ComboboxQueHaras ComboESSeleccion
        {
            get => _comboESSeleccion;
            set
            {
                _comboESSeleccion = value; NotifyPropertyChanged(nameof(ComboESSeleccion));
            }
        }
        public ObservableCollection<ComboboxQueHaras> ComboESOpciones
        {
            get => _comboESOpciones;
            set
            {
                _comboESOpciones = value; NotifyPropertyChanged(nameof(ComboESOpciones));
            }
        }
        #endregion

        #region ComboConcepto
        public ComboboxConcepto ComboConceptoSeleccion
        {
            get => _comboConceptoSeleccion;
            set
            {
                _comboConceptoSeleccion = value; NotifyPropertyChanged(nameof(ComboConceptoSeleccion));
            }
        }
        public ObservableCollection<ComboboxConcepto> ComboConceptoOpciones
        {
            get => _comboConceptoOpciones;
            set
            {
                _comboConceptoOpciones = value; NotifyPropertyChanged(nameof(ComboConceptoOpciones));
            }
        }
        #endregion

        #region InputConcepto
        public InputConceptoViewModel InputConcepto
        {
            get => _inputConcepto;
            set
            {
                _inputConcepto = value; NotifyPropertyChanged(nameof(InputConcepto));
            }
        }
        #endregion

        public string Comentario
        {
            get => _comentario;
            set
            {
                _comentario = value; NotifyPropertyChanged(nameof(Comentario));
            }
        }

        #region InputCajera
        public InputCajeraViewModel InputCajera
        {
            get => _inputCajera;
            set
            {
                _inputCajera = value; NotifyPropertyChanged(nameof(InputCajera));
            }
        }
        #endregion
        
        public string Monto
        {
            get => _monto;
            set
            {
                _monto = value; NotifyPropertyChanged(nameof(Monto));
            }
        }

        public bool MuestroSeccionCajera
        {
            get => _muestroSeccionCajera;
            set
            {
                _muestroSeccionCajera = value; NotifyPropertyChanged(nameof(MuestroSeccionCajera));
            }
        }

        public ToastViewModel Toast => _toast;


        #endregion

        #region Propiedades Errors Handling 

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public ErroresViewModel Errores => _errores;
        public bool HasErrors => _errores?.HasErrors ?? false;
        public bool CanCreate => !HasErrors;
        
        #endregion

        #region Comandos

        public ICommand SeleccionarAnonimosCommand { get; }
        public ICommand SeleccionarConceptoCommand { get; }
        public ICommand AgregarMovimientoEnCajaCommand { get; }
        public ICommand RestablecerCommand { get; }

        #endregion

        public EnCajaViewModel(IAuthenticator aut, ITieService tieService, ICajaService cajaService)
        {

            _toast = new ToastViewModel();
            _aut = aut;
            _tieService = tieService;
            _cajaService = cajaService;

            _anonimos = tieService.LeerTiposAnonimos();

            _nombreBanca = _aut?.BancaConfiguracion?.BancaDto?.BanContacto ?? "...";

            _comboESOpciones = ComboboxQueHarasHelper.LeerOpcionesDefaultDeCombobox();
            _comboESSeleccion = ComboESOpciones.FirstOrDefault();

            _inputCajera = new InputCajeraViewModel() { Cajera = string.Empty, Muestro = true };

            SeleccionarAnonimosCommand = new SeleccionarAnonimosCommand(this);
            SeleccionarAnonimosCommand.Execute(null);
            SeleccionarConceptoCommand = new SeleccionarConceptoCommand(this);

            _comentario = string.Empty; 

            _monto = string.Empty;

            _errores = new ErroresViewModel();
            _errores.ErrorsChanged += EnCambioDeErrores;

            AgregarMovimientoEnCajaCommand = new AgregarMovimientoEnCajaCommand(this,_aut,_cajaService);

            RestablecerCommand = new RestablecerCommand(this);


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
