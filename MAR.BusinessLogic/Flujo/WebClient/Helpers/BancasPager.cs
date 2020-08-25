
using System;
using System.Linq;
using System.Collections.Generic;
using Flujo.Entities.WebClient.POCO;
using MAR.BusinessLogic.Flujo.WebClient.Helpers.Base;

using Flujo.DataAccess.FlujoRepositories.WebClient.Helpers;
 

namespace MAR.BusinessLogic.Flujo.WebClient.Helpers
{
    [Serializable()]
    public class BancasPager: Pager<BancaRuta>
    {
        public BancasPager():base(  SelectView.SelectBancasActivas  )
        {
            this.BancasModificadas
                 =(base.ColeccionRecords != null &&  base.ColeccionRecords.Count > 0)
                 ?  base.ColeccionRecords.Select(x => (BancaRuta)x.Clone()).ToList()
                 :  new List<BancaRuta>();            
        }

        public List<BancaRuta>  BancasModificadas
        {
            get;
            set;
        }

        public void MergeBancaSegmento(List<BancaRuta> bancas)
        {
            if (BancasModificadas.Count > 0)
            {
                foreach (var banca in  bancas)
                {
                    int index = BancasModificadas.FindIndex(y => y.Posicion == banca.Posicion);

                    if (index != -1)
                    {
                        BancaRuta b = BancasModificadas[index];

                        b.BancaSeleccionada = banca.BancaSeleccionada;
                    }                    
                }
            }
        }

        public void MergeBancasModificaciones()
        {
            if (BancasModificadas.Count > 0)
            {
                foreach (var banca in BancasModificadas)
                {
                    int index = base.ColeccionRecords.FindIndex(y => y.Posicion == banca.Posicion);

                    if (index != -1)
                    {
                        BancaRuta b = ColeccionRecords[index];

                        b.BancaSeleccionada = banca.BancaSeleccionada;
                    }
                }
            }
        }

        public void RestablishBancasModificadas()
        {
            this.BancasModificadas.Clear();

            this.BancasModificadas
                 = (base.ColeccionRecords != null && base.ColeccionRecords.Count > 0)
                 ? base.ColeccionRecords.Select(x => (BancaRuta)x.Clone()).ToList()
                 : new List<BancaRuta>();

        }

        public override List<BancaRuta> GetPaginacion(int start, int pageSize)
        {
            base.Start = start;

            base.PageSize = pageSize;

            if (this.BancasModificadas == null) return new List<BancaRuta>();

            List<BancaRuta> ColeccionSubRecords = this.BancasModificadas.Skip((start - 1) * pageSize).Take(pageSize).ToList();

            return ColeccionSubRecords;
        }

    }
}
