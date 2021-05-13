using ClienteMarWPFWin7.UI.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMarWPFWin7.UI.Modules.EnLinea
{
    public class EnLineaViewModel : BaseViewModel
    {
        private string url;

        public string Url
        {
            get => url;
            set
            {
                url = value;
                NotifyPropertyChanged(nameof(Url));
            }
        }
        public EnLineaViewModel()
        {
            url = "https://www.google.com/";
        }
        
    }
}
