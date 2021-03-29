using System;
using System.Windows;
using System.Windows.Threading;

namespace ClienteMarWPFWin7.UI.ViewModels.Helpers
{
    public static class WpfThreadHelper
    {
        public static void Marshall(Action uiAction)
        { Marshall(uiAction, DispatcherPriority.DataBind); }

        public static void Marshall(Action uiAction, DispatcherPriority priority)
        {
            if (Application.Current == null) return;
            if (Application.Current.Dispatcher.CheckAccess())
            {
                uiAction();
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(uiAction, priority);
            }
        }

        public static void MarshallWait(Action uiAction)
        { 
            MarshallWait(uiAction, DispatcherPriority.DataBind); 
        }

        public static void MarshallWait(Action uiAction, DispatcherPriority priority)
        {
            if (Application.Current == null) return;
            if (Application.Current.Dispatcher.CheckAccess())
            {
                uiAction();
            }
            else
            {
                Application.Current.Dispatcher.Invoke(uiAction, priority);
            }
        }






    }//fin de clase WpfThreadHelper
}
