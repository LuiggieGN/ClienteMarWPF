   using System;
using System.Linq;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Input;
using Flujo.Entities.WpfClient.POCO;
using FlujoCustomControl.Code.BussinessLogic;
using Flujo.Entities.WpfClient.ResponseModels;

namespace FlujoCustomControl.Views
{
    public partial class GestorWindows : Window
    {
        public Window parentWindows;
        public int IdBancaQueSeVaHaCuadrar { get; set; }

        public MUsuario usuario { get; set; }
        public Caja usuarioCaja { get; set; }
        public RutaAsignacion rutaAsignacion { get; set; }
        public RutaRecorrido rutaRecorrido { get; set; }
        public bool cuadreEsPermitido { get; set; }


        private TokenDeSeguridadResponseModel tokenAEvaluarMetodo1;

        public GestorWindows(Window parent, int bancaId)
        {
            this.parentWindows = parent;
            this.IdBancaQueSeVaHaCuadrar = bancaId;
            this.cuadreEsPermitido = false;
            InitializeComponent();
            hideError();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Window mainWindow = this.parentWindows;
            this.Left = mainWindow.Left + (mainWindow.Width - this.ActualWidth) / 2;
            this.Top = mainWindow.Top + (mainWindow.Height - this.ActualHeight) / 2;
            inputPin.Focus();
        }
        private void inputPin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                botonVerificarPin.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));  // Ejecuta el evento click del botonVerificarPin
            }
        }
        private void botonVerificarPin_Click(object sender, RoutedEventArgs e) // Entidades llenadas |usuario|usuarioCaja|rutaAsignacion?|rutaRecorrido?
        {
            try
            {
                hideError();
                botonVerificarPin.IsEnabled = false;

                this.usuario = UsuarioLogic.GetUsuarioByPin(inputPin.Password);

                if (this.usuario == null)
                {
                    showError("Pin inválido");
                    return;
                }
                if (!this.usuario.UsuActivo)
                {
                    showError("Usuario deshabilitado.");
                    return;
                }

                if (!this.usuario.UsuPuedeCuadrar.HasValue || this.usuario.UsuPuedeCuadrar == 0)// ** Usuario no tiene permiso de cuadrar
                {
                    showError("No posee permiso para cuadrar");
                    return;
                }

                this.usuarioCaja = CajaLogic.GetUsuarioCaja(this.usuario.UsuarioID);

                if (this.usuarioCaja == null)
                {
                    showError("No posee caja asignada");
                    return;
                }


                if (this.usuario.UsuPuedeCuadrar == 1) // ** Usuario puede cuadrar solo con asignacion
                {
                    this.rutaAsignacion = UsuarioLogic.GetGestorAsignacionPendiente(this.usuario.UsuarioID, this.IdBancaQueSeVaHaCuadrar);

                    if (this.rutaAsignacion == null)
                    {
                        showError("No posee asignaciones pendientes.");
                        return;
                    }

                    this.rutaRecorrido = JsonConvert.DeserializeObject<RutaRecorrido>(rutaAsignacion.OrdenRecorrido);

                    TerminalRecord terminal = this.rutaRecorrido.Terminales.First(banca => banca.BancaID == this.IdBancaQueSeVaHaCuadrar);

                    if (terminal.FueRecorrida == true)
                    {
                        showError("No posee asignaciones pendientes. La Terminal ya fue cuadrada.");
                        return;
                    }
                }
                 

                if (this.usuario.UsuPuedeCuadrar == 2)
                {
                    this.rutaAsignacion = null;
                    this.rutaRecorrido = null;
                }



                // if ( this.usuario.UsuPuedeCuadrar == 2 )        //** Usuario puede cuadrar sin restrincciones

                challengeMethod();
            }
            catch (Exception ex)
            {
                showError(ex.Message.Take(50).ToString() + "..");
            }
            finally 
            {
               botonVerificarPin.IsEnabled = true;

            }

        }





        private void challengeMethod()
        {

            if (this.usuario.TipoAutenticacion == 2) //** Metodo NO REQUIERE autenticacion
            {
                this.cuadreEsPermitido = true;
                this.Close();
            }    
            else if(this.usuario.TipoAutenticacion == 1) //** Metodo REQUIERE Token de Autenticacion
            {
                this.cuadreEsPermitido = false ;

                UsuarioTarjetaClave tarjeta = UsuarioLogic.GetUsuarioTarjeta(this.usuario.UsuarioID);

                if (tarjeta == null)
                {
                    showError("No posee tarjeta asignada");
                    return;
                }

                this.tokenAEvaluarMetodo1 = UsuarioLogic.GetTarjetaTokenAleatorio(tarjeta);
                metodo1();
            }

        }

        private void showError(string error)
        {
            reset();
            panelError.Visibility = Visibility.Visible;
            etiquetaError.Content = error;
            inputPin.Clear();
            inputPin.Focus();
        }

        private void hideError()
        {
            reset();
            etiquetaError.Content = "";
            panelError.Visibility = Visibility.Hidden;
        }


        private void reset()
        {
            this.cuadreEsPermitido = false;
            this.usuario = null;
            this.usuarioCaja = null;
            this.rutaAsignacion = null;
            this.rutaRecorrido = null;
            this.tokenAEvaluarMetodo1 = null;

            metodo1_etiquetaPrincipal.Visibility = Visibility.Collapsed;
            metodo1_etiquetaPosicion.Visibility = Visibility.Collapsed;
            metodo1_etiquetaPosicion.Content = "";
            metodo1_inputToken.Visibility = Visibility.Collapsed;
            metodo1_inputToken.Clear();

            metodo1_botonAplicar.Visibility = Visibility.Collapsed;
            metodo1_botonAplicar.IsEnabled = false;
        }


        private void metodo1() 
        {
            metodo1_etiquetaPrincipal.Visibility = Visibility.Visible;
            metodo1_etiquetaPosicion.Content = this.tokenAEvaluarMetodo1.Posicion;
            metodo1_etiquetaPosicion.Visibility = Visibility.Visible;

            metodo1_inputToken.Clear();
            metodo1_inputToken.Visibility = Visibility.Visible;

            metodo1_botonAplicar.IsEnabled = true;
            metodo1_botonAplicar.Visibility = Visibility.Visible;

            metodo1_inputToken.Focus();
        }

        private void metodo1_botonAplicar_Click(object sender, RoutedEventArgs e)
        {
            if (this.tokenAEvaluarMetodo1 != null)
            {
                string token = metodo1_inputToken.Password;

                if (token.Equals(this.tokenAEvaluarMetodo1.Toquen))
                {
                    this.cuadreEsPermitido = true;
                    this.Close();
                }
                else
                {
                    this.cuadreEsPermitido = false;
                    panelError.Visibility = Visibility.Visible;
                    etiquetaError.Content = "Token inválido";

                    metodo1_inputToken.Clear();
                    metodo1_inputToken.Focus();
                }
            }
        }

        private void metodo1_inputToken_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                metodo1_botonAplicar.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));  // Ejecuta el evento click del botonVerificarPin
            }
        }
    }
}
