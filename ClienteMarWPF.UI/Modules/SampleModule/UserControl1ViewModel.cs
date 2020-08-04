using ClienteMarWPF.UI.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

using ClienteMarWPF.UI.Extensions;


namespace ClienteMarWPF.UI.Modules.SampleModule
{
    public class UserControl1ViewModel : BaseViewModel
    {
        private int valorA;
        private int valorB;
        private int valorC;

        private List<string> CarrosToyota = new List<string>() { "Toyota A" };
        private List<string> CarrosHyundai = new List<string>() { "Hyundai A" };
        private List<string> Carros = new List<string>();

        private List<int> SeleccionA = new List<int>() { 6 };
        private List<int> SeleccionB = new List<int>() { 10, 20};
        private List<int> SeleccionC = new List<int>();

        public int ValorA { 
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
                valorB =  value;
                NotifyPropertyChanged();
            }
        }

        public int ValorC
        {
            get
            {
                return valorC;
            }
            set
            {
                Carros.AgregaListadoCadaElemento(CarrosToyota);
                Carros.AgregaListadoCadaElemento(CarrosHyundai);


                SeleccionC.AgregaListadoCadaElemento(SeleccionA);  
                SeleccionC.AgregaListadoCadaElemento(SeleccionB); 


                valorA = 10;
                valorB = 20;
                valorC = valorA + valorB;
                NotifyPropertyChanged(nameof(ValorA), nameof(ValorB));
            }
        }







    }
}
