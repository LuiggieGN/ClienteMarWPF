

#region Namespaces
using System;
using System.Windows.Controls;
using ClienteMarWPFWin7.UI.ViewModels.Helpers;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPFWin7.Domain.Services.MultipleService;
using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Movimiento.Views.View2;
#endregion


namespace ClienteMarWPFWin7.UI.ViewModels.Commands.FlujoEfectivo.Movimiento.DesdeHastaView
{

    public class SeleccionarGestorCommand : ActionCommand
    {
        private readonly DesdeHastaViewModel _viewmodel;
        private readonly IMultipleService _multipleService;


        public SeleccionarGestorCommand(DesdeHastaViewModel viewmodel, IMultipleService multipleService) : base()
        {
            SetAction(new Action<object>(SeleccionarGestorPorPin));

            _viewmodel = viewmodel;
            _multipleService = multipleService;
        }

        public void SeleccionarGestorPorPin(object box)
        {

            PasswordBox pass = (PasswordBox)box;

            string pin = pass.Password;

            pin = InputHelper.InputIsBlank(pin) ? "" : pin;

            if (_viewmodel != null && pin != string.Empty)
            {
                try
                {
                    ResetErrors();

                    var datosGestor = _multipleService.LeerUsuarioSuCajaYSuTarjetaPorPinDeUsuario(pin);
                    
                    SetGestor(datosGestor);
                }
                catch
                {
                    SetInvalidGestor();
                    _viewmodel.Toast.ShowError("Ha ocurrido un error al procesar la operación. Verificar conexión de Internet");
                }
            } 

            pass.Clear();
            pass.Focus();
        }


        #region Validaciones de SubMit
        private void SetGestor(MultipleDTO<MUsuarioDTO, CajaDTO, TarjetaDTO> gestor)
        {
            if (gestor.PrimerDTO == null) // Si esto es asi el usuario no fue encontrado
            {
                SetInvalidGestor();
                _viewmodel.Errores.AgregarError(nameof(_viewmodel.PinGestor), "* Número de pin inválido");
                return;
            }

            if (gestor.PrimerDTO.UsuActivo == false)
            {
                SetInvalidGestor();
                _viewmodel.Errores.AgregarError(nameof(_viewmodel.PinGestor), "* Usuario deshabilitado.");
                return;
            }

            if (gestor.SegundoDTO == null)
            {
                SetInvalidGestor();
                _viewmodel.Errores.AgregarError(nameof(_viewmodel.PinGestor), "* Usuario no posee Control de Efectivo activo.");
                return;
            }

            if (gestor.TercerDTO == null ||
                gestor.TercerDTO.InlineTokens == null ||
                gestor.TercerDTO.InlineTokens.Count != 40)
            {
                SetInvalidGestor();
                _viewmodel.Errores.AgregarError(nameof(_viewmodel.PinGestor), "* No posee tarjeta de tokens. Favor.. Contactarse con supervisor.");
                return;
            }

            SetValidGestor(gestor);
        }

        private void ResetErrors()
        {
            _viewmodel.Errores.EliminarError(nameof(_viewmodel.PinGestor));
        }

        private void SetInvalidGestor()
        {
            _viewmodel.MuestroBotonNuevaTrasferencia = false;
            _viewmodel.Gestor = null;
            _viewmodel.NomGestor = "-- Seleccionar Gestor --";
        }

        private void SetValidGestor(MultipleDTO<MUsuarioDTO, CajaDTO, TarjetaDTO> gestor)
        {
            _viewmodel.MuestroBotonNuevaTrasferencia = true;
            _viewmodel.Gestor = gestor;
            _viewmodel.NomGestor = gestor?.PrimerDTO?.UsuNombre ?? string.Empty + ' ' + gestor?.PrimerDTO?.UsuApellido ?? string.Empty;
        }

        #endregion







    }
}
