using FlujoCustomControl.FlujoServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlujoCustomControl.Views;

namespace FlujoCustomControl.Code.BussinessLogic
{
    public class WebServiceLogic
    {
        public static   Action<int, string, Action<FlujoServices.MAR_FlujoResponse>, Action<FlujoServices.MAR_FlujoResponse>> ServiceMethod;
        private static int SolicitudID = 0;
        public static LocalBL lcvr = new LocalBL();
        private static FlujoServices.mar_flujoSoapClient flujoSvr = lcvr.GetFlujoServiceClient(false);
        public static FlujoServices.MAR_Session MarSession = new FlujoServices.MAR_Session();


        internal void Flujo_ServiceCallBegin(int pServiceMetodo, string JSONString,
            Action<FlujoServices.MAR_FlujoResponse> onSuccess = null,
            Action<FlujoServices.MAR_FlujoResponse> onFail = null)
        {
            SolicitudID = (SolicitudID + 1);
            FlujoServices.ArrayOfAnyType parametros = new FlujoServices.ArrayOfAnyType();
            parametros.Add(JSONString);

            try
            {
                Action<FlujoServices.MAR_FlujoResponse>[] arr = new Action<MAR_FlujoResponse>[] { onSuccess, onFail };

                flujoSvr.BeginCallFlujoIndexFunction(pServiceMetodo, MainFlujoWindows.MarSession, parametros,
                    Flujo_ServiceCallEnd, arr);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Flujo_ServiceCallEnd(IAsyncResult pResult)
        {

            if (pResult.IsCompleted)
            {
                Action<FlujoServices.MAR_FlujoResponse>[] callBacks = (Action<MAR_FlujoResponse>[])pResult.AsyncState;
                try
                {

                    var response = flujoSvr.EndCallFlujoIndexFunction(pResult);
                    if (response != null && response.OK)
                    {
                        if (callBacks != null && callBacks.Length > 0 && callBacks[0] != null) //success
                        {
                            Action<FlujoServices.MAR_FlujoResponse> arraryAction = (Action<FlujoServices.MAR_FlujoResponse>)(callBacks[0]);
                            arraryAction(response);
                        }
                        else if (callBacks != null && callBacks.Length > 1 && callBacks[1] != null)//fail
                        {
                            Action<FlujoServices.MAR_FlujoResponse> arraryAction = (Action<FlujoServices.MAR_FlujoResponse>)(callBacks[1]);
                            arraryAction(response);
                        }
                    }
                }
                catch (Exception ex)
                {
                    var t = ex;

                }
            }
        }


    }
}
