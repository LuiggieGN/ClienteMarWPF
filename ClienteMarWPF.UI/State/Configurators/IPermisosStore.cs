using System;
using System.Collections.Generic;
using System.Text;

using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Entities;

namespace ClienteMarWPF.UI.State.Configurators
{
    public interface IPermisosStore
    {
        PermisosDTO Permisos { get; set; }
        event Action StateChanged;
    }
}
