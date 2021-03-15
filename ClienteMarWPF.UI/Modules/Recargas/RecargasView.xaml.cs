using ClienteMarWPF.UI.ViewModels.ModelObservable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

namespace ClienteMarWPF.UI.Modules.Recargas
{
    /// <summary>
    /// Interaction logic for RecargasView.xaml
    /// </summary>
    public partial class RecargasView : UserControl
    {
        public ObservableCollection<ProveedorRecargasObservable> Proveedors;
        public bool IsArrow { get; set; } = false;
        // public ProveedorRecargasObservable Provedor { get; set; }
        public ProveedorRecargasObservable ProveedorSelect { get; set; }

        public RecargasView()
        {
            InitializeComponent();
            //Proveedors = new ObservableCollection<ProveedorRecargasObservable> {
            //    new ProveedorRecargasObservable(){ OperadorID=1, IsSelected=false, Operador="Claro", Pais="DO", Url = "pack://application:,,,/ClienteMarWPF.UI;component/StartUp/Images/Operador/Claro_logo.png" },
            //    new ProveedorRecargasObservable(){ OperadorID=2, IsSelected=false, Operador="Altice", Pais="DO", Url = "pack://application:,,,/ClienteMarWPF.UI;component/StartUp/Images/Operador/AlticeB_logo.png" },
            //    new ProveedorRecargasObservable(){ OperadorID=3, IsSelected=false, Operador="Viva", Pais="DO", Url = "pack://application:,,,/ClienteMarWPF.UI;component/StartUp/Images/Operador/VivaT_logo.png" },
            //    new ProveedorRecargasObservable(){ OperadorID=5, IsSelected=false, Operador="Tricon", Pais="DO", Url = "pack://application:,,,/ClienteMarWPF.UI;component/StartUp/Images/Operador/Tricom_logo.jpeg" },
            //    new ProveedorRecargasObservable(){ OperadorID=5, IsSelected=false, Operador="Wind", Pais="DO", Url = "pack://application:,,,/ClienteMarWPF.UI;component/StartUp/Images/Operador/Wind_logo.png" },
            //    new ProveedorRecargasObservable(){ OperadorID=4, IsSelected=false, Operador="Digicel", Pais="HT", Url = "pack://application:,,,/ClienteMarWPF.UI;component/StartUp/Images/Operador/Digice_logo.jpg" },
            //    new ProveedorRecargasObservable(){ OperadorID=6, IsSelected=false, Operador="Natcom", Pais="HT", Url = "pack://application:,,,/ClienteMarWPF.UI;component/StartUp/Images/Operador/NatcomW_logo.jpg" }
            //};

            //listSorteo.DataContext = Proveedors;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

    

        private void listSorteo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void img_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            IsArrow = false;
            switch (e.Key)
            {

                case Key.Space:
                    IsArrow = true;
                    if (proveedores.SelectedItem != null)
                    {
                        var seleccionado = proveedores.SelectedItem as ProveedorRecargasObservable;
                        seleccionado.IsSelected = true;
                        var vm = DataContext as RecargasViewModel;
                        vm.SetProveedorFromkeyDown(seleccionado.OperadorID);

                    }
                    break;


                case Key.Enter:
                    IsArrow = true;
                    FocusInputs();
                    break;

                case Key.Up:
                    IsArrow = true;
                    if (telefono.IsFocused || monto.IsFocused)
                    {
                        proveedores.Focus();
                        proveedores.SelectedIndex = 0;
                        //var prueba = (List<ProveedorRecargasObservable>) proveedores.ItemsSource;
                        //proveedores.SelectedItem = prueba.First();
                       


                    }
                    break;


                case Key.Left:

                    IsArrow = true;
                    break;


                case Key.Right:
                    IsArrow = true;

                    break;


                case Key.Down:

                    IsArrow = true;
                  
                    break;





            }

        }

        public void FocusInputs()
        {
           
                //var seleccionado = proveedores.SelectedItem as ProveedorRecargasObservable;
                //seleccionado.IsSelected = true;
                if (telefono.Text.Length > 0 && monto.Text.Length > 0)
                {
                   

                }
                else
                {
                    if (telefono.IsFocused)
                    {

                        monto.Focus();
                    }
                    else
                    {
                        telefono.Focus();
                    }
                }
          

            

        }

        private void ListProveedores(object sender, SelectionChangedEventArgs e)
        {
             var prueba = proveedores.ItemsSource;
            var ok = proveedores.SelectedItem;
            //for (int i = 0; i < prueba.; i++)
            //{

            //}
        }

        private void proveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsArrow)
            {
                var seleccionado = proveedores.SelectedItem as ProveedorRecargasObservable;

                if (seleccionado != null)
                {
                    seleccionado.IsSelected = true;
                    var vm = DataContext as RecargasViewModel;
                    vm.SetProveedorFromkeyDown(seleccionado.OperadorID);
                }                
            }
            IsArrow = false;
        }




    }
}
