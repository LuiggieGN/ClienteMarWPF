using System;
using System.Collections.Generic;
using System.Globalization;

namespace Flujo.Entities.WebClient.POCO
{
    [Serializable()]
    public class ConsultaBalanceCaja
    {
        public string BancaID { get; set; }
        public int CajaID { get; set; }
        public string CategoriaTipo { get; set; }
        public string BancaNombreOTipoUsuarioNombre { get; set; }
        public decimal Balance { get; set; }
        public string BalanceEnFormato
        {
            get
            {
                return "$ " + ((Balance == 0) ? "0"
                            : Balance.ToString("#,##0.00", CultureInfo.CreateSpecificCulture("en-US"))
                              );
            }
        }

        public string BalanceEnFormatoClase
        {
            get
            {
                return Balance == 0 ? "Indefinido" : Balance > 0 ? "Ingreso" : "Egreso";
                //  EntradaOSalida == 0 ? "Indefinido" : Categoria;
            }
        }

        public DateTime FechaUltimaActualizacion { get; set; }

        public string Estado { get; set; }
    }
}