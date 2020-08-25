using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.DataAccess.ViewModels
{
    public class ReportesViewModels
    {
        public class SumaVentasTransacciones
        {
            public double Venta { get; set; }
            public double Comision { get; set; }
        }

        public class ReportePagosServicios
        {
            public string Producto { get; set; }
            public string Monto { get; set; }
            public string Suplidor { get; set; }
        }
        public class ReporteJuegaMasCliente
        {
            public string Monto { get; set; }
            public string Serial { get; set; }
        }
        public class ReportePremios
        {
            public string Loteria { get; set; }
            public string Primera { get; set; }
            public string Segunda { get; set; }
            public string Tercera { get; set; }
        }
    }
}
