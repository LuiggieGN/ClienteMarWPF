
namespace MAR.DataAccess.Tables.ControlEfectivoDTOs
{
    public class SupraMovimientoEnBancaDTO
    {
        public int CajaId { get; set; }
        public int BancaId { get; set; }
        public int UsuarioId { get; set; } = 0;
        public decimal Monto { get; set; }
        public string Comentario { get; set; } = string.Empty;
        public int KeyIE { get; set; }

    }
}
