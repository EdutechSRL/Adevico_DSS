<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_GenericUploadFile.ascx.vb" Inherits="Comunita_OnLine.UC_GenericUploadFile" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<div style="width:100%">


    <telerik:RadUpload ID="RDUfiles" runat="server" Culture="(Default)" 
	   InitialFileInputsCount="4" MaxFileInputsCount="4" 
	   ReadOnlyFileInputs="True" Skin="Default" ControlObjectsVisibility="AddButton,RemoveButtons" EnableFileInputSkinning="true"
	   InputSize="70" Width="100%">
    </telerik:RadUpload>  
    <telerik:RadProgressArea ID="RPApersonalFiles" runat="server"  Skin="Default" >
    </telerik:RadProgressArea>
    <telerik:RadProgressManager ID="RPMpersonalFiles" runat="server" 
	   EnableEmbeddedBaseStylesheet="False" 
	   EnableEmbeddedSkins="False" 
	   />
</div>