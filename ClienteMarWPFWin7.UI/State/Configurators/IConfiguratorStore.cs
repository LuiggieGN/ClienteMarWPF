using System;
using System.Collections.Generic;
using System.Text;

using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Models.Entities;

namespace ClienteMarWPFWin7.UI.State.Configurators
{
    public interface IConfiguratorStore
    {
        BancaConfiguracionDTO CurrentBancaConfiguracion { get; set; }
        event Action StateChanged;
    }
}
