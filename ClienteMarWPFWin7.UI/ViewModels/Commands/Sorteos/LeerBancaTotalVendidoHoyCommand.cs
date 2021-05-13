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
                ViewModel.TotalesCargados = No; decimal totPendientes = 0;


                int bancaid = ViewModel.Autenticador.BancaConfiguracion.BancaDto.BancaID;                
                decimal totalvendidoloterias = ViewModel.BancaServicio.LeerVentaDeHoyDeLoterias(bancaid);
                decimal totalvendidoproductos = ViewModel.BancaServicio.LeerVentaDeHoyDeProductos(bancaid);


                if (ViewModel.TransaccionesPendientes != null && ViewModel.TransaccionesPendientes.Any())
                {
                    totPendientes = ViewModel.TransaccionesPendientes.Sum(x => x);
                    ViewModel.TransaccionesPendientes?.Clear();
                }


                ViewModel.TotalesCargados = Si;
                ViewModel.TotalVendidoLoteria = totalvendidoloterias + totPendientes;
                ViewModel.TotalVendidoProductos = totalvendidoproductos;                
                ViewModel.TotalVendidoHoy = $"Vendido Hoy | Sorteo :  {ViewModel.TotalVendidoLoteria.ToString("C",new CultureInfo("en-US"))} | Productos : {ViewModel.TotalVendidoProductos.ToString("C", new CultureInfo("en-US"))}";


            }
            catch 
            {
                ViewModel.TransaccionesPendientes?.Clear();
                ViewModel.TotalesCargados = null;
                ViewModel.TotalVendidoLoteria = 0;
                ViewModel.TotalVendidoProductos = 0;
                ViewModel.TotalVendidoHoy = $"Vendido Hoy | Sorteo :  $0.00 | Productos : $0.00 ";
            }     
        }






    }//fin de clase
}
