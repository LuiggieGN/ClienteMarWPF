
#region Namespaces
using System;
using System.Globalization;
using ClienteMarWPFWin7.Domain.Services.CajaService;
using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Movimiento.Modal;
using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Movimiento.Views.View2;
using ClienteMarWPFWin7.UI.ViewModels.Helpers;
#endregion


namespace ClienteMarWPFWin7.UI.ViewModels.Commands.FlujoEfectivo.Movimiento.DesdeHastaView
{

    public class AbrirModalTokenCommand : ActionCommand
    {
        private readonly DesdeHastaViewModel _viewmodel;
        private readonly ICajaService _cajaService;
        private decimal _cajaOrigenBalanceActual;
        private decimal _monto;

        public AbrirModalTokenCommand(DesdeHastaViewModel viewmodel, ICajaService cajaService) : base()
        {
            SetAction(new Action<object>(AbrirModalTokenConfirmacion));

            _viewmodel = viewmodel;
            _cajaService = cajaService;
        }

        public void AbrirModalTokenConfirmacion(object box)
        {
            if (_viewmodel != null &&
                _viewmodel.Gestor != null &&
                _viewmodel.Gestor.TercerDTO != null &&
                _viewmodel.Gestor.TercerDTO.InlineTokens != null &&
                _viewmodel.Gestor.TercerDTO.InlineTokens.Count > 0
                )
            {
                try
                {
                    if (_viewmodel.ComboTransferirSeleccion.Key == 1)
                    {
                        _cajaOrigenBalanceActual = _cajaService.LeerCajaBalance(_viewmodel.Gestor.SegundoDTO.CajaID);  // @@@ Caja Origen >>Gestor
                    }
                    else
                    {
                        _cajaOrigenBalanceActual = _cajaService.LeerCajaBalance(_viewmodel.Aut.BancaConfiguracion.CajaEfectivoDto.CajaID);  // @@@ Caja Origen >>Banca
                    }
                    
                    ValidarSubmit();

                    if (_viewmodel.CanCreate)
                    {
                        DesplegarVentanaDeToken();
                    }
                    else
                    {
                        _viewmodel.FocusCuandoHayErrorEnElModeloACrear?.Invoke();
                    }
                }
                catch
                {
                    _viewmodel.Toast.ShowError("Ha ocurrido un error al procesar la operación. Verificar conexión de Internet");
                    _viewmodel.FocusAlFallar?.Invoke();
                }
            }
        }

        private void DesplegarVentanaDeToken() 
        {
            Random random = new Random();

            int totalDeTokens = _viewmodel.Gestor.TercerDTO.InlineTokens.Count;

            int indiceToken = random.Next(totalDeTokens);

            int tokenPosicion = indiceToken + 1;

            string secretToken = _viewmodel.Gestor.TercerDTO.InlineTokens[indiceToken];

            _viewmodel.MovimientoVm.Dialog = new DialogoTokenViewModel(
                $"{tokenPosicion}",
                secretToken,
                new ActionCommand((object p) => {
                    
                    _viewmodel.MovimientoVm.Dialog.Ocultar();
                    _viewmodel.FocusAlFallar?.Invoke();
                }),
                new RegistrarTransferenciaCommand(_viewmodel, _cajaService)
            );

            _viewmodel.MovimientoVm.Dialog.Mostrar();
        }

        #region Validaciones de SubMit

        private void ValidarSubmit()
        {
            ResetErrors();

            if (InputHelper.InputIsBlank(_viewmodel.Comentario))
            {
                _viewmodel.Errores.AgregarError(nameof(_viewmodel.Comentario), "* Debe ingresar un Comentario");
            }

            if (_viewmodel.ComboTransferirSeleccion.Key == 1)
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

                    if ((_cajaOrigenBalanceActual - _monto) < 0)
                    {
                        _viewmodel.Errores.AgregarError(nameof(_viewmodel.Monto), $"* Fondos Insuficientes." + Environment.NewLine + $"   - Balance en caja de({(_viewmodel.ComboTransferirSeleccion.Key == 1 ? "Gestor" : "Banca")}) : { _cajaOrigenBalanceActual.ToString("C", new CultureInfo("en-US")) }." + Environment.NewLine + $"   - Monto a Transferir : { _monto.ToString("C", new CultureInfo("en-US")) }");
                    }
                }
            }


        }// fin de metodo ValidarSubmit();

        private void ResetErrors()
        {
            _viewmodel.Errores.EliminarError(nameof(_viewmodel.Comentario));
            _viewmodel.Errores.EliminarError(nameof(_viewmodel.InputCajera));
            _viewmodel.Errores.EliminarError(nameof(_viewmodel.Monto));
        }

        #endregion







    }
}
