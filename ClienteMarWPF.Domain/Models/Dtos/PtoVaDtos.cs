using System;
using System.Collections.Generic;
using System.Text;
using MarPuntoVentaServiceReference;
using ClienteMarWPF.Domain.Enums;

namespace ClienteMarWPF.Domain.Models.Dtos
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
