using ClienteMarWPF.Domain.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.UI.State.Configurators
{
    public class ConfiguratorStore : IConfiguratorStore
    {
        private BancaConfiguracionDTO _currentBancaConfiguracion;   
        public BancaConfiguracionDTO CurrentBancaConfiguracion { 
            get => _currentBancaConfiguracion; 
            set { 
                _currentBancaConfiguracion = value;
                StateChanged?.Invoke();
            }
        }

        public event Action StateChanged;
    }
}
