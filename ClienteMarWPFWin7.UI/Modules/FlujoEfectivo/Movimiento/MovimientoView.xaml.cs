using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Movimiento.Views.View1;
using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Movimiento.Views.View2;
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
        private bool FlatCargando1 = true;
        private bool FlatCargando2 = false;

        private int TabSelectedIndex;

        public static UIElement Tab0_LastFocusControl = null;
        public static UIElement Tab1_LastFocusControl = null;

        public MovimientoView()
        {
            InitializeComponent();

            FlatCargando1 = true;
            FlatCargando2 = false;

            TabSelectedIndex = 0;
            TabsControl.SelectedIndex = TabSelectedIndex;

            Tab0_LastFocusControl = null;
            Tab1_LastFocusControl = null;
        }

        private void CuandoTabCambia(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource == TabsControl)
            {
                TabSelectedIndex = TabsControl.SelectedIndex; 

                IniciaFocusAlCambiarTab();
            }
        }


        private void IniciaFocusAlCambiarTab()
        {

            var vm = DataContext as MovimientoViewModel;

            switch (TabSelectedIndex)
            {
                case 0: //@@                                                                        - Modulo de Registro Ingresos y Gastos

                    FlatCargando1 = true;
                    FlatCargando2 = false;

                    var control1 = UserControl1; 

                    if (control1 != null &&
                        control1.IsLoaded)
                    {                   
                        var TxtComentario = control1.txtComentario;
                        var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(400) };

                        if (Tab0_LastFocusControl == null)
                        {
                            timer.Tick += (sender, args) =>
                            {
                                timer.Stop();
                                TxtComentario.Focus();
                            };
                        }
                        else
                        {
                            timer.Tick += (sender, args) =>
                            {
                                timer.Stop();
                                Tab0_LastFocusControl.Focus();
                            };
                        }
                        timer.Start();
                    }
                    break;


                case 1: //@@                                                                        - Modulo de Registro Entrega y Recibo de Dinero

                    FlatCargando1 = false;
                    FlatCargando2 = true;

                    var control2 = UserControl2; 

                    if (control2 != null &&
                        control2.IsLoaded )
                    {
                        var PassGestor = control2.PasswordControl;
 
                        var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(400) };

                        if (Tab1_LastFocusControl == null)
                        {
                            timer.Tick += (sender, args) =>
                            {
                                timer.Stop();
                                PassGestor.Focus();
                            };
                        }
                        else
                        {
                            timer.Tick += (sender, args) =>
                            {
                                timer.Stop();
                                Tab1_LastFocusControl.Focus();
                            };
                        }
                        timer.Start();
                    }
                    break;



                default:
                    break;
            }
        }

        private void Carga1(object sender, RoutedEventArgs e)
        {
            if (FlatCargando1 == true)
            {
                var encajaview = UserControl1;
                if (encajaview != null)
                {
                    encajaview.CuandoVistaCarga();
                }

                IniciaFocusAlCambiarTab();
            }
        }
        private void Carga2(object sender, RoutedEventArgs e)
        {
            if (FlatCargando2 == true)
            {
                var desdehastaview = UserControl2;
                if (desdehastaview != null)
                {
                    desdehastaview.CuandoVistaCarga();
                }

                IniciaFocusAlCambiarTab();
            }
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (TabSelectedIndex == 0)// @@                                              - Modulo de Registro Ingresos y Gastos
            {
                if ( e.Key == Key.F9 || (e.Key == Key.System && e.SystemKey == Key.F9))
                {
                    e.Handled = true;
                    var vm = DataContext as MovimientoViewModel;
                    var encajaviewmodel = vm?.View1 ?? (EnCajaViewModel)null;
                    if (vm != null && encajaviewmodel != null && UserControl1.IsLoaded)
                    {
                        encajaviewmodel.RestablecerCommand?.Execute(null);
                    }
                }
                else if (
                     ( e.Key == Key.F12 || (e.Key == Key.System && e.SystemKey == Key.F12)) ||
                     ( e.Key == Key.Add || (e.Key == Key.System && e.SystemKey == Key.Add)  ) ||
                     ( e.Key == Key.OemPlus || (e.Key == Key.System && e.SystemKey == Key.OemPlus)  ) 
                )
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
            else if (TabSelectedIndex == 1)
            {
                if (e.Key == Key.F5 || (e.Key == Key.System && e.SystemKey == Key.F5))
                {
                    e.Handled = true;
                    var vm = DataContext as MovimientoViewModel;
                    if (vm!= null)
                    {
                        var desdehastaview = UserControl2;

                        if (vm.Dialog == null)
                        {
                            desdehastaview.TriggerSeleccionarGestor();
                        }
                        else
                        {
                            if (vm.Dialog.MuestroDialogo == false)
                            {
                                desdehastaview.TriggerSeleccionarGestor();
                            }
                        }
                    }// fin if Vm
                }
                else if (e.Key == Key.F9 || (e.Key == Key.System && e.SystemKey == Key.F9))
                {
                    e.Handled = true;
                    var vm = DataContext as MovimientoViewModel;
                    if (vm != null)
                    {
                        var desdehastaview = UserControl2;

                        if (vm.Dialog == null)
                        {
                            desdehastaview.TriggerRestablecer();
                        }
                        else
                        {
                            if (vm.Dialog.MuestroDialogo == false)
                            {
                                desdehastaview.TriggerRestablecer();
                            }
                        }
                    }// fin if Vm
                }
                else if (
                     (e.Key == Key.F12 || (e.Key == Key.System && e.SystemKey == Key.F12)) ||
                     (e.Key == Key.Add || (e.Key == Key.System && e.SystemKey == Key.Add)) ||
                     (e.Key == Key.OemPlus || (e.Key == Key.System && e.SystemKey == Key.OemPlus))
                )
                {
                    e.Handled = true;
                    var vm = DataContext as MovimientoViewModel;
                    var v2 = vm?.View2 ?? (DesdeHastaViewModel)null;

                    if (vm != null && v2 != null)
                    {
                        if (v2.MuestroBotonNuevaTrasferencia)
                        {
                            if (vm.Dialog == null)
                            {
                                v2.AbrirModalTokenCommand?.Execute(null);
                            }
                            else
                            {
                                if (vm.Dialog.MuestroDialogo == false)
                                {
                                    v2.AbrirModalTokenCommand?.Execute(null);
                                }
                            }
                        }
                    }// fin if Vm
                }


            }//fin else if TabSelectedIndex == 1







        }










    }
}
