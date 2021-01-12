
#region Namespaces
using System;
using System.Windows;
using System.Windows.Controls;
using ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPF.UI.ViewModels.Helpers;
using ClienteMarWPF.UI.State.Accounts;
using ClienteMarWPF.UI.Modules.FlujoEfectivo.Cuadre.Windows.CuadreLogin;
using ClienteMarWPF.UI.Modules.FlujoEfectivo.Cuadre.Windows.Cuadre;
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

            var main = Application.Current.MainWindow;

            var viewLogin = new CuadreLoginView(main);

            var viewLoginContext = new CuadreLoginViewModel(_viewmodel.AutService, _viewmodel.MultipleService, _viewmodel.RutaService);

            viewLogin.DataContext = viewLoginContext;
            viewLogin.Owner = main;
            viewLogin.ShowDialog();

            if (viewLoginContext.CuadreEsPermitido)
            {
                try
                {
                    var gestorStored = new GestorStore();
                    gestorStored.GestorSesion = new GestorSesionDTO();
                    gestorStored.GestorSesion.Gestor = viewLoginContext.Gestor;
                    gestorStored.GestorSesion.Asignacion = viewLoginContext.Asignacion;

                    if (gestorStored.GestorSesion.Gestor != null &&
                        gestorStored.GestorSesion.Gestor.PrimerDTO != null &&
                        gestorStored.GestorSesion.Gestor.SegundoDTO != null
                       )
                    {
                        var viewCuadreContext = new CuadreViewModel(_viewmodel.AutService, gestorStored, _viewmodel.CuadreBuilder);
                        var viewCuadre = new CuadreView(main, viewCuadreContext, _viewmodel.AutService, _viewmodel.CuadreBuilder);
                        viewCuadre.Owner = main;
                        viewCuadre.ShowDialog();
                    }
                    else
                    {
                        _viewmodel.Toast.ShowError("Ha ocurrido un error. inesperado");
                    }
                }
                catch (Exception e)
                {
                    _viewmodel.Toast.ShowError("Ha ocurrido un error. Verificar Conexión de Internet");
                }
            }
            else
            {
                _viewmodel.Toast.ShowError("Operación de cuadre fue cancelada");
            }
        }








    }
}






