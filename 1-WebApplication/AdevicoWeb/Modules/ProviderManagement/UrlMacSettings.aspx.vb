Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProviderManagement
Imports lm.Comol.Core.BaseModules.ProviderManagement.Presentation

Public Class UrlMacSettings
    Inherits PageBase
    Implements IViewMacUrlProviderSettings

#Region "Context"
    Private _Presenter As MacUrlProviderSettingsPresenter
    Private ReadOnly Property CurrentPresenter() As MacUrlProviderSettingsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New MacUrlProviderSettingsPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property PreloadedIdProvider As Long Implements IViewMacUrlProviderSettings.PreloadedIdProvider
        Get
            If IsNumeric(Request.QueryString("IdProvider")) Then
                Return CLng(Request.QueryString("IdProvider"))
            Else : Return CLng(0)
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedIdInEditingAttribute As Long Implements IViewMacUrlProviderSettings.PreloadedIdInEditingAttribute
        Get
            If IsNumeric(Request.QueryString("idEditAttribute")) Then
                Return CLng(Request.QueryString("idEditAttribute"))
            Else : Return CLng(0)
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedAttributeType As lm.Comol.Core.Authentication.UrlMacAttributeType Implements IViewMacUrlProviderSettings.PreloadedAttributeType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.Authentication.UrlMacAttributeType).GetByString(Request.QueryString("sType"), lm.Comol.Core.Authentication.UrlMacAttributeType.profile)
        End Get
    End Property

    Private Property IdProvider As Long Implements IViewMacUrlProviderSettings.IdProvider
        Get
            Return ViewStateOrDefault("IdProvider", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdProvider") = value
        End Set
    End Property
    Private Property AllowEdit As Boolean Implements IViewMacUrlProviderSettings.AllowEdit
        Get
            Return ViewStateOrDefault("AllowEdit", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("AllowEdit") = value
            Me.HYPbackToProviderEdit.Visible = value
            Me.HYPbackToProviderEditBottom.Visible = value
            HYPbackToProviderEdit.NavigateUrl = BaseUrl & RootObject.EditProvider(IdProvider)
            HYPbackToProviderEditBottom.NavigateUrl = BaseUrl & RootObject.EditProvider(IdProvider)
            Me.BTNaddMacAttribute.Visible = value
        End Set
    End Property
    Private WriteOnly Property AllowManagement As Boolean Implements IViewMacUrlProviderSettings.AllowManagement
        Set(value As Boolean)
            Me.HYPbackToManagementTop.Visible = value
            Me.HYPbackToManagementTop.NavigateUrl = BaseUrl & RootObject.Management(IdProvider)
            Me.HYPbackToManagementBottom.Visible = value
            Me.HYPbackToManagementBottom.NavigateUrl = Me.HYPbackToManagementTop.NavigateUrl
        End Set
    End Property
    Private Property IdAttributeEditing As Long Implements IViewMacUrlProviderSettings.IdAttributeEditing
        Get
            Return ViewStateOrDefault("IdAttributeEditing", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdAttributeEditing") = value
        End Set
    End Property
    'Private Property EncryptionInfo As lm.Comol.Core.Authentication.Helpers.EncryptionInfo Implements IViewMacUrlProviderSettings.EncryptionInfo
    '    Get
    '        Dim dto As New lm.Comol.Core.Authentication.Helpers.EncryptionInfo()

    '        dto.InitializationVector = Me.TXBinitializationVector.Text
    '        dto.Key = Me.TXBkey.Text
    '        dto.EncryptionAlgorithm = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.Authentication.Helpers.EncryptionAlgorithm).GetByString(DDLencryptionAlgorithm.SelectedValue, lm.Comol.Core.Authentication.Helpers.EncryptionAlgorithm.Rijndael)
    '        Return dto
    '    End Get
    '    Set(value As lm.Comol.Core.Authentication.Helpers.EncryptionInfo)
    '        Me.TXBkey.Text = value.Key
    '        Me.TXBinitializationVector.Text = value.InitializationVector
    '        Me.DDLencryptionAlgorithm.SelectedValue = value.EncryptionAlgorithm.ToString
    '    End Set
    'End Property
    Private ReadOnly Property CurrentAttributeToAdd As lm.Comol.Core.Authentication.UrlMacAttributeType Implements IViewMacUrlProviderSettings.CurrentAttributeToAdd
        Get
            If Me.DDLattributesType.SelectedIndex = -1 Then
                Return lm.Comol.Core.Authentication.UrlMacAttributeType.profile
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.Authentication.UrlMacAttributeType).GetByString(Me.DDLattributesType.SelectedValue, lm.Comol.Core.Authentication.UrlMacAttributeType.profile)
            End If
        End Get
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected ReadOnly Property DisplayMode(ByVal idItem As Long) As String
        Get
            Return IIf(idItem = IdAttributeEditing, "editmode", "viewmode")
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False
        If Page.IsPostBack = False Then
            CurrentPresenter.InitView()
        End If
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
        MyBase.SetCulture("pg_AuthenticationProvider", "Modules", "ProviderManagement")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("serviceMacUrlSettingsProviderTitle")
            Me.Master.ServiceNopermission = .getValue("serviceMacUrlSettingsProviderNopermission")

            .setButton(Me.BTNaddMacAttribute, True)
            .setHyperLink(Me.HYPbackToManagementTop, True, True)
            .setHyperLink(Me.HYPbackToManagementBottom, True, True)
            .setHyperLink(Me.HYPbackToProviderEdit, True, True)

            .setHyperLink(Me.HYPbackToProviderEditBottom, True, True)
            .setLabel(LBmacAttributeType_t)
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

#Region "Implements"

#Region "Errors"
    Private Sub DisplayAttributeAdded() Implements IViewMacUrlProviderSettings.DisplayAttributeAdded
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayAttributeAdded"), Helpers.MessageType.success)
    End Sub
    Private Sub DisplayAttributeNotAdded() Implements IViewMacUrlProviderSettings.DisplayAttributeNotAdded
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayAttributeNotAdded"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayAttributeNotDeleted() Implements IViewMacUrlProviderSettings.DisplayAttributeNotDeleted
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayAttributeNotDeleted"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayAttributeOptionNotDeleted() Implements IViewMacUrlProviderSettings.DisplayAttributeOptionNotDeleted
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayAttributeOptionNotDeleted"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayAttributeOptionNotAdded() Implements IViewMacUrlProviderSettings.DisplayAttributeOptionNotAdded
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayAttributeOptionNotAdded"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayDuplicatedRemoteCode() Implements IViewMacUrlProviderSettings.DisplayDuplicatedRemoteCode
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayDuplicatedRemoteCode"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayInvalidAttribute() Implements IViewMacUrlProviderSettings.DisplayInvalidAttribute
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayInvalidAttribute"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayInvalidAttributeAndRemoteCode() Implements IViewMacUrlProviderSettings.DisplayInvalidAttributeAndRemoteCode
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayInvalidAttributeAndRemoteCode"), Helpers.MessageType.error)
    End Sub
#End Region

    Private Sub DisplayDeletedProvider(name As String, type As lm.Comol.Core.Authentication.AuthenticationProviderType) Implements IViewMacUrlProviderSettings.DisplayDeletedProvider
        Me.MLVdata.SetActiveView(VIWempty)
        Me.LBmessage.Text = String.Format(Me.Resource.getValue("DisplayDeletedProvider"), name, Me.Resource.getValue("AuthenticationProviderType." & type.ToString))
    End Sub
    Private Sub DisplayNoPermission() Implements IViewMacUrlProviderSettings.DisplayNoPermission
        Me.MLVdata.SetActiveView(VIWempty)
        Me.LBmessage.Text = Me.Resource.getValue("DisplayNoPermission")
    End Sub
    Private Sub DisplayProviderUnknown() Implements IViewMacUrlProviderSettings.DisplayProviderUnknown
        Me.MLVdata.SetActiveView(VIWempty)
        Me.LBmessage.Text = Me.Resource.getValue("DisplayProviderUnknown")
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewMacUrlProviderSettings.DisplaySessionTimeout
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = RootObject.EditUrlProviderSettings(IIf(IdProvider > 0, IdProvider, PreloadedIdProvider))
        webPost.Redirect(dto)
    End Sub

    Private Sub GotoUrl(url As String) Implements IViewMacUrlProviderSettings.GotoUrl
        PageUtility.RedirectToUrl(url)
    End Sub
    Private Sub GotoManagement() Implements IViewMacUrlProviderSettings.GotoManagement
        PageUtility.RedirectToUrl(RootObject.Management(IdProvider))
    End Sub

    Private Sub LoadAvailableTypes(types As List(Of lm.Comol.Core.Authentication.UrlMacAttributeType), selected As lm.Comol.Core.Authentication.UrlMacAttributeType) Implements IViewMacUrlProviderSettings.LoadAvailableTypes
        Dim items As New List(Of TranslatedItem(Of String))

        items = (From s In types Select New TranslatedItem(Of String) With {.Id = s.ToString, .Translation = Me.Resource.getValue("UrlMacAttributeType." & s.ToString)}).OrderBy(Function(i) i.Translation).ToList

        Me.DDLattributesType.DataSource = items
        Me.DDLattributesType.DataTextField = "Translation"
        Me.DDLattributesType.DataValueField = "Id"
        Me.DDLattributesType.DataBind()

        If types.Contains(selected) Then
            Me.DDLattributesType.SelectedValue = selected.ToString
        End If
        Me.DDLattributesType.Enabled = (items.Count > 1)
        Me.BTNaddMacAttribute.Enabled = Me.BTNaddMacAttribute.Visible AndAlso (items.Count > 0)
    End Sub

    Private Sub LoadAttributes(items As List(Of lm.Comol.Core.Authentication.BaseUrlMacAttribute)) Implements IViewMacUrlProviderSettings.LoadAttributes
        Me.RPTattributes.DataSource = items
        Me.RPTattributes.DataBind()
    End Sub

    Private Sub LoadProviderInfo(provider As lm.Comol.Core.BaseModules.ProviderManagement.dtoMacUrlProvider) Implements IViewMacUrlProviderSettings.LoadProviderInfo
        Me.LoadAttributes(provider.Attributes)
    End Sub
#End Region

    Private Sub RPTattributes_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTattributes.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oControl As UC_EditMacUrlAttribute = e.Item.FindControl("CTRLattribute")
            Dim item As lm.Comol.Core.Authentication.BaseUrlMacAttribute = DirectCast(e.Item.DataItem, lm.Comol.Core.Authentication.BaseUrlMacAttribute)
            If item.Id = IdAttributeEditing Then
                oControl.InitializeEditControl(IdProvider, item, AllowEdit)
            Else
                oControl.InitializeDisplayControl(IdProvider, item, AllowEdit AndAlso IdAttributeEditing = 0)
            End If
        End If
    End Sub

#Region "Management Attributes"

#Region "Common"
    Private Sub BTNaddMacAttribute_Click(sender As Object, e As System.EventArgs) Handles BTNaddMacAttribute.Click
        Me.CurrentPresenter.AddAttribute(GetNewAttribute())
    End Sub
    Private Function GetNewAttribute() As lm.Comol.Core.Authentication.BaseUrlMacAttribute
        Dim attribute As lm.Comol.Core.Authentication.BaseUrlMacAttribute
        Dim type As lm.Comol.Core.Authentication.UrlMacAttributeType = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.Authentication.UrlMacAttributeType).GetByString(Me.DDLattributesType.SelectedValue, lm.Comol.Core.Authentication.UrlMacAttributeType.profile)
        Select Case type
            Case lm.Comol.Core.Authentication.UrlMacAttributeType.applicationId
                Dim aAttribute As New lm.Comol.Core.Authentication.ApplicationAttribute()
                aAttribute.Name = Resource.getValue("ApplicationAttribute.Name")
                aAttribute.Description = Resource.getValue("ApplicationAttribute.Description")
                aAttribute.QueryStringName = "ApplicationID"
                aAttribute.Value = String.Format(Resource.getValue("ApplicationAttribute.Value"), IdProvider.ToString)
                aAttribute.Type = type
                attribute = aAttribute

            Case (lm.Comol.Core.Authentication.UrlMacAttributeType.coursecatalogue)
                Dim cAttribute As New lm.Comol.Core.Authentication.CatalogueAttribute()
                cAttribute.Name = Resource.getValue("CatalogueAttribute.Name")
                cAttribute.Description = Resource.getValue("CatalogueAttribute.Description")
                cAttribute.QueryStringName = Resource.getValue("CatalogueAttribute.QueryStringName")
                cAttribute.Items = New List(Of lm.Comol.Core.Authentication.CatalogueAttributeItem)
                cAttribute.AllowMultipleValue = False
                cAttribute.MultipleValueSeparator = ""
                cAttribute.Type = type
                attribute = cAttribute

            Case lm.Comol.Core.Authentication.UrlMacAttributeType.functionId
                Dim fAttribute As New lm.Comol.Core.Authentication.FunctionAttribute()
                fAttribute.Name = Resource.getValue("FunctionAttribute.Name")
                fAttribute.Description = Resource.getValue("FunctionAttribute.Description")
                fAttribute.QueryStringName = Resource.getValue("FunctionAttribute.QueryStringName")
                fAttribute.Value = String.Format(Resource.getValue("FunctionAttribute.Value"), IdProvider.ToString)
                fAttribute.Type = type
                attribute = fAttribute

            Case lm.Comol.Core.Authentication.UrlMacAttributeType.mac
                Dim mAttribute As New lm.Comol.Core.Authentication.MacAttribute()
                mAttribute.Name = Resource.getValue("MacAttribute.Name")
                mAttribute.Description = Resource.getValue("MacAttribute.Description")
                mAttribute.QueryStringName = Resource.getValue("MacAttribute.QueryStringName")
                mAttribute.Items = New List(Of lm.Comol.Core.Authentication.MacAttributeItem)
                mAttribute.Type = type
                attribute = mAttribute

            Case lm.Comol.Core.Authentication.UrlMacAttributeType.organization
                Dim oAttribute As New lm.Comol.Core.Authentication.OrganizationAttribute()
                oAttribute.Name = Resource.getValue("OrganizationAttribute.Name")
                oAttribute.Description = Resource.getValue("OrganizationAttribute.Description")
                oAttribute.QueryStringName = Resource.getValue("OrganizationAttribute.QueryStringName")
                oAttribute.Items = New List(Of lm.Comol.Core.Authentication.OrganizationAttributeItem)
                oAttribute.AllowMultipleValue = False
                oAttribute.MultipleValueSeparator = ""
                oAttribute.Type = type
                attribute = oAttribute

            Case lm.Comol.Core.Authentication.UrlMacAttributeType.profile
                Dim uAttribute As New lm.Comol.Core.Authentication.UserProfileAttribute()
                uAttribute.Name = Resource.getValue("UserProfileAttribute.Name")
                uAttribute.Description = Resource.getValue("UserProfileAttribute.Description")
                uAttribute.QueryStringName = Resource.getValue("UserProfileAttribute.QueryStringName")
                uAttribute.Attribute = lm.Comol.Core.Authentication.ProfileAttributeType.skip
                uAttribute.Type = type
                attribute = uAttribute

            Case lm.Comol.Core.Authentication.UrlMacAttributeType.timestamp
                Dim tAttribute As New lm.Comol.Core.Authentication.TimestampAttribute()
                tAttribute.Name = Resource.getValue("TimestampAttribute.Name")
                tAttribute.Description = Resource.getValue("TimestampAttribute.Description")
                tAttribute.QueryStringName = Resource.getValue("TimestampAttribute.QueryStringName")
                tAttribute.Format = lm.Comol.Core.Authentication.TimestampFormat.aaaammgghhmmss
                tAttribute.Type = type
                attribute = tAttribute

            Case lm.Comol.Core.Authentication.UrlMacAttributeType.url
                Dim tAttribute As New lm.Comol.Core.Authentication.BaseUrlMacAttribute()
                tAttribute.Name = Resource.getValue("BaseUrlMacAttribute.Name")
                tAttribute.Description = Resource.getValue("BaseUrlMacAttribute.Description")
                tAttribute.QueryStringName = Resource.getValue("BaseUrlMacAttribute.QueryStringName")
                tAttribute.Type = type
                attribute = tAttribute

            Case lm.Comol.Core.Authentication.UrlMacAttributeType.compositeProfile
                Dim cmpAttribute As New lm.Comol.Core.Authentication.CompositeProfileAttribute()
                cmpAttribute.Name = Resource.getValue("CompositeUserProfileAttribute.Name")
                cmpAttribute.Description = Resource.getValue("CompositeUserProfileAttribute.Description")
                cmpAttribute.QueryStringName = Resource.getValue("CompositeUserProfileAttribute.QueryStringName")
                cmpAttribute.Type = type
                attribute = cmpAttribute
        End Select
        Return attribute
    End Function
    Protected Sub VirtualDeleteAttribute(idAttribute As Long)
        Me.CurrentPresenter.VirtualDeleteAttribute(idAttribute)
    End Sub
    Protected Sub RemoveAttributeItem(idOwner As Long, idItem As Long, type As lm.Comol.Core.Authentication.UrlMacAttributeType)
        Me.CurrentPresenter.VirtualDeleteUrlMacAttributeItem(idOwner, idItem, type)
    End Sub
    Protected Sub EditAttribute(idAttribute As Long)
        GotoUrl(RootObject.EditUrlMacProviderSettings(IdProvider, CurrentAttributeToAdd, idAttribute, True))
    End Sub
    Protected Sub UndoEditing(idAttribute As Long)
        GotoUrl(RootObject.EditUrlMacProviderSettings(IdProvider, CurrentAttributeToAdd, idAttribute, False))
    End Sub
    Protected Sub SavedAttribute(idAttribute As Long)
        GotoUrl(RootObject.EditUrlMacProviderSettings(IdProvider, CurrentAttributeToAdd, idAttribute, False))
    End Sub
    Protected Sub OnUnsavedAttribute(idAttribute As Long)
        DisplayInvalidAttribute()
    End Sub
    Protected Sub InvalidCode(idAttribute As Long)
        DisplayDuplicatedRemoteCode()
    End Sub

#End Region

#Region "Mac attribute"
    Protected Sub AddMacAttributeItem(idOwner As Long, idAttributeItem As Long)
        Me.CurrentPresenter.AddMacAttributeItem(idOwner, idAttributeItem)
    End Sub
#End Region

#Region "Composite attribute"
    Protected Sub AddCompositeAttributeItem(idOwner As Long, idAttributeItem As Long)
        Me.CurrentPresenter.AddCompositeAttributeItem(idOwner, idAttributeItem)
    End Sub
#End Region

#Region "Organization attribute"
    Protected Sub AddOrganizationAttributeItem(idOwner As Long, idOrganization As Int32, idDefaultPage As Long, idDefaultProfile As Int32, remoteCode As String)
        Me.CurrentPresenter.AddOrganizationAttributeItem(idOwner, idOrganization, idDefaultPage, idDefaultProfile, remoteCode)
    End Sub
#End Region

#Region "Catalogue attribute"
    Protected Sub AddCatalogueAttributeItem(idOwner As Long, idCatalogue As Long, remoteCode As String)
        Me.CurrentPresenter.AddCatalogueAttributeItem(idOwner, idCatalogue, remoteCode)
    End Sub
#End Region

#End Region

End Class