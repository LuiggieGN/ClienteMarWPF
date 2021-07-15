using ClienteMarWPFWin7.UI.ViewModels.ModelObservable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClienteMarWPFWin7.UI.Modules.PegaMas
{
    public partial class PegaMasView : UserControl
    {
        private PegaMasViewModel vm;
        public PegaMasView()
        {
            InitializeComponent();
        }

        private void ModuloKeyUp(object sender, KeyEventArgs e)
        {
            if (vm != null && e.Key == Key.Enter)
            {
                vm.AgregaApuestaCommand?.Execute(null);
            }
            else if (vm != null && e.Key == Key.F9)
            {
                vm.RemoverJugadasCommand?.Execute(null);
            }
        }

        private void ModuloCargado(object sender, RoutedEventArgs e)
        {
            vm = DataContext as PegaMasViewModel;
            vm.FocusEnPrimerInput = () => IniciarFoco();
            vm.FocusEnPrimerInput?.Invoke();
        }

        private void RemoverSoloUnaJugada(object sender, RoutedEventArgs e)
        {
            var objEnFila = GridJugadas.SelectedItem as PegaMasApuestaObservable;

            if (objEnFila != null && vm != null)
            {
                vm.RemoverSoloUnaJugadaCommand?.Execute(objEnFila.Id);
            }
        }


        private void IniciarFoco() => EntradaD1.Focus();




    }// Clase
}
