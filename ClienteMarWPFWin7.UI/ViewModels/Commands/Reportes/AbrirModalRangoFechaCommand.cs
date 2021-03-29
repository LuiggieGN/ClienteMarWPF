using ClienteMarWPFWin7.Domain.Services.ReportesService;
using ClienteMarWPFWin7.UI.Modules.Reporte;
using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.UI.State.PinterConfig;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Text;
using ClienteMarWPFWin7.UI.Modules.Reporte.Modal;

namespace ClienteMarWPFWin7.UI.ViewModels.Commands.Reportes
{
    class AbrirModalRangoFechaCommand : ActionCommand
    {

        private readonly ReporteViewModel ViewModel;


        public AbrirModalRangoFechaCommand(ReporteViewModel viewModel)
        {
            ViewModel = viewModel;

            Action<object> comando = new Action<object>(AbrirModal);
            base.SetAction(comando);

        }
        private void AbrirModal(object parametro)
        {

            ViewModel.Dialogo = new DialogoReporteViewModel(
                new ActionCommand((object p) => ViewModel.Dialogo.Ocultar()), //@@ Logica Boton ( Cancelar )
                new ActionCommand((object p) =>
                {
                    // @@ Logica Boton ( Aceptar )
                    // Desde  aqui puedo acceder a propiedades del ViewModel ... ejemplo  ViewModel.Fecha

                    ViewModel.FechaInicio = ViewModel.Dialogo.FechaInicio;
                    ViewModel.FechaFin = ViewModel.Dialogo.FechaFin;
                    ViewModel.SoloTotales = ViewModel.Dialogo.SoloTotales;
                    ViewModel.Dialogo.Ocultar();
                    ViewModel.ObtenerReportes?.Execute(null);
                }) 
            );

            ViewModel.Dialogo.Mostrar();


        }




    }
}
