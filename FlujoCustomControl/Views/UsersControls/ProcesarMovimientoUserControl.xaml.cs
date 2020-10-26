using FlujoCustomControl.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlujoCustomControl.Views.UsersControls
{
    /// <summary>
    /// Lógica de interacción para ProcesarMovimientoUserControl.xaml
    /// </summary>
    public partial class ProcesarMovimientoUserControl : UserControl
    {
        public ProcesarMovimientoUserControl()
        {
            InitializeComponent();

        }

        private static readonly Regex _regex = new Regex("[^0-9.]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }


        private void RootElement_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox s = e.Source as TextBox;
                PasswordBox p = e.Source as PasswordBox;

                if (s != null)
                {
                    s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
                else if (p != null)
                {
                    if (p.Name == "passBoxUserPin" && !p.Password.Equals(string.Empty))
                    {
                        var viewModel = (ProcesarMovimientoViewModel)DataContext;
                        GroupedPasswordBox grouped = new GroupedPasswordBox();
                        grouped.SecureCedula = passBoxUserCedula;
                        grouped.SecurePin = passBoxUserPin;


                        if (viewModel.Button_Procesar_Command.CanExecute(grouped))
                        {
                            viewModel.Button_Procesar_Command.Execute(grouped);
                        }
                    }

                    if (token.IsVisible)
                    {
                        p.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                        p.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    }
                    p.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }

                e.Handled = true;
            }
        }

        private void TxtMonto_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        // Use the DataObject.Pasting Handler 
        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
    }
}
