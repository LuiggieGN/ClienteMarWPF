﻿using System;
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
using System.Windows.Threading;

namespace ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Movimiento.Modal
{
    public partial class DialogoToken : UserControl
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

        public static readonly DependencyProperty CargarDialogoProperty = DependencyProperty.Register("CargarDialogo", typeof(bool), typeof(DialogoToken), new UIPropertyMetadata(false, CargarDialogoChanged));
        public static readonly DependencyProperty OverlayOnProperty = DependencyProperty.Register("OverlayOn", typeof(UIElement), typeof(DialogoToken), new UIPropertyMetadata(null));
        public static readonly DependencyProperty AceptarCommandProperty = DependencyProperty.Register("AceptarCommand", typeof(ICommand), typeof(DialogoToken), new PropertyMetadata(null));

        public static void CargarDialogoChanged(DependencyObject modal, DependencyPropertyChangedEventArgs e)
        {
            var verdialogo = (bool)e.NewValue;
            var dialogo = (DialogoToken)modal;

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

            PasswordPin.Focus();
        }


        public void Ocultar() 
        {
            Visibility = Visibility.Hidden;
            OverlayOn.IsEnabled = _padreFueHabilitado;
        }



        public DialogoToken()
        {
            InitializeComponent();
            Visibility = Visibility.Hidden;
            PasswordPin.Focus();

            //var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(400) };
            //timer.Tick += (sender, args) =>
            //{
            //    timer.Stop();
            //    PasswordPin.Focus();
            //};

            //timer.Start();


        }


        private void Aceptar(object sender, RoutedEventArgs e)
        {
            if (AceptarCommand != null)
            {
                AceptarCommand.Execute(PasswordPin);
            }
        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(400) };
            timer.Tick += (s1, args) =>
            {
                timer.Stop();
                PasswordPin.Focus();
            };

            timer.Start();
        }

        private void PasswordPin_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Aceptar(sender, e);
            }
        }

    }//fin de clase
}
