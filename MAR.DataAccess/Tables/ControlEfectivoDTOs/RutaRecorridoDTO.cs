﻿

namespace MAR.DataAccess.Tables.ControlEfectivoDTOs
{
    public class RutaRecorridoDTO
    {
        public BancaEnRecorridoDTO[] Terminales { get; set; }
        public decimal GestorBalanceAlAsignarRuta { get; set; }
        public decimal GestorMontoEntregado { get; set; }

    }
}
