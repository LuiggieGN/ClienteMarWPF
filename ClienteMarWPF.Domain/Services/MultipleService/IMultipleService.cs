
using ClienteMarWPF.Domain.Enums;
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos;

namespace ClienteMarWPF.Domain.Services.MultipleService
{
    public interface IMultipleService
    {
        MultipleDTO<MUsuarioDTO, CajaDTO, TarjetaDTO> LeerUsuarioSuCajaYSuTarjetaPorPinDeUsuario(string pin);
    }
}


