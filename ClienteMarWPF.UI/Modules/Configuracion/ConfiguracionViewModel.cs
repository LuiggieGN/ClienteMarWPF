using System.Linq;
using System.Windows.Input;
using System.Collections.ObjectModel;
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.UI.State.LocalClientSetting;
using ClienteMarWPF.UI.ViewModels.Base;
using ClienteMarWPF.UI.ViewModels.Commands.Configuracion;

namespace ClienteMarWPF.UI.Modules.Configuracion
{
    public class ConfiguracionViewModel : BaseViewModel
    {
        #region Fields

        private int _bancaId;
        private string _direccion;
        private int _ticket;
        private int _ticketValorArchivo;
        private LocalClientSettingDTO _localsetting;

        #endregion


        #region Properties

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
            set { _ticket = value; NotifyPropertyChanged(nameof(Ticket)); }
        }
        public ObservableCollection<int> Tickets { get; set; }
        public int TicketValorArchivo
        {
            get { return _ticketValorArchivo; }
            set { _ticketValorArchivo = value; NotifyPropertyChanged(nameof(TicketValorArchivo)); }
        }


        public LocalClientSettingDTO LocalSetting => _localsetting;
        #endregion


        #region Commands
        public ICommand SaveConfigCommand { get; }
        #endregion


        public ConfiguracionViewModel(ILocalClientSettingStore localClient)
        {
                       
            _localsetting = new LocalClientSettingDTO();

            try
            {
                localClient.ReadDektopLocalSetting();

                _localsetting.BancaId = localClient.LocalClientSettings.BancaId;
                _localsetting.LF = localClient.LocalClientSettings.LF;
                _localsetting.Direccion = localClient.LocalClientSettings.Direccion;
                _localsetting.Identidad = localClient.LocalClientSettings.Identidad;
                _localsetting.Tickets = localClient.LocalClientSettings.Tickets;
                _localsetting.Espera = localClient.LocalClientSettings.Espera;
                _localsetting.ServerIP = localClient.LocalClientSettings.ServerIP;
            }
            catch 
            {
                _localsetting.BancaId = 0;
                _localsetting.LF = 0;
                _localsetting.Direccion = "0.0.0.0" ;
                _localsetting.Identidad = 0;
                _localsetting.Tickets = 1;
                _localsetting.Espera = 60;
                _localsetting.ServerIP = string.Empty;
            }
 
            BancaID = _localsetting.BancaId;
            Direccion = _localsetting.Direccion;
            Tickets = new ObservableCollection<int> { 1, 2, 3 };
            Ticket = Tickets == null || Tickets.Count() == 0 ? 0 : Tickets.FirstOrDefault();
            TicketValorArchivo = _localsetting.Tickets;



            SaveConfigCommand = new SetAndSaveConfigCommand(this, localClient);
        }










    }
}
