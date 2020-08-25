using System;
using System.Globalization;

namespace Flujo.Entities.WebClient.POCO
{
    public class BancaBalance
    {
        public int         CajaID    { get; set; }
        public int         BancaID { get; set; }
        public string    Banca     { get; set; }
        public decimal BalanceActual { get; set; }
        public string    BalanceEnFormato
        {
            get
            {
                return "$ " + (
                               (BalanceActual == 0)
                            ?  "0" 
                            :   BalanceActual.ToString("#,##0.00", CultureInfo.CreateSpecificCulture("en-US"))
                              );
            }
        }

        public string  Estado
        {
            get
            {
                return Activa ? "Activa" : "Inactiva";
            }
        }

        public bool Activa { get; set; }

    }
}
