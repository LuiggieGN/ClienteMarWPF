using System;
using System.Collections.Generic;
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
using Flujo.Entities.WpfClient.Enums;
using Flujo.Entities.WpfClient.POCO;
using Flujo.Entities.WpfClient.ResponseModels;
using FlujoCustomControl.Code.BussinessLogic;


namespace FlujoCustomControl.Views
{
    /// <summary>
    /// Lógica de interacción para MensajeroWindows.xaml
    /// </summary>
    public partial class MensajeroWindows : Window
    {
        public Window ParentWindows { get; set; }
        public event Action<int> SetMensajeroCajaID;
        private UsuarioResponseModel usuario { get; set; }
        private TokenDeSeguridadResponseModel usuario__token { get; set; }


        public MensajeroWindows(Window parent, Action<int>  accion)
        {
            this.ParentWindows = parent;
            this.SetMensajeroCajaID = accion;
            InitializeComponent();
            WrapPanelTokentoken.Visibility = Visibility.Hidden;
            SetMensajeroCajaID?.Invoke(-1);  // 0.7* a 2.7*

            Grid__Mensajero.RowDefinitions[1].Height = new GridLength(0.8, GridUnitType.Star);


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Left = ParentWindows.Left + (ParentWindows.Width - this.ActualWidth) / 2;
            this.Top = ParentWindows.Top + (ParentWindows.Height - this.ActualHeight) / 2;
        }


        private void BtnAplicar_Click(object sender, RoutedEventArgs e)
        {
            string strCedula = passBoxUserCedula == null ? null : passBoxUserCedula.Password;

            if (
                  strCedula !=  null &&   !( Regex.Replace(strCedula, @"\s+", "").Equals(string.Empty) )
               )
            {
                this.usuario = UsuarioLogic.BuscarUsuarioMensajeroDocumento( strCedula, UsuarioTiposDocumento.Cedula);

                if (this.usuario != null)
                {
                    this.usuario__token = UsuarioTokenSeguridadLogic.ConsultaUnSoloTokenDeSeguridadAleatorio(this.usuario.UsuarioID);

                    if (this.usuario__token == null)
                    {
                        this.usuario = null;
                        this.usuario__token = null;

                        TDigit.Text = "";
                        WrapPanelTokentoken.Visibility = Visibility.Hidden;
                        if (passBoxUserCedula != null)
                        {
                            passBoxUserCedula.Clear();
                        }

                        if (passBoxUserPin != null)
                        {
                            passBoxUserPin.Clear();
                        }

                        SetMensajeroCajaID?.Invoke(-1);
                        MessageBox.Show("Usuario sin tarjeta de seguridad", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                        return;
                    }
                    
                    passBoxUserPin.Clear();
                    TDigit.Text = this.usuario__token.Posicion+ "";
                    WrapPanelTokentoken.Visibility = Visibility.Visible;
                    Grid__Mensajero.RowDefinitions[1].Height = new GridLength(2.7, GridUnitType.Star);

                    passBoxUserPin.Focus();

                }
                else
                {

                    this.usuario = null;
                    this.usuario__token = null;

                    TDigit.Text = "";

                    WrapPanelTokentoken.Visibility = Visibility.Hidden;

                    if (passBoxUserCedula != null)
                    {
                        passBoxUserCedula.Clear();
                    }

                    if (passBoxUserPin != null)
                    {
                        passBoxUserPin.Clear();
                    }

                    SetMensajeroCajaID?.Invoke(-1);
                    MessageBox.Show("No, Documento inválido", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    //Grid__Mensajero.RowDefinitions[1].Height = new GridLength(0.7, GridUnitType.Auto);

                }
            }
            else
            {
                this.usuario = null;
                SetMensajeroCajaID?.Invoke(-1);
                //Grid__Mensajero.RowDefinitions[1].Height = new GridLength(0.7, GridUnitType.Auto);
                MessageBox.Show("Debe especificar el No. de Documento", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancelarSecurityToken_Click(object sender, RoutedEventArgs e)
        {
            this.usuario = null;
            this.usuario__token = null;

            TDigit.Text = "";

            WrapPanelTokentoken.Visibility = Visibility.Hidden;
            
            if (passBoxUserCedula != null)
            {
                passBoxUserCedula.Clear();
            }

            if (passBoxUserPin != null)
            {
                passBoxUserPin.Clear();
            }

            SetMensajeroCajaID?.Invoke(-1);

            lbError.Visibility = Visibility.Hidden;

            Grid__Mensajero.RowDefinitions[1].Height = new GridLength(0.8, GridUnitType.Star);
        }


        private void BtnIniciar_Click(object sender, RoutedEventArgs e)
        {
 
            if (this.usuario != null && this.usuario__token != null && passBoxUserPin != null && this.usuario__token.Toquen.Equals(passBoxUserPin.Password))
            {

                Caja c = CajaLogic.GetUsuarioCaja(this.usuario.UsuarioID);

                if (c !=  null)
                {
                    SetMensajeroCajaID?.Invoke(c.CajaID);
                    lbError.Visibility = Visibility.Hidden;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Error ! Mensajero no posee cuenta asignada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                SetMensajeroCajaID?.Invoke(-1);
                lbError.Visibility = Visibility.Visible;

            }

        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            SetMensajeroCajaID?.Invoke(-1);
            this.Close();
        }

 

        private void PassBoxUserPin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RoutedEventArgs newEvent = new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent);
                BtnIniciar.RaiseEvent(newEvent);
            }
        }

        private void PassBoxUserCedula_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RoutedEventArgs newEvent = new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent);
                btnAplicar.RaiseEvent(newEvent);
                passBoxUserPin.Focus();
            }
        }
    }
}
