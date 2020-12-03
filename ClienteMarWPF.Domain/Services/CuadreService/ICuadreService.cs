
using ClienteMarWPF.Domain.Enums;
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos;

namespace ClienteMarWPF.Domain.Services.CuadreService
{
    public interface ICuadreService
    {
        CuadreRegistroResultDTO Registrar(BancaDTO banca, CuadreRegistroDTO ope, bool enablePrinting ,out string toPrint);

    }
}


