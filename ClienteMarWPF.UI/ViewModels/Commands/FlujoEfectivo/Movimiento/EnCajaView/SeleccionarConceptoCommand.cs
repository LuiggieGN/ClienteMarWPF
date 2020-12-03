
using System;
using System.Linq;

using ClienteMarWPF.UI.Modules.FlujoEfectivo.Movimiento.Views.View1;

using ClienteMarWPF.Domain.Enums;
using ClienteMarWPF.Domain.Helpers;
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos;


using ClienteMarWPF.UI.ViewModels.Helpers;
//using ClienteMarWPF.Domain.Services.CuadreService;

//using ClienteMarWPF.UI.State.Authenticators;
//using ClienteMarWPF.UI.State.Navigators;
//using ClienteMarWPF.UI.ViewModels.Factories;


namespace ClienteMarWPF.UI.ViewModels.Commands.FlujoEfectivo.Movimiento.EnCajaView
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
            }// fin de if 

        }//fin de metodo ComboboxSeleccionCambio


 




    }
}
