using ClienteMarWPFWin7.Domain.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPFWin7.UI.State.Configurators
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
