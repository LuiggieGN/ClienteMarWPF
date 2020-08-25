using MAR.DataAccess.Tables.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.DataAccess.ViewModels.Mappers
{
    public class JuegaMasMappers
    {
        public static ReportesViewModels.ReporteJuegaMasCliente[] MapFromTransaccionToReporteJuegaMas(IEnumerable<object> pTransacciones)
        {
            if (pTransacciones is IEnumerable<VP_Transaccion>)
            {
                var transacciones = (IEnumerable<VP_Transaccion>)pTransacciones;
                return (from p in transacciones
                    select new ReportesViewModels.ReporteJuegaMasCliente
                    {
                        Monto = p.Ingresos.ToString("N2"),
                        Serial = p.Referencia,
                    }).ToArray();
            }
            else
            {
                var transacciones = (IEnumerable<VP_HTransaccion>)pTransacciones;
                return (from p in transacciones
                    select new ReportesViewModels.ReporteJuegaMasCliente
                    {
                        Monto = p.Ingresos.ToString("N2"),
                        Serial = p.Referencia
                    }).ToArray();
            }
        }
    }
}
