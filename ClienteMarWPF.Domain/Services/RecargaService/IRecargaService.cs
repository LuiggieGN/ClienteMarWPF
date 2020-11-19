using MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.Domain.Services.RecargaService
{
    public interface IRecargaService
    {
         MAR_Suplidor[] GetSuplidor(MAR_Session _Session);

    }
}
