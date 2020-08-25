

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using MAR.AppLogic.MARHelpers;

using ClienteMarWPF.Domain.Enums;
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Entities;
using ClienteMarWPF.Domain.Services.BancaService;


using ClienteMarWPF.DataAccess.Services.Helpers;

namespace ClienteMarWPF.DataAccess.Services
{
    public class BancaDataService : IBancaService
    {
        public static LocalBL lcvr;
        private static FlujoServices.mar_flujoSoapClient flujoSvr;

        static BancaDataService()
        {
            lcvr = new LocalBL();
            flujoSvr = lcvr.GetFlujoServiceClient(false);
        }


        public Task<bool> AddRange(List<Banca> entities)
        {
            throw new NotImplementedException();
        }

        public Task<Banca> Create(Banca entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Banca entity)
        {
            throw new NotImplementedException();
        }

        public Task<Banca> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Banca>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<int> BuscaSuCajaId(int bancaid, FlujoServices.MAR_Session sesion)
        {
            try
            {
                string bancaidStr = JSONHelper.SerializeToJSON(bancaid);

                FlujoServices.ArrayOfAnyType colleccionParametros = new FlujoServices.ArrayOfAnyType();
                colleccionParametros.Add(bancaidStr);

                FlujoServices.MAR_FlujoResponse servicioRespuesta = await Task.Run(() =>
                {
                    return
                           flujoSvr.CallFlujoIndexFunctionAsync(
                               (int)FlujoEfectivoRoutingFunctions.GetBancaCajaId,
                                sesion,
                                colleccionParametros
                            ).Result.Body.CallFlujoIndexFunctionResult;
                });


                if (servicioRespuesta != null && servicioRespuesta.OK == true)
                {
                    return JSONHelper.CreateNewFromJSONNullValueIgnore<int>(servicioRespuesta.Respuesta);
                }
                else { return -1; }
            }
            catch 
            {
                return -1;
            }

        }// fin de metodo BuscaSuCajaId()



        public async Task<decimal> GetBalance(int bancaid, FlujoServices.MAR_Session sesion)
        {
            try
            {
                int cajaid = await BuscaSuCajaId(bancaid, sesion);

                if (cajaid == -1)
                {
                    return 0;
                }

                string cajaidStr = JSONHelper.SerializeToJSON(cajaid);
                FlujoServices.ArrayOfAnyType colleccionParametros = new FlujoServices.ArrayOfAnyType();
                colleccionParametros.Add(cajaidStr);

                FlujoServices.MAR_FlujoResponse servicioRespuesta = await Task.Run(() =>
                {
                    return
                           flujoSvr.CallFlujoIndexFunctionAsync(
                               (int)FlujoEfectivoRoutingFunctions.GetCajaBalanceActual,
                                sesion,
                                colleccionParametros
                            ).Result.Body.CallFlujoIndexFunctionResult;
                });

                if (servicioRespuesta != null && servicioRespuesta.OK == true)
                {
                    return JSONHelper.CreateNewFromJSONNullValueIgnore<decimal>(servicioRespuesta.Respuesta);
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }// fin de metodo GetBalance()

        public Task<bool> Update(Banca entity)
        {
            throw new NotImplementedException();
        }
    }
}
