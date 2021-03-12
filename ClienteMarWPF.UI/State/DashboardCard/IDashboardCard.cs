using System;

namespace ClienteMarWPF.UI.State.DashboardCard
{
    public interface IDashboardCard
    {
        public string Card_Ventas_Loterias { get; set; }
        public string Card_Ventas_Productos { get; set; }
        public string Card_Comisiones { get; set; }
        public string Card_Anulaciones { get; set; }
        public string Card_Balances { get; set; }

        public DateTime FechaAConsultar { get; set; }

        public DateTime? UltimaFechaDeActualizacion { get; set; }
        public string UltimaFechaDeActualizacionStr { get; }


        public bool IsLoadingForFirstTime { get; set; }


        public void SetCardsValue(string card_ventas_loterias,
                                  string card_ventas_productos,
                                  string card_comisiones,
                                  string card_anulaciones,
                                  string card_balances,
                                  DateTime? ultimaActualizacion);


        public void SetFechaAConsultar(DateTime fecha);


    }
}
