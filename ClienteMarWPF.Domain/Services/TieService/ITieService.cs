
using ClienteMarWPF.Domain.Enums;
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos;

namespace ClienteMarWPF.Domain.Services.TieService
{
    public interface ITieService
    {
        TieDTO LeerTiposAnonimos();
    }
}
