using ClienteMarWPFWin7.Domain.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClienteMarWPFWin7.Domain.Models.Dtos.ProdutosDTO;

namespace ClienteMarWPFWin7.Domain.Services.CincoMinutosService
{
    public interface ICincoMinutosServices
    {
        ProductoViewModel SetProducto(string producto, CuentaDTO cuenta);
        ProductoViewModelResponse GetProductosDisponibles(CuentaDTO cuenta);
    }
}
