using ClienteMarWPFWin7.UI.ViewModels.ModelObservable;
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

namespace ClienteMarWPFWin7.UI.Modules.Recargas
{
    /// <summary>
    /// Interaction logic for RecargasView.xaml
    /// </summary>
    public partial class RecargasView : UserControl
    {
        public ObservableCollection<ProveedorRecargasObservable> Proveedors;
        int contador = 0;
        public static readonly DependencyProperty RecargaCommandProperty = DependencyProperty.Register("SendRecargaCommand", typeof(ICommand), typeof(RecargasView), new PropertyMetadata(null));

        public ICommand SendRecargaCommand
        {
            get
            {
                return (ICommand)GetValue(RecargaCommandProperty);

            }
            set { SetValue(RecargaCommandProperty, value); }
        }



        public bool IsArrow { get; set; } = false;
        // public ProveedorRecargasObservable Provedor { get; set; }
        public ProveedorRecargasObservable ProveedorSelect { get; set; }

        public RecargasView()
        {
            InitializeComponent();
            IsArrow = true;
  

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
                    if (monto.IsFocused)
                    {
                        telefono.Focus();
                    }

                    if (proveedores.IsFocused)
                    {
                        proveedores.SelectedIndex -= 1;

                        if(proveedores.SelectedIndex == -1)
                        {
                            proveedores.SelectedIndex = 0;
                        }
                    }
                    break;


                case Key.Right:
                    IsArrow = true;
                    if (telefono.IsFocused)
                    {
                        FocusInputs();

                    }

                    if (proveedores.IsFocused)
                    {
                        proveedores.SelectedIndex += 1;
                    }

                    break;


                case Key.Down:

                    IsArrow = true;
                    telefono.Focus();
                  
                    break;





            }

        }

        public void FocusInputs()
        {
           
                //var seleccionado = proveedores.SelectedItem as ProveedorRecargasObservable;
                //seleccionado.IsSelected = true;
                if (telefono.Text.Trim().Length > 0 && monto.Text.Length > 0)
                {
                    SendRecargaCommand.Execute("");

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
            if(contador  == 0)
            {
                proveedores.SelectedIndex = 0;
                proveedores.Focus();
                contador++;
            }

          

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

        private void botonIniciar_Click(object sender, RoutedEventArgs e)
        {
            if(SendRecargaCommand != null)
            {
                SendRecargaCommand.Execute(null);
            }
        }

     
    }
}
