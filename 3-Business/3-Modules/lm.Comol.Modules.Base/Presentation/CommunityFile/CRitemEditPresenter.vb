Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports COL_BusinessLogic_v2

Namespace lm.Comol.Modules.Base.Presentation
    Public Class CRitemEditPresenter
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

        Public Overloads ReadOnly Property View() As IViewCommunityFileEdit
            Get
                Return MyBase.View
            End Get
        End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewCommunityFileEdit)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub

        Public Sub InitView()
            Dim CommunityID As Integer = Me.View.PreloadedCommunityID
            If CommunityID = 0 Then
                CommunityID = Me.UserContext.CurrentCommunityID
            End If
            If Not Me.UserContext.isAnonymous Then
                Dim oFile As CommunityFile = Me.CurrentManager.GetFileItemById(Me.View.PreloadedItemID)

                If IsNothing(oFile) Then
                    ' REDIRECT TO
                    Me.GotoManagementDownloadPage(oFile)
                Else
                    If oFile.CommunityOwner Is Nothing Then
                        CommunityID = 0
                    Else
                        CommunityID = oFile.CommunityOwner.Id
                    End If
                    Dim oCommunity As Community = Me.CurrentManager.GetCommunity(CommunityID)
                    If oCommunity Is Nothing AndAlso CommunityID <> 0 Then
                        Me.View.NoPermission(CommunityID, Me.CommonManager.GetModuleID(UCServices.Services_File.Codex))
                    Else
                        Dim oPermission As ModuleCommunityRepository = Me.View.CommunityRepositoryPermission(CommunityID)
                        Dim FolderID As Long = oFile.FolderId
                    
                        If oPermission.Administration OrElse oPermission.UploadFile Then
                            Dim SeeFolder As Boolean = True
                            If FolderID > 0 Then
                                SeeFolder = Me.CurrentManager.HasPermissionToSeeItem(FolderID, oPermission.Administration, oPermission.Administration, Me.UserContext.CurrentUserID)
                            End If

                            Dim SeeItem As Boolean = Me.CurrentManager.HasPermissionToSeeItem(oFile.Id, oPermission.Administration, oPermission.Administration, Me.UserContext.CurrentUserID)
                            If Not SeeFolder OrElse Not SeeItem Then
                                FolderID = 0
                                Me.View.NoPermissionToEdit(CommunityID, Me.CommonManager.GetModuleID(UCServices.Services_File.Codex), oFile.Id, oFile.isFile, oFile.isSCORM)
                            Else
                                Me.View.SendActionInit(CommunityID, Me.CommonManager.GetModuleID(UCServices.Services_File.Codex), oFile.Id, oFile.isFile, oFile.isSCORM)
                                Me.InitializeView(oFile, oCommunity, oPermission)
                            End If
                        Else
                            Me.View.NoPermissionToEdit(CommunityID, Me.CommonManager.GetModuleID(UCServices.Services_File.Codex), oFile.Id, oFile.isFile, oFile.isSCORM)
                        End If
                        Me.View.AllowBackToDownloads(FolderID, CommunityID, Me.View.PreloadedPreviousView) = oPermission.Administration OrElse oPermission.ListFiles
                        Me.View.AllowBackToManagement(FolderID, CommunityID, Me.View.PreloadedPreviousView) = oPermission.Administration
                    End If
                End If
            Else
                CommunityID = IIf(Me.View.PreloadedCommunityID > 0, Me.View.PreloadedCommunityID, Me.UserContext.CurrentCommunityID)
                Me.View.NoPermission(CommunityID, Me.CommonManager.GetModuleID(UCServices.Services_File.Codex))
            End If
        End Sub

        Private Sub InitializeView(ByVal oItem As CommunityFile, ByVal oCommunity As Community, ByVal oPermission As ModuleCommunityRepository)
            Dim RepositoryCommunityID As Integer = 0
            If IsNothing(oCommunity) Then
                Me.View.CommunityName = Me.View.Portalname
            Else
                Me.View.CommunityName = oCommunity.Name
                RepositoryCommunityID = oCommunity.Id
            End If
            Me.View.RepositoryItemID = oItem.Id
            Me.View.RepositoryCommunityID = RepositoryCommunityID
            Me.View.Description = oItem.Description
            'If oItem.isFile Then
            '    Me.View.FileName = oItem.DisplayName
            'Else
            '    Me.View.FolderName = oItem.Name
            'End If
            View.ItemName = oItem.Name
            View.ItemExtension = IIf(oItem.isFile, oItem.Extension, "")
            Dim ExludeFolderID As Long = -1
            If oItem.FolderId = 0 Then
                Me.View.ItemPath = Me.View.BaseFolder & "/"
            Else
                Me.View.ItemPath = Me.View.BaseFolder & Me.CurrentManager.GetFolderPathName(oItem.FolderId)
            End If
            If Not oItem.isFile Then
                ExludeFolderID = oItem.Id
            End If
            Me.View.DownlodableByCommunity = Me.CurrentManager.HasCommunityAssignment(oItem.Id, False)
            Me.View.VisibleToDownloaders = oItem.isVisible
            Me.View.LoadFolderSelector(ExludeFolderID, oItem.FolderId, RepositoryCommunityID, oPermission.Administration, oPermission.Administration)
            Me.View.AllowDownload = oItem.IsDownloadable
            Me.View.AllowUpload = oItem.AllowUpload
            Me.View.InitializeView(oItem.isFile, oItem.RepositoryItemType)
        End Sub

        Public Sub Verify(ByVal FolderId As Long, ByVal FolderName As String)
            Dim oItem As CommunityFile = Me.CurrentManager.GetFileItemById(Me.View.RepositoryItemID)
            Dim oFolder As CommunityFile = Me.CurrentManager.GetFileItemById(FolderId)
            If IsNothing(oItem) Then
                Me.GotoManagementDownloadPage(oItem)
            ElseIf IsNothing(oFolder) AndAlso FolderId > 0 Then
                Me.View.ShowFolderDoesntExist(FolderName)
            Else
                If Me.CurrentManager.ExistInFolder(FolderId, oItem.CommunityOwner, oItem.Id, oItem.DisplayName, oItem.isFile) Then
                    Me.View.RepositoryFolderID = oItem.FolderId
                    If oItem.isFile Then
                        Me.View.ShowFileNameExist(FolderName, oItem.DisplayName)
                    Else
                        Me.View.ShowFolderExist(FolderName, oItem.Name)
                    End If
                Else
                    Me.View.RepositoryFolderID = FolderId
                    Me.View.ItemFolderChanged()
                End If
            End If
        End Sub
        Public Sub SaveItem(ByVal ToSubitems As Boolean)
            Dim oItem As CommunityFile = Me.CurrentManager.GetFileItemById(Me.View.RepositoryItemID)
            Dim FolderID As Long = Me.View.RepositoryFolderID
            Dim oFolder As CommunityFile = Me.CurrentManager.GetFileItemById(FolderID)

            If IsNothing(oItem) OrElse (IsNothing(oFolder) AndAlso FolderID > 0) Then
                Me.GotoManagementDownloadPage(oItem)
            Else
                Dim oSavedItem As CommunityFile = Nothing
                Dim SavedForCommunity As Boolean = Me.CurrentManager.HasCommunityAssignment(oItem.Id, False)
                Dim SavedVisibility As Boolean = oItem.isVisible
                Dim AllowUploadEdited As Boolean = (oItem.AllowUpload <> View.AllowUpload)
              
                oItem.isVisible = Me.View.VisibleToDownloaders
                oItem.IsDownloadable = Me.View.AllowDownload
                oItem.Description = Me.View.Description
                oItem.FolderId = FolderID
                If Not oItem.isFile Then : oItem.AllowUpload = View.AllowUpload
                End If
                If CurrentManager.ExistInFolder(FolderID, oItem.CommunityOwner, oItem.Id, Me.View.ItemName & oItem.Extension, oItem.isFile) Then
                    Dim ContainerName As String = Me.View.BaseFolder
                    If Not IsNothing(oFolder) Then
                        ContainerName = oFolder.Name
                    ElseIf FolderID > 0 Then
                        ContainerName = "-"
                    End If
                    View.RenameItemError(ContainerName, Me.View.ItemName & oItem.Extension, oItem.isFile)
                Else
                    ' If oItem.isFile = False Then
                    oItem.Name = Me.View.ItemName
                    ' End If

                    oSavedItem = Me.CurrentManager.SaveItem(oItem, Me.UserContext.CurrentUserID)
                    If Not IsNothing(oSavedItem) Then
                        Dim CommunityID As Integer = 0
                        If Not oSavedItem.CommunityOwner Is Nothing Then
                            CommunityID = oSavedItem.CommunityOwner.Id
                        End If
                        Dim FatherName As String = Me.View.BaseFolder
                        If FolderID > 0 Then
                            FatherName = oFolder.Name
                        End If
                        Dim UserID As Integer = 0
                        If Me.UserContext.CurrentUserID <> oSavedItem.Owner.Id Then
                            UserID = oSavedItem.Owner.Id
                        End If
                        Dim Permission As Long = COL_BusinessLogic_v2.UCServices.Services_File.Base2Permission.DownloadFile
                        If oSavedItem.AllowUpload Then : Permission = Permission Or COL_BusinessLogic_v2.UCServices.Services_File.Base2Permission.UploadFile
                        End If
                        If AllowUploadEdited AndAlso SavedForCommunity AndAlso SavedForCommunity = Me.View.DownlodableByCommunity Then
                            Me.CurrentManager.UpdateFolderPermission(Me.UserContext.CurrentUserID, oItem, ToSubitems, Permission)
                        End If
                        If SavedForCommunity <> Me.View.DownlodableByCommunity Then
                           
                            If Me.View.DownlodableByCommunity Then
                                Me.CurrentManager.ChangeItemAssignmentToCommunity(Me.UserContext.CurrentUserID, oItem, False, ToSubitems, Permission)
                            ElseIf IsNothing(oItem.CommunityOwner) Then
                                Me.CurrentManager.ChangePortalItemAssignmentToSome(Me.UserContext.CurrentUserID, oItem, False, ToSubitems, Permission)
                            Else
                                Me.CurrentManager.ChangeItemAssignmentToSome(Me.UserContext.CurrentUserID, oItem, False, ToSubitems, Permission)
                            End If
                            Me.View.NotifyPermissionChanged(Me.View.DownlodableByCommunity, UserID, CommunityID, oSavedItem.Id, oSavedItem.DisplayName, FolderID, FatherName, oSavedItem.isFile, oSavedItem.UniqueID, oSavedItem.isVisible, oSavedItem.RepositoryItemType)
                        ElseIf SavedVisibility <> oSavedItem.isVisible Then
                            Me.View.NotifyVisibilityChange(CommunityID, oSavedItem.DisplayName, oSavedItem.Id, FolderID, FatherName, oSavedItem.isFile, oSavedItem.UniqueID, oSavedItem.isVisible, oSavedItem.RepositoryItemType)
                        Else
                            'If oSavedItem.isSCORM Then
                            Me.View.NotifyItemChanges(CommunityID, UserID, oSavedItem.DisplayName, oSavedItem.Id, FolderID, FatherName, oSavedItem.UniqueID, oSavedItem.isVisible, oSavedItem.RepositoryItemType)
                            'Else
                            '    Me.View.NotifyItemChanges(CommunityID, UserID, oSavedItem.DisplayName, oSavedItem.Id, FolderID, FatherName, oSavedItem.isFile, oSavedItem.isVisible)
                            'End If
                        End If

                        Me.View.SendActionCompleted(CommunityID, Me.CommonManager.GetModuleID(UCServices.Services_File.Codex), oSavedItem.Id, oSavedItem.isFile, oSavedItem.isSCORM)
                        If Me.View.DownlodableByCommunity Then
                            Me.GotoManagementDownloadPage(oItem)
                        Else
                            Me.GotoPermissionManagement(oItem)
                        End If
                    Else
                        Me.GotoManagementDownloadPage(oItem)
                        End If
                End If
            End If
        End Sub

        Public Sub ChangePermissionSelector()
            Dim oItem As CommunityFile = Me.CurrentManager.GetFileItemById(Me.View.RepositoryItemID)
            If Not IsNothing(oItem) AndAlso Not oItem.isFile Then
                Me.View.AskToApplyToAllSubItems = Me.CurrentManager.HasCommunityAssignment(Me.View.RepositoryItemID, False) <> Me.View.DownlodableByCommunity
            Else
                Me.View.AskToApplyToAllSubItems = False
            End If
        End Sub
        Public Sub ChangeUploadDeleteFolderItems(ByVal AllowUpload As Boolean)
            Dim oItem As CommunityFile = Me.CurrentManager.GetFileItemById(Me.View.RepositoryItemID)
            If Not IsNothing(oItem) AndAlso Not oItem.isFile Then
                Me.View.AskToApplyToAllSubItems = (oItem.AllowUpload <> AllowUpload)
            Else
                Me.View.AskToApplyToAllSubItems = False
            End If
        End Sub
        Public Sub GotoManagementDownloadPage(ByVal oFile As CommunityFile)
            Dim CommunityID As Integer = Me.UserContext.CurrentCommunityID
            Dim FolderID As Long = 0
            If Not IsNothing(oFile) Then
                If oFile.CommunityOwner Is Nothing Then
                    CommunityID = 0
                Else
                    CommunityID = oFile.CommunityOwner.Id
                End If
                FolderID = oFile.FolderId
            End If
            Me.View.LoadRepositoryPage(CommunityID, FolderID, Me.View.PreloadedPreviousView, Me.View.PreLoadedPage)
        End Sub
        Public Sub GotoPermissionManagement(ByVal oFile As CommunityFile)
            Dim CommunityID As Integer = Me.UserContext.CurrentCommunityID
            Dim FolderID As Long = 0
            If Not IsNothing(oFile) Then
                If oFile.CommunityOwner Is Nothing Then
                    CommunityID = 0
                Else
                    CommunityID = oFile.CommunityOwner.Id
                End If
                FolderID = oFile.FolderId
                Me.View.LoadEditingPermission(oFile.Id, CommunityID, FolderID, Me.View.PreloadedPreviousView, Me.View.PreLoadedPage)
            Else
                Me.View.LoadRepositoryPage(CommunityID, FolderID, Me.View.PreloadedPreviousView, Me.View.PreLoadedPage)
            End If
        End Sub
    End Class
End Namespace