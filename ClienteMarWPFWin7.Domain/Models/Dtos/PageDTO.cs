namespace ClienteMarWPFWin7.Domain.Models.Dtos
{
    public class PageDTO  
    {
        public int PaginaNo { get; set; }
        public int PaginaSize { get; set; }
        public bool OrdenAsc { get; set; }
        public string OrdenColumna { get; set; }
    }
}


 