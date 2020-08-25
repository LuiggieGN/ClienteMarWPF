using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MAR.DataAccess.Tables.DTOs;

namespace MAR.DataAccess.ViewModels.Mappers
{
    public class PagosServiciosMapper
    {
        public static ReportesViewModels.ReportePagosServicios[] MapFromTransaccionToReportePagosServicioses(IEnumerable<object> pTransacciones)
        {

            if (pTransacciones is IEnumerable<VP_Transaccion>)
            {
                var transacciones = (IEnumerable<VP_Transaccion>)pTransacciones;
                return (from p in transacciones
                    select new ReportesViewModels.ReportePagosServicios
                    {
                        Monto = p.Ingresos.ToString("N2"),
                        Suplidor = p.VP_Suplidor.Nombre,
                        Producto = p.VP_Producto.Nombre
                    }).ToArray();
            }
            else
            {
                var transacciones = (IEnumerable<VP_HTransaccion>)pTransacciones;
                return (from p in transacciones
                    select new ReportesViewModels.ReportePagosServicios
                    {
                        Monto = p.Ingresos.ToString("N2"),
                        Suplidor = p.VP_Suplidor.Nombre,
                        Producto = p.VP_Producto.Nombre
                    }).ToArray();
            }
        }

    }
}

