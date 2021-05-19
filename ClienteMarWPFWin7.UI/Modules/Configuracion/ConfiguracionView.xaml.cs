using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

namespace ClienteMarWPFWin7.UI.Modules.Configuracion
{
    /// <summary>
    /// Interaction logic for ConfiguracionView.xaml
    /// </summary>
    public partial class ConfiguracionView : Window
    {

        public Window ParentWindow;
        public static readonly DependencyProperty SaveConfigCommandProperty = DependencyProperty.Register("SaveConfigCommand", typeof(ICommand), typeof(ConfiguracionView), new PropertyMetadata(null));

        public ICommand SaveConfigCommand
        {
            get { return (ICommand)GetValue(SaveConfigCommandProperty); }
            set { SetValue(SaveConfigCommandProperty, value); }
        }

        public ConfiguracionView(Window parent, object context)
        {
            DataContext = context;

            InitializeComponent();

            ParentWindow = parent;

            txtClaveAutorizacion.Focus();
            BotonAutorizar.BorderThickness = new Thickness(2, 2, 2, 2);
            BotonCerrar.BorderThickness = new Thickness(2, 2, 2, 2);
            BotonAutorizar.BorderBrush = Brushes.Black;
            BotonCerrar.BorderBrush = Brushes.Black;

        }

        private void ConfiguracionWindow_Loaded(object sender, RoutedEventArgs e)
        {

            if (ParentWindow != null)
            {
                this.Left = ParentWindow.Left + (ParentWindow.Width - this.ActualWidth) / 2;
                this.Top = ParentWindow.Top + (ParentWindow.Height - this.ActualHeight) / 2;
                
            }
            else
            {
                CenterWindowOnScreen();
               
            }
        }

        private void PressTecla(object sender, KeyEventArgs e)
        {
            if (BotonCerrar.IsFocused)
            {
                BotonAutorizar.BorderBrush = Brushes.Blue;
                BotonCerrar.BorderBrush = Brushes.Black;
                BotonCerrar.BorderThickness = new Thickness(2, 2, 2, 2);

            }
            else if (BotonAutorizar.IsFocused)
            {
                BotonCerrar.BorderBrush = Brushes.Blue;
                BotonAutorizar.BorderBrush = Brushes.Black;
                BotonAutorizar.BorderThickness = new Thickness(2, 2, 2, 2);

            }

            switch (e.Key)
            {
                case Key.Left:
                    BotonCerrar.Focus();
                    if (direccionip.IsFocused)
                    {
                        bancaid.Focus();
                        TriggerButtonClickEvent(btnSeleccionaID);

                    }else if (ticket.IsFocused)
                    {
                        direccionip.Focus();
                        TriggerButtonClickEvent(btnSeleccionaIP);
                    }
                    break;

                case Key.Right:
                    BotonAutorizar.Focus();

                    if (bancaid.IsFocused)
                    {
                        direccionip.Focus();
                        TriggerButtonClickEvent(btnSeleccionaIP);
                    }
                    else if (direccionip.IsFocused)
                    {
                        ticket.Focus();
                    }

                    break;

                case Key.Escape:
                    if(bancaid.IsFocused || !bancaid.IsFocused)
                    {
                        if (PanelConfiguracion.Visibility == Visibility.Visible)
                        {
                            Cerrar(sender, e);
                        }
                    }

                    break;

                //case Key.Down:
                //    if (bancaid.IsFocused)
                //    {
                //        botonConfirmar.Focus();

                //    }
                //    break;


            }
        }



        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
            if (e.Handled)
            {
                if (Keyboard.IsKeyDown(Key.Enter))
                {
                    Button_Click(sender, e);
                }
            }
               
            if(!e.Handled || e.Handled)
            {
                if (Keyboard.IsKeyDown(Key.Escape))
                {
                    if(PanelAutoriacion.Visibility == Visibility.Visible && PanelConfiguracion.Visibility == Visibility.Collapsed)
                    {
                        Button_Click_1(sender, e);
                    }

                }
            }

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

            int clave;

            bool puede = Int32.TryParse(txtClaveAutorizacion?.Password??string.Empty, out clave);


            if (puede)
            {
                if (clave == 159753)
                {
                    PanelAutoriacion.Visibility = Visibility.Collapsed;
                    PanelConfiguracion.Visibility = Visibility.Visible;
                    bancaid.Focus();
                    TriggerButtonClickEvent(btnSeleccionaID);
                }
                else
                {
                    PanelAutoriacion.Visibility = Visibility.Visible;
                    PanelConfiguracion.Visibility = Visibility.Collapsed;
                    MessageBox.Show("La clave es incorrecta",
                    "Aviso", MessageBoxButton.OK);
                }
            }
            else
            {
                MessageBox.Show("El campo de clave no puede estar vacio",
                    "Aviso", MessageBoxButton.OK);
            }


        }

        private void Cerrar(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SeleccionaID(object sender, RoutedEventArgs e)
        {
            bancaid.Focus();
            bancaid.SelectAll();
        }

        private void SeleccionaIP(object sender, RoutedEventArgs e)
        {
            direccionip.Focus();
            direccionip.SelectAll();
        }

        public void TriggerButtonClickEvent(Button boton)
        {
            try
            {

                if (boton != null)
                {
                    var peer = new ButtonAutomationPeer(boton);
                    var invokeProvider = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                    invokeProvider?.Invoke();
                }
            }
            catch { }
        }

   
    }
}
