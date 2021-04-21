using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.UI.ViewModels.Commands.Reporte;
using ClienteMarWPFWin7.UI.ViewModels.ModelObservable; 
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
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

namespace ClienteMarWPFWin7.UI.Modules.Reporte
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
        public static string OpcionTicketSeleccionado;
        private readonly ReporteViewModel ViewModel;
        public ObservableCollection<ReporteListaTicketsObservable> listaTicket=new ObservableCollection<ReporteListaTicketsObservable>() { };
        public ObservableCollection<ReporteListaTicketsObservable> listaTicketVolatil;
        
        public static readonly DependencyProperty GetReportesCommandProperty = DependencyProperty.Register("GetReportesCommand", typeof(ICommand), typeof(ReporteView), new PropertyMetadata(null));
        public static readonly DependencyProperty PrintReportsProperty = DependencyProperty.Register("PrintReportsCommand", typeof(ICommand), typeof(ReporteView), new PropertyMetadata(null));


        public ICommand GetReportesCommand
        {
            get { return (ICommand)GetValue(GetReportesCommandProperty); }
            set { SetValue(GetReportesCommandProperty, value); }
        }

        public ICommand PrintReportsCommand
        {
            get { return (ICommand)GetValue(PrintReportsProperty); }
            set { SetValue(PrintReportsProperty, value); }
        }
        public ReporteView()
        {
            InitializeComponent();
            var VM = DataContext as ReporteViewModel;
            
            listSorteo.SelectedIndex = 0;
            EnableScrollBars();
            NoAutogenerarColumnas();
        }

        private void EnableScrollBars()
        {
            ScrollViewer.SetVerticalScrollBarVisibility(this.infoReport, ScrollBarVisibility.Visible);
            ScrollViewer.SetVerticalScrollBarVisibility(this.GridVentaFechas, ScrollBarVisibility.Visible);
            ScrollViewer.SetVerticalScrollBarVisibility(this.GridListTicket, ScrollBarVisibility.Visible);
            ScrollViewer.SetVerticalScrollBarVisibility(this.GridPagosRemotos,ScrollBarVisibility.Visible);
            ScrollViewer.SetVerticalScrollBarVisibility(this.GridTicketPagados,ScrollBarVisibility.Visible);
            ScrollViewer.SetVerticalScrollBarVisibility(this.GridTicketPendientesPagoss, ScrollBarVisibility.Visible);
            ScrollViewer.SetVerticalScrollBarVisibility(this.GridTicketSinReclamar, ScrollBarVisibility.Visible);
            ScrollViewer.SetVerticalScrollBarVisibility(this.TempleateListaTarjetas, ScrollBarVisibility.Visible);
            ScrollViewer.SetVerticalScrollBarVisibility(this.GridListTicketNulosVentas, ScrollBarVisibility.Visible);
        }


        private void PressTecla(object sender, KeyEventArgs e)
        {
            var textFecha = VisualTreeHelper.GetChild(Fecha, 0);
            var reportes = ListReportes.Children;
            var posicionReporte = 0;
            object objectoReporte = new object();
            var VM = DataContext as ReporteViewModel;

            if (VM != null)
            {
                GetReportesCommand = VM.ObtenerReportes;
                PrintReportsCommand = VM.PrintReportes;
            }

            var AlgunReporteFocus = false;
            for (var i = 0; i < ListReportes.Children.Count; i++)
            {
                if (ListReportes.Children[i].IsFocused)
                {
                    AlgunReporteFocus = true;
                }
            }
            foreach (var reporte in reportes)
            {
                var rep = reporte as Control;
                objectoReporte = rep;
                if (rep.IsFocused==true) {
                    posicionReporte = reportes.IndexOf(rep);
                    posicionReporte = posicionReporte + 1;
                }
            }
            switch (e.Key)
            {
                case Key.Enter:
                    
                    if (listSorteo.IsFocused)
                    {
                        listSorteo.IsDropDownOpen = true;
                    }
                    if (Fecha.IsKeyboardFocusWithin==true)
                    {
                        Fecha.IsDropDownOpen=true;
                    }
                    if (posicionReporte != 0)
                    {
                        reporteid_Click(objectoReporte, e);
                        if (GetReportesCommand != null)
                        {
                            GetReportesCommand.Execute(null);
                        }
                    }
                    if (AlgunReporteFocus==true)
                    {
                        GetReportesCommand.Execute(null);
                    }
                    break;

                case Key.Down:
                    if (Fecha.IsDropDownOpen == false) { 
                        if (listSorteo.IsFocused == false && ListReportes.IsFocused == false)
                        {
                            if (AlgunReporteFocus==false) {
                                listSorteo.Focus();
                            }
                        }
                    }
                    break;

                case Key.Up:
                    if (Fecha.IsDropDownOpen == false)
                    {

                        if (listSorteo.IsFocused && AlgunReporteFocus == false)
                        {
                            contenedorFecha.Focus();
                        }
                    }
                     
                    break;

                case Key.Right:
                    if (Fecha.IsDropDownOpen == false)
                    {

                        if (listSorteo.IsFocused == false && ListReportes.IsFocused == false && AlgunReporteFocus == false)
                        {
                            ListReportes.Focus();
                            ListReportes.Children[0].Focus();
                        }
                        if (listSorteo.IsFocused)
                        {
                            btnRPTVentas.Focus();
                        }
                    }
                    break;
                case Key.Left:
                    if (Fecha.IsDropDownOpen == false)
                    {
                        if (btnRPTVentas.IsFocused)
                        {
                            contenedorFecha.Focus();
                        }
                    }
                    break;
                case Key.F6:
                        PrintReportsCommand.Execute(null);
                    break;
            }
        }

        private void reporteid_Click(object sender, RoutedEventArgs e)
        {
            
            var fase1 = (sender as Button);
            object elementos = fase1.Content;
            StackPanel elemto = elementos as StackPanel;
            UIElementCollection CollecionElementos = elemto.Children;
            TextBlock elementosTextBlock = CollecionElementos[1] as TextBlock;
            Nombre = elementosTextBlock.Text.ToString();

            
            if (Nombre == null)
            {
                Nombre = "Reportes de Ventas";
                //VentaFecha.IsOpen = false;
            }
            //else if (Nombre == "Ventas por Fecha")
            //{
                
            //    VentaFecha.IsOpen = true;
            //}
            //else
            //{
            //    VentaFecha.IsOpen = false;
            //}
            
        }

        public void NoAutogenerarColumnas()
        {
            // --------------Para que no se autogeneren las columnas ------------------------------//
            GridVentaFechas.AutoGenerateColumns = false;
           
            // -----------------------------------------------------------------------------------//

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

            MAR_Loteria2 objeto = ((sender as ComboBox).SelectedItem as MAR_Loteria2);
            if (objeto != null) {
                Loteria = objeto.LoteriaKey;
                NombreLoteria = objeto.Nombre;
            } 
            
        }

        public int GetLoteriaID()
        {
            return Loteria;
        }

        //public void OcultarModal(object sender, RoutedEventArgs e)
        //{
        //    //VentaFecha.IsOpen = false;
        //}

       
        //private void OcultandoDesdeVista(object sender, RoutedEventArgs e)
        //{
        //    VentaFecha.IsOpen = false;
        //}

        public void EliminandoTemplateGanadores(bool GridPagados,bool GridPendientePago,bool GridSinReclamar)
        {
            if (GridPagados == true)
            {
                GridGanadores.RowDefinitions.Remove(GridTicketPagados);
                GridGanadores.RowDefinitions.Remove(EncabezadoPagados);
            }

            if (GridPendientePago == true)
            {
                GridGanadores.RowDefinitions.Remove(GridTicketPendientesPagoss);
                GridGanadores.RowDefinitions.Remove(EncabezadoPendientePagos);
            }
            
            if (GridSinReclamar == true)
            {
                GridGanadores.RowDefinitions.Remove(GridTicketSinReclamar);
                GridGanadores.RowDefinitions.Remove(EncabezadoSinReclamar);
            }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            OpcionTicketSeleccionado = (sender as RadioButton).Content as String;
           
        }

        public string ObtenerSeleccionTicket()
        {
            return OpcionTicketSeleccionado;
        }

        private void VistaCompleta_Loaded(object sender, RoutedEventArgs e)
        {
            Fecha.Focus();
           
        }
    }
}
