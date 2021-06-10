using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
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
using System.Threading.Tasks;
using System.Windows.Threading;
using ClienteMarWPFWin7.UI.ViewModels.Helpers;

namespace ClienteMarWPFWin7.UI.Modules.Sorteos.Modal
{
    /// <summary>
    /// Interaction logic for ValidarPagoTicket.xaml
    /// </summary>
    public partial class ValidarPagoTicket : UserControl
    {
        private bool _padreFueHabilitado = true;
        private static string ticketSeleccionado;



        public bool CargarDialogo
        {
            get
            {
                return (bool)GetValue(CargarDialogoProperty);
            }
            set
            {
                SetValue(CargarDialogoProperty, value);
            }
        }

        //public void OrdenarListTickte()
        //{
        //    tbVentas.Columns.
        //}

        public UIElement OverlayOn
        {
            get
            {
                return (UIElement)GetValue(OverlayOnProperty);
            }
            set
            {
                SetValue(OverlayOnProperty, value);
            }
        }

        public static readonly DependencyProperty CargarDialogoProperty = DependencyProperty.Register("CargarDialogo", typeof(bool), typeof(ValidarPagoTicket), new UIPropertyMetadata(false, CargarDialogoChanged));
        public static readonly DependencyProperty OverlayOnProperty = DependencyProperty.Register("OverlayOn", typeof(UIElement), typeof(ValidarPagoTicket), new UIPropertyMetadata(null));
        public static readonly DependencyProperty GetListadoTicketsCommandProperty = DependencyProperty.Register("GetListadoTicketsCommand", typeof(ICommand), typeof(ValidarPagoTicket), new PropertyMetadata(null));
        public static readonly DependencyProperty CopiarTicketCommandProperty = DependencyProperty.Register("CopiarTicketCommand", typeof(ICommand), typeof(ValidarPagoTicket), new PropertyMetadata(null));
        public static readonly DependencyProperty CerrarValidarPagoTicketCommandProperty = DependencyProperty.Register("CerrarValidarPagoTicketCommand", typeof(ICommand), typeof(ValidarPagoTicket), new PropertyMetadata(null));

        public static readonly DependencyProperty ConsultarTicketCommandProperty = DependencyProperty.Register("ConsultarTicketCommand", typeof(ICommand), typeof(ValidarPagoTicket), new PropertyMetadata(null));
        public static readonly DependencyProperty ReimprimirTicketCommandProperty = DependencyProperty.Register("ReimprimirTicketCommand", typeof(ICommand), typeof(ValidarPagoTicket), new PropertyMetadata(null));
        public static readonly DependencyProperty AnularTicketCommandProperty = DependencyProperty.Register("AnularTicketCommand", typeof(ICommand), typeof(ValidarPagoTicket), new PropertyMetadata(null));


        public ICommand GetListadoTicketsCommand
        {
            get { return (ICommand)GetValue(GetListadoTicketsCommandProperty); }
            set { SetValue(GetListadoTicketsCommandProperty, value); }
        }

        public ICommand CopiarTicketCommand
        {
            get { return (ICommand)GetValue(CopiarTicketCommandProperty); }
            set { SetValue(CopiarTicketCommandProperty, value); }
        }

        public ICommand ConsultarTicketCommand
        {
            get { return (ICommand)GetValue(ConsultarTicketCommandProperty); }
            set { SetValue(ConsultarTicketCommandProperty, value); }
        }

        public ICommand ReimprimirTicketCommand
        {
            get { return (ICommand)GetValue(ReimprimirTicketCommandProperty); }
            set { SetValue(ReimprimirTicketCommandProperty, value); }
        }

        public ICommand AnularTicketCommand
        {
            get { return (ICommand)GetValue(AnularTicketCommandProperty); }
            set { SetValue(AnularTicketCommandProperty, value); }
        }

        public ICommand CerrarValidarPagoTicketCommand
        {
            get { return (ICommand)GetValue(CerrarValidarPagoTicketCommandProperty); }
            set { SetValue(CerrarValidarPagoTicketCommandProperty, value); }
        }


        public static void CargarDialogoChanged(DependencyObject modal, DependencyPropertyChangedEventArgs e)
        {
            var verdialogo = (bool)e.NewValue;
            var dialogo = (ValidarPagoTicket)modal;

            if (verdialogo)
            {
                dialogo.Mostrar();
            }
            else
            {
                dialogo.Ocultar();
            }

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public void Mostrar()
        {
            var bindingExpresionOverlayOn = this.GetBindingExpression(OverlayOnProperty);

            if (bindingExpresionOverlayOn != null)
            {
                bindingExpresionOverlayOn.UpdateTarget();
            }
            if (OverlayOn == null)
            {
                throw new InvalidOperationException("Las propiedades necesarias no están vinculadas al modelo.");
            }

            Visibility = Visibility.Visible;
            _padreFueHabilitado = OverlayOn.IsEnabled;
            OverlayOn.IsEnabled = false;
        }

        public void Ocultar()
        {
            Visibility = Visibility.Hidden;
            OverlayOn.IsEnabled = _padreFueHabilitado;
        }



        public ValidarPagoTicket()
        {
            InitializeComponent();
            Visibility = Visibility.Hidden;

            //GetListadoTicketsCommand.Execute(null);
        }

        public void GetTicketSeleccionado(object sender, MouseButtonEventArgs e)
        {
            var VM = DataContext as ValidarPagoTicketViewModel;
            var VMC = DataContext as SorteosView;
            var elemento = (sender as Control);
            MAR_Bet ticket = (MAR_Bet)elemento.DataContext;
            ticketSeleccionado = ticket.TicketNo;

            if (VM.CopiarTicketCommand != null)
            {
                VM.CopiarTicketCommand.Execute(new TicketCopiadoResponse { TicketNo = ticketSeleccionado });
                VM.SorteoVM.SorteoViewClass.GetJugadasTicket();
            }
            else
            {
                return;
            }

        }

        public void SeleccionarTicket(object sender, MouseButtonEventArgs e)
        {
            var Vm = DataContext as ValidarPagoTicketViewModel;
            Vm.TicketNumero = GetTicketNumber();
        }


        private void PackIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            var elemento = (sender as Control);
            MAR_Bet ticket = (MAR_Bet)elemento.DataContext;
            ticketSeleccionado = ticket.TicketNo;
        }

        private string GetTicketNumber()
        {
            return ticketSeleccionado;
        }

        private void ReimprimirTicket(object sender, RoutedEventArgs e)
        {
            if (ticketSeleccionado != null)
            {
                var VM = DataContext as ValidarPagoTicketViewModel;
                if (VM.ReimprimirTicketCommand != null)
                {
                    ReimprimirTicketCommand?.Execute(null);
                }
            }
        }

        private void vistaPago_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var VM = DataContext as ValidarPagoTicketViewModel;

            switch (e.Key)
            {
                case Key.F4:
                    ConsultarGanador(sender, e);
                    break;

                case Key.F8:
                    ReImprimirTicket(sender, e);
                    break;

                case Key.F7:
                    AnularTicket(sender, e);
                    break;

                case Key.Escape:
                    if (VM.CerrarValidarPagoTicketCommand != null)
                    {
                        VM.CerrarValidarPagoTicketCommand.Execute(null);
                    }
                    break;

                case Key.F6:
                    CopiarTicketInput(sender, e);
                    break;

                case Key.Right:
                    if (TxtTicket.IsFocused)
                    {
                        TxtPin.Focus();
                        TxtPin.Select(TxtPin?.Text?.Length??0,0);
                    }
                    break;

                case Key.Left:
                    if (TxtPin.IsFocused)
                    {
                        e.Handled = true;
                        TxtTicket.Focus();
                        TxtTicket.Select(TxtTicket?.Text?.Length ?? 0, 0);
                    }
                    break;

                case Key.Tab:

                    if (TxtPin.IsFocused)
                    {
                        e.Handled = true;
                        TxtTicket.Focus();
                        TxtTicket.Select(TxtTicket?.Text?.Length ?? 0, 0);
                    }
                    break;


            }


        }

        private void DataGridRow_MouseEnter(object sender, MouseEventArgs e)
        {
            var elemento = (sender as Control);
            elemento.Focus();
            MAR_Bet ticket = (MAR_Bet)elemento.DataContext;
            ticketSeleccionado = ticket.TicketNo;
        }

        private void DataGridRow_LostMouseCapture(object sender, MouseEventArgs e)
        {
            var Vm = DataContext as ValidarPagoTicketViewModel;
            Vm.TicketNumero = GetTicketNumber();

        }

        private void DataGridCell_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var Vm = DataContext as ValidarPagoTicketViewModel;
            Vm.TicketNumero = GetTicketNumber();
            TxtPin.Focus();
            //this.Focus();
        }

        #region Consultar Ticket Ganador
        private bool consultarGanadorThreadIsBusy = false;
        private void ConsultarGanador(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ValidarPagoTicketViewModel;
            if (ConsultarTicketCommand != null && vm != null)
            {
                string noTicket = TxtTicket.Text;
                string pin = TxtPin.Text;
                bool noTicketEsVacio = InputHelper.InputIsBlank(noTicket);
                bool pinEsVacio = InputHelper.InputIsBlank(pin);

                if (noTicketEsVacio || pinEsVacio)
                {
                    vm.SetMensaje(mensaje: "Favor digitar el \"Ticket No.\" y \"Pin\" ",
                                         icono: "Error",
                                         background: "#DC3545",
                                         puedeMostrarse: true);

                    if (noTicketEsVacio)
                    {
                        TxtTicket.Focus(); return;
                    }

                    if (pinEsVacio)
                    {
                        TxtPin.Focus(); return;
                    }
                }

                if (
                     consultarGanadorThreadIsBusy == false &&
                    (!noTicketEsVacio) &&
                    (!pinEsVacio)

                    )
                {
                    botonConsultarGanador.IsEnabled = false;
                    SpinnerConsultarGanador.Visibility = Visibility.Visible;

                    Task.Factory.StartNew(() =>
                    {
                        consultarGanadorThreadIsBusy = true;
                        System.Threading.Thread.Sleep(777);
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new Action(() =>
                        {
                            try
                            {
                                ConsultarTicketCommand?.Execute(null);
                            }
                            catch { }

                            botonConsultarGanador.IsEnabled = true;
                            SpinnerConsultarGanador.Visibility = Visibility.Collapsed;
                            consultarGanadorThreadIsBusy = false;
                        }));
                    });

                }//fin de If thread is busy

            }//fin de If comando no es null

        }//fin de metodo
        #endregion

        #region Copiar Ticket
        private bool copiarTicketThreadIsBusy = false;
        private void CopiarTicketInput(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ValidarPagoTicketViewModel;

            if (CopiarTicketCommand != null && vm != null)
            {
                string noTicket = TxtTicket.Text;
                bool noTicketEsVacio = InputHelper.InputIsBlank(noTicket);

                if (noTicketEsVacio)
                {
                    vm.SetMensaje(mensaje: "Favor digitar el \"Ticket No.\"",
                                         icono: "Error",
                                         background: "#DC3545",
                                         puedeMostrarse: true);

                    TxtTicket.Focus(); return;
                }

                if (copiarTicketThreadIsBusy == false && (!noTicketEsVacio))
                {
                    botonCopiar.IsEnabled = false;
                    SpinnerCopiar.Visibility = Visibility.Visible;
                    Task.Factory.StartNew(() =>
                    {
                        copiarTicketThreadIsBusy = true;
                        System.Threading.Thread.Sleep(777);
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new Action(() =>
                        {
                            try
                            {
                                CopiarTicketCommand?.Execute(new TicketCopiadoResponse { TicketNo = noTicket });
                            }
                            catch { }

                            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(200) };
                            timer.Tick += (s, args) =>
                            {
                                timer.Stop();
                                botonCopiar.IsEnabled = true;
                                SpinnerCopiar.Visibility = Visibility.Collapsed;
                                copiarTicketThreadIsBusy = false;
                                vm.SorteoVM.SorteoViewClass.GetJugadasTicket();
                            };
                            timer.Start();

                        }));
                    });

                }//fin de If thread is busy

            }//fin de If comando no es null

        }//fin de metodo
        #endregion

        #region Re-Imprimir Ticket
        private bool reImprimirTicketThreadIsBusy = false;
        private void ReImprimirTicket(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ValidarPagoTicketViewModel;

            if (ReimprimirTicketCommand != null && vm != null)
            {
                string noTicket = TxtTicket.Text;
                bool noTicketEsVacio = InputHelper.InputIsBlank(noTicket);

                if (noTicketEsVacio)
                {
                    vm.SetMensaje(mensaje: "Favor digitar el \"Ticket No.\"",
                                         icono: "Error",
                                         background: "#DC3545",
                                         puedeMostrarse: true);

                    TxtTicket.Focus(); return;
                }


                if (reImprimirTicketThreadIsBusy == false && (!noTicketEsVacio))
                {
                    botonReImprimir.IsEnabled = false;
                    SpinnerReImprimir.Visibility = Visibility.Visible;
                    Task.Factory.StartNew(() =>
                    {
                        reImprimirTicketThreadIsBusy = true;
                        System.Threading.Thread.Sleep(777);
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new Action(() =>
                        {
                            try
                            {
                                ReimprimirTicketCommand?.Execute(null);
                            }
                            catch { }

                            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(200) };
                            timer.Tick += (s, args) =>
                            {
                                timer.Stop();
                                botonReImprimir.IsEnabled = true;
                                SpinnerReImprimir.Visibility = Visibility.Collapsed;
                                reImprimirTicketThreadIsBusy = false;
                            };
                            timer.Start();

                        }));
                    });

                }//fin de If thread is busy

            }//fin de If comando no es null

        }//fin de metodo
        #endregion

        #region Anular Ticket
        private bool anularThreadIsBusy = false;
        private void AnularTicket(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ValidarPagoTicketViewModel;
            if (AnularTicketCommand != null && vm != null)
            {
                string noTicket = TxtTicket.Text;
                string pin = TxtPin.Text;
                bool noTicketEsVacio = InputHelper.InputIsBlank(noTicket);
                bool pinEsVacio = InputHelper.InputIsBlank(pin);

                if (noTicketEsVacio || pinEsVacio)
                {
                    vm.SetMensaje(mensaje: "Favor digitar el \"Ticket No.\" y \"Pin\" ",
                                         icono: "Error",
                                         background: "#DC3545",
                                         puedeMostrarse: true);

                    if (noTicketEsVacio)
                    {
                        TxtTicket.Focus(); return;
                    }

                    if (pinEsVacio)
                    {
                        TxtPin.Focus(); return;
                    }
                }

                if (
                     anularThreadIsBusy == false &&
                    (!noTicketEsVacio) &&
                    (!pinEsVacio)

                    )
                {
                    botonAnular.IsEnabled = false;
                    SpinnerAnular.Visibility = Visibility.Visible;

                    Task.Factory.StartNew(() =>
                    {
                        anularThreadIsBusy = true;
                        System.Threading.Thread.Sleep(777);
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new Action(() =>
                        {
                            try
                            {
                                AnularTicketCommand?.Execute(null);
                            }
                            catch { }

                            botonAnular.IsEnabled = true;
                            SpinnerAnular.Visibility = Visibility.Collapsed;
                            anularThreadIsBusy = false;
                        }));
                    });

                }//fin de If thread is busy

            }//fin de If comando no es null

        }//fin de metodo
        #endregion










    }//fin de Clase
}
