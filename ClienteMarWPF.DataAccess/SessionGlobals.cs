using ClienteMarWPF.Domain.Models.Dtos;
using HaciendaService;
using MarPuntoVentaServiceReference;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ClienteMarWPF.Domain.Models.Dtos.SorteosDisponibles;

namespace ClienteMarWPF.DataAccess
{
    public static class SessionGlobals
    {
        public static List<MAR_Loteria2> LoteriasTodas { get; set; }
        public static List<MAR_Loteria2> LoteriasDisponibles { get; set; }
        public static List<MAR_Loteria2> LoteriasYSupersDisponibles { get; set; }
        public static List<SuperPaleDisponible> SuperPaleDisponibles { get; set; }
        public static double SolicitudID { get { return solicitudID; } }

        //PRIVATE VARIABLE INTERNAL CLASS
        private static double solicitudID = 1;
        private static int solicitudIDCount = 1;



        public static void GetLoteriasDisponibles(MAR_Loteria2[] loterias, MAR_HaciendaResponse sorteosdisponibles)
        {
            LoteriasTodas = new List<MAR_Loteria2>();
            LoteriasDisponibles = new List<MAR_Loteria2>();
            LoteriasYSupersDisponibles = new List<MAR_Loteria2>();
            SuperPaleDisponibles = new List<SuperPaleDisponible>();

            try
            {
                var result = JsonConvert.DeserializeObject<ReponseSorteos>(sorteosdisponibles.Respuesta);
                var sorteosDisp = JsonConvert.DeserializeObject<SorteosDisponibles>(result.Respuesta.ToString());

                
                for (int i = 0; i < loterias.Length; i++)
                {
                    var loteria = loterias[i];
                    LoteriasTodas.Add(loteria);

                    foreach (var item in sorteosDisp.LoteriasIDRegular)
                    {
                        if (loteria.Numero == item)
                        {
                            LoteriasDisponibles.Add(loteria);
                        }
                    }

                    foreach (var item in sorteosDisp.LoteriasIDTodas)
                    {
                        if (loteria.Numero == item)
                        {
                            LoteriasYSupersDisponibles.Add(loteria);
                        }
                    }
                }



                SuperPaleDisponibles = sorteosDisp.SuperPaleDisponibles;
            }
            catch (Exception ex)
            {

            }


        }

        public static void GenerateNewSolicitudID(int session, bool incrementar = false)
        {
            string sessionString = session.ToString();

            if (incrementar)
            {
                solicitudID = Convert.ToDouble(sessionString.PadRight(9, '0')) + solicitudIDCount;
                solicitudIDCount += 1;
            }
            else
            {
                solicitudID = Convert.ToDouble(sessionString.PadRight(9, '0'));
            }
        }




    }

}
