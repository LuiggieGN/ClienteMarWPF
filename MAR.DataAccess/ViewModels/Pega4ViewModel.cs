using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.DataAccess.ViewModels
{
    public class Pega4ViewModel
    {
        public class PagoConsultado
        {
   
            public bool Aprobado { get; set; }
            public decimal Saco { get; set; }
            public string Mensaje { get; set; }
            public string Referencia { get; set; }
        }
    }
}
