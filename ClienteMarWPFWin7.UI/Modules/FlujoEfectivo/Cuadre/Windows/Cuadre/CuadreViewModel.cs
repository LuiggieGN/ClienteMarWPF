
using ClienteMarWPFWin7.Domain.Enums;
using ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPFWin7.UI.ViewModels.Base;
using ClienteMarWPFWin7.UI.ViewModels.Errors;
using ClienteMarWPFWin7.UI.State.Accounts;
using ClienteMarWPFWin7.UI.State.CuadreBuilders;
using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.UI.ViewModels.Helpers;
using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Cuadre.Windows.Cuadre.ViewModels;
using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Cuadre.Windows.Cuadre.Modal;
using ClienteMarWPFWin7.UI.ViewModels.Commands.FlujoEfectivo.Cuadre.Cuadre;
using System.ComponentModel;
using System.Collections;
using System;
using System.Windows.Input;
using System.Globalization;


namespace ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Cuadre.Windows.Cuadre
{
    public class CuadreViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        #region Fields
        private bool _habilitarBotones;
        private ConsultaInicialViewModel _consultaInicial;
        private string _montoContado;
        private ArqueoResultanteViewModel _arqueoResultante;
        private RecomendacionViewModel _recomendacion;
        private CuadreGestorAccion _cuadreGestorAccion = CuadreGestorAccion.Depositar;
        private string _montoDepositoORetiro;
        private string _nombreCajera;
        private DialogoTokenViewModel _dialog;
        #endregion

        #region Properties                
        public IAuthenticator AutService { get; }
        public GestorStore GestorStored { get; }
        public ICuadreBuilder CuadreBuilder { get; }
        public string Titulo { get; }
        public bool HabilitarBotones
        {
            get => _habilitarBotones;
            set { _habilitarBotones = value; NotifyPropertyChanged(nameof(HabilitarBotones)); }
        }
        public string GestorNombre { get; }
        public string BancaNombre { get; }
        public ConsultaInicialViewModel ConsultaInicial
        {
            get => _consultaInicial;
            set { _consultaInicial = value; NotifyPropertyChanged(nameof(ConsultaInicial)); }
        }
        public string MontoContado
        {
            get => _montoContado;
            set
            {
                _montoContado = value;
                NotifyPropertyChanged(nameof(MontoContado));
                OnMontoContatoChanged();
            }
        }
        public ArqueoResultanteViewModel ArqueoResultante
        {
            get => _arqueoResultante;
            set { _arqueoResultante = value; NotifyPropertyChanged(nameof(ArqueoResultante)); }
        }
        public RecomendacionViewModel Recomendacion
        {
            get => _recomendacion;
            set { _recomendacion = value; NotifyPropertyChanged(nameof(Recomendacion)); }
        }
        public CuadreGestorAccion CuadreGestorAccion
        {
            get => _cuadreGestorAccion;
            set
            {
                _cuadreGestorAccion = value;
                NotifyPropertyChanged(nameof(CuadreGestorAccion));
            }
        }
        public string MontoDepositoORetiro
        {
            get => _montoDepositoORetiro;
            set { _montoDepositoORetiro = value; NotifyPropertyChanged(nameof(MontoDepositoORetiro)); }
        }
        public string NombreCajera
        {
            get => _nombreCajera;
            set { _nombreCajera = value; NotifyPropertyChanged(nameof(NombreCajera)); }
        }
        public DialogoTokenViewModel Dialog
        {
            get => _dialog;           
            set
            {
                _dialog = value; NotifyPropertyChanged(nameof(Dialog));
            }
        }
        #endregion

        #region Propeties for errors handling 
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public ErroresViewModel Errores { get; }
        public bool HasErrors => Errores?.HasErrors ?? false;
        public bool CanCreate => !HasErrors;
        #endregion

        #region ICommand
        public ICommand InitCuadreCommand { get; }
        public ICommand SetRecomendacionCommand { get; set; }
        public ICommand AbrirDesgloseCommand { get; }
        public ICommand RegistrarCuadreCommand { get; }
        #endregion

        public CuadreViewModel(IAuthenticator aut, GestorStore gestorStore, ICuadreBuilder cuadreBuilder)
        {

            cuadreBuilder.SetearCajaDisponibilidad(new CajaDisponibilidadDTO() { Cajaid = null, Bancaid = aut.BancaConfiguracion.BancaDto.BancaID, Disponibilidad = false });

            Errores = new ErroresViewModel();
            Errores.ErrorsChanged += OnErrors;

            AutService = aut;
            GestorStored = gestorStore;
            CuadreBuilder = cuadreBuilder;

            HabilitarBotones = Booleano.No;

            GestorNombre = (gestorStore?.GestorSesion?.Gestor?.PrimerDTO?.UsuNombre ?? string.Empty) + " " + (gestorStore?.GestorSesion?.Gestor?.PrimerDTO?.UsuApellido ?? string.Empty);

            Titulo = $" Bienvenido, Gestor : {GestorNombre}";

            BancaNombre = aut?.BancaConfiguracion?.BancaDto?.BanContacto ?? "...";

            ConsultaInicial = new ConsultaInicialViewModel();

            MontoContado = string.Empty;

            ArqueoResultante = new ArqueoResultanteViewModel();

            Recomendacion = new RecomendacionViewModel();

            //CuadreGestorAccion = CuadreGestorAccion.None;

            MontoDepositoORetiro = string.Empty;

            NombreCajera = string.Empty;

            // ||SetRecomendacionCommand||  este comando se setea cuando se ejecuta InitCuadreCommand

            InitCuadreCommand = new InitCuadreCommand(this);
            InitCuadreCommand.Execute(null);

            AbrirDesgloseCommand = new AbrirDesgloseCommand(this);

            RegistrarCuadreCommand = new RegistrarCuadreCommand(this);
        }






        #region Method for errors handling
        private void OnErrors(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            NotifyPropertyChanged(nameof(CanCreate));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return Errores.GetErrors(propertyName);
        }
        #endregion

        private void OnMontoContatoChanged()
        {
            decimal monto = 0;

            if (InputHelper.InputIsBlank(this.MontoContado))
            {
                monto = 0;
            }
            else
            {
                bool noEsValido = !decimal.TryParse(this.MontoContado, NumberStyles.Any, new CultureInfo("en-US"), out monto);

                if (noEsValido || monto <= 0)
                {
                    monto = 0;
                }
                else
                {
                    SetRecomendacionCommand?.Execute(monto);
                }
            }
        }//fin de metodo








    }
}
