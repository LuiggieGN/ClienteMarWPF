
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

        bool RegistrarMovimientoDesdeHasta();

        void ConsultarCajaMovimientos();

        decimal LeerCajaBalance(int cajaid);

        CajaDTO LeerCajaDeUsuarioPorUsuarioId(int usuarioid);



    }
}
