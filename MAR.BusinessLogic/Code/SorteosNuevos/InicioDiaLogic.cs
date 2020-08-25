using FluentScheduler;
using MAR.BusinessLogic.Code.Hacienda.SharedOperations;
using MarConnectCliente;
using MarConnectCliente.BusinessLogic;
using MarConnectCliente.BusinessLogic.ShareFuntions;
using MarConnectCliente.Enums;
using MarConnectCliente.RequestModels;
using MarConnectCliente.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Code.SorteosNuevos
{
    public class InicioDiaLogic
    {
        public static bool IniciaDia(int pBancaID)
        {
            try
            {
                var diaFueIniciado = DataAccess.EFRepositories.Hacienda.InicioDiaRepository.ValidaInicioDia();
                //FORZANDO EL DIA FUE INICIADO PARA PROBAR 
                diaFueIniciado = true;
                return diaFueIniciado;
                //Inicializa el JOB para los tickets Fuera de lina
               //  JobManager.Initialize(new Scheduller.MyRegistry());
                if (!diaFueIniciado)
                {
                

                    // inicializa transaccion ClienteHttp
                    var transaccion = new DataAccess.Tables.DTOs.TransaccionClienteHttp()
                    {
                        BancaID = pBancaID,
                        Activo = false,
                        Autorizacion = "",
                        Comentario = "MAR, Solicitud de Inicio de dia",
                        Estado = "Solicitud",
                        Referencia = "InicioDiaMar",
                        TipoTransaccionID = (int)MarConnectCliente.Enums.MetodosEnum.MetodoServicio.Inicio,
                        Fecha = DateTime.Now,
                        FechaSolicitud = DateTime.Now,
                        TipoAutorizacion = 1,
                        FechaRespuesta = null,
                        NautCalculado = ""
                    };

                    //Agrega TransaccionClienteHttp
                    var transaccionIniciada = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.AgregaTransaccion(transaccion);

                    //Busca cuenta y producto
                    var cuenta = DataAccess.EFRepositories.PMCuentasRepository.GetPMCuenta(100000, "SorteosNuevos", pBancaID,1);

                    //Crea base parameters
                    var baseParams = BaseRequestLogic.CreaBaseRequest(cuenta, transaccionIniciada.TransaccionID.ToString());
                    InicioDiaRequestModel pIniciaDiaModel = new InicioDiaRequestModel();

                    var p = pIniciaDiaModel;
                    p.EstablecimientoID = cuenta.PMCuenta.CueComercio;
                    p.Usuario = cuenta.PMCuenta.RecargaID;
                    p.Password = cuenta.PMCuenta.CueServidor;
                    p.CodigoOperacion = transaccionIniciada.TransaccionID.ToString();
                    p.DiaOperacion = baseParams.DiaOperacion;
                    p.FechaHoraSolicitud = baseParams.FechaHoraSolicitud;
                    p.ServiceUrl = new Uri(cuenta.SWProducto.URL);
                    p.CodigoOperacion = baseParams.CodigoOperacion;
                    var autenticacion = new AuthenticationHeaderValue("Basic", BaseRequestLogic.Base64Encode($"{p.Usuario}:{p.Password}"));

                    //Consume servicio
                    var iniciaDia = ClienteHTTP.CallService<InicioDiaReponseModel, InicioDiaRequestModel>(MetodosEnum.MetodoServicio.Inicio, p, MetodosEnum.HttpMethod.POST, autenticacion);

                    //Actualiza TransaccionClienteHttp

                    if (iniciaDia != null)
                    {
                        if (iniciaDia.CodigoRespuesta == "100" || iniciaDia.CodigoRespuesta == "900")
                        {
                            transaccion.Activo = true;
                        }
                        transaccion.Respuesta = iniciaDia.ToString();
                        transaccion.FechaRespuesta = SharedFunctionsLogic.ConvertirATipoFechaFECHASOLICITUD(iniciaDia.FechaHoraRespuesta);
                        transaccion.Autorizacion = iniciaDia.CodigoValidacionOffline;
                        transaccion.Estado = iniciaDia.Estado;
                        transaccion.Peticion = p.ToString();
                        transaccion.Comentario = iniciaDia.MensajeRespuesta;
                        transaccion.TransaccionID = transaccionIniciada.TransaccionID;
                        var transaccionActulizada = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.ActualizaTransaccion(transaccion);

                        //Agrega transaccionRefencia 
                        //*********************revisa esto para los parametros. el detallee***
                        DataAccess.EFRepositories.Hacienda.TransaccionReferenciaRepository.AgregaTransaccion(transaccion.TransaccionID, MetodosEnum.MetodoServicio.Inicio, iniciaDia.ToString());
                        if (iniciaDia.CodigoRespuesta == "100" || iniciaDia.CodigoRespuesta == "900")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        transaccion.Respuesta = "Fallo la coneccion al servicio MAR";
                        transaccion.FechaRespuesta = DateTime.Now;
                        transaccion.Peticion = p.ToString();
                        transaccion.TransaccionID = transaccionIniciada.TransaccionID;
                        var transaccionActulizada = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.ActualizaTransaccion(transaccion);
                        return false;
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }


        }

    }
}
