﻿
using ClienteMarWPF.Domain.Models.Dtos;

using ClienteMarWPF.UI.Modules.RegistrarPC;

using System;
using System.Windows;

namespace ClienteMarWPF.UI.ViewModels.Commands.RegistrarPC
{
    public class RegistrarCommand : ActionCommand
    {
        private readonly RegistrarPCControlViewModel ViewModel;


        public RegistrarCommand(RegistrarPCControlViewModel viewModel) : base()
        {
            ViewModel = viewModel;
            Action<object> comando = new Action<object>(RegistrarPC);
            base.SetAction(comando);
        }

        private void RegistrarPC(object parametro)
        {
            try
            {
                RegistroPCResultDTO regPC = ViewModel.Ptova.RegistraCambioPC(bancaid: ViewModel.Settings.BancaId, hwkey: ViewModel.Settings.Identidad);

                if (regPC.FueExitoso && Convert.ToDecimal(regPC.CertificadoNumero) > 0) 
                {

                    ViewModel.Settings.Identidad = regPC.CertificadoNumero;
                    ViewModel.LocalSettingReaderAndWriter.WriteDesktopLocalSetting(ViewModel.Settings);
                    ViewModel.RegistroDePCFueExitoso = true;
                    MessageBox.Show("Su computador ha sido registrado exitosamente.", "Registrada!!", MessageBoxButton.OK, MessageBoxImage.Information);
                    ViewModel.CloseAction?.Invoke();
                }
                else
                {
                    ViewModel.RegistroDePCFueExitoso = false;
                    MessageBox.Show("No tiene autorización para registrar esta PC en el sistema. Comuniquese con la central", "Error!!", MessageBoxButton.OK, MessageBoxImage.Warning);

                }
            }
            catch (Exception)
            {
                ViewModel.RegistroDePCFueExitoso = false;
                MessageBox.Show("Hubo un error al realizar la operación", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }








    }
}
