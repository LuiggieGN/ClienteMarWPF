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
using System.Windows.Shapes;

namespace ClienteMarWPFWin7.UI.Modules.Mensajeria
{
    /// <summary>
    /// Lógica de interacción para NotificacionMensajeWindow.xaml
    /// </summary>
    public partial class NotificacionMensajeWindow : Window
    {
        public NotificacionMensajeWindow(string destino, string mensaje)
        {
            InitializeComponent();
            Texto.Text = $"Mensaje nuevo \n \n Enviado por: {destino}";
            Texto2.Text = $"Mensaje: {mensaje}";
        }

        private void botonConfirmar_Click(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel.TimerConsultaMensaje.Start();
            this.Close();
        }
    }
}
