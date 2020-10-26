using System;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using FlujoCustomControl.ViewModels.Commands;

using Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories;
using FlujoCustomControl.Views;
using Newtonsoft.Json;
using FlujoCustomControl.Code.BussinessLogic;

namespace FlujoCustomControl.ViewModels.CommonViews
{

    public class Pager<T> : CommonBase where T : class
    {

        public static LocalBL lcvr = new LocalBL();
        private static FlujoServices.mar_flujoSoapClient flujoSvr = lcvr.GetFlujoServiceClient(false);
        #region Campos

        private string sourceQuery;
        private Dictionary<string,object> sourceQueryParameters;

        private ObservableCollection<T> coleccionElementos;
        private int start;
        private int itemCount;
        private string sortColumn;
        private bool ascending;
        private int totalItems = 0;

        private ICommand firstCommand;
        private ICommand previousCommand;
        private ICommand nextCommand;
        private ICommand lastCommand;

        #endregion

        public Pager(Dictionary<string,object> parameters, string query, string pSortColumnName, int pPageSize)
        {
            this.start = 0;
            this.itemCount = pPageSize;
            this.sortColumn = pSortColumnName;
            this.ascending = true;
            this.totalItems = 0;
            this.sourceQuery = query;
            this.sourceQueryParameters = parameters;

            RefrescarConsultaDeElementos();
        }

        public  void ResetPager(int pStart,  int pPageSize, string pDefaultSortColumn,  bool pIsAscending,  int pTotalItems,  string pSourceQuery, Dictionary<string, object> pSourceQueryParameters)
        {
            this.start = pStart;
            this.itemCount = pPageSize;
            this.sortColumn = pDefaultSortColumn;
            this.ascending = pIsAscending;
            this.totalItems = pTotalItems;
            this.sourceQuery = pSourceQuery;
            this.sourceQueryParameters = pSourceQueryParameters;

            RefrescarConsultaDeElementos();      

        }


        public ObservableCollection<T> ColeccionElementos
        {
            get
            {
                return coleccionElementos;
            }
           private set
            {
                if (object.ReferenceEquals(coleccionElementos, value) != true)
                {
                    coleccionElementos = value;
                    OnPropertyChanged("ColeccionElementos");
                }
            }
        }
        public int Start { get { return start + 1; } }
        public int End { get { return start + itemCount < totalItems ? start + itemCount : totalItems; } }
        public int TotalItems { get { return totalItems; } }
        
        /// <summary>
        ///    Comando para moverce al primer elemento
        /// </summary>
        public ICommand FirstCommand
        {
            get
            {
                if (firstCommand == null)
                {
                    firstCommand = new RelayCommand (

                        param =>
                        {
                            start = 0;  RefrescarConsultaDeElementos();
                        },
                        param =>
                        {
                            return start - itemCount >= 0 ? true : false;
                        }
                    );
                }
                return firstCommand;
            }
        }

        /// <summary>
        ///       Comando para moverse a la pagina anterior
        /// </summary>
        public ICommand PreviousCommand
        {
            get
            {
                if (previousCommand == null)
                {
                    previousCommand = new RelayCommand (
                        param =>
                        {
                            start -= itemCount; RefrescarConsultaDeElementos();
                        },
                        param =>
                        {
                            return start - itemCount >= 0 ? true : false;
                        }
                    );
                }
                return previousCommand;
            }
        }
        
        /// <summary>
        ///  Comando para moverse a la siguiente pagina 
        /// </summary>
        public ICommand NextCommand
        {
            get
            {
                if (nextCommand == null)
                {
                    nextCommand = new RelayCommand
                    (
                        param =>
                        {
                            start += itemCount;  RefrescarConsultaDeElementos();   
                        },
                        param =>
                        {
                            return start + itemCount < totalItems ? true : false;
                        }
                    );
                }
                return nextCommand;
            }
        }

        /// <summary>
        /// Se mueve a la ultima pagina de la consulta
        /// </summary>
        public ICommand LastCommand
        {
            get
            {
                if (lastCommand == null)
                {
                    lastCommand = new RelayCommand
                    (
                        param =>
                        {
                            start = (totalItems / itemCount - 1) * itemCount;
                            start += totalItems % itemCount == 0 ? 0 : itemCount; RefrescarConsultaDeElementos();
                        },
                        param =>
                        {
                            return start + itemCount < totalItems ? true : false;
                        }
                    );
                }
                return lastCommand;
            }
        }

        private void RefrescarConsultaDeElementos() 
        {

            //Dim startIndex = Integer.Parse(pParams(0).ToString())
            //Dim itemCount = Integer.Parse(pParams(1).ToString())
            //Dim sortColumn = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON(Of String)(pParams(3).ToString())
            //Dim ascending = Boolean.Parse(pParams(4).ToString())
            //Dim sourceQuery = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON(Of String)(pParams(5).ToString())
            //Dim sourceQueryParams = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON(Of Dictionary(Of String, Object))(pParams(6).ToString())


            var SstartIndex= JsonConvert.SerializeObject(this.start);
            var  itemCount  = JsonConvert.SerializeObject(this.itemCount);
            var  sortColumn  = JsonConvert.SerializeObject(this.sortColumn);
            var  ascending  = JsonConvert.SerializeObject(this.ascending);
            var  sourceQuery = JsonConvert.SerializeObject(this.sourceQuery);
            var sourceQueryParams = JsonConvert.SerializeObject(this.sourceQueryParameters);


            FlujoServices.ArrayOfAnyType parametros = new FlujoServices.ArrayOfAnyType();
            parametros.Add(SstartIndex);
            parametros.Add(itemCount);
            parametros.Add(sortColumn);
            parametros.Add(ascending);
            parametros.Add(sourceQuery);
            parametros.Add(sourceQueryParams);
            parametros.Add(JsonConvert.SerializeObject((int)this.sourceQueryParameters.ElementAt(0).Value));
            parametros.Add(JsonConvert.SerializeObject((string)this.sourceQueryParameters.ElementAt(1).Value));
            parametros.Add(JsonConvert.SerializeObject((string)this.sourceQueryParameters.ElementAt(2).Value));
            var pResponse = flujoSvr.CallFlujoIndexFunction(23, MainFlujoWindows.MarSession, parametros);

            PagerRepositorio.PagingResult<T> result = JsonConvert.DeserializeObject<PagerRepositorio.PagingResult<T>>(pResponse.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            this.totalItems = result.TotalPages;
            this.ColeccionElementos = result.Listado;
            OnPropertyChanged("Start");
            OnPropertyChanged("End");
            OnPropertyChanged("TotalItems");
        }





    }
}
