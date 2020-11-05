using System;

namespace ClienteMarWPF.Domain.Enums
{ 
    public enum Modulos
    {
        Home = 0,
        Login = 1,
        Modulo = 2,
        Reporte = 3,
        Sorteos = 4,
        CincoMinutos = 5,
        Recargas = 6,
        Mensajeria = 7,
        PagoServicios = 8,
        Configuracion = 9,
        InicioControlEfectivo = 100        
    }

    public enum MarRoutingFunctions
    {

    }

    public enum ControlEfectivoFunciones
    {
        CajaController_Create_NewRecord = 1000


    }
 



    public enum MarSettingExt 
    {
      ini =0,
      txt = 1,
      json = 2
    }


}


//GetBancaCajaId = 16,
//GetCajaBalanceActual = 30,