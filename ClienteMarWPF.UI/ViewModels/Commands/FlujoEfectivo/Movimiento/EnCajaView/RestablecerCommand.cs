

#region Namespaces
using System;
using System.Linq;

 

using ClienteMarWPF.UI.Modules.FlujoEfectivo.Movimiento.Views.View1;
using ClienteMarWPF.UI.Modules.FlujoEfectivo.Movimiento.Views.View1.ViewModels;
#endregion


namespace ClienteMarWPF.UI.ViewModels.Commands.FlujoEfectivo.Movimiento.EnCajaView
{

    public class RestablecerCommand : ActionCommand
    {
        private readonly EnCajaViewModel _viewmodel;

        public RestablecerCommand(EnCajaViewModel viewmodel ) : base()
        {
            SetAction(new Action<object>(Restablecer));

            _viewmodel = viewmodel;
        }

        public void Restablecer(object parametro)
        {
            if (
                  _viewmodel != null && 
                  _viewmodel.ComboESSeleccion != null &&
                  _viewmodel.ComboESOpciones != null
 
                )
            {
                _viewmodel.ComboESSeleccion = _viewmodel.ComboESOpciones.FirstOrDefault();
                _viewmodel.InputCajera = new InputCajeraViewModel() { Cajera = string.Empty, Muestro = true };
                _viewmodel.SeleccionarAnonimosCommand.Execute(null);
                _viewmodel.Comentario = string.Empty;
                _viewmodel.Monto = string.Empty;
                ResetErrors();
            }
        }



        private void ResetErrors()
        {
            _viewmodel.Errores.EliminarError(nameof(_viewmodel.InputConcepto));
            _viewmodel.Errores.EliminarError(nameof(_viewmodel.Comentario));
            _viewmodel.Errores.EliminarError(nameof(_viewmodel.InputCajera));
            _viewmodel.Errores.EliminarError(nameof(_viewmodel.Monto));
        }






    }
}
