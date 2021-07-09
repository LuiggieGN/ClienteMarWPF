using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMarWPFWin7.Domain.Models.Dtos
{
    public class BaseCincoMinutosDTO
    {
        public bool OK { get; set; }
        public string Error { get; set; }
        public string Mensaje { get; set; }
        public List<string[]> PrintData { get; set; }
    }
}
