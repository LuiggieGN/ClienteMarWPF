
using System.Windows;
using System.Windows.Threading;
using System.Windows.Input;

using ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.UI.State.CuadreBuilders;

using System;
using System.Text.RegularExpressions;

namespace ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Cuadre.Windows.Cuadre
{
    public partial class CuadreView : Window
    {
        public Window ParentWindow;
        private IAuthenticator _aut;
        private ICuadreBuilder _builder;
        private Regex _patronSoloTextoEsValido = new Regex("^[a-zA-Z]+$");

        public CuadreView(Window parentWindow, object cuadreContext, IAuthenticator aut, ICuadreBuilder cuadreBuilder)
        {
            _aut = aut;
            _builder = cuadreBuilder;

            InitializeComponent();

            ParentWindow = parentWindow;

            DataContext = cuadreContext;

            var vm = (DataContext as CuadreViewModel);

            if (vm != null)
            {
                vm.SetFocusOnMontoContado = () =>
                {
                    var timer = new DispatcherTimer();

                    timer.Interval = TimeSpan.FromMilliseconds(7);

                    timer.Tick += (object sender, EventArgs e) => {
                        timer.Stop();
                        inpMontoContado.Focus();
                        inpMontoContado.Select(inpMontoContado?.Text?.Length ?? 0, 0);
                    };
                    timer.Start();
                };
            }

            inpMontoContado.Focus();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Left = ParentWindow.Left + (ParentWindow.Width - ActualWidth) / 2;
            Top = ParentWindow.Top + (ParentWindow.Height - ActualHeight) / 2;
        }

        private void OnCerrarVentanaClick(object sender, RoutedEventArgs e)
        {
            SetCajaDeBancaDisponible();
            Close();
        }

        private void OnCerrarVentanaClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SetCajaDeBancaDisponible();
        }

        private void SetCajaDeBancaDisponible()
        {
            try
            {
                _builder.SetearCajaDisponibilidad(new CajaDisponibilidadDTO() { Cajaid = null, Bancaid = _aut.BancaConfiguracion.BancaDto.BancaID, Disponibilidad = true });
            }
            catch { }
        }


        private void txtNombreCajera_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool elTextoEsPermitido = _patronSoloTextoEsValido.IsMatch(e.Text);

            e.Handled = !elTextoEsPermitido;
        }

        private void CuandoTeclaBaja_VentanaPrincipal(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                OnCerrarVentanaClick(sender, e);
                return;
            }

            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                LogicaSigueAlSiguienteInputDescendiendo();
            }
            else if (e.Key == Key.Tab)
            {
                e.Handled = true;
                LogicaSigueAlSiguienteInputDescendiendo();
            }
        }
        private void CuandoTeclaSube_VentanaPrincipal(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                e.Handled = true;
                LogicaSigueAlSiguienteInputDescendiendo();
            }
            else if (e.Key == Key.Up)
            {
                e.Handled = true;
                LogicaSigueAlSiguienteInputAscendiendo();
            }
            else if (e.Key == Key.F6)
            {
                e.Handled = true;
                var vm = DataContext as CuadreViewModel;
                if (vm != null)
                {
                    e.Handled = true;
                    vm.AbrirDesgloseCommand?.Execute(new object[] { CuadreVista });
                }
            }
            else if (e.Key == Key.F9)
            {
                e.Handled = true;
                OnCerrarVentanaClick(sender, e);
            }
            else if (
                e.Key == Key.F12 ||
                e.Key == Key.Add
            )
            {
                var vm = DataContext as CuadreViewModel;
                if (vm != null)
                {
                    e.Handled = true;
                    vm.RegistrarCuadreCommand?.Execute(CuadreVista);
                }
            }
        }

        #region Descendiendo

        private void LogicaSigueAlSiguienteInputDescendiendo()
        {
            if (txtDescripcionBanca.IsFocused)
            {
                txtDescripcionBalance.Focus();
            }
            else if (txtDescripcionBalance.IsFocused)
            {
                txtDescripcionBalanceMinimo.Focus();
            }
            else if (txtDescripcionBalanceMinimo.IsFocused)
            {
                txtDescripcionMontoPorPagar.Focus();
            }
            else if (txtDescripcionMontoPorPagar.IsFocused)
            {
                inpMontoContado.Focus();
                inpMontoContado.Select(inpMontoContado?.Text?.Length ?? 0, 0);
            }
            else if (inpMontoContado.IsFocused)
            {
                txtArqueoResultante.Focus();
            }
            else if (txtArqueoResultante.IsFocused)
            {
                txtMontoARetirarODepositar.Focus();
                txtMontoARetirarODepositar.Select(txtMontoARetirarODepositar?.Text?.Length ?? 0, 0);
            }
            else if (txtMontoARetirarODepositar.IsFocused)
            {
                txtNombreCajera.Focus();
                txtNombreCajera.Select(txtNombreCajera?.Text?.Length ?? 0, 0);
            }
            else if (txtNombreCajera.IsFocused)
            {
                inpMontoContado.Focus();
            }
        }


        #endregion

        #region Ascendiendo

        private void LogicaSigueAlSiguienteInputAscendiendo()
        {
            if (txtNombreCajera.IsFocused)
            {
                txtMontoARetirarODepositar.Focus();
                txtMontoARetirarODepositar.Select(txtMontoARetirarODepositar?.Text?.Length ?? 0, 0);
            }
            else if (txtMontoARetirarODepositar.IsFocused)
            {
                txtArqueoResultante.Focus();
            }
            else if (txtArqueoResultante.IsFocused)
            {
                inpMontoContado.Focus();
                inpMontoContado.Select(inpMontoContado?.Text?.Length ?? 0, 0);
            }
            else if (inpMontoContado.IsFocused)
            {
                txtDescripcionMontoPorPagar.Focus();
            }
            else if (txtDescripcionMontoPorPagar.IsFocused)
            {
                txtDescripcionBalanceMinimo.Focus();
            }
            else if (txtDescripcionBalanceMinimo.IsFocused)
            {
                txtDescripcionBalance.Focus();
            }
            else if (txtDescripcionBalance.IsFocused)
            {
                txtDescripcionBanca.Focus();
            }
            else if (txtDescripcionBanca.IsFocused)
            {
                txtNombreCajera.Focus();
                txtNombreCajera.Select(txtNombreCajera?.Text?.Length ?? 0, 0);
            }
        }
















        #endregion



    }//fin de clase
}
