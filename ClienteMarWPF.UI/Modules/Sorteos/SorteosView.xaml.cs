using ClienteMarWPF.UI.ViewModels.ModelObservable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
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
        public List<SorteosObservable> SorteosBinding;

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

            ltJugada.ItemsSource = listaJugadas;

            SorteosBinding = new List<SorteosObservable> {
                new SorteosObservable(){ LoteriaID=1, IsSelected=false, IsSuper = false, Loteria="La Fecha Dia", Image = "WbSunny" },
                new SorteosObservable(){ LoteriaID=2, IsSelected=false, IsSuper = false, Loteria="La Fecha Noche", Image = "Brightness3" },
                new SorteosObservable(){ LoteriaID=3, IsSelected=false, IsSuper = false, Loteria="Loteka Dia", Image = "WbSunny" },
                new SorteosObservable(){ LoteriaID=4, IsSelected=false, IsSuper = false, Loteria="Loteka Noche", Image = "Brightness3" },
                new SorteosObservable(){ LoteriaID=5, IsSelected=false, IsSuper = false, Loteria="Nacional Dia", Image = "WbSunny" },
                new SorteosObservable(){ LoteriaID=6, IsSelected=false, IsSuper = false, Loteria="Nacional Noche", Image = "Brightness3" },
                new SorteosObservable(){ LoteriaID=7, IsSelected=false, IsSuper = false, Loteria="New York Dia", Image = "WbSunny" },
                new SorteosObservable(){ LoteriaID=8, IsSelected=false, IsSuper = false, Loteria="New York Noche", Image = "Brightness3" },
                new SorteosObservable(){ LoteriaID=9, IsSelected=false, IsSuper = false, Loteria="Pega 3 Dia", Image = "WbSunny" },
                new SorteosObservable(){ LoteriaID=10, IsSelected=false, IsSuper = false, Loteria="Pega 3 Noche", Image = "Brightness3" }

            };

            listSorteo.DataContext = SorteosBinding;

        }

        // #region LOGICA PARA SORTEOS
        private void ValidateSelectOnlyTwo()
        {
            int count = 0;
            foreach (var item in SorteosBinding)
            {
                if (count <= 1 && item.IsSelected)
                {
                    count++;
                }
                else
                {
                    item.IsSelected = false;
                }
            }
        }


        // #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Window.GetWindow(this)).MensajesAlerta("Esto es una prueba de alertas", "Aviso");

        }

        private void PressTecla(object sender, KeyEventArgs e)
        {
            int Count = 0;
            int SorteoIndex = 0;

            switch (e.Key)
            {
                case Key.Subtract:
                    //DeleteItem();
                    break;   
                    
                case Key.Add:
                    //RealizaApuesta();
                    break;    
                    
                case Key.Multiply:
                    //OpenCombinacion();
                    break;    
                    
                case Key.F5:
                   // ShowConsultaTiket();
                    break;     
                    
                case Key.F11:
                    if (txtMonto.IsFocused || txtJugada.IsFocused)
                    { listSorteo.Focus(); } else { txtMonto.Focus(); }
                    break;

                case Key.Left:
                    int sorteo = listSorteo.SelectedIndex + 1;
                    if (sorteo % 2 == 1)
                    {
                        if (Count == 1 && SorteoIndex == sorteo)
                        {
                            txtMonto.Focus();
                            Count = 0;
                            SorteoIndex = 0;

                        }
                        else if (SorteoIndex != sorteo)
                        {
                            SorteoIndex = listSorteo.SelectedIndex + 1;
                            Count = 0;
                        }

                        Count++;
                    }
                    break;

                case Key.Right:
                    if (txtMonto.IsFocused || txtJugada.IsFocused)
                    {
                        listSorteo.Focus();
                        listSorteo.SelectedIndex = 0;
                        listSorteo.Items.Refresh();
                    }
                    else if (listSorteo.SelectedIndex == listSorteo.Items.Count - 1)
                    {
                        listSorteo.SelectedIndex = 0;
                    }
                    else
                    {
                        listSorteo.SelectedIndex += 1;
                    }
                    break;

                case Key.Up:
                    if (listSorteo.SelectedIndex != 0 && listSorteo.SelectedIndex != 1)
                    {
                        listSorteo.SelectedIndex = listSorteo.SelectedIndex - 2;
                    }
                    break;

                case Key.Space:
                    if (listSorteo.SelectedItem != null)
                    {
                        var item = listSorteo.SelectedItem as SorteosObservable;
                        //SorteosBinding.Where(x => x.LoteriaID == item.LoteriaID).Select(x => { x.IsSelected = !x.IsSelected; return x; }).FirstOrDefault();
                    }
                    break;

            }


        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void listSorteo_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            ValidateSelectOnlyTwo();
        }


        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            ValidateSelectOnlyTwo();
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
