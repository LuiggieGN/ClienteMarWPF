
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ClienteMarWPFWin7.Domain.Models.Base;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPFWin7.Domain.Models.Entities;

namespace ClienteMarWPFWin7.Domain.Services.RutaService
{
    public interface IRutaService  
    {
        RutaAsignacionDTO LeerGestorAsignacionPendiente(int gestorUsuarioId, int bancaId ); 
    }
}
