
#region Namespaces
using System;
using System.Windows;
using System.Windows.Controls;
using ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPF.UI.ViewModels.Helpers;
using ClienteMarWPF.UI.State.Accounts;
using ClienteMarWPF.UI.Modules.Configuracion;

#endregion


namespace ClienteMarWPF.UI.ViewModels.Commands.MainWindow
{

    public class CambiarTerminalConfiguracionLocalCommand : ActionCommand
    {
        private readonly MainWindowViewModel _viewmodel;


        public CambiarTerminalConfiguracionLocalCommand(MainWindowViewModel viewmodel) : base()
        {
            SetAction(new Action<object>(AbrirVentanaConfiguracionLocal));
            _viewmodel = viewmodel;
        }



        public void AbrirVentanaConfiguracionLocal(object parametro)
        {
            var main = Application.Current.MainWindow;

            var clientLocalSettingWindowContext = new ConfiguracionViewModel(_viewmodel.LocalClientSetting);

            var clientLocalSettingWindow = new ConfiguracionView(main, clientLocalSettingWindowContext) ;

            clientLocalSettingWindow.Owner = main;

            clientLocalSettingWindow.ShowDialog();
 
        }








    }
}






