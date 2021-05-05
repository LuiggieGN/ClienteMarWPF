using MAR.AppLogic.MARHelpers;
using ClienteMarWPFWin7.UI.Extensions;
using ClienteMarWPFWin7.Data;
using ClienteMarWPFWin7.UI.Modules.Sorteos;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using System;
using System.Globalization;
using System.Linq;

namespace ClienteMarWPFWin7.UI.ViewModels.Commands.Sorteos
{
    public class LeerBancaTotalVendidoHoyCommand : ActionCommand
    {
        private readonly SorteosViewModel ViewModel;

        public LeerBancaTotalVendidoHoyCommand(SorteosViewModel viewmodel)
        {
            ViewModel = viewmodel;             
            base.SetAction(new Action<object>(LeerTotalVendidoDeLoterias));
        }


        private void LeerTotalVendidoDeLoterias(object parametro)
        {
            try
            {
                int bancaid = ViewModel.Autenticador.BancaConfiguracion.BancaDto.BancaID;
                
                decimal totalvendido = ViewModel.BancaServicio.LeerVentaDeHoyDeLoterias(bancaid);

                ViewModel.TotalVendidoHoy = $"Ventas de Sorteos | Total Vendido Hoy : {totalvendido.ToString("C",CultureInfo.InvariantCulture)}";
                
            }
            catch 
            {
                ViewModel.TotalVendidoHoy = $"Ventas de Sorteos | Total Vendido Hoy : -N/A-";
            }     
        }






    }//fin de clase
}
