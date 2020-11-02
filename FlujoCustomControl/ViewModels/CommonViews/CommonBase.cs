using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FlujoCustomControl.ViewModels.CommonViews
{
   public  class CommonBase: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        ///     Este metodo detecta cuando alguna propiedad del ViewModel cambia 
        public void OnPropertyChanged(string prop)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(prop));
            }
        }

    }
}
