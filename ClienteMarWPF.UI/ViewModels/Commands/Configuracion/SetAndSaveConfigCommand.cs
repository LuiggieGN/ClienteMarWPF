using ClienteMarWPF.UI.Modules.Configuracion;
using ClienteMarWPF.UI.State.LocalClientSetting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.UI.ViewModels.Commands.Configuracion
{
    public class SetAndSaveConfigCommand: ActionCommand
    {
        private readonly ConfiguracionViewModel ViewModel;
        private readonly ILocalClientSettingStore LocalConfig;

        public SetAndSaveConfigCommand(ConfiguracionViewModel viewModel, ILocalClientSettingStore localClient) : base()
        {
            ViewModel = viewModel;
            LocalConfig = localClient;

            Action<object> comando = new Action<object>(SaveConfig);
            base.SetAction(comando);
        }

        private void SaveConfig(object parametro)
        {
            try
            {
                var settingDTO = new Domain.Models.Dtos.LocalClientSettingDTO {
                    BancaId = ViewModel.BancaID,
                    Direccion = ViewModel.Direccion,
                    Tickets = ViewModel.Ticket
                };

                LocalConfig.WriteDesktopLocalSetting(settingDTO);
            }
            catch (Exception)
            {

            }

        }
    }
}
