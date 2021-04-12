using System;

namespace ClienteMarWPFWin7.UI.State.DashboardCard
{
    public interface IDashboardCard
    {
        string Card_Ventas_Loterias { get; set; }
        string Card_Ventas_Productos { get; set; }
        string Card_Comisiones { get; set; }
        string Card_Anulaciones { get; set; }
        string Card_Pagos { get; set; }
        string Card_Descuentos_Productos { get; set; }
        string Card_Balances { get; set; }

        DateTime FechaAConsultar { get; set; }


        DateTime? UltimaFechaDeActualizacion { get; set; }
        string UltimaFechaDeActualizacionStr { get; }


        bool IsLoadingForFirstTime { get; set; }


        void SetCardsValue(string card_ventas_loterias,
                           string card_ventas_productos,
                           string card_comisiones,
                           string card_anulaciones, 
                           string card_pagos,
                           string card_descuentos_productos,
                           string card_balances,
                           DateTime? ultimaActualizacion);


        void SetFechaAConsultar(DateTime fecha);


    }
}
