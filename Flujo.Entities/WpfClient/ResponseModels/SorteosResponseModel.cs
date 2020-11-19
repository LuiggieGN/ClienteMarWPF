using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Flujo.Entities.WpfClient.ResponseModels
{
    public class SorteosResponseModel
    {
        public class SorteosDisponibles
        {
            public SorteosDisponibles()
            {
                LoteriasIDRegular = new List<int>();
                LoteriasIDTodas = new List<int>();
                SuperPaleDisponibles = new List<SuperPaleDisponible>();
            }
            public List<int> LoteriasIDRegular { get; set; }
            public List<int> LoteriasIDTodas { get; set; }
            public List<SuperPaleDisponible> SuperPaleDisponibles { get; set; }
            public class SuperPaleDisponible
            {
                public int LoteriaID1 { get; set; }
                public int LoteriaID2 { get; set; }
                public int LoteriaIDDestino { get; set; }
            }
        }
    }
}











