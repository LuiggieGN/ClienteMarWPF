
using ClienteMarWPFWin7.Domain.Enums;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos;

namespace ClienteMarWPFWin7.Domain.Services.MultipleService
{
    public interface IMultipleService
    {
        MultipleDTO<MUsuarioDTO, CajaDTO, TarjetaDTO> LeerUsuarioSuCajaYSuTarjetaPorPinDeUsuario(string pin);
    }
}


