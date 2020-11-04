
using ClienteMarWPF.UI.ViewModels.Commands.Modulo;

using ClienteMarWPF.UI.ViewModels;
using ClienteMarWPF.UI.ViewModels.Base;


using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ClienteMarWPF.Domain.Services.BancaService;
using ClienteMarWPF.UI.ViewModels.Commands;
using System.Threading.Tasks;

namespace ClienteMarWPF.UI.Modules.Modulo
{
    public class ModuloViewModel:BaseViewModel
    {

        private string _balanceConsultado;
        public string BalanceConsultado
        {
            get
            {
                return _balanceConsultado;
            }
            set
            {
                _balanceConsultado = value;
                NotifyPropertyChanged(nameof(BalanceConsultado));
            }
        }

        private string _bancaMensaje;
        public string BancaMensaje
        {
            get
            {
                return _bancaMensaje;
            }
            set
            {
                _bancaMensaje = value;
                NotifyPropertyChanged(nameof(BancaMensaje));
               }
        }


        public ICommand BancaBalanceConsultaCommand { get; }
        public ICommand OtroBancaIdCommand { get; }


        public int BancaId { get; set; }


        public ModuloViewModel(IBancaService bancaService)
        {
            this.BancaId = 7;
            this.BalanceConsultado = "Balance  ... ";   
              
              
            this.BancaBalanceConsultaCommand = new BancaBalanceConsultaCommand(this, bancaService, null);
            this.OtroBancaIdCommand = new GeneraOtroBancaIdCommand(this);
        }


 




    }
}
