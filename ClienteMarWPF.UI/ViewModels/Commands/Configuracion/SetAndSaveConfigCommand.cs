﻿using ClienteMarWPF.UI.Modules.Configuracion;
using ClienteMarWPF.UI.State.LocalClientSetting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

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
                ViewModel.LocalSetting.BancaId = ViewModel.BancaID;
                ViewModel.LocalSetting.Direccion = ViewModel.Direccion;
                ViewModel.LocalSetting.Tickets = ViewModel.Ticket;
                LocalConfig.WriteDesktopLocalSetting(ViewModel.LocalSetting);
                MessageBox.Show("Operaciòn Completada", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Hubo un error al guardar la configuraciòn", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
