
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.ViewModels.Helpers;
using ClienteMarWPF.UI.ViewModels;
using ClienteMarWPF.UI.Views.WindowsModals;

using System.Windows.Input;
using System.Windows.Threading;
using System;
using System.Timers;
using System.Windows;


namespace ClienteMarWPF.UI.Extensions
{

    public static class InactividadExtension
    {

        public static InactivityTimer Inactividad { get; set; } = null;


        public static void SetInactividad(Window ventana, TimeSpan tiempo)
        {
            Inactividad = new InactivityTimer();
            Inactividad.Start(ventana, tiempo);
        }


        public static void RemoveInactividad()
        {
            if (Inactividad != null)
            {
                try
                {
                    Inactividad.Dispose();
                }
                catch
                {
                }

                Inactividad = null;
            }

        }//fin de metodo RemoveInactividad 


    }



    public class InactivityTimer : IDisposable
    {
        private Action ExpiredAction;

        private Timer Timer;

        private Point InactiveMousePosition;

        private bool IsDisposed, IsExpired;

        private Window Window;

        public InactivityTimer() { }

        public void Start(Window ventana, TimeSpan tiempo)
        {
            ExpiredAction = () =>
            {
                var vm = ventana.DataContext as MainWindowViewModel;

                if (vm != null)
                {
                    Action cierraSesion = ()=> vm.LogoutCommand?.Execute(null);

                    var alertaContexto = new InactvidadWindowViewModel(cierraSesion, tiempo);
                    
                    var alerta = new InactividadWindow(alertaContexto);

                    alerta.Owner = Application.Current.MainWindow;

                    alertaContexto.ControlVentanaInactividad = alerta;

                    alerta.ShowDialog();
                }
            };

            Timer = new Timer(tiempo.TotalMilliseconds) { AutoReset = false };

            Timer.Elapsed += Timer_Elapsed;

            InputManager.Current.PreProcessInput += OnInputActivity;

            Window = ventana;
            Window.Activated += OnWindowActivity;
            Window.LocationChanged += OnWindowActivity;
            Window.SizeChanged += OnWindowActivity;
        }

        private void OnInputActivity(object sender, PreProcessInputEventArgs e)
        {
            if (IsActive(e.StagingItem.Input))
            {
                KeepAlive();
            }
        }

        private void OnWindowActivity(object sender, EventArgs e)
        {
            KeepAlive();
        }

        public void KeepAlive()
        {
            if (IsDisposed) throw new ObjectDisposedException(GetType().Name);
            if (IsExpired) return;
            Timer.Stop();
            Timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (IsExpired) return;
            IsExpired = true;
            WpfThreadHelper.Marshall(ExpiredAction, DispatcherPriority.Background);
        }

        public void Dispose()
        {
            if (IsDisposed) return;
            Timer.Dispose();
            ReleaseEventHandlers();
            IsDisposed = true;
        }

        private bool IsActive(InputEventArgs inputEventArgs)
        {
            if (inputEventArgs is KeyboardEventArgs) return true;
            var mouseEventArgs = inputEventArgs as MouseEventArgs;
            return mouseEventArgs != null && IsActiveMouse(mouseEventArgs);
        }

        private bool IsActiveMouse(MouseEventArgs mouseEventArgs)
        {
            bool buttonPressed = mouseEventArgs.LeftButton != MouseButtonState.Released
                              || mouseEventArgs.RightButton != MouseButtonState.Released
                              || mouseEventArgs.MiddleButton != MouseButtonState.Released
                              || mouseEventArgs.XButton1 != MouseButtonState.Released
                              || mouseEventArgs.XButton2 != MouseButtonState.Released;
            if (buttonPressed) return true;
            var mousePosition = mouseEventArgs.GetPosition(Application.Current.MainWindow);
            if (mousePosition == InactiveMousePosition) return false;
            InactiveMousePosition = mousePosition;
            return true;
        }

        private void ReleaseEventHandlers()
        {
            InputManager.Current.PreProcessInput -= OnInputActivity;
            if (Window == null) return;
            Window.Activated -= OnWindowActivity;
            Window.LocationChanged -= OnWindowActivity;
            Window.SizeChanged -= OnWindowActivity;
        }



    }//fin de clase 





}//fin de namespace
