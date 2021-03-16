
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ClienteMarWPF.Domain.Models.Base;
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Entities;

namespace ClienteMarWPF.Domain.Services.BancaService
{
    public interface IBancaService:IServiceBase<BancaDTO,int>
    {
        BancaConfiguracionDTO LeerBancaConfiguraciones(int bancaid);

        decimal LeerBancaMontoReal(int bancaid);

        bool BancaUsaControlEfectivo(int bancaid, bool incluyeConfig);
 
        decimal LeerDeudaDeBanca(int bancaid);

        int LeerInactividad(int bancaid);


    }
}
