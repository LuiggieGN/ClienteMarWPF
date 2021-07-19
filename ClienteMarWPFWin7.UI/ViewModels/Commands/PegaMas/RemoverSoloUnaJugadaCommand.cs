
#region Namespace
using ClienteMarWPFWin7.UI.ViewModels.ModelObservable;
using ClienteMarWPFWin7.UI.ViewModels.Helpers;
using ClienteMarWPFWin7.UI.Modules.PegaMas;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.UI;
using System.Collections.Generic;
using System.Windows;
using System;
using System.Linq;
#endregion

namespace ClienteMarWPFWin7.UI.ViewModels.Commands.PegaMas
{
    public class RemoverSoloUnaJugadaCommand : ActionCommand
    {
        private readonly PegaMasViewModel vm;

        public RemoverSoloUnaJugadaCommand(PegaMasViewModel viewmodel) : base()
        {
            vm = viewmodel;
            base.SetAction(new Action<object>(RemoverSoloUna));
        }

        private void RemoverSoloUna(object parametro)
        {
            int id;

            if (int.TryParse(parametro?.ToString()??"-1", out id))
            {
                if (vm.Jugadas != null && vm.Jugadas.Count > 0)
                {
                    PegaMasApuestaObservable item = vm.Jugadas.Where(f => f.Id == id).FirstOrDefault();

                    if (item != null)
                    {
                        vm.Jugadas.Remove(item);
                        vm.CalcularMontoTotalJugadoCommand?.Execute(null);
                        vm.TriggerJugadasUpd();
                        vm.FocusEnPrimerInput?.Invoke();
                    }
                }
            }
        }




    }// Clase
}
