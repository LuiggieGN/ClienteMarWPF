using System;
 

namespace ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos
{
    public class MovimientoDTO
    {
        public int CajaID { get; set; }
        public string Categoria { get; set; }
        public int CategoriaSubTipoID { get; set; }
        public string CategoriaConcepto { get; set; }
        public int Orden { get; set; }
        public long MovimientoID { get; set; }
        public DateTime Fecha { get; set; }
        public string Referencia { get; set; }
        public string Descripcion { get; set; }
        public decimal EntradaOSalida { get; set; }
        public decimal Balance { get; set; } 
    }
}
