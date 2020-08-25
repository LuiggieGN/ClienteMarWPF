using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Flujo.Entities.WebClient.POCO
{
    public class SobranteFaltanteRecord
    {
        public int CuadreID { get; set; }
        public int CajaID { get; set; }
        public int BancaID { get; set; }
        public string BanContacto { get; set; }
        public int RiferoID { get; set; }
        public string RifNombre { get; set; }
        public string CajeraResponsable { get; set; }
        public string TipoDeArqueo { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }

        public string MontoEnFormato
        {
            get
            {
                return "$ " + ((Monto == 0) ? "0"
                            : Monto.ToString("#,##0.00", CultureInfo.CreateSpecificCulture("en-US"))
                              );
            }
        }

        public string FechaEnFormato__FechaParte
        {
            get
            {
                return Fecha.ToString("dd MMM yyyy", CultureInfo.CreateSpecificCulture("es"));
            }
        }

        public string FechaEnFormato__TiempoParte
        {
            get
            {
                var t = Fecha.ToString("hh:mm tt", CultureInfo.CreateSpecificCulture("es"));

                return t;
            }
        }


        public string FYear
        {
            get
            {
                return Fecha.ToString("yyyy", CultureInfo.CreateSpecificCulture("es"));
            }

        }

        public string FMes
        {
            get
            {
                return Fecha.ToString("dd MMM", CultureInfo.CreateSpecificCulture("es")).ToUpper();
            }
        }

        public string FTiempo
        {
            get
            {
                var t = Fecha.ToString("hh:mm tt", CultureInfo.CreateSpecificCulture("es"));
                return t;
            }
        }

    }
}