
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using ClienteMarWPFWin7.Domain.Models.Base;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Models.Entities;

namespace ClienteMarWPFWin7.Domain.Services.BancaService
{
    public interface IBancaService : IServiceBase<BancaDTO, int>
    {
        BancaConfiguracionDTO LeerBancaConfiguraciones(int bancaid);

        decimal LeerBancaMontoReal(int bancaid);

        bool BancaUsaControlEfectivo(int bancaid, bool incluyeConfig);

        decimal LeerDeudaDeBanca(int bancaid);

        int LeerInactividad(int bancaid);

        List<MarOperacionDTO> LeerBancaMarOperacionesDia(int bancaid, string strdia_yyyyMMdd);

        string LeerBancaComandoRemoteCmd(int bancaid);

        bool LeerEstadoBancaEstaActiva(int bancaid);

        decimal LeerVentaDeHoyDeLoterias(int bancaid);

        decimal LeerVentaDeHoyDeProductos(int bancaid);

        bool LeerBancaTicketFueAnulado(string noTicket);

        bool Rel(int bancaid, string hwkey);

        List<TicketDTO> LeerTicketsHoy(int bancaid);
    }
}
