using ClienteMarWPF.Domain.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.UI.State.Configurators
{
    public class ConfiguratorStore : IConfiguratorStore
    {
        private BancaConfiguracion _currentBancaConfiguracion;   
        public BancaConfiguracion CurrentBancaConfiguracion { 
            get => _currentBancaConfiguracion; 
            set { 
                _currentBancaConfiguracion = value;
                StateChanged?.Invoke();
            }
        }

        public event Action StateChanged;
    }
}
