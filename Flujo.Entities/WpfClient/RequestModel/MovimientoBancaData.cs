using System;
using Flujo.Entities.WpfClient.Enums;

namespace Flujo.Entities.WpfClient.RequestModel
{
    public class MovimientoBancaData
    {
        public int       BancaID                { get; set; }
        public TipoFlujo TipoFlujo              { get; set; }
        public int       TipoFlujoTipoID   { get; set; }
        public decimal   Monto                   { get; set; }
        public DateTime  MovimientoFecha { get; set; }
        public int       CajaID                   { get; set; }
        public int       MovimientoID       { get; set; }
        public string    Descripcion            { get; set; }
        public string    Aux_RefTransaccion { get; set; } = "";


    }
}
