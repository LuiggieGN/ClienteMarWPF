﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JuegaMasService
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
    [System.Runtime.Serialization.DataContractAttribute(Name="MAR_JuegaMasResponse", Namespace="mar.do")]
    public partial class MAR_JuegaMasResponse : object
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
    [System.ServiceModel.ServiceContractAttribute(Namespace="mar.do", ConfigurationName="JuegaMasService.JuegaMasSoap")]
    public interface JuegaMasSoap
    {
        
        // CODEGEN: Generating message contract since element name pSesion from namespace mar.do is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="mar.do/CallJuegaMaxIndexFunction", ReplyAction="*")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(JuegaMasService.MAR_Session))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(JuegaMasService.ArrayOfAnyType))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(JuegaMasService.MAR_JuegaMasResponse))]
        JuegaMasService.CallJuegaMaxIndexFunctionResponse CallJuegaMaxIndexFunction(JuegaMasService.CallJuegaMaxIndexFunctionRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="mar.do/CallJuegaMaxIndexFunction", ReplyAction="*")]
        System.Threading.Tasks.Task<JuegaMasService.CallJuegaMaxIndexFunctionResponse> CallJuegaMaxIndexFunctionAsync(JuegaMasService.CallJuegaMaxIndexFunctionRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class CallJuegaMaxIndexFunctionRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="CallJuegaMaxIndexFunction", Namespace="mar.do", Order=0)]
        public JuegaMasService.CallJuegaMaxIndexFunctionRequestBody Body;
        
        public CallJuegaMaxIndexFunctionRequest()
        {
        }
        
        public CallJuegaMaxIndexFunctionRequest(JuegaMasService.CallJuegaMaxIndexFunctionRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="mar.do")]
    public partial class CallJuegaMaxIndexFunctionRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public int pMetodo;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public JuegaMasService.MAR_Session pSesion;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public JuegaMasService.ArrayOfAnyType pParams;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=3)]
        public int pSolicitud;
        
        public CallJuegaMaxIndexFunctionRequestBody()
        {
        }
        
        public CallJuegaMaxIndexFunctionRequestBody(int pMetodo, JuegaMasService.MAR_Session pSesion, JuegaMasService.ArrayOfAnyType pParams, int pSolicitud)
        {
            this.pMetodo = pMetodo;
            this.pSesion = pSesion;
            this.pParams = pParams;
            this.pSolicitud = pSolicitud;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class CallJuegaMaxIndexFunctionResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="CallJuegaMaxIndexFunctionResponse", Namespace="mar.do", Order=0)]
        public JuegaMasService.CallJuegaMaxIndexFunctionResponseBody Body;
        
        public CallJuegaMaxIndexFunctionResponse()
        {
        }
        
        public CallJuegaMaxIndexFunctionResponse(JuegaMasService.CallJuegaMaxIndexFunctionResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="mar.do")]
    public partial class CallJuegaMaxIndexFunctionResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public JuegaMasService.MAR_JuegaMasResponse CallJuegaMaxIndexFunctionResult;
        
        public CallJuegaMaxIndexFunctionResponseBody()
        {
        }
        
        public CallJuegaMaxIndexFunctionResponseBody(JuegaMasService.MAR_JuegaMasResponse CallJuegaMaxIndexFunctionResult)
        {
            this.CallJuegaMaxIndexFunctionResult = CallJuegaMaxIndexFunctionResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    public interface JuegaMasSoapChannel : JuegaMasService.JuegaMasSoap, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    public partial class JuegaMasSoapClient : System.ServiceModel.ClientBase<JuegaMasService.JuegaMasSoap>, JuegaMasService.JuegaMasSoap
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public JuegaMasSoapClient(EndpointConfiguration endpointConfiguration) : 
                base(JuegaMasSoapClient.GetBindingForEndpoint(endpointConfiguration), JuegaMasSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public JuegaMasSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(JuegaMasSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public JuegaMasSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(JuegaMasSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public JuegaMasSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        JuegaMasService.CallJuegaMaxIndexFunctionResponse JuegaMasService.JuegaMasSoap.CallJuegaMaxIndexFunction(JuegaMasService.CallJuegaMaxIndexFunctionRequest request)
        {
            return base.Channel.CallJuegaMaxIndexFunction(request);
        }
        
        public JuegaMasService.MAR_JuegaMasResponse CallJuegaMaxIndexFunction(int pMetodo, JuegaMasService.MAR_Session pSesion, JuegaMasService.ArrayOfAnyType pParams, int pSolicitud)
        {
            JuegaMasService.CallJuegaMaxIndexFunctionRequest inValue = new JuegaMasService.CallJuegaMaxIndexFunctionRequest();
            inValue.Body = new JuegaMasService.CallJuegaMaxIndexFunctionRequestBody();
            inValue.Body.pMetodo = pMetodo;
            inValue.Body.pSesion = pSesion;
            inValue.Body.pParams = pParams;
            inValue.Body.pSolicitud = pSolicitud;
            JuegaMasService.CallJuegaMaxIndexFunctionResponse retVal = ((JuegaMasService.JuegaMasSoap)(this)).CallJuegaMaxIndexFunction(inValue);
            return retVal.Body.CallJuegaMaxIndexFunctionResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<JuegaMasService.CallJuegaMaxIndexFunctionResponse> JuegaMasService.JuegaMasSoap.CallJuegaMaxIndexFunctionAsync(JuegaMasService.CallJuegaMaxIndexFunctionRequest request)
        {
            return base.Channel.CallJuegaMaxIndexFunctionAsync(request);
        }
        
        public System.Threading.Tasks.Task<JuegaMasService.CallJuegaMaxIndexFunctionResponse> CallJuegaMaxIndexFunctionAsync(int pMetodo, JuegaMasService.MAR_Session pSesion, JuegaMasService.ArrayOfAnyType pParams, int pSolicitud)
        {
            JuegaMasService.CallJuegaMaxIndexFunctionRequest inValue = new JuegaMasService.CallJuegaMaxIndexFunctionRequest();
            inValue.Body = new JuegaMasService.CallJuegaMaxIndexFunctionRequestBody();
            inValue.Body.pMetodo = pMetodo;
            inValue.Body.pSesion = pSesion;
            inValue.Body.pParams = pParams;
            inValue.Body.pSolicitud = pSolicitud;
            return ((JuegaMasService.JuegaMasSoap)(this)).CallJuegaMaxIndexFunctionAsync(inValue);
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
            if ((endpointConfiguration == EndpointConfiguration.JuegaMasSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.JuegaMasSoap12))
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
            if ((endpointConfiguration == EndpointConfiguration.JuegaMasSoap))
            {
                return new System.ServiceModel.EndpointAddress("http://pruebasmar.ddns.net/Mar-Svr5/mar-juegamas.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.JuegaMasSoap12))
            {
                return new System.ServiceModel.EndpointAddress("http://pruebasmar.ddns.net/Mar-Svr5/mar-juegamas.asmx");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        public enum EndpointConfiguration
        {
            
            JuegaMasSoap,
            
            JuegaMasSoap12,
        }
    }
}