
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
    public class RemoverJugadasCommand : ActionCommand
    {
        private readonly PegaMasViewModel vm;

        public RemoverJugadasCommand(PegaMasViewModel viewmodel) : base()
        {
            vm = viewmodel;
            base.SetAction(new Action<object>(Remover));
        }

        private void Remover(object parametro)
        {
            if (vm.Jugadas != null && vm.Jugadas.Count > 0)
            {
                vm.Jugadas.Clear();
                vm.TriggerJugadasUpd();
                vm.FocusEnPrimerInput?.Invoke();
            }
        }




    }// Clase
}
