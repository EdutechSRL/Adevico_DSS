Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.Tiles.Presentation
Imports lm.Comol.Core.Dashboard.Domain
Public MustInherit Class TLeditPageBase
    Inherits PageBase
    Implements IViewEditPageBase


    '#Region "Context"
    '    Private _presenter As TileEditPagePresenter
    '    Protected Friend ReadOnly Property CurrentPresenter As TileEditPagePresenter
    '        Get
    '            If IsNothing(_presenter) Then
    '                _presenter = New TileEditPagePresenter(PageUtility.CurrentContext, Me)
    '            End If
    '            Return _presenter
    '        End Get
    '    End Property
    '#End Region

#Region "Implements"

#Region "Preload"
    Protected Friend ReadOnly Property PreloadIdTile As Long Implements IViewEditPageBase.PreloadIdTile
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("idTile")) AndAlso IsNumeric(Request.QueryString("idTile")) Then
                Return CLng(Request.QueryString("idTile"))
            Else
                Return 0
            End If
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadIdCommunity As Integer Implements IViewEditPageBase.PreloadIdCommunity
        Get
            If IsNumeric(Request.QueryString("idCommunity")) Then
                Return CInt(Request.QueryString("idCommunity"))
            Else
                Return -2
            End If
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadDashboardType As DashboardType Implements IViewEditPageBase.PreloadDashboardType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of DashboardType).GetByString(Request.QueryString("type"), DashboardType.Portal)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadIdDashboard As Long Implements IViewEditPageBase.PreloadIdDashboard
        Get
            If IsNumeric(Request.QueryString("fromIdDashboard")) Then
                Return CLng(Request.QueryString("fromIdDashboard"))
            Else
                Return 0
            End If
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadStep As WizardDashboardStep Implements IViewEditPageBase.PreloadStep
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of WizardDashboardStep).GetByString(Request.QueryString("step"), WizardDashboardStep.None)
        End Get
    End Property
#End Region

#Region "Settings"
    Protected Friend Property IdTile As Long Implements IViewEditPageBase.IdTile
        Get
            Return ViewStateOrDefault("IdTile", 0)
        End Get
        Set(value As Long)
            ViewState("IdTile") = value
        End Set
    End Property
    Protected Friend Property IdTileCommunity As Integer Implements IViewEditPageBase.IdTileCommunity
        Get
            Return ViewStateOrDefault("IdTileCommunity", -2)
        End Get
        Set(value As Integer)
            ViewState("IdTileCommunity") = value
        End Set
    End Property
    Protected Friend Property IdContainerCommunity As Integer Implements IViewEditPageBase.IdContainerCommunity
        Get
            Return ViewStateOrDefault("IdContainerCommunity", 0)
        End Get
        Set(value As Integer)
            ViewState("IdContainerCommunity") = value
        End Set
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
    '#Region "Internal"

    '#End Region

#Region "Inherits"
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Dashboard", "Modules", "Dashboard")
    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

#End Region

#Region "Implements"
    Protected Friend Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleDashboard.ActionType) Implements IViewEditPageBase.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, Nothing, InteractionType.UserWithLearningObject)
    End Sub
    Protected Friend Sub SendUserAction(idCommunity As Integer, idModule As Integer, idTile As Long, action As ModuleDashboard.ActionType) Implements IViewEditPageBase.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleDashboard.ObjectType.Tile, idTile.ToString), InteractionType.UserWithLearningObject)
    End Sub

#Region "Display"
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewEditPageBase.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, ModuleDashboard.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub
    Private Sub DisplayUnknownTile() Implements IViewEditPageBase.DisplayUnknownTile
        Dim oControl As UC_ActionMessages = GetMessageControl()
        If Not IsNothing(oControl) Then
            oControl.InitializeControl(Resource.getValue("DisplayUnknownTile"), Helpers.MessageType.alert)
        End If
    End Sub
 
#End Region
    Protected Friend MustOverride Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
    Protected Sub RedirectOnSessionTimeOut(ByVal destinationUrl As String, ByVal idCommunity As Integer)
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = destinationUrl

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If

        webPost.Redirect(dto)
    End Sub
    Private Sub SetBackUrl(url As String) Implements IViewEditPageBase.SetBackUrl
        Dim oHyperlink As HyperLink = GetBackUrlItemTop()
        If Not IsNothing(oHyperlink) Then
            oHyperlink.NavigateUrl = PageUtility.BaseUrl & url
            oHyperlink.Visible = True
        End If
        oHyperlink = GetBackUrlItemBottom()
        If Not IsNothing(oHyperlink) Then
            oHyperlink.NavigateUrl = PageUtility.BaseUrl & url
            oHyperlink.Visible = True
        End If
    End Sub

#End Region

#Region "Internal"
    Protected Friend MustOverride Function GetBackUrlItemBottom() As HyperLink
    Protected Friend MustOverride Function GetBackUrlItemTop() As HyperLink
    Protected Friend MustOverride Function GetMessageControl() As UC_ActionMessages

    Public Function GetBaseUrl() As String
        Return PageUtility.BaseUrl
    End Function
    Protected Friend Sub DisplayMessage(action As lm.Comol.Core.Dashboard.Domain.ModuleDashboard.ActionType)
        Dim oControl As UC_ActionMessages = GetMessageControl()
        If Not IsNothing(oControl) Then
            oControl.Visible = True
            oControl.InitializeControl(Resource.getValue("DisplayMessage.ModuleTile.ActionType." & action.ToString), GetMessageType(action))
        End If
    End Sub
    Protected Friend Sub DisplayMessage(errorType As lm.Comol.Core.BaseModules.Tiles.Business.ErrorMessageType)
        Dim oControl As UC_ActionMessages = GetMessageControl()
        If Not IsNothing(oControl) Then
            oControl.Visible = True
            oControl.InitializeControl(Resource.getValue("TileException.ErrorType." & errorType.ToString), GetMessageType(errorType))
        End If
    End Sub

    Private Function GetMessageType(action As ModuleDashboard.ActionType) As Helpers.MessageType
        Dim mType = lm.Comol.Core.DomainModel.Helpers.MessageType.error
        Select Case action
            Case ModuleDashboard.ActionType.TileDisable, ModuleDashboard.ActionType.TileEnable, ModuleDashboard.ActionType.TileVirtualDelete, ModuleDashboard.ActionType.TileVirtualUndelete, ModuleDashboard.ActionType.TileAdded, ModuleDashboard.ActionType.TileSaved, ModuleDashboard.ActionType.TileImageAssigned, ModuleDashboard.ActionType.TileImageUploaded
                mType = lm.Comol.Core.DomainModel.Helpers.MessageType.success
        End Select
        Return mType
    End Function
    Private Function GetMessageType(errorType As lm.Comol.Core.BaseModules.Tiles.Business.ErrorMessageType) As Helpers.MessageType
        Dim mType = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
        Select Case errorType
            Case lm.Comol.Core.BaseModules.Tiles.Business.ErrorMessageType.SavingTranslations
                mType = lm.Comol.Core.DomainModel.Helpers.MessageType.error
        End Select
        Return mType
    End Function
#End Region

  
End Class