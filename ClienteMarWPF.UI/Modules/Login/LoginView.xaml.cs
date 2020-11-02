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

namespace ClienteMarWPF.UI.Modules.Login
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
        }



        private void Login(object sender, RoutedEventArgs e) 
        {
            if (LoginCommand != null)
            {                  
                LoginCommand.Execute(PasswordControl.Password);
            }        
        }





    }// Fin de Clase LoginView
}// Fin de namespace 
