using ClienteMarWPF.UI.ViewModels.ModelObservable;
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

namespace ClienteMarWPF.UI.Modules.Mensajeria
{
    /// <summary>
    /// Interaction logic for MensajeriaView.xaml
    /// </summary>
    public partial class MensajeriaView : UserControl
    {
        public MensajeriaView()
        {
            InitializeComponent();
        }



        private void AddMensaje(string mensaje)
        {
           // MensajeriaBinding.Add(new MensajesObservable { Mensaje = mensaje, Destinatario = "Javier de Jesus", IsMe=true });
            listMensajeChat.Items.MoveCurrentToLast();
            //listMensajeChat.ScrollIntoView(listMensajeChat.Items.CurrentItem);
            txtChatMensaje.Text = "";
        }


        private void btnChatEnviar(object sender, RoutedEventArgs e)
        {
            string mensaje = txtChatMensaje.Text;
            if (mensaje != string.Empty)
            {
                AddMensaje(mensaje);
            }

        }

        private void txtChatMensaje_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string mensaje = txtChatMensaje.Text;
                if (mensaje != string.Empty)
                {
                    AddMensaje(mensaje);
                }
            }

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
           // listMensajeChat.ScrollIntoView(listMensajeChat.Items.CurrentItem);
        }

    }
}
