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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClienteMarWPFWin7.UI.Modules.RegistrarPC
{
    /// <summary>
    /// Lógica de interacción para RegistrarPCControl.xaml
    /// </summary>
    public partial class RegistrarPCControl : UserControl
    {
        public static readonly DependencyProperty RegistrarCommandProperty = DependencyProperty.Register("RegistrarCommand", typeof(ICommand), typeof(RegistrarPCControl), new PropertyMetadata(null));
        public static readonly DependencyProperty CancelarCommandProperty = DependencyProperty.Register("CancelarCommand", typeof(ICommand), typeof(RegistrarPCControl), new PropertyMetadata(null));
        public ICommand RegistrarCommand
        {
            get
            {
                return (ICommand)GetValue(RegistrarCommandProperty);

            }
            set { SetValue(RegistrarCommandProperty, value); }
        }
        public ICommand CancelarCommand
        {
            get
            {
                return (ICommand)GetValue(CancelarCommandProperty);

            }
            set { SetValue(CancelarCommandProperty, value); }
        }

        public RegistrarPCControl()
        {
            InitializeComponent();
        }
        

        private void Registrar(object sender, RoutedEventArgs e)
        {
            RegPC();
        }

        private void Cancelar(object sender, RoutedEventArgs e)
        {
            NoRegPC();
        }

        private void RegPC()
        {
            if (RegistrarCommand != null)
            {
                RegistrarCommand.Execute(null);
            }
        }

        private void NoRegPC()
        {
            if (CancelarCommand != null)
            {
                CancelarCommand.Execute(null);
            }
        }










    }
}
