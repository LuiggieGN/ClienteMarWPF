
using ClienteMarWPF.Domain.Exceptions;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.State.Navigators;
using ClienteMarWPF.UI.State.LocalClientSetting;
using ClienteMarWPF.UI.Modules.Login;
using ClienteMarWPF.UI.ViewModels.Helpers;

using System;
using System.ComponentModel;

namespace ClienteMarWPF.UI.ViewModels.Commands.Login
{
    public class LoginCommand : ActionCommand
    {
        private readonly LoginViewModel viewmodellogin;
        private readonly IAuthenticator autenticador;
        private readonly IRenavigator navegaAppVistaInicial;
        private readonly ILocalClientSettingStore localclientsettings;
        private readonly BackgroundWorker worker = new BackgroundWorker();
        private bool loginSucces;

        public LoginCommand(LoginViewModel viewmodellogin, IAuthenticator autenticador, IRenavigator renavigator, ILocalClientSettingStore localclientsettings) : base()
        {
            this.viewmodellogin = viewmodellogin;
            this.autenticador = autenticador;
            this.navegaAppVistaInicial = renavigator;
            this.localclientsettings = localclientsettings;

            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;

            Action<object> comando = new Action<object>(IniciarSesion);
            Predicate<object> puede = new Predicate<object>(PuedeIniciarSesion);
            base.SetAction(comando,puede);
        }


        public bool PuedeIniciarSesion(object parametro) 
        {
            return viewmodellogin?.InicioPC?.EstaPCTienePermisoDeConexionAServicioDeMAR ?? false;
        
        }
        

        public void IniciarSesion(object password)
        {
            worker.RunWorkerAsync(argument: password);
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            loginSucces = false;
            viewmodellogin.ErrorMessage = string.Empty;
            viewmodellogin.Cargando = Booleano.Si;
            string password = e.Argument.ToString();

            try
            {
                localclientsettings.ReadDektopLocalSetting(); //@leo el archivo .ini que contiene el ipadress y el banca id
                autenticador.IniciarSesion(viewmodellogin.Username, $"{password}", localclientsettings.LocalClientSettings.BancaId, localclientsettings.LocalClientSettings.Direccion);
                viewmodellogin.Cargando = Booleano.No;
                loginSucces = true; 
            }
            catch (MarFileReadException ex)
            {
                loginSucces = false;
                viewmodellogin.Cargando = Booleano.No;
                viewmodellogin.ErrorMessage = ex.Message;
            }
            catch (UserNotFoundException ex)
            {
                loginSucces = false;
                viewmodellogin.Cargando = Booleano.No;
                viewmodellogin.ErrorMessage = ex.Message;
            }
            catch (BancaConfiguracionesException ex)
            {
                loginSucces = false;
                viewmodellogin.Cargando = Booleano.No;
                viewmodellogin.ErrorMessage = ex.Message;
            }
            catch (InvalidPasswordException)
            {
                loginSucces = false;
                viewmodellogin.Cargando = Booleano.No;
                viewmodellogin.ErrorMessage = "Credenciales inválidas";
            }
            catch (Exception)
            {
                loginSucces = false;
                viewmodellogin.Cargando = Booleano.No;
                viewmodellogin.ErrorMessage = "Verificar conexión de internet";
            }

        }

        private void worker_RunWorkerCompleted(object sender,
                                                   RunWorkerCompletedEventArgs e)
        {
            viewmodellogin.Cargando = Booleano.No;

            if (loginSucces && autenticador.CurrentAccount != null)
            { 
                autenticador.IsLoggedIn = true;
                navegaAppVistaInicial.Renavigate();
            }





        }





    }// fin de Clase LoginCommand

}// fin de namespace 
