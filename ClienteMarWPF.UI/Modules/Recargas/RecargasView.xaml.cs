using ClienteMarWPF.UI.ViewModels.ModelObservable;
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

namespace ClienteMarWPF.UI.Modules.Recargas
{
    /// <summary>
    /// Interaction logic for RecargasView.xaml
    /// </summary>
    public partial class RecargasView : UserControl
    {
        public ObservableCollection<ProveedorRecargasObservable> Proveedors;


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
            
        
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {

        }
    }
}
