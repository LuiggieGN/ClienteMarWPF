#region Namespaces
using System;
using System.Linq;
using System.Collections;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.ObjectModel;

using ClienteMarWPF.UI.State.Authenticators;

using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPF.Domain.Services.CajaService;
using ClienteMarWPF.Domain.Services.MultipleService;

using ClienteMarWPF.UI.ViewModels;
using ClienteMarWPF.UI.ViewModels.Base;
using ClienteMarWPF.UI.ViewModels.Helpers;
using ClienteMarWPF.UI.ViewModels.Errors;
using ClienteMarWPF.UI.ViewModels.ModelObservable.ComboboxModels;

using ClienteMarWPF.UI.Modules.FlujoEfectivo.Movimiento.Views.View1.ViewModels;
using ClienteMarWPF.UI.ViewModels.Commands.FlujoEfectivo.Movimiento.DesdeHastaView;
#endregion


namespace ClienteMarWPF.UI.Modules.FlujoEfectivo.Movimiento.Views.View2
{
    public class DesdeHastaViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        #region Fields
        private MovimientoViewModel _movimientoVm;
        private readonly ToastViewModel _toast;
        private readonly ErroresViewModel _errores;
        private readonly IAuthenticator _aut;
        private readonly ICajaService _cajaService;
        private readonly IMultipleService _multipleService;
        private ComboboxTransferir _comboTransferirSeleccion;
        private ObservableCollection<ComboboxTransferir> _comboTransferirOpciones;
        private string _pinGestor;
        private string _nomBanca;
        private string _nomGestor;
        private string _comentario;
        private InputCajeraViewModel _inputCajera;
        private string _monto;
        private MultipleDTO<MUsuarioDTO, CajaDTO, TarjetaDTO> _gestor;
        private bool _muestroBotonNuevaTrasferencia;
        private bool _muestroSeccionCajera;
        #endregion

        #region Propiedades
        public IAuthenticator Aut => _aut;
        public MovimientoViewModel MovimientoVm => _movimientoVm;
        public ToastViewModel Toast => _toast;

        #region Propiedades Errors Handling 
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public ErroresViewModel Errores => _errores;
        public bool HasErrors => _errores?.HasErrors ?? false;
        public bool CanCreate => !HasErrors;
        #endregion

        #region ComboTransferir
        public ComboboxTransferir ComboTransferirSeleccion
        {
            get => _comboTransferirSeleccion;
            set
            {
                _comboTransferirSeleccion = value;

                if (InputCajera != null)
                {
                    if (_comboTransferirSeleccion.Key == 1)
                    {
                        MuestroSeccionCajera = true;
                        InputCajera.Muestro = true;
                    }
                    else
                    {
                        MuestroSeccionCajera = false;
                        InputCajera.Muestro = false;
                    }
                }

                NotifyPropertyChanged(nameof(ComboTransferirSeleccion),
                                      nameof(EsEntradaParaLaBanca),
                                      nameof(EsSalidaParaLaBanca),
                                      nameof(InputCajera));
            }
        }
        public ObservableCollection<ComboboxTransferir> ComboTransferirOpciones
        {
            get => _comboTransferirOpciones;
            set
            {
                _comboTransferirOpciones = value;
                NotifyPropertyChanged(nameof(ComboTransferirOpciones));
            }
        }
        #endregion
        public bool EsEntradaParaLaBanca => ComboTransferirSeleccion.Key == 1 ? true : false;
        public bool EsSalidaParaLaBanca => !EsEntradaParaLaBanca;
        public string PinGestor
        {
            get => _pinGestor;
            set
            {
                _pinGestor = value; NotifyPropertyChanged(nameof(PinGestor));
            }
        }
        public string NomBanca
        {
            get => _nomBanca;
            set
            {
                _nomBanca = value; NotifyPropertyChanged(nameof(NomBanca));
            }
        }
        public string NomGestor
        {
            get => _nomGestor;
            set
            {
                _nomGestor = value; NotifyPropertyChanged(nameof(NomGestor));
            }
        }
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
        public MultipleDTO<MUsuarioDTO, CajaDTO, TarjetaDTO> Gestor
        {
            get => _gestor;
            set
            {
                _gestor = value;
            }
        }
        public bool MuestroBotonNuevaTrasferencia
        {
            get => _muestroBotonNuevaTrasferencia;
            set
            {
                _muestroBotonNuevaTrasferencia = value; NotifyPropertyChanged(nameof(MuestroBotonNuevaTrasferencia));
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

        #endregion

        #region Comandos
        public ICommand SeleccionarGestorCommand { get; }
        public ICommand AbrirModalTokenCommand { get; }
        public ICommand RestablecerCommand { get; }
        #endregion

        public DesdeHastaViewModel(MovimientoViewModel movimientoVm, IAuthenticator aut, ICajaService cajaService, IMultipleService multipleService)
        {
            _toast = new ToastViewModel();

            _errores = new ErroresViewModel();
            _errores.ErrorsChanged += EnCambioDeErrores;

            _movimientoVm = movimientoVm;

            _aut = aut;
            _cajaService = cajaService;
            _multipleService = multipleService;

            ComboTransferirOpciones = ComboboxTransferirHelper.LeerDefaults();
            ComboTransferirSeleccion = ComboTransferirOpciones.FirstOrDefault();

            _pinGestor = string.Empty;

            _nomBanca = _aut?.BancaConfiguracion?.BancaDto?.BanContacto;

            _nomGestor = "-- Seleccionar Gestor --";

            _comentario = string.Empty;

            _muestroSeccionCajera = true;
            _inputCajera = new InputCajeraViewModel() { Cajera = string.Empty, Muestro = true };

            _monto = string.Empty;

            _muestroBotonNuevaTrasferencia = false;

            SeleccionarGestorCommand = new SeleccionarGestorCommand(this, _multipleService);

            AbrirModalTokenCommand = new AbrirModalTokenCommand(this, _cajaService);

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
