
#region Namespace
 
using ClienteMarWPFWin7.UI.Modules.PegaMas; 
using System;
using System.Linq;
using System.Globalization;
#endregion

namespace ClienteMarWPFWin7.UI.ViewModels.Commands.PegaMas
{
    public class CalcularMontoTotalJugadoCommand : ActionCommand
    {
        private readonly PegaMasViewModel vm;

        public CalcularMontoTotalJugadoCommand(PegaMasViewModel viewmodel) : base()
        {
            vm = viewmodel;
            base.SetAction(new Action<object>(Calcular));
        }

        private void Calcular(object parametro)
        {
            if (vm.Jugadas != null && vm.Jugadas.Count > 0)
            {
                decimal totalJugado = vm.Jugadas.Sum(j => j.Monto);

                vm.TotalJugado = $"${totalJugado.ToString("N", CultureInfo.InvariantCulture)}";

            }
            else
            {
                vm.TotalJugado = "$0.00";
            }
        }




    }// Clase
}
