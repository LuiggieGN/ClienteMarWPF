using System;
using System.Collections.Generic;
using System.Text;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using ClienteMarWPFWin7.Domain.Enums;

namespace ClienteMarWPFWin7.Domain.Models.Dtos
{
    public class InicioPCResultDTO 
    {
        public MAR_Array InicioPCResponse { get; set; }
        
        public ServicioMarConexion SevidorConexion { get; set; }

        public bool EstaPCTienePermisoDeConexionAServicioDeMAR { get; set; } = false;

    }

    public class RegistroPCResultDTO
    {
        public bool FueExitoso { get; set; }
        public string Mensaje { get; set; }
        public string CertificadoNumero { get; set; }
    }



}
