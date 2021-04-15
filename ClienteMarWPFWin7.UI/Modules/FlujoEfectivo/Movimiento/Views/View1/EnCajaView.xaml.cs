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

namespace ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Movimiento.Views.View1
{
    public partial class EnCajaView : UserControl
    {
        public EnCajaView()
        {
            InitializeComponent();
        }

        private void CuandoComboConceptoCambia()
        {
            if (txtConcepto != null && txtConcepto.IsVisible)
            {
                txtConcepto.Focus();
            }
            else
            {
                txtComentario.Focus();
            }
        }

        private void CuandoComboQueHarasCambia()
        {
            if (txtCajera != null && txtCajera.IsVisible)
            {
                txtCajera.Focus();
            }
            else
            {
                txtMonto.Focus();
            }
        }



        public void CuandoVistaCarga()
        {
            var vm = DataContext as EnCajaViewModel;
            if (vm != null)
            {
                vm.FocusEnCambioDeConcepto = () =>
                {
                    CuandoComboConceptoCambia();
                };

                vm.FocusAlAgregarMovimiento = () =>
                {
                    txtComentario.Focus();
                };

                vm.FocusAlFallar = () =>
                {
                    txtMonto.Focus();
                };

                vm.FocusCuandoHayErrorEnElModeloACrear = () => 
                {
                    bool inputConcepto = false,
                         inputComentario = InputHelper.InputIsBlank(txtComentario.Text),
                         inputCajera = false,
                         inputMonto = InputHelper.InputIsBlank(txtMonto.Text);

                    if (txtConcepto.IsVisible)
                    {
                        inputConcepto = InputHelper.InputIsBlank(txtConcepto.Text);
                    }

                    if (txtCajera.IsVisible)
                    {
                        inputCajera = InputHelper.InputIsBlank(txtCajera.Text);
                    }

                    if (inputConcepto)
                    {
                        txtConcepto.Focus(); return;
                    }

                    if (inputComentario)
                    {
                        txtComentario.Focus(); return;
                    }

                    if (inputCajera)
                    {
                        txtCajera.Focus(); return;
                    }

                    if (inputMonto)
                    {
                        txtMonto.Focus(); return;
                    }
                };


            }//fin de if
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is ComboBox)
            {
                if (e.Key == Key.Enter)
                {
                    e.Handled = true;

                    //if (!Submit()) // -- Aqui valido si se puede hacer el submit y lo realizo de lo contrario se enfoca el proximo
                    //{
                    //    FallowNext();
                    //}
                    FallowNext();
                }
                else if (e.Key == Key.Tab)
                {
                    e.Handled = true;                    
                    FallowNext();
                }
            }
            else if (sender is TextBox)
            {

                var input = sender as TextBox;

                if (e.Key == Key.Enter)
                {
                    e.Handled = true;

                    //if (!Submit()) // -- Aqui valido si se puede hacer el submit y lo realizo de lo contrario se enfoca el proximo
                    //{
                    //    FallowNext();
                    //}
                    FallowNext();

                }
                else if (e.Key == Key.Tab)
                {
                    e.Handled = true;

                    FallowNext();
                }
            }
        }


        //private bool Submit()  // Esto realiza el submit de los datos 
        //{
        //    bool canSubmit = false;

        //    bool combo1 = ComboQueHaras.SelectedItem != null,
        //         combo2 = ComboQueHaras.SelectedItem != null,
        //         inputConcepto = true,
        //         inputComentario = !InputHelper.InputIsBlank(txtComentario.Text),
        //         inputCajera = true,
        //         inputMonto = !InputHelper.InputIsBlank(txtMonto.Text);

        //    if (txtConcepto.IsVisible)
        //    {
        //        inputConcepto = !InputHelper.InputIsBlank(txtConcepto.Text);
        //    }

        //    if (txtCajera.IsVisible)
        //    {
        //        inputCajera = !InputHelper.InputIsBlank(txtCajera.Text);
        //    }

        //    if (combo1 && combo2 && inputConcepto && inputComentario && inputCajera && inputMonto)
        //    {
        //        canSubmit = true;

        //        var vm = DataContext as EnCajaViewModel;

        //        if (vm != null)
        //        {
        //            vm.AgregarMovimientoEnCajaCommand.Execute(null);
        //        }
        //    }

        //    return canSubmit;        
        //}

        private void FallowNext() 
        {
            if (ComboQueHaras.IsFocused)
            {
                ComboConcepto.Focus();
            }
            else if (ComboConcepto.IsFocused)
            {
                CuandoComboConceptoCambia();
            }
            else if (txtConcepto.IsFocused)
            {
                txtComentario.Focus();
            }
            else if (txtComentario.IsFocused)
            {
                CuandoComboQueHarasCambia();
            }
            else if (txtCajera.IsFocused)
            {
                txtMonto.Focus();
            }
            else if (txtMonto.IsFocused)
            {
                ComboQueHaras.Focus();
            }
        }

        private void OnFocus(object sender, RoutedEventArgs e)
        {
            UIElement elementToSave = sender as UIElement;
            MovimientoView.Tab0_LastFocusControl = elementToSave;
        }







    }
}
