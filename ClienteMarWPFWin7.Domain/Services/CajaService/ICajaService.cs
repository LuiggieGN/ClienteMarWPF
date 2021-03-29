
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ClienteMarWPFWin7.Domain.Models.Base;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPFWin7.Domain.Models.Entities;

namespace ClienteMarWPFWin7.Domain.Services.CajaService
{
    public interface ICajaService  
    {
        SupraMovimientoEnBancaResultDTO RegistrarMovimientoEnBanca(SupraMovimientoEnBancaDTO movimiento);

        SupraMovimientoDesdeHastaResultDTO RegistrarMovimientoDesdeHasta(SupraMovimientoDesdeHastaDTO transferencia);

        MultipleDTO<PagerResumenDTO, List<MovimientoDTO>> LeerMovimientos(MovimientoPageDTO paginaRequest);

        List<MovimientoDTO> LeerMovimientosNoPaginados(MovimientoPageDTO paginaRequest);

        decimal LeerCajaBalance(int cajaid);

        decimal LeerCajaBalanceMinimo(int cajaid);

        CajaDTO LeerCajaDeUsuarioPorUsuarioId(int usuarioid);

        bool SetearCajaDisponibilidad(CajaDisponibilidadDTO disponibilidad);
    }
}
