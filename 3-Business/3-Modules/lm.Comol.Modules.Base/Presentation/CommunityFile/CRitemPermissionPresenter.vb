Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports COL_BusinessLogic_v2

Namespace lm.Comol.Modules.Base.Presentation
    Public Class CRitemPermissionPresenter
        Inherits DomainPresenter

        Private _CommonManager As ManagerCommon
        Public Overloads Property CurrentManager() As ManagerCommunityFiles
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As ManagerCommunityFiles)
                _CurrentManager = value
            End Set
        End Property
        Public Overloads Property CommonManager() As ManagerCommon
            Get
                Return _CommonManager
            End Get
            Set(ByVal value As ManagerCommon)
                _CommonManager = value
            End Set
        End Property

        Public Overloads ReadOnly Property View() As IviewItemPermission
            Get
                Return MyBase.View
            End Get
        End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IviewItemPermission)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub


        Public Sub InitView()
            Dim CommunityID As Integer = Me.View.PreloadedCommunityID
            If CommunityID = 0 Then
                CommunityID = Me.UserContext.CurrentCommunityID
            End If
            If Me.View.PreloadedPreserveUrl Then
                Me.View.SavePreservedUrl()
            End If
            Me.View.RemoteItemID = Me.View.PreloadedRemoteItemID
            If Not Me.UserContext.isAnonymous Then
                Dim ItemID As Long = Me.View.PreloadedItemID
                Dim oItemsID As List(Of Long) = Me.View.PreloadedMultipleItemsID
                Dim oItem As CommunityFile = Nothing

                If ItemID = 0 AndAlso oItemsID.Count > 0 Then
                    ItemID = oItemsID(0)
                    Me.View.isSetPermissionForMultipleFile = True
                Else
                    Me.View.isSetPermissionForMultipleFile = False
                End If
                oItem = Me.CurrentManager.GetFileItemById(ItemID)
              
                If IsNothing(oItem) Then
                    GotoManagementDownloadPage(Nothing)
                Else
                    Dim ItemCommunityID As Long = 0
                    If Not IsNothing(oItem.CommunityOwner) Then
                        ItemCommunityID = oItem.CommunityOwner.Id
                    End If
                    Me.View.ItemCommunityID = ItemCommunityID
                    Dim oCommunity As Community = Me.CurrentManager.GetCommunity(ItemCommunityID)
                    If oCommunity Is Nothing AndAlso ItemCommunityID <> 0 Then
                        Me.View.NoPermission(ItemCommunityID, Me.CommonManager.GetModuleID(UCServices.Services_File.Codex))
                    Else
                        Dim oPermission As ModuleCommunityRepository = Me.View.CommunityRepositoryPermission(ItemCommunityID)
                        Dim FolderID As Long = Me.View.PreloadedFolderID
                        Dim AllowView As Boolean = Me.CurrentManager.HasPermissionToSeeItem(ItemID, oPermission.Administration, oPermission.Administration, Me.UserContext.CurrentUserID)
                        Dim oPerson As Person = Me.CommonManager.GetPerson(Me.UserContext.CurrentUserID)

                        If AllowView AndAlso (oPermission.Administration OrElse oItem.Owner Is oPerson) Then
                            Me.View.AllowSave = True
                            InitializeView(oItem)
                        Else
                            Me.View.NoPermissionToEdit(CommunityID, Me.CommonManager.GetModuleID(UCServices.Services_File.Codex), oItem.Id, oItem.isFile, oItem.isSCORM)
                            Me.View.AllowSave = False
                        End If

                        If Me.View.PreloadedPreserveUrl Then
                            If String.IsNullOrEmpty(Me.View.PreservedUrl) Then
                                Me.View.SetBackUrl(Me.View.ItemCommunityID, Me.View.PreloadedItemID, oItem.FolderId, Me.View.PreloadedPreviousView, Me.View.PreLoadedPage)
                            Else
                                Me.View.SetBackUrlToPrevious()
                            End If
                        ElseIf Me.View.isSetPermissionForMultipleFile Then
                            Me.View.SetBackUrl(Me.View.ItemCommunityID, 0, oItem.FolderId, Me.View.PreloadedPreviousView, Me.View.PreLoadedPage)
                        ElseIf Me.View.PreloadedItemID > 0 Then
                            Me.View.SetBackUrl(Me.View.ItemCommunityID, Me.View.PreloadedItemID, oItem.FolderId, Me.View.PreloadedPreviousView, Me.View.PreLoadedPage)
                        Else
                            Me.View.SetBackUrl(Me.View.ItemCommunityID, 0, 0, Me.View.PreloadedPreviousView, Me.View.PreLoadedPage)
                        End If
                    End If
                End If
            Else
                Me.View.NoPermission(CommunityID, Me.CommonManager.GetModuleID(UCServices.Services_File.Codex))
            End If
        End Sub

        Private Sub InitializeView(ByVal oItem As CommunityFile)
            If Not Me.View.isSetPermissionForMultipleFile Then
                If oItem.isFile Then
                    Me.View.FileName = oItem.DisplayName
                Else
                    Me.View.FolderName = oItem.Name
                End If
                Me.View.RepositoryItemID = oItem.Id
            Else
                Me.View.FilesName = Me.View.PreloadedMultipleItemsName
            End If

            Dim oUsers As List(Of dtoMember(Of Integer)) = Me.CurrentManager.GetAssignedPerson(oItem, False)
            Dim oRoles As List(Of FilterElement)
            If Me.View.ItemCommunityID > 0 Then
                Dim oRolesID As List(Of Integer) = Me.CurrentManager.GetRolesAssignmentID(oItem.Id, False)
                Dim oTranslatedRoles = CL_permessi.COL_TipoRuolo.List(Me.UserContext.Language.Id)

                oRoles = (From o In oTranslatedRoles Where oRolesID.Contains(o.ID) Order By o.Name Select New FilterElement(o.ID, o.Name)).ToList
            Else
                Dim oTypesID As List(Of Integer) = Me.CurrentManager.GetAssignedUserTypesID(oItem.Id, False)
                Dim oTranslatedTypes = (From o In CL_persona.COL_TipoPersona.ListForCreate(Me.UserContext.Language.Id) Select o).ToList
                'Where o.ID <> Main.TipoPersonaStandard.Guest 
                oRoles = (From o In oTranslatedTypes Where oTypesID.Contains(o.ID) Order By o.Descrizione Select New FilterElement(o.ID, o.Descrizione)).ToList
            End If
            Me.View.AskToApplyToAllSubItems = Not oItem.isFile
            Me.View.SendActionInit(Me.View.ItemCommunityID, Me.CommonManager.GetModuleID(UCServices.Services_File.Codex), oItem.Id, oItem.isFile, oItem.isSCORM)
            Me.View.Initialize(oRoles, oUsers, Me.CurrentManager.HasCommunityAssignment(oItem.Id, False))
            Me.View.InitializeMembersSelection(IIf(Me.View.ItemCommunityID <= 0, 0, Me.View.ItemCommunityID))
            Me.LoadRoles()
        End Sub
        Public Sub LoadRoles()
            Dim oRoles As List(Of FilterElement)
            If Me.View.ItemCommunityID > 0 Then
                Dim oRolesID As List(Of Integer) = Me.CommonManager.GetRolesAvailableID(Me.View.ItemCommunityID)
                Dim oTranslatedRoles = CL_permessi.COL_TipoRuolo.List(Me.UserContext.Language.Id)
                oRoles = (From o In oTranslatedRoles Order By o.Name Where oRolesID.Contains(o.ID) Select New FilterElement(o.ID, o.Name)).ToList
            Else
                Dim oTranslatedTypes = (From o In CL_persona.COL_TipoPersona.ListForCreate(Me.UserContext.Language.Id) Select o).ToList
                'Where o.ID <> Main.TipoPersonaStandard.Guest 
                oRoles = (From o In oTranslatedTypes Order By o.Descrizione Select New FilterElement(o.ID, o.Descrizione)).ToList
            End If
            Me.View.InitializeAvailableRoles(oRoles, Me.View.SelectedRolesID)
        End Sub
        Public Sub UpdateSelectedRoles(ByVal selectedID As List(Of Integer))
            Dim oRoles As List(Of FilterElement)
            If Me.View.ItemCommunityID > 0 Then
                Dim oTranslatedRoles = CL_permessi.COL_TipoRuolo.List(Me.UserContext.Language.Id)
                oRoles = (From o In oTranslatedRoles Where selectedID.Contains(o.ID) Order By o.Name Select New FilterElement(o.ID, o.Name)).ToList
            Else
                Dim oTranslatedTypes = (From o In CL_persona.COL_TipoPersona.ListForCreate(Me.UserContext.Language.Id) Select o).ToList
                'Where o.ID <> Main.TipoPersonaStandard.Guest 
                oRoles = (From o In oTranslatedTypes Where selectedID.Contains(o.ID) Order By o.Descrizione Select New FilterElement(o.ID, o.Descrizione)).ToList
            End If
            Me.View.SelectedRoles = oRoles
            Me.View.UpdateSelectRoles(oRoles)
        End Sub
        Public Sub LoadMembers()
            Me.View.InitializeMembersSelection(IIf(Me.View.ItemCommunityID > 0, Me.View.ItemCommunityID, -1))
        End Sub
        Public Sub UpdateSelectedMembers(ByVal oList As List(Of dtoMember(Of Integer)))
            Dim SelectedID As List(Of Integer) = Me.View.SelectedMembersID
            Me.View.SelectedMembers.AddRange((From d In oList Where Not SelectedID.Contains(d.Id) Select d).ToList)
            Me.View.UpdateSelecteMembers(Me.View.SelectedMembers)
        End Sub


        Public Sub SavePermission()
            PersistPermission(False)
        End Sub
        Public Sub SavePermissionToSubFolders()
            PersistPermission(True)
        End Sub
        Private Sub PersistPermission(ByVal ToSubitems As Boolean)
            Dim ForMultiple As Boolean = Me.View.isSetPermissionForMultipleFile
            Dim ItemsID As New List(Of Long)
            Dim Permission As Long = COL_BusinessLogic_v2.UCServices.Services_File.Base2Permission.DownloadFile

            If ForMultiple Then
                ItemsID.AddRange(Me.View.PreloadedMultipleItemsID)
            Else
                ItemsID.Add(Me.View.RepositoryItemID)
            End If

            Dim ItemID As Long = 0
            Dim FolderID As Long = 0
            Dim FolderName As String = Me.View.BaseFolder
            Dim UserID As Integer = 0
            If Me.View.isSetPermissionForMultipleFile Then
                Dim oFile As CommunityFile = Me.CurrentManager.GetFileItemById(Me.View.PreloadedMultipleItemsID(0))
                If Not IsNothing(oFile) Then
                    FolderID = oFile.FolderId
                    If oFile.AllowUpload Then : Permission = Permission Or COL_BusinessLogic_v2.UCServices.Services_File.Base2Permission.UploadFile

                    End If
                End If
            Else
                ItemID = Me.View.RepositoryItemID
                Dim oFile As CommunityFile = Me.CurrentManager.GetFileItemById(Me.View.RepositoryItemID)
                If Not IsNothing(oFile) Then
                    FolderID = oFile.FolderId
                    If oFile.AllowUpload Then : Permission = Permission Or COL_BusinessLogic_v2.UCServices.Services_File.Base2Permission.UploadFile

                    End If
                End If
                If oFile.Owner.Id <> Me.UserContext.CurrentUserID Then
                    UserID = Me.UserContext.CurrentUserID
                End If
            End If

            If Me.View.AllMembers Then
                Me.CurrentManager.SetCommunityAssignmentToItems(Me.UserContext.CurrentUserID, FolderID, ItemsID, False, ToSubitems, Permission)
            Else
                If Me.View.ItemCommunityID = 0 Then
                    Me.CurrentManager.AddPortalAssignmentToItems(Me.UserContext.CurrentUserID, ItemsID, Me.View.SelectedRolesID, Me.View.SelectedMembersID, False, ToSubitems, Permission)
                Else
                    Me.CurrentManager.AddAssignmentToItems(Me.UserContext.CurrentUserID, ItemsID, Me.View.SelectedRolesID, Me.View.SelectedMembersID, False, ToSubitems, Permission)
                End If
            End If

            Dim oItem As CommunityFile = Me.CurrentManager.GetFileItemById(ItemID)
            If Not IsNothing(oItem) Then
                If FolderID > 0 Then
                    FolderName = Me.CurrentManager.GetFolderName(FolderID)
                End If
                Me.View.NotifyPermissionChanged(Me.View.AllMembers, UserID, Me.View.ItemCommunityID, ItemID, oItem.DisplayName, FolderID, FolderName, oItem.isFile, oItem.UniqueID, oItem.isVisible, oItem.RepositoryItemType)
            End If
            Dim oAction As PermissionAction = Me.View.PreloadedAction

            Dim oPage As RepositoryPage = Me.View.PreLoadedPage

            If oAction = PermissionAction.SingleUpload OrElse oAction = PermissionAction.MultipleUpload Then

            ElseIf oAction = PermissionAction.EditMultipleItems Then

            Else

            End If
            Me.View.LoadRepositoryPage(Me.View.ItemCommunityID, ItemID, FolderID, Me.View.PreloadedPreviousView, oPage)
        End Sub
        Public Sub ChangePermissionSelector()
            Dim oItem As CommunityFile = Me.CurrentManager.GetFileItemById(Me.View.RepositoryItemID)
            If Not IsNothing(oItem) AndAlso Not oItem.isFile Then
                Me.View.AskToApplyToAllSubItems = True ' Me.CurrentManager.HasCommunityAssignment(Me.View.RepositoryItemID, False) <> Me.View.AllMembers
            Else
                Me.View.AskToApplyToAllSubItems = False
            End If

            If Not Me.View.AllMembers AndAlso Me.View.SelectedMembersID.Count = 0 Then
                Dim oList As New List(Of dtoMember(Of Integer))
                oList.Add(New dtoMember(Of Integer) With {.Id = Me.UserContext.CurrentUserID, .Name = Me.UserContext.CurrentUser.SurnameAndName})
                Me.UpdateSelectedMembers(oList)
            End If
        End Sub
        Public Sub GotoManagementDownloadPage(ByVal oFile As CommunityFile)
            Dim CommunityID As Integer = Me.UserContext.CurrentCommunityID
            Dim FolderID As Long = 0
            Dim ItemID As Long = 0
            If Not IsNothing(oFile) Then
                If oFile.CommunityOwner Is Nothing Then
                    CommunityID = 0
                Else
                    CommunityID = oFile.CommunityOwner.Id
                End If
                FolderID = oFile.FolderId
                ItemID = oFile.Id
            End If
            Me.View.LoadRepositoryPage(CommunityID, ItemID, FolderID, Me.View.PreloadedPreviousView, Me.View.PreLoadedPage)
        End Sub
        'Public Sub GotoPermissionManagement(ByVal oFile As CommunityFile)
        '    Dim CommunityID As Integer = Me.UserContext.CurrentCommunityID
        '    Dim FolderID As Long = 0
        '    If Not IsNothing(oFile) Then
        '        If oFile.CommunityOwner Is Nothing Then
        '            CommunityID = 0
        '        Else
        '            CommunityID = oFile.CommunityOwner.Id
        '        End If
        '        FolderID = oFile.FolderId
        '        Me.View.LoadEditingPermission(oFile.Id, CommunityID, FolderID, Me.View.PreloadedPreviousView, Me.View.PreLoadedPage)
        '    Else
        '        Me.View.LoadRepositoryPage(CommunityID, FolderID, Me.View.PreloadedPreviousView, Me.View.PreLoadedPage)
        '    End If
        'End Sub
    End Class
End Namespace