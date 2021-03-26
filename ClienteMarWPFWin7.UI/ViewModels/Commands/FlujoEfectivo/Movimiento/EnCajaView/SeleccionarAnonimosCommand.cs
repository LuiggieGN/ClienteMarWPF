
using System;
using System.Linq;

using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Movimiento.Views.View1;

using ClienteMarWPFWin7.Domain.Enums;
using ClienteMarWPFWin7.Domain.Helpers;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos;


using ClienteMarWPFWin7.UI.ViewModels.Helpers;
using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Movimiento.Views.View1.ViewModels;
//using ClienteMarWPFWin7.Domain.Services.CuadreService;

//using ClienteMarWPFWin7.UI.State.Authenticators;
//using ClienteMarWPFWin7.UI.State.Navigators;
//using ClienteMarWPFWin7.UI.ViewModels.Factories;


namespace ClienteMarWPFWin7.UI.ViewModels.Commands.FlujoEfectivo.Movimiento.EnCajaView
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
