﻿
using System;
using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Inicio.Modal;


namespace ClienteMarWPFWin7.UI.ViewModels.Commands.FlujoEfectivo.Inicio
{    
     
    public class CerrarDialogoInicioCommand : ActionCommand
    {
        private readonly DialogInicioViewModel _viewmodel;

        public CerrarDialogoInicioCommand(DialogInicioViewModel viewmodel) :base()
        {
            _viewmodel = viewmodel;  SetAction(new Action<object>(CerrarDialogo)); 
        }



        public void CerrarDialogo( object parametro ) 
        {
            if (_viewmodel != null)
            {
                _viewmodel.Ocultar();
            }
        }




    }
}
