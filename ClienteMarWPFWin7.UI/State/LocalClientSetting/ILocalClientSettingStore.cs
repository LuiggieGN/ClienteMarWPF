using ClienteMarWPFWin7.Domain.Models.Dtos;

using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPFWin7.UI.State.LocalClientSetting
{
    public interface ILocalClientSettingStore
    {
        LocalClientSettingDTO LocalClientSettings { get; set; }
        void ReadDektopLocalSetting(bool CanWriteServerFile = false);
        void WriteDesktopLocalSetting(LocalClientSettingDTO setting);
    }
}
