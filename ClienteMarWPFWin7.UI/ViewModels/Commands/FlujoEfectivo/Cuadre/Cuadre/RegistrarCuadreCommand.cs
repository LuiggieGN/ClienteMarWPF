
#region Namespaces
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using ClienteMarWPFWin7.UI.ViewModels.Helpers;
using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Cuadre.Windows.Cuadre;
using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Cuadre.Windows.Cuadre.Modal;
using ClienteMarWPFWin7.Domain.Enums;
using ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPFWin7.Domain.Helpers;
#endregion


namespace ClienteMarWPFWin7.UI.ViewModels.Commands.FlujoEfectivo.Cuadre.Cuadre
{

    public class RegistrarCuadreCommand : ActionCommand
    {
        private readonly CuadreViewModel _viewmodel;
        private decimal _cajaOrigenBalanceActual;
        private decimal _montoContado;
        private decimal _montoRetirarODepositar;
        private Window VentanaCuadre;

        public RegistrarCuadreCommand(CuadreViewModel viewmodel) : base()
        {
            SetAction(new Action<object>(RegistrarCuadre),
                      new Predicate<object>(SePuedeRegistrarCuadre));
            _viewmodel = viewmodel;
        }


        public bool SePuedeRegistrarCuadre(object param)
        {
            return _viewmodel?.HabilitarBotones ?? Booleano.No;
        }

        public void RegistrarCuadre(object parametro)
        {
            this.VentanaCuadre = parametro as Window;

            if (_viewmodel != null &&
                _viewmodel.CuadreBuilder != null &&
                _viewmodel.ConsultaInicial != null &&
                _viewmodel.AutService != null &&
                _viewmodel.AutService.BancaConfiguracion != null &&
                _viewmodel.AutService.BancaConfiguracion.CajaEfectivoDto != null &&
                _viewmodel.GestorStored != null &&
                _viewmodel.GestorStored.GestorSesion != null &&
                _viewmodel.GestorStored.GestorSesion.Gestor != null &&
                _viewmodel.GestorStored.GestorSesion.Gestor.PrimerDTO != null &&
                _viewmodel.GestorStored.GestorSesion.Gestor.SegundoDTO != null)
            {
                try
                {
                    if (_viewmodel.CuadreGestorAccion == CuadreGestorAccion.Retirar) // Caja Origen es la de la banca
                    {
                        _cajaOrigenBalanceActual = _viewmodel.CuadreBuilder.LeerCajaBalance(_viewmodel.AutService.BancaConfiguracion.CajaEfectivoDto.CajaID);
                    }
                    else // Caja Origen es la del gestor
                    {
                        _cajaOrigenBalanceActual = _viewmodel.CuadreBuilder.LeerCajaBalance(_viewmodel.GestorStored.GestorSesion.Gestor.SegundoDTO.CajaID);
                    }

                    ValidarSubmit();

                    if (_viewmodel.CanCreate)
                    {
                        if (_viewmodel.GestorStored.GestorSesion.Gestor.PrimerDTO.TipoAutenticacion == 2) // NO Challenge to Apply
                        {//NO REQUIERE TOKEN PARA CUADRAR

                            CuadrarTerminal();
                        }
                        else //if (_gestor.PrimerDTO.TipoAutenticacion == 1) // Token Challenge is required
                        {//REQUIERE TOKEN

                            ValidarGestorTarjetaTokenYCuadrar();
                        }
                    }


                }
                catch
                {
                    MessageBox.Show("Ha ocurrido un error al procesar la operación. Verificar conexión de internet", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }// fin de if

        }// fin de metodo 


        private void ValidarGestorTarjetaTokenYCuadrar()
        {
            Random random = new Random();

            int totalDeTokens = _viewmodel.GestorStored.GestorSesion.Gestor.TercerDTO.InlineTokens.Count;

            int indiceToken = random.Next(totalDeTokens);

            int tokenPosicion = indiceToken + 1;

            string secretToken = _viewmodel.GestorStored.GestorSesion.Gestor.TercerDTO.InlineTokens[indiceToken];

            _viewmodel.Dialog = new DialogoTokenViewModel(
                $"{tokenPosicion}",
                secretToken,
                new ActionCommand((object p) => _viewmodel.Dialog.Ocultar()), // cuando se presiona ( Cancelar )
                new ActionCommand((object p) =>
                {
                    var pass = (PasswordBox)p;
                    var token = pass.Password;

                    token = InputHelper.InputIsBlank(token) ?
                            string.Empty :
                            token;

                    if (token != string.Empty)
                    {
                        _viewmodel.Dialog.ErrMensaje = string.Empty;

                        if (secretToken != token)
                        {
                            _viewmodel.Dialog.ErrMensaje = "* Token Inválido.";
                            pass.Clear();
                            pass.Focus();
                            return;
                        }
                        else
                        {
                            _viewmodel.Dialog.ErrMensaje = string.Empty;
                            pass.Clear();
                            _viewmodel.Dialog.Ocultar();
                            CuadrarTerminal();
                        }
                    }

                }) // cuando se presiona ( Aceptar )
            );

            _viewmodel.Dialog.Mostrar();


        }


        private void CuadrarTerminal()
        {
            var ope = new CuadreRegistroDTO();
            ope.RutaAsignacion = _viewmodel.GestorStored.GestorSesion.Asignacion;
            ope.CuadreGestorAccion = _viewmodel.CuadreGestorAccion;
            ope.Cuadre = new CuadreDTO();
            ope.Cuadre.Tipo = CuadreHelper.Get(CuadreTipo.Manual);
            ope.Cuadre.BalanceMinimo = _viewmodel.ConsultaInicial.BancaBalanceMinimo;
            ope.Cuadre.CajaID = _viewmodel.AutService.BancaConfiguracion.CajaEfectivoDto.CajaID;
            ope.Cuadre.UsuarioCaja = _viewmodel.NombreCajera;
            ope.Cuadre.CajaOrigenID = _viewmodel.GestorStored.GestorSesion.Gestor.SegundoDTO.CajaID;
            ope.Cuadre.UsuarioOrigenID = _viewmodel.GestorStored.GestorSesion.Gestor.PrimerDTO.UsuarioID;
            ope.Cuadre.AuxMensajeroNombre = _viewmodel.GestorNombre;
            ope.Cuadre.MontoContado = _montoContado;
            ope.Cuadre.MontoRetirado = _montoRetirarODepositar;
            ope.Cuadre.MontoPorPagar = _viewmodel.ConsultaInicial.BancaDeuda;

            if (_viewmodel.CuadreGestorAccion == CuadreGestorAccion.Depositar)
            {
                ope.Cuadre.MontoDepositado = ope.Cuadre.MontoRetirado;
                ope.Cuadre.MontoRetirado = ope.Cuadre.MontoRetirado * -1;
            }

            string toPrint;
            var result = _viewmodel.CuadreBuilder.BuildCuadre(_viewmodel.AutService.BancaConfiguracion.BancaDto, ope, true, out toPrint);

            if (result.FueProcesado)
            {

                _viewmodel.AutService.RefrescarBancaBalance(); //@@Actualizo el Balance de Banca 

                _viewmodel.InitCuadreCommand?.Execute(null);   //@@ Re-Inicia vista para un nuevo cuadre

                PrinterHelper.SendToPrinter(toPrint);          //@@Imprime Cuadre
                PrinterHelper.SendToPrinter(toPrint);          //@@Imprime Cuadre
                MessageBox.Show("El cuadre fue procesado", "Operación Completada", MessageBoxButton.OK, MessageBoxImage.Information);
                this.VentanaCuadre?.Close();

            }
            else
            {
                MessageBox.Show(result.Error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ValidarSubmit()
        {
            ResetErrors();

            if (InputHelper.InputIsBlank(_viewmodel.MontoContado))
            {
                _montoContado = 0;
            }
            else
            {
                decimal montoConvertido;
                bool montoNoEsValido = !decimal.TryParse(_viewmodel.MontoContado, NumberStyles.Any, new CultureInfo("en-US"), out montoConvertido);

                if (montoNoEsValido || montoConvertido <= 0)
                {
                    _montoContado = 0;
                }
                else
                {
                    _montoContado = montoConvertido;
                }
            }

            if (InputHelper.InputIsBlank(_viewmodel.MontoDepositoORetiro))
            {
                _montoRetirarODepositar = 0;
            }
            else
            {
                decimal montoConvertido;
                bool montoNoEsValido = !decimal.TryParse(_viewmodel.MontoDepositoORetiro, NumberStyles.Any, new CultureInfo("en-US"), out montoConvertido);

                if (montoNoEsValido || montoConvertido <= 0)
                {
                    _montoRetirarODepositar = 0;
                }
                else
                {
                    _montoRetirarODepositar = montoConvertido;

                    if ((_cajaOrigenBalanceActual - _montoRetirarODepositar) < 0)
                    {
                        _viewmodel.Errores.AgregarError(nameof(_viewmodel.MontoDepositoORetiro), $"* Fondos Insuficientes." + Environment.NewLine + $"   - Balance en caja de({(_viewmodel.CuadreGestorAccion == CuadreGestorAccion.Depositar ? "Gestor" : "Banca")}) : { (_viewmodel.CuadreGestorAccion == CuadreGestorAccion.Depositar ? "Fondos Insuficientes." : _cajaOrigenBalanceActual.ToString("C", new CultureInfo("en-US")))}." + Environment.NewLine + $"   - Monto a Transferir : { _montoRetirarODepositar.ToString("C", new CultureInfo("en-US")) }");
                    }
                }
            }

            if (InputHelper.InputIsBlank(_viewmodel.NombreCajera))
            {
                _viewmodel.Errores.AgregarError(nameof(_viewmodel.NombreCajera), "* Debe especificar el nombre de la cajera.");
            }
        }
        private void ResetErrors()
        {
            _viewmodel.Errores.EliminarError(nameof(_viewmodel.MontoDepositoORetiro));
            _viewmodel.Errores.EliminarError(nameof(_viewmodel.NombreCajera));
        }




    }//fin de clase
}//  fin de namespace




