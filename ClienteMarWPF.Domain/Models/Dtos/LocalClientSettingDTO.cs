 

namespace ClienteMarWPF.Domain.Models.Dtos
{
    public class LocalClientSettingDTO
    {
        public int BancaId { get; set; }
        public int LF { get; set; }
        public string Direccion { get; set; }
        public string Identidad { get; set; }
        public int Tickets { get; set; }
        public int Espera { get; set; }
        public string ServerIP { get; set; }
    }
}
