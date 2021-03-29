using System;
using System.Globalization;

namespace ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos
{
    public class SupraMovimientoDesdeHastaResultDTO
    {
        public bool FueProcesado { get; set; }
        public int? OrigenMovId { get; set; }
        public int? DestinoMovId { get; set; }
        public string RefOrigen { get; set; }
        public string RefDestino { get; set; }
        public string Error { get; set; }
        public DateTime FechaTransferencia { get; set; }

        public string FechaTransferencia_dd_MMM_yyyy_hh_mm_tt
        {
            get
            {
                return FechaTransferencia.ToString("dd MMMM yyyy hh:mm tt", new CultureInfo("es-ES"));
            }
        }




    }
}
