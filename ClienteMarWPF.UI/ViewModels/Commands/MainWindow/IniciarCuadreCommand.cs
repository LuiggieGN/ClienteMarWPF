
#region Namespaces
using System; 
using System.Windows;
using System.Windows.Controls;
using ClienteMarWPF.UI.ViewModels.Helpers;
using ClienteMarWPF.UI.Modules.FlujoEfectivo.Cuadre.Windows.CuadreLogin;
#endregion


namespace ClienteMarWPF.UI.ViewModels.Commands.MainWindow
{

    public class IniciarCuadreCommand : ActionCommand
    {
        private readonly MainWindowViewModel _viewmodel;


        public IniciarCuadreCommand(MainWindowViewModel viewmodel) : base()
        {
            SetAction(new Action<object>(IniciarCuadre));
            _viewmodel = viewmodel;
        }

 

        public void IniciarCuadre(object parametro)
        {

            Window parent = Application.Current.MainWindow;
            CuadreLoginView window = new CuadreLoginView(parent);
            var loginContext = new CuadreLoginViewModel(_viewmodel.AutService, _viewmodel.MultipleService, _viewmodel.RutaService);
            window.DataContext = loginContext;
            window.Owner = parent;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();

            if (loginContext.CuadreEsPermitido)
            {

            }
            else
            {
                _viewmodel.Toast.ShowError("Operación de cuadre fue cancelada");
            }
        }








    }
}






