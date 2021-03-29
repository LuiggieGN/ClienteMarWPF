using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPFWin7.Domain.Models.Dtos
{
   public static class RecargaDTO
    {
        public static int OperadorID { get; set; }
        public static string Operador { get; set; }
        public static string Pais { get; set; }
        public static string Url { get; set; }
        public static  bool IsSelected { get; set; }
    }
}
