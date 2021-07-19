
#region Namespaces
using ClienteMarWPFWin7.UI.ViewModels.Helpers;
using ClienteMarWPFWin7.UI.ViewModels.ModelObservable;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
#endregion

namespace ClienteMarWPFWin7.UI.Modules.PegaMas
{
    public partial class PegaMasView : UserControl
    {
        private PegaMasViewModel vm;
        public PegaMasView()
        {
            InitializeComponent();
        }

        private void ModuloKeyUp(object sender, KeyEventArgs e)
        {
            if (vm != null && e.Key == Key.Enter)
            {
                vm.AgregaApuestaCommand?.Execute(null);
            }
            else if (vm != null && e.Key == Key.F9)
            {
                vm.RemoverJugadasCommand?.Execute(null);
            }
            else if (vm != null && (e.Key == Key.F12 || e.Key == Key.Add))
            {
                vm.VenderCommand?.Execute(null);
            }
            else if (e.Key == Key.Right ||
                     e.Key == Key.Left)
            {
                e.Handled = true;
                Seleccionar_Entrada_Segun_Direccion(direccionAsc: (e.Key == Key.Right) ? true : false);
            }
            else
            {
                var entrada = sender as TextBox;
                if (entrada != null)
                {                    
                    Navegar_Ala_Siguiente_Entrada_Al_Exceder_Limite_De_Caracteres(entrada);
                }
            }
        }

        private void ModuloKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                e.Handled = true;
                Seleccionar_Entrada_Segun_Direccion(direccionAsc: true);
            }
        }

        private void ModuloCargado(object sender, RoutedEventArgs e)
        {
            vm = DataContext as PegaMasViewModel;

            vm.FocusEnPrimerInput = () => Enfocar_Inicial();
            vm.FocusEnUltimoInput = () => Enfocar_Ultimo();
            vm.FocusEnPrimerInput?.Invoke();
        }

        private void RemoverSoloUnaJugada(object sender, RoutedEventArgs e)
        {
            var objEnFila = GridJugadas.SelectedItem as PegaMasApuestaObservable;

            if (objEnFila != null && vm != null)
            {
                vm.RemoverSoloUnaJugadaCommand?.Execute(objEnFila.Id);
            }
        }

        private void Seleccionar_Entrada_Segun_Direccion(bool direccionAsc)
        {
            //  direccionAsc => 0 = Anterior 1 = Siguiente 

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

        }//Seleccionar_Entrada_Segun_Direccion( )

        private void Navegar_Ala_Siguiente_Entrada_Al_Exceder_Limite_De_Caracteres(TextBox entrada)
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

                    if (indiceFinal == indiceDeEntrada){ entrada.Text = nuevo; entrada.Focus(); return; }

                    Enfocar_Entrada(entradas[indiceDeEntrada + 1]);

                }//If

            }//If        

        }//Navegar_Ala_Siguiente_Entrada_Al_Exceder_Limite_De_Caracteres( )








        private void Enfocar_Entrada(TextBox entrada)
        {
            entrada.Focus();
            entrada.SelectAll();
        }
        private void Enfocar_Inicial() => EntradaD1.Focus();
        private void Enfocar_Ultimo() => EntradaD5.Focus();

    }// Clase
}
