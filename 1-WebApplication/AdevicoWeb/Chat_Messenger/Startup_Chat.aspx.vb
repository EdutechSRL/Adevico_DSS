Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.UCServices.Services_CHAT
Imports Comunita_OnLine.ModuloGenerale
Imports lm.ActionDataContract


Partial Public Class Startup_Chat
	Inherits PageBase



#Region "Private Property"
	Private _PageUtility As OLDpageUtility
	Private _Servizio As Services_CHAT
	Private _BaseUrl As String
#End Region

#Region "View property"
	Private ReadOnly Property CurrentService() As Services_CHAT
		Get
			If IsNothing(_Servizio) Then
				If isPortalCommunity Then
					_Servizio = Services_CHAT.Create
					_Servizio.Admin = (Me.TipoPersonaID = Main.TipoPersonaStandard.AdminSecondario OrElse Me.TipoPersonaID = Main.TipoPersonaStandard.SysAdmin)
					_Servizio.GestionePermessi = (Me.TipoPersonaID = Main.TipoPersonaStandard.AdminSecondario OrElse Me.TipoPersonaID = Main.TipoPersonaStandard.SysAdmin)
					_Servizio.Read = (Me.TipoPersonaID <> Main.TipoPersonaStandard.Guest)
					_Servizio.Write = (Me.TipoPersonaID <> Main.TipoPersonaStandard.Guest)
				ElseIf Me.isModalitaAmministrazione Then 'And Me.isUtenteAnonimo 
					_Servizio = New Services_CHAT(COL_Comunita.GetPermessiForServizioByCode(Main.TipoRuoloStandard.AdminComunità, Me.AmministrazioneComunitaID, Services_CHAT.Codex))
				Else
					_Servizio = Me.PageUtility.GetCurrentServices.Find(Services_CHAT.Codex)
					If IsNothing(_Servizio) Then
						_Servizio = Services_CHAT.Create
					End If
				End If
			End If
			Return _Servizio
		End Get
	End Property
	Public ReadOnly Property PageUtility() As OLDpageUtility
		Get
			If IsNothing(_PageUtility) Then
				_PageUtility = New OLDpageUtility(Me.Context)
			End If
			Return _PageUtility
		End Get
	End Property
#End Region

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

	End Sub

	Public Overrides ReadOnly Property AlwaysBind() As Boolean
		Get
			Return False
		End Get
	End Property
	Public Overrides Sub BindDati()
		If Me.AccessoSistema = False Then
			If Me.TipoPersonaID <> Main.TipoPersonaStandard.SysAdmin Then
				Me.BindNoPermessi()
				Me.RedirectToUrl("Chat_Messenger/Chat_Ext_Login.aspx")
				Exit Sub
			End If
		End If
		Me.MLVchat.SetActiveView(Me.VIWchat)
		Me.RegisterClientScriptBlock("autochat", "<script language='javascript'>window.open('Chat_Ext.aspx','win', 'menubar=no,location=no,toolbar=no,scrollbars=yes,resizable=yes,status=no,width=430,height=440');</script>")
	End Sub

	Public Overrides Sub BindNoPermessi()
		Me.MLVchat.SetActiveView(Me.VIWnoRegistri)
	End Sub

	Public Overrides Function HasPermessi() As Boolean
		Return (CurrentService.Read OrElse CurrentService.Admin OrElse CurrentService.Write)
	End Function

	Public Overrides Sub RegistraAccessoPagina()
		Me.PageUtility.AddAction(ActionType.None, Nothing, InteractionType.UserWithUser)
	End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Startup_Chat", "Chat_Messenger")
    End Sub

	Public Overrides Sub SetInternazionalizzazione()
		With MyBase.Resource
			.setLabel(Me.LBNopermessi)
            '.setLabel(Me.LBTitolo)
            Me.Master.ServiceTitle = .getValue("LBTitolo.text")
			.setLabel(Me.LBlink)
			.setLinkButton(Me.LNBapri, True, True)
		End With
	End Sub

	Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

	End Sub

	Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
		Get
			Return True
		End Get
	End Property
End Class


'<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">

'<html xmlns="http://www.w3.org/1999/xhtml" >
'<head runat="server">
'    <title>Chat</title>
'    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
'    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
'    <meta name=vs_defaultClientScript content="JavaScript"/>
'    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5"/>
'    <%--<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>--%>
' </head>

'  <body >

'    <form id="aspnetForm" method="post" runat="server">
'    <asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
'	   <div id="DVcontenitore" align="center">
'		  <div id="DVheader" style="width: 900px; text-align:left;" align="center">
'<%--		   <HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER>--%>
'		  </div>
'		  <div id="DVtitle" style="width: 900px; text-align:left;" class="RigaTitolo" align="center">
'			 <%--<asp:Label ID="LBTitolo" Runat="server">Chat</asp:Label>--%>
'		  </div>
'		  <div align="center" style="width: 900px;  padding-top:5px; ">

'		  </div>
'		  <div id="DVfooter" align="center" style="clear: both;">
'			 <%--<FOOTER:CTRLFOOTER id="Piede" runat="server"></FOOTER:CTRLFOOTER>--%>
'		  </div>
'	   </div>
'	</form>
'  </body>
'</html>