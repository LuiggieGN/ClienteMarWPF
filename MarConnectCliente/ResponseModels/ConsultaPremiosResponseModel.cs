using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarConnectCliente.ResponseModels
{
    public class ConsultaPremiosResponseModel : BaseResponseModel
    {
        public List<ConsultaPremios> Premios { get; set; }


    }
    public class ConsultaPremios
    {
        public int SorteoID { get; set; }
  
        public string Numeros { get; set; }

    }
}