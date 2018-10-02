Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports COL_BusinessLogic_v2

Namespace lm.Comol.Modules.Base.Presentation
    Public Class WKIPublishFileToCommunity
        Inherits DomainPresenter

        Private _ModuleID As Integer
        Private _CommonManager As ManagerCommon
        Private _RepositoryManager As ManagerCommunityFiles
        Private ReadOnly Property ModuleID() As Integer
            Get
                If _ModuleID <= 0 Then
                    _ModuleID = Me.CommonManager.GetModuleID(UCServices.Services_WorkBook.Codex)
                End If
                Return _ModuleID
            End Get
        End Property

        Private Overloads Property RepositoryManager() As ManagerCommunityFiles
            Get
                Return _RepositoryManager
            End Get
            Set(ByVal value As ManagerCommunityFiles)
                _RepositoryManager = value
            End Set
        End Property
        Private Overloads Property CommonManager() As ManagerCommon
            Get
                Return _CommonManager
            End Get
            Set(ByVal value As ManagerCommon)
                _CommonManager = value
            End Set
        End Property
        Private Overloads Property CurrentManager() As ManagerWorkBook
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As ManagerWorkBook)
                _CurrentManager = value
            End Set
        End Property
        Private Overloads ReadOnly Property View() As IWKpublishFileToCommunity
            Get
                Return MyBase.View
            End Get
        End Property

        'Private _UserSubscriptionID As Integer
        'Private ReadOnly Property UserSubscriptionID(ByVal CommunityIdToUpload As Integer) As Integer
        '    Get
        '        If _UserSubscriptionID = 0 Then
        '            Dim oSubscription As New Comunita.COL_RuoloPersonaComunita
        '            oSubscription.Estrai(CommunityIdToUpload, Me.UserContext.CurrentUserID)
        '            _UserSubscriptionID = oSubscription.Id
        '        End If
        '        Return _UserSubscriptionID
        '    End Get
        'End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerWorkBook(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
            Me.RepositoryManager = New ManagerCommunityFiles(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IWKpublishFileToCommunity)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerWorkBook(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
            Me.RepositoryManager = New ManagerCommunityFiles(MyBase.AppContext)
        End Sub

        Public Sub InitView()
            Dim oItem As WorkBookItem = Me.CurrentManager.GetWorkBookItem(Me.View.PreloadedItemID)
            Dim CommunityID As Integer = Me.UserContext.CurrentCommunityID
            If Not IsNothing(oItem) AndAlso Not IsNothing(oItem.WorkBookOwner.CommunityOwner) Then
                CommunityID = oItem.WorkBookOwner.CommunityOwner.Id
            End If
            If Not Me.UserContext.isAnonymous Then
                Me.View.BackToManagement = oItem.Id
                If IsNothing(oItem) Then
                    Me.View.NoPermissionToManagementFiles()
                ElseIf HasPermission(oItem) Then
                    Dim oFilePermission As New ModuleCommunityRepository
                    If IsNothing(oItem.WorkBookOwner.CommunityOwner) Then
                        Me.View.InitCommunitySelection()
                        oFilePermission.UploadFile = (Me.View.CommunitySelectionLoaded)
                    Else
                        oFilePermission = Me.View.CommunityRepositoryPermission(oItem.WorkBookOwner.CommunityOwner.Id)
                    End If
                    If Not (oFilePermission.Administration OrElse oFilePermission.UploadFile) Then
                        Me.View.NoPermissionToPublishFiles()
                    Else
                        Me.View.SelectedFolder = -1
                        LoadFilesToPublish(oItem)
                    End If
                Else
                    Me.View.NoPermissionToPublishFiles()
                End If
            Else
                Me.View.NoPermissionToAccessPage(CommunityID, Me.ModuleID)
            End If
        End Sub
        'Me.View.InitializeFileSelector(oItem.WorkBookOwner.CommunityOwner.Id, oSelectedFiles, (oFilePermission.Administration))
        Private Function HasPermission(ByVal oItem As WorkBookItem) As Boolean
            Dim ModulePermission As ModuleWorkBook
            If oItem.WorkBookOwner.CommunityOwner Is Nothing Then : ModulePermission = ModuleWorkBook.CreatePortalmodule
            Else : ModulePermission = (From p In Me.View.CommunitiesPermission Where p.ID = oItem.WorkBookOwner.CommunityOwner.Id Select p.Permissions).FirstOrDefault
            End If

            Dim oPermission As WorkBookItemPermission = Me.CurrentManager.GetWorkBookItemPermission(Me.UserContext.CurrentUserID, oItem, ModulePermission)

            If IsNothing(oPermission) Then
                Return False
            Else
                Return oPermission.Read OrElse oPermission.Write
            End If
        End Function

        Private Sub LoadFilesToPublish(ByVal oItem As WorkBookItem)
            Dim oFiles As New List(Of GenericFilterItem(Of System.Guid, BaseFile))

            oFiles = (From f In Me.CurrentManager.NEW_WorkbookItemInternalFiles(oItem) Select New GenericFilterItem(Of System.Guid, BaseFile) With {.Id = f.Id, .Name = f.File}).ToList

            Dim oSelectedFiles As New List(Of System.Guid)
            If oFiles.Count > 0 Then
                oSelectedFiles.Add(Me.View.PreloadedFileID)
            End If
            Me.View.LoadWorkbookFiles(oFiles.OrderBy(Function(f) f.Name.DisplayName).ToList)
            Me.View.SelectedFilesID = oSelectedFiles
        End Sub

        Public Sub FindCommunityFolderOrCommunity()
            Dim oItem As WorkBookItem = Me.CurrentManager.GetWorkBookItem(Me.View.PreloadedItemID)
            If oItem.WorkBookOwner.CommunityOwner Is Nothing Then
                If Me.View.CommunitySelectionLoaded Then
                    Me.View.ShowSelectCommunity()
                Else
                    Me.View.InitCommunitySelection()
                End If
            Else
                Me.View.SelectedCommunityID = oItem.WorkBookOwner.CommunityOwner.Id
                Me.FindCommunityFolder()
            End If
        End Sub

        Public Sub FindCommunityFolder()
            Dim oItem As WorkBookItem = Me.CurrentManager.GetWorkBookItem(Me.View.PreloadedItemID)
            Dim oFilePermission As ModuleCommunityRepository = Me.View.CommunityRepositoryPermission(Me.View.SelectedCommunityID)

            Dim CommunityID As Integer = Me.View.SelectedCommunityID

            If Me.View.SelectedFolder = -1 Then
                Me.View.InitializeFolderSelector(Me.View.SelectedCommunityID, 0, (oFilePermission.Administration))
            End If
            Me.View.ShowFoldersList()

        End Sub
        Public Sub FindCommunityFolder(ByVal CommunityID As Integer)
            Dim oItem As WorkBookItem = Me.CurrentManager.GetWorkBookItem(Me.View.PreloadedItemID)
            Dim oFilePermission As ModuleCommunityRepository = Me.View.CommunityRepositoryPermission(CommunityID)

            Me.View.SelectedCommunityID = CommunityID
            If CommunityID > 0 Then
                If Me.View.SelectedFolder = -1 Then
                    Me.View.InitializeFolderSelector(CommunityID, 0, (oFilePermission.Administration))
                Else
                    Me.View.ShowFoldersList()
                End If
            Else
                Me.View.ShowSelectCommunity()
            End If
            
        End Sub
        Public Sub TryToPublish(ByVal FolderID As Long)
            Dim oSelectedFiles As List(Of System.Guid) = Me.View.SelectedFilesID
            Dim oItem As WorkBookItem = Me.CurrentManager.GetWorkBookItem(Me.View.PreloadedItemID)

            If oSelectedFiles.Count > 0 Then
                Dim oDuplicatedFiles As New List(Of WorkBookInternalFile)
                Dim oFileManager As New ManagerCommunityFiles(Me.DataContext)
                Dim oInternals As List(Of WorkBookInternalFile) = Me.CurrentManager.NEW_WorkbookItemInternalFilesById(oSelectedFiles)

                Dim oCommunity As Community = Me.CurrentManager.GetCommunity(Me.View.SelectedCommunityID)
                For Each oInternal As WorkBookInternalFile In oInternals
                    Dim oCommunityFile As CommunityFile = oFileManager.FindFile(oCommunity, FolderID, oInternal.File.Name, oInternal.File.Extension)

                    If Not IsNothing(oCommunityFile) Then
                        oDuplicatedFiles.Add(oInternal)
                    End If
                Next
                If oDuplicatedFiles.Count > 0 Then
                    Dim oFilesToChange As List(Of dtoFileExist(Of System.Guid))
                    oFilesToChange = (From dup In oDuplicatedFiles Select New dtoFileExist(Of System.Guid) With {.Id = dup.Id, .ExistFileName = dup.File.DisplayName, .ProposedFileName = dup.File.Name & "_" & Replace(Now.Date, "/", "-"), .Extension = dup.File.Extension}).ToList
                    Me.View.LoadMultipleFileName(oFilesToChange)
                Else
                    Me.GetSummary()
                    Me.View.ShowCompleteMessage()
                End If
            Else
                Me.View.ShowFileList()
            End If
        End Sub

        Public Sub GotoPreviousFromSummary()
            Dim oRenamedFilesID As List(Of System.Guid) = (From o In Me.View.GetChangedFileName Select o.Id).ToList
            If oRenamedFilesID.Count > 0 Then
                Me.View.ShowRenamedFileList()
            Else
                Me.View.ShowFoldersList()
            End If
        End Sub
        Public Sub GetSummary()
            Dim oItem As WorkBookItem = Me.CurrentManager.GetWorkBookItem(Me.View.PreloadedItemID)

            Dim FolderID As Long = Me.View.SelectedFolder


            Dim oRenamedFiles As List(Of dtoFileExist(Of System.Guid)) = Me.View.GetChangedFileName
            Dim oRenamedFilesID As List(Of System.Guid) = (From o In oRenamedFiles Select o.Id).ToList

            Dim oSelectedID As List(Of System.Guid) = (From id In Me.View.SelectedFilesID Where oRenamedFilesID.Contains(id) = False Select id).ToList


            Dim oList As New List(Of dtoFileToPublish)
            oList = (From f In Me.CurrentManager.NEW_WorkbookItemInternalFilesById(oSelectedID) Select New dtoFileToPublish() With { _
                     .FileID = f.Id, .FileName = f.File.Name, .Extension = f.File.Extension, .IsVisible = True}).ToList

            If oRenamedFiles.Count > 0 Then
                oList.AddRange((From f In oRenamedFiles Select New dtoFileToPublish() With {.FileID = f.Id, .FileName = f.ProposedFileName, .Extension = f.Extension, .IsVisible = True}).ToList)
            End If
            Dim oCommunity As Community = Me.CurrentManager.GetCommunity(Me.View.SelectedCommunityID)
            Me.View.LoadSummary(oCommunity.Name, Me.View.SelectedFolderName, oList.OrderBy(Function(d) d.FileName).ToList)
        End Sub

        Public Sub PublishIntoCommunityRepository(ByVal UserRepositoryFilePath As String, ByVal DefaultCategoryID As Integer)
            Dim oItem As WorkBookItem = Me.CurrentManager.GetWorkBookItem(Me.View.PreloadedItemID)
            If IsNothing(oItem) Then
                Me.View.ReturnToFileManagement(Me.View.PreloadedItemID)
            Else
                Dim FolderID As Long = Me.View.SelectedFolder
                ' All selected files to publish
                Dim oFilesToPublish As List(Of dtoFileToPublish) = Me.View.GetFilesToPublish
                If Me.View.SelectedCommunityID < 1 Then
                    Me.View.ReturnToFileManagement(Me.View.PreloadedItemID)
                Else
                    Dim CommunityID As Integer = Me.View.SelectedCommunityID
                    Dim oCommunity As Community = Me.CurrentManager.GetCommunity(CommunityID)
                    Dim virtualPath As String = ""
                    Dim oFileManager As New ManagerCommunityFiles(Me.DataContext)


                    Dim oResetFilesID As New List(Of System.Guid)
                    Dim oResetRenameFiles As New List(Of dtoFileExist(Of System.Guid))

                    For Each oFile In oFilesToPublish
                        Dim oInternalFile As WorkBookInternalFile = Me.CurrentManager.NEW_WorkbookItemInternalFile(oFile.FileID)
                        If Not IsNothing(oInternalFile) Then
                            Dim oCommunityFile As CommunityFile = oFileManager.FindFile(oCommunity, FolderID, oFile.FileName, oFile.Extension)

                            If IsNothing(oCommunityFile) Then
                                If Not AddFileTocommunityRepository(oInternalFile.File, FolderID, CommunityID, oFile.FileName, oFile.IsVisible, UserRepositoryFilePath, DefaultCategoryID) Then
                                    oResetFilesID.Add(oFile.FileID)
                                    If oFile.FileName <> oInternalFile.File.DisplayName Then
                                        oResetRenameFiles.Add(New dtoFileExist(Of System.Guid) With {.Id = oInternalFile.Id, .ExistFileName = oInternalFile.File.DisplayName, .ProposedFileName = oInternalFile.File.Name & "_" & Replace(Now.Date, "/", "-"), .Extension = oInternalFile.File.Extension})
                                    End If
                                End If
                            Else
                                oResetRenameFiles.Add(New dtoFileExist(Of System.Guid) With {.Id = oInternalFile.Id, .ExistFileName = oInternalFile.File.DisplayName, .ProposedFileName = oInternalFile.File.Name & "_" & Replace(Now.Date, "/", "-"), .Extension = oInternalFile.File.Extension})
                            End If
                        End If
                    Next
                    Me.View.SelectedFilesID = oResetFilesID
                    Me.View.LoadMultipleFileName(oResetRenameFiles)

                    If oResetFilesID.Count = 0 Then
                        Me.View.ReturnToFileManagement(Me.View.PreloadedItemID)
                    End If
                End If
            End If

        End Sub

        Private Function AddFileTocommunityRepository(ByVal oFile As BaseFile, ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal FileName As String, ByVal isVisible As Boolean, ByVal UserRepositoryFilePath As String, ByVal DefaultFileCategoryID As Integer) As Boolean
            Dim iResponse As Boolean = False
            Dim virtualPath As String = ""
            Dim oCommunityFile As CommunityFile = Me.CreateCommunityFile(oFile, FolderID, CommunityID, FileName, DefaultFileCategoryID)
            Dim AddedFile As CommunityFile = Nothing
            Try
                Dim fileStatus As ItemRepositoryStatus = ItemRepositoryStatus.None
                oCommunityFile.isVisible = isVisible
                AddedFile = Me.RepositoryManager.AddFile(oCommunityFile, UserRepositoryFilePath & oFile.Id.ToString & ".stored", UCServices.Services_File.Base2Permission.DownloadFile, fileStatus)
                If Not IsNothing(AddedFile) Then
                    Dim ParentFolder As String = Me.View.BaseFolder
                    If AddedFile.FolderId > 0 Then
                        ParentFolder = Me.RepositoryManager.GetFolderName(AddedFile.FolderId)
                    End If
                    Me.View.Notify(CommunityID, Me.ModuleID, AddedFile, ParentFolder)
                End If
            Catch ex As Exception

            End Try
            Return Not IsNothing(AddedFile)
        End Function

        Private Function CreateCommunityFile(ByVal oFile As BaseFile, ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal FileName As String, ByVal CategoryID As Integer) As CommunityFile
            Dim oCommunityFile As New CommunityFile
            With oCommunityFile
                .CloneID = 0
                .ContentType = oFile.ContentType
                .Description = oFile.Description
                .DisplayOrder = 1
                .Downloads = 0
                .FileCategoryID = CategoryID
                .FolderId = FolderID
                .isDeleted = False
                .isPersonal = False
                .isSCORM = False
                .isVirtual = False
                .isVideocast = False
                .IsDownloadable = True
                .isVisible = False
                .isFile = True
                .Level = 0
                .Name = FileName
                .Size = oFile.Size
                .UniqueID = System.Guid.NewGuid
                .Extension = oFile.Extension
                .CommunityOwner = Me.CommonManager.GetCommunity(CommunityID)
                If Not String.IsNullOrEmpty(.Extension) Then
                    .Extension = .Extension.ToLower
                End If
            End With
            Return oCommunityFile
        End Function

    End Class
End Namespace