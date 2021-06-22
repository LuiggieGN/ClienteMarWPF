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

namespace ClienteMarWPFWin7.UI.Modules.Recargas.Modal
{
    /// <summary>
    /// Interaction logic for DialogImprimirTicket.xaml
    /// </summary>
    public partial class DialogImprimirTicket : UserControl
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

        public static readonly DependencyProperty CargarDialogoProperty = DependencyProperty.Register("CargarDialogo", typeof(bool), typeof(DialogImprimirTicket), new UIPropertyMetadata(false, CargarDialogoChanged));
        public static readonly DependencyProperty OverlayOnProperty = DependencyProperty.Register("OverlayOn", typeof(UIElement), typeof(DialogImprimirTicket), new UIPropertyMetadata(null));


        public static void CargarDialogoChanged(DependencyObject modal, DependencyPropertyChangedEventArgs e)
        {
            
            var verdialogo = (bool)e.NewValue;
            var dialogo = (DialogImprimirTicket)modal;

            if (verdialogo)
            {
                dialogo.Mostrar();
                dialogo.botonSalir.Focus();
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
            var recargas = new RecargasView();
            Visibility = Visibility.Hidden;
            OverlayOn.IsEnabled = _padreFueHabilitado;
            recargas.telefono.Focus();
        }



        public DialogImprimirTicket()
        {
            InitializeComponent();
            Visibility = Visibility.Hidden;
            //botonSalir.Focusable = true;
            //botonSalir.Focus();
            FocusManager.SetIsFocusScope(botonSalir, false);
        }

    }
}
