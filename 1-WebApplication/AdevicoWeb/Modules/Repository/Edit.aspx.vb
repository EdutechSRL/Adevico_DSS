Imports lm.Comol.Core.FileRepository.Domain
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation
Public Class RepositoryItemEdit
    Inherits FReditViewDetailsPageBase
    Implements IViewEditViewDetails

#Region "Context"
    Private _Presenter As EditViewDetailsPresenter
    Private ReadOnly Property CurrentPresenter() As EditViewDetailsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EditViewDetailsPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private WriteOnly Property AllowSave As Boolean Implements IViewEditViewDetails.AllowSave
        Set(value As Boolean)
            BTNsaveItemDetails.Visible = value
        End Set
    End Property
    Private WriteOnly Property AllowShowItem As Boolean Implements IViewEditViewDetails.AllowShowItem
        Set(value As Boolean)
            BTNshowItem.Visible = value
        End Set
    End Property
    Private WriteOnly Property AllowHideItem As Boolean Implements IViewEditViewDetails.AllowHideItem
        Set(value As Boolean)
            BTNhideItem.Visible = value
        End Set
    End Property
#End Region

#Region "Internal"
    Private _messages As List(Of lm.Comol.Core.DomainModel.Helpers.dtoMessage)
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        CurrentPresenter.InitView(True, PreloadIdItem, PreloadIdFolder, PreloadIdentifierPath, PreloadSetBackUrl, PreloadBackUrl)
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub

    Protected Friend Overrides Sub DisplaySessionTimeout()
        Dim url As String = DefaultLogoutUrl
        If String.IsNullOrEmpty(url) Then
            url = GetCurrentUrl()
        End If
        RedirectOnSessionTimeOut(url, RepositoryIdCommunity)
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setHyperLink(HYPbackToPreviousUrl, False, True)
            .setHyperLink(HYPgotoViewDetailsPage, False, True)
            .setButton(BTNsaveItemDetails, True)
            .setButton(BTNhideItem, True)
            .setButton(BTNshowItem, True)
        End With
    End Sub
    Protected Friend Overrides Function GetBackUrlItem(Optional top As Boolean = True) As HyperLink
        Return HYPbackToPreviousUrl
    End Function
#End Region

#Region "Implements"
#Region "Messages"
    Private Sub DisplayUnknownItem() Implements IViewEditViewDetails.DisplayUnknownItem
        DisplayMessage(Resource.getValue("IViewEditViewDetails.DisplayUnknownItem.Edit.True"), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
        MLVcontent.SetActiveView(VIWempty)
    End Sub

    Private Sub DisplayAddedVersion(addedVersion As dtoCreatedItem, folderName As String) Implements IViewEditViewDetails.DisplayAddedVersion
        Dim key As String = "IViewRepository.DisplayAddedVersion." & addedVersion.IsAdded
        If Not addedVersion.IsAdded Then
            key &= ".UnableToAddFile.ItemUploadError." & addedVersion.Error.ToString
        End If
        Dim message As String = Resource.getValue(key)
        If Not String.IsNullOrWhiteSpace(message) Then
            message = Replace(message, "#renderFile#", GetFilenameRender(addedVersion.ToAdd.DisplayName, addedVersion.ToAdd.Extension, addedVersion.ToAdd.Type))
            CTRLmessages.InitializeControl(message, IIf(addedVersion.IsAdded, lm.Comol.Core.DomainModel.Helpers.MessageType.success, lm.Comol.Core.DomainModel.Helpers.MessageType.error))
        End If
    End Sub
#End Region
    Private Sub InitializeHeader() Implements IViewEditViewDetails.InitializeHeader
        CTRLheader.InitializeHeader(True, lm.Comol.Core.FileRepository.Domain.PresetType.None, New Dictionary(Of PresetType, List(Of ViewOption)), New Dictionary(Of PresetType, List(Of ViewOption)), -2, -2, "EditRepositoryItem")
    End Sub
    Private Sub InitializeDefaultTags(tags As List(Of String)) Implements IViewEditViewDetails.InitializeDefaultTags
        CTRLheader.InitializeHeader(True, lm.Comol.Core.FileRepository.Domain.PresetType.None, New Dictionary(Of PresetType, List(Of ViewOption)), New Dictionary(Of PresetType, List(Of ViewOption)), -2, -2, "EditRepositoryItem", tags)
    End Sub
    Private Sub SetTitle(type As ItemType) Implements IViewEditViewDetails.SetTitle
        Dim title As String = Resource.getValue("Edit.ServiceTitle.ItemType." & type.ToString)
        Master.ServiceTitle = title
        Master.ServiceTitleToolTip = title
        Master.ServiceNopermission = Resource.getValue("Edit.ServiceTitle.NoPermission.ItemType." & type.ToString)
    End Sub

    Private Sub DisplayItemDetails(editMode As Boolean, item As dtoDisplayRepositoryItem, thumbnailWidth As Integer, thumbnailHeight As Integer, allowedExtensionsForPreview As String) Implements IViewEditViewDetails.DisplayItemDetails
        CTRLitemDetails.InitializeControl(editMode, item, thumbnailWidth, thumbnailHeight, allowedExtensionsForPreview)
        If item.Permissions.VersioningAvailable OrElse (item.Permissions.AddVersion OrElse item.Permissions.RemoveVersion OrElse item.Permissions.SetVersion) Then
            CTRLitemVersions.Visible = True
            CTRLitemVersions.InitializeControl(editMode, item)
        Else
            CTRLitemVersions.Visible = False
        End If
    End Sub
    Private Sub DisplayItemPermissions(editMode As Boolean, item As dtoDisplayRepositoryItem) Implements IViewEditViewDetails.DisplayItemPermissions
        CTRLitemPermissions.InitializeControl(editMode, item)
        CTRLitemPermissions.Visible = True
    End Sub
    Private Sub SetUrlForView(url As String) Implements IViewEditViewDetails.SetUrlForView
        HYPgotoViewDetailsPage.Visible = Not String.IsNullOrWhiteSpace(url)
        HYPgotoViewDetailsPage.NavigateUrl = ApplicationUrlBase() & url
    End Sub
    Private Sub UpdateItemDetails(item As dtoDisplayRepositoryItem, thumbnailWidth As Integer, thumbnailHeight As Integer, allowedExtensionsForPreview As String) Implements IViewEditViewDetails.UpdateItemDetails
        CTRLitemDetails.InitializeControl(False, item, thumbnailWidth, thumbnailHeight, allowedExtensionsForPreview)
    End Sub
#Region "not used"
    Private Sub SetUrlForEdit(url As String) Implements IViewEditViewDetails.SetUrlForEdit

    End Sub
#End Region

#End Region

#Region "Internal"
    Private Function GetFilenameRender(fullname As String, fileExtension As String, type As ItemType) As String
        Dim template As String = LTtemplateFile.Text
        Select Case type
            Case lm.Comol.Core.FileRepository.Domain.ItemType.Folder
                template = Replace(template, "#ico#", LTitemFolderCssClass.Text)
            Case lm.Comol.Core.FileRepository.Domain.ItemType.Link
                template = Replace(template, "#ico#", LTitemUrlCssClass.Text)
            Case lm.Comol.Core.FileRepository.Domain.ItemType.Multimedia
                template = Replace(template, "#ico#", LTitemMultimediaCssClass.Text)
            Case lm.Comol.Core.FileRepository.Domain.ItemType.ScormPackage
                template = Replace(template, "#ico#", LTitemScormPackageCssClass.Text)
            Case Else
                If Not String.IsNullOrWhiteSpace(fileExtension) Then
                    fileExtension = fileExtension.ToLower
                End If
                If fileExtension.StartsWith(".") Then
                    template = Replace(template, "#ico#", LTitemExtensionCssClass.Text & Replace(fileExtension, ".", ""))
                Else
                    template = Replace(template, "#ico#", LTitemExtensionCssClass.Text)
                End If
        End Select
        template = Replace(template, "#name#", fullname)
        Return template
    End Function
    Private Sub DisplayMessage(ByVal message As String, type As lm.Comol.Core.DomainModel.Helpers.MessageType)
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(message, type)
    End Sub
#Region "Details"
    Private Sub CTRLitemDetails_CheckIsValidOperation(ByRef isvalid As Boolean) Handles CTRLitemDetails.CheckIsValidOperation
        isvalid = isValidOperation()
    End Sub
    Private Sub CTRLitemDetails_DisplayMessage(message As String, type As lm.Comol.Core.DomainModel.Helpers.MessageType) Handles CTRLitemDetails.DisplayMessage
        If Not IsNothing(_messages) Then
            _messages.Add(New lm.Comol.Core.DomainModel.Helpers.dtoMessage() With {.Text = message, .Type = type})

            CTRLmessages.Visible = True
            CTRLmessages.InitializeControl(_messages)
        Else
            DisplayMessage(message, type)
        End If
    End Sub
    Private Sub CTRLitemDetails_UpdateContainerTags(tags As List(Of String)) Handles CTRLitemDetails.UpdateContainerTags
        InitializeDefaultTags(tags)
    End Sub
    Private Sub BTNhideItem_Click(sender As Object, e As EventArgs) Handles BTNhideItem.Click
        _messages = New List(Of lm.Comol.Core.DomainModel.Helpers.dtoMessage)()
        If CTRLitemDetails.ChangeVisibility(ItemAction.hide) Then
            BTNshowItem.Visible = True
            BTNhideItem.Visible = False
        End If
    End Sub
    Private Sub BTNshowItem_Click(sender As Object, e As EventArgs) Handles BTNshowItem.Click
        _messages = New List(Of lm.Comol.Core.DomainModel.Helpers.dtoMessage)()
        If CTRLitemDetails.ChangeVisibility(ItemAction.show) Then
            BTNshowItem.Visible = False
            BTNhideItem.Visible = True
        End If
    End Sub
    Private Sub BTNsaveItemDetails_Click(sender As Object, e As EventArgs) Handles BTNsaveItemDetails.Click
        _messages = New List(Of lm.Comol.Core.DomainModel.Helpers.dtoMessage)()
        If isValidOperation() Then
            If CTRLitemDetails.SaveDetails() Then
                If CTRLitemPermissions.HasItemsToSave Then
                    CTRLitemPermissions.AllowUpload = CTRLitemDetails.AllowUpload
                    CTRLitemPermissions.TryToSavePermissions()
                ElseIf CTRLitemPermissions.AllowUpload <> CTRLitemDetails.AllowUpload Then
                    CTRLitemPermissions.AllowUpload = CTRLitemDetails.AllowUpload
                    CTRLitemPermissions.UpdateForUpload(CTRLitemDetails.AllowUpload)
                End If
            End If
        End If
    End Sub
#End Region

#Region "Versions"
 
    Private Sub CTRLitemVersions_DisplayMessage(message As String, type As lm.Comol.Core.DomainModel.Helpers.MessageType) Handles CTRLitemVersions.DisplayMessage
        _messages = New List(Of lm.Comol.Core.DomainModel.Helpers.dtoMessage)()
        DisplayMessage(message, type)
    End Sub
    Private Sub CTRLitemVersions_VersionUpdated() Handles CTRLitemVersions.GenericVersionUpdated
        CurrentPresenter.CurrentVersionUpdated(IdItem)
    End Sub
    Private Sub CTRLitemVersions_VersionUpdated1(item As dtoDisplayRepositoryItem, thumbnailWidth As Integer, thumbnailHeight As Integer, allowedExtensionsForPreview As String) Handles CTRLitemVersions.VersionUpdated
        CTRLitemDetails.InitializeControl(True, item, thumbnailWidth, thumbnailHeight, allowedExtensionsForPreview)
    End Sub
    Private Sub CTRLitemVersions_SessionTimeout() Handles CTRLitemVersions.SessionTimeout
        DisplaySessionTimeout()
    End Sub
#End Region
#Region "Permissions"
    Private Sub CTRLitemPermissions_AskForApply(name As String) Handles CTRLitemPermissions.AskForApply

    End Sub
    Private Sub CTRLitemPermissions_UpdateMyAllowUpload() Handles CTRLitemPermissions.UpdateMyAllowUpload
        CTRLitemPermissions.AllowUpload = CTRLitemDetails.AllowUpload
    End Sub
    Private Sub CTRLitemPermissions_DisplayMessage(message As String, type As lm.Comol.Core.DomainModel.Helpers.MessageType) Handles CTRLitemPermissions.DisplayMessage
        DisplayMessage(message, type)
    End Sub
    Private Sub CTRLitemPermissions_HideModalWindow() Handles CTRLitemPermissions.HideModalWindow
        Master.SetOpenDialogOnPostbackByCssClass("")
    End Sub
    Private Sub CTRLitemPermissions_SessionTimeout() Handles CTRLitemPermissions.SessionTimeout
        DisplaySessionTimeout()
    End Sub
#End Region
#End Region

    Private Sub RepositoryItemEdit_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Master.ShowDocType = True
    End Sub

   
End Class