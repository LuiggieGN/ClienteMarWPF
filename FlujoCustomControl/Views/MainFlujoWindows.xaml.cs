using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using FlujoCustomControl.ViewModels;
using FlujoCustomControl.Code.BussinessLogic;
using Flujo.Entities.WpfClient.RequestModel;
using FlujoCustomControl.Views.UsersControls;
using Flujo.Entities.WpfClient.POCO;
using static Flujo.Entities.WpfClient.PublicModels.ProductoResponseModel;
using System.Windows.Interop;

namespace FlujoCustomControl.Views
{
    public partial class MainFlujoWindows : Window
    {
        public static FlujoServices.MAR_Session MarSession = new FlujoServices.MAR_Session();        
        public static BingoServices.MAR_Session MarBingoSession = new BingoServices.MAR_Session();        
        public static List<ProductoViewModel> ProductosDisponiblesList = new List<ProductoViewModel>();      
        public static CincoMinutosRequestModel.DetalleJugadas DetalleJugadas = new CincoMinutosRequestModel.DetalleJugadas();

        internal static ProductoViewModel ProductoSelected { get; set; }
        public static FlujoServices.MAR_Session FillSession(int pBancaID, int pBancaSession, int pUsuarioId,  int pPrinterSize, string pPrintHeader, string pPrintFooter)
        {
            FlujoServices.MAR_Session MarSession = new FlujoServices.MAR_Session();         
            return MarSession;
        }
        public int BancaID { get; set; }
        public int UsuarioID { get; set; }
        public bool BancaPoseeCuadreInicial { get; set; }
        public string UsuarioNombre { get; set; }


        public MainFlujoWindows(int pUsuarioID, int pBancaID, int pBancaSession,  string pTab = "", int pPrinterSize = 32, string pPrintHeader = "", string pPrintFooter = "")
        {
            try
            {
                //para cargar los iconos
                _FontAwesome_MSBuildXamlFix();
                InitializeComponent();
                MarginSpace();
                this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight; 
                MarSession.Sesion = MarBingoSession.Sesion = pBancaSession;
                MarSession.Banca = MarBingoSession.Banca = pBancaID;
                MarSession.Usuario = MarBingoSession.Usuario = pUsuarioID;
                MarBingoSession.PrinterSize = pPrinterSize;
                MarBingoSession.PrinterHeader = pPrintHeader;
                MarBingoSession.PrinterFooter = pPrintFooter;


             //   GetProductosDisponibles();  ## Descomentar Al subir CAMBIOS

                this.UsuarioID = pUsuarioID;
                this.UsuarioNombre = "";
                this.BancaID = pBancaID;

                bool poseeCuadreInicial = CuadreLogic.EstaBancaPoseeCuadreInicial(pBancaID);
                this.BancaPoseeCuadreInicial = poseeCuadreInicial;

                if (poseeCuadreInicial)
                {
                    btnProcesarMovimiento.Visibility = Visibility.Visible;
                    btnConsultaDeMovimientos.Visibility = Visibility.Visible;
                    BotonCuadre.Visibility = Visibility.Visible;

                    if (pTab == "Flujo")
                    {
                        this.DataContext = new ProcesarMovimientoViewModel(this, pBancaID, pUsuarioID, (MainWindowsTitle) => lbMain_Title.Content = MainWindowsTitle, (MainWindowsBancaBalance) => lbMain_BancaBalanceActual.Content = MainWindowsBancaBalance, this.UsuarioNombre);

                    }
                    else if (pTab == "Ventas")
                    {
                        this.DataContext = new CincoMinutosViewModel();
                    }

                }
                else
                {
                    CajaLogic.Asigna__CajaABanca(pBancaID);
                    btnProcesarMovimiento.Visibility = Visibility.Collapsed;
                    btnConsultaDeMovimientos.Visibility = Visibility.Collapsed;
                    BotonCuadre.Visibility = Visibility.Collapsed;

                    decimal BANCA_BALANCE = CuadreLogic.GetBancaACuadrarMontoReal(pBancaID);

                    if (BANCA_BALANCE >= 0)
                    {
                        BotonCuadre.Visibility = Visibility.Visible;
                    }

                    if (pTab == "Flujo")
                    {
                        Action<bool> activaTagsFlujo = delegate (bool ShowSeccionesFlujo)
                        {
                            if (ShowSeccionesFlujo)
                            {
                                btnProcesarMovimiento.Visibility = Visibility.Visible;
                                btnConsultaDeMovimientos.Visibility = Visibility.Visible;
                                this.DataContext = new ProcesarMovimientoViewModel(this, pBancaID, pUsuarioID, (MainWindowsTitle) => lbMain_Title.Content = MainWindowsTitle, (MainWindowsBancaBalance) => lbMain_BancaBalanceActual.Content = MainWindowsBancaBalance, this.UsuarioNombre);
                            }
                        };

                        this.DataContext = new IniciaFlujoEfectivoUserControl(this.BancaID, activaTagsFlujo);
                    }
                    else if (pTab == "Ventas")
                    {
                        this.DataContext = new CincoMinutosViewModel();
                    }
                }

            }
            catch (Exception ex)
            {
               MessageBox.Show("BTN P" + ex.Message + ex.StackTrace);
            }


        }

        #region METODOS JAVIER
        //------------- METODOS JAVIER ----------------//
        private void _FontAwesome_MSBuildXamlFix()
        {
            /*
             * WORKAROUND
             * we need this method so that FontAwesome.WPF.dll gets copied as part of the build process
             * 
             * https://stackoverflow.com/a/18221455/1600
             */

            var type = typeof(FontAwesome.WPF.FontAwesome);
        }
        private void MarginSpace()
        {
            var helper = new WindowInteropHelper(this); //this being the wpf form
            var currentScreen = System.Windows.Forms.Screen.FromHandle(helper.Handle);
            if (currentScreen.Bounds.Width > 800)
            {
                double marginLeft = currentScreen.Bounds.Width / 15;
                double marginTop = marginLeft / 2;
                ControlContent.Margin = new Thickness(marginLeft, marginTop, 0, 50);
            }
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
            var state = this.WindowState;
            if (state == WindowState.Maximized)
            {
                this.BtnManimize.Visibility = Visibility.Visible;
                this.BtnMaximize.Visibility = Visibility.Collapsed;
                ControlContent.Margin = new Thickness(100);
            }
        }
        private void MinimizeButton(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Normal;
            this.BtnMaximize.Visibility = Visibility.Visible;
            this.BtnManimize.Visibility = Visibility.Collapsed;
            ControlContent.Margin = new Thickness(0);
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }
        private void MaximizeButton(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
            this.BtnManimize.Visibility = Visibility.Visible;
            this.BtnMaximize.Visibility = Visibility.Collapsed;
            MarginSpace();
        }
        private void CloseButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void BtnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void BtnDesgloseTab(object sender, RoutedEventArgs e)
        {
            var dw = new DesgloseWindowsTab();
            dw.Owner = this;
            dw.ShowDialog();
        }




        public void CloseWpfFlujoForm()
        {
            this.Close();
        }
        //------------- TERMINA METODOS JAVIER ----------------//
        #endregion




        private void GetProductosDisponibles()
        {
            var productosResponse = Code.BussinessLogic.CincoMinutosLogic.GetProductosDisponibles();
            if (productosResponse.OK)
            {
                ProductosDisponiblesList = productosResponse.Productos;
            }
        }
        private void btnProcesarMovimiento_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!(this.DataContext is ProcesarMovimientoViewModel))
                {
                    this.DataContext = new ProcesarMovimientoViewModel(this, BancaID, UsuarioID, (MainWindowsTitle) => lbMain_Title.Content = MainWindowsTitle, (MainWindowsBancaBalance) => lbMain_BancaBalanceActual.Content = MainWindowsBancaBalance, this.UsuarioNombre);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("BTN P" + ex.Message + ex.StackTrace);
            }

        }
        private void btnConsultaDeMovimientos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!(this.DataContext is ConsultaMovimientoViewModel))
                {
                    this.DataContext = new ConsultaMovimientoViewModel(BancaID, UsuarioID, (MainWindowsTitle) => lbMain_Title.Content = MainWindowsTitle, (MainWindowsBancaBalance) => lbMain_BancaBalanceActual.Content = MainWindowsBancaBalance);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("btnConsultaDeMovimientos_Click" + ex.Message + ex.StackTrace);
            }

        }
        private void btnAbrirVentasMenu_Click(object serder, RoutedEventArgs e)
        {
            try
            {
                if (!(this.DataContext is CincoMinutosViewModel))
                {
                    this.DataContext = new CincoMinutosViewModel();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("btnAbrirVentasMenu_Click" + ex.Message + ex.StackTrace);
            }
        }




 

        private void Button_RealizarCuadreClick(object sender, RoutedEventArgs e)
        {

            var gestorVentana = new GestorWindows(this, BancaID);
            gestorVentana.Owner = this;
            gestorVentana.ShowDialog();

            MUsuario usuario = gestorVentana.usuario;
            Caja usuarioCaja = gestorVentana.usuarioCaja;
            RutaAsignacion rutaAsignacion = gestorVentana.rutaAsignacion;
            RutaRecorrido rutaRecorrido = gestorVentana.rutaRecorrido;
            bool cuadreEsPermitido = gestorVentana.cuadreEsPermitido;

            if (cuadreEsPermitido)
            {
                Action<bool> seccionesFlujoTarget = delegate (bool ShowSeccionesFlujo)
                {
                    if (ShowSeccionesFlujo)
                    {
                        btnProcesarMovimiento.Visibility = Visibility.Visible;
                        btnConsultaDeMovimientos.Visibility = Visibility.Visible;
                        this.DataContext = new ProcesarMovimientoViewModel(this, this.BancaID, this.UsuarioID, (MainWindowsTitle) => lbMain_Title.Content = MainWindowsTitle, (MainWindowsBancaBalance) => lbMain_BancaBalanceActual.Content = MainWindowsBancaBalance, this.UsuarioNombre);
                    }
                };

                var cw = new CuadreWindows(this, this.BancaID, usuarioCaja.CajaID, usuario, rutaAsignacion, rutaRecorrido, (MainWindowsBancaBalance) => lbMain_BancaBalanceActual.Content = MainWindowsBancaBalance, (!this.BancaPoseeCuadreInicial), seccionesFlujoTarget, this.UsuarioNombre);
                cw.Owner = this;
                cw.ShowDialog();

            }

 
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                CajaLogic.ConfigurarCajaDisponibilidad(this.BancaID, true);
            }
            catch (Exception ex)
            {

            }

        }
    }
}
