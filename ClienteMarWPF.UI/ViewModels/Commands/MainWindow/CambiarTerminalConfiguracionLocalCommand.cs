﻿
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

            var contexto = new ConfiguracionViewModel(_viewmodel.LocalClientSetting);
            contexto.CargandoDesdelogin = true;
            contexto.ConfiguracionFueCambiada = false;

            var ventana = new ConfiguracionView(main, contexto);
            ventana.Owner = main;
            ventana.ShowDialog();

            if (contexto.ConfiguracionFueCambiada)
            { 
                _viewmodel.ReIniciarApp?.Invoke();
                _viewmodel.Aplicacion.CerrarAplicacion();                
            }
        }








    }
}






