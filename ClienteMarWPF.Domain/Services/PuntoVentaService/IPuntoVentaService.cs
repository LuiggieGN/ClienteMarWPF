
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ClienteMarWPF.Domain.Models.Base;
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPF.Domain.Models.Entities;

using MarPuntoVentaServiceReference;

namespace ClienteMarWPF.Domain.Services.PuntoVentaService
{
    public interface IPtoVaService
    {
        public InicioPCResultDTO IniciarPC(int bancaid, string bancaip_And_Hwkey); // "ip; hwkey = DB(CerHwKey)"

        public RegistroPCResultDTO RegistraCambioPC(int bancaid, string hwkey); // hwkey 


    }
}
