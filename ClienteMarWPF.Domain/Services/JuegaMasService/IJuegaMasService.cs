
using System.Collections.Generic;
using ClienteMarWPF.Domain.Enums;
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos;
using JuegaMasService;

namespace ClienteMarWPF.Domain.Services.JuegaMasService
{
    public interface IJuegaMasService
    {
        List<object> LeerReporteJuegaMas(MAR_Session sesion);
    }
}
