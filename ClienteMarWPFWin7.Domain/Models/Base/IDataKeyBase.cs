using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPFWin7.Domain.Models.Base
{
    public interface IDataKeyBase<TId>
    {
        TId Id { get; }
    }
}
