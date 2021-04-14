using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Movimiento.Views.View1;
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
using System.Windows.Threading;

namespace ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Movimiento
{
    public partial class MovimientoView : UserControl
    {
        private int TabSelectedIndex;

        public MovimientoView()
        {
            InitializeComponent();

            TabSelectedIndex = 0;

            TabsControl.SelectedIndex = TabSelectedIndex;
        }

        private void CuandoTabCambia(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource == TabsControl)
            {
                TabSelectedIndex = TabsControl.SelectedIndex;

                AplicarLogicaInicioDeFocusSegunTabSeleccionado();
            }
        }


        private void AplicarLogicaInicioDeFocusSegunTabSeleccionado()
        {

            var vm = DataContext as MovimientoViewModel;

            switch (TabSelectedIndex)
            {
                case 0: //@@                                          - Modulo de Registro Ingresos y Gastos

                    var control1 = UserControl1;
                    var control1Contexto = vm?.View1 ?? (EnCajaViewModel)null;

                    if (control1 != null &&
                        control1.IsLoaded &&
                        control1Contexto != null &&
                        control1Contexto.InputConcepto != null)
                    {
                        var TxtConcepto = control1.txtConcepto;
                        var TxtComentario = control1.txtComentario;
                        var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };

                        if (control1Contexto.InputConcepto.Muestro)
                        {
                            timer.Tick += (sender, args) =>
                            {
                                timer.Stop();
                                TxtConcepto.Focus();
                            };
                        }
                        else
                        {
                            timer.Tick += (sender, args) =>
                            {
                                timer.Stop();
                                TxtComentario.Focus();
                            };                            
                        }

                        timer.Start();
                    }
                    break;
                default:
                    break;
            }
        }

        private void CuandoUserControl1Carga(object sender, RoutedEventArgs e)
        {
            var encajaview = UserControl1;
 
            if (encajaview != null )
            {
                encajaview.CuandoVistaCarga();
            }

           AplicarLogicaInicioDeFocusSegunTabSeleccionado();
        }

        private void OnTeclaKeyDownModuloMovimiento(object sender, KeyEventArgs e)
        {
            if (TabSelectedIndex == 0)//@@                                          - Modulo de Registro Ingresos y Gastos
            {
                if ( e.Key == Key.F5 || (e.Key == Key.System && e.SystemKey == Key.F5))
                {
                    e.Handled = true;
                    var vm = DataContext as MovimientoViewModel;
                    var encajaviewmodel = vm?.View1 ?? (EnCajaViewModel)null;
                    if (vm != null && encajaviewmodel != null && UserControl1.IsLoaded)
                    {
                        encajaviewmodel.RestablecerCommand?.Execute(null);
                    }
                }
                else if (e.Key == Key.F6 || (e.Key == Key.System && e.SystemKey == Key.F6))
                {
                    e.Handled = true;
                    var vm = DataContext as MovimientoViewModel;
                    var encajaviewmodel = vm?.View1 ?? (EnCajaViewModel)null;
                    if (vm != null && encajaviewmodel != null && UserControl1.IsLoaded)
                    {
                        encajaviewmodel.AgregarMovimientoEnCajaCommand?.Execute(null);
                    }
                }
            }
        }






    }
}
