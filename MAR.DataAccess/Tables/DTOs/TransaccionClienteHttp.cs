using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.DataAccess.Tables.DTOs
{
    public partial class TransaccionClienteHttp
    {
        [Key]
        public int TransaccionID { get; set; }
        public int BancaID { get; set; }
        public int TipoTransaccionID { get; set; }
        public DateTime Fecha { get; set; }
        public string Referencia { get; set; }
        public decimal Monto { get; set; }
        public string Autorizacion { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaRespuesta { get; set; }
        public string Estado { get; set; }
        public bool Activo { get; set; }
        public int TipoAutorizacion { get; set; }
        public string Peticion { get; set; }
        public string Respuesta { get; set; }
        public string NautCalculado { get; set; }
        public string Comentario { get; set; }
    }
}
