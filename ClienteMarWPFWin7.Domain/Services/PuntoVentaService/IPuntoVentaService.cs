
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ClienteMarWPFWin7.Domain.Models.Base;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPFWin7.Domain.Models.Entities;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;

namespace ClienteMarWPFWin7.Domain.Services.PuntoVentaService
{
    public interface IPtoVaService
    {
        InicioPCResultDTO IniciarPC(int bancaid, string bancaip_And_Hwkey); // "ip; hwkey = DB(CerHwKey)"

        RegistroPCResultDTO RegistraCambioPC(int bancaid, string hwkey); // hwkey 

    }
}
