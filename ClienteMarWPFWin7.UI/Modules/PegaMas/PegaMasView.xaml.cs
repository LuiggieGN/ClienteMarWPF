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
        public PegaMasView()
        {
            InitializeComponent(); 
        }

        private void CuandoSeEjecutaKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var vm = DataContext as PegaMasViewModel;
                if (vm != null)
                {
                    vm.AgregaApuestaCommand?.Execute(null);
                }
            }
        }

        private void CuandoCargaElModulo(object sender, RoutedEventArgs e)
        {
            IniciarFoco();

            var vm = DataContext as PegaMasViewModel;
            if (vm != null)
            {
                vm.FocusEnPrimerInput = () => IniciarFoco();
            }
        }

        private void IniciarFoco() 
        {
            EntradaD1.Focus();
        }

    }// Clase
}
