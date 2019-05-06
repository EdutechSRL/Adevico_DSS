Imports lm.Comol.Core.BaseModules.Web
Imports lm.Comol.Core.BaseModules.Web.Controls
Imports lm.Comol.Modules.Standard.Skin.Domain
Imports lm.Comol.Modules.Standard.Skin.Presentation
Public Class AddSkin
    Inherits PageBase
    Implements IViewModuleSkinAdd


#Region "Context"
    Private _BaseUrl As String
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As ModuleSkinAddPresenter

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
    Private ReadOnly Property CurrentPresenter() As ModuleSkinAddPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ModuleSkinAddPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property SkinType As SkinType Implements IViewModuleSkinAdd.SkinType
        Get
            Return ViewStateOrDefault("SkinType", SkinType.NotAssigned)
        End Get
        Set(value As SkinType)
            ViewState("SkinType") = value
        End Set
    End Property
    Private Property BackUrl As String Implements IViewModuleSkinAdd.BackUrl
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
    Private Property Source As lm.Comol.Core.DomainModel.ModuleObject Implements IViewModuleSkinAdd.Source
        Get
            Return ViewStateOrDefault("Source", New lm.Comol.Core.DomainModel.ModuleObject())
        End Get
        Set(value As lm.Comol.Core.DomainModel.ModuleObject)
            ViewState("Source") = value
        End Set
    End Property
    Private ReadOnly Property PreloadedIdCommunity As Integer Implements IViewModuleSkinAdd.PreloadedIdCommunity
        Get
            If IsNumeric(Request.QueryString("idCommunity")) Then
                Return CInt(Request.QueryString("idCommunity"))
            Else
                Return CInt(0)
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedIdItem As Long Implements IViewModuleSkinAdd.PreloadedIdItem
        Get
            If IsNumeric(Request.QueryString("idModuleItem")) Then
                Return CLng(Request.QueryString("idModuleItem"))
            Else
                Return CLng(0)
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedSkinType As SkinType Implements IViewModuleSkinAdd.PreloadedSkinType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SkinType).GetByString(Request.QueryString("type"), SkinType.NotAssigned)
        End Get
    End Property
    Private ReadOnly Property PreloadedIdModule As Integer Implements IViewModuleSkinAdd.PreloadedIdModule
        Get
            If IsNumeric(Request.QueryString("idModule")) Then
                Return CInt(Request.QueryString("idModule"))
            Else
                Return CInt(0)
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedIdItemType As Integer Implements IViewModuleSkinAdd.PreloadedIdItemType
        Get
            If IsNumeric(Request.QueryString("idItemType")) Then
                Return CInt(Request.QueryString("idItemType"))
            Else
                Return CInt(0)
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedBackUrl As String Implements IViewModuleSkinAdd.PreloadedBackUrl
        Get
            Return Request.QueryString("BackUrl")
        End Get
    End Property
    Private WriteOnly Property AllowAdd As Boolean Implements IViewModuleSkinAdd.AllowAdd
        Set(value As Boolean)
            Me.BTNaddSkin.Visible = value
        End Set
    End Property
    Private Property isInitialized As Boolean Implements IViewModuleSkinAdd.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public Property IdSkin As Long Implements IViewModuleSkinBase.IdSkin
        Get
            Return ViewStateOrDefault("IdSkin", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdSkin") = value
        End Set
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
        If Not Page.IsPostBack Then
            Me.CurrentPresenter.InitView(PreloadedIdModule, PreloadedIdCommunity, PreloadedIdItem, PreloadedIdItemType, PreloadedSkinType)
        End If
    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_SkinModuleManagement", "Skin")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(LBskinName_t)
            .setHyperLink(HYPback, True, True)
            .setButton(BTNaddSkin, True, , , True)
            Master.ServiceTitle = .getValue("ServiceTitle.Add.text")
            Master.ServiceTitleToolTip = .getValue("ServiceTitle.Add.ToolTip")
        End With
    End Sub
    Public Overrides Sub BindNoPermessi()
        Me.Master.ShowNoPermission = True
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub



    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Public Sub DisplayForm(item As lm.Comol.Core.DomainModel.ModuleObject, allowAdd As Boolean) Implements IViewModuleSkinAdd.DisplayForm
        Me.MLVaddSkin.SetActiveView(VIWdata)
        Me.Source = item
        Me.AllowAdd = allowAdd
        Me.TXBskinName.Text = ""
        LBdisplayInfo.Text = Resource.getValue("LBdisplayInfo." & item.ServiceCode)
    End Sub
    Public Sub DisplayNoPermission() Implements IViewModuleSkinBase.DisplayNoPermission
        Me.MLVaddSkin.SetActiveView(VIWempty)
        LBskinMessage.Text = Resource.getValue("DisplayNoPermission.Add")
    End Sub

    Public Sub DisplaySessionTimeout() Implements IViewModuleSkinBase.DisplaySessionTimeout
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()

        If PreloadedIdItem > 0 Then
            dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
            dto.DestinationUrl = RootObject.AddModuleSkin(PreloadedIdModule, PreloadedIdCommunity, PreloadedIdModule, PreloadedIdItemType, Server.UrlEncode(PreloadedBackUrl))
            dto.IdCommunity = PreloadedIdCommunity
        Else
            dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.None
        End If
        webPost.Redirect(dto)
    End Sub
    Public Sub RedirectToEdit() Implements IViewModuleSkinAdd.RedirectToEdit
        PageUtility.RedirectToUrl(RootObject.EditModuleSkin(IdSkin, Source.ServiceID, Source.CommunityID, Source.ObjectLongID, Source.ObjectTypeID, Server.UrlEncode(BackUrl)))
    End Sub
    Public Function HasPermissionForItem(item As lm.Comol.Core.DomainModel.ModuleObject) As Boolean Implements IViewModuleSkinBase.HasPermissionForItem
        'Dim retval As Boolean
        'Dim oSender As PermissionService.IServicePermission = Nothing
        'Try
        '    oSender = New PermissionService.ServicePermissionClient
        '    retval = oSender.AllowStandardAction(lm.Comol.Core.DomainModel.StandardActionType.Edit, item, Nothing, CurrentContext.UserContext.CurrentUserID)

        '    If Not IsNothing(oSender) Then
        '        Dim service As System.ServiceModel.ClientBase(Of PermissionService.IServicePermission) = DirectCast(oSender, System.ServiceModel.ClientBase(Of PermissionService.IServicePermission))
        '        service.Close()
        '        service = Nothing
        '    End If
        'Catch ex As Exception
        '    If Not IsNothing(oSender) Then
        '        Dim service As System.ServiceModel.ClientBase(Of PermissionService.IServicePermission) = DirectCast(oSender, System.ServiceModel.ClientBase(Of PermissionService.IServicePermission))
        '        service.Abort()
        '        service = Nothing
        '    End If
        'End Try
        'Return retval

        Dim retval As Boolean = False



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

        If Not retval Then

            Dim currentTypeId As Integer = CurrentPresenter.UserContext.UserTypeID

            If Not IsNothing(SystemSettings.SkinSettings.PersonTypeIds) Then
                retval = SystemSettings.SkinSettings.PersonTypeIds.Any(Function(t) t = currentTypeId)
            End If

        End If

        If Not retval Then

            Dim currentUserId As Integer = CurrentPresenter.UserContext.CurrentUserID

            If Not IsNothing(SystemSettings.SkinSettings.PersonTypeIds) Then
                retval = SystemSettings.SkinSettings.PersonTypeIds.Any(Function(p) p = currentUserId)
            End If

        End If

        Return retval
    End Function
#End Region

    Private Sub BTNaddSkin_Click(sender As Object, e As System.EventArgs) Handles BTNaddSkin.Click
        If Me.CurrentPresenter.AddSkin(Me.TXBskinName.Text, MyBase.SystemSettings.SkinSettings.SkinPhisicalPath) Then
            RedirectToEdit()
        ElseIf Not String.IsNullOrEmpty(BackUrl) Then
            PageUtility.RedirectToUrl(BackUrl)
        End If
    End Sub
End Class