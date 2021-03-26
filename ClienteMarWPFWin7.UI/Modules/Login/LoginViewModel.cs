﻿#region Namespaces

using ClienteMarWPFWin7.Domain.Services.BancaService;
using ClienteMarWPFWin7.Domain.Models.Dtos;

using ClienteMarWPFWin7.UI.ViewModels;
using ClienteMarWPFWin7.UI.ViewModels.Base;
using ClienteMarWPFWin7.UI.ViewModels.Commands.Login;
using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.UI.State.Navigators;
using ClienteMarWPFWin7.UI.State.LocalClientSetting;
using ClienteMarWPFWin7.UI.ViewModels.Helpers;

using System.Windows.Input;

#endregion

namespace ClienteMarWPFWin7.UI.Modules.Login
{
    public class LoginViewModel : BaseViewModel
    {

        #region Fields
        private string _username;
        private bool _cargando;
        #endregion

        #region Properties
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                NotifyPropertyChanged(nameof(Username));
            }
        }
        public bool Cargando
        {
            get
            {
                return _cargando;
            }
            set
            {
                _cargando = value;
                NotifyPropertyChanged(nameof(Cargando));
            }
        }
        public MessageViewModel ErrorMessageViewModel { get; }
        public string ErrorMessage
        {
            set => ErrorMessageViewModel.Message = value;
        }
        public InicioPCResultDTO InicioPC { get; }
        public IBancaService BancaServicio { get; }
        #endregion

        #region ICommand
        public ICommand LoginCommand { get; }
        #endregion


        public LoginViewModel(IAuthenticator autenticador,
                              IRenavigator renavigator,
                              ILocalClientSettingStore localclientsettings,
                              IBancaService bancaServicio,
                              InicioPCResultDTO inicioPC)
        {

            BancaServicio = bancaServicio;

            Cargando = Booleano.No;

            ErrorMessageViewModel = new MessageViewModel();
            
            InicioPC = inicioPC;
            
            LoginCommand = new LoginCommand(this, autenticador, renavigator, localclientsettings);

            if (!inicioPC.EstaPCTienePermisoDeConexionAServicioDeMAR)
            {
                ErrorMessage = "Su PC no esta autorizada. Comuniquese con la Central.";
            }

        }


        public void ReIniciarApp() 
        {
            if (InicioPC != null && ErrorMessageViewModel != null)
            {
                InicioPC.EstaPCTienePermisoDeConexionAServicioDeMAR = false;
                ErrorMessage = "Se requiere Re-Inicio para aplicar cambios.";
                NotifyPropertyChanged(nameof(ErrorMessage), nameof(InicioPC));
            }        
        }





    }
}
