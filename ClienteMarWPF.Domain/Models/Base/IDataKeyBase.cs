using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.Domain.Models.Base
{
    public interface IDataKeyBase<TId>
    {
        TId Id { get; }
    }
}
