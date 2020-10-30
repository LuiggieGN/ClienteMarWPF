using System;
using System.Collections.Generic;
using System.Text;

using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Entities;

namespace ClienteMarWPF.UI.State.Accounts
{
    public interface IAccountStore
    {
        CuentaDTO CurrentAccount { get; set; }
        event Action StateChanged;
    }
}
