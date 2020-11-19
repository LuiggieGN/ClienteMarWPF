using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.UI.ViewModels.ModelObservable;
using MAR.AppLogic.MARHelpers;
using MarPuntoVentaServiceReference;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes; 

namespace ClienteMarWPF.UI.Modules.Reporte
{
    /// <summary>
    /// Interaction logic for ReporteView.xaml
    /// </summary>
    public partial class ReporteView : UserControl
    {
        private List<SorteosObservable> SorteosBinding;
        public static string Nombre;
        public static int Loteria;
        public static string NombreLoteria;
        
        public ReporteView()
        {
            InitializeComponent();

            
            SorteosBinding = new List<SorteosObservable> {
                new SorteosObservable(){ LoteriaID=0, IsSelected=true,  Loteria="Todas" },
                new SorteosObservable(){ LoteriaID=1, IsSelected=false,  Loteria="La Fecha Dia",},
                new SorteosObservable(){ LoteriaID=2, IsSelected=false,  Loteria="La Fecha Noche"},
                new SorteosObservable(){ LoteriaID=3, IsSelected=false,  Loteria="Loteka Dia" },
                new SorteosObservable(){ LoteriaID=4, IsSelected=false,  Loteria="Loteka Noche"},
                new SorteosObservable(){ LoteriaID=5, IsSelected=false,  Loteria="Nacional Dia" },
                new SorteosObservable(){ LoteriaID=6, IsSelected=false,  Loteria="Nacional Noche"},
                new SorteosObservable(){ LoteriaID=7, IsSelected=false,  Loteria="New York Dia"},
                new SorteosObservable(){ LoteriaID=8, IsSelected=false,  Loteria="New York Noche" },
                new SorteosObservable(){ LoteriaID=9, IsSelected=false,  Loteria="Pega 3 Dia" },
                new SorteosObservable(){ LoteriaID=10, IsSelected=false,  Loteria="Pega 3 Noche"}
               
            };

            listSorteo.DataContext = SorteosBinding;
            listSorteo.SelectedIndex = 0;
            
            EnableScrollBars();
           
        }

        private void EnableScrollBars()
        {
            ScrollViewer.SetVerticalScrollBarVisibility(this.infoReport, ScrollBarVisibility.Visible);
        }

       
        private void reporteid_Click(object sender, RoutedEventArgs e)
        {
            Nombre = (sender as RadioButton).Content as string;
            if (Nombre == null)
            {
                Nombre = "Reportes de Ventas";
                VentaFecha.IsOpen = false;
            }
            else if (Nombre == "Ventas por Fecha")
            {
                
                VentaFecha.IsOpen = true;
            }
            else
            {
                VentaFecha.IsOpen = false;
            }
            
        }

        public string GetReporteNombre()
        {
            return Nombre;
        }

        public string GetNombreLoteria()
        {
            return NombreLoteria;
            
        }

        private void listSorteo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SorteosObservable objeto =((sender as ComboBox).SelectedItem as SorteosObservable);
            Loteria = objeto.LoteriaID;
            NombreLoteria = objeto.Loteria.ToString();
            
        }

        public int GetLoteriaID()
        {
            return Loteria;
        }

    }
}
