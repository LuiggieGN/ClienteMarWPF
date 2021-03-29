
using ClienteMarWPFWin7.Domain.Enums;
using ClienteMarWPFWin7.UI.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ClienteMarWPFWin7.UI
{



    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {



        public MainWindow(MainWindowViewModel dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;

        }

        private void CerrarAplicacion_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }

        private void OpenMenu_Click(object sender, RoutedEventArgs e)
        {
            OpenMenu();
        }
        private void CloseMenu_Click(object sender, RoutedEventArgs e)
        {

            foreach (Button btn in FindVisualChildren<Button>(this))
            {
                if (btn != null)
                {
                    if (btn.Name == "CloseMenuSTB")
                    {
                        btn.Visibility = Visibility.Collapsed;
                    }
                    else if (btn.Name == "OpenMenuSTB")
                    {
                        btn.Visibility = Visibility.Visible;
                    }

                }
            }

        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public void MensajesAlerta(string mensajeIn, string title = "Alerta", string color = "#FFA00101", int time = 3000)
        {
            bxBody.Text = mensajeIn;
            bxTitle.Content = title;
            bxMensaje.Visibility = Visibility.Visible;
            try
            {
                SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                mySolidColorBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(color));
                bxMensaje.Background = mySolidColorBrush;
            }
            catch (Exception)
            {

                bxMensaje.Background = Brushes.Red;
            }



            var aTimer = new System.Timers.Timer(time);
            aTimer.Elapsed += (sender, e) =>
            {

                Dispatcher.Invoke(new Action(() => { bxMensaje.Visibility = Visibility.Hidden; }), DispatcherPriority.ContextIdle);
            };
            aTimer.AutoReset = false;
            aTimer.Enabled = true;
        }

        public void TriggerOpenMenu()
        {
            try
            {
                var botonOpenMenu = GetButtonOpenMenuSTB();

                if (botonOpenMenu != null)
                {
                    var peer = new ButtonAutomationPeer(botonOpenMenu);
                    var invokeProvider = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                    invokeProvider?.Invoke();
                }
            }
            catch { }
        }


        private Button GetButtonOpenMenuSTB()
        {
            Button boton = null;

            foreach (Button btn in FindVisualChildren<Button>(this))
            {
                if (btn.Name == "OpenMenuSTB")
                {
                    boton = btn; break;
                }
            }
            return boton;
        }

        private void OpenMenu()
        {
            foreach (Button btn in FindVisualChildren<Button>(this))
            {
                if (btn != null)
                {
                    if (btn.Name == "CloseMenuSTB")
                    {
                        btn.Visibility = Visibility.Visible;
                    }
                    else if (btn.Name == "OpenMenuSTB")
                    {
                        btn.Visibility = Visibility.Collapsed;
                    }

                }
            }

        }

        private void GridMenu_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel vm = this.DataContext as MainWindowViewModel;

            if (vm != null && vm.EstaLogueado == true)
            {
                TriggerOpenMenu();
            }

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OnTeclaKeyDown(object sender, KeyEventArgs e)
        {
            var myVm = DataContext as MainWindowViewModel;

            if (myVm != null)
            {
                if (myVm.EstaLogueado)
                {
                    //if ((e.Key == Key.System && e.SystemKey == Key.F9) && Keyboard.Modifiers == ModifierKeys.Alt) // Alt + F9    ** Primero van los Alt y luego teclas funcionales
                    //{
                    //    Close();
                    //}

                    if (e.Key == Key.F1)
                    {
                        //myVm.LogoutCommand?.Execute(null);
                        myVm.UpdateCurrentViewModelCommand?.Execute(Modulos.Home);
                    }
                    else if (e.Key == Key.F2)
                    {
                        myVm.UpdateCurrentViewModelCommand?.Execute(Modulos.Sorteos);
                    }
                    else if (e.Key == Key.F3)
                    {
                        myVm.UpdateCurrentViewModelCommand?.Execute(Modulos.Reporte);
                    }
                    else if ((e.Key == Key.System && e.SystemKey == Key.F10))
                    {
                        myVm.UpdateCurrentViewModelCommand?.Execute(Modulos.Mensajeria);
                    }

                }//fin if EstaLogueado = true
                else
                {
                    if (e.Key == Key.F9) // Alt + F9
                    {
                        Close();
                    }

                    //if ((e.Key == Key.System && e.SystemKey == Key.F9) && Keyboard.Modifiers == ModifierKeys.Alt) // Alt + F9
                    //{
                    //    Close();
                    //}

                }//fin else EstaLogueado = false

            }//fin de if myVm

        }//fin de metodo OnTeclaKeyDown





    }//fin de clase MainWindow
}
