Partial Public Class GenericMailSender : Inherits PageBase

#Region "Page Property"
	Public Overrides ReadOnly Property AlwaysBind() As Boolean
		Get
			Return False
		End Get
	End Property
	Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
		Get
			Return False
		End Get
	End Property
#End Region
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

	End Sub

	

	Public Overrides Sub BindDati()
		Dim oServiceMail As COL_BusinessLogic_v2.UCServices.Services_Mail = COL_BusinessLogic_v2.UCServices.Services_Mail.Create
		oServiceMail.Admin = True
		oServiceMail.SendMail = True

		Dim oServiceBase As New ServiceBase(0, oServiceMail.Codex, oServiceMail.PermessiAssociati)
		Dim oClause As New GenericClause(Of ServiceClause)
		oClause.OperatorForNextClause = OperatorType.OrCondition
		oClause.Clause = New ServiceClause(oServiceBase, OperatorType.OrCondition)

		Me.CTRLsearchCommunity.ServiceClauses = oClause
        Me.CTRLsearchCommunity.InitializeControl(-1)
		If Me.CTRLsearchCommunity.SelectedCommunitiesID.Count = 0 Then
			Me.WZRmail.ActiveStepIndex = 0
			Dim loNextButton As Button = Me.WZRmail.FindControl("StartNavigationTemplateContainerID").FindControl("StartNextButton")
			loNextButton.Enabled = False
		End If
	End Sub

	Public Overrides Sub BindNoPermessi()

	End Sub

	Public Overrides Function HasPermessi() As Boolean
		Return True
	End Function

	Public Overrides Sub RegistraAccessoPagina()

	End Sub

	Public Overrides Sub SetCultureSettings()

	End Sub

	Public Overrides Sub SetInternazionalizzazione()

	End Sub

	Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

	End Sub

	
End Class