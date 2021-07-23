
#region Namespaces
using ClienteMarWPFWin7.UI.ViewModels.Helpers;
using ClienteMarWPFWin7.UI.ViewModels.ModelObservable;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows.Media;
using ClienteMarWPFWin7.UI.Modules.CincoMinutos;
#endregion

namespace ClienteMarWPFWin7.UI.Modules.PegaMas
{
    public partial class PegaMasView : UserControl
    {
        private PegaMasViewModel vm;
        private bool ventaHiloOcupado = false;

        public PegaMasView() => InitializeComponent();

        private void ModuloCargado(object sender, RoutedEventArgs e)
        {
            vm = DataContext as PegaMasViewModel;

            vm.FocusEnPrimerInput = () => Enfocar_Inicial();

            vm.FocusEnUltimoInput = () => Enfocar_Ultimo();

            vm.EscrolearHaciaAbajoGridApuesta = () => EscrolearHaciaAbajoGridJugdas();

            vm.FocusEnPrimerInput?.Invoke();
        }



        private void ModuloKeyUp(object sender, KeyEventArgs e)
        {
            if (vm != null && e.Key == Key.Enter)
            {
                AgregarJugadaSiNoExisteEntradaVacia();
            }
            else if (vm != null && e.Key == Key.F9)
            {
                vm.RemoverJugadasCommand?.Execute(null);
            }
            else if (vm != null && (e.Key == Key.F12 || e.Key == Key.Add))
            {
                Vender(sender, e);
            }
            else if (e.Key == Key.Right ||
                     e.Key == Key.Left)
            {
                e.Handled = true;
                SeleccionarEntradaSegunDireccion(direccionAsc: (e.Key == Key.Right) ? true : false);
            }
            else
            {
                var entrada = sender as TextBox;
                if (entrada != null)
                {
                    NavegarAProximaEntradaSiExcedeLimite(entrada);
                }
            }
        }

        private void ModuloKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                e.Handled = true;
                SeleccionarEntradaSegunDireccion(direccionAsc: true);
            }
        }

        private void RemoverSoloUnaJugada(object sender, RoutedEventArgs e)
        {
            var objEnFila = GridJugadas.SelectedItem as PegaMasApuestaObservable;

            if (objEnFila != null && vm != null)
            {
                vm.RemoverSoloUnaJugadaCommand?.Execute(objEnFila.Id);
            }
        }

        private void AgregarJugadaSiNoExisteEntradaVacia()
        {
            var entradas = new List<TextBox>() { EntradaD1, EntradaD2, EntradaD3, EntradaD4, EntradaD5 };

            bool sepuedeagregar = true;

            foreach (var item in entradas)
            {
                if (InputHelper.InputIsBlank(item.Text))
                {
                    sepuedeagregar = false;
                    Enfocar_Entrada(entrada: item);
                    break;
                }
            }

            if (sepuedeagregar && vm != null)
            {
                vm.AgregaApuestaCommand?.Execute(null);
            }

        }// AgregarJugadaSiNoExisteEntradaVacia( )

        private void SeleccionarEntradaSegunDireccion(bool direccionAsc)
        {
            var entradas = new List<TextBox>() { EntradaD1, EntradaD2, EntradaD3, EntradaD4, EntradaD5 };

            for (int i = 0; i < entradas.Count; i++)
            {
                if (entradas[i].IsFocused)
                {
                    if (direccionAsc)
                    {
                        if (i == entradas.Count - 1)
                        {
                            Enfocar_Entrada(EntradaD1); break;
                        }
                        else
                        {
                            Enfocar_Entrada(entradas[i + 1]); break;
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            Enfocar_Entrada(EntradaD5); break;
                        }
                        else
                        {
                            Enfocar_Entrada(entradas[i - 1]); break;
                        }
                    }

                }//If

            }//For

        }// SeleccionarEntradaSegunDireccion( )

        private void NavegarAProximaEntradaSiExcedeLimite(TextBox entrada)
        {
            if ((!InputHelper.InputIsBlank(entrada.Text)) && entrada.Text.Length > 1)
            {
                const int limite = 2;

                var entradas = new List<TextBox>() { EntradaD1, EntradaD2, EntradaD3, EntradaD4, EntradaD5 };

                int indiceFinal = entradas.Count - 1;

                int indiceDeEntrada = entradas.FindIndex(inp => inp.Name.Equals(entrada.Name, StringComparison.OrdinalIgnoreCase));

                if (indiceDeEntrada >= 0 && indiceDeEntrada <= indiceFinal)
                {
                    var nuevo = Regex.Replace(entrada.Text ?? string.Empty, @"\s+", "").Trim();

                    if (nuevo == string.Empty) { entrada.Text = nuevo; entrada.Focus(); return; }

                    if (nuevo.Length < limite) { entrada.Text = nuevo; entrada.Focus(); return; }

                    if (indiceFinal == indiceDeEntrada) { entrada.Text = nuevo; entrada.Focus(); return; }

                    Enfocar_Entrada(entradas[indiceDeEntrada + 1]);

                }//If

            }//If        

        }// NavegarHaciaElOtro( )

        private void Vender(object sender, RoutedEventArgs e)
        {
            Thread.Sleep(500);

            if (vm != null)
            {
                if (ventaHiloOcupado == false)
                {
                    BotonVender.IsEnabled = false;
                    SpinnerVender.Visibility = Visibility.Visible;

                    Task.Factory.StartNew(() =>
                    {
                        ventaHiloOcupado = true;

                        Thread.Sleep(500);

                        Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new Action(() =>
                        {
                            try
                            {
                                vm.VenderCommand?.Execute(null);
                            }
                            catch { }

                            BotonVender.IsEnabled = true;
                            SpinnerVender.Visibility = Visibility.Collapsed;
                            ventaHiloOcupado = false;
                        }));

                    });//StarNew( )

                }//If ventaHiloOcupado == false 

            }//If vm != null

        }// Vender( )

        private void Enfocar_Entrada(TextBox entrada)
        {
            entrada.Focus();
            entrada.SelectAll();
        }

        private void Enfocar_Inicial() => EntradaD1.Focus();
        private void Enfocar_Ultimo() => EntradaD5.Focus();
        private void EscrolearHaciaAbajoGridJugdas()
        {
            if (GridJugadas.Items.Count > 3)
            {
                GridJugadas.UpdateLayout();

                GridJugadas.ScrollIntoView(GridJugadas.Items.GetItemAt(GridJugadas.Items.Count - 1), null);

            }// If

        }// EscrolearHaciaAbajoGridJugdas ( )

        private void Consultar(object sender, RoutedEventArgs e)
        {
            try
            {
                ConsultaCincoMinutos modal = new ConsultaCincoMinutos();
                modal.ShowDialog();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }//Clase

}
