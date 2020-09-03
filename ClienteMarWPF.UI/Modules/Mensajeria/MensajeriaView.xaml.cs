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
        public ObservableCollection<Mensajes> MensajeriaBinding;
        public MensajeriaView()
        {
            InitializeComponent();

            MensajeriaBinding = new ObservableCollection<Mensajes> {
                new Mensajes(){ MensajeID=1, IsSelected=false,  IsRead=true, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new Mensajes(){ MensajeID=2, IsSelected=false,  IsRead=false, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new Mensajes(){ MensajeID=3, IsSelected=false,  IsRead=true, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new Mensajes(){ MensajeID=4, IsSelected=false,  IsRead=true, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new Mensajes(){ MensajeID=5, IsSelected=false,  IsRead=true, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new Mensajes(){ MensajeID=6, IsSelected=false,  IsRead=true, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new Mensajes(){ MensajeID=7, IsSelected=false,  IsRead=true, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new Mensajes(){ MensajeID=8, IsSelected=false,  IsRead=true, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new Mensajes(){ MensajeID=9, IsSelected=false,  IsRead=true, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new Mensajes(){ MensajeID=10, IsSelected=true, IsRead=true, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new Mensajes(){ MensajeID=11, IsSelected=true, IsRead=true, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new Mensajes(){ MensajeID=12, IsSelected=true, IsRead=true, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new Mensajes(){ MensajeID=13, IsSelected=true, IsRead=true, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new Mensajes(){ MensajeID=14, IsSelected=true, IsRead=true, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new Mensajes(){ MensajeID=15, IsSelected=true, IsRead=false, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new Mensajes(){ MensajeID=16, IsSelected=true, IsRead=false, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new Mensajes(){ MensajeID=17, IsSelected=true, IsRead=false, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." }

            };


            // listMensajes.DataContext = MensajeriaBinding.Where(x => x.IsRead == false).ToList();
            listMensajes.DataContext = MensajeriaBinding;
            listBancas.DataContext = MensajeriaBinding;
            listMensajeChat.DataContext = MensajeriaBinding;
        }



        private void AddMensaje(string mensaje)
        {
            MensajeriaBinding.Add(new Mensajes { Mensaje = mensaje, Destinatario = "Javier de Jesus" });
            listMensajeChat.Items.MoveCurrentToLast();
            listMensajeChat.ScrollIntoView(listMensajeChat.Items.CurrentItem);
            txtChatMensaje.Text = "";
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            modalChat.Visibility = Visibility.Collapsed;
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


        private void listView_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as ListBox).SelectedItem;
            if (item != null)
            {
                modalChat.Visibility = Visibility.Visible;
               listMensajeChat.Items.MoveCurrentToLast();
               listMensajeChat.ScrollIntoView(listMensajeChat.Items.CurrentItem);
            }
        }


    }
}
