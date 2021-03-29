
#region Namespaces
using System; 
using System.Windows;
using System.Windows.Controls;
using ClienteMarWPFWin7.UI.ViewModels.Helpers;
using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Cuadre.Windows.CuadreLogin;
#endregion


namespace ClienteMarWPFWin7.UI.ViewModels.Commands.FlujoEfectivo.Cuadre.CuadreLogin
{

    public class ValidarTokenChallengeCommand : ActionCommand
    {
        private readonly CuadreLoginViewModel _viewmodel;

        private PasswordBox _secretTarjetaToken;
        private Window _window;



        public ValidarTokenChallengeCommand(CuadreLoginViewModel viewmodel) : base()
        {
            SetAction(new Action<object>(ValidarTokenChallenge),
                      new Predicate<object>(SePuedeValidarTokenChallenge));

            _viewmodel = viewmodel;
        }

        public bool SePuedeValidarTokenChallenge(object param)
        {
            return _viewmodel?.HabilitaBotones ?? Booleano.No;
        }

        public void ValidarTokenChallenge(object parametro)
        {
            var parametros = (object[])parametro;
 
            _secretTarjetaToken = (PasswordBox)parametros[0];
            _window = (Window)parametros[1];

            string pin = _secretTarjetaToken.Password;

            pin = InputHelper.InputIsBlank(pin) ? "" : pin;

            if (_viewmodel != null &&
                _viewmodel.Gestor != null &&
                _viewmodel.Gestor.PrimerDTO != null &&
                _viewmodel.Gestor.SegundoDTO != null &&
                _viewmodel.Posicion != null &&
                _viewmodel.Posicion != string.Empty &&
                _viewmodel.Token != null &&
                _viewmodel.Token != string.Empty &&                
                pin != string.Empty)
            {

                if (pin.Equals(_viewmodel.Token))
                {
                    _viewmodel.CuadreEsPermitido = Booleano.Si;
                    _window.Close();
                }
                else
                {
                    _viewmodel.CuadreEsPermitido = Booleano.No;
                    _viewmodel.ErrorMessage = "Token inválido";
                    _secretTarjetaToken.Clear();
                    _secretTarjetaToken.Focus();
                }
 
            }
        }








    }
}






