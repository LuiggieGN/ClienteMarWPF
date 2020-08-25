using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Flujo.Entities.WpfClient.POCO
{
    public class MovimientoInsertEstado
    {
        public int MovimientoID { get; set; }
        public int? IngresoID { get; set; }
        public int? EgresoID { get; set; }
        public DateTime MovFecha { get; set; }
        public DateTime IngresoFecha { get; set; }
        public DateTime EgresoFecha { get; set; }
        public bool FueProcesado { get; set; }
        public string MensajeError { get; set; }
        public string Referencia { get; set; }


        public string MovFechaConFormato_dd_MMM_yyyy_hh_mm_tt
        {
            get
            {
                return MovFecha.ToString("dd MMMM yyyy hh:mm tt", new CultureInfo("es-ES"));
            }
        }
        public string IngresoFechaConFormato_dd_MMM_yyyy_hh_mm_tt
        {
            get
            {
                return IngresoFecha.ToString("dd MMMM yyyy hh:mm tt", new CultureInfo("es-ES"));
            }
        }
        public string EgresoFechaConFormato_dd_MMM_yyyy_hh_mm_tt
        {
            get
            {
                return EgresoFecha.ToString("dd MMMM yyyy hh:mm tt", new CultureInfo("es-ES"));
            }
        }
    }
}