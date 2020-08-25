using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Models.RequestModel
{
   public class SegurosRequestModel
    {

        public class Poliza
        {
            public string PolizaNumero { get; set; }
            public int TransaccionID { get; set; }
            public cliente Cliente { get; set; }
            public vehiculo Vehiculo { get; set; }
            public decimal Precio { get; set; }
            public string CodigoAutorizacion { get; set; }
            public string Vigencia { get; set; }
            public class cliente
            {
                public string Cedula { get; set; }
            }
            public class vehiculo
            { 
                public string Chasis { get; set; }
            }
        }




    }
}
