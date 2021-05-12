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

        private void PressTecla(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:

                    if (FechaInicio.IsKeyboardFocusWithin==true)
                    {
                        FechaInicio.IsDropDownOpen = true;
                        FechaFin.IsDropDownOpen = false;
                    }
                    if (FechaFin.IsKeyboardFocusWithin == true)
                    {
                        FechaInicio.IsDropDownOpen = false;
                        FechaFin.IsDropDownOpen = true;
                    }
                    if (SoloTotales.IsFocused)
                    {
                        if (SoloTotales.IsChecked==true)
                        {
                            SoloTotales.IsChecked = false;
                        }else
                        if (SoloTotales.IsChecked == false)
                        {
                            SoloTotales.IsChecked = true;
                        }
                    }
                    break;

                case Key.Down:
                    if (FechaInicio.IsKeyboardFocusWithin == true && FechaInicio.IsDropDownOpen == false)
                    {
                        FechaFin.Focus();
                    }else if (FechaFin.IsKeyboardFocusWithin == true && FechaFin.IsDropDownOpen == false)
                    {
                        SoloTotales.Focus();
                    }else if (SoloTotales.IsFocused==true)
                    {
                        btnCancelar.Focus();
                    }
                    break;

                case Key.Up:
                    if (btnCancelar.IsFocused || btnAceptar.IsFocused)
                    {
                        SoloTotales.Focus();
                    }else if (SoloTotales.IsFocused)
                    {
                        FechaFin.Focus();
                    }else if (FechaFin.IsKeyboardFocusWithin && FechaFin.IsDropDownOpen == false)
                    {
                        FechaInicio.Focus();
                    }
                    else if (FechaInicio.IsKeyboardFocusWithin)
                    {
                        
                    }

                    break;

                case Key.Right:
                    if (btnCancelar.IsFocused)
                    {
                        btnAceptar.Focus();
                    }
                    if (FechaFin.IsKeyboardFocusWithin==true)
                    {
                        SoloTotales.Focus();
                    }
                    break;
                case Key.Left:
                    if (btnAceptar.IsFocused)
                    {
                        btnCancelar.Focus();
                    }
                    break;
                case Key.Escape:
                    Cancelar(sender,e);
                    break;
            }

        }
    }//fin de clase
}
