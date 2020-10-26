using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FlujoCustomControl.Views
{
    /// <summary>
    /// Lógica de interacción para CajeraDialog.xaml
    /// </summary>
    public partial class CajeraDialog : Window
    {
        public CajeraDialog( )
        {
            InitializeComponent();
            txtAnswer.Text = "";
        }


        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            txtAnswer.SelectAll();
            txtAnswer.Focus();
        }

        public string NombreCajera
        {
            get { return txtAnswer.Text; }
        }


    }
}
