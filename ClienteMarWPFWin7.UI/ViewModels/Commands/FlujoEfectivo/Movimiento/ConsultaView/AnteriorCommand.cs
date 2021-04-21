
#region Namespaces
using System;
using System.Linq;
using ClienteMarWPFWin7.UI.Extensions;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPFWin7.Domain.Services.CajaService;
using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Movimiento.Views.View3; 
#endregion

namespace ClienteMarWPFWin7.UI.ViewModels.Commands.FlujoEfectivo.Movimiento.ConsultaView
{
    public class AnteriorCommand : ActionCommand
    {
        private readonly ConsultaMovimientoViewModel _viewmodel;
        private readonly ICajaService _cajaService;

        public AnteriorCommand(ConsultaMovimientoViewModel viewmodel,
                               ICajaService cajaService) : base()
        {
            SetAction(new Action<object>(MoveToPreviousPage),
                      new Predicate<object>(CanMoveToPreviousPage));

            _viewmodel = viewmodel;
            _cajaService = cajaService;
        }

        public bool CanMoveToPreviousPage(object parameter)
        {
            return _viewmodel.Movimientos.PaginaNo != 1;
        }   


        public void MoveToPreviousPage(object parametro)
        {
            try
            {
                if (
                    _viewmodel.Movimientos != null &&
                    _viewmodel.Aut != null &&
                    _viewmodel.Aut.BancaConfiguracion != null &&
                    _viewmodel.Aut.BancaConfiguracion.CajaEfectivoDto != null
                   )
                {
                    var toSend = new MovimientoPageDTO();

                    DateTime fechaInicio = _viewmodel.From.HasValue ? _viewmodel.From.Value : DateTime.Now,
                                fechaFin = _viewmodel.To.HasValue ? _viewmodel.To.Value : DateTime.Now;

                    toSend.PaginaNo = _viewmodel.Movimientos.PaginaNo - 1;
                    toSend.PaginaSize = _viewmodel.Movimientos.PaginaSize;
                    toSend.OrdenAsc = _viewmodel.Movimientos.OrdenAscendiendo;
                    toSend.OrdenColumna = _viewmodel.Movimientos.OrdenColumna;
                    toSend.BancaId = null;
                    toSend.CajaId = _viewmodel.Aut.BancaConfiguracion.CajaEfectivoDto.CajaID;
                    toSend.FechaDesde = fechaInicio;
                    toSend.FechaHasta = fechaFin;
                    toSend.CategoriaOperacion = null;

                    var result = _cajaService.LeerMovimientos(toSend);

                    _viewmodel.Movimientos.PaginaSize = result.PrimerDTO.PaginaSize;
                    _viewmodel.Movimientos.PaginaNo = result.PrimerDTO.PaginaNo;
                    _viewmodel.Movimientos.TotalPaginas = result.PrimerDTO.TotalPaginas;
                    _viewmodel.Movimientos.TotalRecords = result.PrimerDTO.TotalRecords;
                    _viewmodel.Movimientos.OrdenAscendiendo = result.PrimerDTO.OrdenAsc;
                    _viewmodel.Movimientos.OrdenColumna = result.PrimerDTO.OrdenColumna;
                    _viewmodel.Movimientos.VistaPaginada = result.SegundoDTO.ToListOfMovimientoObservable();
                    _viewmodel.PageWasChanged();

                    _viewmodel.FocusOperacionCompletada?.Invoke();
                }
            }
            catch
            {
                _viewmodel.FocusOperacionFallida?.Invoke();
                _viewmodel.Toast.ShowError("Ha ocurrido un error al procesar la operación.Verificar conexión de internet.");
            }
        }



 





    }
}
