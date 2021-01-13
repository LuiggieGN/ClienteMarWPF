using ClienteMarWPF.Domain.Services.ReportesService;
using ClienteMarWPF.UI.Modules.Reporte;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.State.PinterConfig;
using MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Text;
using ClienteMarWPF.UI.Modules.Reporte.Modal;

namespace ClienteMarWPF.UI.ViewModels.Commands.Reportes
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
