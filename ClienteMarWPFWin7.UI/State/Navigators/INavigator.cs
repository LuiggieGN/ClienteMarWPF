
using System;
using ClienteMarWPFWin7.UI.ViewModels.Base;
 

namespace ClienteMarWPFWin7.UI.State.Navigators
{
    public interface INavigator
    {   
        BaseViewModel CurrentViewModel { get; set; }

        event Action StateChanged;
    }
}
