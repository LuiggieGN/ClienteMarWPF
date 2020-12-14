
using System;

namespace MAR.DataAccess.Tables.ControlEfectivoDTOs
{
    public class PagerResumenDTO
    {
        public int PaginaNo { get; set; }
        public int PaginaSize { get; set; }
        public int TotalPaginas { get; set; }
        public int TotalRecords { get; set; }
        public int PaginaRecords { get; set; }
        public bool OrdenAsc { get; set; }
        public string OrdenColumna { get; set; }
    }
}


