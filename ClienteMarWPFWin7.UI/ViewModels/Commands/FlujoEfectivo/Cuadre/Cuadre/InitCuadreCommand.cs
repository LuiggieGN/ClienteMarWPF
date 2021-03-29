
#region Namespaces
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ClienteMarWPFWin7.UI.ViewModels.Helpers;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos;

using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Cuadre.Windows.CuadreLogin;
using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Cuadre.Windows.Cuadre;
using System.Windows.Input;

#endregion


namespace ClienteMarWPFWin7.UI.ViewModels.Commands.FlujoEfectivo.Cuadre.Cuadre
{

    public class InitCuadreCommand : ActionCommand
    {
        private readonly CuadreViewModel _viewmodel;
 

        public InitCuadreCommand(CuadreViewModel viewmodel) : base()
        {

            _viewmodel = viewmodel;
            _viewmodel.SetRecomendacionCommand = new SetRecomendacionCommand(viewmodel);

            SetAction(new Action<object>(InitCuadre));  
        }
        


        public void InitCuadre(object parametro)
        {
            try
            {
                if (_viewmodel != null)
                {
                    _viewmodel.HabilitarBotones = Booleano.No;
                    _viewmodel.MontoContado = string.Empty;
                    _viewmodel.MontoDepositoORetiro = string.Empty;
                    _viewmodel.ConsultaInicial = _viewmodel.CuadreBuilder.LeerCuadreConsultaInicial(_viewmodel.AutService);
                    _viewmodel.SetRecomendacionCommand?.Execute((decimal)0);
                    _viewmodel.HabilitarBotones = Booleano.Si;
                }
            }
            catch (Exception ex)
            {
               throw ex;
            } 
        }

 


    }//fin de clase
}//  fin de namespace




