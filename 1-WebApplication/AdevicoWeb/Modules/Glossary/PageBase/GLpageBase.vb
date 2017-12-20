Imports lm.ActionDataContract
Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview

Public MustInherit Class GLpageBase
    Inherits PageBase
    Implements IViewPageBase

#Region "Implements"

#Region "Preload"

    Protected Friend ReadOnly Property PreloadIdCommunity As Integer Implements IViewPageBase.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property

    Protected Friend ReadOnly Property PreloadFromType As Integer Implements IViewPageBase.PreloadFromType
        Get
            If IsNumeric(Me.Request.QueryString("fromType")) Then
                Return CInt(Me.Request.QueryString("fromType"))
            Else
                Return 0
            End If
        End Get
    End Property

    Public ReadOnly Property LoadfromCookies() As Boolean Implements IViewPageBase.LoadfromCookies
        Get
            Try
                Dim value As String = Me.Request.QueryString("LoadfromCookies")
                If Not String.IsNullOrWhiteSpace(value) Then
                    Return Convert.ToBoolean(value)
                End If
            Catch ex As Exception
            End Try
            Return False
        End Get
    End Property

    Public ReadOnly Property PreloadTermList() As String Implements IViewPageBase.PreloadTermList
        Get
            Try
                Dim value As String = Me.Request.QueryString("PreloadTermList")
                Return value
            Catch ex As Exception
            End Try
            Return String.Empty
        End Get
    End Property

    Public ReadOnly Property IdCookies() As String Implements IViewPageBase.IdCookies
        Get
            Try
                Dim value As String = Me.Request.QueryString("IdCookies")
                Return value
            Catch ex As Exception
            End Try
            Return String.Empty
        End Get
    End Property

    Protected Friend ReadOnly Property PreloadIdGlossary As Long Implements IViewPageBase.PreloadIdGlossary
        Get
            If IsNumeric(Me.Request.QueryString("idGlossary")) Then
                Return CInt(Me.Request.QueryString("idGlossary"))
            Else
                Return 0
            End If
        End Get
    End Property

    Protected Friend ReadOnly Property PreloadIdTerm As Long Implements IViewPageBase.PreloadIdTerm
        Get
            If IsNumeric(Me.Request.QueryString("idTerm")) Then
                Return CInt(Me.Request.QueryString("idTerm"))
            Else
                Return 0
            End If
        End Get
    End Property

    Protected Friend ReadOnly Property PreloadPageIndex As Int32 Implements IViewPageBase.PreloadPageIndex
        Get
            If IsNumeric(Me.Request.QueryString("Page")) Then
                Return CInt(Me.Request.QueryString("Page"))
            Else
                Return 0
            End If
        End Get
    End Property

    Protected Friend ReadOnly Property PreloadShowSave As Boolean Implements IViewPageBase.PreloadShowSave
        Get
            Try
                Dim value As String = Me.Request.QueryString("fromAdd")
                If Not String.IsNullOrWhiteSpace(value) Then
                    Return Convert.ToBoolean(value)
                End If
            Catch ex As Exception
            End Try
            Return false
        End Get
    End Property


    Protected Friend ReadOnly Property PreloadIsFirstOpen As Boolean Implements IViewPageBase.IsFirstOpen
        Get
            Try
                Dim value As String = Me.Request.QueryString("isFirstOpen")
                If Not String.IsNullOrWhiteSpace(value) Then
                    Return Convert.ToBoolean(value)
                End If
            Catch ex As Exception
            End Try
            Return True
        End Get
    End Property

    Protected Friend ReadOnly Property PreloadIsFromListGlossary As Boolean Implements IViewPageBase.IsFromListGlossary
        Get
            Try
                Dim value As String = Me.Request.QueryString("isFromListGlossary")
                If Not String.IsNullOrWhiteSpace(value) Then
                    Return Convert.ToBoolean(value)
                End If
            Catch ex As Exception
            End Try
            Return True
        End Get
    End Property

    Protected Friend ReadOnly Property PreloadForManagement As Boolean Implements IViewPageBase.PreloadForManagement
        Get
            Try
                Dim value As String = Me.Request.QueryString("manage")
                If Not String.IsNullOrWhiteSpace(value) Then
                    Return Convert.ToBoolean(value)
                End If
            Catch ex As Exception
            End Try
            Return False
        End Get
    End Property

    Protected Friend ReadOnly Property PreloadFromViewMap As Boolean Implements IViewPageBase.PreloadFromViewMap
        Get
            Try
                Dim value As String = Me.Request.QueryString("fromViewMap")
                If Not String.IsNullOrWhiteSpace(value) Then
                    Return Convert.ToBoolean(value)
                End If
            Catch ex As Exception
            End Try
            Return False
        End Get
    End Property

#End Region

#Region "Settings"

    Protected Friend Property IdCommunity As Integer Implements IViewPageBase.IdCommunity
        Get
            Return ViewStateOrDefault("IdCommunity", 0)
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("IdCommunity") = value
        End Set
    End Property

    Protected Friend Property IdGlossary As Long Implements IViewPageBase.IdGlossary
        Get
            Return ViewStateOrDefault("IdGlossary", 0)
        End Get
        Set(ByVal value As Long)
            Me.ViewState("IdGlossary") = value
        End Set
    End Property

    Public Property IdTerm() As Long Implements IViewPageBase.IdTerm
        Get
            Return ViewStateOrDefault("IdTerm", 0)
        End Get
        Set(ByVal value As Long)
            Me.ViewState("IdTerm") = value
        End Set
    End Property

    Public Property FromViewMap() As Boolean Implements IViewPageBase.FromViewMap
        Get
            Return ViewStateOrDefault("FromViewMap", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("FromViewMap") = value
        End Set
    End Property

    Public Property FromViewGlossary() As Boolean Implements IViewPageBase.FromViewGlossary
        Get
            Return ViewStateOrDefault("FromViewGlossary", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("FromViewGlossary") = value
        End Set
    End Property

#End Region

#Region "Filters"

    Protected Friend ReadOnly Property SearchString As String Implements IViewPageBase.SearchString
        Get
            Return HttpUtility.HtmlEncode(Me.Request.QueryString("SearchString"))
        End Get
    End Property

    Protected Friend ReadOnly Property LemmaString As String Implements IViewPageBase.LemmaString
        Get
            Return HttpUtility.HtmlEncode(Me.Request.QueryString("LemmaString"))
        End Get
    End Property

    Protected Friend ReadOnly Property LemmaContentString As String Implements IViewPageBase.LemmaContentString
        Get
            Return HttpUtility.HtmlEncode(Me.Request.QueryString("LemmaContentString"))
        End Get
    End Property

    Protected Friend ReadOnly Property LemmaSearchType As Integer Implements IViewPageBase.LemmaSearchType
        Get
            If IsNumeric(Me.Request.QueryString("LemmaSearchType")) Then
                Return CInt(Me.Request.QueryString("LemmaSearchType"))
            Else
                Return 0
            End If
        End Get
    End Property

    Protected Friend ReadOnly Property LemmaVisibilityType As Integer Implements IViewPageBase.LemmaVisibilityType
        Get
            If IsNumeric(Me.Request.QueryString("LemmaVisibilityType")) Then
                Return CInt(Me.Request.QueryString("LemmaVisibilityType"))
            Else
                Return 0
            End If
        End Get
    End Property

#End Region

#End Region

#Region "Inherits"

    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property

#End Region

#Region "Inherits"

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()
    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("Glossary", "Modules", "Glossary")
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)
    End Sub

#End Region

#Region "Implements"

    Public Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleGlossaryNew.ActionType) Implements IViewPageBase.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, Nothing, InteractionType.UserWithLearningObject)
    End Sub

    Public Sub SendUserAction(idCommunity As Integer, idModule As Integer, idItem As Long, type As ModuleGlossaryNew.ObjectType, action As ModuleGlossaryNew.ActionType) Implements IViewPageBase.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, type, idItem), InteractionType.UserWithLearningObject)
    End Sub

#Region "Display"

    Public Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission
        PageUtility.AddActionToModule(idCommunity, idModule, ModuleGlossaryNew.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        BindNoPermessi()
    End Sub

#End Region

    Protected Friend MustOverride Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout

    Protected Sub RedirectOnSessionTimeOut(ByVal destinationUrl As String, ByVal idCommunity As Integer,
                                           Optional ByVal display As dtoExpiredAccessUrl.DisplayMode =
                                              dtoExpiredAccessUrl.DisplayMode.SameWindow)
        Dim webPost As New LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New dtoExpiredAccessUrl()
        dto.Display = dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = destinationUrl

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If

        webPost.Redirect(dto)
    End Sub

#End Region

#Region "Internal"

    Public Function GetBaseUrl() As String
        Return PageUtility.BaseUrl
    End Function

#End Region
End Class

Public Module GlossaryHelper
    '<Extension()>
    'Public Sub SetLanguage(ByVal control As HyperLink, ByVal resource As ResourceManager)
    '    resource.setHyperLink(control, True, True, True, True)
    'End Sub

    '<Extension()>
    'Public Sub SetLanguageWithConfirm(ByVal control As LinkButton, ByVal resource As ResourceManager)
    '    resource.setLinkButton(control, True, True, True, True)
    'End Sub
End Module