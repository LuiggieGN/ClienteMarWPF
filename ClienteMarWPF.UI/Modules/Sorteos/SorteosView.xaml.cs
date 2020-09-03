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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClienteMarWPF.UI.Modules.Sorteos
{
    /// <summary>
    /// Interaction logic for SorteosView.xaml
    /// </summary>
    public partial class SorteosView : UserControl
    {
        public SorteosView()
        {
            InitializeComponent();

            var listaJugadas = new List<Jugadas>
            {
                new Jugadas { TipoJugada = "Quiniela", Jugada="01", Monto= 300},
                new Jugadas { TipoJugada = "Quiniela", Jugada="25", Monto= 200},
                new Jugadas { TipoJugada = "Quiniela", Jugada="25", Monto= 200},
                new Jugadas { TipoJugada = "Quiniela", Jugada="25", Monto= 200},
                new Jugadas { TipoJugada = "Quiniela", Jugada="25", Monto= 200},
                new Jugadas { TipoJugada = "Quiniela", Jugada="25", Monto= 200},
                new Jugadas { TipoJugada = "Quiniela", Jugada="25", Monto= 200},
                new Jugadas { TipoJugada = "Quiniela", Jugada="25", Monto= 200},
                new Jugadas { TipoJugada = "Quiniela", Jugada="25", Monto= 200},
                new Jugadas { TipoJugada = "Quiniela", Jugada="25", Monto= 200},
                new Jugadas { TipoJugada = "Quiniela", Jugada="25", Monto= 200},
                new Jugadas { TipoJugada = "Quiniela", Jugada="25", Monto= 200},
                new Jugadas { TipoJugada = "Quiniela", Jugada="25", Monto= 200},
                new Jugadas { TipoJugada = "Quiniela", Jugada="25", Monto= 200},
                new Jugadas { TipoJugada = "Pale", Jugada="01-25", Monto= 500},
                new Jugadas { TipoJugada = "Pale", Jugada="25-01", Monto= 65},
                new Jugadas { TipoJugada = "Tripleta", Jugada="25-01-25", Monto= 500}
            };

            TableJugadas.ItemsSource = listaJugadas;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Window.GetWindow(this)).MensajesAlerta("Esto es una prueba de alertas", "Aviso");

        }
    }

    // ESTA CLASE SOLO ES DE EJEMPLO MOVER DONDE VA
    public class Jugadas
    {
        public string TipoJugada { get; set; }
        public string Jugada { get; set; }
        public int Monto { get; set; }
    }
}
