Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports COL_BusinessLogic_v2
Imports lm.Comol.Core

Namespace lm.Comol.Modules.Base.Presentation
    Public Class SingleFileUploaderPresenter
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

        Public Overloads ReadOnly Property View() As IviewSingleFileUploader
            Get
                Return MyBase.View
            End Get
        End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IviewSingleFileUploader)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub


        Public Sub InitView(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal ForCreate As ItemRepositoryToCreate, ByVal oPermission As ModuleCommunityRepository)
            If FolderID <> 0 Then
                Dim oFolder As CommunityFile = Me.CurrentManager.GetFileItemById(FolderID)
                If IsNothing(oFolder) OrElse oFolder.isFile Then
                    FolderID = 0
                ElseIf Not IsNothing(oFolder.CommunityOwner) AndAlso oFolder.CommunityOwner.Id <> CommunityID Then
                    CommunityID = oFolder.CommunityOwner.Id
                End If
            End If
            Dim oCommunity As Community = Me.CurrentManager.GetCommunity(CommunityID)
            If CommunityID > 0 AndAlso oCommunity Is Nothing Then
                CommunityID = Me.UserContext.CurrentCommunityID
            End If

            Me.View.RepositoryCommunityID = CommunityID
            If CommunityID = 0 Then
                Me.View.CommunityName = Me.View.Portalname
            Else
                oCommunity = Me.CurrentManager.GetCommunity(CommunityID)
                Me.View.CommunityName = oCommunity.Name
            End If

            Me.View.AllowFolderChange = Me.CurrentManager.CommunityHasFolders(CommunityID)
            Me.View.FolderID = FolderID
            Dim FolderPath As String = Me.View.BaseFolder
            If FolderID > 0 Then
                Dim oFolder As CommunityFile = Me.CurrentManager.GetFileItemById(FolderID)
                FolderPath &= Me.CurrentManager.GetFolderPathName(FolderID)
            End If
            Me.View.FilePath = FolderPath
            Me.View.ItemType = Repository.RepositoryItemType.FileStandard
            Me.View.AllowDownload = True
            Me.View.Description = ""
            'If ForCreate = ItemRepositoryToCreate.Folder Then
            '    '0Me.View.FolderName = ""
            '    Me.View.AllowUpload = True
            'Else
            '    Me.View.AllowUpload = False
            'End If
            Me.View.DownlodableByCommunity = True
            Me.View.VisibleToDonwloaders = True
            Me.View.LoadFolderSelector(FolderID, CommunityID, oPermission.Administration, oPermission.Administration)
        End Sub

        Public Function AddFileToRepository(ByVal oFile As CommunityFile, ByVal SavedFile As String, ByVal CommunityPath As String) As dtoUploadedFile
            Dim iResponse As New dtoUploadedFile(oFile, ItemRepositoryStatus.CreationError)
            If Not IsNothing(oFile) AndAlso File.Exists.File(SavedFile) Then

                Dim SavedFolder As CommunityFile = Nothing
                Dim CommunityID As Integer = Me.View.RepositoryCommunityID
                Dim oCommunity As Community = Me.CurrentManager.GetCommunity(CommunityID)
                Dim fileStatus As ItemRepositoryStatus = ItemRepositoryStatus.None
                If (CommunityID > 0 AndAlso Not IsNothing(CommunityID)) OrElse (IsNothing(oCommunity) AndAlso CommunityID = 0) Then
                    oFile.Owner = Me.CommonManager.GetPerson(Me.UserContext.CurrentUserID)
                    oFile.CommunityOwner = oCommunity
                    oFile.CreatedOn = Now
                    oFile.ModifiedOn = Now
                    oFile.ModifiedBy = oFile.Owner
                    Dim Permission As Long = UCServices.Services_File.Base2Permission.DownloadFile
                    If Me.View.DownlodableByCommunity Then
                        SavedFolder = Me.CurrentManager.AddFile(oFile, CommunityPath, Permission, fileStatus)
                    Else
                        SavedFolder = Me.CurrentManager.AddFile(oFile, oFile.Owner, CommunityPath, Permission, fileStatus)
                    End If
                    If Not IsNothing(SavedFolder) Then
                        iResponse.Link = (Me.CurrentManager.CreateModuleActionLink(SavedFolder, UCServices.Services_File.Base2Permission.DownloadFile, Me.CurrentManager.ModuleId))
                    End If
                End If

                iResponse.Status = fileStatus
                'If IsNothing(SavedFolder) Then
                '    iResponse.Status = ItemRepositoryStatus.FileExist
                '    iResponse.
                'ElseIf SavedFolder.Id > 0 Then
                '    iResponse.Status = ItemRepositoryStatus.FileUploaded
                'End If
            End If
            Return iResponse
        End Function

        Public Function AddFolderToRepository(ByVal oFolder As CommunityFile) As dtoUploadedFile
            Dim iResponse As New dtoUploadedFile(oFolder, ItemRepositoryStatus.CreationError)
            If Not IsNothing(oFolder) Then
                Dim SavedFolder As CommunityFile = Nothing
                Dim CommunityID As Integer = Me.View.RepositoryCommunityID
                Dim oCommunity As Community = Me.CurrentManager.GetCommunity(CommunityID)
                Dim fileStatus As ItemRepositoryStatus = ItemRepositoryStatus.None
                If (CommunityID > 0 AndAlso Not IsNothing(CommunityID)) OrElse (IsNothing(oCommunity) AndAlso CommunityID = 0) Then
                    oFolder.Owner = Me.CommonManager.GetPerson(Me.UserContext.CurrentUserID)
                    oFolder.CommunityOwner = Me.CurrentManager.GetCommunity(CommunityID)
                    oFolder.CreatedOn = Now
                    oFolder.ModifiedOn = Now
                    oFolder.ModifiedBy = oFolder.Owner
                    oFolder.AllowUpload = View.AllowUpload
                    Dim Permission As Long = UCServices.Services_File.Base2Permission.DownloadFile
                    If oFolder.AllowUpload Then : Permission = Permission Or UCServices.Services_File.Base2Permission.UploadFile
                    End If
                    If Me.View.DownlodableByCommunity Then
                        SavedFolder = Me.CurrentManager.AddFolder(oFolder, Permission, fileStatus)
                    Else
                        SavedFolder = Me.CurrentManager.AddFolder(oFolder, oFolder.Owner, Permission, fileStatus)
                    End If
                    If Not IsNothing(SavedFolder) Then
                        'iResponse.Link = (Me.CurrentManager.CreateModuleActionLink(SavedFolder, UCServices.Services_File.Base2Permission.DownloadFile, _
                        '        UCServices.Services_File.ActionType.DownloadFile, CurrentManager.ModuleId))
                        iResponse.Link = (Me.CurrentManager.CreateModuleActionLink(SavedFolder, UCServices.Services_File.Base2Permission.DownloadFile, CurrentManager.ModuleId))
                    End If
                End If

                '  If IsNothing(SavedFolder) Then
                iResponse.Status = fileStatus
                'ElseIf SavedFolder.Id > 0 Then
                '   iResponse.Status = ItemRepositoryStatus.FolderCreated
                'End If
            End If
            Return iResponse
        End Function

        Public Function GetFolderName(ByVal FolderID As Long) As String
            Return Me.CurrentManager.GetFolderName(FolderID)
        End Function
        Public Function GetFolderPathName(ByVal FolderID As Long) As String
            Return Me.CurrentManager.GetFolderPathName(FolderID)
        End Function
        Public Function HasPermissionToUploadIntoFolder(ByVal FolderID As Long, ByVal oModule As ModuleCommunityRepository) As String
            Return Me.CurrentManager.HasPermissionToUploadIntoFolder(FolderID, UserContext.CurrentUserID, oModule)
        End Function
    End Class
End Namespace