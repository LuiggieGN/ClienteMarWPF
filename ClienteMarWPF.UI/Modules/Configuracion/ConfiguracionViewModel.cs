using ClienteMarWPF.UI.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

using ClienteMarWPF.UI.Extensions;
using System.Windows.Input;
using ClienteMarWPF.UI.ViewModels.Commands;

namespace ClienteMarWPF.UI.Modules.Configuracion
{
    public class ConfiguracionViewModel: BaseViewModel
    {
        private int valorA;
        private int valorB;
        private int valorC;

        public int ValorA
        {
            get
            {
                return valorA;
            }
            set
            {
                valorA = value;
                NotifyPropertyChanged();
            }
        }

        public int ValorB
        {
            get
            {
                return valorB;
            }
            set
            {
                valorB = value;
                NotifyPropertyChanged();
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ICommand SaveCommand
        {
            ///  Agrego mi codigo referente al comando 
            ///  
            get{

                return new ActionCommand(Action => Save(this));
            }
        }



        private void Save(ConfiguracionViewModel config) 
        {
         
        
        }




 
    }
}
