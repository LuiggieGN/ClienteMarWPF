

#region Namespaces
using System;
using System.Windows;
using System.Globalization;

using ClienteMarWPF.UI.ViewModels.Helpers;
using ClienteMarWPF.UI.State.Authenticators;

using ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPF.Domain.Services.CajaService;

using ClienteMarWPF.UI.Modules.FlujoEfectivo.Movimiento.Views.View1;
#endregion


namespace ClienteMarWPF.UI.ViewModels.Commands.FlujoEfectivo.Movimiento.EnCajaView
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
                            _viewmodel.Comentario = string.Empty;
                            _viewmodel.Monto = string.Empty;
                            _viewmodel.Toast.ShowSuccess("Operación Completada");

                            // @@ Pendiente en esta linea Imprimir Movimiento


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
