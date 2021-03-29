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
using System.Windows.Shapes;

namespace ClienteMarWPFWin7.UI.Modules.Sorteos.Modal
{
    /// <summary>
    /// Lógica de interacción para ConfirmarMontoWindow.xaml
    /// </summary>
    public partial class ConfirmarMontoWindow : Window
    {

        public bool Confirmar { get; set; } = false;
        public string Mensaje { get; set; } = "";
        public string Texto { get; set; }


        public ConfirmarMontoWindow()
        {
            InitializeComponent();
            botonCancelar.Focus();
          
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Confirmar = false;
            this.Close();
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            Confirmar = true;
            this.Close();
        }

       
        public void Mostrar(string texto)
        {
           Mensaje = $"Esta seguro que desea agregar esta quiniela de RD$ { texto }?";
        }

        private void PressTecla(object sender, KeyEventArgs e)
        {
            switch( e.Key )
            {
                case Key.Right:
                    botonConfirmar.Focus();
                    break;

                case Key.Left:
                    botonCancelar.Focus();
                    break;
            }
        }

    }
}
