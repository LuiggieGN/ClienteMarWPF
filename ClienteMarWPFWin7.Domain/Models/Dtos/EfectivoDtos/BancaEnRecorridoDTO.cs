

namespace ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos
{
    public class BancaEnRecorridoDTO
    {
        public int BancaID { get; set; }
        public string Terminal { get; set; }
        public int Orden { get; set; }
        public string Direccion { get; set; }
        public bool FueRecorrida { get; set; }
        public bool IncluirEnRecorrido { get; set; }
        public int DepositarORetirar { get; set; }
        public decimal MontoRuta { get; set; }
        public decimal BalanceAlCuadreGestor { get; set; }

    }
}
