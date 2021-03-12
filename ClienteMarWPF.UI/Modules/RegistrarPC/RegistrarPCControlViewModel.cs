
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Services.PuntoVentaService;

using ClienteMarWPF.UI.State.LocalClientSetting;
using ClienteMarWPF.UI.ViewModels.Base;
using ClienteMarWPF.UI.ViewModels.Commands;
using ClienteMarWPF.UI.ViewModels.Commands.RegistrarPC;

using System;
using System.Windows.Input;


namespace ClienteMarWPF.UI.Modules.RegistrarPC
{
    public class RegistrarPCControlViewModel : BaseViewModel
    {

        public Action CloseAction { get; set; } 

        
        public LocalClientSettingDTO Settings { get; }
        public ILocalClientSettingStore LocalSettingReaderAndWriter { get; }
        public IPtoVaService Ptova { get; }
        public bool RegistroDePCFueExitoso { get; set; } = false;



        public ICommand RegistrarCommand { get; }
        public ICommand CancelarCommand { get; }


        public RegistrarPCControlViewModel(ILocalClientSettingStore localSettingReaderAndWriter, IPtoVaService ptova,  LocalClientSettingDTO settings)
        {
            Settings = settings;

            LocalSettingReaderAndWriter = localSettingReaderAndWriter;
            
            Ptova = ptova;

            RegistrarCommand = new RegistrarCommand(this);

            CancelarCommand = new ActionCommand(new Action<object>(
                (object param) =>
                {
                    RegistroDePCFueExitoso = false;
                    CloseAction?.Invoke();
                }
            ));
        }










    }
}
