
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ClienteMarWPF.Domain.Models.Base;
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Entities;

namespace ClienteMarWPF.Domain.Services.BancaService
{
    public interface IBancaService:IServiceBase<Banca,int>
    {
        Task<int> BuscaSuCajaId(int bancaid, FlujoService.MAR_Session sesion);
        Task<decimal> GetBalance(int bancaid, FlujoService.MAR_Session sesion);
    }
}
