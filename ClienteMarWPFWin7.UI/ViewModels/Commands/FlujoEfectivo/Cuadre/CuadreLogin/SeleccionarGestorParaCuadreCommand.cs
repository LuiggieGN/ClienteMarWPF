
#region Namespaces
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ClienteMarWPFWin7.UI.ViewModels.Helpers;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos;

using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Cuadre.Windows.CuadreLogin;

#endregion


namespace ClienteMarWPFWin7.UI.ViewModels.Commands.FlujoEfectivo.Cuadre.CuadreLogin
{

    public class SeleccionarGestorParaCuadreCommand : ActionCommand
    {
        private readonly CuadreLoginViewModel _viewmodel;

        private PasswordBox _secretGestorPin;
        private PasswordBox _secretTarjetaToken;
        private Window _window;

        MultipleDTO<MUsuarioDTO, CajaDTO, TarjetaDTO> _gestor;
        RutaAsignacionDTO _asignacion;

        public SeleccionarGestorParaCuadreCommand(CuadreLoginViewModel viewmodel) : base()
        {
            SetAction(new Action<object>(SeleccionarGestorParaCuadre),
                      new Predicate<object>(SePuedeSeleccionarGestorParaCuadre));

            _viewmodel = viewmodel;
        }

        public bool SePuedeSeleccionarGestorParaCuadre(object param)
        {
            return _viewmodel?.HabilitaBotones ?? Booleano.No;
        }




        public void SeleccionarGestorParaCuadre(object parametro)
        { 
            var parametros = (object[])parametro;
         
            _secretGestorPin = (PasswordBox)parametros[0];
            _secretTarjetaToken = (PasswordBox)parametros[1];
            _window = (Window)parametros[2];

            string pin = _secretGestorPin.Password;

            pin = InputHelper.InputIsBlank(pin) ? "" : pin;

            if (_viewmodel != null && pin != string.Empty)
            {
                try
                {
                    Reset();
                    _gestor = _viewmodel.MultipleService.LeerUsuarioSuCajaYSuTarjetaPorPinDeUsuario(pin);
                    CallValidaciones();
                }
                catch
                {
                    SetInvalidGestor(error: "Ha ocurrido un error al procesar la operación. Verificar conexión de Internet");
                }
            } 
        }

        private void CallValidaciones()
        {
            if (_gestor.PrimerDTO == null)
            {
                SetInvalidGestor(error: "* Pin inválido");  return;
            }

            if (_gestor.PrimerDTO.UsuActivo == false)
            {
                SetInvalidGestor(error: "* El usuario está deshabilitado"); return;
            }

            if (_gestor.SegundoDTO == null)
            {
                SetInvalidGestor(error: "* El usuario no posee Control de Efectivo activo."); return;
            }

            if (_gestor.PrimerDTO.UsuPuedeCuadrar.HasValue == false || _gestor.PrimerDTO.UsuPuedeCuadrar.Value == 0)// **  0 : USUARIO no puede cuadrar
            {
                SetInvalidGestor(error: "* El usuario no posee permiso para cuadrar"); return; 
            }

            if (_gestor.PrimerDTO.UsuPuedeCuadrar.Value == 1) // ** 1 : USUARIO puede cuadrar solo CON ASIGNACION de ruta
            {
                try
                {
                    int bancaid = _viewmodel.AutService.BancaConfiguracion.BancaDto.BancaID;
                    _asignacion = _viewmodel.RutaService.LeerGestorAsignacionPendiente(_gestor.PrimerDTO.UsuarioID, bancaid);

                    if (_asignacion == null)
                    {
                        SetInvalidGestor(error: "* El usuario no tiene asignación de ruta pendiente"); return;
                    }

                    BancaEnRecorridoDTO bancaEnRecorrido = _asignacion.RutaRecorridoDTO.Terminales.First(t => t.BancaID == bancaid);

                    if (bancaEnRecorrido.FueRecorrida == true)
                    {
                        SetInvalidGestor(error: "* El usuario no tiene asignación de ruta pendiente. La Terminal ya fue cuadrada."); return;
                    }
                }
                catch 
                {
                    SetInvalidGestor(error: "* Ha ocurrido un error al leer la asignacion de ruta del usuario"); return;
                }                
            }

            if (_gestor.PrimerDTO.UsuPuedeCuadrar.Value == 2) // ** 1 : USUARIO puede cuadrar sin Ruta Asignada (no posee restrinciones)
            {
                _asignacion = null;
            }

            _viewmodel.Gestor = _gestor;
            _viewmodel.Asignacion = _asignacion;

            Challenge();
        }

        private void Challenge() 
        {
            if (_gestor.PrimerDTO.TipoAutenticacion == 2) // NO Challenge to Apply
            {
                _viewmodel.CuadreEsPermitido = Booleano.Si;
                _window.Close();
            }
            else //if (_gestor.PrimerDTO.TipoAutenticacion == 1) // Token Challenge is required
            {
                // @@ Esta seccion habilita el token desde la vista de login de cuadre
                //_viewmodel.CuadreEsPermitido = Booleano.No;

                //if (_gestor.TercerDTO == null ||
                //    _gestor.TercerDTO.InlineTokens == null ||
                //    _gestor.TercerDTO.InlineTokens.Count != 40)
                //{
                //    SetInvalidGestor(error: "* El usuario no posee tarjeta de tokens. Favor.. Contactarse con supervisor."); return; 
                //}
                //SetRandomTokenChallenge();

                //@@ Esta seccion fue agregada {lo acordado con Jaasiel}
                _viewmodel.CuadreEsPermitido = Booleano.No;

                if (_gestor.TercerDTO == null ||
                    _gestor.TercerDTO.InlineTokens == null ||
                    _gestor.TercerDTO.InlineTokens.Count != 40)
                {
                    SetInvalidGestor(error: "* El usuario no posee tarjeta de tokens. Favor.. Contactarse con supervisor."); return;
                }
                else
                {
                    _viewmodel.CuadreEsPermitido = Booleano.Si;
                    _window.Close();
                }
   


            }
        }

        private void Reset()
        {
            _viewmodel.NecesitoTokenValidacion = Booleano.No;
            _viewmodel.CuadreEsPermitido = Booleano.No;
            _viewmodel.Gestor = null;
            _viewmodel.Asignacion = null;
            _viewmodel.ErrorMessage = string.Empty;
            _viewmodel.SetearTokenAValidar(pos: string.Empty, token: string.Empty);
            _secretTarjetaToken.Clear();
        }

        private void SetInvalidGestor(string error)
        {
            _viewmodel.NecesitoTokenValidacion = Booleano.No;
            _viewmodel.CuadreEsPermitido = Booleano.No;
            _viewmodel.Gestor = null;
            _viewmodel.Asignacion = null;
            _viewmodel.ErrorMessage = error;
            _viewmodel.SetearTokenAValidar(pos: string.Empty, token: string.Empty);    
            _secretTarjetaToken.Clear();
            _secretGestorPin.Focus();
            _secretGestorPin.SelectAll();
        }

        private void SetRandomTokenChallenge() 
        {
            Random random = new Random();

            int totalDeTokens = _gestor.TercerDTO.InlineTokens.Count;

            int indiceToken = random.Next(totalDeTokens);

            int tokenPosicion = indiceToken + 1;

            string secretToken = _gestor.TercerDTO.InlineTokens[indiceToken];   

            _viewmodel.SetearTokenAValidar(pos: $"{tokenPosicion}", token: secretToken);

            _viewmodel.NecesitoTokenValidacion = Booleano.Si;
            _secretTarjetaToken.Clear();
            _secretTarjetaToken.Focus();
        }
                     


    }
}




