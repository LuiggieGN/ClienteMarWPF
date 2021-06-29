namespace ClienteMarWPFWin7.UI.ViewModels.ModelObservable
{
    public class ChatMensajeObservable
    {         
        public int BancaID { get; set; }
        public int MensajeID { get; set; }
        public string Tipo { get; set; }
        public string Asunto { get; set; }
        public string Contenido { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }  
        public string Origen { get; set; }  
        public string Destino { get; set; } 
        public bool Leido { get; set; }
        public int SinLeerTotal { get; set; }
        public bool EsMiMensaje { get; set; }
    }
}
