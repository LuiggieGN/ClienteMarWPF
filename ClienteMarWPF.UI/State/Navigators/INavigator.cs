
using System;
using ClienteMarWPF.UI.ViewModels.Base;
 

namespace ClienteMarWPF.UI.State.Navigators
{
    public interface INavigator
    {   
        BaseViewModel CurrentViewModel { get; set; }

        event Action StateChanged;
    }
}
