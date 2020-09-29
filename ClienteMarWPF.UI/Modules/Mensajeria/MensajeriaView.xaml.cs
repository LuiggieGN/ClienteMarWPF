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
        public ObservableCollection<MensajesObservable> MensajeriaBinding;
        public MensajeriaView()
        {
            InitializeComponent();

            MensajeriaBinding = new ObservableCollection<MensajesObservable> {
                new MensajesObservable(){ MensajeID=1, IsSelected=false,  IsRead=true, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new MensajesObservable(){ MensajeID=2, IsSelected=false,  IsRead=false, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new MensajesObservable(){ MensajeID=3, IsSelected=false,  IsRead=true, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new MensajesObservable(){ MensajeID=4, IsSelected=false,  IsRead=true, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new MensajesObservable(){ MensajeID=5, IsSelected=false,  IsRead=true, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new MensajesObservable(){ MensajeID=6, IsSelected=false,  IsRead=true, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new MensajesObservable(){ MensajeID=7, IsSelected=false,  IsRead=true, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new MensajesObservable(){ MensajeID=8, IsSelected=false,  IsRead=true, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new MensajesObservable(){ MensajeID=9, IsSelected=false,  IsRead=true, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new MensajesObservable(){ MensajeID=10, IsSelected=true, IsRead=true, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new MensajesObservable(){ MensajeID=11, IsSelected=true, IsRead=true, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new MensajesObservable(){ MensajeID=12, IsSelected=true, IsRead=true, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new MensajesObservable(){ MensajeID=13, IsSelected=true, IsRead=true, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new MensajesObservable(){ MensajeID=14, IsSelected=true, IsRead=true, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new MensajesObservable(){ MensajeID=15, IsSelected=true, IsRead=false, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new MensajesObservable(){ MensajeID=16, IsSelected=true, IsRead=false, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." },
                new MensajesObservable(){ MensajeID=17, IsSelected=true, IsRead=false, Asunto="Aprovecha nuestras ofertas", Remitente="Javier De Jesus", Destinatario = "Mario Hernandez", Mensaje="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s." }

            };



            listMensajeChat.DataContext = MensajeriaBinding;
        }



        private void AddMensaje(string mensaje)
        {
            MensajeriaBinding.Add(new MensajesObservable { Mensaje = mensaje, Destinatario = "Javier de Jesus" });
            listMensajeChat.Items.MoveCurrentToLast();
            listMensajeChat.ScrollIntoView(listMensajeChat.Items.CurrentItem);
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
            listMensajeChat.ScrollIntoView(listMensajeChat.Items.CurrentItem);
        }

    }
}
