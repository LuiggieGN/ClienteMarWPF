using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flujo.Entities.WebClient.POCO
{
    public class Caja
    {
        public int CajaID { get; set; }
        public int TipoCajaID { get; set; }
        public string CajaDescripcion { get; set; }
        public int? ZonaID { get; set; }
        public int? UsuarioID { get; set; }
        public int? BancaID { get; set; }
        public string Ubicacion { get; set; }
        public decimal BalanceActual { get; set; }
        public decimal BalanceMinimo { get; set; }
        public DateTime FechaBalance { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Disponible { get; set; }
        public bool Activa { get; set; }

        public string BalanceEnFormato {
           get
           {
              return "$ " + ((BalanceActual == 0) ? "0" 
                          : BalanceActual.ToString("#,##0.00", CultureInfo.CreateSpecificCulture("en-US"))
                    
              );
           }
        }

        public static Caja ToVirtual(int UsuarioID)
        {

            return new Caja
            {
                Activa = true,
                TipoCajaID = 2,
                UsuarioID = UsuarioID,
            };

        }

        public string CajaPropietarioNombre { get; set; }
    }
}
