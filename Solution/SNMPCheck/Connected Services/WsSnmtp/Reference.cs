﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SNMPCheck.WsSnmtp {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="dtoActionValues", Namespace="http://tempuri.org/")]
    [System.SerializableAttribute()]
    public partial class dtoActionValues : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SystemField;
        
        private long ProgressiveField;
        
        private long EventIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private SNMPCheck.WsSnmtp.dtoUserValues UserField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private SNMPCheck.WsSnmtp.dtoActionData ActionField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
        public string System {
            get {
                return this.SystemField;
            }
            set {
                if ((object.ReferenceEquals(this.SystemField, value) != true)) {
                    this.SystemField = value;
                    this.RaisePropertyChanged("System");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=1)]
        public long Progressive {
            get {
                return this.ProgressiveField;
            }
            set {
                if ((this.ProgressiveField.Equals(value) != true)) {
                    this.ProgressiveField = value;
                    this.RaisePropertyChanged("Progressive");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=2)]
        public long EventId {
            get {
                return this.EventIdField;
            }
            set {
                if ((this.EventIdField.Equals(value) != true)) {
                    this.EventIdField = value;
                    this.RaisePropertyChanged("EventId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public SNMPCheck.WsSnmtp.dtoUserValues User {
            get {
                return this.UserField;
            }
            set {
                if ((object.ReferenceEquals(this.UserField, value) != true)) {
                    this.UserField = value;
                    this.RaisePropertyChanged("User");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public SNMPCheck.WsSnmtp.dtoActionData Action {
            get {
                return this.ActionField;
            }
            set {
                if ((object.ReferenceEquals(this.ActionField, value) != true)) {
                    this.ActionField = value;
                    this.RaisePropertyChanged("Action");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="dtoUserValues", Namespace="http://tempuri.org/")]
    [System.SerializableAttribute()]
    public partial class dtoUserValues : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private int idField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string mailField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string taxCodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string loginField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string nameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string surnameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IpField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ProxyIpField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public int id {
            get {
                return this.idField;
            }
            set {
                if ((this.idField.Equals(value) != true)) {
                    this.idField = value;
                    this.RaisePropertyChanged("id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
        public string mail {
            get {
                return this.mailField;
            }
            set {
                if ((object.ReferenceEquals(this.mailField, value) != true)) {
                    this.mailField = value;
                    this.RaisePropertyChanged("mail");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
        public string taxCode {
            get {
                return this.taxCodeField;
            }
            set {
                if ((object.ReferenceEquals(this.taxCodeField, value) != true)) {
                    this.taxCodeField = value;
                    this.RaisePropertyChanged("taxCode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string login {
            get {
                return this.loginField;
            }
            set {
                if ((object.ReferenceEquals(this.loginField, value) != true)) {
                    this.loginField = value;
                    this.RaisePropertyChanged("login");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string name {
            get {
                return this.nameField;
            }
            set {
                if ((object.ReferenceEquals(this.nameField, value) != true)) {
                    this.nameField = value;
                    this.RaisePropertyChanged("name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=5)]
        public string surname {
            get {
                return this.surnameField;
            }
            set {
                if ((object.ReferenceEquals(this.surnameField, value) != true)) {
                    this.surnameField = value;
                    this.RaisePropertyChanged("surname");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=6)]
        public string Ip {
            get {
                return this.IpField;
            }
            set {
                if ((object.ReferenceEquals(this.IpField, value) != true)) {
                    this.IpField = value;
                    this.RaisePropertyChanged("Ip");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=7)]
        public string ProxyIp {
            get {
                return this.ProxyIpField;
            }
            set {
                if ((object.ReferenceEquals(this.ProxyIpField, value) != true)) {
                    this.ProxyIpField = value;
                    this.RaisePropertyChanged("ProxyIp");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="dtoActionData", Namespace="http://tempuri.org/")]
    [System.SerializableAttribute()]
    public partial class dtoActionData : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ModuleIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ModuleCodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ActionCodeIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ActionTypeIdField;
        
        private int CommunityIdField;
        
        private bool CommunityIsFederatedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string InteractionTypeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ObjectTypeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ObjectIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SuccessInfoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string GenericInfoField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
        public string ModuleId {
            get {
                return this.ModuleIdField;
            }
            set {
                if ((object.ReferenceEquals(this.ModuleIdField, value) != true)) {
                    this.ModuleIdField = value;
                    this.RaisePropertyChanged("ModuleId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string ModuleCode {
            get {
                return this.ModuleCodeField;
            }
            set {
                if ((object.ReferenceEquals(this.ModuleCodeField, value) != true)) {
                    this.ModuleCodeField = value;
                    this.RaisePropertyChanged("ModuleCode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string ActionCodeId {
            get {
                return this.ActionCodeIdField;
            }
            set {
                if ((object.ReferenceEquals(this.ActionCodeIdField, value) != true)) {
                    this.ActionCodeIdField = value;
                    this.RaisePropertyChanged("ActionCodeId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string ActionTypeId {
            get {
                return this.ActionTypeIdField;
            }
            set {
                if ((object.ReferenceEquals(this.ActionTypeIdField, value) != true)) {
                    this.ActionTypeIdField = value;
                    this.RaisePropertyChanged("ActionTypeId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=4)]
        public int CommunityId {
            get {
                return this.CommunityIdField;
            }
            set {
                if ((this.CommunityIdField.Equals(value) != true)) {
                    this.CommunityIdField = value;
                    this.RaisePropertyChanged("CommunityId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=5)]
        public bool CommunityIsFederated {
            get {
                return this.CommunityIsFederatedField;
            }
            set {
                if ((this.CommunityIsFederatedField.Equals(value) != true)) {
                    this.CommunityIsFederatedField = value;
                    this.RaisePropertyChanged("CommunityIsFederated");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=6)]
        public string InteractionType {
            get {
                return this.InteractionTypeField;
            }
            set {
                if ((object.ReferenceEquals(this.InteractionTypeField, value) != true)) {
                    this.InteractionTypeField = value;
                    this.RaisePropertyChanged("InteractionType");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=7)]
        public string ObjectType {
            get {
                return this.ObjectTypeField;
            }
            set {
                if ((object.ReferenceEquals(this.ObjectTypeField, value) != true)) {
                    this.ObjectTypeField = value;
                    this.RaisePropertyChanged("ObjectType");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=8)]
        public string ObjectId {
            get {
                return this.ObjectIdField;
            }
            set {
                if ((object.ReferenceEquals(this.ObjectIdField, value) != true)) {
                    this.ObjectIdField = value;
                    this.RaisePropertyChanged("ObjectId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=9)]
        public string SuccessInfo {
            get {
                return this.SuccessInfoField;
            }
            set {
                if ((object.ReferenceEquals(this.SuccessInfoField, value) != true)) {
                    this.SuccessInfoField = value;
                    this.RaisePropertyChanged("SuccessInfo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=10)]
        public string GenericInfo {
            get {
                return this.GenericInfoField;
            }
            set {
                if ((object.ReferenceEquals(this.GenericInfoField, value) != true)) {
                    this.GenericInfoField = value;
                    this.RaisePropertyChanged("GenericInfo");
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="WsSnmtp.WsSnmtpSoap")]
    public interface WsSnmtpSoap {
        
        // CODEGEN: Generating message contract since element name id from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SendTrapString", ReplyAction="*")]
        SNMPCheck.WsSnmtp.SendTrapStringResponse SendTrapString(SNMPCheck.WsSnmtp.SendTrapStringRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SendTrapString", ReplyAction="*")]
        System.Threading.Tasks.Task<SNMPCheck.WsSnmtp.SendTrapStringResponse> SendTrapStringAsync(SNMPCheck.WsSnmtp.SendTrapStringRequest request);
        
        // CODEGEN: Generating message contract since element name id from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SendTrapActionValue", ReplyAction="*")]
        SNMPCheck.WsSnmtp.SendTrapActionValueResponse SendTrapActionValue(SNMPCheck.WsSnmtp.SendTrapActionValueRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SendTrapActionValue", ReplyAction="*")]
        System.Threading.Tasks.Task<SNMPCheck.WsSnmtp.SendTrapActionValueResponse> SendTrapActionValueAsync(SNMPCheck.WsSnmtp.SendTrapActionValueRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SendTrapStringRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SendTrapString", Namespace="http://tempuri.org/", Order=0)]
        public SNMPCheck.WsSnmtp.SendTrapStringRequestBody Body;
        
        public SendTrapStringRequest() {
        }
        
        public SendTrapStringRequest(SNMPCheck.WsSnmtp.SendTrapStringRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class SendTrapStringRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string id;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string value;
        
        public SendTrapStringRequestBody() {
        }
        
        public SendTrapStringRequestBody(string id, string value) {
            this.id = id;
            this.value = value;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SendTrapStringResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SendTrapStringResponse", Namespace="http://tempuri.org/", Order=0)]
        public SNMPCheck.WsSnmtp.SendTrapStringResponseBody Body;
        
        public SendTrapStringResponse() {
        }
        
        public SendTrapStringResponse(SNMPCheck.WsSnmtp.SendTrapStringResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class SendTrapStringResponseBody {
        
        public SendTrapStringResponseBody() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SendTrapActionValueRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SendTrapActionValue", Namespace="http://tempuri.org/", Order=0)]
        public SNMPCheck.WsSnmtp.SendTrapActionValueRequestBody Body;
        
        public SendTrapActionValueRequest() {
        }
        
        public SendTrapActionValueRequest(SNMPCheck.WsSnmtp.SendTrapActionValueRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class SendTrapActionValueRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string id;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public SNMPCheck.WsSnmtp.dtoActionValues value;
        
        public SendTrapActionValueRequestBody() {
        }
        
        public SendTrapActionValueRequestBody(string id, SNMPCheck.WsSnmtp.dtoActionValues value) {
            this.id = id;
            this.value = value;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SendTrapActionValueResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SendTrapActionValueResponse", Namespace="http://tempuri.org/", Order=0)]
        public SNMPCheck.WsSnmtp.SendTrapActionValueResponseBody Body;
        
        public SendTrapActionValueResponse() {
        }
        
        public SendTrapActionValueResponse(SNMPCheck.WsSnmtp.SendTrapActionValueResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class SendTrapActionValueResponseBody {
        
        public SendTrapActionValueResponseBody() {
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface WsSnmtpSoapChannel : SNMPCheck.WsSnmtp.WsSnmtpSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WsSnmtpSoapClient : System.ServiceModel.ClientBase<SNMPCheck.WsSnmtp.WsSnmtpSoap>, SNMPCheck.WsSnmtp.WsSnmtpSoap {
        
        public WsSnmtpSoapClient() {
        }
        
        public WsSnmtpSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WsSnmtpSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WsSnmtpSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WsSnmtpSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SNMPCheck.WsSnmtp.SendTrapStringResponse SNMPCheck.WsSnmtp.WsSnmtpSoap.SendTrapString(SNMPCheck.WsSnmtp.SendTrapStringRequest request) {
            return base.Channel.SendTrapString(request);
        }
        
        public void SendTrapString(string id, string value) {
            SNMPCheck.WsSnmtp.SendTrapStringRequest inValue = new SNMPCheck.WsSnmtp.SendTrapStringRequest();
            inValue.Body = new SNMPCheck.WsSnmtp.SendTrapStringRequestBody();
            inValue.Body.id = id;
            inValue.Body.value = value;
            SNMPCheck.WsSnmtp.SendTrapStringResponse retVal = ((SNMPCheck.WsSnmtp.WsSnmtpSoap)(this)).SendTrapString(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<SNMPCheck.WsSnmtp.SendTrapStringResponse> SNMPCheck.WsSnmtp.WsSnmtpSoap.SendTrapStringAsync(SNMPCheck.WsSnmtp.SendTrapStringRequest request) {
            return base.Channel.SendTrapStringAsync(request);
        }
        
        public System.Threading.Tasks.Task<SNMPCheck.WsSnmtp.SendTrapStringResponse> SendTrapStringAsync(string id, string value) {
            SNMPCheck.WsSnmtp.SendTrapStringRequest inValue = new SNMPCheck.WsSnmtp.SendTrapStringRequest();
            inValue.Body = new SNMPCheck.WsSnmtp.SendTrapStringRequestBody();
            inValue.Body.id = id;
            inValue.Body.value = value;
            return ((SNMPCheck.WsSnmtp.WsSnmtpSoap)(this)).SendTrapStringAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SNMPCheck.WsSnmtp.SendTrapActionValueResponse SNMPCheck.WsSnmtp.WsSnmtpSoap.SendTrapActionValue(SNMPCheck.WsSnmtp.SendTrapActionValueRequest request) {
            return base.Channel.SendTrapActionValue(request);
        }
        
        public void SendTrapActionValue(string id, SNMPCheck.WsSnmtp.dtoActionValues value) {
            SNMPCheck.WsSnmtp.SendTrapActionValueRequest inValue = new SNMPCheck.WsSnmtp.SendTrapActionValueRequest();
            inValue.Body = new SNMPCheck.WsSnmtp.SendTrapActionValueRequestBody();
            inValue.Body.id = id;
            inValue.Body.value = value;
            SNMPCheck.WsSnmtp.SendTrapActionValueResponse retVal = ((SNMPCheck.WsSnmtp.WsSnmtpSoap)(this)).SendTrapActionValue(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<SNMPCheck.WsSnmtp.SendTrapActionValueResponse> SNMPCheck.WsSnmtp.WsSnmtpSoap.SendTrapActionValueAsync(SNMPCheck.WsSnmtp.SendTrapActionValueRequest request) {
            return base.Channel.SendTrapActionValueAsync(request);
        }
        
        public System.Threading.Tasks.Task<SNMPCheck.WsSnmtp.SendTrapActionValueResponse> SendTrapActionValueAsync(string id, SNMPCheck.WsSnmtp.dtoActionValues value) {
            SNMPCheck.WsSnmtp.SendTrapActionValueRequest inValue = new SNMPCheck.WsSnmtp.SendTrapActionValueRequest();
            inValue.Body = new SNMPCheck.WsSnmtp.SendTrapActionValueRequestBody();
            inValue.Body.id = id;
            inValue.Body.value = value;
            return ((SNMPCheck.WsSnmtp.WsSnmtpSoap)(this)).SendTrapActionValueAsync(inValue);
        }
    }
}
