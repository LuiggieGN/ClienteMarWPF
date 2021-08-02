using System;
using System.Collections.Generic;
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
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using ClienteMarWPFWin7.UI.ViewModels.Helpers;
using ClienteMarWPFWin7.UI.Modules.CincoMinutos;
using ClienteMarWPFWin7.Data;
using System.IO.Compression;
using System.Threading;

namespace ClienteMarWPFWin7.UI.Modules.Login
{

    public partial class LoginView : UserControl
    {

        public static readonly DependencyProperty LoginCommandProperty = DependencyProperty.Register("LoginCommand", typeof(ICommand), typeof(LoginView), new PropertyMetadata(null));


        public ICommand LoginCommand
        {
            get
            { 
               return (ICommand)GetValue(LoginCommandProperty); 
            
            }
            set { SetValue(LoginCommandProperty, value); }
        }



        public LoginView()
        {
            InitializeComponent();
            TxtUsername.Focus();
        }

        private void Login(object sender, RoutedEventArgs e) 
        {
            if (LoginCommand != null)
            {                  
                LoginCommand.Execute(PasswordControl.Password);

                Thread.Sleep(1000);
                if (SessionGlobals.permisos)
                {
                    Pizarra pizarra = new Pizarra();
                    pizarra.Show();
                }

            }

        }

        private void PressTecla(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    if ( TxtUsername.Text != "" && PasswordControl.Password == "")
                    {
                        PasswordControl.Focus();
                    }else if( TxtUsername.Text == "" && PasswordControl.Password != "")
                    {
                        TxtUsername.Focus();
                    }
                    else if( PasswordControl.IsFocused || TxtUsername.IsFocused ) 
                    {
                        try
                        {
                            var btnIniciar = botonIniciar;

                            if (btnIniciar != null)
                            {
                                var peer = new ButtonAutomationPeer(btnIniciar);
                                var invokeProvider = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                                invokeProvider?.Invoke();
                            }
                        }
                        catch { }
                    }
                    
                    break;

                case Key.Up:
                    if (PasswordControl.IsFocused)
                    {
                        TxtUsername.Focus();
                    }
                        break;

                case Key.Down:
                    if (TxtUsername.IsFocused)
                    {
                        PasswordControl.Focus();
                    }
                    break;

            }
        }

        private void Carga(object sender, RoutedEventArgs e)
        {
            TxtUsername.Focus();
        }
    }// Fin de Clase LoginView
}// Fin de namespace 
