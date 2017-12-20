Imports lm.Comol.Core.FileRepository.Domain
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain
Imports lm.ActionDataContract
Public Class UC_ItemPermissions
    Inherits FRbaseControl
    Implements IViewItemPermissions

#Region "Context"
    Private _Presenter As ItemPermissionsPresenter
    Public ReadOnly Property CurrentPresenter() As ItemPermissionsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ItemPermissionsPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property IdRepositoryItem As Long Implements IViewItemPermissions.IdRepositoryItem
        Get
            Return ViewStateOrDefault("IdRepositoryItem", 0)
        End Get
        Set(value As Long)
            ViewState("IdRepositoryItem") = value
        End Set
    End Property
    Private Property RepositoryItemType As ItemType Implements IViewItemPermissions.RepositoryItemType
        Get
            Return ViewStateOrDefault("RepositoryItemType", ItemType.File)
        End Get
        Set(value As ItemType)
            ViewState("RepositoryItemType") = value
        End Set
    End Property
    Public Property HasPendingChanges As Boolean Implements IViewItemPermissions.HasPendingChanges
        Get
            Return ViewStateOrDefault("HasPendingChanges", False)
        End Get
        Set(value As Boolean)
            ViewState("HasPendingChanges") = value
            DVpendingChanges.Visible = value
        End Set
    End Property
    Public Property AllowUpload As Boolean Implements IViewItemPermissions.AllowUpload
        Get
            Return ViewStateOrDefault("AllowUpload", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowUpload") = value
        End Set
    End Property
    Private Function GetFolderTypeTranslation() As Dictionary(Of FolderType, String) Implements IViewItemPermissions.GetFolderTypeTranslation
        Return (From e As FolderType In [Enum].GetValues(GetType(FolderType)) Select e).ToDictionary(Function(e) e, Function(e) Resource.getValue("FolderName.FolderType." & e.ToString))
    End Function
    Private Function GetTypesTranslations() As Dictionary(Of ItemType, String) Implements IViewItemPermissions.GetTypesTranslations
        Return (From e As ItemType In [Enum].GetValues(GetType(ItemType)) Select e).ToDictionary(Function(e) e, Function(e) Resource.getValue("FolderName.ItemType." & e.ToString))
    End Function
    Private Function GetUnknownUserName() As String Implements IViewItemPermissions.GetUnknownUserName
        Return Resource.getValue("UnknownUserName")
    End Function

#End Region

#Region "Internal"
    Private _ShowActions As Boolean
    Public Event DisplayMessage(message As String, type As lm.Comol.Core.DomainModel.Helpers.MessageType)
    Public Event CheckIsValidOperation(ByRef isvalid As Boolean)
    Public Event SessionTimeout()
    Public Event HideModalWindow()
    Public Event AskForApply(ByVal name As String)
    Public Event UpdateMyAllowUpload()
    Private Property EditMode As Boolean
        Get
            Return ViewStateOrDefault("EditMode", False)
        End Get
        Set(value As Boolean)
            ViewState("EditMode") = value
        End Set
    End Property

    Private _typeTranslations As Dictionary(Of AssignmentType, String)
    Private ReadOnly Property AssignmentTypeTranslations As Dictionary(Of AssignmentType, String)
        Get
            If IsNothing(_typeTranslations) Then
                _typeTranslations = (From i In System.Enum.GetValues(GetType(AssignmentType)) Select New With {.Id = i, .Value = Resource.getValue("MoreItems.AssignmentType." & i.ToString)}).ToDictionary(Of AssignmentType, String)(Function(i) i.Id, Function(x) x.Value)
            End If
            Return _typeTranslations
        End Get
    End Property

    Private _Translations As Dictionary(Of PermissionsTranslation, String)
    Private ReadOnly Property Translations As Dictionary(Of PermissionsTranslation, String)
        Get
            If IsNothing(_Translations) Then
                _Translations = (From i In System.Enum.GetValues(GetType(lm.Comol.Core.FileRepository.Domain.PermissionsTranslation)) Select New With {.Id = i, .Value = Resource.getValue("PermissionsTranslation." & i.ToString)}).ToDictionary(Of lm.Comol.Core.FileRepository.Domain.PermissionsTranslation, String)(Function(i) i.Id, Function(x) x.Value)
            End If
            Return _Translations
        End Get
    End Property
    Private _permissionsTranslations As Dictionary(Of ModuleRepository.Base2Permission, String)
    Private ReadOnly Property PermissionsTranslations As Dictionary(Of ModuleRepository.Base2Permission, String)
        Get
            If IsNothing(_permissionsTranslations) Then
                _permissionsTranslations = (From i In System.Enum.GetValues(GetType(ModuleRepository.Base2Permission)) Select New With {.Id = i, .Value = Resource.getValue("Permissions.Base2Permission." & i.ToString)}).ToDictionary(Of ModuleRepository.Base2Permission, String)(Function(i) i.Id, Function(x) x.Value)
            End If
            Return _permissionsTranslations
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLiteral(LTpermissionsSection)
            .setLiteral(LTpermissionsRolePerson_t)
            .setLiteral(LTpermissionsPermissions_t)

            .setLinkButton(LNBaddRoles, False, True)
            .setLinkButton(LNBaddUsers, False, True)

            .setLabel(LBselectRolesDescription)
            .setButton(BTNsaveRoleAssignments, True)
            .setHyperLink(HYPcloseAddRoleDialog, False, True)


            .setLiteral(LTpendingChangesOnPermissions)
            .setLinkButton(LNBapplyPermissionsToContent, False, True)
            .setLinkButton(LNBreloadPermissionsToItem, False, True)


            .setLabel(LBapplyToFolder)
            .setButton(BTNapplyToFolder, True)
            .setLabel(LBapplyToFolderContent)
            .setButton(BTNapplyToFolderContent, True)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(edit As Boolean, item As dtoDisplayRepositoryItem) Implements IViewItemPermissions.InitializeControl
        If item.Type = ItemType.Folder Then
            Resource.setLinkButtonToValue(LNBapplyPermissionsToItem, "Folder", False, True)
        Else
            Resource.setLinkButton(LNBapplyPermissionsToItem, False, True)
        End If

        InitializeRepositoryPath(item.Repository)
        IdRepositoryItem = item.Id
        RepositoryItemType = item.Type
        EditMode = edit
        CurrentPresenter.InitView(edit, item, GetUnknownUserName, AssignmentTypeTranslations, Translations, PermissionsTranslations)
    End Sub

    Private Sub DisplayUserMessage(messageType As UserMessageType) Implements IViewItemPermissions.DisplayUserMessage
        RaiseEvent DisplayMessage(Resource.getValue("UserMessageType." & messageType.ToString), GetMessageIcon(messageType))
    End Sub
    Private Sub DisplayUserMessage(messageType As UserMessageType, items As Integer) Implements IViewItemPermissions.DisplayUserMessage
        Dim key As String = "UserMessageType." & messageType.ToString
        Dim message As String = ""
        Select Case items
            Case 0, 1
                key &= "." & items
                message = Resource.getValue(key)
            Case Else
                key &= ".n"
                message = String.Format(message, items)
        End Select
        RaiseEvent DisplayMessage(message, GetMessageIcon(messageType))
    End Sub

    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleRepository.ActionType, idItem As Long, objType As ModuleRepository.ObjectType) Implements IViewItemPermissions.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, objType, idItem.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub LoadAssignments(items As List(Of dtoEditAssignment)) Implements IViewItemPermissions.LoadAssignments
        _ShowActions = EditMode
        THactions.Visible = _ShowActions
        RPTpermissions.DataSource = items
        RPTpermissions.DataBind()
    End Sub


    Private Function GetPermissions() As List(Of dtoEditAssignment) Implements IViewItemPermissions.GetPermissions
        Dim items As New List(Of dtoEditAssignment)
        For Each r As RepeaterItem In RPTpermissions.Items
            Dim oLiteral As Literal = DirectCast(r.FindControl("LTinherited"), Literal)
            If oLiteral.Text <> Boolean.TrueString Then
                Dim item As New dtoEditAssignment()
                oLiteral = DirectCast(r.FindControl("LTidAssignment"), Literal)
                item.Id = CInt(oLiteral.Text)
                oLiteral = DirectCast(r.FindControl("LTassignmentType"), Literal)
                item.Type = DirectCast(CInt(oLiteral.Text), AssignmentType)
                item.DisplayName = DirectCast(r.FindControl("LTdisplayName"), Literal).Text

                oLiteral = r.FindControl("LTassignments")
                Dim values As List(Of String) = oLiteral.Text.Split("|").ToList()
                Integer.TryParse(values(0), item.IdCommunity)
                Integer.TryParse(values(1), item.IdPerson)
                Integer.TryParse(values(2), item.IdRole)

                oLiteral = r.FindControl("LToldValue")
                Boolean.TryParse(oLiteral.Text, item.OldDenyed)

                Dim oCheckbox As CheckBox = r.FindControl("CBXallowAccess")
                item.Denyed = Not oCheckbox.Checked
                item.IsDeleted = Not r.Visible
                items.Add(item)
            End If
        Next
        Return items
    End Function
    Private Sub DisplaySessionTimeout() Implements IViewItemPermissions.DisplaySessionTimeout
        DVcommands.Visible = False
        RaiseEvent SessionTimeout()
        For Each row As RepeaterItem In RPTpermissions.Items
            Dim cell As HtmlControls.HtmlTableCell = DirectCast(row.FindControl("TDactions"), HtmlTableCell)
            cell.Visible = False
        Next
        THactions.Visible = False
    End Sub
    Private Sub InitializeCommands(allowAddRole As Boolean, allowAddUsers As Boolean) Implements IViewItemPermissions.InitializeCommands
        LNBaddRoles.Visible = allowAddRole
        LNBaddUsers.Visible = allowAddUsers
        DVcommands.Visible = EditMode AndAlso (allowAddRole OrElse allowAddUsers)
        If Not (allowAddRole AndAlso allowAddUsers) Then
            DVcommands.Attributes("class") = Replace(DVcommands.Attributes("class"), " enabled", "")
        End If
    End Sub

    Private Sub InitializeRolesSelector(roles As List(Of lm.Comol.Core.DomainModel.dtoTranslatedRoleType)) Implements IViewItemPermissions.InitializeRolesSelector
        DVselectRoles.Visible = True
        CBLroles.DataSource = roles
        CBLroles.DataTextField = "Name"
        CBLroles.DataValueField = "Id"
        CBLroles.DataBind()
        SetCheckBoxListItemsCssClass(CBLroles)
    End Sub
    Private Sub InitializeUsersSelector(idCommunity As Integer, removeUsers As List(Of Integer)) Implements IViewItemPermissions.InitializeUsersSelector
        DVselectUsers.Visible = True
        CTRLselectUsers.InitializeControl(lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.CommunityUsers, True, idCommunity, removeUsers, Nothing, Resource.getValue("UsersSelector.Community"))
    End Sub
    Private Sub InitializePortalUsersSelector(removeUsers As List(Of Integer)) Implements IViewItemPermissions.InitializePortalUsersSelector
        DVselectUsers.Visible = True
        CTRLselectUsers.InitializeControl(lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.SystemUsers, True, False, removeUsers, Nothing, Resource.getValue("UsersSelector.Portal"))
    End Sub
    Private Sub InitializeMyUsersSelector(idCommunities As List(Of Integer), removeUsers As List(Of Integer)) Implements IViewItemPermissions.InitializeMyUsersSelector
        DVselectUsers.Visible = True
        CTRLselectUsers.InitializeControl(lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.CommunityUsers, True, idCommunities, removeUsers, Nothing, Resource.getValue("UsersSelector.MyUsers"))
    End Sub
    Private Sub AskUserForApply(name As String) Implements IViewItemPermissions.AskUserForApply
        DVaskForApply.Visible = True
        Resource.setLabel(LBfolderToApplyOptions)
        LBfolderToApplyOptions.Text = String.Format(LBfolderToApplyOptions.Text, name)
    End Sub
    Public Function HasItemsToSave() As Boolean Implements IViewItemPermissions.HasItemsToSave
        Return HasPendingChanges OrElse GetPermissions().Any(Function(p) p.Denyed <> p.OldDenyed)
    End Function
#End Region

#Region "internal"
    Private Function GetMessageIcon(messageType) As lm.Comol.Core.DomainModel.Helpers.MessageType
        Dim mType As lm.Comol.Core.DomainModel.Helpers.MessageType = lm.Comol.Core.DomainModel.Helpers.MessageType.error
        Select Case messageType
            Case UserMessageType.permissionsSaved
                mType = lm.Comol.Core.DomainModel.Helpers.MessageType.success
            Case UserMessageType.permissionsNoItemToSave
                mType = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
        End Select
        Return mType
    End Function
    Private Sub RPTpermissions_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTpermissions.ItemCommand
        Dim idAssignment As Long = 0
        Long.TryParse(e.CommandArgument, idAssignment)


        Select Case e.CommandName
            Case "delete"
                Dim oLiteral As Literal = DirectCast(e.Item.FindControl("LTidAssignment"), Literal)
                Dim item As New dtoEditAssignment()
                item.Id = CInt(oLiteral.Text)
                oLiteral = DirectCast(e.Item.FindControl("LTassignmentType"), Literal)
                item.Type = DirectCast(CInt(oLiteral.Text), AssignmentType)
                item.DisplayName = DirectCast(e.Item.FindControl("LTdisplayName"), Literal).Text
                oLiteral = e.Item.FindControl("LTassignments")
                Dim values As List(Of String) = oLiteral.Text.Split("|").ToList()
                Integer.TryParse(values(0), item.IdCommunity)
                Integer.TryParse(values(1), item.IdPerson)
                Integer.TryParse(values(2), item.IdRole)

                Dim oCheckbox As CheckBox = e.Item.FindControl("CBXallowAccess")
                item.Denyed = Not oCheckbox.Checked
                item.IsDeleted = Not e.Item.Visible
                e.Item.Visible = False
                CurrentPresenter.RemoveItem(IdRepositoryItem, item, GetPermissions)
        End Select

    End Sub
    Private Sub RPTpermissions_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTpermissions.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim dto As dtoEditAssignment = DirectCast(e.Item.DataItem, dtoEditAssignment)
                Dim cell As HtmlControls.HtmlTableCell = DirectCast(e.Item.FindControl("TDactions"), HtmlTableCell)
                cell.Visible = _ShowActions

                If _ShowActions Then
                    Dim oButton As Button = DirectCast(e.Item.FindControl("BTNdeleteItemPermission"), Button)
                    oButton.Visible = Not dto.IsDeleted AndAlso Not dto.Inherited AndAlso Not dto.Type = AssignmentType.community
                    Resource.setButton(oButton, True)
                End If

                Dim oLiteral As Literal = DirectCast(e.Item.FindControl("LTassignments"), Literal)
                oLiteral.Text = dto.IdCommunity.ToString() & "|" & dto.IdPerson.ToString() & "|" & dto.IdRole.ToString()

                Dim oLabel As Label = DirectCast(e.Item.FindControl("LBpermissions"), Label)
                '     oLabel.CssClass &= " " & Replace(dto.TranslatedPermissions.Type.ToString(), ",", " ")
                oLabel.Visible = dto.ReadOnly OrElse Not _ShowActions

                Dim oCheckBox As CheckBox = DirectCast(e.Item.FindControl("CBXallowAccess"), CheckBox)
                oCheckBox.Visible = Not dto.ReadOnly AndAlso _ShowActions

                If (dto.Permissions <= 0) Then
                    oCheckBox.Text = PermissionsTranslations(ModuleRepository.Base2Permission.DownloadOrPlay)
                ElseIf (lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(ModuleRepository.Base2Permission.DownloadOrPlay, dto.Permissions)) Then
                    oCheckBox.Text = PermissionsTranslations(ModuleRepository.Base2Permission.DownloadOrPlay)
                    oCheckBox.Checked = True
                End If
                If (AllowUpload AndAlso lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(ModuleRepository.Base2Permission.Upload, dto.Permissions)) Then
                    If String.IsNullOrWhiteSpace(oCheckBox.Text) Then
                        oCheckBox.Text = PermissionsTranslations(ModuleRepository.Base2Permission.Upload)
                    Else
                        oCheckBox.Text &= ", " & PermissionsTranslations(ModuleRepository.Base2Permission.Upload)
                    End If
                    oCheckBox.Checked = True
                End If
                e.Item.Visible = Not dto.IsDeleted
        End Select
    End Sub

#Region "Add Items"
    Private Sub BTNsaveRoleAssignments_Click(sender As Object, e As EventArgs) Handles BTNsaveRoleAssignments.Click
        If CBLroles.SelectedIndex >= 0 Then
            Dim items As New List(Of lm.Comol.Core.DomainModel.dtoTranslatedRoleType)

            For Each i As ListItem In CBLroles.Items
                If i.Selected Then
                    items.Add(New lm.Comol.Core.DomainModel.dtoTranslatedRoleType With {.Id = CInt(i.Value), .Name = i.Text})
                End If
            Next
            CurrentPresenter.AddRoles(EditMode, IdRepositoryItem, items, AssignmentTypeTranslations, Translations, PermissionsTranslations, GetPermissions)
        End If
    End Sub

    Private Sub CTRLselectUsers_CloseWindow() Handles CTRLselectUsers.CloseWindow
        RaiseEvent HideModalWindow()
    End Sub
    Private Sub CTRLselectUsers_UsersSelected(idUsers As List(Of Integer)) Handles CTRLselectUsers.UsersSelected
        If idUsers.Any() Then
            CurrentPresenter.AddUsers(EditMode, IdRepositoryItem, idUsers, AssignmentTypeTranslations, Translations, PermissionsTranslations, GetPermissions)
        End If
        RaiseEvent HideModalWindow()
    End Sub
#End Region

#Region "Save"
    Private Sub LNBapplyPermissionsToContent_Click(sender As Object, e As EventArgs) Handles LNBapplyPermissionsToContent.Click
        DVaskForApply.Visible = False
        RaiseEvent UpdateMyAllowUpload()
            CurrentPresenter.SaveAssignments(IdRepositoryItem, GetPermissions, True, AssignmentTypeTranslations, Translations, PermissionsTranslations)
    End Sub
    Private Sub LNBapplyPermissionsToItem_Click(sender As Object, e As EventArgs) Handles LNBapplyPermissionsToItem.Click
        DVaskForApply.Visible = False
        RaiseEvent UpdateMyAllowUpload()
        CurrentPresenter.SaveAssignments(IdRepositoryItem, GetPermissions, False, AssignmentTypeTranslations, Translations, PermissionsTranslations)
    End Sub
    Private Sub LNBreloadPermissionsToItem_Click(sender As Object, e As EventArgs) Handles LNBreloadPermissionsToItem.Click
        HasPendingChanges = False
        DVaskForApply.Visible = False
        RaiseEvent UpdateMyAllowUpload()
        CurrentPresenter.ReloadPermissions(IdRepositoryItem, AssignmentTypeTranslations, Translations, PermissionsTranslations)
    End Sub
    Public Function TryToSavePermissions() As Boolean
        HasPendingChanges = True
        CurrentPresenter.TryToSave(IdRepositoryItem, GetPermissions, AssignmentTypeTranslations, Translations, PermissionsTranslations)
    End Function

    Public Function UpdateForUpload(allowUpload) As Boolean
        CurrentPresenter.UpdateForUpload(IdRepositoryItem, allowUpload, GetPermissions, AssignmentTypeTranslations, Translations, PermissionsTranslations)
    End Function


    Private Function SavePermissions(applyToContent As String) As Boolean
        HasPendingChanges = True
        CurrentPresenter.SaveAssignments(IdRepositoryItem, GetPermissions, applyToContent, AssignmentTypeTranslations, Translations, PermissionsTranslations)
    End Function
#End Region

#Region "Confirm"
    Private Sub BTNapplyToFolder_Click(sender As Object, e As EventArgs) Handles BTNapplyToFolder.Click
        DVaskForApply.Visible = False
        SavePermissions(False)
    End Sub

    Private Sub BTNapplyToFolderContent_Click(sender As Object, e As EventArgs) Handles BTNapplyToFolderContent.Click
        DVaskForApply.Visible = False
        SavePermissions(True)
    End Sub
#End Region

#End Region

End Class