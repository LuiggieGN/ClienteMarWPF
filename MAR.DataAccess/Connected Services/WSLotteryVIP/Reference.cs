﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MAR.DataAccess.WSLotteryVIP {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="RespuestaAuth", Namespace="http://schemas.datacontract.org/2004/07/")]
    [System.SerializableAttribute()]
    public partial class RespuestaAuth : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AutorizacionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private MAR.DataAccess.WSLotteryVIP.Carta CartaBingoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CodRespField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescRespField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NumeroTicketPozoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ReferenciaField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Autorizacion {
            get {
                return this.AutorizacionField;
            }
            set {
                if ((object.ReferenceEquals(this.AutorizacionField, value) != true)) {
                    this.AutorizacionField = value;
                    this.RaisePropertyChanged("Autorizacion");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public MAR.DataAccess.WSLotteryVIP.Carta CartaBingo {
            get {
                return this.CartaBingoField;
            }
            set {
                if ((object.ReferenceEquals(this.CartaBingoField, value) != true)) {
                    this.CartaBingoField = value;
                    this.RaisePropertyChanged("CartaBingo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CodResp {
            get {
                return this.CodRespField;
            }
            set {
                if ((object.ReferenceEquals(this.CodRespField, value) != true)) {
                    this.CodRespField = value;
                    this.RaisePropertyChanged("CodResp");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DescResp {
            get {
                return this.DescRespField;
            }
            set {
                if ((object.ReferenceEquals(this.DescRespField, value) != true)) {
                    this.DescRespField = value;
                    this.RaisePropertyChanged("DescResp");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NumeroTicketPozo {
            get {
                return this.NumeroTicketPozoField;
            }
            set {
                if ((object.ReferenceEquals(this.NumeroTicketPozoField, value) != true)) {
                    this.NumeroTicketPozoField = value;
                    this.RaisePropertyChanged("NumeroTicketPozo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Referencia {
            get {
                return this.ReferenciaField;
            }
            set {
                if ((object.ReferenceEquals(this.ReferenciaField, value) != true)) {
                    this.ReferenciaField = value;
                    this.RaisePropertyChanged("Referencia");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Carta", Namespace="http://schemas.datacontract.org/2004/07/")]
    [System.SerializableAttribute()]
    public partial class Carta : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CodSorteoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FechaSorteoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NumeroCartaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NumeroSerieField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NumeroSorteoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ReferenciaBingoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string linea1Field;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string linea2Field;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string linea3Field;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string linea4Field;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string linea5Field;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CodSorteo {
            get {
                return this.CodSorteoField;
            }
            set {
                if ((object.ReferenceEquals(this.CodSorteoField, value) != true)) {
                    this.CodSorteoField = value;
                    this.RaisePropertyChanged("CodSorteo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FechaSorteo {
            get {
                return this.FechaSorteoField;
            }
            set {
                if ((object.ReferenceEquals(this.FechaSorteoField, value) != true)) {
                    this.FechaSorteoField = value;
                    this.RaisePropertyChanged("FechaSorteo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NumeroCarta {
            get {
                return this.NumeroCartaField;
            }
            set {
                if ((object.ReferenceEquals(this.NumeroCartaField, value) != true)) {
                    this.NumeroCartaField = value;
                    this.RaisePropertyChanged("NumeroCarta");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NumeroSerie {
            get {
                return this.NumeroSerieField;
            }
            set {
                if ((object.ReferenceEquals(this.NumeroSerieField, value) != true)) {
                    this.NumeroSerieField = value;
                    this.RaisePropertyChanged("NumeroSerie");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NumeroSorteo {
            get {
                return this.NumeroSorteoField;
            }
            set {
                if ((object.ReferenceEquals(this.NumeroSorteoField, value) != true)) {
                    this.NumeroSorteoField = value;
                    this.RaisePropertyChanged("NumeroSorteo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ReferenciaBingo {
            get {
                return this.ReferenciaBingoField;
            }
            set {
                if ((object.ReferenceEquals(this.ReferenciaBingoField, value) != true)) {
                    this.ReferenciaBingoField = value;
                    this.RaisePropertyChanged("ReferenciaBingo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string linea1 {
            get {
                return this.linea1Field;
            }
            set {
                if ((object.ReferenceEquals(this.linea1Field, value) != true)) {
                    this.linea1Field = value;
                    this.RaisePropertyChanged("linea1");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string linea2 {
            get {
                return this.linea2Field;
            }
            set {
                if ((object.ReferenceEquals(this.linea2Field, value) != true)) {
                    this.linea2Field = value;
                    this.RaisePropertyChanged("linea2");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string linea3 {
            get {
                return this.linea3Field;
            }
            set {
                if ((object.ReferenceEquals(this.linea3Field, value) != true)) {
                    this.linea3Field = value;
                    this.RaisePropertyChanged("linea3");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string linea4 {
            get {
                return this.linea4Field;
            }
            set {
                if ((object.ReferenceEquals(this.linea4Field, value) != true)) {
                    this.linea4Field = value;
                    this.RaisePropertyChanged("linea4");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string linea5 {
            get {
                return this.linea5Field;
            }
            set {
                if ((object.ReferenceEquals(this.linea5Field, value) != true)) {
                    this.linea5Field = value;
                    this.RaisePropertyChanged("linea5");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="WSLotteryVIP.ILotteryAuthService")]
    public interface ILotteryAuthService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILotteryAuthService/AutorizaJugadaObj", ReplyAction="http://tempuri.org/ILotteryAuthService/AutorizaJugadaObjResponse")]
        MAR.DataAccess.WSLotteryVIP.RespuestaAuth AutorizaJugadaObj(string strConsorcio, string strUsuario, string strPassword, string strDataXml);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILotteryAuthService/AutorizaJugadaObj", ReplyAction="http://tempuri.org/ILotteryAuthService/AutorizaJugadaObjResponse")]
        System.Threading.Tasks.Task<MAR.DataAccess.WSLotteryVIP.RespuestaAuth> AutorizaJugadaObjAsync(string strConsorcio, string strUsuario, string strPassword, string strDataXml);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILotteryAuthService/AutorizaJugadaXml", ReplyAction="http://tempuri.org/ILotteryAuthService/AutorizaJugadaXmlResponse")]
        string AutorizaJugadaXml(string strConsorcio, string strUsuario, string strPassword, string strDataXml);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILotteryAuthService/AutorizaJugadaXml", ReplyAction="http://tempuri.org/ILotteryAuthService/AutorizaJugadaXmlResponse")]
        System.Threading.Tasks.Task<string> AutorizaJugadaXmlAsync(string strConsorcio, string strUsuario, string strPassword, string strDataXml);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILotteryAuthService/AutorizaJugadaJson", ReplyAction="http://tempuri.org/ILotteryAuthService/AutorizaJugadaJsonResponse")]
        string AutorizaJugadaJson(string strConsorcio, string strUsuario, string strPassword, string strDataJson);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILotteryAuthService/AutorizaJugadaJson", ReplyAction="http://tempuri.org/ILotteryAuthService/AutorizaJugadaJsonResponse")]
        System.Threading.Tasks.Task<string> AutorizaJugadaJsonAsync(string strConsorcio, string strUsuario, string strPassword, string strDataJson);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILotteryAuthService/AnulaJugadaObj", ReplyAction="http://tempuri.org/ILotteryAuthService/AnulaJugadaObjResponse")]
        MAR.DataAccess.WSLotteryVIP.RespuestaAuth AnulaJugadaObj(string strConsorcio, string strUsuario, string strPassword, string strAutorizacion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILotteryAuthService/AnulaJugadaObj", ReplyAction="http://tempuri.org/ILotteryAuthService/AnulaJugadaObjResponse")]
        System.Threading.Tasks.Task<MAR.DataAccess.WSLotteryVIP.RespuestaAuth> AnulaJugadaObjAsync(string strConsorcio, string strUsuario, string strPassword, string strAutorizacion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILotteryAuthService/AnulaJugadaXml", ReplyAction="http://tempuri.org/ILotteryAuthService/AnulaJugadaXmlResponse")]
        string AnulaJugadaXml(string strConsorcio, string strUsuario, string strPassword, string strAutorizacion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILotteryAuthService/AnulaJugadaXml", ReplyAction="http://tempuri.org/ILotteryAuthService/AnulaJugadaXmlResponse")]
        System.Threading.Tasks.Task<string> AnulaJugadaXmlAsync(string strConsorcio, string strUsuario, string strPassword, string strAutorizacion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILotteryAuthService/AnulaJugadaJson", ReplyAction="http://tempuri.org/ILotteryAuthService/AnulaJugadaJsonResponse")]
        string AnulaJugadaJson(string strConsorcio, string strUsuario, string strPassword, string strAutorizacion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILotteryAuthService/AnulaJugadaJson", ReplyAction="http://tempuri.org/ILotteryAuthService/AnulaJugadaJsonResponse")]
        System.Threading.Tasks.Task<string> AnulaJugadaJsonAsync(string strConsorcio, string strUsuario, string strPassword, string strAutorizacion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILotteryAuthService/ConsultaJugadasFechaXml", ReplyAction="http://tempuri.org/ILotteryAuthService/ConsultaJugadasFechaXmlResponse")]
        string ConsultaJugadasFechaXml(string strConsorcio, string strUsuario, string strPassword, string strFecha, int pPagina);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILotteryAuthService/ConsultaJugadasFechaXml", ReplyAction="http://tempuri.org/ILotteryAuthService/ConsultaJugadasFechaXmlResponse")]
        System.Threading.Tasks.Task<string> ConsultaJugadasFechaXmlAsync(string strConsorcio, string strUsuario, string strPassword, string strFecha, int pPagina);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILotteryAuthService/ConsultaJugadasFechaJson", ReplyAction="http://tempuri.org/ILotteryAuthService/ConsultaJugadasFechaJsonResponse")]
        string ConsultaJugadasFechaJson(string strConsorcio, string strUsuario, string strPassword, string strFecha, int pPagina);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILotteryAuthService/ConsultaJugadasFechaJson", ReplyAction="http://tempuri.org/ILotteryAuthService/ConsultaJugadasFechaJsonResponse")]
        System.Threading.Tasks.Task<string> ConsultaJugadasFechaJsonAsync(string strConsorcio, string strUsuario, string strPassword, string strFecha, int pPagina);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILotteryAuthService/ResultadoSorteos", ReplyAction="http://tempuri.org/ILotteryAuthService/ResultadoSorteosResponse")]
        string ResultadoSorteos(string strFecha, string formato);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILotteryAuthService/ResultadoSorteos", ReplyAction="http://tempuri.org/ILotteryAuthService/ResultadoSorteosResponse")]
        System.Threading.Tasks.Task<string> ResultadoSorteosAsync(string strFecha, string formato);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILotteryAuthService/BingoProximoXml", ReplyAction="http://tempuri.org/ILotteryAuthService/BingoProximoXmlResponse")]
        string BingoProximoXml(string strConsorcio, string strUsuario, string strPassword, int intNumSorteo, string strDataXml);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILotteryAuthService/BingoProximoXml", ReplyAction="http://tempuri.org/ILotteryAuthService/BingoProximoXmlResponse")]
        System.Threading.Tasks.Task<string> BingoProximoXmlAsync(string strConsorcio, string strUsuario, string strPassword, int intNumSorteo, string strDataXml);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILotteryAuthService/SorteosBingo", ReplyAction="http://tempuri.org/ILotteryAuthService/SorteosBingoResponse")]
        string SorteosBingo(string strConsorcio, string strUsuario, string strPassword, string strFormatoSalida);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILotteryAuthService/SorteosBingo", ReplyAction="http://tempuri.org/ILotteryAuthService/SorteosBingoResponse")]
        System.Threading.Tasks.Task<string> SorteosBingoAsync(string strConsorcio, string strUsuario, string strPassword, string strFormatoSalida);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILotteryAuthService/PagoTicketBingo", ReplyAction="http://tempuri.org/ILotteryAuthService/PagoTicketBingoResponse")]
        string PagoTicketBingo(string strConsorcio, string strUsuario, string strPassword, string strTicket, string ValorPagado, string strFormatoSalida);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILotteryAuthService/PagoTicketBingo", ReplyAction="http://tempuri.org/ILotteryAuthService/PagoTicketBingoResponse")]
        System.Threading.Tasks.Task<string> PagoTicketBingoAsync(string strConsorcio, string strUsuario, string strPassword, string strTicket, string ValorPagado, string strFormatoSalida);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILotteryAuthService/GanadoresBingoPorFecha", ReplyAction="http://tempuri.org/ILotteryAuthService/GanadoresBingoPorFechaResponse")]
        string GanadoresBingoPorFecha(string strConsorcio, string strUsuario, string strPassword, string Fecha, string strFormatoSalida);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILotteryAuthService/GanadoresBingoPorFecha", ReplyAction="http://tempuri.org/ILotteryAuthService/GanadoresBingoPorFechaResponse")]
        System.Threading.Tasks.Task<string> GanadoresBingoPorFechaAsync(string strConsorcio, string strUsuario, string strPassword, string Fecha, string strFormatoSalida);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILotteryAuthService/GanadorBingoPorTicket", ReplyAction="http://tempuri.org/ILotteryAuthService/GanadorBingoPorTicketResponse")]
        string GanadorBingoPorTicket(string strConsorcio, string strUsuario, string strPassword, string strTicket, string strFormatoSalida);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILotteryAuthService/GanadorBingoPorTicket", ReplyAction="http://tempuri.org/ILotteryAuthService/GanadorBingoPorTicketResponse")]
        System.Threading.Tasks.Task<string> GanadorBingoPorTicketAsync(string strConsorcio, string strUsuario, string strPassword, string strTicket, string strFormatoSalida);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILotteryAuthService/Check", ReplyAction="http://tempuri.org/ILotteryAuthService/CheckResponse")]
        MAR.DataAccess.WSLotteryVIP.RespuestaAuth Check(string Token);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILotteryAuthService/Check", ReplyAction="http://tempuri.org/ILotteryAuthService/CheckResponse")]
        System.Threading.Tasks.Task<MAR.DataAccess.WSLotteryVIP.RespuestaAuth> CheckAsync(string Token);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ILotteryAuthServiceChannel : MAR.DataAccess.WSLotteryVIP.ILotteryAuthService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class LotteryAuthServiceClient : System.ServiceModel.ClientBase<MAR.DataAccess.WSLotteryVIP.ILotteryAuthService>, MAR.DataAccess.WSLotteryVIP.ILotteryAuthService {
        
        public LotteryAuthServiceClient() {
        }
        
        public LotteryAuthServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public LotteryAuthServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LotteryAuthServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LotteryAuthServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public MAR.DataAccess.WSLotteryVIP.RespuestaAuth AutorizaJugadaObj(string strConsorcio, string strUsuario, string strPassword, string strDataXml) {
            return base.Channel.AutorizaJugadaObj(strConsorcio, strUsuario, strPassword, strDataXml);
        }
        
        public System.Threading.Tasks.Task<MAR.DataAccess.WSLotteryVIP.RespuestaAuth> AutorizaJugadaObjAsync(string strConsorcio, string strUsuario, string strPassword, string strDataXml) {
            return base.Channel.AutorizaJugadaObjAsync(strConsorcio, strUsuario, strPassword, strDataXml);
        }
        
        public string AutorizaJugadaXml(string strConsorcio, string strUsuario, string strPassword, string strDataXml) {
            return base.Channel.AutorizaJugadaXml(strConsorcio, strUsuario, strPassword, strDataXml);
        }
        
        public System.Threading.Tasks.Task<string> AutorizaJugadaXmlAsync(string strConsorcio, string strUsuario, string strPassword, string strDataXml) {
            return base.Channel.AutorizaJugadaXmlAsync(strConsorcio, strUsuario, strPassword, strDataXml);
        }
        
        public string AutorizaJugadaJson(string strConsorcio, string strUsuario, string strPassword, string strDataJson) {
            return base.Channel.AutorizaJugadaJson(strConsorcio, strUsuario, strPassword, strDataJson);
        }
        
        public System.Threading.Tasks.Task<string> AutorizaJugadaJsonAsync(string strConsorcio, string strUsuario, string strPassword, string strDataJson) {
            return base.Channel.AutorizaJugadaJsonAsync(strConsorcio, strUsuario, strPassword, strDataJson);
        }
        
        public MAR.DataAccess.WSLotteryVIP.RespuestaAuth AnulaJugadaObj(string strConsorcio, string strUsuario, string strPassword, string strAutorizacion) {
            return base.Channel.AnulaJugadaObj(strConsorcio, strUsuario, strPassword, strAutorizacion);
        }
        
        public System.Threading.Tasks.Task<MAR.DataAccess.WSLotteryVIP.RespuestaAuth> AnulaJugadaObjAsync(string strConsorcio, string strUsuario, string strPassword, string strAutorizacion) {
            return base.Channel.AnulaJugadaObjAsync(strConsorcio, strUsuario, strPassword, strAutorizacion);
        }
        
        public string AnulaJugadaXml(string strConsorcio, string strUsuario, string strPassword, string strAutorizacion) {
            return base.Channel.AnulaJugadaXml(strConsorcio, strUsuario, strPassword, strAutorizacion);
        }
        
        public System.Threading.Tasks.Task<string> AnulaJugadaXmlAsync(string strConsorcio, string strUsuario, string strPassword, string strAutorizacion) {
            return base.Channel.AnulaJugadaXmlAsync(strConsorcio, strUsuario, strPassword, strAutorizacion);
        }
        
        public string AnulaJugadaJson(string strConsorcio, string strUsuario, string strPassword, string strAutorizacion) {
            return base.Channel.AnulaJugadaJson(strConsorcio, strUsuario, strPassword, strAutorizacion);
        }
        
        public System.Threading.Tasks.Task<string> AnulaJugadaJsonAsync(string strConsorcio, string strUsuario, string strPassword, string strAutorizacion) {
            return base.Channel.AnulaJugadaJsonAsync(strConsorcio, strUsuario, strPassword, strAutorizacion);
        }
        
        public string ConsultaJugadasFechaXml(string strConsorcio, string strUsuario, string strPassword, string strFecha, int pPagina) {
            return base.Channel.ConsultaJugadasFechaXml(strConsorcio, strUsuario, strPassword, strFecha, pPagina);
        }
        
        public System.Threading.Tasks.Task<string> ConsultaJugadasFechaXmlAsync(string strConsorcio, string strUsuario, string strPassword, string strFecha, int pPagina) {
            return base.Channel.ConsultaJugadasFechaXmlAsync(strConsorcio, strUsuario, strPassword, strFecha, pPagina);
        }
        
        public string ConsultaJugadasFechaJson(string strConsorcio, string strUsuario, string strPassword, string strFecha, int pPagina) {
            return base.Channel.ConsultaJugadasFechaJson(strConsorcio, strUsuario, strPassword, strFecha, pPagina);
        }
        
        public System.Threading.Tasks.Task<string> ConsultaJugadasFechaJsonAsync(string strConsorcio, string strUsuario, string strPassword, string strFecha, int pPagina) {
            return base.Channel.ConsultaJugadasFechaJsonAsync(strConsorcio, strUsuario, strPassword, strFecha, pPagina);
        }
        
        public string ResultadoSorteos(string strFecha, string formato) {
            return base.Channel.ResultadoSorteos(strFecha, formato);
        }
        
        public System.Threading.Tasks.Task<string> ResultadoSorteosAsync(string strFecha, string formato) {
            return base.Channel.ResultadoSorteosAsync(strFecha, formato);
        }
        
        public string BingoProximoXml(string strConsorcio, string strUsuario, string strPassword, int intNumSorteo, string strDataXml) {
            return base.Channel.BingoProximoXml(strConsorcio, strUsuario, strPassword, intNumSorteo, strDataXml);
        }
        
        public System.Threading.Tasks.Task<string> BingoProximoXmlAsync(string strConsorcio, string strUsuario, string strPassword, int intNumSorteo, string strDataXml) {
            return base.Channel.BingoProximoXmlAsync(strConsorcio, strUsuario, strPassword, intNumSorteo, strDataXml);
        }
        
        public string SorteosBingo(string strConsorcio, string strUsuario, string strPassword, string strFormatoSalida) {
            return base.Channel.SorteosBingo(strConsorcio, strUsuario, strPassword, strFormatoSalida);
        }
        
        public System.Threading.Tasks.Task<string> SorteosBingoAsync(string strConsorcio, string strUsuario, string strPassword, string strFormatoSalida) {
            return base.Channel.SorteosBingoAsync(strConsorcio, strUsuario, strPassword, strFormatoSalida);
        }
        
        public string PagoTicketBingo(string strConsorcio, string strUsuario, string strPassword, string strTicket, string ValorPagado, string strFormatoSalida) {
            return base.Channel.PagoTicketBingo(strConsorcio, strUsuario, strPassword, strTicket, ValorPagado, strFormatoSalida);
        }
        
        public System.Threading.Tasks.Task<string> PagoTicketBingoAsync(string strConsorcio, string strUsuario, string strPassword, string strTicket, string ValorPagado, string strFormatoSalida) {
            return base.Channel.PagoTicketBingoAsync(strConsorcio, strUsuario, strPassword, strTicket, ValorPagado, strFormatoSalida);
        }
        
        public string GanadoresBingoPorFecha(string strConsorcio, string strUsuario, string strPassword, string Fecha, string strFormatoSalida) {
            return base.Channel.GanadoresBingoPorFecha(strConsorcio, strUsuario, strPassword, Fecha, strFormatoSalida);
        }
        
        public System.Threading.Tasks.Task<string> GanadoresBingoPorFechaAsync(string strConsorcio, string strUsuario, string strPassword, string Fecha, string strFormatoSalida) {
            return base.Channel.GanadoresBingoPorFechaAsync(strConsorcio, strUsuario, strPassword, Fecha, strFormatoSalida);
        }
        
        public string GanadorBingoPorTicket(string strConsorcio, string strUsuario, string strPassword, string strTicket, string strFormatoSalida) {
            return base.Channel.GanadorBingoPorTicket(strConsorcio, strUsuario, strPassword, strTicket, strFormatoSalida);
        }
        
        public System.Threading.Tasks.Task<string> GanadorBingoPorTicketAsync(string strConsorcio, string strUsuario, string strPassword, string strTicket, string strFormatoSalida) {
            return base.Channel.GanadorBingoPorTicketAsync(strConsorcio, strUsuario, strPassword, strTicket, strFormatoSalida);
        }
        
        public MAR.DataAccess.WSLotteryVIP.RespuestaAuth Check(string Token) {
            return base.Channel.Check(Token);
        }
        
        public System.Threading.Tasks.Task<MAR.DataAccess.WSLotteryVIP.RespuestaAuth> CheckAsync(string Token) {
            return base.Channel.CheckAsync(Token);
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="", ConfigurationName="WSLotteryVIP.ILotteryAuthServiceWeb")]
    public interface ILotteryAuthServiceWeb {
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:ILotteryAuthServiceWeb/ResultadoSorteosRest", ReplyAction="urn:ILotteryAuthServiceWeb/ResultadoSorteosRestResponse")]
        string ResultadoSorteosRest(string fecha, string formato, string token);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:ILotteryAuthServiceWeb/ResultadoSorteosRest", ReplyAction="urn:ILotteryAuthServiceWeb/ResultadoSorteosRestResponse")]
        System.Threading.Tasks.Task<string> ResultadoSorteosRestAsync(string fecha, string formato, string token);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:ILotteryAuthServiceWeb/AutorizaJugadaJsonRest", ReplyAction="urn:ILotteryAuthServiceWeb/AutorizaJugadaJsonRestResponse")]
        string AutorizaJugadaJsonRest(string strConsorcio, string strUsuario, string strPassword, string strDataJson, string strSerial, string strToken);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:ILotteryAuthServiceWeb/AutorizaJugadaJsonRest", ReplyAction="urn:ILotteryAuthServiceWeb/AutorizaJugadaJsonRestResponse")]
        System.Threading.Tasks.Task<string> AutorizaJugadaJsonRestAsync(string strConsorcio, string strUsuario, string strPassword, string strDataJson, string strSerial, string strToken);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:ILotteryAuthServiceWeb/AnulaJugadaJsonRest", ReplyAction="urn:ILotteryAuthServiceWeb/AnulaJugadaJsonRestResponse")]
        string AnulaJugadaJsonRest(string strConsorcio, string strUsuario, string strPassword, string strAutorizacion, string strFecha, string strToken);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:ILotteryAuthServiceWeb/AnulaJugadaJsonRest", ReplyAction="urn:ILotteryAuthServiceWeb/AnulaJugadaJsonRestResponse")]
        System.Threading.Tasks.Task<string> AnulaJugadaJsonRestAsync(string strConsorcio, string strUsuario, string strPassword, string strAutorizacion, string strFecha, string strToken);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:ILotteryAuthServiceWeb/SorteosBingo", ReplyAction="urn:ILotteryAuthServiceWeb/SorteosBingoResponse")]
        string SorteosBingo(string strConsorcio, string strUsuario, string strPassword, string strFormatoSalida);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:ILotteryAuthServiceWeb/SorteosBingo", ReplyAction="urn:ILotteryAuthServiceWeb/SorteosBingoResponse")]
        System.Threading.Tasks.Task<string> SorteosBingoAsync(string strConsorcio, string strUsuario, string strPassword, string strFormatoSalida);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ILotteryAuthServiceWebChannel : MAR.DataAccess.WSLotteryVIP.ILotteryAuthServiceWeb, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class LotteryAuthServiceWebClient : System.ServiceModel.ClientBase<MAR.DataAccess.WSLotteryVIP.ILotteryAuthServiceWeb>, MAR.DataAccess.WSLotteryVIP.ILotteryAuthServiceWeb {
        
        public LotteryAuthServiceWebClient() {
        }
        
        public LotteryAuthServiceWebClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public LotteryAuthServiceWebClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LotteryAuthServiceWebClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LotteryAuthServiceWebClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string ResultadoSorteosRest(string fecha, string formato, string token) {
            return base.Channel.ResultadoSorteosRest(fecha, formato, token);
        }
        
        public System.Threading.Tasks.Task<string> ResultadoSorteosRestAsync(string fecha, string formato, string token) {
            return base.Channel.ResultadoSorteosRestAsync(fecha, formato, token);
        }
        
        public string AutorizaJugadaJsonRest(string strConsorcio, string strUsuario, string strPassword, string strDataJson, string strSerial, string strToken) {
            return base.Channel.AutorizaJugadaJsonRest(strConsorcio, strUsuario, strPassword, strDataJson, strSerial, strToken);
        }
        
        public System.Threading.Tasks.Task<string> AutorizaJugadaJsonRestAsync(string strConsorcio, string strUsuario, string strPassword, string strDataJson, string strSerial, string strToken) {
            return base.Channel.AutorizaJugadaJsonRestAsync(strConsorcio, strUsuario, strPassword, strDataJson, strSerial, strToken);
        }
        
        public string AnulaJugadaJsonRest(string strConsorcio, string strUsuario, string strPassword, string strAutorizacion, string strFecha, string strToken) {
            return base.Channel.AnulaJugadaJsonRest(strConsorcio, strUsuario, strPassword, strAutorizacion, strFecha, strToken);
        }
        
        public System.Threading.Tasks.Task<string> AnulaJugadaJsonRestAsync(string strConsorcio, string strUsuario, string strPassword, string strAutorizacion, string strFecha, string strToken) {
            return base.Channel.AnulaJugadaJsonRestAsync(strConsorcio, strUsuario, strPassword, strAutorizacion, strFecha, strToken);
        }
        
        public string SorteosBingo(string strConsorcio, string strUsuario, string strPassword, string strFormatoSalida) {
            return base.Channel.SorteosBingo(strConsorcio, strUsuario, strPassword, strFormatoSalida);
        }
        
        public System.Threading.Tasks.Task<string> SorteosBingoAsync(string strConsorcio, string strUsuario, string strPassword, string strFormatoSalida) {
            return base.Channel.SorteosBingoAsync(strConsorcio, strUsuario, strPassword, strFormatoSalida);
        }
    }
}
