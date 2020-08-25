using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.DataAccess.Views
{
    public class MiEjemploRecord:BaseViewRecord
    {
        public int Campo1 { get; set; }
        public string Campo2 { get; set; }
        public string Campo3 { get; set; }
        public int? Campo4 { get; set; }  //el ? representa una columna NULLABLE en la base de datos
    }
}
