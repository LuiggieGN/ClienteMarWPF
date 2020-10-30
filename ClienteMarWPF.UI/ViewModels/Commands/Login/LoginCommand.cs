
using ClienteMarWPF.Domain.Exceptions;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.State.Navigators;
using ClienteMarWPF.UI.Modules.Login;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
 

namespace ClienteMarWPF.UI.ViewModels.Commands.Login
{
    public class LoginCommand : ActionCommand
    {
        private readonly LoginViewModel viewmodellogin;
        private readonly IAuthenticator autenticador;
        private readonly IRenavigator navegaAppVistaInicial;
 

        public LoginCommand(LoginViewModel viewmodellogin, IAuthenticator autenticador, IRenavigator renavigator) : base()
        {
            this.viewmodellogin = viewmodellogin;
            this.autenticador = autenticador;
            this.navegaAppVistaInicial = renavigator; 

            Action<object> comando = new Action<object>(IniciarSesion);
            base.SetAction(comando);
        }
        

        public void IniciarSesion(object password)
        {
            viewmodellogin.ErrorMessage = string.Empty;


            try
            {
                int bancaid = 6;                   //@Pendiente definir logica para obtener banca id;
                string ipaddress = "172.10.10.2";  //@Pendiente definir logica para obtener ipaddress;


                autenticador.IniciarSesion(viewmodellogin.Username, $"{password}" , bancaid, ipaddress );
                navegaAppVistaInicial.Renavigate();

 

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
