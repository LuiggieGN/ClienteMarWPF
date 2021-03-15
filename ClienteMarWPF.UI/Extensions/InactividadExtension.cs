

using ClienteMarWPF.UI.ViewModels.Helpers;
using System.Windows.Input;
using System.Windows.Threading;
using System;
using System.Timers;
using System.Windows;




namespace ClienteMarWPF.UI.Extensions
{


    public static class InactividadExtension
    {
        public static bool? TimedShowDialog(this Window window, TimeSpan inactivityTimeLimit)
        {
            using (new InactivityTimer(window, inactivityTimeLimit))
            {
                return window.ShowDialog();
            }
        }
    }



    public class InactivityTimer : IDisposable
    {
        private Action ExpiredAction;

        private Timer Timer;

        private Point InactiveMousePosition;

        private bool IsDisposed, IsExpired;

        private Window Window;

        public InactivityTimer(Window window, TimeSpan inactivityTimeLimit) :
           this(
                () =>
                {
                    window.Close();
                    Application.Current.Shutdown();
                },
               inactivityTimeLimit)
        {
            Window = window;
            Window.Activated += OnWindowActivity;
            Window.LocationChanged += OnWindowActivity;
            Window.SizeChanged += OnWindowActivity;
        }

        public InactivityTimer(Action expiredAction, TimeSpan inactivityTimeLimit)
        {
            ExpiredAction = expiredAction;
            Timer = new Timer(inactivityTimeLimit.TotalMilliseconds) { AutoReset = false };
            Timer.Elapsed += Timer_Elapsed;
            InputManager.Current.PreProcessInput += OnInputActivity;
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
