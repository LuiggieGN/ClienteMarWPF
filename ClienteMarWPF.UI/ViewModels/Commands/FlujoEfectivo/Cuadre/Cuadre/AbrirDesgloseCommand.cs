
#region Namespaces
using System;
using System.Windows; 
using ClienteMarWPF.UI.ViewModels.Helpers;
using ClienteMarWPF.UI.Modules.FlujoEfectivo.Cuadre.Windows.Cuadre; 
using ClienteMarWPF.UI.Modules.FlujoEfectivo.Cuadre.Windows.CuadreDesglose;
#endregion


namespace ClienteMarWPF.UI.ViewModels.Commands.FlujoEfectivo.Cuadre.Cuadre
{

    public class AbrirDesgloseCommand : ActionCommand
    {
        private readonly CuadreViewModel _viewmodel;
        private Window _window;

        public AbrirDesgloseCommand(CuadreViewModel viewmodel) : base()
        {
            SetAction(new Action<object>(AbrirDesglose),
                      new Predicate<object>(SePuedeAbrirDesglose));
            _viewmodel = viewmodel;
        }


        public bool SePuedeAbrirDesglose(object param)
        {
            return _viewmodel?.HabilitarBotones ?? Booleano.No;
        }

        public void AbrirDesglose(object parametro)
        {
            var parametros = (object[])parametro;

            _window = (Window)parametros[0];

            if (_viewmodel != null &&
                _viewmodel.ConsultaInicial != null)
            {

                var desglose = new DesgloseWindows(_window);
                desglose.Owner = _window;
                desglose.ShowDialog();

                if (desglose.Resultado.HasValue && desglose.Resultado != 0)
                {
                    _viewmodel.MontoContado = desglose.Resultado.ToString(); 
                }
            }
        }


 







    }//fin de clase
}//  fin de namespace




