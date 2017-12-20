Imports lm.Comol.Core.BaseModules.Web
Imports lm.Comol.Core.BaseModules.Web.Controls
Imports lm.Comol.Modules.Standard.Skin.Domain
Imports lm.Comol.Modules.Standard.Skin.Presentation
Public Class DeleteSkin
    Inherits PageBase
    Implements IViewModuleSkinDelete

#Region "Context"
    Private _BaseUrl As String
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As ModuleSkinDeletePresenter

    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentUserContext, .DataContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Private ReadOnly Property CurrentPresenter() As ModuleSkinDeletePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ModuleSkinDeletePresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property SkinType As SkinType Implements IViewModuleSkinDelete.SkinType
        Get
            Return ViewStateOrDefault("SkinType", SkinType.NotAssigned)
        End Get
        Set(value As SkinType)
            ViewState("SkinType") = value
        End Set
    End Property
    Private Property BackUrl As String Implements IViewModuleSkinDelete.BackUrl
        Get
            Return ViewStateOrDefault("BackUrl", "")
        End Get
        Set(value As String)
            If String.IsNullOrEmpty(value) Then
                Dim item As lm.Comol.Core.DomainModel.ModuleObject = Me.Source
                value = RootObject.ManagementModuleSkin(item.ServiceID, item.CommunityID, item.ObjectLongID, item.ObjectTypeID, "")
            End If
            ViewState("BackUrl") = value
            value = BaseUrl & value
            value = value.Replace("//", "/")
            HYPback.NavigateUrl = value
        End Set
    End Property
    Private Property Source As lm.Comol.Core.DomainModel.ModuleObject Implements IViewModuleSkinDelete.Source
        Get
            Return ViewStateOrDefault("Source", New lm.Comol.Core.DomainModel.ModuleObject())
        End Get
        Set(value As lm.Comol.Core.DomainModel.ModuleObject)
            ViewState("Source") = value
        End Set
    End Property
    Private ReadOnly Property PreloadedIdCommunity As Integer Implements IViewModuleSkinDelete.PreloadedIdCommunity
        Get
            If IsNumeric(Request.QueryString("idCommunity")) Then
                Return CInt(Request.QueryString("idCommunity"))
            Else
                Return CInt(0)
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedIdItem As Long Implements IViewModuleSkinDelete.PreloadedIdItem
        Get
            If IsNumeric(Request.QueryString("idModuleItem")) Then
                Return CLng(Request.QueryString("idModuleItem"))
            Else
                Return CLng(0)
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedIdModule As Integer Implements IViewModuleSkinDelete.PreloadedIdModule
        Get
            If IsNumeric(Request.QueryString("idModule")) Then
                Return CInt(Request.QueryString("idModule"))
            Else
                Return CInt(0)
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedIdItemType As Integer Implements IViewModuleSkinDelete.PreloadedIdItemType
        Get
            If IsNumeric(Request.QueryString("idItemType")) Then
                Return CInt(Request.QueryString("idItemType"))
            Else
                Return CInt(0)
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedIdSkin As Long Implements IViewModuleSkinDelete.PreloadedIdSkin
        Get
            If IsNumeric(Request.QueryString("idSkin")) Then
                Return CLng(Request.QueryString("idSkin"))
            Else
                Return CLng(0)
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedSkinType As SkinType Implements IViewModuleSkinDelete.PreloadedSkinType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SkinType).GetByString(Request.QueryString("type"), SkinType.NotAssigned)
        End Get
    End Property
    Private ReadOnly Property PreloadedBackUrl As String Implements IViewModuleSkinDelete.PreloadedBackUrl
        Get
            Return Request.QueryString("BackUrl")
        End Get
    End Property
    Private WriteOnly Property AllowDelete As Boolean Implements IViewModuleSkinDelete.AllowDelete
        Set(value As Boolean)
            Me.BTNdeleteSkin.Visible = value
        End Set
    End Property
    Private Property isInitialized As Boolean Implements IViewModuleSkinDelete.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Private Property IdSkin As Long Implements IViewModuleSkinBase.IdSkin
        Get
            Return ViewStateOrDefault("IdSkin", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdSkin") = value
        End Set
    End Property
    Private ReadOnly Property FullSkinManagement As Boolean Implements IViewModuleSkinDelete.FullSkinManagement
        Get
            Dim SkinSettings As SkinSettings = MyBase.SystemSettings.SkinSettings

            Return (SkinSettings.PersonTypeIds.Contains(CurrentContext.UserContext.UserTypeID) OrElse _
                SkinSettings.PersonsIds.Contains(CurrentContext.UserContext.CurrentUserID))
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
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.CurrentPresenter.InitView(PreloadedIdSkin, PreloadedIdModule, PreloadedIdCommunity, PreloadedIdItem, PreloadedIdItemType)
    End Sub
    Public Overrides Sub BindNoPermessi()
        Me.Master.ShowNoPermission = True
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_SkinModuleManagement", "Skin")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setHyperLink(HYPback, True, True)
            .setButton(BTNdeleteSkin, True, , , True)

            Master.ServiceTitle = .getValue("ServiceTitle.Delete.text")
            Master.ServiceTitleToolTip = .getValue("ServiceTitle.Delete.ToolTip")
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub DisplayUnknownItem() Implements IViewModuleSkinDelete.DisplayUnknownItem
        Me.MLVeditSkin.SetActiveView(VIWempty)
        LBskinMessage.Text = Resource.getValue("DisplayUnknownItem")
        Me.BTNdeleteSkin.Visible = False
    End Sub
    Private Sub LoadSkinInfo(idSkin As Long, name As String, allowEdit As Boolean) Implements IViewModuleSkinDelete.LoadSkinInfo
        Me.MLVeditSkin.SetActiveView(VIWdata)
        Me.LBinfoDelete.Text = String.Format(Resource.getValue("LBinfoDelete." & SkinType.ToString), name)
    End Sub
    Private Sub DisplayNoPermission() Implements IViewModuleSkinDelete.DisplayNoPermission
        Me.MLVeditSkin.SetActiveView(VIWempty)
        Me.BTNdeleteSkin.Visible = False
        LBskinMessage.Text = Resource.getValue("DisplayNoPermission.Delete")
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewModuleSkinBase.DisplaySessionTimeout
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()

        If PreloadedIdItem > 0 Then
            dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
            dto.DestinationUrl = RootObject.DeleteModuleSkin(PreloadedIdSkin, PreloadedIdModule, PreloadedIdCommunity, PreloadedIdModule, PreloadedIdItemType, PreloadedBackUrl)
            dto.IdCommunity = PreloadedIdCommunity
        Else
            dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.None
        End If
        webPost.Redirect(dto)
    End Sub

    Private Function HasPermissionForItem(item As lm.Comol.Core.DomainModel.ModuleObject) As Boolean Implements IViewModuleSkinBase.HasPermissionForItem
        Dim retval As Boolean
        Dim oSender As PermissionService.IServicePermission = Nothing
        Try
            oSender = New PermissionService.ServicePermissionClient
            retval = oSender.AllowStandardAction(lm.Comol.Core.DomainModel.StandardActionType.Edit, item, Nothing, CurrentContext.UserContext.CurrentUserID)

            If Not IsNothing(oSender) Then
                Dim service As System.ServiceModel.ClientBase(Of PermissionService.IServicePermission) = DirectCast(oSender, System.ServiceModel.ClientBase(Of PermissionService.IServicePermission))
                service.Close()
                service = Nothing
            End If
        Catch ex As Exception
            If Not IsNothing(oSender) Then
                Dim service As System.ServiceModel.ClientBase(Of PermissionService.IServicePermission) = DirectCast(oSender, System.ServiceModel.ClientBase(Of PermissionService.IServicePermission))
                service.Abort()
                service = Nothing
            End If
        End Try
        Return retval
    End Function
#End Region

    Private Sub BTNdeleteSkin_Click(sender As Object, e As System.EventArgs) Handles BTNdeleteSkin.Click
        If Me.CurrentPresenter.DeleteSkin(IdSkin, MyBase.SystemSettings.SkinSettings.SkinPhisicalPath) Then
            If Not String.IsNullOrEmpty(Me.BackUrl) Then
                PageUtility.RedirectToUrl(BackUrl)
            End If
        End If
    End Sub

End Class