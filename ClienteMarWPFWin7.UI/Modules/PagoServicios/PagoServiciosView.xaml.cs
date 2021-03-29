using ClienteMarWPFWin7.UI.ViewModels.ModelObservable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ClienteMarWPFWin7.UI.Modules.PagoServicios
{
    /// <summary>
    /// Interaction logic for PagoServiciosView.xaml
    /// </summary>
    public partial class PagoServiciosView : UserControl
    {
        public PagoServiciosView()
        {
            InitializeComponent();

            var lista = new ObservableCollection<FacturaObservable> {
                new FacturaObservable(){ FacturaID=1, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new FacturaObservable(){ FacturaID=2, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new FacturaObservable(){ FacturaID=3, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new FacturaObservable(){ FacturaID=4, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new FacturaObservable(){ FacturaID=5, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new FacturaObservable(){ FacturaID=6, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new FacturaObservable(){ FacturaID=7, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new FacturaObservable(){ FacturaID=8, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new FacturaObservable(){ FacturaID=9, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new FacturaObservable(){ FacturaID=10, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new FacturaObservable(){ FacturaID=11, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new FacturaObservable(){ FacturaID=12, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new FacturaObservable(){ FacturaID=13, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new FacturaObservable(){ FacturaID=14, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new FacturaObservable(){ FacturaID=15, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new FacturaObservable(){ FacturaID=16, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new FacturaObservable(){ FacturaID=17, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new FacturaObservable(){ FacturaID=18, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new FacturaObservable(){ FacturaID=19, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new FacturaObservable(){ FacturaID=20, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 }
            };

            var lista2 = new List<Factura> {
                new Factura(){ FacturaID=1, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new Factura(){ FacturaID=2, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new Factura(){ FacturaID=3, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new Factura(){ FacturaID=4, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new Factura(){ FacturaID=5, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new Factura(){ FacturaID=6, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new Factura(){ FacturaID=7, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new Factura(){ FacturaID=8, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new Factura(){ FacturaID=9, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new Factura(){ FacturaID=10, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new Factura(){ FacturaID=11, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new Factura(){ FacturaID=12, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new Factura(){ FacturaID=13, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new Factura(){ FacturaID=14, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new Factura(){ FacturaID=15, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new Factura(){ FacturaID=16, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new Factura(){ FacturaID=17, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new Factura(){ FacturaID=18, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new Factura(){ FacturaID=19, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 },
                new Factura(){ FacturaID=20, Seleccion=false, FechaFactura="2018-03-01",FechaVence="2018-03-01", Total = 5500 }
            };

            tbFactura.DataContext = lista;
        }

        private void tbFactura_Loaded(object sender, RoutedEventArgs e)
        {
            tbFactura.Columns[0].Visibility = Visibility.Collapsed;
        }

        private void tbFactura_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName != "Seleccion")
            {
                e.Column.IsReadOnly = true;
            }
            else
            {
                e.Column.IsReadOnly = false;
            }
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            if (cbServicio.SelectedItem != null && txtCedula.Text != string.Empty)
            {
                ComboBoxItem typeItem = (ComboBoxItem)cbServicio.SelectedItem;
                string value = typeItem.Content.ToString();
                var user = new { Cedula = txtCedula.Text, Servicio = value };

              //  modalSearch.Visibility = Visibility.Hidden;
            }
            else
            {
                ((MainWindow)Window.GetWindow(Parent)).MensajesAlerta("Hay campos vacios", "Informacion", "#007ACC");
            }
        }

        private void btnCancelModal_Click(object sender, RoutedEventArgs e)
        {
            modal.Visibility = Visibility.Hidden;
        }

        private void btnOpenModal_Click(object sender, RoutedEventArgs e)
        {
            modal.Visibility = Visibility.Visible;
        }

        private void btnOpenModalSerchClick(object sender, RoutedEventArgs e)
        {
            txtCedula.Text = string.Empty;
            cbServicio.SelectedItem = cbServicio.Items[0];
           // modalSearch.Visibility = Visibility.Visible;
        }
    }

    public class Factura
    {
        public int FacturaID { get; set; }
        public string FechaFactura { get; set; }
        public string FechaVence { get; set; }
        public double Total { get; set; }
        public bool Seleccion { get; set; }
    }

}
