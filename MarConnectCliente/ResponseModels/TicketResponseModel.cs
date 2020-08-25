using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarConnectCliente.ResponseModels
{
    public class TicketResponseModel
    {

        public class TicketDetalle
        {
            public string Sorteo { get; set; }
            public string Codigo { get; set; }
            public string SorteoID { get; set; }
            public string SorteoNumero { get; set; }
            public string SorteoFecha { get; set; }
            public double Saco { get; set; }
            public string Jugada { get; set; }
            public double Aposto { get; set; }
            public string FechaPago { get; set; }
            public int JugadaEstado { get; set; }
         }
    }
}
