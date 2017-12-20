Imports lm.ActionDataContract
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview.UC
Imports ObjectAction = PresentationLayer.WS_Actions.ObjectAction

Public MustInherit Class GLbaseControl
    Inherits BaseControl
    Implements IView_PageBaseControls

#Region "Preload"

    Protected Friend ReadOnly Property PreloadIdCommunity As Integer Implements IView_PageBaseControls.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property

    Protected Friend ReadOnly Property PreloadIdGlossary As Long Implements IView_PageBaseControls.PreloadIdGlossary
        Get
            If IsNumeric(Me.Request.QueryString("idGlossary")) Then
                Return CInt(Me.Request.QueryString("idGlossary"))
            Else
                Return 0
            End If
        End Get
    End Property

#End Region

#Region "Settings"

    Protected Friend Property IdCommunity As Integer Implements IView_PageBaseControls.IdCommunity
        Get
            Return ViewStateOrDefault("IdCommunity", 0)
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("IdCommunity") = value
        End Set
    End Property

    Protected Friend Property IdGlossary As Long Implements IView_PageBaseControls.IdGlossary
        Get
            Return ViewStateOrDefault("IdGlossary", 0)
        End Get
        Set(ByVal value As Long)
            Me.ViewState("IdGlossary") = value
        End Set
    End Property

    Protected Friend Property IdTerm As Long Implements IView_PageBaseControls.IdTerm
        Get
            Return ViewStateOrDefault("IdTerm", 0)
        End Get
        Set(ByVal value As Long)
            Me.ViewState("IdTerm") = value
        End Set
    End Property

    Public Property ForManage() As Boolean Implements IView_PageBaseControls.ForManage
        Get
            Return ViewStateOrDefault("ForManage", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("ForManage") = value
        End Set
    End Property

    Public Property ForManageEnabled() As Boolean Implements IView_PageBaseControls.ForManageEnabled
        Get
            Return ViewStateOrDefault("ForManageEnabled", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("ForManageEnabled") = value
        End Set
    End Property

#End Region

#Region "User Actions"

    Private _DisableUserAction As Boolean

    Protected Friend Property DisableUserAction As Boolean Implements IView_PageBaseControls.DisableUserAction
        Get
            Return _DisableUserAction
        End Get
        Set(ByVal value As Boolean)
            _DisableUserAction = value
        End Set
    End Property

    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleGlossaryNew.ActionType) _
        Implements IView_PageBaseControls.SendUserAction
        If Not DisableUserAction Then
            PageUtility.AddActionToModule(idCommunity, idModule, action, Nothing, InteractionType.UserWithLearningObject)
        End If
    End Sub

    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idGlossary As Long,
                               action As ModuleGlossaryNew.ActionType) _
        Implements IView_PageBaseControls.SendUserAction
        If Not DisableUserAction Then
            PageUtility.AddActionToModule(idCommunity, idModule, action,
                                          PageUtility.CreateObjectsList(idModule, ModuleGlossaryNew.ObjectType.Glossary, idGlossary),
                                          InteractionType.UserWithLearningObject)
        End If
    End Sub

    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idGlossary As Long, idItem As Long,
                               action As ModuleGlossaryNew.ActionType) _
        Implements IView_PageBaseControls.SendUserAction
        If Not DisableUserAction Then
            Dim oList As New List(Of ObjectAction)
            oList.Add(New ObjectAction With {.ObjectTypeId = ModuleGlossaryNew.ObjectType.Glossary, .ValueID = idGlossary, .ModuleID = idModule})
            oList.Add(New ObjectAction With {.ObjectTypeId = ModuleGlossaryNew.ObjectType.Term, .ValueID = idItem, .ModuleID = idModule})
            PageUtility.AddActionToModule(idCommunity, idModule, action,
                                          oList, InteractionType.UserWithLearningObject)
        End If
    End Sub

#End Region

#Region "Inherits"

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property

#End Region

#Region "Internal"

    Public Event SessionTimeout()

    Protected Friend Function GetDateTimeString(ByVal datetime As DateTime?, defaultString As String,
                                                Optional removeZero As Boolean = False) As String
        If datetime.HasValue Then
            Dim time As String = GetTimeToString(datetime, defaultString, removeZero)
            If String.IsNullOrEmpty(time) Then
                Return GetDateToString(datetime, defaultString)
            Else
                Return GetDateToString(datetime, defaultString) & " " & time
            End If
        Else
            Return defaultString
        End If
    End Function

    Protected Friend Function GetDateToString(ByVal datetime As DateTime?, defaultString As String) As String
        If datetime.HasValue Then
            Dim pattern As String = Resource.CultureInfo.DateTimeFormat.ShortDatePattern
            If (pattern.Contains("yyyy")) Then
                pattern = pattern.Replace("yyyy", "yy")
            End If
            Return datetime.Value.ToString(pattern)
        Else
            Return defaultString
        End If
    End Function

    Protected Friend Function GetTimeToString(ByVal datetime As DateTime?, defaultString As String,
                                              Optional removeZero As Boolean = False) As String
        If datetime.HasValue Then
            If removeZero AndAlso datetime.Value.Minute = 0 Then
                Return ""
            Else
                Return datetime.Value.ToString(Resource.CultureInfo.DateTimeFormat.ShortTimePattern)
            End If
        Else
            Return defaultString
        End If
    End Function

    Public Function GetBaseUrl() As String
        Return PageUtility.BaseUrl
    End Function

#End Region

#Region "Inherits"

    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("Glossary", "Modules", "Glossary")
    End Sub

#End Region

#Region "Implements"

    Private Sub DisplaySessionTimeout() Implements IView_PageBaseControls.DisplaySessionTimeout
        'LAscia che sia il padre a gestirsi la sessione chiusa,
        RaiseEvent SessionTimeout()
    End Sub
    'SICURO CHE VADA QUI ? 
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IView_PageBaseControls.DisplayNoPermission
        'SE intendi farlogestire controllo per controllo visto che da qui NON puoi accedere
        'ad elementi dell'UC per cui deiv aggiungere un metodo Protected MustOverride Sub SetDisplayNoPermission(idCommunity As Integer, idModule As Integer)()
        'da richiamre da qui
        ' OPPURE fare in riasevent specifico...
    End Sub

#End Region

    Public ReadOnly Property ApplicationUrlBase(Optional ByVal WithoutSSLfromConfig As Boolean = False) As String
        Get
            Dim Redirect As String = "http"

            If RequireSSL AndAlso Not WithoutSSLfromConfig Then
                Redirect &= "s://" & Me.Request.Url.Host & Me.BaseUrl
            Else
                Redirect &= "://" & Me.Request.Url.Host & Me.BaseUrl
            End If
            ApplicationUrlBase = Redirect
        End Get
    End Property

    Public ReadOnly Property RequireSSL() As Boolean
        Get
            Dim RichiediSSL As Boolean = False
            Try
                RequireSSL = SystemSettings.Login.isSSLrequired
            Catch ex As Exception
                RequireSSL = False
            End Try
        End Get
    End Property
End Class