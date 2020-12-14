
#region Namespaces
using System;
using System.Linq;
using System.Windows.Controls;

using ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos;

using ClienteMarWPF.UI.ViewModels.Helpers;
using ClienteMarWPF.UI.Modules.FlujoEfectivo.Movimiento.Views.View2;
using ClienteMarWPF.UI.Modules.FlujoEfectivo.Movimiento.Views.View1.ViewModels;
#endregion


namespace ClienteMarWPF.UI.ViewModels.Commands.FlujoEfectivo.Movimiento.DesdeHastaView
{

    public class RestablecerCommand : ActionCommand
    {
        private readonly DesdeHastaViewModel _viewmodel;

        public RestablecerCommand(DesdeHastaViewModel viewmodel) : base()
        {
            SetAction(new Action<object>(Restablecer));

            _viewmodel = viewmodel;
        }

        public void Restablecer(object box)
        {
            var pass = (PasswordBox)box;      

            if (_viewmodel != null)
            {
                ResetErrors();
                _viewmodel.PinGestor = string.Empty;
                _viewmodel.ComboTransferirSeleccion = _viewmodel.ComboTransferirOpciones.FirstOrDefault();
                SetInvalidGestor();
                _viewmodel.Comentario = string.Empty;
                _viewmodel.MuestroSeccionCajera = true;
                _viewmodel.InputCajera = new InputCajeraViewModel() { Cajera = string.Empty, Muestro = true };
                _viewmodel.Monto = string.Empty;
                pass.Clear();
                pass.Focus();

            }//fin de If
        } 





        #region ResetErrors
        private void ResetErrors()
        {
            _viewmodel.Errores.EliminarError(nameof(_viewmodel.PinGestor));
            _viewmodel.Errores.EliminarError(nameof(_viewmodel.Comentario));
            _viewmodel.Errores.EliminarError(nameof(_viewmodel.InputCajera));
            _viewmodel.Errores.EliminarError(nameof(_viewmodel.Monto));
        }
        #endregion


        private void SetInvalidGestor()
        {
            _viewmodel.MuestroBotonNuevaTrasferencia = false;
            _viewmodel.Gestor = null;
            _viewmodel.NomGestor = "-- Seleccionar Gestor --";
        }













    }// fin de clase
}
