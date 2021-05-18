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
                if (VM.CopiarTicketCommand != null)
                {
                    VM.CopiarTicketCommand.Execute(new TicketCopiadoResponse { TicketNo = ticketSeleccionado });
                }
            }
        }

        private void vistaPago_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var VM = DataContext as ValidarPagoTicketViewModel;

            switch (e.Key)
            {
                case Key.F4:
                    if (VM.ConsultarTicketCommand != null)
                    {
                        VM.ConsultarTicketCommand.Execute(null);
                    }
                    break;

                case Key.F8:
                    if (VM.ReimprimirTicketCommand != null)
                    {
                        VM.ReimprimirTicketCommand.Execute(null);
                    }
                    break;

                case Key.F7:
                    if (VM.AnularTicketCommand != null)
                    {
                        VM.AnularTicketCommand.Execute(null);
                    }
                    break;

                case Key.Escape:
                    if (VM.CerrarValidarPagoTicketCommand != null)
                    {
                        VM.CerrarValidarPagoTicketCommand.Execute(null);
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

        }

        private void CopiarTicketInput(object sender, MouseButtonEventArgs e)
        {
            var VM = DataContext as ValidarPagoTicketViewModel;
            if (VM.CopiarTicketCommand != null)
            {
                if (VM.TicketNumero != null && VM.TicketNumero.Length != 0)
                {
                    VM.CopiarTicketCommand.Execute(new TicketCopiadoResponse { TicketNo = VM.TicketNumero });
                    VM.SorteoVM.SorteoViewClass.GetJugadasTicket();
                }

            }
        }



        private bool reImprimirTicketThreadIsBusy = false;
        private void ReImprimirTicket(object sender, RoutedEventArgs e)
        {
            if (ReimprimirTicketCommand != null)
            {
                string noTicket = TxtTicket.Text;

                if (reImprimirTicketThreadIsBusy == false && (!InputHelper.InputIsBlank(noTicket)))
                {
                    botonReImprimir.IsEnabled = false;

                    Task.Factory.StartNew(() =>
                    {
                        reImprimirTicketThreadIsBusy = true;
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new Action(() =>
                        {
                            try
                            {
                                ReimprimirTicketCommand?.Execute(null);
                            }
                            catch {  }

                            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(5000) };
                            timer.Tick += (s, args) =>
                            {
                                timer.Stop();
                                botonReImprimir.IsEnabled = true;
                                reImprimirTicketThreadIsBusy = false;
                            };
                            timer.Start();

                        }));
                    });

                }//fin de If thread is busy

            }//fin de If comando no es null

        }//fin de metodo

    }
}
