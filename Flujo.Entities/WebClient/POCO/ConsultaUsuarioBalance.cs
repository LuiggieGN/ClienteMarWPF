using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flujo.Entities.WebClient.POCO
{
    public class ConsultaUsuarioBalance
    {
        public int CajaID { get; set; }
        public int         Posicion { get; set; } // UsuarioID
        public string    Nombre { get; set; }
        public string    Documento { get; set; }
        public decimal BalanceActual { get; set; }
        public string    BalanceActualEnFormato  { get { return "$" + BalanceActual.ToString("#,##0.00", CultureInfo.CreateSpecificCulture("en-US")); }  }
        public bool      Activo { get; set; }
        public string   StrActivo { get { return (Activo ? "Activo" : "Inactivo");  } }

    }
}
