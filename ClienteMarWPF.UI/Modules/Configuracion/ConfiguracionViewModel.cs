using ClienteMarWPF.UI.State.LocalClientSetting;
using ClienteMarWPF.UI.ViewModels.Base;
using ClienteMarWPF.UI.ViewModels.Commands.Configuracion;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace ClienteMarWPF.UI.Modules.Configuracion
{
    public class ConfiguracionViewModel: BaseViewModel
    {
        public ICommand SaveConfigCommand { get; }
        public ObservableCollection<int> Tickets { get; set; }
        public ConfiguracionViewModel(ILocalClientSettingStore localClient)
        {
            SaveConfigCommand = new SetAndSaveConfigCommand(this, localClient);
            Tickets = new ObservableCollection<int> { 1, 2, 3 };
            BancaID = localClient.LocalClientSettings.BancaId;
            Direccion = localClient.LocalClientSettings.Direccion;
            Ticket = localClient.LocalClientSettings.Tickets;
        }

        #region PropertyOfView
        //###########################################################
        private int _bancaId;
        private string _direccion;
        private int _ticket;
        //###########################################################
        public int BancaID
        {
            get { return _bancaId; }
            set { _bancaId = value; NotifyPropertyChanged(nameof(BancaID)); }
        }        
        public string Direccion
        {
            get { return _direccion; }
            set { _direccion = value; NotifyPropertyChanged(nameof(Direccion)); }
        }    
        public int Ticket
        {
            get { return _ticket; }
            set { _ticket = value; NotifyPropertyChanged(nameof(Direccion)); }
        }
        //###########################################################
        #endregion

    }
}
