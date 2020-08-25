using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Flujo.Entities.WebClient.POCO
{
    [Serializable()]
    public class MensajeroBalanceActualRecord
    {
        public int     RowRank { get; set; }
        public string  Documento { get; set; }
        public string  TipoDocumento { get; set; }
        public string  Mensajero { get; set; }

        public decimal BalanceMinimo { get; set; }
        public decimal BalanceActual { get; set; }

        public string BalanceMinimoEnFormato
        {
            get
            {
                return "$ " + (
                               (BalanceMinimo == 0)
                            ? "0"
                            : BalanceMinimo.ToString("#,##0.00", CultureInfo.CreateSpecificCulture("en-US"))
                              );
            }
        }

        public string BalanceActualEnFormato
        {
            get
            {
                return "$ " + (
                               (BalanceActual == 0)
                            ? "0"
                            : BalanceActual.ToString("#,##0.00", CultureInfo.CreateSpecificCulture("en-US"))
                              );
            }
        }


        public string CategoriaClase
        {
            get
            {
                return BalanceActual == 0 ? "Indefinido" : (BalanceActual > 0 ? "Ingreso" : "Egreso");

            }
        }





    }
}
