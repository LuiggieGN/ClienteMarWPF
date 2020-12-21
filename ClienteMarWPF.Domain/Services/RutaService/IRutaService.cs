
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ClienteMarWPF.Domain.Models.Base;
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPF.Domain.Models.Entities;

namespace ClienteMarWPF.Domain.Services.RutaService
{
    public interface IRutaService  
    {
        RutaAsignacionDTO LeerGestorAsignacionPendiente(int gestorUsuarioId, int bancaId ); 
    }
}
