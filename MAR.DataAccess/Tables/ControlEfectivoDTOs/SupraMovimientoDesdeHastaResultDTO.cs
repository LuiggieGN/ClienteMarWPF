﻿using System;


namespace MAR.DataAccess.Tables.ControlEfectivoDTOs
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

    }
}
