Imports lm.Comol.Core.BaseModules.Web
Imports lm.Comol.Core.BaseModules.Web.Controls
Imports lm.Comol.Modules.Standard.Skin.Domain
Imports lm.Comol.Modules.Standard.Skin.Presentation

Public Class EditSkin
    Inherits PageBase
    Implements IViewModuleSkinEdit

#Region "Context"
    Private _BaseUrl As String
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As ModuleSkinEditPresenter

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
    Private ReadOnly Property CurrentPresenter() As ModuleSkinEditPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ModuleSkinEditPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property SkinType As SkinType Implements IViewModuleSkinEdit.SkinType
        Get
            Return ViewStateOrDefault("SkinType", SkinType.NotAssigned)
        End Get
        Set(value As SkinType)
            ViewState("SkinType") = value
        End Set
    End Property
    Private Property BackUrl As String Implements IViewModuleSkinEdit.BackUrl
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
            HYPback.NavigateUrl = value
        End Set
    End Property
    Private Property Source As lm.Comol.Core.DomainModel.ModuleObject Implements IViewModuleSkinEdit.Source
        Get
            Return ViewStateOrDefault("Source", New lm.Comol.Core.DomainModel.ModuleObject())
        End Get
        Set(value As lm.Comol.Core.DomainModel.ModuleObject)
            ViewState("Source") = value
        End Set
    End Property
    Private ReadOnly Property PreloadedIdCommunity As Integer Implements IViewModuleSkinEdit.PreloadedIdCommunity
        Get
            If IsNumeric(Request.QueryString("idCommunity")) Then
                Return CInt(Request.QueryString("idCommunity"))
            Else
                Return CInt(0)
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedIdItem As Long Implements IViewModuleSkinEdit.PreloadedIdItem
        Get
            If IsNumeric(Request.QueryString("idModuleItem")) Then
                Return CLng(Request.QueryString("idModuleItem"))
            Else
                Return CLng(0)
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedIdModule As Integer Implements IViewModuleSkinEdit.PreloadedIdModule
        Get
            If IsNumeric(Request.QueryString("idModule")) Then
                Return CInt(Request.QueryString("idModule"))
            Else
                Return CInt(0)
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedIdItemType As Integer Implements IViewModuleSkinEdit.PreloadedIdItemType
        Get
            If IsNumeric(Request.QueryString("idItemType")) Then
                Return CInt(Request.QueryString("idItemType"))
            Else
                Return CInt(0)
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedIdSkin As Long Implements IViewModuleSkinEdit.PreloadedIdSkin
        Get
            If IsNumeric(Request.QueryString("idSkin")) Then
                Return CLng(Request.QueryString("idSkin"))
            Else
                Return CLng(0)
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedSkinType As SkinType Implements IViewModuleSkinEdit.PreloadedSkinType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SkinType).GetByString(Request.QueryString("type"), SkinType.NotAssigned)
        End Get
    End Property
    Private ReadOnly Property PreloadedBackUrl As String Implements IViewModuleSkinEdit.PreloadedBackUrl
        Get
            Return Request.QueryString("BackUrl")
        End Get
    End Property
    Private WriteOnly Property AllowEdit As Boolean Implements IViewModuleSkinEdit.AllowEdit
        Set(value As Boolean)
            Me.BTNsaveSkinName.Visible = value
            Me.TXBskinName.ReadOnly = Not value
            'Me.CTRLtemplate.AllowEdit = value
            CTRLfootText.AllowEdit = value
            CTRLfootLogo.AllowEdit = value
        End Set
    End Property
    Private Property isInitialized As Boolean Implements IViewModuleSkinEdit.isInitialized
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
    Private ReadOnly Property FullSkinManagement As Boolean Implements IViewModuleSkinEdit.FullSkinManagement
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
            .setLabel(LBskinName_t)
            .setHyperLink(HYPback, True, True)
            .setButton(BTNsaveSkinName, True, , , True)

            Master.ServiceTitle = .getValue("ServiceTitle.Edit.text")
            Master.ServiceTitleToolTip = .getValue("ServiceTitle.Edit.ToolTip")

            For Each Tab As Telerik.Web.UI.RadTab In Me.TBSskinEdit.Tabs
                .setRadTab(Tab, True)
            Next


        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub DisplayUnknownItem() Implements IViewModuleSkinEdit.DisplayUnknownItem
        Me.MLVeditSkin.SetActiveView(VIWempty)
        LBskinMessage.Text = Resource.getValue("DisplayUnknownItem")
        Me.BTNsaveSkinName.Visible = False
    End Sub
    Private Sub LoadSkinInfo(idSkin As Long, name As String, allowEdit As Boolean, ByVal OverrideVoidFooterLogos As Boolean) Implements IViewModuleSkinEdit.LoadSkinInfo
        Me.MLVeditSkin.SetActiveView(VIWdata)
        Me.TXBskinName.Text = name
        Dim tab As Telerik.Web.UI.RadTab = Me.TBSskinEdit.FindTabByValue("SkinView." & SkinView.HeaderLogo.ToString)
        If Not IsNothing(tab) Then
            tab.Selected = True
        End If
        Me.Show(SkinView.HeaderLogo)

        Me.CTRLmainLogo.InitializeControl(idSkin, allowEdit)
        Me.CTRLfootLogo.InitializeControl(idSkin, allowEdit)
        Me.CTRLfootLogo.OverrideVoid = OverrideVoidFooterLogos
        Me.CTRLfootText.InitializeControl(idSkin, allowEdit)
        'Me.CTRLtemplate.InitializeControl(idSkin, allowEdit)

    End Sub
    Private Sub DisplayNoPermission() Implements IViewModuleSkinBase.DisplayNoPermission
        Me.MLVeditSkin.SetActiveView(VIWempty)
        Me.BTNsaveSkinName.Visible = False
        LBskinMessage.Text = Resource.getValue("DisplayNoPermission.Edit")
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewModuleSkinBase.DisplaySessionTimeout
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()

        If PreloadedIdItem > 0 Then
            dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
            dto.DestinationUrl = RootObject.EditModuleSkin(PreloadedIdSkin, PreloadedIdModule, PreloadedIdCommunity, PreloadedIdModule, PreloadedIdItemType, PreloadedBackUrl)
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
    Private Sub LoadAvailableViews(views As List(Of SkinView)) Implements IViewModuleSkinEdit.LoadAvailableViews
        For Each view As SkinView In views
            Dim tab As Telerik.Web.UI.RadTab = Me.TBSskinEdit.FindTabByValue("SkinView." & view.ToString)
            If Not IsNothing(tab) Then
                tab.Visible = True
            End If
        Next
    End Sub
#End Region

#Region "NAVIGAZIONE"
    Private Sub TBSSkinEdit_TabClick(sender As Object, e As Telerik.Web.UI.RadTabStripEventArgs) Handles TBSskinEdit.TabClick
        Me.Show(lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SkinView).GetByString(Me.TBSskinEdit.SelectedTab.Value.Replace("SkinView.", ""), SkinView.HeaderLogo))
    End Sub

    Private Sub Show(ByVal view As SkinView)
        Select Case view
            Case SkinView.Css
                Me.MLVskins.SetActiveView(Me.VIWcss)
            Case SkinView.Images
                Me.MLVskins.SetActiveView(Me.VIWimages)
            Case SkinView.HeaderLogo
                Me.MLVskins.SetActiveView(Me.VIWheaderLogo)
            Case SkinView.FooterLogos
                Me.MLVskins.SetActiveView(Me.VIWfooterLogos)
            Case SkinView.FooterText
                Me.MLVskins.SetActiveView(Me.VIWfooterText)
            Case SkinView.Shares
                Me.MLVskins.SetActiveView(Me.VIWshares)
                'Case SkinView.Templates
                '    Me.MLVskins.SetActiveView(Me.VIWtemplate)
        End Select
    End Sub
#End Region

    Private Sub BTNsaveSkinName_Click(sender As Object, e As System.EventArgs) Handles BTNsaveSkinName.Click
        Me.CurrentPresenter.SaveSkinName(TXBskinName.Text, Me.CTRLfootLogo.OverrideVoid)
    End Sub

End Class