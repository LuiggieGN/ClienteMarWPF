﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FlujoService
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name="MAR_Session", Namespace="mar.do")]
    public partial class MAR_Session : object
    {
        
        private int BancaField;
        
        private int UsuarioField;
        
        private int SesionField;
        
        private string ErrField;
        
        private int LastTckField;
        
        private int LastPinField;
        
        private int PrinterSizeField;
        
        private string PrinterHeaderField;
        
        private string PrinterFooterField;
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public int Banca
        {
            get
            {
                return this.BancaField;
            }
            set
            {
                this.BancaField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public int Usuario
        {
            get
            {
                return this.UsuarioField;
            }
            set
            {
                this.UsuarioField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=2)]
        public int Sesion
        {
            get
            {
                return this.SesionField;
            }
            set
            {
                this.SesionField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string Err
        {
            get
            {
                return this.ErrField;
            }
            set
            {
                this.ErrField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=4)]
        public int LastTck
        {
            get
            {
                return this.LastTckField;
            }
            set
            {
                this.LastTckField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=5)]
        public int LastPin
        {
            get
            {
                return this.LastPinField;
            }
            set
            {
                this.LastPinField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=6)]
        public int PrinterSize
        {
            get
            {
                return this.PrinterSizeField;
            }
            set
            {
                this.PrinterSizeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=7)]
        public string PrinterHeader
        {
            get
            {
                return this.PrinterHeaderField;
            }
            set
            {
                this.PrinterHeaderField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=8)]
        public string PrinterFooter
        {
            get
            {
                return this.PrinterFooterField;
            }
            set
            {
                this.PrinterFooterField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.CollectionDataContractAttribute(Name="ArrayOfAnyType", Namespace="mar.do", ItemName="anyType")]
    public class ArrayOfAnyType : System.Collections.Generic.List<object>
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name="MAR_FlujoResponse", Namespace="mar.do")]
    public partial class MAR_FlujoResponse : object
    {
        
        private bool OKField;
        
        private string MensajeField;
        
        private string RespuestaField;
        
        private string ErrField;
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public bool OK
        {
            get
            {
                return this.OKField;
            }
            set
            {
                this.OKField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string Mensaje
        {
            get
            {
                return this.MensajeField;
            }
            set
            {
                this.MensajeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string Respuesta
        {
            get
            {
                return this.RespuestaField;
            }
            set
            {
                this.RespuestaField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string Err
        {
            get
            {
                return this.ErrField;
            }
            set
            {
                this.ErrField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="mar.do", ConfigurationName="FlujoService.mar_flujoSoap")]
    public interface mar_flujoSoap
    {
        
        // CODEGEN: Generating message contract since element name pSesion from namespace mar.do is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="mar.do/CallFlujoIndexFunction", ReplyAction="*")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(FlujoService.MAR_Session))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(FlujoService.ArrayOfAnyType))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(FlujoService.MAR_FlujoResponse))]
        FlujoService.CallFlujoIndexFunctionResponse CallFlujoIndexFunction(FlujoService.CallFlujoIndexFunctionRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="mar.do/CallFlujoIndexFunction", ReplyAction="*")]
        System.Threading.Tasks.Task<FlujoService.CallFlujoIndexFunctionResponse> CallFlujoIndexFunctionAsync(FlujoService.CallFlujoIndexFunctionRequest request);
        
        // CODEGEN: Generating message contract since element name parametros from namespace mar.do is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="mar.do/CallControlEfectivoFunciones", ReplyAction="*")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(FlujoService.MAR_Session))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(FlujoService.ArrayOfAnyType))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(FlujoService.MAR_FlujoResponse))]
        FlujoService.CallControlEfectivoFuncionesResponse CallControlEfectivoFunciones(FlujoService.CallControlEfectivoFuncionesRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="mar.do/CallControlEfectivoFunciones", ReplyAction="*")]
        System.Threading.Tasks.Task<FlujoService.CallControlEfectivoFuncionesResponse> CallControlEfectivoFuncionesAsync(FlujoService.CallControlEfectivoFuncionesRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class CallFlujoIndexFunctionRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="CallFlujoIndexFunction", Namespace="mar.do", Order=0)]
        public FlujoService.CallFlujoIndexFunctionRequestBody Body;
        
        public CallFlujoIndexFunctionRequest()
        {
        }
        
        public CallFlujoIndexFunctionRequest(FlujoService.CallFlujoIndexFunctionRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="mar.do")]
    public partial class CallFlujoIndexFunctionRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public int pMetodo;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public FlujoService.MAR_Session pSesion;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public FlujoService.ArrayOfAnyType pParams;
        
        public CallFlujoIndexFunctionRequestBody()
        {
        }
        
        public CallFlujoIndexFunctionRequestBody(int pMetodo, FlujoService.MAR_Session pSesion, FlujoService.ArrayOfAnyType pParams)
        {
            this.pMetodo = pMetodo;
            this.pSesion = pSesion;
            this.pParams = pParams;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class CallFlujoIndexFunctionResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="CallFlujoIndexFunctionResponse", Namespace="mar.do", Order=0)]
        public FlujoService.CallFlujoIndexFunctionResponseBody Body;
        
        public CallFlujoIndexFunctionResponse()
        {
        }
        
        public CallFlujoIndexFunctionResponse(FlujoService.CallFlujoIndexFunctionResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="mar.do")]
    public partial class CallFlujoIndexFunctionResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public FlujoService.MAR_FlujoResponse CallFlujoIndexFunctionResult;
        
        public CallFlujoIndexFunctionResponseBody()
        {
        }
        
        public CallFlujoIndexFunctionResponseBody(FlujoService.MAR_FlujoResponse CallFlujoIndexFunctionResult)
        {
            this.CallFlujoIndexFunctionResult = CallFlujoIndexFunctionResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class CallControlEfectivoFuncionesRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="CallControlEfectivoFunciones", Namespace="mar.do", Order=0)]
        public FlujoService.CallControlEfectivoFuncionesRequestBody Body;
        
        public CallControlEfectivoFuncionesRequest()
        {
        }
        
        public CallControlEfectivoFuncionesRequest(FlujoService.CallControlEfectivoFuncionesRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="mar.do")]
    public partial class CallControlEfectivoFuncionesRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public int metodo;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public FlujoService.ArrayOfAnyType parametros;
        
        public CallControlEfectivoFuncionesRequestBody()
        {
        }
        
        public CallControlEfectivoFuncionesRequestBody(int metodo, FlujoService.ArrayOfAnyType parametros)
        {
            this.metodo = metodo;
            this.parametros = parametros;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class CallControlEfectivoFuncionesResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="CallControlEfectivoFuncionesResponse", Namespace="mar.do", Order=0)]
        public FlujoService.CallControlEfectivoFuncionesResponseBody Body;
        
        public CallControlEfectivoFuncionesResponse()
        {
        }
        
        public CallControlEfectivoFuncionesResponse(FlujoService.CallControlEfectivoFuncionesResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="mar.do")]
    public partial class CallControlEfectivoFuncionesResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public FlujoService.MAR_FlujoResponse CallControlEfectivoFuncionesResult;
        
        public CallControlEfectivoFuncionesResponseBody()
        {
        }
        
        public CallControlEfectivoFuncionesResponseBody(FlujoService.MAR_FlujoResponse CallControlEfectivoFuncionesResult)
        {
            this.CallControlEfectivoFuncionesResult = CallControlEfectivoFuncionesResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    public interface mar_flujoSoapChannel : FlujoService.mar_flujoSoap, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    public partial class mar_flujoSoapClient : System.ServiceModel.ClientBase<FlujoService.mar_flujoSoap>, FlujoService.mar_flujoSoap
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public mar_flujoSoapClient(EndpointConfiguration endpointConfiguration) : 
                base(mar_flujoSoapClient.GetBindingForEndpoint(endpointConfiguration), mar_flujoSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public mar_flujoSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(mar_flujoSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public mar_flujoSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(mar_flujoSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public mar_flujoSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        FlujoService.CallFlujoIndexFunctionResponse FlujoService.mar_flujoSoap.CallFlujoIndexFunction(FlujoService.CallFlujoIndexFunctionRequest request)
        {
            return base.Channel.CallFlujoIndexFunction(request);
        }
        
        public FlujoService.MAR_FlujoResponse CallFlujoIndexFunction(int pMetodo, FlujoService.MAR_Session pSesion, FlujoService.ArrayOfAnyType pParams)
        {
            FlujoService.CallFlujoIndexFunctionRequest inValue = new FlujoService.CallFlujoIndexFunctionRequest();
            inValue.Body = new FlujoService.CallFlujoIndexFunctionRequestBody();
            inValue.Body.pMetodo = pMetodo;
            inValue.Body.pSesion = pSesion;
            inValue.Body.pParams = pParams;
            FlujoService.CallFlujoIndexFunctionResponse retVal = ((FlujoService.mar_flujoSoap)(this)).CallFlujoIndexFunction(inValue);
            return retVal.Body.CallFlujoIndexFunctionResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<FlujoService.CallFlujoIndexFunctionResponse> FlujoService.mar_flujoSoap.CallFlujoIndexFunctionAsync(FlujoService.CallFlujoIndexFunctionRequest request)
        {
            return base.Channel.CallFlujoIndexFunctionAsync(request);
        }
        
        public System.Threading.Tasks.Task<FlujoService.CallFlujoIndexFunctionResponse> CallFlujoIndexFunctionAsync(int pMetodo, FlujoService.MAR_Session pSesion, FlujoService.ArrayOfAnyType pParams)
        {
            FlujoService.CallFlujoIndexFunctionRequest inValue = new FlujoService.CallFlujoIndexFunctionRequest();
            inValue.Body = new FlujoService.CallFlujoIndexFunctionRequestBody();
            inValue.Body.pMetodo = pMetodo;
            inValue.Body.pSesion = pSesion;
            inValue.Body.pParams = pParams;
            return ((FlujoService.mar_flujoSoap)(this)).CallFlujoIndexFunctionAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        FlujoService.CallControlEfectivoFuncionesResponse FlujoService.mar_flujoSoap.CallControlEfectivoFunciones(FlujoService.CallControlEfectivoFuncionesRequest request)
        {
            return base.Channel.CallControlEfectivoFunciones(request);
        }
        
        public FlujoService.MAR_FlujoResponse CallControlEfectivoFunciones(int metodo, FlujoService.ArrayOfAnyType parametros)
        {
            FlujoService.CallControlEfectivoFuncionesRequest inValue = new FlujoService.CallControlEfectivoFuncionesRequest();
            inValue.Body = new FlujoService.CallControlEfectivoFuncionesRequestBody();
            inValue.Body.metodo = metodo;
            inValue.Body.parametros = parametros;
            FlujoService.CallControlEfectivoFuncionesResponse retVal = ((FlujoService.mar_flujoSoap)(this)).CallControlEfectivoFunciones(inValue);
            return retVal.Body.CallControlEfectivoFuncionesResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<FlujoService.CallControlEfectivoFuncionesResponse> FlujoService.mar_flujoSoap.CallControlEfectivoFuncionesAsync(FlujoService.CallControlEfectivoFuncionesRequest request)
        {
            return base.Channel.CallControlEfectivoFuncionesAsync(request);
        }
        
        public System.Threading.Tasks.Task<FlujoService.CallControlEfectivoFuncionesResponse> CallControlEfectivoFuncionesAsync(int metodo, FlujoService.ArrayOfAnyType parametros)
        {
            FlujoService.CallControlEfectivoFuncionesRequest inValue = new FlujoService.CallControlEfectivoFuncionesRequest();
            inValue.Body = new FlujoService.CallControlEfectivoFuncionesRequestBody();
            inValue.Body.metodo = metodo;
            inValue.Body.parametros = parametros;
            return ((FlujoService.mar_flujoSoap)(this)).CallControlEfectivoFuncionesAsync(inValue);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.mar_flujoSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.mar_flujoSoap12))
            {
                System.ServiceModel.Channels.CustomBinding result = new System.ServiceModel.Channels.CustomBinding();
                System.ServiceModel.Channels.TextMessageEncodingBindingElement textBindingElement = new System.ServiceModel.Channels.TextMessageEncodingBindingElement();
                textBindingElement.MessageVersion = System.ServiceModel.Channels.MessageVersion.CreateVersion(System.ServiceModel.EnvelopeVersion.Soap12, System.ServiceModel.Channels.AddressingVersion.None);
                result.Elements.Add(textBindingElement);
                System.ServiceModel.Channels.HttpTransportBindingElement httpBindingElement = new System.ServiceModel.Channels.HttpTransportBindingElement();
                httpBindingElement.AllowCookies = true;
                httpBindingElement.MaxBufferSize = int.MaxValue;
                httpBindingElement.MaxReceivedMessageSize = int.MaxValue;
                result.Elements.Add(httpBindingElement);
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.mar_flujoSoap))
            {
                return new System.ServiceModel.EndpointAddress("http://localhost:14217/mar-flujo.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.mar_flujoSoap12))
            {
                return new System.ServiceModel.EndpointAddress("http://localhost:14217/mar-flujo.asmx");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        public enum EndpointConfiguration
        {
            
            mar_flujoSoap,
            
            mar_flujoSoap12,
        }
    }
}
