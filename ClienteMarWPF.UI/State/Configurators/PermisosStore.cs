using ClienteMarWPF.Domain.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.UI.State.Configurators
{
    public class PermisosStore : IPermisosStore
    {
        PermisosDTO _permisos;
        public PermisosDTO Permisos
        {
            get => _permisos;
            set {
                _permisos = value;
                StateChanged?.Invoke();
            }
        }

        public event Action StateChanged;
    }
}
