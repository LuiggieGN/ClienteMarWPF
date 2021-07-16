using ClienteMarWPFWin7.UI.ViewModels.ModelObservable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        }


        private void ModuloKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                e.Handled = true;
                Seleccionar_Entrada_Segun_Direccion(direccionAsc:  true  );
            }
        }

        private void ModuloCargado(object sender, RoutedEventArgs e)
        {
            vm = DataContext as PegaMasViewModel;
            vm.FocusEnPrimerInput = () => IniciarFoco();
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

            var entradas = new TextBox[] { EntradaD1, EntradaD2, EntradaD3, EntradaD4, EntradaD5 };

            for (int i = 0; i < entradas.Length; i++)
            {
                if (entradas[i].IsFocused)
                {
                    if (direccionAsc)
                    {
                        if (i == entradas.Length - 1)
                        {
                            SeleccionarTodoElContenido(EntradaD1); break;
                        }
                        else
                        {
                            SeleccionarTodoElContenido(entradas[i + 1]); break;
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            SeleccionarTodoElContenido(EntradaD5); break;
                        }
                        else
                        {
                            SeleccionarTodoElContenido(entradas[i - 1]); break;
                        }
                    }

                }//fin de If

            }//fin  de For
        }

        private void SeleccionarTodoElContenido(TextBox entrada) 
        {
            entrada.Focus();
            entrada.SelectAll();
        }

        private void IniciarFoco() => EntradaD1.Focus();


    }// Clase
}
