using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Flujo.Entities.WebClient.POCO
{
    [Serializable()]
    public class BancaBalanceActualRecord
    {
        public int RowRank { get; set; }
        public int BancaID { get; set; }
        public int CajaID { get; set; }
        public string BanNombre { get; set; }
        public decimal BalanceActual { get; set; }
        public decimal BalanceMinimo { get; set; }

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


        public string CategoriaClase
        {
            get
            {
                return BalanceActual == 0 ? "Indefinido" : (BalanceActual > 0 ? "Ingreso" : "Egreso");

            }
        }


        public decimal BancaPorPagar { get; set; }


        public string BancaPorPagarEnFormato
        {
            get
            {
                return "$ " + (
                               (BancaPorPagar == 0)
                            ? "0"
                            : BancaPorPagar.ToString("#,##0.00", CultureInfo.CreateSpecificCulture("en-US"))
                              );
            }
        }


        // == @@Banca por Recojer
        public decimal BancaPorCobrar { get; set; }


        public string BancaPorCobrarEnFormato
        {
            get
            {
                return "$ " + (
                               (BancaPorCobrar == 0)
                            ? "0"
                            : BancaPorCobrar.ToString("#,##0.00", CultureInfo.CreateSpecificCulture("en-US"))
                              );
            }
        }


        public decimal BancaPorLlevar { get; set; }


        public string BancaPorLlevarEnFormato
        {
            get
            {
                return "$ " + (
                               (BancaPorLlevar == 0)
                            ? "0"
                            : BancaPorLlevar.ToString("#,##0.00", CultureInfo.CreateSpecificCulture("en-US"))
                              );
            }
        }


    }
}