

using ClienteMarWPF.Domain.Enums;
using ClienteMarWPF.Domain.Models.Dtos;

namespace ClienteMarWPF.UI.State.LocalClientSetting
{
    public class LocalClientBase
    {
        protected const string BaseDirectory = @"C:\MAR";

        protected const string FileName = "Mar.ini";

        protected const MarSettingExt FileExtension = MarSettingExt.ini;

        protected const string IniFileKey = "MAR Initialize 2.0";

    }
}
