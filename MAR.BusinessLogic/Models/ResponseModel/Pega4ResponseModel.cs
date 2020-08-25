using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Models.ResponseModel
{
    public class Pega4ResponseModel
    {
        public class TicketViewModel
        {
            public string TicNumero { get; set; }
            public string TicFecha { get; set; }
            public string Hora { get; set; }
            public int TicketID { get; set; }
            public int Sorteo { get; set; }
            public decimal TicCosto { get; set; }
            public decimal TicPago { get; set; }
            public bool TicNulo { get; set; }
            public string Pin { get; set; }
            public string SorteoNombre { get; set; }
            public string Firma { get; set; }
            public string Estado { get; set; }
            public string NumeroSorteo { get; set; }
            public string FechaSorteo { get; set; }
            public string HoraSorteo { get; set; }
            public string Serial { get; set; }
            public bool Activo { get; set; }
            public JugadaPega4[] Jugadas { get; set; }
            public class JugadaPega4
            {
                public double Cantidad { get; set; }
                public string Jugada { get; set; }
                public decimal Monto { get; set; }
                public string Sorteo { get; set; }
                public string Referencia { get; set; }
                public string TipoJugada { get; set; }
                public int SorteoTipoJugadaID { get; set; }
            }
        }
    }
}
