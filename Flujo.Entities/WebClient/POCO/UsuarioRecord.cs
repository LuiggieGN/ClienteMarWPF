using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Flujo.Entities.WebClient.POCO
{
    [Serializable()]
    public class UsuarioRecord
    {
        public int UsuarioID { get; set; }
        public string UsuarioRol { get; set; }
        public int TipoUsuarioID { get; set; }
        public int TipoDocumentoID { get; set; }
        public string Nombre { get; set; }
        public string DocTipo { get; set; }
        public string Documento { get; set; }
        public int ZonaID { get; set; }
        public int Activo { get; set; }
        public int ToquenFallidos { get; set; }
        public int LoginFallidos { get; set; }


        public decimal CajaBalanceMinimo { get; set; }
        public string CajaBalanceMinimoEnFormato
        {
            get
            {
                string formato = "$" + (
                                            (CajaBalanceMinimo == 0)
                                          ? "0.00"
                                          : CajaBalanceMinimo.ToString("#,##0.00", CultureInfo.CreateSpecificCulture("en-US"))
                                        );

                return formato;
            }
        }


        public bool SePuedeEditar { get; set; }
        public bool SePuedeEliminar { get; set; }

        public bool EnableMessageCantMakeAction
        {
            get
            {
                bool show = (SePuedeEditar == false && SePuedeEliminar == false) ? true : false;

                return show;

            }
        }

    }
}