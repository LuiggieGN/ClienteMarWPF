using System.Linq;
using System.Windows.Input;
using System.Collections.ObjectModel;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.UI.State.LocalClientSetting;
using ClienteMarWPFWin7.UI.ViewModels.Base;
using ClienteMarWPFWin7.UI.ViewModels.Commands.Configuracion;

namespace ClienteMarWPFWin7.UI.Modules.Configuracion
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

        public bool CargandoDesdelogin { get; set; } = false;

        public bool ConfiguracionFueCambiada { get; set; } = false;

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
                _localsetting.BancaId = 1;
                _localsetting.LF = 1;
                _localsetting.Direccion = "0.0.0.0" ;
                _localsetting.Identidad = "0";
                _localsetting.Tickets = 1;
                _localsetting.Espera = 60;
                _localsetting.ServerIP = string.Empty;
            }
 
            BancaID = _localsetting.BancaId;
            Direccion = _localsetting.Direccion;
            Tickets = new ObservableCollection<int> { 1, 2, 3, 4, 5, 6 };
            Ticket = Tickets == null || Tickets.Count() == 0 ? 0 : Tickets.FirstOrDefault();
            TicketValorArchivo = _localsetting.Tickets;



            SaveConfigCommand = new SetAndSaveConfigCommand(this, localClient);
        }










    }
}
