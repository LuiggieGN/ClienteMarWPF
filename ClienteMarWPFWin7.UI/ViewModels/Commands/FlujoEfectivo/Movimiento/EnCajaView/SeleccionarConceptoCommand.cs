
using System;
using System.Linq;

using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Movimiento.Views.View1;

using ClienteMarWPFWin7.Domain.Enums;
using ClienteMarWPFWin7.Domain.Helpers;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos;


using ClienteMarWPFWin7.UI.ViewModels.Helpers;
//using ClienteMarWPFWin7.Domain.Services.CuadreService;

//using ClienteMarWPFWin7.UI.State.Authenticators;
//using ClienteMarWPFWin7.UI.State.Navigators;
//using ClienteMarWPFWin7.UI.ViewModels.Factories;


namespace ClienteMarWPFWin7.UI.ViewModels.Commands.FlujoEfectivo.Movimiento.EnCajaView
{

    public class SeleccionarConceptoCommand : ActionCommand
    {
        private readonly EnCajaViewModel _viewmodel;
 

        public SeleccionarConceptoCommand(EnCajaViewModel viewmodel ) : base()
        {
            SetAction(new Action<object>(SeleccionarConcepto));

            _viewmodel = viewmodel; 
        }

        public void SeleccionarConcepto(object parametro)
        {
            if (_viewmodel != null &&
                _viewmodel.ComboConceptoSeleccion != null
                )
            {
                if (_viewmodel.ComboConceptoSeleccion.Id == 0)
                {
                    _viewmodel.InputConcepto.Muestro = true;
                }
                else
                {
                    _viewmodel.InputConcepto.Muestro = false;
                }

                _viewmodel.FocusEnCambioDeConcepto?.Invoke();
            }// fin de if 

        }//fin de metodo ComboboxSeleccionCambio


 




    }
}
