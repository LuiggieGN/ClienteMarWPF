using System;
using System.Windows;
using System.Windows.Controls;
using Flujo.Entities.WpfClient.POCO;
using Flujo.Entities.WpfClient.RequestModel;
using Flujo.Entities.WpfClient.ResponseModels;
using FlujoCustomControl.Code.BussinessLogic;
 

namespace FlujoCustomControl.Views.UsersControls
{
    /// <summary>
    /// Lógica de interacción para IniciaFlujoEfectivoUserControl.xaml
    /// </summary>
    public partial class IniciaFlujoEfectivoUserControl : UserControl
    {

        public IniciaFlujoEfectivoUserControl()
        {
        }

        public IniciaFlujoEfectivoUserControl(int pBancaID, Action<bool> activaTagsFlujo)
        {
            InitializeComponent();
            btnProcesar.IsEnabled = false;
            btnProcesar.Visibility = Visibility.Hidden;
            modalInfo.Visibility = Visibility.Collapsed;
            this.BancaID = pBancaID;
            this.ActivaTagsFlujo = activaTagsFlujo;
        }

        private void Evento_WindowsCargado(object sender, RoutedEventArgs e)
        {
            btnProcesar.IsEnabled = false;
            try
            {
                this.BalanceActualEnBanca = CuadreLogic.GetBancaACuadrarMontoReal(this.BancaID);
                if (this.BalanceActualEnBanca >= 0)
                {
                    btnProcesar.IsEnabled = true;
                    btnProcesar.Visibility = Visibility.Visible;
                }
                else
                {
                    btnProcesar.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                btnProcesar.Visibility = Visibility.Hidden;
                MessageBox.Show(ex.Message + ". " + ex.StackTrace, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Procesar(object sender, RoutedEventArgs e)
        {
            txtBlock.Text = $"Usted tiene un balance de ${this.BalanceActualEnBanca.ToString("N0")} en base al resultado de las Ventas del hoy menos los Pagos a ganadores del dia de hoy.";
            modalInfo.Visibility = Visibility.Visible;
        }


        private void Cancelar(object sender, RoutedEventArgs e)
        {
            modalInfo.Visibility = Visibility.Collapsed;
        }

        private void Aceptar(object sender, RoutedEventArgs e)
        {
            try
            {
                MUsuario superUsuario = UsuarioLogic.GetFirstSurperUsuario();
                Caja usuarioCaja = CajaLogic.GetUsuarioCaja(superUsuario.UsuarioID);

                CuadreModel defaultCuadre = new CuadreModel();
                defaultCuadre.Tipo = "INICIO";
                defaultCuadre.BalanceMinimo = CajaLogic.GetBancaBalanceMinimo(this.BancaID);
                defaultCuadre.CajaID = CajaLogic.GetBancaCajaID(this.BancaID);
                defaultCuadre.UsuarioCaja = "";
                defaultCuadre.CajaOrigenID = usuarioCaja.CajaID;
                defaultCuadre.UsuarioOrigenID = superUsuario.UsuarioID;
                defaultCuadre.AuxMensajeroNombre = superUsuario.UsuNombre+" "+ superUsuario.UsuApellido;
                defaultCuadre.MontoContado = this.BalanceActualEnBanca;
                defaultCuadre.MontoDepositado = 0;
                defaultCuadre.MontoRetirado = 0;
                defaultCuadre.MontoPorPagar = 0;

                string str__PrintCuadre;
                Accion accion = new Accion();
                accion.OpcionSeleccionada = EnumAccionCuadre.Depositar;
                CuadreResponse response = CuadreLogic.Procesar(this.BancaID, defaultCuadre, accion, out str__PrintCuadre); //@@@@ Procesa el cuadro y setea el recibo a imprimir

                if (response.FueProcesado)
                {                   
                    ActivaTagsFlujo?.Invoke(true);  //@@ Activa los tabs de flujo y la seccion de registrar movmientos
                }
                else
                {
                    MessageBox.Show(response.Error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " ." + ex.StackTrace, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public int BancaID { get; set; }
        public decimal BalanceActualEnBanca { get; set; }
        private event Action<bool> ActivaTagsFlujo;
    }



}