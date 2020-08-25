using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Flujo.Entities.WebClient.POCO
{
    public class EstadoConsorcio
    {
        public decimal BancasBalanceDisponibleAcumulados { get; set; }
        public decimal ConsorcioPendientePorLlevar { get; set; }
        public decimal ConsorcioPorRecoger { get; set; }
        public decimal AcumuladoBalanceDisponibleMensajero { get; set; }


        public string EnFormatoBancasBalanceDisponibleAcumulados
        {
            get
            {
                return BancasBalanceDisponibleAcumulados.ToString("#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
            }
        }

        public string EnFormatoConsorcioPendientePorLlevar
        {
            get
            {
                return ConsorcioPendientePorLlevar.ToString("#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
            }
        }

        public string EnFormatoConsorcioPorRecoger
        {
            get
            {
                return ConsorcioPorRecoger.ToString("#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
            }
        }

        public string EnFormatoAcumuladoBalanceDisponibleMensajero
        {
            get
            {
                return AcumuladoBalanceDisponibleMensajero.ToString("#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
            }
        }



    }
}