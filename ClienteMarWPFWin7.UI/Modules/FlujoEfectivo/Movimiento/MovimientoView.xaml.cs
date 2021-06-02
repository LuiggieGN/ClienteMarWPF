using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Movimiento.Views.View1;
using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Movimiento.Views.View2;
using System;
using System.Linq;
using System.Collections.Generic; 
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Movimiento
{
    public partial class MovimientoView : UserControl
    {
        private bool FlatCargando1 = true;
        private bool FlatCargando2 = false;
        private bool FlatCargando3 = false;

        private int TabSelectedIndex;
        private string TabKey;

        public static UIElement Tab0_LastFocusControl = null;
        public static UIElement Tab1_LastFocusControl = null;
        public static UIElement Tab2_LastFocusControl = null;

        Dictionary<int, string> TrackingTabs;

        public MovimientoView()
        {
            TrackingTabs = new Dictionary<int, string>();

            InitializeComponent();

            FlatCargando1 = true;
            FlatCargando2 = false;
            FlatCargando3 = false;

            TabSelectedIndex = 0;

            TabsControl.SelectedIndex = TabSelectedIndex;

            Tab0_LastFocusControl = null;
            Tab1_LastFocusControl = null;
            Tab2_LastFocusControl = null;

            LeePermisoDeControlDeEfectivo();
        }

        private void CuandoTabCambia(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource == TabsControl)
            {
                TabSelectedIndex = TabsControl.SelectedIndex;

                TabKey = TrackTabSeleccionado(indiceDeSeguimiento: TabSelectedIndex);

                IniciaFocusAlCambiarTab();
            }
        }


        private void IniciaFocusAlCambiarTab()
        {

            var vm = DataContext as MovimientoViewModel;

            switch (TabKey)
            {
                case "Tab_0": //@@                                                                        - Modulo de Registro de Ingresos y Gastos

                    FlatCargando1 = true;
                    FlatCargando2 = false;
                    FlatCargando3 = false;

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


                case "Tab_1": //@@                                                                        - Modulo de Registro de Entrega y Recibo de Dinero

                    FlatCargando1 = false;
                    FlatCargando2 = true;
                    FlatCargando3 = false;

                    var control2 = UserControl2;

                    if (control2 != null &&
                        control2.IsLoaded)
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
                case "Tab_2": //@@                                                                        - Modulo de Consulta de Movimientos

                    FlatCargando1 = false;
                    FlatCargando2 = false;
                    FlatCargando3 = true;

                    var control3 = UserControl3;

                    if (control3 != null &&
                        control3.IsLoaded)
                    {
                        var PickerInicio = control3.DatePickerFechaInicio;

                        var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(400) };

                        if (Tab2_LastFocusControl == null)
                        {
                            timer.Tick += (sender, args) =>
                            {
                                timer.Stop();
                                PickerInicio.Focus();                                
                            };
                        }
                        else
                        {
                            timer.Tick += (sender, args) =>
                            {
                                timer.Stop();
                                Tab2_LastFocusControl.Focus();
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

        private void Carga3(object sender, RoutedEventArgs e)
        {
            if (FlatCargando3 == true)
            {
                var consultamovimientoview = UserControl3;
                if (consultamovimientoview != null)
                {
                    consultamovimientoview.CuandoVistaCarga();
                }

                IniciaFocusAlCambiarTab();
            }
        }


        private void LeePermisoDeControlDeEfectivo()
        {
            var ventaPrincipal = Application.Current.MainWindow;
            var vm = ventaPrincipal.DataContext as MainWindowViewModel;

            if (vm != null)
            {
                PermisosDTO permisos = vm.AutService.Permisos;

                TrackingTabs[0] = "Tab_0"; //Tab Entrda y Salida de Dinero
                TrackingTabs[1] = "Tab_1"; //Tab Entrega de Efectivo
                TrackingTabs[2] = "Tab_2"; //Tab Consulta de Movimiento


                if (
                      permisos.Permiso_Flujo_Efectivo_Registrar_Movimiento == false &&
                      permisos.Permiso_Flujo_Efectivo_Entrega_De_Efectivo  == false
                   )
                {
                    TrackingTabs = new Dictionary<int, string>();
                    TrackingTabs[0] = "Tab_2";
                }
                else if (permisos.Permiso_Flujo_Efectivo_Registrar_Movimiento == true &&
                         permisos.Permiso_Flujo_Efectivo_Entrega_De_Efectivo  == false)
                {
                    TrackingTabs = new Dictionary<int, string>();
                    TrackingTabs[0] = "Tab_0";
                    TrackingTabs[1] = "Tab_2";
                }
                else if (permisos.Permiso_Flujo_Efectivo_Registrar_Movimiento == false &&
                         permisos.Permiso_Flujo_Efectivo_Entrega_De_Efectivo == true)
                {
                    TrackingTabs = new Dictionary<int, string>();
                    TrackingTabs[0] = "Tab_1";
                    TrackingTabs[1] = "Tab_2";
                }


                #region Removiendo Tabs No Permitidos
                if (permisos.Permiso_Flujo_Efectivo_Registrar_Movimiento == false) // Oculto Registrar (E/S) de dinero
                {
                    UserControl1.Visibility = Visibility.Collapsed;
                    TabsControl.Items.Remove(TabOne);
                }

                if (permisos.Permiso_Flujo_Efectivo_Entrega_De_Efectivo == false) // Oculto Entrega y Recibo de dinero
                {
                    UserControl2.Visibility = Visibility.Collapsed;
                    TabsControl.Items.Remove(TabTwo);
                }
                #endregion


                if (permisos.Permiso_Flujo_Efectivo_Registrar_Movimiento == true)
                {
                    SetearTabSeleccionado(cargando: 1, tab: "Tab_0");
                }
                else if(permisos.Permiso_Flujo_Efectivo_Entrega_De_Efectivo == true)
                {
                    SetearTabSeleccionado(cargando: 2, tab: "Tab_1");
                }
                else
                {
                    SetearTabSeleccionado(cargando: 3, tab: "Tab_2");
                }








            }//fin de if vm != null       
        }

        private void SetearTabSeleccionado(int cargando, string tab) 
        {
            FlatCargando1 = cargando == 1 ? true : false;
            FlatCargando2 = cargando == 2 ? true : false;
            FlatCargando3 = cargando == 3 ? true : false;

            TabKey = tab;

            TabSelectedIndex = LeerKeyDesdeElValor(tab);

            TabsControl.SelectedIndex = TabSelectedIndex;

            Tab0_LastFocusControl = null;
            Tab1_LastFocusControl = null;
            Tab2_LastFocusControl = null;
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (TabKey == "Tab_0")// @@                                              
            {
                if (e.Key == Key.F9 || (e.Key == Key.System && e.SystemKey == Key.F9))
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
                     (e.Key == Key.F12 || (e.Key == Key.System && e.SystemKey == Key.F12)) ||
                     (e.Key == Key.Add || (e.Key == Key.System && e.SystemKey == Key.Add))
                     //||(e.Key == Key.OemPlus || (e.Key == Key.System && e.SystemKey == Key.OemPlus))
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
            else if (TabKey == "Tab_1")
            {
                if (e.Key == Key.F5 || (e.Key == Key.System && e.SystemKey == Key.F5))
                {
                    e.Handled = true;
                    var vm = DataContext as MovimientoViewModel;
                    if (vm != null)
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
                     (e.Key == Key.Add || (e.Key == Key.System && e.SystemKey == Key.Add)) 
                     
                     //||(e.Key == Key.OemPlus || (e.Key == Key.System && e.SystemKey == Key.OemPlus))
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


            }
            else if (TabKey == "Tab_2")
            {
                if (e.Key == Key.F5 || (e.Key == Key.System && e.SystemKey == Key.F5))
                {
                    e.Handled = true;
                    var vm = DataContext as MovimientoViewModel;
                    if (vm != null && vm.View3 != null && vm.View3.From.HasValue && vm.View3.To.HasValue)
                    {
                        vm.View3.ConsultarMovimientosCommand?.Execute(null);
                    }// fin if Vm
                }
                else if (e.Key == Key.F6 || (e.Key == Key.System && e.SystemKey == Key.F6))
                {
                    e.Handled = true;

                    if (UserControl3 != null)
                    {
                        UserControl3.DatePickerFechaInicio.IsDropDownOpen = true;
                    }
                }
                else if (e.Key == Key.F7 || (e.Key == Key.System && e.SystemKey == Key.F7))
                {
                    e.Handled = true;

                    if (UserControl3 != null)
                    {
                        UserControl3.DatePickerFechaFin.IsDropDownOpen = true;
                    }
                }
            }

        }//fin de evento OnPreviewKeyDown




        private string TrackTabSeleccionado(int indiceDeSeguimiento) 
        {
            if (TrackingTabs == null || TrackingTabs.Count == 0)
            {
                return string.Empty;
            }

            if (TrackingTabs.ContainsKey(indiceDeSeguimiento))
            {
                return TrackingTabs[indiceDeSeguimiento];
            }
            else
            {
                return string.Empty;
            }
        }


        private int LeerKeyDesdeElValor(string tab) 
        {
            if (TrackingTabs == null || TrackingTabs.Count == 0)
            {
                return -1;
            }

            try
            {
              return TrackingTabs.FirstOrDefault(x => x.Value == tab).Key;

            }
            catch {  

                return -1;
            }        
        }





    }//fin de clase
}
