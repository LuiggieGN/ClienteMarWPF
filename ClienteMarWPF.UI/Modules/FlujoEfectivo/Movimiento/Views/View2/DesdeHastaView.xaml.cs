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

namespace ClienteMarWPF.UI.Modules.FlujoEfectivo.Movimiento.Views.View2
{
    public partial class DesdeHastaView : UserControl
    {
        public static readonly DependencyProperty SeleccionarGestorCommandProperty = DependencyProperty.Register("SeleccionarGestorCommand", typeof(ICommand), typeof(DesdeHastaView), new PropertyMetadata(null));
        public static readonly DependencyProperty RestablecerCommandProperty = DependencyProperty.Register("RestablecerCommand", typeof(ICommand), typeof(DesdeHastaView), new PropertyMetadata(null));

        public ICommand SeleccionarGestorCommand
        {
            get
            {
                return (ICommand)GetValue(SeleccionarGestorCommandProperty);

            }
            set { SetValue(SeleccionarGestorCommandProperty, value); }
        }

        public ICommand RestablecerCommand
        {
            get
            {
                return (ICommand)GetValue(RestablecerCommandProperty);

            }
            set { SetValue(RestablecerCommandProperty, value); }
        }

        public DesdeHastaView()
        {
            InitializeComponent();
            PasswordControl.Focus();
        }


        private void SeleccionarGestor(object sender, RoutedEventArgs e)
        {
            if (SeleccionarGestorCommand != null)
            {
                SeleccionarGestorCommand.Execute(PasswordControl);
            }
        }

        private void Restablecer(object sender, RoutedEventArgs e)
        {
            if (RestablecerCommand != null)
            {
                RestablecerCommand.Execute(PasswordControl);
            }
        }



    }
}
