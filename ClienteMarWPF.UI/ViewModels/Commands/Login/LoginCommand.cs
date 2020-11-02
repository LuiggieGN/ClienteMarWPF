
using ClienteMarWPF.Domain.Exceptions;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.State.Navigators;
using ClienteMarWPF.UI.State.LocalClientSetting;
using ClienteMarWPF.UI.Modules.Login;

using System; 

namespace ClienteMarWPF.UI.ViewModels.Commands.Login
{
    public class LoginCommand : ActionCommand
    {
        private readonly LoginViewModel viewmodellogin;
        private readonly IAuthenticator autenticador;
        private readonly IRenavigator navegaAppVistaInicial;
        private readonly ILocalClientSettingStore localclientsettings;


        public LoginCommand(LoginViewModel viewmodellogin, IAuthenticator autenticador, IRenavigator renavigator, ILocalClientSettingStore localclientsettings) : base()
        {
            this.viewmodellogin = viewmodellogin;
            this.autenticador = autenticador;
            this.navegaAppVistaInicial = renavigator;
            this.localclientsettings = localclientsettings;

            Action<object> comando = new Action<object>(IniciarSesion);
            base.SetAction(comando);
        }
        

        public void IniciarSesion(object password)
        {
            viewmodellogin.ErrorMessage = string.Empty;

            try
            {
                //int bancaid = 6;                   //@Pendiente definir logica para obtener banca id;
                //string ipaddress = "172.10.10.2";  //@Pendiente definir logica para obtener ipaddress;


                localclientsettings.ReadDektopLocalSetting(); //@leo el archivo .ini que contiene el ipadress y el banca id

                autenticador.IniciarSesion(viewmodellogin.Username, $"{password}" , localclientsettings.LocalClientSettings.BancaId , localclientsettings.LocalClientSettings.Direccion );
                navegaAppVistaInicial.Renavigate();

 

            }
            catch (MarFileReadException ex)
            {
                viewmodellogin.ErrorMessage = ex.Message;
            } 
            catch (UserNotFoundException ex)
            {
                viewmodellogin.ErrorMessage = ex.Message;
            }
            catch (InvalidPasswordException)
            {
                viewmodellogin.ErrorMessage = "Credenciales inválidas";
            }
            catch (Exception)
            {
                viewmodellogin.ErrorMessage = "Verificar conexión de internet";
            }
        }

 




    }// fin de Clase LoginCommand

}// fin de namespace 
