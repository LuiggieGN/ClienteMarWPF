using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flujo.Entities.WpfClient.ResponseModel
{
    public class BaseCincoMinutosResponseModel
    {
       
        public bool OK { get; set; }
        public string Error { get; set; }
        public string Mensaje { get; set; }
        public List<string[]> PrintData { get; set; }


    }
}
