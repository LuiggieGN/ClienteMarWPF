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

using System.Threading;
using System.Globalization;
using System.Windows.Controls.Primitives;

namespace ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Movimiento.Views.View3
{
    public partial class ConsultaMovimientoView : UserControl
    {
        public ConsultaMovimientoView()
        {
            InitializeComponent();
        }

        public void CuandoVistaCarga()
        {
            var vm = DataContext as ConsultaMovimientoViewModel;
            if (vm != null)
            {
                vm.FocusOperacionCompletada = () =>
                {
                    var picker2 = DatePickerFechaFin.Template.FindName("PART_TextBox", DatePickerFechaFin) as DatePickerTextBox;
                    if (picker2 != null) {
                        picker2.Focus();
                    } 
                };

                vm.FocusOperacionFallida = () =>
                {
                    var picker2 = DatePickerFechaFin.Template.FindName("PART_TextBox", DatePickerFechaFin) as DatePickerTextBox;
                    if (picker2 != null) {
                        picker2.Focus();
                    }
                };
            }
        }

        private void FallowNext()
        {
            var picker1 = DatePickerFechaInicio.Template.FindName("PART_TextBox", DatePickerFechaInicio) as DatePickerTextBox;
            var picker2 = DatePickerFechaFin.Template.FindName("PART_TextBox", DatePickerFechaFin) as DatePickerTextBox;

            if (picker1 != null && picker2 != null)
            {
                if (DatePickerFechaInicio.IsFocused || picker1.IsFocused)
                {
                    picker2.Focus();
                }
                else if (DatePickerFechaFin.IsFocused || picker2.IsFocused)
                {
                    picker1.Focus();
                }
            }
        }


        private void OnKeyUp(object sender, KeyEventArgs e) 
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                FallowNext();
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                e.Handled = true;
                FallowNext();
            }
        }

        private void OnKeyboardGotFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            UIElement elementToSave = sender as UIElement;
            MovimientoView.Tab2_LastFocusControl = elementToSave;
        }




    }
}
