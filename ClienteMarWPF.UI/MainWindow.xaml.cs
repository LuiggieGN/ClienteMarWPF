
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClienteMarWPF.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(object dataContext)
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
    }
}
