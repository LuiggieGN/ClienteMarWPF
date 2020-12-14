
namespace ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos
{
    public class SupraMovimientoDesdeHastaDTO
    {
        public int CajaOrigenId { get; set; }
        public int TipoCajaOrigen { get; set; }
        public int CajaDestinoId { get; set; }
        public int TipoCajaDestino { get; set; }
        public int UsuarioId { get; set; } = 0; 
        public string Comentario { get; set; } = string.Empty;
        public decimal Monto { get; set; }

    }
}
