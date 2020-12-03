
using System;
using System.Linq;

using ClienteMarWPF.UI.Modules.FlujoEfectivo.Movimiento.Views.View1;

using ClienteMarWPF.Domain.Enums;
using ClienteMarWPF.Domain.Helpers;
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos;


using ClienteMarWPF.UI.ViewModels.Helpers;
using ClienteMarWPF.UI.Modules.FlujoEfectivo.Movimiento.Views.View1.ViewModels;
//using ClienteMarWPF.Domain.Services.CuadreService;

//using ClienteMarWPF.UI.State.Authenticators;
//using ClienteMarWPF.UI.State.Navigators;
//using ClienteMarWPF.UI.ViewModels.Factories;


namespace ClienteMarWPF.UI.ViewModels.Commands.FlujoEfectivo.Movimiento.EnCajaView
{

    public class SeleccionarAnonimosCommand : ActionCommand
    {
        private readonly EnCajaViewModel _viewmodel;
 

        public SeleccionarAnonimosCommand(EnCajaViewModel viewmodel ) : base()
        {
            SetAction(new Action<object>(SeleccionarAnonimo));

            _viewmodel = viewmodel; 
        }

        public void SeleccionarAnonimo(object parametro)
        {
            if (_viewmodel != null &&
                _viewmodel.Anonimos != null &&
                _viewmodel.Anonimos.TiposIngresosQueSonAnonimo != null &&
                _viewmodel.Anonimos.TiposEgresosQueSonAnonimo != null  &&
                _viewmodel.ComboESSeleccion != null

                )
            {
                _viewmodel.InputConcepto = new InputConceptoViewModel() { Muestro = false, Texto = "" };  //@@ Esta linea Resetea el Input Concepto y lo oculta          

                if (_viewmodel.ComboESSeleccion.Key == 1)
                {// Se desea hacer una |Entrada| de caja

                    var comboConceptoOpciones = _viewmodel.Anonimos.TiposIngresosQueSonAnonimo.MapToObservableCollection(1,true);
                    _viewmodel.ComboConceptoOpciones = comboConceptoOpciones;
                    _viewmodel.ComboConceptoSeleccion = comboConceptoOpciones.First();

                    _viewmodel.MuestroSeccionCajera = true;
                    _viewmodel.InputCajera.Muestro = true;

                }
                else
                {// Se desea hacer una |Salida| de caja
                    var comboConceptoOpciones = _viewmodel.Anonimos.TiposEgresosQueSonAnonimo.MapToObservableCollection(0,true);
                    _viewmodel.ComboConceptoOpciones = comboConceptoOpciones;
                    _viewmodel.ComboConceptoSeleccion = comboConceptoOpciones.First();

                    _viewmodel.MuestroSeccionCajera = false;
                    _viewmodel.InputCajera.Muestro = false;
                }
            }// fin de if 

        }//fin de metodo ComboboxSeleccionCambio


 




    }
}
