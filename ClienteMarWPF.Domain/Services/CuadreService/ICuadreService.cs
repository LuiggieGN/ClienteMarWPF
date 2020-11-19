 
using ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos;

namespace ClienteMarWPF.Domain.Services.BancaService
{
    public interface ICuadreService 
    {



        CuadreRegistroResultDTO AplicarCuadre(CuadreDTO cuadre, bool esUnRetiro);

        CuadreRegistroResultDTO Registrar(CuadreRegistroDTO registro, out string cuadreAImprimir);

 
    }
}
