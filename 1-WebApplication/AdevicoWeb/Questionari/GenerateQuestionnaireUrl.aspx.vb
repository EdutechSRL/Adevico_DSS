Imports COL_Questionario

Public Class GenerateQuestionnaireUrl
    Inherits PageBase
    Implements IViewGenerateQuesionnaireUrl

#Region "Context"
    Private _presenter As GenerateQuestionnaireUrlPresenter
    Public ReadOnly Property CurrentPresenter As GenerateQuestionnaireUrlPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New GenerateQuestionnaireUrlPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
#End Region

#Region "Inherits"
    Private ReadOnly Property PreloadedIdQuestionnnaire As Integer Implements IViewGenerateQuesionnaireUrl.PreloadedIdQuestionnnaire
        Get
            If IsNumeric(Request.QueryString("idQ")) Then
                Return CInt(Request.QueryString("idQ"))
            Else
                Return 0
            End If
        End Get
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return True
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.CurrentPresenter.InitView()
    End Sub
    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_GenerateQuestionnaireUrl", "Questionari")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        SetCultureSettings()
        With Me.Resource
            Master.ServiceTitle = .getValue("ServiceTitle")
            Master.ServiceNopermission = .getValue("ServiceNopermission")
        End With
    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region ""
    Private Sub DisplayAccessError(qName As String) Implements IViewGenerateQuesionnaireUrl.DisplayAccessError
        Me.MLVgenerate.SetActiveView(VIWinfo)
        LBmessage.Text = String.Format(Resource.getValue("DisplayAccessError"), qName)
    End Sub
    Private Sub DisplayAccessError(qName As String, uName As String, surname As String) Implements IViewGenerateQuesionnaireUrl.DisplayAccessError
        Me.MLVgenerate.SetActiveView(VIWinfo)
        LBmessage.Text = String.Format(Resource.getValue("DisplayAccessErrorU"), qName, uName, surname)
    End Sub
    Private Sub DisplayNoPermission() Implements IViewGenerateQuesionnaireUrl.DisplayNoPermission
        Me.MLVgenerate.SetActiveView(VIWinfo)
        LBmessage.Text = String.Format(Resource.getValue("DisplayNoPermission"))
    End Sub
    Private Sub DisplayUnknownQuesionnaire() Implements IViewGenerateQuesionnaireUrl.DisplayUnknownQuesionnaire
        Me.MLVgenerate.SetActiveView(VIWinfo)
        LBmessage.Text = String.Format(Resource.getValue("DisplayUnknownQuesionnaire"))
    End Sub
    Private Sub DisplaySessionTimeout(ByVal idCommunity As Integer) Implements IViewGenerateQuesionnaireUrl.DisplaySessionTimeout
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = RootObject.GenerateQuestionnaireUrl(PreloadedIdQuestionnnaire)
        dto.IdCommunity = idCommunity
        webPost.Redirect(dto)
    End Sub
    Private Sub GotoQuestionnaire(idQuestionnaire As Integer, idUser As Integer) Implements COL_Questionario.IViewGenerateQuesionnaireUrl.GotoQuestionnaire
        Response.Redirect(Me.EncryptedUrl(COL_Questionario.RootObject.compileUrl, "idq=" & idQuestionnaire, SecretKeyUtil.EncType.Questionario))
        ' Response.Redirect(Me.EncryptedUrl(COL_Questionario.RootObject.compileUrlUI, "idq=" & idQuestionnaire & "&idu=" & idUser & "&idl=" & LinguaID, SecretKeyUtil.EncType.Questionario))
    End Sub
#End Region

End Class