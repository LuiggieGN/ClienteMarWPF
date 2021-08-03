using ClienteMarWPFWin7.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Forms.Integration;
using Microsoft.VisualBasic.CompilerServices;

namespace ClienteMarWPFWin7.UI.Modules.CincoMinutos
{
    /// <summary>
    /// Lógica de interacción para Pizarra.xaml
    /// </summary>
    public partial class Pizarra : Window
    {
        public bool TooglePantalla = true;
        public Pizarra()
        {
            InitializeComponent();
        }
      
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (SessionGlobals.Productos != null)
            {
                if (SessionGlobals.Productos.Productos[0].ProductoConfig[4].ConfigKey == "URLPizarraCincoMinutos" && Screen.AllScreens.Length > 1 && SessionGlobals.Productos.Productos[0].ProductoConfig[4].ConfigValue.Length > 5)
                {
                    Screen DisplayPrimary = Screen.AllScreens.First(p => Operators.ConditionalCompareObjectEqual(p.Primary, true, false));
                    Screen DisplaySecondary = Screen.AllScreens.First(s => Operators.ConditionalCompareObjectEqual(s.Primary, false, false));
                    var currentAreaPrimary = DisplayPrimary.WorkingArea;
                    var currentAreaSecondary = DisplaySecondary.WorkingArea;
                    this.WindowStartupLocation = WindowStartupLocation.Manual;
                    this.Top = currentAreaPrimary.Top + (currentAreaPrimary.Height / 2 - this.Height / 2);
                    this.Left = currentAreaPrimary.Left + (currentAreaPrimary.Width / 2 - this.Width / 2);
                    this.WindowStartupLocation = WindowStartupLocation.Manual;
                    this.WindowState = WindowState.Normal;
                    this.Top = currentAreaSecondary.Top + (currentAreaSecondary.Height / 2 - this.Height / 2);
                    this.Left = currentAreaSecondary.Left + (currentAreaSecondary.Width / 2 - this.Width / 2);
                    this.WindowState = WindowState.Maximized;

                    browser.Source = new Uri(SessionGlobals.Productos.Productos[0].ProductoConfig[4].ConfigValue);
                }
            }


        }

        //public void CambiarMonitor(object sender, RoutedEventArgs e)
        //{
        //    Screen DisplayPrimary = Screen.AllScreens.First(p => Operators.ConditionalCompareObjectEqual(p.Primary, true, false));
        //    Screen DisplaySecondary = Screen.AllScreens.First(s => Operators.ConditionalCompareObjectEqual(s.Primary, false, false));
        //    var currentAreaPrimary = DisplayPrimary.WorkingArea;
        //    var currentAreaSecondary = DisplaySecondary.WorkingArea;
        //    if (TooglePantalla)
        //    {
        //        this.WindowStartupLocation = WindowStartupLocation.Manual;
        //        this.Top = currentAreaSecondary.Top + (currentAreaSecondary.Height / 2 - this.Height / 2);
        //        this.Left = currentAreaSecondary.Left + (currentAreaSecondary.Width / 2 - this.Width / 2);
        //        this.WindowStartupLocation = WindowStartupLocation.Manual;
        //        this.WindowState = WindowState.Normal;
        //        this.Top = currentAreaPrimary.Top + (currentAreaPrimary.Height / 2 - this.Height / 2);
        //        this.Left = currentAreaPrimary.Left + (currentAreaPrimary.Width / 2 - this.Width / 2);
        //        //this.Location = DisplayPrimary.Bounds.Location;
        //        this.WindowState = WindowState.Maximized;
        //    }
        //    else
        //    {
        //        this.WindowStartupLocation = WindowStartupLocation.Manual;
        //        this.Top = currentAreaPrimary.Top + (currentAreaPrimary.Height / 2 - this.Height / 2);
        //        this.Left = currentAreaPrimary.Left + (currentAreaPrimary.Width / 2 - this.Width / 2);
        //        this.WindowStartupLocation = WindowStartupLocation.Manual;
        //        this.WindowState = WindowState.Normal;
        //        this.Top = currentAreaSecondary.Top + (currentAreaSecondary.Height / 2 - this.Height / 2);
        //        this.Left = currentAreaSecondary.Left + (currentAreaSecondary.Width / 2 - this.Width / 2);
        //        this.WindowState = WindowState.Maximized;
        //    }
        //}
    }
}