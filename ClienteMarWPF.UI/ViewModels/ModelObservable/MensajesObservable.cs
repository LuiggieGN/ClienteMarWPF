using ClienteMarWPF.UI.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.UI.ViewModels.ModelObservable
{
    public class MensajesObservable : BaseViewModel
    {

        private int _mensajeID;
        private string _asunto;
        private string _remitente;
        private string _destinatario { get; set; }
        private string _mensaje { get; set; }
        private bool _isSelected { get; set; }
        private bool _isRead { get; set; }

        public int MensajeID
        {
            get { return _mensajeID; }
            set { _mensajeID = value; NotifyPropertyChanged(nameof(MensajeID)); }
        }

        public string Asunto
        {
            get { return _asunto; }
            set { _asunto = value; NotifyPropertyChanged(nameof(Asunto)); }
        }

        public string Remitente
        {
            get { return _remitente; }
            set { _remitente = value; NotifyPropertyChanged(nameof(Remitente)); }
        }

        public string Destinatario
        {
            get { return _destinatario; }
            set { _destinatario = value; NotifyPropertyChanged(nameof(Destinatario)); }
        }

        public string Mensaje
        {
            get { return _mensaje; }
            set { _mensaje = value; NotifyPropertyChanged(nameof(Mensaje)); }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; NotifyPropertyChanged(nameof(IsSelected)); }
        }

        public bool IsRead
        {
            get { return _isRead; }
            set { _isRead = value; NotifyPropertyChanged(nameof(IsRead)); }
        }


    }
}
