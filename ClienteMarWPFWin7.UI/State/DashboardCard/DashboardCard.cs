using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClienteMarWPFWin7.UI.State.DashboardCard
{
    public class DashboardCard : IDashboardCard
    {
        public string Card_Ventas_Loterias { get; set; }
        public string Card_Ventas_Productos { get; set; }
        public string Card_Comisiones { get; set; }
        public string Card_Anulaciones { get; set; }
        public string Card_Pagos { get; set; }
        public string Card_Descuentos_Productos { get; set; }
        public string Card_Balances { get; set; }
        public DateTime FechaAConsultar { get; set; }
        public DateTime? UltimaFechaDeActualizacion { get; set; }
        public string UltimaFechaDeActualizacionStr => UltimaFechaDeActualizacion.HasValue ? $"Última Actualización : {UltimaFechaDeActualizacion.Value.ToString("hh:mm:ss tt")}" : "";


        public bool IsLoadingForFirstTime { get; set; }


        public DashboardCard()
        {
            IsLoadingForFirstTime = true;

            SetFechaAConsultar(DateTime.Now);

            SetCardsValue(card_ventas_loterias: "*",
                          card_ventas_productos: "*",
                          card_comisiones: "*",
                          card_anulaciones: "*",
                          card_pagos: "*",
                          card_descuentos_productos:"*",
                          card_balances: "*",
                          ultimaActualizacion: null);
        }


        public void SetCardsValue(string card_ventas_loterias,
                                  string card_ventas_productos,
                                  string card_comisiones,
                                  string card_anulaciones,
                                  string card_pagos,
                                  string card_descuentos_productos,
                                  string card_balances,
                                  DateTime? ultimaActualizacion)
        {
            Card_Ventas_Loterias = card_ventas_loterias;
            Card_Ventas_Productos = card_ventas_productos;
            Card_Comisiones = card_comisiones;
            Card_Anulaciones = card_anulaciones;
            Card_Pagos = card_pagos;
            Card_Descuentos_Productos = card_descuentos_productos;
            Card_Balances = card_balances;
            UltimaFechaDeActualizacion = ultimaActualizacion;
        }

        public void SetFechaAConsultar(DateTime fecha)
        {
            FechaAConsultar = fecha;
        }





 
    }
}
