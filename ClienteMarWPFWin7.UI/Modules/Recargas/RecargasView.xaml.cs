﻿using ClienteMarWPFWin7.UI.ViewModels.ModelObservable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClienteMarWPFWin7.UI.Modules.Recargas
{
    /// <summary>
    /// Interaction logic for RecargasView.xaml
    /// </summary>
    public partial class RecargasView : UserControl
    {
        public ObservableCollection<ProveedorRecargasObservable> Proveedors;
        int contador = 0;
        public static readonly DependencyProperty RecargaCommandProperty = DependencyProperty.Register("SendRecargaCommand", typeof(ICommand), typeof(RecargasView), new PropertyMetadata(null));
        public bool EstaEnElUltimoCaracterDeTelefono { get; set; }
        public bool EstaEnElPrimerCaracterDeMonto { get; set; }
        public bool IsFirstTime { get; set; } = true;
        public ICommand SendRecargaCommand
        {
            get
            {
                return (ICommand)GetValue(RecargaCommandProperty);

            }
            set { SetValue(RecargaCommandProperty, value); }
        }



        public bool IsArrow { get; set; } = false;
        // public ProveedorRecargasObservable Provedor { get; set; }
        public ProveedorRecargasObservable ProveedorSelect { get; set; }

        public RecargasView()
        {
            InitializeComponent();
            IsArrow = true;
            EstaEnElPrimerCaracterDeMonto = false;
            EstaEnElUltimoCaracterDeTelefono = false;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);

        }



        private void listSorteo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void img_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            IsArrow = false;
            switch (e.Key)
            {

                case Key.Space:
                    IsArrow = true;
                    if (proveedores.SelectedItem != null)
                    {
                        var seleccionado = proveedores.SelectedItem as ProveedorRecargasObservable;
                        seleccionado.IsSelected = true;
                        var vm = DataContext as RecargasViewModel;
                        vm.SetProveedorFromkeyDown(seleccionado.OperadorID);
                    }

                    FocusInputs();
                    EstaEnElUltimoCaracterDeTelefono = true;
                    break;


                case Key.Enter:
                    IsArrow = true;

                    if (proveedores.SelectedItem != null)
                    {
                        var seleccionado = proveedores.SelectedItem as ProveedorRecargasObservable;
                        seleccionado.IsSelected = true;
                        var vm = DataContext as RecargasViewModel;
                        vm.SetProveedorFromkeyDown(seleccionado.OperadorID);

                    }

                    FocusInputs();
                    EstaEnElUltimoCaracterDeTelefono = true;
                    break;

                case Key.Up:
                    IsArrow = true;
                    if (telefono.IsFocused || monto.IsFocused)
                    {
                        proveedores.Focus();

                        proveedores.SelectedIndex = proveedores.SelectedIndex;

                        //var prueba = (List<ProveedorRecargasObservable>) proveedores.ItemsSource;
                        //proveedores.SelectedItem = prueba.First();

                    }



                    break;

                case Key.Left:

                    IsArrow = true;
                    EstaEnElUltimoCaracterDeTelefono = false;

                    if (monto.IsFocused && monto.SelectionStart == 0)
                    {
                        if (monto.Text.Length > 0)
                        {
                            if (EstaEnElPrimerCaracterDeMonto)
                            {
                                telefono.Focus();
                                telefono.SelectionStart = telefono.Text.Length;
                                telefono.SelectionLength = 0;
                                EstaEnElPrimerCaracterDeMonto = false;
                            }
                            EstaEnElPrimerCaracterDeMonto = true;
                            EstaEnElUltimoCaracterDeTelefono = true;
                        }
                        else
                        {
                            telefono.Focus();
                            EstaEnElUltimoCaracterDeTelefono = true;
                        }


                        //telefono.Focus();
                        //telefono.SelectionStart = telefono.Text.Length;
                        //telefono.SelectionLength = 0;
                        //EstaEnElUltimo = false;
                        //TriggerButtonClickEvent(btnSeleccionaTelefono);
                    }


                    if (proveedores.IsFocused)
                    {
                        proveedores.SelectedIndex -= 1;

                        if (proveedores.SelectedIndex == -1)
                        {
                            proveedores.SelectedIndex = 0;
                        }
                    }
                    break;



                case Key.Right:
                    IsArrow = true;
                    EstaEnElPrimerCaracterDeMonto = false;

                    if (telefono.IsFocused && telefono.SelectionStart == telefono.Text.Length)
                    {
                        if (telefono.Text.Length > 0)
                        {
                            if (EstaEnElUltimoCaracterDeTelefono)
                            {
                                monto.Focus();
                                monto.SelectionStart = 0;
                                monto.SelectionLength = 0;
                                EstaEnElUltimoCaracterDeTelefono = false;
                            }
                            EstaEnElUltimoCaracterDeTelefono = true;
                            if (monto.SelectionStart == monto.Text.Length)
                            {
                                EstaEnElPrimerCaracterDeMonto = false;
                            }
                            else
                            {
                                EstaEnElPrimerCaracterDeMonto = true;
                            }

                        }
                        else {

                            monto.Focus();
                            EstaEnElPrimerCaracterDeMonto = false;
                        }


                        //FocusInputs();
                        //TriggerButtonClickEvent(btnSeleccionaMonto);
                    }



                    if (proveedores.IsFocused || IsFirstTime)
                    {
                        proveedores.SelectedIndex += 1;
                        IsFirstTime = false;
                    }

                    break;


                case Key.Down:

                    IsArrow = true;

                    if (proveedores.SelectedItem != null)
                    {
                        var seleccionado = proveedores.SelectedItem as ProveedorRecargasObservable;
                        seleccionado.IsSelected = true;
                        var vm = DataContext as RecargasViewModel;
                        vm.SetProveedorFromkeyDown(seleccionado.OperadorID);
                        telefono.Focus();
                    }

                    EstaEnElUltimoCaracterDeTelefono = true;
                    telefono.SelectionStart = telefono.Text.Length;
                    //telefono.Focus();
                    ////proveedores.SelectedItem = null;
                    //Console.WriteLine(proveedores.SelectedIndex);
                    break;

                case Key.F12:
                    if ((telefono.Text.Trim().Length > 0 && monto.Text.Length > 0) && (telefono.IsFocused || monto.IsFocused || proveedores.IsFocused))
                    {
                        SendRecargaCommand.Execute("");

                    }
                    break;

            }

        }

        public void FocusInputs()
        {

            //var seleccionado = proveedores.SelectedItem as ProveedorRecargasObservable;
            //seleccionado.IsSelected = true;
            if ((telefono.Text.Trim().Length > 0 && monto.Text.Length > 0) && (telefono.IsFocused || monto.IsFocused))
            {
                SendRecargaCommand.Execute("");

            }
            else
            {
                if (telefono.IsFocused)
                {

                    monto.Focus();
                }
                else
                {
                    telefono.Focus();
                }
            }




        }

        private void ListProveedores(object sender, SelectionChangedEventArgs e)
        {
            var prueba = proveedores.ItemsSource;
            var ok = proveedores.SelectedItem;
            //for (int i = 0; i < prueba.; i++)
            //{

            //}
        }

        private void proveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (contador == 0)
            {
                proveedores.SelectedIndex = 0;
                proveedores.Focus();
                contador++;
            }



            if (!IsArrow)
            {
                var seleccionado = proveedores.SelectedItem as ProveedorRecargasObservable;

                if (seleccionado != null)
                {
                    seleccionado.IsSelected = true;
                    var vm = DataContext as RecargasViewModel;
                    vm.SetProveedorFromkeyDown(seleccionado.OperadorID);
                }
            }
            IsArrow = false;
        }

        private void botonIniciar_Click(object sender, RoutedEventArgs e)
        {

            if (SendRecargaCommand != null)
            {
                SendRecargaCommand.Execute(null);
                telefono.Focus();
            }


        }

        private void btnSeleccionaTelefono_Click(object sender, RoutedEventArgs e)
        {
            telefono.Focus();
            telefono.SelectAll();
        }

        private void btnSeleccionaJugada_Click(object sender, RoutedEventArgs e)
        {
            monto.Focus();
            monto.SelectAll();
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            proveedores.SelectedIndex = 0;
            proveedores.Items.Refresh();
        }

        private void telefono_GotFocus(object sender, RoutedEventArgs e)
        {
            if (proveedores.SelectedItem != null)
            {
                var seleccionado = proveedores.SelectedItem as ProveedorRecargasObservable;
                seleccionado.IsSelected = true;
                var vm = DataContext as RecargasViewModel;
                vm.SetProveedorFromkeyDown(seleccionado.OperadorID);

            }
        }

        private void monto_GotFocus(object sender, RoutedEventArgs e)
        {
            if (proveedores.SelectedItem != null)
            {
                var seleccionado = proveedores.SelectedItem as ProveedorRecargasObservable;
                seleccionado.IsSelected = true;
                var vm = DataContext as RecargasViewModel;
                vm.SetProveedorFromkeyDown(seleccionado.OperadorID);

            }
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            if( (proveedores.SelectedItem != null && !proveedores.IsFocused) && (!telefono.IsFocused && !monto.IsFocused))
            {
                telefono.Focus();
            }
        }


        //private void telefono_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    proveedores.Items.Refresh();
        //}

        //private void monto_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    proveedores.Items.Refresh();
        //}
    }
}
