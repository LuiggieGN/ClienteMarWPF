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

namespace ClienteMarWPFWin7.UI.Modules.Reporte.Modal
{
    public partial class DialogoReporte : UserControl
    {

        private bool _padreFueHabilitado = true;


        public bool CargarDialogo
        {
            get
            {
                return (bool)GetValue(CargarDialogoProperty);
            }
            set
            {
                SetValue(CargarDialogoProperty, value);
            }
        }

        public UIElement OverlayOn
        {
            get
            {
                return (UIElement)GetValue(OverlayOnProperty);
            }
            set
            {
                SetValue(OverlayOnProperty, value);
            }
        }

        public ICommand AceptarCommand
        {
            get
            {
                return (ICommand)GetValue(AceptarCommandProperty);
            }
            set { SetValue(AceptarCommandProperty, value); }
        }

        public ICommand CancelarCommand
        {
            get
            {
                return (ICommand)GetValue(CancelarCommandProperty);
            }
            set { SetValue(CancelarCommandProperty, value); }
        }

        public static readonly DependencyProperty CargarDialogoProperty = DependencyProperty.Register("CargarDialogo", typeof(bool), typeof(DialogoReporte), new UIPropertyMetadata(false, CargarDialogoChanged));
        public static readonly DependencyProperty OverlayOnProperty = DependencyProperty.Register("OverlayOn", typeof(UIElement), typeof(DialogoReporte), new UIPropertyMetadata(null));
        public static readonly DependencyProperty AceptarCommandProperty = DependencyProperty.Register("AceptarCommand", typeof(ICommand), typeof(DialogoReporte), new PropertyMetadata(null));
        public static readonly DependencyProperty CancelarCommandProperty = DependencyProperty.Register("CancelarCommand", typeof(ICommand), typeof(DialogoReporte), new PropertyMetadata(null));

        public static void CargarDialogoChanged(DependencyObject modal, DependencyPropertyChangedEventArgs e)
        {
            var verdialogo = (bool)e.NewValue;
            var dialogo = (DialogoReporte)modal;

            if (verdialogo)
            {
                dialogo.Mostrar();
            }
            else
            {
                dialogo.Ocultar();
            }

        }


        public void Mostrar() 
        {
            var bindingExpresionOverlayOn = this.GetBindingExpression(OverlayOnProperty);

            if (bindingExpresionOverlayOn != null)
            {
                bindingExpresionOverlayOn.UpdateTarget();
            }
            if (OverlayOn == null)
            {
                throw new InvalidOperationException("Las propiedades necesarias no están vinculadas al modelo.");
            }

            Visibility = Visibility.Visible;
            _padreFueHabilitado = OverlayOn.IsEnabled;
            OverlayOn.IsEnabled = false;
        }


        public void Ocultar() 
        {
            Visibility = Visibility.Hidden;
            OverlayOn.IsEnabled = _padreFueHabilitado;
        }



        public DialogoReporte()
        {
            InitializeComponent();
            Visibility = Visibility.Hidden;
        }


        private void Aceptar(object sender, RoutedEventArgs e)
        {
            if (AceptarCommand != null)
            {
                AceptarCommand.Execute(null);
            }
        }

        private void Cancelar(object sender, RoutedEventArgs e)
        {
            if (CancelarCommand != null)
            {
                CancelarCommand.Execute(null);
            }
        }


    }//fin de clase
}
