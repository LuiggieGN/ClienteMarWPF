using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.Domain.Models.Dtos
{

    public class SorteosDTO
    {
        public int LoteriaID { get; set; }
        public string Loteria { get; set; }
    }

    public class CombinationPossibility
    {
        public int Loteria1 { get; set; }
        public int Loteria2 { get; set; }
        public int LoteriaDestino { get; set; }
    }

    public class Jugada
    {
        public string TipoJugada { get; set; }
        public string Jugadas { get; set; }
        public int Monto { get; set; }
    }

    public class SorteosResponse
    {
        public List<int> Apuestas { get; set; }
    }

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

    public class ReponseSorteos
    {
        public bool OK { get; set; }
        public object Respuesta { get; set; }
    }

}
