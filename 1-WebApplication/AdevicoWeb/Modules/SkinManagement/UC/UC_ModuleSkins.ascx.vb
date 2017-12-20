Imports lm.Comol.Core.BaseModules.Web
Imports lm.Comol.Core.BaseModules.Web.Controls
Imports lm.Comol.Modules.Standard.Skin.Domain
Imports lm.Comol.Modules.Standard.Skin.Presentation

Public Class UC_ModuleSkins
    Inherits BaseControl
    Implements IViewModuleSkinSelector

#Region "Context"
    Private _BaseUrl As String
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As ModuleSkinSelectorPresenter

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
    Private ReadOnly Property CurrentPresenter() As ModuleSkinSelectorPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ModuleSkinSelectorPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implemnts"
    Private Property AllowEditSelection As Boolean Implements IViewModuleSkinSelector.AllowEditSelection
        Get
            Return ViewStateOrDefault("AllowEditSelection", True)
        End Get
        Set(value As Boolean)
            Dim idItem As Long = SelectedItem.Id

            ViewState("AllowEditSelection") = value
            Dim mObject As lm.Comol.Core.DomainModel.ModuleObject = Source
            Me.HYPedit.Visible = value AndAlso idItem > 0
            Me.HYPedit.NavigateUrl = BaseUrl & RootObject.EditModuleSkin(idItem, mObject.ServiceID, mObject.CommunityID, mObject.ObjectLongID, mObject.ObjectTypeID, DestinationUrl)
        End Set
    End Property
    Private Property Source As lm.Comol.Core.DomainModel.ModuleObject Implements IViewModuleSkinSelector.Source
        Get
            Return ViewStateOrDefault("Source", New lm.Comol.Core.DomainModel.ModuleObject())
        End Get
        Set(value As lm.Comol.Core.DomainModel.ModuleObject)
            ViewState("Source") = value
        End Set
    End Property
    Private Property LoadModuleSkinBy As LoadItemsBy Implements IViewModuleSkinSelector.LoadModuleSkinBy
        Get
            Return ViewStateOrDefault("LoadModuleSkinBy", LoadItemsBy.None)
        End Get
        Set(value As LoadItemsBy)
            ViewState("LoadModuleSkinBy") = value
        End Set
    End Property
    Public Property isInitialized As Boolean Implements IViewModuleSkinSelector.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public ReadOnly Property isValid As Boolean Implements IViewModuleSkinSelector.isValid
        Get

        End Get
    End Property

    Private Property AllowDelete As Boolean Implements IViewModuleSkinSelector.AllowDelete
        Get
            Return ViewStateOrDefault("AllowDelete", True)
        End Get
        Set(value As Boolean)
            Dim idItem As Long = SelectedItem.Id
            ViewState("AllowDelete") = value
            Dim mObject As lm.Comol.Core.DomainModel.ModuleObject = Source
            Me.HYPdelete.Visible = value
            Me.HYPdelete.NavigateUrl = BaseUrl & RootObject.DeleteModuleSkin(idItem, mObject.ServiceID, mObject.CommunityID, mObject.ObjectLongID, mObject.ObjectTypeID, DestinationUrl)
        End Set
    End Property
    Private Property AllowAdd As Boolean Implements IViewModuleSkinSelector.AllowAdd
        Get
            Return ViewStateOrDefault("AllowAdd", True)
        End Get
        Set(value As Boolean)
            ViewState("AllowAdd") = value
            Dim mObject As lm.Comol.Core.DomainModel.ModuleObject = Source
            Me.HYPadd.Visible = value
            Me.HYPadd.NavigateUrl = BaseUrl & RootObject.AddModuleSkin(mObject.ServiceID, mObject.CommunityID, mObject.ObjectLongID, mObject.ObjectTypeID, DestinationUrl)
        End Set
    End Property
    Private Property AllowEdit As Boolean Implements IViewModuleSkinSelector.AllowEdit
        Get
            Return ViewStateOrDefault("AllowEdit", True)
        End Get
        Set(value As Boolean)
            ViewState("AllowEdit") = value
        End Set
    End Property
    Private WriteOnly Property AllowPreview As Boolean Implements IViewModuleSkinSelector.AllowPreview
        Set(value As Boolean)
            Dim mObject As lm.Comol.Core.DomainModel.ModuleObject = Source
            Me.HYPpreview.Visible = value
            Me.HYPpreview.NavigateUrl = BaseUrl & RootObject.Preview(SelectedItem, mObject.CommunityID, mObject.ServiceID)
        End Set
    End Property
    Private ReadOnly Property DestinationUrl As String Implements IViewModuleSkinSelector.DestinationUrl
        Get
            Dim url As String = Request.Url.LocalPath
            If Me.BaseUrl <> "/" Then
                url = Replace(Request.Url.LocalPath, Me.BaseUrl, "")
            End If
            url = Server.UrlEncode(url & Request.Url.Query & "#" & ItemIdentifier)
            Return url
        End Get
    End Property
    Private Property Items As List(Of DtoDisplaySkin) Implements IViewModuleSkinSelector.Items
        Get
            Return ViewStateOrDefault("Items", New List(Of DtoDisplaySkin))
        End Get
        Set(value As List(Of DtoDisplaySkin))
            ViewState("Items") = value
        End Set
    End Property
    Private ReadOnly Property FullSkinManagement As Boolean Implements IViewModuleSkinSelector.FullSkinManagement
        Get
            Dim SkinSettings As SkinSettings = MyBase.SystemSettings.SkinSettings

            Return (SkinSettings.PersonTypeIds.Contains(CurrentContext.UserContext.UserTypeID) OrElse _
                SkinSettings.PersonsIds.Contains(CurrentContext.UserContext.CurrentUserID))
        End Get
    End Property
    Public ReadOnly Property SelectedItem As DtoDisplaySkin Implements IViewModuleSkinSelector.SelectedItem
        Get
            Dim item As DtoDisplaySkin

            If Me.DDLskin.Items.Count > 0 Then
                item = Items.Where(Function(i) GetUniqueIdItem(i.Id, i.Type) = Me.DDLskin.SelectedValue).FirstOrDefault()
            End If
            If item Is Nothing Then
                item = New DtoDisplaySkin() With {.Id = 0, .Type = SkinDisplayType.Empty}
            End If
            Return item
        End Get
    End Property
#End Region

    Private Property ItemIdentifier As String
        Get
            Return ViewStateOrDefault("ItemIdentifier", "")
        End Get
        Set(value As String)
            ViewState("ItemIdentifier") = value
            If String.IsNullOrEmpty(value) Then
                LTidentifier.Visible = False
            Else
                LTidentifier.Text = "<a name=""" & value & """> </a>"
                LTidentifier.Visible = True
            End If
        End Set
    End Property

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_SkinModuleManagement", "Skin")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setHyperLink(HYPpreview, True, True)
            .setHyperLink(HYPedit, True, True)
            .setHyperLink(HYPadd, True, True)
            .setHyperLink(HYPdelete, True, True)
            .setLabel(LBskinTitle_t)
        End With
    End Sub
#End Region

#Region "Inherits"
    Public Sub InitializeControl(idModule As Integer, idCommunity As Integer, idModuleItem As Long, idItemType As Integer, fullyQualifiedName As String, loadModuleSkinBy As LoadItemsBy) Implements IViewModuleSkinSelector.InitializeControl
        InitializeControl(idModule, idCommunity, idModuleItem, idItemType, fullyQualifiedName, True, True, loadModuleSkinBy)
    End Sub
    Public Sub InitializeControl(idModule As Integer, idCommunity As Integer, idModuleItem As Long, idItemType As Integer, fullyQualifiedName As String, allowAdd As Boolean, allowEdit As Boolean, loadModuleSkinBy As LoadItemsBy) Implements IViewModuleSkinSelector.InitializeControl
        ItemIdentifier = "skin_" & idModuleItem.ToString
        Me.CurrentPresenter.InitView(idModule, idCommunity, idModuleItem, idItemType, fullyQualifiedName, allowAdd, allowEdit, loadModuleSkinBy)
    End Sub

    Public Sub DisplaySessionTimeout() Implements IViewModuleSkinSelector.DisplaySessionTimeout
        HYPadd.Visible = False
        HYPedit.Visible = False
        HYPpreview.Visible = False
        Me.DDLskin.Enabled = False
    End Sub
    Public Sub LoadSkins(skins As List(Of DtoDisplaySkin), selectedItem As DtoDisplaySkin) Implements IViewModuleSkinSelector.LoadSkins
        Me.Items = skins
        Me.DDLskin.Items.Clear()
        If (skins.Where(Function(s) s.Type = SkinDisplayType.Empty).Any()) Then
            Me.DDLskin.Items.Add(New ListItem() With {.Value = GetUniqueIdItem(0, SkinDisplayType.Empty), .Text = Me.Resource.getValue("SkinDisplayType." & SkinDisplayType.Empty.ToString)})
        End If

        If (skins.Where(Function(s) s.Type = SkinDisplayType.Portal).Any()) Then
            Me.DDLskin.AddItemGroup(Resource.getValue("SkinDisplayType." & SkinDisplayType.Portal.ToString))
            For Each skin As DtoDisplaySkin In skins.Where(Function(s) s.Type = SkinDisplayType.Portal).ToList()
                Me.DDLskin.Items.Add(New ListItem() With {.Value = GetUniqueIdItem(skin.Id, skin.Type), .Text = IIf(String.IsNullOrEmpty(skin.Name), Me.Resource.getValue("Skin." & skin.Type.ToString), skin.Name)})
            Next
        End If
        If (skins.Where(Function(s) s.Type = SkinDisplayType.Organization).Any()) Then
            Me.DDLskin.AddItemGroup(Resource.getValue("SkinDisplayType." & SkinDisplayType.Organization.ToString))
            RenderItems(skins.Where(Function(s) s.Type = SkinDisplayType.Organization).ToList())
        End If
        If (skins.Where(Function(s) s.Type = SkinDisplayType.Community OrElse s.Type = SkinDisplayType.CurrentCommunity).Any()) Then
            Me.DDLskin.AddItemGroup(Resource.getValue("SkinDisplayType." & SkinDisplayType.Community.ToString))

            RenderItems(skins.Where(Function(s) s.Type = SkinDisplayType.Community).ToList())
            RenderItems(skins.Where(Function(s) s.Type = SkinDisplayType.CurrentCommunity).ToList())
        End If
        If (skins.Where(Function(s) s.Type = SkinDisplayType.Module).Any()) Then
            Me.DDLskin.AddItemGroup(Resource.getValue("SkinDisplayType." & SkinDisplayType.Module.ToString))
            RenderItems(skins.Where(Function(s) s.Type = SkinDisplayType.Module).ToList())
        End If

        Try
            If Not IsNothing(selectedItem) Then
                Me.DDLskin.SelectedValue = GetUniqueIdItem(selectedItem.Id, selectedItem.Type)
            End If

        Catch ex As Exception

        End Try

    End Sub
    Private Sub RenderItems(skins As List(Of DtoDisplaySkin))
        For Each skin As DtoDisplaySkin In skins
            Me.DDLskin.Items.Add(New ListItem() With {.Value = GetUniqueIdItem(skin.Id, skin.Type), .Text = String.Format(Resource.getValue("Skin.Isvalid." & skin.IsValid.ToString()), skin.Name)})
        Next
    End Sub
    Private Function GetUniqueIdItem(idItem As Long, itemType As SkinDisplayType) As String
        Return idItem.ToString() & " - " & itemType.ToString
    End Function
    Public Sub LoadEmptySkin() Implements IViewModuleSkinSelector.LoadEmptySkin
        Me.DDLskin.Items.Clear()

        Me.DDLskin.Items.Add(New ListItem() With {.Value = GetUniqueIdItem(0, SkinDisplayType.Empty), .Text = Me.Resource.getValue("SkinDisplayType." & SkinDisplayType.Empty.ToString)})

    End Sub
    Public Function SaveSkinAssociation() As Boolean Implements IViewModuleSkinSelector.SaveSkinAssociation
        Return Me.CurrentPresenter.SaveSkinAssociation(SelectedItem, Source)
    End Function

    Public Function ObjectHasSkinAssociation(idModule As Integer, idCommunity As Integer, idModuleItem As Long, idItemType As Integer, fullyQualifiedName As String) As Boolean Implements IViewModuleSkinSelector.ObjectHasSkinAssociation
        Return Me.CurrentPresenter.HasSkinAssociation(idModule, idCommunity, idModuleItem, idItemType, fullyQualifiedName)
    End Function

#End Region

    Private Sub DDLskin_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLskin.SelectedIndexChanged
        Dim dto As DtoDisplaySkin

        Me.CurrentPresenter.SelectSkin(Me.SelectedItem)
    End Sub


   
End Class