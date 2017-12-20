Imports COL_BusinessLogic_v2.Comol.Configurations

Partial Public Class ShowErrors
	Inherits PageBase

    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext


    Private ReadOnly Property InternalDisplay() As Boolean
        Get
            If String.IsNullOrEmpty(Me.Request.QueryString("InternalDisplay")) Then
                Return False
            Else
                Return IIf(LCase(Me.Request.QueryString("InternalDisplay")) = "true", True, False)
            End If
        End Get
    End Property
    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentUserContext}
            End If
            Return _CurrentContext
        End Get
    End Property

    Private Sub CommunityRepositoryItemError_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        'If Me.CurrentContext.UserContext.isAnonymous Then
        '    Me.MasterPageFile = Me.Request.ApplicationPath & "/Registrazione.master"
        'ElseIf InternalDisplay Then
        '    Me.MasterPageFile = Me.Request.ApplicationPath & "/AjaxPopup.master"
        'Else
        '    Me.MasterPageFile = Me.Request.ApplicationPath & "/AjaxPortal.master"
        'End If
    End Sub

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

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Page.IsPostBack = False Then
			BindDati()
		End If
	End Sub

	Public Overrides Sub BindDati()
        Dim ErrorType As ErrorSettings.ErrorType = ErrorSettings.ErrorType.None

        If Request.QueryString.HasKeys Then
            Try
                Dim ErrorTypeID As String = Me.DecryptQueryString("ErroreID", SecretKeyUtil.EncType.Altro)
                If ErrorTypeID <> "" Then
                    ErrorType = CInt(ErrorTypeID)
                Else
                    ErrorType = ErrorSettings.ErrorType.None
                End If
            Catch ex As Exception
                ErrorType = ErrorSettings.ErrorType.None
            End Try

        End If
        Me.LBmessage.Text = Me.Resource.getValue("ErrorType." & ErrorType)
		

	End Sub

	Public Overrides Function HasPermessi() As Boolean
		Return True
	End Function

	Public Overrides Sub SetCultureSettings()
		MyBase.SetCulture("pg_Errori", "Root")
	End Sub

	Public Overrides Sub SetInternazionalizzazione()
		With Resource

		End With
	End Sub

#Region "Non usati"
	Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

	End Sub
	Public Overrides Sub RegistraAccessoPagina()

	End Sub
	Public Overrides Sub BindNoPermessi()
        If TypeOf Me.Master Is AjaxPortal Then
            DirectCast(Me.Master, AjaxPortal).ShowNoPermission = True
            'ElseIf TypeOf Me.Master Is Registrazione Then
            '    DirectCast(Me.Master, Registrazione).ShowNoPermission = True
        ElseIf TypeOf Me.Master Is AjaxPopup Then
            DirectCast(Me.Master, AjaxPopup).ShowNoPermission = True
        End If
	End Sub
#End Region


End Class