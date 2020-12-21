
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPF.Domain.Services.MultipleService;
using ClienteMarWPF.Domain.Services.RutaService;

using ClienteMarWPF.UI.ViewModels;
using ClienteMarWPF.UI.ViewModels.Base;
using ClienteMarWPF.UI.ViewModels.Helpers;
using ClienteMarWPF.UI.State.Authenticators;

using System.Windows.Input;
using ClienteMarWPF.UI.ViewModels.Commands.FlujoEfectivo.Cuadre.CuadreLogin;

namespace ClienteMarWPF.UI.Modules.FlujoEfectivo.Cuadre.Windows.CuadreLogin
{
    public class CuadreLoginViewModel : BaseViewModel
    {
        #region Fields
        private readonly IAuthenticator _aut;
        private readonly IMultipleService _multipleService;
        private readonly IRutaService _rutaService;

        private bool _cuadreEsPermitido = false;
        private bool _habilitoBotones = true;
        private bool _necesitoTokenValidacion = false;
        private string _posicion;
        private string _token;
        private RutaAsignacionDTO _asignacion = null;
        private MultipleDTO<MUsuarioDTO, CajaDTO, TarjetaDTO> _gestor;
        #endregion

        #region Properties

        #region Error
        public MessageViewModel Error { get; }
        public string ErrorMessage
        {
            set => Error.Message = value;
        }
        #endregion

        public IAuthenticator AutService => _aut;
        public IMultipleService MultipleService => _multipleService;
        public IRutaService RutaService => _rutaService;

        public bool CuadreEsPermitido { get => _cuadreEsPermitido; set { _cuadreEsPermitido = value; NotifyPropertyChanged(nameof(CuadreEsPermitido)); } }
        public bool HabilitaBotones { get => _habilitoBotones; set { _habilitoBotones = value; NotifyPropertyChanged(nameof(HabilitaBotones)); } }
        public bool NecesitoTokenValidacion { get => _necesitoTokenValidacion; set { _necesitoTokenValidacion = value; NotifyPropertyChanged(nameof(NecesitoTokenValidacion)); } }

        public string Posicion { get => _posicion; set { _posicion = value; NotifyPropertyChanged(nameof(Posicion)); } }
        public string Token { get => _token; private set { _token = value; } }


        public MultipleDTO<MUsuarioDTO, CajaDTO, TarjetaDTO> Gestor { get => _gestor; set { _gestor = value; } }
        public RutaAsignacionDTO Asignacion { get => _asignacion; set { _asignacion = value; } }

        #endregion

        #region Commands
        public ICommand SeleccionarGestor { get; }
        public ICommand ValidarToken { get; }
        #endregion

        public CuadreLoginViewModel(IAuthenticator aut, IMultipleService multipleService, IRutaService rutaService)
        {
            Error = new MessageViewModel();

            _aut = aut;
            _multipleService = multipleService;
            _rutaService = rutaService;

            _cuadreEsPermitido = Booleano.No;
            _habilitoBotones = Booleano.Si;
            _necesitoTokenValidacion = Booleano.No;
            SetearTokenAValidar(string.Empty, string.Empty);
            _gestor = null;
            _asignacion = null;


            SeleccionarGestor = new SeleccionarGestorParaCuadreCommand(this);
            ValidarToken = new ValidarTokenChallengeCommand(this);
        }


        public void SetearTokenAValidar(string pos, string token)
        {
            Posicion = pos;
            Token = token;
        }

    }
}


