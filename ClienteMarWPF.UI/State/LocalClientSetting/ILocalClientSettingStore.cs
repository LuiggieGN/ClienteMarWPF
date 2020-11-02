using ClienteMarWPF.Domain.Models.Dtos;

using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.UI.State.LocalClientSetting
{
    public interface ILocalClientSettingStore
    {
        LocalClientSettingDTO LocalClientSettings { get; set; }

        void ReadDektopLocalSetting();

        void WriteDesktopLocalSetting(LocalClientSettingDTO setting);
    }
}
