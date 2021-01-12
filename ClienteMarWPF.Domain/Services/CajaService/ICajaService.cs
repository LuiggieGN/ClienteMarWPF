
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ClienteMarWPF.Domain.Models.Base;
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPF.Domain.Models.Entities;

namespace ClienteMarWPF.Domain.Services.CajaService
{
    public interface ICajaService  
    {
        SupraMovimientoEnBancaResultDTO RegistrarMovimientoEnBanca(SupraMovimientoEnBancaDTO movimiento);

        SupraMovimientoDesdeHastaResultDTO RegistrarMovimientoDesdeHasta(SupraMovimientoDesdeHastaDTO transferencia);

        MultipleDTO<PagerResumenDTO, List<MovimientoDTO>> LeerMovimientos(MovimientoPageDTO paginaRequest);

        decimal LeerCajaBalance(int cajaid);

        decimal LeerCajaBalanceMinimo(int cajaid);

        CajaDTO LeerCajaDeUsuarioPorUsuarioId(int usuarioid);

        bool SetearCajaDisponibilidad(CajaDisponibilidadDTO disponibilidad);
    }
}
