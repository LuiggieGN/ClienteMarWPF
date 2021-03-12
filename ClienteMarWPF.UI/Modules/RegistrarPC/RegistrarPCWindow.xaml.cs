using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClienteMarWPF.UI.Modules.RegistrarPC
{
    /// <summary>
    /// Lógica de interacción para RegistrarPCWindow.xaml
    /// </summary>
    public partial class RegistrarPCWindow : Window
    {
        public RegistrarPCWindow(RegistrarPCWindowViewModel contexto)
        {
            contexto.Control.CloseAction = () => CerrarVentana();

            InitializeComponent();

            DataContext = contexto;

            RegWindow.Focus();
        }

        public void CerrarVentana()
        {
            Close();
        }


        private void OnTeclaKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F7)
            {
                RegPC();
            }

            if (e.Key == Key.F5)
            {
                NoRegPC();
            }
        }

        private void RegPC()
        {
            ICommand RegistrarCommand = (DataContext as RegistrarPCWindowViewModel)?.Control?.RegistrarCommand ?? null;

            if (RegistrarCommand != null)
            {
                RegistrarCommand.Execute(null);
            }
        }

        private void NoRegPC()
        {
            ICommand CancelarCommand = (DataContext as RegistrarPCWindowViewModel)?.Control?.CancelarCommand ?? null;

            if (CancelarCommand != null)
            {
                CancelarCommand.Execute(null);
            }
        }







    }
}
