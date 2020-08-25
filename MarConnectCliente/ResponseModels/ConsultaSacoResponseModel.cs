using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarConnectCliente.ResponseModels
{
    public class ConsultaSacoResponseModel : BaseResponseModel
    {
        public List<ConsultaSaco> Consulta { get; set; }


    }
    public class ConsultaSaco
    {
        public int TerminalID { get; set; }
        public double Saco { get; set; }
        public string Referencia { get; set; }
        public int MARBancaID { get; set; }
        public string Autorizacion { get; set; }
        public DateTime? FechaPago { get; set; }

    }
}