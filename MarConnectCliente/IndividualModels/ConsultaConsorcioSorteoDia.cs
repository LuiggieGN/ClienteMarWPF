using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarConnectCliente.IndividualModels
{
    public class ConsultaConsorcioSorteoDia
    {
        public int      SorteoDiaID { get; set; }
        public int      SorteoID { get; set; }
        public DateTime Dia { get; set; }
        public DateTime HoraInicioVentas { get; set; }
        public DateTime HoraCierreVentas { get; set; }
        public bool     VentasCerradas { get; set; }
        public bool     DiaCerrado { get; set; }
        public DateTime HoraCierreDia { get; set; }
        public string   Estado { get; set; }
        public string   Referencia { get; set; }

        public String     TokenIni { get; set; }

        public string     TokenFin { get; set; }
        public int      CuentaID { get; set; }
        public string     TokenCierreVenta { get; set; }

        public int CantidadTotalOpeExitosasCierreVentas { get; set; }
        public decimal MontoTotalApuestasCierreVentas { get; set; }
        public string DesgloseTotales { get; set; }

    }
}
