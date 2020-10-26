using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;

using Flujo.Entities.WpfClient.POCO;
using Flujo.Entities.WpfClient.RequestModel;
using Flujo.Entities.WpfClient.ResponseModels;

using FlujoCustomControl.Code.BussinessLogic;
using FlujoCustomControl.Helpers;

namespace FlujoCustomControl.Views
{
    /// <summary>
    /// Interaction logic for CuadreWindows.xaml
    /// </summary>
    public partial class CuadreWindows : Window
    {
        private Window ParentWindows;
        private Accion AccionSeleccion { get; set; }
        private decimal Balance { get; set; }
        private int BancaID { get; set; }
        private decimal Banca_BalanceMinimo { get; set; }
        private decimal Banca_MontoContado { get; set; }
        private decimal Banca_MontoPorPagar { get; set; }  //db_Monto_Totalizado_Tickets_Pendiente_De_Pago_Sin_Reclamar
        private int gestorCajaId { get; set; }
        private MUsuario gestorUsuario { get; set; }
        private List<ErrorMetadata> LaListaDeErrores { get; set; }
        private event Action<string> Set_Windows_MainLabelBancaBalanceActual;
        private event Action<bool> ActivaSeccionesFlujo;
        private bool EsteCuadreEsInical;
        private static readonly Regex regexDigitosValidos = new Regex(@"^[\d]|\.");
        private static readonly Regex regexEsEnteroODecimal = new Regex(@"^[0-9]\d*(\.\d{1,2})?$");

        public RutaAsignacion rutaAsignacion { get; set; }
        public RutaRecorrido rutaRecorrido { get; set; }

        private static bool ElTestoEsPermitido(string texto)
        {
            bool machea = regexDigitosValidos.IsMatch(texto);

            return machea;
        }
        private void TxtMontoRetirado_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!ElTestoEsPermitido(e.Text))
            {
                e.Handled = true;
            }
        }
        private void TxtMontoRetirado_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        public string UsuarioNombre { get; set; }

        private decimal db_Recomendado_A_Dejar { get; set; }
        private decimal db_Recomendado_A_Retirar { get; set; }


        public CuadreWindows(Window pParentWindow, int pBancaID, int pGestorCajaID, MUsuario pGestorUsuario, RutaAsignacion pRutaAsignacion, RutaRecorrido pRutaRecorrido, Action<string> accionMainBancaBalance, bool pEsUnCuadreInical, Action<bool> accionActivaSeccionFlujo, string pUsuarioNombre = "")
        {
            this.ParentWindows = pParentWindow;
            this.BancaID = pBancaID;
            this.gestorCajaId = pGestorCajaID;
            this.gestorUsuario = pGestorUsuario;
            this.rutaAsignacion = pRutaAsignacion;
            this.rutaRecorrido = pRutaRecorrido;


            this.LaListaDeErrores = new List<ErrorMetadata>();
            this.Set_Windows_MainLabelBancaBalanceActual = accionMainBancaBalance;
            this.ActivaSeccionesFlujo = accionActivaSeccionFlujo;
            this.EsteCuadreEsInical = pEsUnCuadreInical;
            this.UsuarioNombre = pUsuarioNombre;

            InitializeComponent();

            WrapRecomendacion.Visibility = Visibility.Collapsed;
            lbFaltante.Visibility = Visibility.Collapsed;

            CajaLogic.ConfigurarCajaDisponibilidad(pBancaID, false);  //@@Disponibilidad

            this.Banca_BalanceMinimo = CajaLogic.GetBancaBalanceMinimo(pBancaID);

            lbMontoTotalizadoPendienteDePago.Content = "Monto por pagar: ...cargando ";

            //Balance Real en Caja
            decimal BalanceSegunSistema = CuadreLogic.GetBancaACuadrarMontoReal(pBancaID);
            this.Balance = BalanceSegunSistema;


            //Seteando el monto contado en caja por default
            this.Banca_MontoContado = 0;
            txtMontoContado.Text = "0";



            //Se setea el Balance y Faltante de la Banca
            if (this.Balance <= 0)
            {
                /**Banca en perdida*/
                lbBalance.Content = $"Balance: {this.Balance.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US")) }";
                lbFaltante.Content = "Monto Faltante: " + (0).ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));

            }
            else
            {
                /**Banca con balance disponible**/
                lbBalance.Content = $"Balance: {this.Balance.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US")) }";
                lbFaltante.Content = "Monto Faltante: " + (this.Balance).ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
            }

            lbBalanceMinimo.Content = "Balance Minimo: " + this.Banca_BalanceMinimo.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));


            this.Loaded += new RoutedEventHandler((s, r) => {

                this.WindowStartupLocation = WindowStartupLocation.CenterOwner;


                //Seteando Gestor
                btnProcesarCuadre.IsEnabled = false;
                lbMensajero.Content = "  Gestor: " + this.gestorUsuario.UsuNombre + " " + this.gestorUsuario.UsuApellido;

                // Seteando Banca Monto Por Pagar
                this.Banca_MontoPorPagar = CuadreLogic.GetMontoTotalizadoTiketsPendientesDePagoSinReclamar(this.BancaID);
                lbMontoTotalizadoPendienteDePago.Content = "Monto por pagar: " + this.Banca_MontoPorPagar.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));


                // Seteando (lo recomendado a retirar)  \\ (lo recomendado a dejar)  
                this.db_Recomendado_A_Dejar = (this.Banca_MontoContado - this.Banca_BalanceMinimo) <= 0

                                                      ?
                                                          (Math.Abs((this.Banca_MontoContado - this.Banca_BalanceMinimo)) + this.Banca_MontoPorPagar)

                                                      : (

                                                           (this.Banca_MontoPorPagar == 0)
                                                                  ?
                                                                    0
                                                                  :

                                                                  (
                                                                      ((this.Banca_MontoContado - this.Banca_BalanceMinimo) >= this.Banca_MontoPorPagar)

                                                                      ?
                                                                         0
                                                                      :

                                                                      /*(this.Banca_MontoContado - this.Banca_BalanceMinimo) + */(this.Banca_MontoPorPagar - (this.Banca_MontoContado - this.Banca_BalanceMinimo))
                                                                  )


                                                         )
                                                      ;

                this.db_Recomendado_A_Retirar = this.Balance - this.Banca_BalanceMinimo - this.Banca_MontoPorPagar;


                if (this.db_Recomendado_A_Retirar <= 0) // < 1
                {
                    //Aqui se selecciona como default |Depositar|

                    this.AccionSeleccion = new Accion();
                    this.AccionSeleccion.OpcionSeleccionada = EnumAccionCuadre.Depositar;
                    this.DataContext = this.AccionSeleccion;

                    lbRecomendadoARetirar.Visibility = Visibility.Collapsed;
                    lbRecomendadoADejar.Visibility = Visibility.Visible;
                    lbRecomendadoADejar.Content = "Recomendado a depositar:  " + this.db_Recomendado_A_Dejar.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));

                }
                else
                {
                    //Aqui se selecciona como default |Retirar|

                    this.AccionSeleccion = new Accion();
                    this.AccionSeleccion.OpcionSeleccionada = EnumAccionCuadre.Retirar;
                    this.DataContext = this.AccionSeleccion;

                    lbRecomendadoADejar.Visibility = Visibility.Collapsed;
                    lbRecomendadoARetirar.Visibility = Visibility.Visible;
                    lbRecomendadoARetirar.Content = "Recomendado a retirar:  " + this.db_Recomendado_A_Retirar.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));

                }


                //Activando el Boton <Procesar Cuadre>
                btnProcesarCuadre.IsEnabled = true;


            });



        }// Fin CuadreWindows() ~




        private void AbrirDesglose(object sender, RoutedEventArgs e)
        {
            var dw = new DesgloseWindows();

            dw.SetMontoContado += delegate (string MontoContadoMensajero)
            {

                //Seteando Monto Contado
                txtMontoContado.Text = MontoContadoMensajero; decimal montoContadoPorMensajero = Convert.ToDecimal(MontoContadoMensajero);

                this.Banca_MontoContado = montoContadoPorMensajero;

                this.db_Recomendado_A_Retirar = montoContadoPorMensajero - this.Banca_BalanceMinimo - this.Banca_MontoPorPagar;

                this.db_Recomendado_A_Dejar = (montoContadoPorMensajero - this.Banca_BalanceMinimo) <= 0

                                                     ?
                                                         (Math.Abs((montoContadoPorMensajero - this.Banca_BalanceMinimo)) + this.Banca_MontoPorPagar)

                                                     : (

                                                          (this.Banca_MontoPorPagar == 0)
                                                                 ?
                                                                   0
                                                                 :

                                                                 (
                                                                     ((montoContadoPorMensajero - this.Banca_BalanceMinimo) >= this.Banca_MontoPorPagar)

                                                                     ?
                                                                        0
                                                                     :

                                                                   /*  (montoContadoPorMensajero - this.Banca_BalanceMinimo) + */(this.Banca_MontoPorPagar - (montoContadoPorMensajero - this.Banca_BalanceMinimo))
                                                                 )


                                                        )
                                                     ;



                if (this.db_Recomendado_A_Retirar <= 0)  // < 1
                {
                    //Aqui se selecciona como default |Depositar|

                    this.AccionSeleccion = new Accion();
                    this.AccionSeleccion.OpcionSeleccionada = EnumAccionCuadre.Depositar;
                    this.DataContext = this.AccionSeleccion;

                    lbRecomendadoARetirar.Visibility = Visibility.Collapsed;
                    lbRecomendadoADejar.Visibility = Visibility.Visible;
                    lbRecomendadoADejar.Content = "Recomendado a depositar:  " + this.db_Recomendado_A_Dejar.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));

                }
                else
                {
                    //Aqui se selecciona como default |Retirar|

                    this.AccionSeleccion = new Accion();
                    this.AccionSeleccion.OpcionSeleccionada = EnumAccionCuadre.Retirar;
                    this.DataContext = this.AccionSeleccion;

                    lbRecomendadoADejar.Visibility = Visibility.Collapsed;
                    lbRecomendadoARetirar.Visibility = Visibility.Visible;
                    lbRecomendadoARetirar.Content = "Recomendado a retirar:  " + this.db_Recomendado_A_Retirar.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));

                }

                this.WrapRecomendacion.Visibility = Visibility.Visible;
                this.lbFaltante.Visibility = Visibility.Visible;

            };


            dw.SetMontoFaltante += delegate (int MontoContadoMensajero) {

                if (this.Balance <= 0)
                {
                    /**Banca en perdida*/
                    if (MontoContadoMensajero == 0)
                    {
                        lbFaltante.Content = "Monto Faltante: " + (0).ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
                    }
                    else if (MontoContadoMensajero > 0)
                    {
                        lbFaltante.Content = "Monto a favor: " + (MontoContadoMensajero).ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
                    }
                }
                else
                {
                    /**Banca con balance disponible**/
                    if ((this.Balance - MontoContadoMensajero) < 0)
                    {
                        lbFaltante.Content = "Monto a favor: " + (-1 * (this.Balance - MontoContadoMensajero)).ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
                    }
                    else if ((this.Balance - MontoContadoMensajero) >= 0)
                    {
                        lbFaltante.Content = "Monto Faltante: " + (this.Balance - MontoContadoMensajero).ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
                    }
                }
            };

            dw.ShowDialog();

        }

        private void ProcesarCuadre(object sender, RoutedEventArgs e)
        {
            try
            {

                CuadreModel cm = new CuadreModel();

                cm.Tipo = "MANUAL";
                cm.BalanceMinimo = this.Banca_BalanceMinimo;
                cm.CajaID = CajaLogic.GetBancaCajaID(this.BancaID);


                if (cm.CajaID == -1 || this.gestorUsuario == null)
                {
                    MessageBox.Show("Un error ha ocurrido intentar mas tarde", "Error", MessageBoxButton.OK, MessageBoxImage.Error); return;
                }



                if (Regex.Replace(txtCajera.Text, @"\s+", "").Equals(string.Empty))
                {
                    MessageBox.Show("Especifique el nombre de la cajera", "Error", MessageBoxButton.OK, MessageBoxImage.Error); return;
                }


                cm.UsuarioCaja = txtCajera.Text;  //this.UsuarioNombre;  


                string strMontoRetirado = txtMontoRetirado.Text;

                #region Validacion de Monto a Retirar o Depositar

                ctListaDeErrores.ItemsSource = null;
                this.LaListaDeErrores.Clear();

                if (Regex.Replace(strMontoRetirado, @"\s+", "").Equals(""))
                {
                    this.LaListaDeErrores.Add(new ErrorMetadata() { MensajeError = AccionSeleccion.OpcionSeleccionada == EnumAccionCuadre.Depositar ? "* Especificar monto a depositar." : "* Especificar monto a retirar." });
                    ctListaDeErrores.ItemsSource = this.LaListaDeErrores;
                    return;
                }

                if (!regexEsEnteroODecimal.IsMatch(strMontoRetirado))
                {
                    this.LaListaDeErrores.Add(new ErrorMetadata() { MensajeError = AccionSeleccion.OpcionSeleccionada == EnumAccionCuadre.Depositar ? "* Monto a depositar  invalido." : "* Monto a retirar invalido." });
                    ctListaDeErrores.ItemsSource = this.LaListaDeErrores;
                    return;
                }

                #endregion


                cm.CajaOrigenID = this.gestorCajaId;
                cm.UsuarioOrigenID = this.gestorUsuario.UsuarioID;
                cm.AuxMensajeroNombre = this.gestorUsuario.UsuNombre + " " + this.gestorUsuario.UsuApellido;

                cm.MontoContado = Convert.ToDecimal(txtMontoContado.Text, CultureInfo.InvariantCulture);
                cm.MontoRetirado = Convert.ToDecimal(strMontoRetirado, CultureInfo.InvariantCulture);


                if (AccionSeleccion.OpcionSeleccionada == EnumAccionCuadre.Depositar)
                {
                    cm.MontoDepositado = cm.MontoRetirado;
                    cm.MontoRetirado = cm.MontoRetirado * -1;

                    Caja cajaUsuExt = CajaLogic.GetUsuarioCaja(cm.UsuarioOrigenID);
                    if ((cajaUsuExt.BalanceActual - (cm.MontoDepositado ?? 0)) < 0)
                    {
                        ctListaDeErrores.ItemsSource = null;

                        this.LaListaDeErrores.Clear();
                        this.LaListaDeErrores.Add(new ErrorMetadata() { MensajeError = $"  * Gestor ext. autorizado fondos insuficientes." });
                        this.LaListaDeErrores.Add(new ErrorMetadata() { MensajeError = $"     - Gestor ext. Balanca en caja: {cajaUsuExt.BalanceEnFormato} ." });
                        this.LaListaDeErrores.Add(new ErrorMetadata() { MensajeError = $"     - Monto requerido a Depositar: $ {(cm.MontoDepositado == null ? "0.00" : cm.MontoDepositado.Value.ToString("#,##0.00", CultureInfo.CreateSpecificCulture("en-US")))} ." });

                        ctListaDeErrores.ItemsSource = this.LaListaDeErrores;
                        return;
                    }
                }
                else
                {

                    if (!CuadreValidacionHelper.SePermiteRetirarMonto(cm.MontoContado, cm.MontoRetirado == null ? 0 : cm.MontoRetirado.Value))
                    {
                        ctListaDeErrores.ItemsSource = null;

                        this.LaListaDeErrores.Clear();
                        this.LaListaDeErrores.Add(new ErrorMetadata() { MensajeError = $"  * La Banca no cuenta con los fondos a retirar." });
                        this.LaListaDeErrores.Add(new ErrorMetadata() { MensajeError = $"     - Monto en caja: {cm.MontoContado.ToString("$ #,##0.00", CultureInfo.CreateSpecificCulture("en-US"))} ." });
                        this.LaListaDeErrores.Add(new ErrorMetadata() { MensajeError = $"     - Cantidad a retirar: {(cm.MontoRetirado == null ? "$ 0.00" : cm.MontoRetirado.Value.ToString("$ #,##0.00", CultureInfo.CreateSpecificCulture("en-US")))} ." });

                        ctListaDeErrores.ItemsSource = this.LaListaDeErrores;

                        return;
                    }

                }

                cm.MontoPorPagar = this.Banca_MontoPorPagar;


                string str__PrintCuadre;  // @@@@ String generado a imprimir en el recibo como un cuadre@@@@

                CuadreResponse response = CuadreLogic.Procesar(this.BancaID, cm, AccionSeleccion, out str__PrintCuadre, this.rutaAsignacion, this.rutaRecorrido  ); //@@@@ Procesa el cuadro y setea el recibo a imprimri

                if (response.FueProcesado)
                {

                    // Seteando el nombre de la cajera al valor default

                    txtCajera.Text = "";

                    //Seteando Balance del Windows Principal
                    string strBancaBalance = CajaLogic.GetBancaBalanceActual(this.BancaID);
                    Set_Windows_MainLabelBancaBalanceActual?.Invoke("|Balance : " + strBancaBalance); // Seteando el balance de la Banca desde la pantalla principal


                    //Balance Real en Caja
                    decimal BalanceSegunSistema = CuadreLogic.GetBancaACuadrarMontoReal(this.BancaID);
                    this.Balance = BalanceSegunSistema;


                    //Seteando Balance y Faltante
                    txtMontoContado.Text = "0";
                    txtMontoRetirado.Text = "";
                    this.Banca_MontoContado = 0;



                    //Se setea el Balance y Faltante de la Banca
                    if (this.Balance <= 0)
                    {
                        /**Banca en perdida*/

                        lbBalance.Content = $"Balance: {this.Balance.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US")) }";
                        lbFaltante.Content = "Monto Faltante: " + (0).ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));

                    }
                    else
                    {
                        /**Banca con balance disponible**/

                        lbBalance.Content = $"Balance: {this.Balance.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US")) }";
                        lbFaltante.Content = "Monto Faltante: " + (this.Balance).ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
                    }


                    // Seteando Banca Monto Por Pagar
                    this.Banca_MontoPorPagar = CuadreLogic.GetMontoTotalizadoTiketsPendientesDePagoSinReclamar(this.BancaID);
                    lbMontoTotalizadoPendienteDePago.Content = "Monto por pagar: " + this.Banca_MontoPorPagar.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));



                    // Seteando (lo recomendado a retirar)  \\ (lo recomendado a dejar)  
                    this.db_Recomendado_A_Dejar = (this.Banca_MontoContado - this.Banca_BalanceMinimo) <= 0

                                                          ?
                                                              (Math.Abs((this.Banca_MontoContado - this.Banca_BalanceMinimo)) + this.Banca_MontoPorPagar)

                                                          : (

                                                               (this.Banca_MontoPorPagar == 0)
                                                                      ?
                                                                        0
                                                                      :

                                                                      (
                                                                          ((this.Banca_MontoContado - this.Banca_BalanceMinimo) >= this.Banca_MontoPorPagar)

                                                                          ?
                                                                             0
                                                                          :

                                                                         /* (this.Banca_MontoContado - this.Banca_BalanceMinimo) +*/ (this.Banca_MontoPorPagar - (this.Banca_MontoContado - this.Banca_BalanceMinimo))
                                                                      )


                                                             )
                                                          ;

                    this.db_Recomendado_A_Retirar = this.Balance - this.Banca_BalanceMinimo - this.Banca_MontoPorPagar;


                    if (this.db_Recomendado_A_Retirar <= 0) // < 1
                    {
                        //Aqui se selecciona como default |Depositar|

                        this.AccionSeleccion = new Accion();
                        this.AccionSeleccion.OpcionSeleccionada = EnumAccionCuadre.Depositar;
                        this.DataContext = this.AccionSeleccion;

                        lbRecomendadoARetirar.Visibility = Visibility.Collapsed;
                        lbRecomendadoADejar.Visibility = Visibility.Visible;
                        lbRecomendadoADejar.Content = "Recomendado a depositar:  " + this.db_Recomendado_A_Dejar.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));

                    }
                    else
                    {
                        //Aqui se selecciona como default |Retirar|

                        this.AccionSeleccion = new Accion();
                        this.AccionSeleccion.OpcionSeleccionada = EnumAccionCuadre.Retirar;
                        this.DataContext = this.AccionSeleccion;

                        lbRecomendadoADejar.Visibility = Visibility.Collapsed;
                        lbRecomendadoARetirar.Visibility = Visibility.Visible;
                        lbRecomendadoARetirar.Content = "Recomendado a retirar:  " + this.db_Recomendado_A_Retirar.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));

                    }

                    PrintOutHelper.SendToPrinter(str__PrintCuadre);
                    //MessageBox.Show("El cuadre fue procesado", "Alerta", MessageBoxButton.OK, MessageBoxImage.Information);



                    //Activando las secciones de flujo si es un cuadre inicial y si el cuadre fue procesado
                    ActivaSeccionesFlujo?.Invoke(this.EsteCuadreEsInical); // Seteando el balance de la Banca desde la pantalla principal

                    this.WrapRecomendacion.Visibility = Visibility.Collapsed;
                    this.lbFaltante.Visibility = Visibility.Collapsed;

                }
                else
                {
                    MessageBox.Show(response.Error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ". " + ex.StackTrace, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CerrarVentana(object sender, RoutedEventArgs e)
        {
            CajaLogic.ConfigurarCajaDisponibilidad(this.BancaID, true);  //@@Disponibilidad
            this.Close();
        }





    }



    public class Accion
    {
        public EnumAccionCuadre OpcionSeleccionada { get; set; }
    }




}


namespace FlujoCustomControl
{
    public enum EnumAccionCuadre
    {
        Depositar,
        Retirar
    }
}