<%@ Control 
	Language="vb" 
	AutoEventWireup="false" 
	CodeBehind="UC_CompactInternalFileUploader2.ascx.vb" 
	Inherits="Comunita_OnLine.UC_CompactInternalFileUploader2" %>

<%--

	<input class="Testo_campo_obbligatorioSmall" id="TXBFile" type="file" size="60" name="TXBFile" runat="server" />

	ControlObjectsVisibility="AddButton" 
	
--%>

<%--	<style>
		.CompactUploder a.ruFileInput
		{
			color: aquamarine;
		}
	</style>--%>

	<telerik:RadAsyncUpload ID="RadAsyncUpload" runat="server" Culture="(Default)" 
	   InitialFileInputsCount="1" MaxFileInputsCount="1" 
	   ReadOnlyFileInputs="True" Skin="Default" 
	   EnableFileInputSkinning="False" CssClass="CompactUploder"
	   RenderMode="Lightweight"
	   InputSize="70"
	   OnClientFilesUploaded="UnLockLinkButton" 
	   OnClientFileUploadRemoved="LockLinkButton" 
       OnClientFileUploadFailed="LockLinkButton" 
       OnClientFileUploading="LockLinkButton"
	   OnClientValidationFailed="ValidationFailed"
		>
	</telerik:RadAsyncUpload>  



<asp:Literal ID="LTextension" runat="server">



</asp:Literal>

<%--
	 
	
		 OnClientFileUploaded="UnlockButton"

	AllowedMimeTypes
	
	
	<asp:CustomValidator runat="server" ID="CV_FileType" Display="Dynamic" ClientValidationFunction="validateRadAsyncUpload"
	OnServerValidate="CV_FileType_ServerValidate">        
	Estensione non valida. Ammessi solo file PDF.
</asp:CustomValidator>--%>

<script type="text/javascript">

	function UnLockLinkButton(sender)
	{
		var button = "<%=ButtonToLock%>";
		
		if (button)
		{
			jQuery("#" + button).removeClass("hide");
			jQuery("#" + button).addClass("linkMenu");
			

			//jQuery("#" + button).attr("class", "linkMenu");
		}

	}

	function LockLinkButton(sender)
	{
		var button = "<%=ButtonToLock%>";

		if (button) {
			jQuery("#" + button).removeClass("linkMenu");
			jQuery("#" + button).addClass("hide");
		}
	}
	<%--function UnlockButton(sender) {
		alert("<%= ButtonToLock()%>");


	 

	}--%>

	function ValidationFailed(sender, args) {
		
		var fileExtention = args.get_fileName().substring(args.get_fileName().lastIndexOf('.') + 1, args.get_fileName().length);

		if (args.get_fileName().lastIndexOf('.') != -1) {//this checks if the extension is correct
			if (sender.get_allowedFileExtensions().indexOf(fileExtention) == -1) {
				alert("File non valido: ammessi solo .pdf o .p7m");
			}
			else {
				alert("Dimensioni file non consentite.");
			}
		}
		else {
			alert("Estensione non valida.");
		}
	}

	<%--function validateRadUpload1(source, arguments) {
		arguments.IsValid = $find("<%= RadAsyncUpload.ClientID %>").validateExtensions();
	}
	--%>
</script>



<asp:UpdatePanel 
	ID="UDPtype" 
	UpdateMode="Conditional" 
	ChildrenAsTriggers="true"
	runat="server">
	
	<ContentTemplate>
		<asp:Label ID="LBtype_t" runat="server" CssClass="Titolo_campoSmall">Type:</asp:Label>

		<asp:RadioButtonList ID="RBLtype" runat="server" 
			CssClass="Testo_campoSmall" RepeatDirection="Horizontal"
			RepeatLayout="Flow" AutoPostBack="true">
		</asp:RadioButtonList>

		<asp:Label ID="LBplay" runat="server" CssClass="Titolo_campoSmall">(Play:</asp:Label>

		<asp:RadioButtonList ID="RBLplay" runat="server" CssClass="Testo_campoSmall" RepeatDirection="Horizontal"
			RepeatLayout="Flow">
			<asp:ListItem Value="False" Selected="True">Only on platform</asp:ListItem>
			<asp:ListItem Value="True">Allow also download</asp:ListItem>
		</asp:RadioButtonList>

		<asp:Literal ID="LTplayClosed" runat="server">):</asp:Literal>

	</ContentTemplate>

</asp:UpdatePanel>