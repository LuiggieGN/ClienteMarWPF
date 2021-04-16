using ClienteMarWPFWin7.UI.ViewModels.Helpers;
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

namespace ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Movimiento.Views.View2
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
        }

        public void CuandoVistaCarga()
        {
            var vm = DataContext as DesdeHastaViewModel;
            if (vm != null)
            {
                vm.FocusEnCambioDeTipoDeTransferencia = () =>
                {
                    PasswordControl.Focus();
                };
                vm.FocusOperacionCompletada = () =>
                {
                    TxtComentario.Focus();
                };
                vm.FocusAlFallar = () =>   //-Falta por implementar
                {
                    TxtMonto.Focus();
                };

                vm.FocusCuandoHayErrorEnElModeloACrear = () =>
                {
                    TxtMonto.Focus();

                    bool  
                         inputComentario = InputHelper.InputIsBlank(TxtComentario.Text),
                         inputCajera = false,
                         inputMonto = InputHelper.InputIsBlank(TxtMonto.Text);


                    if (TxtCajera.IsVisible)
                    {
                        inputCajera = InputHelper.InputIsBlank(TxtCajera.Text);
                    }

                    if (inputComentario)
                    {
                        TxtComentario.Focus(); return;
                    }

                    if (inputCajera)
                    {
                        TxtCajera.Focus(); return;
                    }

                    if (inputMonto)
                    {
                        TxtMonto.Focus(); return;
                    }
                };


            }
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
        private void FallowNext()
        {
            if (ComboTransferir.IsFocused)
            {
                PasswordControl.Focus();
            }
            else if (PasswordControl.IsFocused || TxtInfoGestor1.IsFocused || TxtInfoBanca1.IsFocused || TxtInfoBanca2.IsFocused || TxtInfoGestor2.IsFocused)
            {
                TxtComentario.Focus();
            }
            else if (TxtComentario.IsFocused)
            {
                if (TxtCajera.IsVisible)
                {
                    TxtCajera.Focus();
                }
                else
                {
                    TxtMonto.Focus();
                }
            }
            else if (TxtCajera.IsFocused)
            {
                TxtMonto.Focus();
            }
            else if (TxtMonto.IsFocused)
            {
                ComboTransferir.Focus();
            }
        }



        public void TriggerSeleccionarGestor() 
        {
            SeleccionarGestor(null, null);
        }

        public void TriggerRestablecer()
        {
            Restablecer(null, null);
        }



        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                FallowNext();
            }
            else if (e.Key == Key.Tab)
            {
                e.Handled = true;
                FallowNext();
            }
        }



        private void OnFocus(object sender, RoutedEventArgs e)
        {
            UIElement elementToSave = sender as UIElement;
            MovimientoView.Tab1_LastFocusControl = elementToSave;
        }





    }
}
