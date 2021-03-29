

#region Namespaces
using System;
using System.Windows;
using System.Globalization;

using ClienteMarWPFWin7.UI.ViewModels.Helpers;
using ClienteMarWPFWin7.UI.State.Authenticators;

using ClienteMarWPFWin7.Domain.Helpers;
using ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPFWin7.Domain.Services.CajaService;

using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Movimiento.Views.View1;

#endregion


namespace ClienteMarWPFWin7.UI.ViewModels.Commands.FlujoEfectivo.Movimiento.EnCajaView
{

    public class AgregarMovimientoEnCajaCommand : ActionCommand
    {
        private readonly EnCajaViewModel _viewmodel;
        private readonly IAuthenticator _aut;
        private readonly ICajaService _cajaService;
        private decimal _cajaBalanceActual;
        private decimal _monto;

        public AgregarMovimientoEnCajaCommand(EnCajaViewModel viewmodel, IAuthenticator aut, ICajaService cajaService) : base()
        {
            SetAction(new Action<object>(CrearNuevoMovimiento));

            _aut = aut;
            _cajaService = cajaService;
            _viewmodel = viewmodel;
        }

        public void CrearNuevoMovimiento(object parametro)
        {
            if (
                  _viewmodel != null &&
                  _viewmodel.ComboESSeleccion != null &&
                  _viewmodel.ComboConceptoSeleccion != null &&
                        _aut != null &&
                        _aut.BancaConfiguracion != null &&
                        _aut.BancaConfiguracion.BancaDto != null &&
                        _aut.BancaConfiguracion.CajaEfectivoDto != null
                )
            {
                try
                {
                    _cajaBalanceActual = _cajaService.LeerCajaBalance(_aut.BancaConfiguracion.CajaEfectivoDto.CajaID);

                    ValidarSubmit();

                    if (_viewmodel.CanCreate)
                    {

                        var movimiento = new SupraMovimientoEnBancaDTO();
                        movimiento.CajaId = _aut.BancaConfiguracion.CajaEfectivoDto.CajaID;
                        movimiento.BancaId = _aut.BancaConfiguracion.BancaDto.BancaID;
                        movimiento.UsuarioId = 0;
                        movimiento.Monto = _monto;

                        var commentStart = string.Empty;

                        if (_viewmodel.ComboConceptoSeleccion.Id == 0)
                        {
                            commentStart = (_viewmodel.InputConcepto?.Texto?.Trim() + "| ") ?? commentStart;
                        }
                        else
                        {
                            commentStart = _viewmodel.ComboConceptoSeleccion?.TipoNombre + "| ";
                        }

                        movimiento.Comentario = commentStart + _viewmodel?.Comentario ?? string.Empty;
                        movimiento.KeyIE = _viewmodel.ComboESSeleccion.Key;


                        var result = _cajaService.RegistrarMovimientoEnBanca(movimiento);


                        if (result.FueProcesado)
                        {


                            _aut.RefrescarBancaBalance();          //@@Actualizo el Balance de Banca 
                            _viewmodel.Comentario = string.Empty;
                            _viewmodel.Monto = string.Empty;
                            _viewmodel.Toast.ShowSuccess("Operación Completada");
                            ImprimirTransaccion(result);
                        }
                        else
                        {
                            _viewmodel.Toast.ShowError("Ha ocurrido un error al procesar la operación");
                        }
                    }
                }
                catch
                {
                    _viewmodel.Toast.ShowError("Ha ocurrido un error al procesar la operación. Verificar conexión de Internet");
                }
            }
        }

        #region Imprime Transaccion
        private void ImprimirTransaccion(SupraMovimientoEnBancaResultDTO result)
        {
            var docToPrint = new MovimientoToPrintHelper();
            var banca = _aut.BancaConfiguracion.BancaDto;

            docToPrint.BanContacto = banca.BanContacto;
            docToPrint.BanDireccion = banca.BanDireccion;
            docToPrint.BanTransaccion = result.Referencia;
            docToPrint.FechaTransaccion = result.FechaRegistro_dd_MMM_yyyy_hh_mm_tt;
            docToPrint.BanMonto = _monto.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));

            if (_viewmodel.ComboESSeleccion.Key == 1)
            {//ENTRADA de dinero en BANCA
                docToPrint.Recibido__Por = _viewmodel.InputCajera?.Cajera ?? string.Empty;
                docToPrint.EsUnDeposito = true;
            }
            else
            {//SALIDA de dinero en BANCA
                docToPrint.Recibido__Por = string.Empty;
                docToPrint.EsUnDeposito = false;
            }

            string toPrint = DocumentToPrintGeneratorHelper.GetMovimientoDocument(docToPrint);
            PrinterHelper.SendToPrinter(toPrint);
        }
        #endregion

        #region Validaciones de SubMit
        private void ValidarSubmit()
        {
            ResetErrors();
            if (_viewmodel.ComboConceptoSeleccion.Id == 0)
            {
                if (InputHelper.InputIsBlank(_viewmodel.InputConcepto?.Texto))
                {
                    _viewmodel.Errores.AgregarError(nameof(_viewmodel.InputConcepto), "*Debe ingresar un Concepto");
                }
            }

            if (InputHelper.InputIsBlank(_viewmodel.Comentario))
            {
                _viewmodel.Errores.AgregarError(nameof(_viewmodel.Comentario), "* Debe ingresar un Comentario");
            }

            if (_viewmodel.ComboESSeleccion.Key == 1)
            {
                if (InputHelper.InputIsBlank(_viewmodel.InputCajera?.Cajera))
                {
                    _viewmodel.Errores.AgregarError(nameof(_viewmodel.InputCajera), "* Debe ingresar el Nom. Cajera");
                }
            }

            if (InputHelper.InputIsBlank(_viewmodel.Monto))
            {
                _viewmodel.Errores.AgregarError(nameof(_viewmodel.Monto), "* Debe ingresar un Monto");
            }
            else
            {
                decimal montoConvertido;
                bool montoNoEsValido = !decimal.TryParse(_viewmodel.Monto, NumberStyles.Any, new CultureInfo("en-US"), out montoConvertido);

                if (montoNoEsValido || montoConvertido <= 0)
                {
                    _viewmodel.Errores.AgregarError(nameof(_viewmodel.Monto), "* Monto inválido.");
                }
                else
                {
                    _monto = montoConvertido;

                    if (_viewmodel.ComboESSeleccion.Key == 0)
                    {
                        if ((_cajaBalanceActual - _monto) < 0)
                        {
                            _viewmodel.Errores.AgregarError(nameof(_viewmodel.Monto), $"* Fondos Insuficientes." + Environment.NewLine + $"   - Balance en caja : { _cajaBalanceActual.ToString("C", new CultureInfo("en-US")) }." + Environment.NewLine + $"   - Monto a retirar : { _monto.ToString("C", new CultureInfo("en-US")) }");
                        }
                    }

                }// fin de else

            }// fin de else
        }
        private void ResetErrors()
        {
            _viewmodel.Errores.EliminarError(nameof(_viewmodel.InputConcepto));
            _viewmodel.Errores.EliminarError(nameof(_viewmodel.Comentario));
            _viewmodel.Errores.EliminarError(nameof(_viewmodel.InputCajera));
            _viewmodel.Errores.EliminarError(nameof(_viewmodel.Monto));
        }
        #endregion







    }
}
