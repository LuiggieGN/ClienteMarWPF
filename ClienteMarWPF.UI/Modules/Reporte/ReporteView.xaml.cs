using ClienteMarWPF.UI.ViewModels.ModelObservable;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes; 

namespace ClienteMarWPF.UI.Modules.Reporte
{
    /// <summary>
    /// Interaction logic for ReporteView.xaml
    /// </summary>
    public partial class ReporteView : UserControl
    {
        private List<SorteosObservable> SorteosBinding;
        public ReporteView()
        {
            InitializeComponent();

            SorteosBinding = new List<SorteosObservable> {
                new SorteosObservable(){ LoteriaID=1, IsSelected=false, IsSuper = false, Loteria="La Fecha Dia", Image = "WbSunny" },
                new SorteosObservable(){ LoteriaID=2, IsSelected=false, IsSuper = false, Loteria="La Fecha Noche", Image = "Brightness3" },
                new SorteosObservable(){ LoteriaID=3, IsSelected=false, IsSuper = false, Loteria="Loteka Dia", Image = "WbSunny" },
                new SorteosObservable(){ LoteriaID=4, IsSelected=false, IsSuper = false, Loteria="Loteka Noche", Image = "Brightness3" },
                new SorteosObservable(){ LoteriaID=5, IsSelected=false, IsSuper = false, Loteria="Nacional Dia", Image = "WbSunny" },
                new SorteosObservable(){ LoteriaID=6, IsSelected=false, IsSuper = false, Loteria="Nacional Noche", Image = "Brightness3" },
                new SorteosObservable(){ LoteriaID=7, IsSelected=false, IsSuper = false, Loteria="New York Dia", Image = "WbSunny" },
                new SorteosObservable(){ LoteriaID=8, IsSelected=false, IsSuper = false, Loteria="New York Noche", Image = "Brightness3" },
                new SorteosObservable(){ LoteriaID=9, IsSelected=false, IsSuper = false, Loteria="Pega 3 Dia", Image = "WbSunny" },
                new SorteosObservable(){ LoteriaID=10, IsSelected=false, IsSuper = false, Loteria="Pega 3 Noche", Image = "Brightness3" }

            };
            listSorteo.DataContext = SorteosBinding;
        }

 
    }
}
