
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Services.PuntoVentaService;

using ClienteMarWPFWin7.UI.State.LocalClientSetting;
using ClienteMarWPFWin7.UI.ViewModels.Base;
using ClienteMarWPFWin7.UI.ViewModels.Commands;
using ClienteMarWPFWin7.UI.ViewModels.Commands.RegistrarPC;

using System;
using System.Windows.Input;


namespace ClienteMarWPFWin7.UI.Modules.RegistrarPC
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
