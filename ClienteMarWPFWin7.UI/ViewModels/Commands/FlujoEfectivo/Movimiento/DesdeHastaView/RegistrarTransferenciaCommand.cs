﻿
#region Namespaces
using System;
using System.Globalization;
using System.Windows.Controls;

using ClienteMarWPFWin7.Domain.Helpers;
using ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPFWin7.Domain.Services.CajaService;

using ClienteMarWPFWin7.UI.ViewModels.Helpers;
using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Movimiento.Views.View2;
#endregion




namespace ClienteMarWPFWin7.UI.ViewModels.Commands.FlujoEfectivo.Movimiento.DesdeHastaView
{

    public class RegistrarTransferenciaCommand : ActionCommand
    {
        private readonly DesdeHastaViewModel _viewmodel;
        private readonly ICajaService _cajaService;
        private decimal _cajaOrigenBalanceActual;
        private decimal _monto;

        public RegistrarTransferenciaCommand(DesdeHastaViewModel viewmodel, ICajaService cajaService) : base()
        {
            SetAction(new Action<object>(RegistrarTransferencia));

            _viewmodel = viewmodel;
            _cajaService = cajaService;
        }

        public void RegistrarTransferencia(object box)
        {

            var pass = (PasswordBox)box;
            var token = pass.Password;

            token = InputHelper.InputIsBlank(token) ?
                    string.Empty :
                    token;

            if (
                token != string.Empty &&
                _viewmodel != null &&
                _viewmodel.Aut != null &&
                _viewmodel.Aut.BancaConfiguracion != null &&
                _viewmodel.Aut.BancaConfiguracion.BancaDto != null &&
                _viewmodel.Aut.BancaConfiguracion.CajaEfectivoDto != null &&
                _viewmodel.Gestor != null &&
                _viewmodel.Gestor.PrimerDTO != null &&
                _viewmodel.Gestor.SegundoDTO != null &&
                _viewmodel.Gestor.TercerDTO != null &&
                _viewmodel.MovimientoVm.Dialog != null
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

                    if (_viewmodel.HasErrors)
                    {
                        pass.Clear();
                        _viewmodel.MovimientoVm.Dialog.Ocultar();
                        return;
                    }

                    bool tokenNoEsValido = !(ValidarToken(token)); //:: :: :: :: metodo ValidarToken => retorna true si es valido false si no es valido

                    if (tokenNoEsValido)
                    {
                        pass.Clear();
                        pass.Focus();
                        return;
                    }

                    var transferencia = new SupraMovimientoDesdeHastaDTO();

                    if (_viewmodel.ComboTransferirSeleccion.Key == 1)
                    {//@ De Gestor A Banca
                        transferencia.CajaOrigenId = _viewmodel.Gestor.SegundoDTO.CajaID;
                        transferencia.TipoCajaOrigen = 2;
                        transferencia.CajaDestinoId = _viewmodel.Aut.BancaConfiguracion.CajaEfectivoDto.CajaID;
                        transferencia.TipoCajaDestino = 1;                    }
                    else
                    {//@ De Banca A  Gestor
                        transferencia.CajaOrigenId = _viewmodel.Aut.BancaConfiguracion.CajaEfectivoDto.CajaID;
                        transferencia.TipoCajaOrigen = 1;
                        transferencia.CajaDestinoId = _viewmodel.Gestor.SegundoDTO.CajaID;
                        transferencia.TipoCajaDestino = 2;
                    }

                    transferencia.Comentario = _viewmodel.Comentario;
                    transferencia.Monto = _monto;

                    var result = _cajaService.RegistrarMovimientoDesdeHasta(transferencia);

                    pass.Clear();
                    _viewmodel.MovimientoVm.Dialog.Ocultar();

                    if (result.FueProcesado)
                    {
                        _viewmodel.Aut.RefrescarBancaBalance(); //@@Actualizo el Balance de Banca 
                        _viewmodel.Comentario = string.Empty;
                        _viewmodel.Monto = string.Empty;
                        _viewmodel.Toast.ShowSuccess("Operación Completada");
                        ImprimirTransaccion(result);
                        _viewmodel.FocusOperacionCompletada?.Invoke();
                    }
                    else
                    {
                        _viewmodel.Toast.ShowError("Ha ocurrido un error al procesar la operación");
                        _viewmodel.FocusAlFallar?.Invoke();
                    }
                }
                catch
                {
                    ResetErrors();
                    pass.Clear();
                    _viewmodel.MovimientoVm.Dialog.Ocultar();
                    _viewmodel.Toast.ShowError("Ha ocurrido un error al procesar la operación. Verificar conexión de Internet");
                }
            }//fin de If

        }// fin de metodo RegistrarTransferencia( )


        #region Imprime Transaccion
        private void ImprimirTransaccion(SupraMovimientoDesdeHastaResultDTO result)
        {
            var docToPrint = new MovimientoToPrintHelper();
            var banca = _viewmodel.Aut.BancaConfiguracion.BancaDto;

            docToPrint.BanContacto = banca.BanContacto;
            docToPrint.BanDireccion = banca.BanDireccion;
            docToPrint.FechaTransaccion = result.FechaTransferencia_dd_MMM_yyyy_hh_mm_tt;
            docToPrint.BanMonto = _monto.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));

            if (_viewmodel.ComboTransferirSeleccion.Key == 1)
            {//@ De Gestor A Banca
                docToPrint.Recibido__Por = _viewmodel.InputCajera?.Cajera ?? string.Empty;
                docToPrint.EsUnDeposito = true;
                docToPrint.BanTransaccion = result.RefDestino;
            }
            else
            {//@ De Banca A  Gestor
                docToPrint.Recibido__Por = (_viewmodel.Gestor.PrimerDTO.UsuNombre ?? string.Empty)+" "+ (_viewmodel.Gestor.PrimerDTO.UsuApellido ?? string.Empty);
                docToPrint.EsUnDeposito = false;
                docToPrint.BanTransaccion = result.RefOrigen;
            } 

            string toPrint = DocumentToPrintGeneratorHelper.GetMovimientoDocument(docToPrint);
            PrinterHelper.SendToPrinter(toPrint);
        }
        #endregion

        #region Validacion de Token

        //true: si es valido , false: si no es valido

        private bool ValidarToken(string token)
        {
            bool validacion = false;

            if ( 
                 _viewmodel.MovimientoVm.Dialog.PinGeneral == null||
                 _viewmodel.MovimientoVm.Dialog.PinGeneral == string.Empty||
                 _viewmodel.MovimientoVm.Dialog.PinGeneral.Equals("NONE", StringComparison.OrdinalIgnoreCase)
                )
            {//Logica Cuando No Hay Un Token General

                if (_viewmodel.MovimientoVm.Dialog.Token != token)
                {
                    validacion = false;
                    _viewmodel.MovimientoVm.Dialog.ErrMensaje = "* Token Inválido.";
                }
                else
                {
                    validacion = true;
                    _viewmodel.MovimientoVm.Dialog.ErrMensaje = string.Empty;
                }
            }
            else
            {//Logica Cuando Se Especifica Un Token General

                if (_viewmodel.MovimientoVm.Dialog.Token == token || _viewmodel.MovimientoVm.Dialog.PinGeneral == token)
                {
                    validacion = true;
                    _viewmodel.MovimientoVm.Dialog.ErrMensaje = string.Empty;
                }
                else
                {
                    validacion = false;
                    _viewmodel.MovimientoVm.Dialog.ErrMensaje = "* Token Inválido.";
                }
            }

            return validacion;
        }

        #endregion

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
                        _viewmodel.Errores.AgregarError(nameof(_viewmodel.Monto), $"* Fondos Insuficientes." + Environment.NewLine + $"   - Balance en caja de({(_viewmodel.ComboTransferirSeleccion.Key == 1? "Gestor":"Banca")}) : { _cajaOrigenBalanceActual.ToString("C", new CultureInfo("en-US")) }." + Environment.NewLine + $"   - Monto a Transferir : { _monto.ToString("C", new CultureInfo("en-US")) }");
                    }
                }
            }


        }// fin de metodo ValidarSubmit();

        private void ResetErrors()
        {
            _viewmodel.MovimientoVm.Dialog.ErrMensaje = string.Empty;
            _viewmodel.Errores.EliminarError(nameof(_viewmodel.Comentario));
            _viewmodel.Errores.EliminarError(nameof(_viewmodel.InputCajera));
            _viewmodel.Errores.EliminarError(nameof(_viewmodel.Monto));
        }

        #endregion















    }// fin de clase
}
