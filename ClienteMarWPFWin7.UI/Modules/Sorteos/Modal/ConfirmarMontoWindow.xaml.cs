using ClienteMarWPFWin7.UI.ViewModels.Base;
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
            botonCancelar.BorderThickness = new Thickness(2, 2, 2, 2);
            botonConfirmar.BorderThickness = new Thickness(2, 2, 2, 2);
            botonConfirmar.BorderBrush = Brushes.Black;
            botonCancelar.BorderBrush = Brushes.LightGreen;
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
           var notificar = new BaseViewModel();
           Mensaje = $"Esta seguro que desea agregar esta quiniela de RD$ { texto }?";
            //notificar.NotifyPropertyChanged(nameof(Mensaje));
        }

       
        private void PressTecla(object sender, KeyEventArgs e)
        {
            if (botonCancelar.IsFocused)
            {
                botonConfirmar.BorderBrush = Brushes.LightGreen;
                botonCancelar.BorderBrush = Brushes.Black;
                botonCancelar.BorderThickness = new Thickness(3, 3, 3, 3);

            }
            else if (botonConfirmar.IsFocused)
            {
                botonCancelar.BorderBrush = Brushes.LightGreen;
                botonConfirmar.BorderBrush = Brushes.Black;
                botonConfirmar.BorderThickness = new Thickness(3, 3, 3, 3);

            }

            switch ( e.Key )
            {
                case Key.Left:
                    botonConfirmar.Focus();
                    break;

                case Key.Right:
                    botonCancelar.Focus();
                    break;
            }
        }

    }
}
