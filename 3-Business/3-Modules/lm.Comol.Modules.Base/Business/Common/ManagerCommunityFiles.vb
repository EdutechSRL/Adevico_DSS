Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Repository
Imports NHibernate
Imports NHibernate.Linq
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.File

Namespace lm.Comol.Modules.Base.BusinessLogic
    Public Class ManagerCommunityFiles
        Implements lm.Comol.Core.DomainModel.Common.iDomainManager
        Implements lm.Comol.Core.DomainModel.iLinkedNHibernateService



        Private Const UniqueCode As String = "SRVMATER"
        Private _ModuleID As Integer
        Public ReadOnly Property ModuleId
            Get
                If (_ModuleID = 0) Then
                    _ModuleID = (From m In DC.GetCurrentSession.Linq(Of ModuleDefinition)() Where m.Code.Equals(UniqueCode) Select m.Id).FirstOrDefault()
                End If
                Return _ModuleID
            End Get
        End Property


#Region "Private property"
        'Private _UserContext As iUserContext
        Private _Datacontext As iDataContext
        Private _IcodeonContext As iDataContext
#End Region

#Region "Public property"
        Private ReadOnly Property DC() As iDataContext
            Get
                Return _Datacontext
            End Get
        End Property
        Private ReadOnly Property IC() As iDataContext
            Get
                Return _IcodeonContext
            End Get
        End Property
        'Private ReadOnly Property CurrentUserContext() As iUserContext
        '    Get
        '        Return _UserContext
        '    End Get
        'End Property
#End Region

        Public Sub New()
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext)
            '  Me._UserContext = oContext.UserContext
            Me._Datacontext = oContext.DataContext
        End Sub
        Public Sub New(ByVal oDatacontext As iDataContext)
            ' Me._UserContext = oUserContext
            Me._Datacontext = oDatacontext
        End Sub
        Public Sub New(ByVal oDatacontext As iDataContext, ByVal oIcodeonContext As iDataContext)
            ' Me._UserContext = oUserContext
            Me._Datacontext = oDatacontext
            Me._IcodeonContext = oIcodeonContext
        End Sub

        Public Function GetAnonymousIdUser() As Integer
            Dim idUser As Integer = 0

            Try
                idUser = (From p In DC.GetCurrentSession.Linq(Of Person)() Where p.TypeID = CInt(UserTypeStandard.Guest) Select p.Id).Skip(0).Take(1).ToList().FirstOrDefault()
            Catch ex As Exception
            End Try
            Return idUser
        End Function
        Public Function GetCommunity(ByVal CommunityID As Integer) As Community
            Dim oCommunity As Community = Nothing

            Try
                DC.BeginTransaction()
                oCommunity = _Datacontext.GetById(Of Community)(CommunityID)
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
            Return oCommunity
        End Function

        Public Function GetItem(ByVal ItemID As Long) As BaseCommunityFile
            Dim oFile As BaseCommunityFile = Nothing
            Try
                oFile = DC.GetById(Of BaseCommunityFile)(ItemID)
            Catch ex As Exception
            End Try

            Return oFile
        End Function

        Public Function GetItems(ByVal oCommunity As Community, ByVal FolderId As Long, ByVal ShowAllFiles As Boolean, ByVal AdminPurpose As Boolean, ByVal oPerson As Person) As List(Of CommunityFile)
            Dim oCommunityFiles As New List(Of CommunityFile)
            Try
                DC.BeginTransaction()
                If Not AdminPurpose Then
                    Dim oRole As Role = (From s As Subscription In DC.GetCurrentSession.Linq(Of Subscription)() Where s.Community Is oCommunity AndAlso s.Person Is oPerson Select s.Role).FirstOrDefault
                    Dim ItemsID As List(Of Long) = (From file In DC.GetCurrentSession.Linq(Of CommunityFile)() _
                                       Where file.CommunityOwner Is oCommunity AndAlso file.FolderId = FolderId _
                                       AndAlso (ShowAllFiles = True OrElse (file.isVisible OrElse file.Owner Is oPerson)) Select file.Id).ToList

                    oCommunityFiles = (From fa In DC.GetCurrentSession.Linq(Of CommunityFileCommunityAssignment)() Where ItemsID.Contains(fa.File.Id) AndAlso fa.Inherited AndAlso fa.AssignedTo Is oCommunity AndAlso Not fa.Deny Select fa.File).ToList
                    oCommunityFiles.AddRange((From fa In DC.GetCurrentSession.Linq(Of CommunityFileRoleAssignment)() Where Not oCommunityFiles.Contains(fa.File) AndAlso ItemsID.Contains(fa.File.Id) AndAlso fa.Inherited AndAlso fa.AssignedTo Is oRole AndAlso Not fa.Deny Select fa.File).ToList)
                    oCommunityFiles.AddRange((From fa In DC.GetCurrentSession.Linq(Of CommunityFilePersonAssignment)() Where Not oCommunityFiles.Contains(fa.File) AndAlso ItemsID.Except((From f In oCommunityFiles Select f.Id).ToList).Contains(fa.File.Id) AndAlso fa.Inherited AndAlso fa.AssignedTo Is oPerson AndAlso Not fa.Deny Select fa.File).ToList)
                Else
                    oCommunityFiles = (From file In DC.GetCurrentSession.Linq(Of CommunityFile)() _
                                       Where file.CommunityOwner Is oCommunity AndAlso file.FolderId = FolderId _
                                       AndAlso (ShowAllFiles = True OrElse (file.isVisible OrElse file.Owner Is oPerson)) Select file).ToList
                End If

                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try

            Return oCommunityFiles
        End Function
        Public Function FindFile(ByVal oCommunity As Community, ByVal FolderId As Long, ByVal FileName As String, ByVal FileExtension As String) As CommunityFile
            Dim oCommunityFile As CommunityFile = Nothing
            Try
                DC.BeginTransaction()

                Dim oQuery = (From file In DC.GetCurrentSession.Linq(Of CommunityFile)() Where (file.CommunityOwner Is oCommunity) AndAlso (file.FolderId = FolderId) AndAlso file.isFile Select file)
                oCommunityFile = (From f In oQuery.ToList Where (f.Name.ToLower = FileName.ToLower) AndAlso (FileExtension.ToLower = f.Extension.ToLower) Select f).FirstOrDefault
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try

            Return oCommunityFile
        End Function
        Public Function GetCommunityFilesById(ByVal oList As List(Of Long)) As List(Of CommunityFile)
            Dim oCommunityFiles As New List(Of CommunityFile)
            Try
                DC.BeginTransaction()
                Dim oQuery = (From file In DC.GetCurrentSession.Linq(Of CommunityFile)() Where oList.Contains(file.Id) Select file)

                oCommunityFiles = oQuery.ToList
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try

            Return oCommunityFiles
        End Function

        Public Function GetFolders(ByVal oCommunity As Community, ByVal FolderId As Long, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean, ByVal oPerson As Person) As List(Of CommunityFile)
            Dim oFolders As New List(Of CommunityFile)
            Try
                DC.BeginTransaction()

                If Not AdminPurpose Then
                    Dim oRole As Role = (From s As Subscription In DC.GetCurrentSession.Linq(Of Subscription)() Where s.Community Is oCommunity AndAlso s.Person Is oPerson Select s.Role).FirstOrDefault
                    Dim FoldersID As List(Of Long) = (From file In DC.GetCurrentSession.Linq(Of CommunityFile)() _
                                       Where file.CommunityOwner Is oCommunity AndAlso file.FolderId = FolderId AndAlso file.isFile = False _
                                       AndAlso (ShowHiddenItems = True OrElse (file.isVisible OrElse file.Owner Is oPerson)) Select file.Id).ToList

                    oFolders = (From fa In DC.GetCurrentSession.Linq(Of CommunityFileCommunityAssignment)() Where FoldersID.Contains(fa.File.Id) AndAlso fa.Inherited AndAlso fa.AssignedTo Is oCommunity AndAlso Not fa.Deny Select fa.File).ToList
                    oFolders.AddRange((From fa In DC.GetCurrentSession.Linq(Of CommunityFileRoleAssignment)() Where Not oFolders.Contains(fa.File) AndAlso FoldersID.Contains(fa.File.Id) AndAlso fa.Inherited AndAlso fa.AssignedTo Is oRole AndAlso Not fa.Deny Select fa.File).ToList)

                    Dim oPersonalFolders As List(Of CommunityFile) = (From fa In DC.GetCurrentSession.Linq(Of CommunityFilePersonAssignment)() _
                                     Where Not oFolders.Contains(fa.File) _
                                     AndAlso fa.Inherited AndAlso fa.AssignedTo Is oPerson AndAlso Not fa.Deny Select fa.File).ToList

                    oFolders.AddRange((From p In oPersonalFolders Where FoldersID.Except((From f In oFolders Select f.Id).ToList).Contains(p.Id)).ToList)

                Else
                    oFolders = (From file In DC.GetCurrentSession.Linq(Of CommunityFile)() _
                                       Where file.CommunityOwner Is oCommunity AndAlso file.FolderId = FolderId AndAlso file.isFile = False _
                                       AndAlso (ShowHiddenItems = True OrElse (file.isVisible OrElse file.Owner Is oPerson)) Select file).ToList
                End If
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try

            Return oFolders.OrderBy(Function(f) f.Name).ToList
        End Function

        Public Function GetFoldersForUpload(ByVal oCommunity As Community, ByVal ExludeFolderID As Long, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean, ByVal oPerson As Person) As List(Of CommunityFile)
            Dim oFolders As New List(Of CommunityFile)
            Try
                DC.BeginTransaction()

                If Not AdminPurpose Then
                    Dim oRole As Role = (From s As Subscription In DC.GetCurrentSession.Linq(Of Subscription)() Where s.Community Is oCommunity AndAlso s.Person Is oPerson Select s.Role).FirstOrDefault
                    Dim FoldersID As List(Of Long) = (From file In DC.GetCurrentSession.Linq(Of CommunityFile)() _
                                       Where file.CommunityOwner Is oCommunity AndAlso file.Id <> ExludeFolderID AndAlso file.isFile = False _
                                       AndAlso (ShowHiddenItems = True OrElse (file.isVisible OrElse file.Owner Is oPerson)) Select file.Id).ToList

                    oFolders = (From fa In DC.GetCurrentSession.Linq(Of CommunityFileCommunityAssignment)() Where FoldersID.Contains(fa.File.Id) AndAlso fa.Inherited AndAlso fa.AssignedTo Is oCommunity AndAlso Not fa.Deny Select fa.File).ToList
                    oFolders.AddRange((From fa In DC.GetCurrentSession.Linq(Of CommunityFileRoleAssignment)() Where Not oFolders.Contains(fa.File) AndAlso FoldersID.Contains(fa.File.Id) AndAlso fa.Inherited AndAlso fa.AssignedTo Is oRole AndAlso Not fa.Deny Select fa.File).ToList)

                    Dim oPersonalFolders As List(Of CommunityFile) = (From fa In DC.GetCurrentSession.Linq(Of CommunityFilePersonAssignment)() _
                                     Where Not oFolders.Contains(fa.File) _
                                     AndAlso fa.Inherited AndAlso fa.AssignedTo Is oPerson AndAlso Not fa.Deny Select fa.File).ToList

                    oFolders.AddRange((From p In oPersonalFolders Where FoldersID.Except((From f In oFolders Select f.Id).ToList).Contains(p.Id)).ToList)

                Else
                    oFolders = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)() _
                                       Where f.CommunityOwner Is oCommunity AndAlso f.Id <> ExludeFolderID AndAlso f.isFile = False _
                                       AndAlso (ShowHiddenItems = True OrElse (f.isVisible OrElse f.Owner Is oPerson)) Select f).ToList
                End If
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try

            Return oFolders.OrderBy(Function(f) f.Name).ToList
        End Function

        Public Function GetFileItemById(ByVal FileId As Long) As CommunityFile
            Dim oCommunityFile As CommunityFile = Nothing
            Try
                DC.BeginTransaction()
                oCommunityFile = DC.GetCurrentSession.Get(Of CommunityFile)(FileId)
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try

            Return oCommunityFile
        End Function
#Region "Folders"
        Public Function GetFoldersWithPermission(ByVal oCommunity As Community, ByVal FolderId As Long, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean, ByVal oPerson As Person) As List(Of CommunityFile)
            Return GetFolderItems(False, oCommunity, FolderId, ShowHiddenItems, AdminPurpose, oPerson).OrderBy(Function(f) f.Name).ToList
        End Function
        Public Function GetFoldersCount(ByVal oCommunity As Community, ByVal FolderId As Long, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean, ByVal oPerson As Person) As Integer
            Return GetFolderItemsCount(False, oCommunity, FolderId, ShowHiddenItems, AdminPurpose, oPerson)
        End Function
#End Region

#Region "Folder Content"
        Public Function GetFolderContent(ByVal CommunityID As Integer, ByVal FolderId As Long, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean, ByVal PersonID As Integer) As List(Of CommunityFile)
            Dim oCommunityFiles As New List(Of CommunityFile)
            Try
                DC.BeginTransaction()
                Dim oCommunity As Community = DC.GetCurrentSession.Get(Of Community)(CommunityID)
                Dim oPerson As Person = DC.GetCurrentSession.Get(Of Person)(PersonID)
                DC.Commit()
                If (CommunityID = 0 OrElse Not IsNothing(oCommunity)) AndAlso Not IsNothing(oPerson) Then
                    oCommunityFiles.AddRange(GetFoldersWithPermission(oCommunity, FolderId, ShowHiddenItems, AdminPurpose, oPerson))
                    oCommunityFiles.AddRange(GetFolderFiles(oCommunity, FolderId, ShowHiddenItems, AdminPurpose, oPerson))
                End If
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
            Return oCommunityFiles
        End Function

        Public Function GetFolderContentCount(ByVal CommunityID As Integer, ByVal FolderId As Long, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean, ByVal PersonID As Integer) As Integer
            Dim iResponse As Integer = 0
            Try
                DC.BeginTransaction()
                Dim oCommunity As Community = DC.GetCurrentSession.Get(Of Community)(CommunityID)
                Dim oPerson As Person = DC.GetCurrentSession.Get(Of Person)(PersonID)
                DC.Commit()
                If (CommunityID = 0 OrElse Not IsNothing(oCommunity)) AndAlso Not IsNothing(oPerson) Then
                    iResponse = GetFoldersCount(oCommunity, FolderId, ShowHiddenItems, AdminPurpose, oPerson)
                    iResponse += GetFolderFilesCount(oCommunity, FolderId, ShowHiddenItems, AdminPurpose, oPerson)
                End If
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
            Return iResponse
        End Function

        Public Function AddFolder(ByVal oFolder As CommunityFile, ByVal Permission As Long, ByRef Status As ItemRepositoryStatus) As CommunityFile
            Dim oResponse As CommunityFile = Nothing
            Try
                DC.BeginTransaction()
                Me.VerifyAndUpdateFolderName(oFolder)
                Dim oFolderNames As List(Of String) = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)() Where f.CommunityOwner Is oFolder.CommunityOwner AndAlso f.FolderId = oFolder.FolderId AndAlso f.isFile = False AndAlso f.Name.StartsWith(oFolder.Name, StringComparison.InvariantCultureIgnoreCase) Select f.Name).ToList
                If Not (From name In oFolderNames Where name.ToLower = oFolder.Name.ToLower).Any Then
                    DC.GetCurrentSession.SaveOrUpdate(oFolder)
                    'Dim oAssignment As New CommunityFileCommunityAssignment
                    'oAssignment.AssignedTo = oFolder.CommunityOwner
                    'oAssignment.Deny = False
                    'oAssignment.File = oFolder
                    'oAssignment.Permission = 10
                    'oAssignment.CreatedBy = oFolder.Owner
                    'oAssignment.CreatedOn = oFolder.CreatedOn
                    'DC.GetCurrentSession.SaveOrUpdate(oAssignment)
                    Me.AddCommunityAssignmentToItem(oFolder.CreatedOn, oFolder.Owner, oFolder.Id, False, Permission, False)
                    If oFolder.FolderId = 0 Then
                        Me.AddCommunityAssignmentToItem(oFolder.CreatedOn, oFolder.Owner, oFolder.Id, False, Permission, True)
                    Else
                        Me.ApplyInheritedAssignment(oFolder.CreatedOn, oFolder.Owner, oFolder, (From fa In DC.GetCurrentSession.Linq(Of CommunityFileAssignment)() Where fa.File.Id = oFolder.FolderId AndAlso fa.Inherited Select fa).ToList, False, Permission)
                    End If
                    oResponse = oFolder
                    Status = ItemRepositoryStatus.FolderCreated
                Else
                    Status = ItemRepositoryStatus.FolderExist
                    oResponse = Nothing
                End If
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                If Status <> ItemRepositoryStatus.FolderExist Then
                    Status = ItemRepositoryStatus.CreationError
                End If
            End Try
            Return oResponse
        End Function
        Public Function AddFolder(ByVal oFolder As CommunityFile, ByVal oPerson As Person, ByVal Permission As Long, ByRef Status As ItemRepositoryStatus) As CommunityFile
            Dim oResponse As CommunityFile = Nothing
            Try
                DC.BeginTransaction()
                Me.VerifyAndUpdateFolderName(oFolder)
                Dim oFolderNames As List(Of String) = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)() Where f.CommunityOwner Is oFolder.CommunityOwner AndAlso f.FolderId = oFolder.FolderId AndAlso f.isFile = False AndAlso f.Name.StartsWith(oFolder.Name, StringComparison.InvariantCultureIgnoreCase) Select f.Name).ToList
                If Not (From name In oFolderNames Where name.ToLower = oFolder.Name.ToLower).Any Then
                    DC.GetCurrentSession.SaveOrUpdate(oFolder)
                    'Dim oAssignment As New CommunityFilePersonAssignment
                    'oAssignment.AssignedTo = oPerson
                    'oAssignment.Deny = False
                    'oAssignment.File = oFolder
                    'oAssignment.Permission = 10
                    'oAssignment.CreatedBy = oFolder.Owner
                    'oAssignment.CreatedOn = oFolder.CreatedOn
                    'DC.GetCurrentSession.SaveOrUpdate(oAssignment)
                    Me.AddPersonAssignmentToItem(oFolder.CreatedOn, oFolder.Owner, oFolder, oPerson, False, Permission, False)
                    If oFolder.FolderId = 0 Then
                        Me.AddPersonAssignmentToItem(oFolder.CreatedOn, oFolder.Owner, oFolder, oPerson, False, Permission, True)
                    Else
                        Me.ApplyInheritedAssignment(oFolder.CreatedOn, oFolder.Owner, oFolder, (From fa In DC.GetCurrentSession.Linq(Of CommunityFileAssignment)() Where fa.File.Id = oFolder.FolderId AndAlso fa.Inherited Select fa).ToList, False, Permission)
                    End If
                    oResponse = oFolder
                    Status = ItemRepositoryStatus.FolderCreated
                Else
                    Status = ItemRepositoryStatus.FolderExist
                    oResponse = Nothing
                End If
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                If Status <> ItemRepositoryStatus.FolderExist Then
                    Status = ItemRepositoryStatus.CreationError
                End If
            End Try
            Return oResponse
        End Function
#End Region

        Public Function AddFile(ByVal file As CommunityFile, ByVal CommunityPath As String, ByVal Permission As Long, ByRef Status As ItemRepositoryStatus) As CommunityFile
            Dim oResponse As CommunityFile = Nothing
            Try
                DC.BeginTransaction()
                Me.VerifyAndUpdateItemName(file)
                Dim oFileNames As List(Of String) = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)() Where f.CommunityOwner Is file.CommunityOwner AndAlso f.FolderId = file.FolderId AndAlso f.isFile AndAlso f.DisplayName.StartsWith(file.DisplayName, StringComparison.InvariantCultureIgnoreCase) Select f.Name).ToList
                If Not (From name In oFileNames Where name.ToLower = file.Name.ToLower).Any Then
                    DC.GetCurrentSession.SaveOrUpdate(file)

                    Me.AddCommunityAssignmentToItem(file.CreatedOn, file.Owner, file.Id, False, Permission, False)
                    If file.FolderId = 0 Then
                        Me.AddCommunityAssignmentToItem(file.CreatedOn, file.Owner, file.Id, False, Permission, True)
                    Else
                        Me.ApplyInheritedAssignment(file.CreatedOn, file.Owner, file, (From fa In DC.GetCurrentSession.Linq(Of CommunityFileAssignment)() Where fa.File.Id = file.FolderId AndAlso fa.Inherited Select fa).ToList, False, Permission)
                    End If

                    If (file.isFile AndAlso file.RepositoryItemType <> RepositoryItemType.FileStandard) Then
                        AddFileForTransfer(file, CommunityPath)
                    End If
                    'If oFile.isSCORM Then
                    '    Dim oScorm As New ScormFile()
                    '    oScorm.FileUniqueID = oFile.UniqueID
                    '    oScorm.File = oFile
                    '    If Not CommunityPath.EndsWith("\") Then
                    '        oScorm.Path = CommunityPath & "\"
                    '    Else
                    '        oScorm.Path = CommunityPath
                    '    End If

                    '    oScorm.FileName = oFile.UniqueID.ToString & ".stored"
                    '    DC.GetCurrentSession.SaveOrUpdate(oScorm)
                    'End If
                    oResponse = file
                    Status = ItemRepositoryStatus.FileUploaded
                Else
                    Status = ItemRepositoryStatus.FileExist
                    oResponse = Nothing
                End If
                DC.Commit()
            Catch ex As Exception
                oResponse = Nothing
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                If Status <> ItemRepositoryStatus.FileExist Then
                    Status = IIf(file.isFile, ItemRepositoryStatus.UploadError, ItemRepositoryStatus.CreationError)
                End If
            End Try
            Return oResponse
        End Function
        Public Function AddFile(ByVal file As CommunityFile, ByVal oPerson As Person, ByVal CommunityPath As String, ByVal Permission As Long, ByRef Status As ItemRepositoryStatus) As CommunityFile
            Dim oResponse As CommunityFile = Nothing
            Try
                DC.BeginTransaction()
                Me.VerifyAndUpdateItemName(file)
                Dim oFileNames As List(Of String) = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)() Where f.CommunityOwner Is file.CommunityOwner AndAlso f.FolderId = file.FolderId AndAlso f.isFile AndAlso f.DisplayName.StartsWith(file.DisplayName, StringComparison.InvariantCultureIgnoreCase) Select f.Name).ToList
                If Not (From name In oFileNames Where name.ToLower = file.Name.ToLower).Any Then
                    DC.GetCurrentSession.SaveOrUpdate(file)
                    'Dim oAssignment As New CommunityFilePersonAssignment
                    'oAssignment.AssignedTo = oPerson
                    'oAssignment.Deny = False
                    'oAssignment.File = oFile
                    'oAssignment.Permission = 10
                    'oAssignment.CreatedBy = oFile.Owner
                    'oAssignment.CreatedOn = oFile.CreatedOn
                    'DC.GetCurrentSession.SaveOrUpdate(oAssignment)
                    Me.AddPersonAssignmentToItem(file.CreatedOn, file.Owner, file, file.Owner, False, Permission, False)
                    If file.FolderId = 0 Then
                        Me.AddPersonAssignmentToItem(file.CreatedOn, file.Owner, file, file.Owner, False, Permission, True)
                    Else
                        Me.ApplyInheritedAssignment(file.CreatedOn, file.Owner, file, (From fa In DC.GetCurrentSession.Linq(Of CommunityFileAssignment)() Where fa.File.Id = file.FolderId AndAlso fa.Inherited Select fa).ToList, False, Permission)
                    End If

                    If (file.isFile AndAlso file.RepositoryItemType <> RepositoryItemType.FileStandard) Then
                        AddFileForTransfer(file, CommunityPath)
                    End If


                    'If oFile.isSCORM Then
                    '    Dim oScorm As New ScormFile()
                    '    oScorm.FileUniqueID = oFile.UniqueID
                    '    oScorm.File = oFile
                    '    If Not CommunityPath.EndsWith("\") Then
                    '        oScorm.Path = CommunityPath & "\"
                    '    Else
                    '        oScorm.Path = CommunityPath
                    '    End If
                    '    oScorm.FileName = oFile.UniqueID.ToString & ".stored"
                    '    DC.GetCurrentSession.SaveOrUpdate(oScorm)
                    'End If
                    Status = ItemRepositoryStatus.FileUploaded
                    oResponse = file
                Else
                    Status = ItemRepositoryStatus.FileExist
                    oResponse = Nothing
                End If
                DC.Commit()
            Catch ex As Exception
                oResponse = Nothing
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                If Status <> ItemRepositoryStatus.FileExist Then
                    Status = ItemRepositoryStatus.CreationError
                End If
            End Try
            Return oResponse
        End Function

        Private Sub AddFileForTransfer(file As BaseCommunityFile, destinationPath As String, cloneId As System.Guid)
            Dim fileTransfer As New FileTransfer()

            fileTransfer.FileUniqueID = file.UniqueID
            fileTransfer.File = file
            fileTransfer.CloneId = cloneId
            If (destinationPath.EndsWith("\")) Then
                fileTransfer.Path = destinationPath
            Else
                fileTransfer.Path = destinationPath & "\"
            End If
            fileTransfer.Filename = file.UniqueID.ToString() + ".stored"
            Select Case file.RepositoryItemType
                Case RepositoryItemType.ScormPackage
                    Dim scorm As New ScormFileTransfer(fileTransfer)
                    scorm.Decompress = True
                    scorm.isCompleted = False
                    DC.GetCurrentSession.SaveOrUpdate(scorm)
                Case RepositoryItemType.Multimedia
                    Dim multimedia As New MultimediaFileTransfer(fileTransfer)
                    multimedia.Decompress = True
                    multimedia.isCompleted = False
                    DC.GetCurrentSession.SaveOrUpdate(multimedia)
            End Select
        End Sub
        Private Sub AddFileForTransfer(file As BaseCommunityFile, communityPath As String)
            AddFileForTransfer(file, communityPath, System.Guid.Empty)
        End Sub


#Region "Files of a Folder"
        Public Function GetFolderFiles(ByVal CommunityID As Integer, ByVal FolderId As Long, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean, ByVal PersonID As Integer) As List(Of CommunityFile)
            Try
                DC.BeginTransaction()
                Dim oCommunity As Community = DC.GetCurrentSession.Get(Of Community)(CommunityID)
                Dim oPerson As Person = DC.GetCurrentSession.Get(Of Person)(PersonID)
                DC.Commit()
                If (CommunityID = 0 OrElse Not IsNothing(oCommunity)) AndAlso Not IsNothing(oPerson) Then
                    Return GetFolderFiles(oCommunity, FolderId, ShowHiddenItems, AdminPurpose, oPerson)
                End If
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
            Return New List(Of CommunityFile)
        End Function
        Public Function GetFolderFiles(ByVal oCommunity As Community, ByVal FolderId As Long, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean, ByVal oPerson As Person) As List(Of CommunityFile)
            Return GetFolderItems(True, oCommunity, FolderId, ShowHiddenItems, AdminPurpose, oPerson).OrderBy(Function(f) f.Name).ToList
        End Function

        Public Function GetFolderFilesCount(ByVal CommunityID As Integer, ByVal FolderId As Long, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean, ByVal PersonID As Integer) As Integer
            Try
                DC.BeginTransaction()
                Dim oCommunity As Community = DC.GetCurrentSession.Get(Of Community)(CommunityID)
                Dim oPerson As Person = DC.GetCurrentSession.Get(Of Person)(PersonID)
                DC.Commit()
                If (CommunityID = 0 OrElse Not IsNothing(oCommunity)) AndAlso Not IsNothing(oPerson) Then
                    Return GetFolderFilesCount(oCommunity, FolderId, ShowHiddenItems, AdminPurpose, oPerson)
                End If
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
            Return 0
        End Function
        Public Function GetFolderFilesCount(ByVal oCommunity As Community, ByVal FolderId As Long, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean, ByVal oPerson As Person) As Integer
            Return GetFolderItemsCount(True, oCommunity, FolderId, ShowHiddenItems, AdminPurpose, oPerson)
        End Function

        Private Function GetFolderItems(ByVal isFile As Boolean, ByVal oCommunity As Community, ByVal FolderId As Long, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean, ByVal oPerson As Person) As List(Of CommunityFile)
            Dim oCommunityFiles As New List(Of CommunityFile)
            Try
                DC.BeginTransaction()
                Dim oRole As Role = (From s As Subscription In DC.GetCurrentSession.Linq(Of Subscription)() Where s.Community Is oCommunity AndAlso s.Person Is oPerson Select s.Role).FirstOrDefault
                Dim Query = (From file In DC.GetCurrentSession.Linq(Of CommunityFile)() _
                                   Where file.CommunityOwner Is oCommunity AndAlso file.FolderId = FolderId AndAlso file.isFile = isFile _
                                   AndAlso (ShowHiddenItems = True OrElse (file.isVisible OrElse file.Owner Is oPerson)))

                If AdminPurpose Then
                    oCommunityFiles = (From f In Query Select f).ToList
                Else
                    Dim FilesID As List(Of Long) = (From f In Query Select f.Id).ToList
                    oCommunityFiles = (From fa In DC.GetCurrentSession.Linq(Of CommunityFileCommunityAssignment)() Where FilesID.Contains(fa.File.Id) AndAlso fa.Inherited AndAlso fa.AssignedTo Is oCommunity AndAlso Not fa.Deny Select fa.File).ToList
                    oCommunityFiles.AddRange((From fa In DC.GetCurrentSession.Linq(Of CommunityFileRoleAssignment)() Where Not oCommunityFiles.Contains(fa.File) AndAlso FilesID.Contains(fa.File.Id) AndAlso fa.Inherited AndAlso fa.AssignedTo Is oRole AndAlso Not fa.Deny Select fa.File).ToList)
                    oCommunityFiles.AddRange((From fa In DC.GetCurrentSession.Linq(Of CommunityFilePersonAssignment)() Where Not oCommunityFiles.Contains(fa.File) AndAlso FilesID.Contains(fa.File.Id) AndAlso fa.Inherited AndAlso fa.AssignedTo Is oPerson AndAlso Not fa.Deny Select fa.File).ToList)
                End If
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try

            Return oCommunityFiles.OrderBy(Function(f) f.DisplayName).ToList
        End Function
        Private Function GetFolderItemsCount(ByVal isFile As Boolean, ByVal oCommunity As Community, ByVal FolderId As Long, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean, ByVal oPerson As Person) As Integer
            Dim iResponse As Integer = 0
            Try
                DC.BeginTransaction()
                Dim oRole As Role = (From s As Subscription In DC.GetCurrentSession.Linq(Of Subscription)() Where s.Community Is oCommunity AndAlso s.Person Is oPerson Select s.Role).FirstOrDefault
                Dim Query = (From file In DC.GetCurrentSession.Linq(Of CommunityFile)() _
                                   Where file.CommunityOwner Is oCommunity AndAlso file.FolderId = FolderId AndAlso file.isFile = isFile _
                                   AndAlso (ShowHiddenItems = True OrElse (file.isVisible OrElse file.Owner Is oPerson)))

                If AdminPurpose Then
                    iResponse = Query.Count
                Else
                    Dim FilesID As List(Of Long) = (From f In Query Select f.Id).ToList
                    Dim Items As List(Of Long) = (From fa In DC.GetCurrentSession.Linq(Of CommunityFileCommunityAssignment)() Where FilesID.Contains(fa.File.Id) AndAlso fa.Inherited AndAlso fa.AssignedTo Is oCommunity Select fa.File.Id).ToList

                    Items.AddRange((From fa In DC.GetCurrentSession.Linq(Of CommunityFileRoleAssignment)() Where FilesID.Except(Items).ToList.Contains(fa.File.Id) AndAlso fa.Inherited AndAlso fa.AssignedTo Is oRole Select fa.File.Id).ToList)
                    Items.AddRange((From fa In DC.GetCurrentSession.Linq(Of CommunityFilePersonAssignment)() Where FilesID.Except(Items).ToList.Contains(fa.File.Id) AndAlso fa.Inherited AndAlso fa.AssignedTo Is oPerson Select fa.File.Id).ToList)

                    iResponse = Items.Count
                End If

                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try

            Return iResponse
        End Function

        Public Function GetFolderPathName(ByVal FolderId As Long) As String
            Dim PathName As String = ""
            Try
                If FolderId > 0 Then
                    DC.BeginTransaction()
                    Dim FolderName As String = ""
                    Dim oFolder As CommunityFile = (From cf In DC.GetCurrentSession.Linq(Of CommunityFile)() Where cf.Id = FolderId AndAlso Not cf.isFile Select cf).FirstOrDefault
                    FolderName = oFolder.Name
                    DC.Commit()

                    If oFolder.FolderId = 0 Then
                        PathName = "/" & FolderName
                    Else
                        PathName = GetFolderPathName(oFolder.FolderId) & "/" & FolderName
                    End If
                Else
                    PathName = "/"
                End If
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try

            Return PathName
        End Function
        Public Function GetFolderName(ByVal FolderId As Long) As String
            Dim PathName As String = ""
            Try
                If FolderId > 0 Then
                    DC.BeginTransaction()
                    Dim oFolder As CommunityFile = (From cf In DC.GetCurrentSession.Linq(Of CommunityFile)() Where cf.Id = FolderId AndAlso Not cf.isFile Select cf).FirstOrDefault
                    DC.Commit()
                    PathName = oFolder.Name
                Else
                    PathName = "/"
                End If
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
            Return PathName
        End Function
        Public Function CommunityHasFolders(ByVal CommunityID As Integer) As Boolean
            Dim iResponse As Boolean = False
            Try
                DC.BeginTransaction()
                If CommunityID = 0 Then
                    iResponse = ((From f In DC.GetCurrentSession.Linq(Of CommunityFile)() Where f.CommunityOwner Is Nothing AndAlso f.isFile = False).Count > 0)
                Else
                    iResponse = ((From f In DC.GetCurrentSession.Linq(Of CommunityFile)() Where f.CommunityOwner.Id = CommunityID AndAlso f.isFile = False).Count > 0)
                End If
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
            Return iResponse
        End Function
#End Region

        Public Function HasPermissionToSeeItem(ByVal ItemId As Long, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean, ByVal PersonID As Integer) As Boolean
            Dim oPerson As Person = Nothing
            Try
                DC.BeginTransaction()
                oPerson = DC.GetCurrentSession.Get(Of Person)(PersonID)
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
            Return Me.HasPermissionToSeeItem(ItemId, ShowHiddenItems, AdminPurpose, oPerson)
        End Function
        Public Function HasPermissionToSeeItem(ByVal ItemId As Long, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean, ByVal oPerson As Person) As Boolean
            Dim iResponse As Boolean = False
            Try
                DC.BeginTransaction()
                Dim oCommunityFile As CommunityFile = DC.GetCurrentSession.Get(Of CommunityFile)(ItemId)
                If Not IsNothing(oCommunityFile) Then
                    If AdminPurpose Then
                        iResponse = ShowHiddenItems OrElse (oCommunityFile.isVisible) OrElse oCommunityFile.Owner Is oPerson
                    Else
                        Dim HasPersonAssignment As Boolean = False
                        Dim HasRoleAssignment As Boolean = False
                        Dim HasCommunityAssignment As Boolean = False
                        Dim HasDenyPerson As Boolean = False
                        Dim HasDenyRole As Boolean = False
                        Dim HasDenyCommunity As Boolean = False

                        If oCommunityFile.CommunityOwner Is Nothing Then
                            Dim QueryRole = (From fa In DC.GetCurrentSession.Linq(Of CommunityFilePersonTypeAssignment)() _
                                             Where fa.File Is oCommunityFile AndAlso fa.Inherited AndAlso fa.AssignedTo = oPerson.TypeID AndAlso (ShowHiddenItems OrElse fa.File.isVisible OrElse (Not fa.File.isVisible AndAlso fa.File.Owner Is oPerson)))
                            HasRoleAssignment = (From q In QueryRole Where Not q.Deny).Any
                            HasDenyRole = (From q In QueryRole Where q.Deny).Any

                        Else
                            Dim oRole As Role = (From s As Subscription In DC.GetCurrentSession.Linq(Of Subscription)() Where s.Community Is oCommunityFile.CommunityOwner AndAlso s.Person Is oPerson AndAlso s.Accepted AndAlso s.Enabled Select s.Role).FirstOrDefault

                            Dim QueryRole = (From fa In DC.GetCurrentSession.Linq(Of CommunityFileRoleAssignment)() _
                                              Where fa.File Is oCommunityFile AndAlso fa.AssignedTo Is oRole AndAlso fa.Inherited AndAlso (ShowHiddenItems OrElse fa.File.isVisible OrElse (Not fa.File.isVisible AndAlso fa.File.Owner Is oPerson)))
                            HasRoleAssignment = (From q In QueryRole Where Not q.Deny).Any
                            HasDenyRole = (From q In QueryRole Where q.Deny).Any


                        End If
                        Dim QueryCommunity = (From fa In DC.GetCurrentSession.Linq(Of CommunityFileCommunityAssignment)() _
                                            Where fa.File Is oCommunityFile AndAlso fa.AssignedTo Is oCommunityFile.CommunityOwner AndAlso fa.Inherited AndAlso (ShowHiddenItems OrElse fa.File.isVisible OrElse (Not fa.File.isVisible AndAlso fa.File.Owner Is oPerson)))

                        HasCommunityAssignment = (From q In QueryCommunity Where Not q.Deny).Any
                        HasDenyCommunity = (From q In QueryCommunity Where q.Deny).Any

                        Dim QueryPerson = (From fa In DC.GetCurrentSession.Linq(Of CommunityFilePersonAssignment)() _
                                              Where fa.File Is oCommunityFile AndAlso fa.AssignedTo Is oPerson AndAlso fa.Inherited AndAlso (ShowHiddenItems OrElse fa.File.isVisible OrElse (Not fa.File.isVisible AndAlso fa.File.Owner Is oPerson)))
                        HasPersonAssignment = (From q In QueryPerson Where Not q.Deny).Any
                        HasDenyPerson = (From q In QueryPerson Where q.Deny).Any

                        iResponse = (HasCommunityAssignment OrElse HasRoleAssignment OrElse HasPersonAssignment)

                        If iResponse Then
                            iResponse = (HasCommunityAssignment AndAlso Not HasDenyPerson AndAlso Not HasDenyRole) OrElse _
                                        (HasRoleAssignment AndAlso Not HasDenyPerson) OrElse HasPersonAssignment
                        End If

                    End If
                End If
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try

            Return iResponse
        End Function
        Public Function HasPermissionToUploadIntoFolder(ByVal FolderID As Long, ByVal PersonID As Integer, ByVal oModule As ModuleCommunityRepository) As Boolean
            If oModule.Administration OrElse oModule.UploadFile Then
                Return True
            Else
                Dim oPerson As Person = Nothing
                Try
                    DC.BeginTransaction()
                    oPerson = DC.GetCurrentSession.Get(Of Person)(PersonID)
                    DC.Commit()
                Catch ex As Exception
                    If DC.isInTransaction Then
                        DC.Rollback()
                    End If
                End Try
                Return Me.HasPermissionToUploadIntoFolder(FolderID, oPerson, oModule)
            End If
        End Function
        Public Function HasPermissionToUploadIntoFolder(ByVal FolderID As Long, ByVal oPerson As Person, ByVal oModule As ModuleCommunityRepository) As Boolean
            Dim iResponse As Boolean = oModule.Administration OrElse oModule.UploadFile
            If Not iResponse Then
                Try
                    DC.BeginTransaction()
                    Dim oFolder As CommunityFile = DC.GetCurrentSession.Get(Of CommunityFile)(FolderID)
                    If Not IsNothing(oFolder) Then
                        Dim AssignedPermission As Long = 0

                        Dim oCommunity As Community = oFolder.CommunityOwner
                        If oCommunity Is Nothing Then
                            AssignedPermission = (From fa In DC.GetCurrentSession.Linq(Of CommunityFilePersonTypeAssignment)() _
                                             Where fa.File Is oFolder AndAlso fa.Inherited AndAlso fa.AssignedTo = oPerson.TypeID AndAlso (fa.File.isVisible OrElse (Not fa.File.isVisible AndAlso fa.File.Owner Is oPerson)) AndAlso fa.Deny = False Select fa.Permission).FirstOrDefault
                        Else
                            Dim oRole As Role = (From s As Subscription In DC.GetCurrentSession.Linq(Of Subscription)() Where s.Community Is oCommunity AndAlso s.Person Is oPerson AndAlso s.Accepted AndAlso s.Enabled Select s.Role).FirstOrDefault

                            AssignedPermission = (From fa In DC.GetCurrentSession.Linq(Of CommunityFileRoleAssignment)() _
                                              Where fa.File Is oFolder AndAlso fa.AssignedTo Is oRole AndAlso fa.Inherited AndAlso (fa.File.isVisible OrElse (Not fa.File.isVisible AndAlso fa.File.Owner Is oPerson)) AndAlso fa.Deny = False Select fa.Permission).FirstOrDefault
                        End If
                        If Not (AssignedPermission And COL_BusinessLogic_v2.UCServices.Services_File.Base2Permission.UploadFile) > 0 Then
                            AssignedPermission = AssignedPermission Or (From fa In DC.GetCurrentSession.Linq(Of CommunityFileCommunityAssignment)() _
                                          Where fa.File Is oFolder AndAlso fa.AssignedTo Is oCommunity AndAlso fa.Inherited AndAlso (fa.File.isVisible OrElse (Not fa.File.isVisible AndAlso fa.File.Owner Is oPerson)) AndAlso fa.Deny = False Select fa.Permission).FirstOrDefault

                        End If

                        If Not (AssignedPermission And COL_BusinessLogic_v2.UCServices.Services_File.Base2Permission.UploadFile) > 0 Then
                            AssignedPermission = AssignedPermission Or (From fa In DC.GetCurrentSession.Linq(Of CommunityFilePersonAssignment)() _
                                           Where fa.File Is oFolder AndAlso fa.AssignedTo Is oPerson AndAlso fa.Inherited AndAlso (fa.File.isVisible OrElse (Not fa.File.isVisible AndAlso fa.File.Owner Is oPerson)) AndAlso fa.Deny = False Select fa.Permission).FirstOrDefault

                        End If

                        iResponse = (AssignedPermission And COL_BusinessLogic_v2.UCServices.Services_File.Base2Permission.UploadFile) OrElse oFolder.AllowUpload
                    End If
                    DC.Commit()
                Catch ex As Exception
                    If DC.isInTransaction Then
                        DC.Rollback()
                    End If
                End Try
            End If
            Return iResponse
        End Function
#Region "Verify Selected Permission"
        Public Function HasCommunityAssignment(ByVal ItemId As Long, ByVal Deny As Boolean) As Boolean
            Dim iResponse As Boolean = False
            Try
                DC.BeginTransaction()

                iResponse = (From fa In DC.GetCurrentSession.Linq(Of CommunityFileCommunityAssignment)() _
                             Where fa.File.Id = ItemId AndAlso fa.Deny = Deny AndAlso Not fa.Inherited).Any
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try

            Return iResponse
        End Function
        'Public Function GetRolesAssignment(ByVal ItemId As Long, ByVal Deny As Boolean) As List(Of Role)
        '    Dim oRoles As New List(Of Role)
        '    Try
        '        DC.BeginTransaction()
        '        oRoles = (From fa In DC.GetCurrentSession.Linq(Of CommunityFileRoleAssignment)() _
        '                  Where fa.File.Id = ItemId AndAlso fa.Deny = Deny Select fa.AssignedTo).ToList

        '        DC.Commit()
        '    Catch ex As Exception
        '        If DC.isInTransaction Then
        '            DC.Rollback()
        '        End If
        '    End Try

        '    Return oRoles
        'End Function
        Public Function GetRolesAssignmentID(ByVal ItemId As Long, ByVal Deny As Boolean) As List(Of Integer)
            Dim oRoles As New List(Of Integer)
            Try
                DC.BeginTransaction()
                oRoles = (From fa In DC.GetCurrentSession.Linq(Of CommunityFileRoleAssignment)() _
                          Where fa.File.Id = ItemId AndAlso fa.Deny = Deny AndAlso fa.Inherited = False Select fa.AssignedTo.Id).ToList

                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try

            Return oRoles
        End Function
        Public Function GetAssignedUserTypesID(ByVal ItemId As Long, ByVal Deny As Boolean) As List(Of Integer)
            Dim oRoles As New List(Of Integer)
            Try
                DC.BeginTransaction()
                oRoles = (From fa In DC.GetCurrentSession.Linq(Of CommunityFilePersonTypeAssignment)() _
                          Where fa.File.Id = ItemId AndAlso fa.Deny = Deny AndAlso fa.Inherited = False Select fa.AssignedTo).ToList

                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try

            Return oRoles
        End Function
        Public Function GetAssignedPerson(ByVal oItem As CommunityFile, ByVal Deny As Boolean) As List(Of dtoMember(Of Integer))
            Dim oPersons As New List(Of dtoMember(Of Integer))
            Try
                DC.BeginTransaction()
                Dim oQuery = (From fa In DC.GetCurrentSession.Linq(Of CommunityFilePersonAssignment)() _
                          Where fa.File Is oItem AndAlso fa.Deny = Deny AndAlso fa.Inherited = False Select fa).ToList

                oPersons = (From p In oQuery Select New dtoMember(Of Integer) With {.Id = p.AssignedTo.Id, .Name = p.AssignedTo.Surname & " " & p.AssignedTo.Name}).ToList
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try

            Return oPersons
        End Function

        Public Function GetPersonAssignmentCount(ByVal ItemId As Long, ByVal Deny As Boolean) As Long
            Dim iResponse As Long = 0
            Try
                DC.BeginTransaction()
                iResponse = (From fa In DC.GetCurrentSession.Linq(Of CommunityFilePersonAssignment)() _
                          Where fa.File.Id = ItemId AndAlso fa.Deny = Deny AndAlso fa.Inherited = False).Count

                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try

            Return iResponse
        End Function
        Public Function GetFirstPersonAssignment(ByVal ItemId As Long, ByVal Deny As Boolean, ByVal MaxValue As Long) As List(Of Person)
            Dim iResponse As New List(Of Person)
            Try
                DC.BeginTransaction()

                'Dim o As List(Of CommunityFilePersonAssignment) = (From fa In DC.GetCurrentSession.Linq(Of CommunityFilePersonAssignment)() _
                '                                                   Where fa.File.Id = ItemId AndAlso fa.Deny = Deny Order By fa.AssignedTo.Surname Select fa).Skip(0).Take(MaxValue).ToList()


                'iResponse = (From a In o Select a.AssignedTo).ToList
                iResponse = (From fa In DC.GetCurrentSession.Linq(Of CommunityFilePersonAssignment)() _
                                Where fa.File.Id = ItemId AndAlso fa.Deny = Deny AndAlso fa.Inherited = False _
                                Order By fa.AssignedTo.Surname Select fa.AssignedTo).Skip(0).Take(MaxValue).ToList()

                '  iResponse = (From fa In DC.GetCurrentSession.Linq(Of CommunityFilePersonAssignment)() Where fa.File.Id = ItemId AndAlso fa.Deny = Deny Select fa.AssignedTo).ToList()

                '

                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try

            Return iResponse
        End Function
        Public Function GetFirstPersonNamesAssignment(ByVal ItemId As Long, ByVal Deny As Boolean, ByVal MaxValue As Long) As List(Of String)
            Dim iResponse As New List(Of String)
            Try
                Dim oList As List(Of Person) = Me.GetFirstPersonAssignment(ItemId, Deny, MaxValue)

                DC.BeginTransaction()
                iResponse = (From p In oList Order By p.Surname, p.Name Select p.Surname & " " & p.Name).ToList

                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try

            Return iResponse
        End Function
#End Region

        Public Function SetCommunityAssignmentToItems(ByVal ByUserID As Integer, ByVal FatherID As Long, ByVal ItemsId As List(Of Long), ByVal Deny As Boolean, ByVal ToSubitems As Boolean, ByVal Permission As Long) As Boolean
            Dim iResponse As Boolean = False
            Try
                DC.BeginTransaction()

                Dim CreatedOn As DateTime = Now
                Dim CreatedBy As Person = Me.DC.GetById(Of Person)(ByUserID)
                Dim oItems As List(Of CommunityFile) = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)() Where ItemsId.Contains(f.Id) Select f).ToList
                If Not IsNothing(CreatedBy) Then
                    For Each oItem In oItems
                        Me.ApplyAssignmentToCommunity(CreatedOn, CreatedBy, oItem.FolderId, oItem.Id, Deny, ToSubitems, Permission)

                        Dim FolderID As Long = oItem.FolderId
                        Dim oAssignments As List(Of CommunityFileAssignment)

                        If FolderID = 0 Then
                            oAssignments = (From fa In DC.GetCurrentSession.Linq(Of CommunityFileAssignment)() Where fa.File Is oItem AndAlso fa.Inherited = False Select fa).ToList
                        Else
                            oAssignments = (From fa In DC.GetCurrentSession.Linq(Of CommunityFileAssignment)() Where fa.File.Id = FolderID AndAlso fa.Inherited Select fa).ToList
                        End If
                        Me.ApplyInheritedAssignment(CreatedOn, CreatedBy, oItem, oAssignments, Deny, Permission)
                    Next
                End If

                DC.Commit()
                iResponse = True
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try

            Return iResponse
        End Function
        Private Sub ApplyAssignmentToCommunity(ByVal CreatedOn As DateTime, ByVal CreatedBy As Person, ByVal FatherID As Long, ByVal ItemID As Long, ByVal Deny As Boolean, ByVal ToSubitems As Boolean, ByVal Permission As Long)
            Try
                If Not (From fa In DC.GetCurrentSession.Linq(Of CommunityFileCommunityAssignment)() Where fa.File.Id = ItemID AndAlso fa.Deny = Deny).Any Then
                    Dim OldAssigned As List(Of CommunityFileAssignment) = (From fa In DC.GetCurrentSession.Linq(Of CommunityFileAssignment)() _
                         Where fa.File.Id = ItemID AndAlso fa.Deny = Deny Select fa).ToList
                    For Each oItem In OldAssigned
                        DC.Delete(oItem)
                    Next
                    Me.AddCommunityAssignmentToItem(CreatedOn, CreatedBy, ItemID, Deny, Permission, False)
                End If

                Dim oItemToEdit As CommunityFile = Me.DC.GetById(Of CommunityFile)(ItemID)
                Dim oItemsToApply As New List(Of Long)
                If Not oItemToEdit.isFile AndAlso ToSubitems Then
                    oItemsToApply = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)() Where f.FolderId = ItemID Select f.Id).ToList
                    If oItemsToApply.Count > 0 Then
                        RecursiveApplyAssignmentToCommunity(CreatedOn, CreatedBy, ItemID, oItemsToApply, Deny, ToSubitems, Permission)
                    End If
                End If
            Catch ex As Exception
                Throw New Exception
            End Try
        End Sub
        Private Sub RecursiveApplyAssignmentToCommunity(ByVal CreatedOn As DateTime, ByVal CreatedBy As Person, ByVal FatherID As Long, ByVal ItemsID As List(Of Long), ByVal Deny As Boolean, ByVal ToSubitems As Boolean, ByVal Permission As Long)
            Try
                For Each ItemId In ItemsID
                    Me.ApplyAssignmentToCommunity(CreatedOn, CreatedBy, FatherID, ItemId, Deny, ToSubitems, Permission)
                Next
            Catch ex As Exception
                Throw New Exception
            End Try
        End Sub


        Private Sub ApplyInheritedAssignment(ByVal CreatedOn As DateTime, ByVal CreatedBy As Person, ByVal oItem As CommunityFile, ByVal oFatherAssignments As List(Of CommunityFileAssignment), ByVal Deny As Boolean, ByVal Permission As Long)
            Try
                Dim ToRemove As List(Of CommunityFileAssignment) = (From fa In DC.GetCurrentSession.Linq(Of CommunityFileAssignment)() Where fa.File Is oItem AndAlso fa.Deny = Deny AndAlso fa.Inherited = True Select fa).ToList
                For Each oOldAssignment In ToRemove
                    DC.Delete(oOldAssignment)
                Next

                Dim oAssociated As List(Of CommunityFileAssignment) = (From fa In DC.GetCurrentSession.Linq(Of CommunityFileAssignment)() Where fa.File Is oItem AndAlso fa.Deny = Deny AndAlso Not fa.Inherited Select fa).ToList

                Dim ItemAllowCommunity As Boolean = (From a In oAssociated Where TypeOf a Is CommunityFileCommunityAssignment).Any
                Dim FatherAllowCommunity As Boolean = (From a In oFatherAssignments Where TypeOf a Is CommunityFileCommunityAssignment).Any


                If ItemAllowCommunity AndAlso FatherAllowCommunity Then
                    Me.AddCommunityAssignmentToItem(CreatedOn, CreatedBy, oItem.Id, Deny, Permission, True)
                Else
                    Dim FatherRoles As List(Of Integer) = (From a In oFatherAssignments Where TypeOf a Is CommunityFileRoleAssignment Select DirectCast(a, CommunityFileRoleAssignment).AssignedTo.Id).ToList
                    Dim FatherPersons As List(Of Integer) = (From a In oFatherAssignments Where TypeOf a Is CommunityFilePersonAssignment Select DirectCast(a, CommunityFilePersonAssignment).AssignedTo.Id).ToList
                    Dim FatherPersonTypes As List(Of Integer) = (From a In oFatherAssignments Where TypeOf a Is CommunityFilePersonTypeAssignment Select DirectCast(a, CommunityFilePersonTypeAssignment).AssignedTo).ToList

                    If Not ItemAllowCommunity Then
                        Dim QueryRoles = (From a In oAssociated Where TypeOf a Is CommunityFileRoleAssignment Select DirectCast(a, CommunityFileRoleAssignment).AssignedTo.Id)
                        Dim QueryPersons = (From a In oAssociated Where TypeOf a Is CommunityFilePersonAssignment Select DirectCast(a, CommunityFilePersonAssignment).AssignedTo.Id)
                        Dim QueryTypes = (From a In oAssociated Where TypeOf a Is CommunityFilePersonTypeAssignment Select DirectCast(a, CommunityFilePersonTypeAssignment).AssignedTo)

                        If FatherAllowCommunity Then
                            FatherRoles = QueryRoles.ToList
                            FatherPersonTypes = QueryTypes.ToList
                            FatherPersons = QueryPersons.ToList
                        Else
                            FatherRoles = (From r In FatherRoles Where (QueryRoles.ToList().Contains(r))).ToList
                            FatherPersonTypes = (From t In FatherPersonTypes Where (QueryTypes.ToList().Contains(t))).ToList

                            Dim ExludedUserFromItem As List(Of Integer) = (From p In QueryPersons Where Not (FatherPersons.Contains(p))).ToList
                            Dim ExludedUserFromFather As List(Of Integer) = (From p In FatherPersons Where Not (QueryPersons.Contains(p))).ToList
                            FatherPersons = (From p In FatherPersons Where (QueryPersons.ToList().Contains(p))).ToList

                            If oItem.CommunityOwner Is Nothing Then
                                Dim queryTypePerson = (From s In DC.GetCurrentSession.Linq(Of Person)() Where (ExludedUserFromItem.Contains(s.Id)) Select UserID = s.Id, TypeId = s.TypeID).ToList()
                                FatherPersons.AddRange((From o In queryTypePerson Join typeID In FatherPersonTypes On o.TypeId Equals typeID Select o.UserID).ToList)

                                queryTypePerson = (From s In DC.GetCurrentSession.Linq(Of Person)() Where (ExludedUserFromFather.Contains(s.Id)) Select UserID = s.Id, TypeId = s.TypeID).ToList()
                                FatherPersons.AddRange((From o In queryTypePerson Join typeID In QueryTypes On o.TypeId Equals typeID Select o.UserID).ToList)
                            Else
                                Dim queryRolesPerson = (From s In DC.GetCurrentSession.Linq(Of Subscription)() Where s.Community Is oItem.CommunityOwner AndAlso (ExludedUserFromItem.Contains(s.Person.Id)) Select UserID = s.Person.Id, RoleId = s.Role.Id).ToList()
                                FatherPersons.AddRange((From o In queryRolesPerson Join role In FatherRoles On o.RoleId Equals role Select o.UserID).ToList)

                                queryRolesPerson = (From s In DC.GetCurrentSession.Linq(Of Subscription)() Where s.Community Is oItem.CommunityOwner AndAlso ExludedUserFromFather.Contains(s.Person.Id) Select UserID = s.Person.Id, RoleId = s.Role.Id).ToList()
                                FatherPersons.AddRange((From o In queryRolesPerson Join role In QueryRoles On o.RoleId Equals role Select o.UserID).ToList)

                            End If
                        End If
                    End If
                    If FatherPersons.Count > 0 Then : Me.AddPersonsAssignmentToItem(CreatedOn, CreatedBy, oItem.Id, FatherPersons, Deny, Permission, True)
                    End If
                    If FatherPersonTypes.Count > 0 Then : Me.AddPersonTypesAssignmentToItem(CreatedOn, CreatedBy, oItem.Id, FatherPersonTypes, Deny, Permission, True)
                    End If
                    If FatherRoles.Count > 0 Then : Me.AddRolesAssignmentToItem(CreatedOn, CreatedBy, oItem.Id, FatherRoles, Deny, Permission, True)
                    End If

                End If

                If Not oItem.isFile Then
                    Dim oUpdatedAssociated As List(Of CommunityFileAssignment) = (From fa In DC.GetCurrentSession.Linq(Of CommunityFileAssignment)() Where fa.File Is oItem AndAlso fa.Inherited Select fa).ToList
                    Dim oSubItems As List(Of CommunityFile) = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)() Where f.FolderId = oItem.Id Select f).ToList
                    For Each oSubItem In oSubItems
                        Me.ApplyInheritedAssignment(CreatedOn, CreatedBy, oSubItem, oUpdatedAssociated, Deny, Permission)
                    Next

                End If

            Catch ex As Exception
                Throw New Exception
            End Try
        End Sub


        Private Sub RecursiveAnalyzeInheritedToCommunity(ByVal CreatedOn As DateTime, ByVal CreatedBy As Person, ByVal FatherID As Long, ByVal ItemsID As List(Of Long), ByVal Deny As Boolean, ByVal ToSubitems As Boolean, ByVal Permission As Long)
            Try
                For Each ItemId In ItemsID
                    Me.ApplyInheritedToCommunity(CreatedOn, CreatedBy, FatherID, ItemId, Deny, ToSubitems, Permission)
                Next
            Catch ex As Exception
                Throw New Exception
            End Try
        End Sub

        Private Sub ApplyInheritedToCommunity(ByVal CreatedOn As DateTime, ByVal CreatedBy As Person, ByVal FatherID As Long, ByVal ItemID As Long, ByVal Deny As Boolean, ByVal ToSubitems As Boolean, ByVal Permission As Long)
            Try
                If FatherID = 0 Then
                    Me.AddCommunityAssignmentToItem(CreatedOn, CreatedBy, ItemID, Deny, Permission, True)
                Else

                End If
                If Not (From fa In DC.GetCurrentSession.Linq(Of CommunityFileCommunityAssignment)() Where fa.File.Id = ItemID AndAlso fa.Deny = Deny).Any Then
                    Dim OldAssigned As List(Of CommunityFileAssignment) = (From fa In DC.GetCurrentSession.Linq(Of CommunityFileAssignment)() _
                         Where fa.File.Id = ItemID AndAlso fa.Deny = Deny Select fa).ToList
                    For Each oItem In OldAssigned
                        DC.Delete(oItem)
                    Next
                    Me.AddCommunityAssignmentToItem(CreatedOn, CreatedBy, ItemID, Deny, Permission, False)
                End If

                Dim oItemToEdit As CommunityFile = Me.DC.GetById(Of CommunityFile)(ItemID)
                Dim oItemsToApply As New List(Of Long)
                If Not oItemToEdit.isFile AndAlso ToSubitems Then
                    oItemsToApply = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)() Where f.FolderId = ItemID Select f.Id).ToList
                    If oItemsToApply.Count > 0 Then
                        RecursiveApplyAssignmentToCommunity(CreatedOn, CreatedBy, ItemID, oItemsToApply, Deny, ToSubitems, Permission)
                    End If
                End If
            Catch ex As Exception
                Throw New Exception
            End Try
        End Sub
        'Private Sub RecursiveApplyInheritedToCommunity(ByVal CreatedOn As DateTime, ByVal CreatedBy As Person, ByVal FatherID As Long, ByVal ItemsID As List(Of Long), ByVal Deny As Boolean, ByVal ToSubitems As Boolean, ByVal Permission As Long)
        '    Try
        '        For Each ItemId In ItemsID
        '            Me.ApplyInheritedToCommunity(CreatedOn, CreatedBy, FatherID, ItemId, Deny, ToSubitems, Permission)
        '        Next
        '    Catch ex As Exception
        '        Throw New Exception
        '    End Try
        'End Sub


        'Private Sub AnalizeAssigmentToCommunity(ByVal ByUserID As Integer, ByVal FatherID As Long, ByVal ItemsId As List(Of Long), ByVal Deny As Boolean, ByVal ToSubitems As Boolean, ByVal Permission As Long)
        '    Try
        '        Dim CreatedOn As DateTime = Now
        '        Dim CreatedBy As Person = Me.DC.GetById(Of Person)(ByUserID)
        '        If Not IsNothing(CreatedBy) Then
        '            For Each ItemId In ItemsId
        '                Me.ApplyAssignmentToCommunity(CreatedOn, CreatedBy, FatherID, ItemId, Deny, ToSubitems, Permission)
        '            Next
        '        End If
        '        'Dim OldAssigned As List(Of CommunityFileAssignment) = (From fa In DC.GetCurrentSession.Linq(Of CommunityFileAssignment)() _
        '        '          Where ItemsId.Contains(fa.File.Id) AndAlso fa.Deny = Deny Select fa).ToList

        '        '' DELETE ALL ASSIGNMENT NOT TO COMMUNITY !
        '        'For Each oItem In (From f In OldAssigned Where Not (TypeOf f Is CommunityFileCommunityAssignment) Select f).ToList
        '        '    DC.Delete(oItem)
        '        'Next

        '        'Dim CreatedOn As DateTime = Now
        '        'Dim CreatedBy As Person = Me.DC.GetById(Of Person)(ByUserID)
        '        'Dim AlreadyAssignedID As List(Of Long) = (From a In OldAssigned Where TypeOf a Is CommunityFileCommunityAssignment Select a.File.Id).ToList
        '        'Dim ToInsert As List(Of Long) = (From id In ItemsId Where Not AlreadyAssignedID.Contains(id) Select id).ToList

        '        'If ToInsert.Count > 0 Then
        '        '    Me.AddCommunityAssignmentToItems(CreatedOn, CreatedBy, ToInsert, Deny, Permission, False)
        '        'End If

        '        'Dim AllInherited As List(Of CommunityFileAssignment) = (From fa In DC.GetCurrentSession.Linq(Of CommunityFileAssignment)() _
        '        '        Where ItemsId.Contains(fa.File.Id) AndAlso fa.Deny = Deny AndAlso fa.Inherited Select fa).ToList
        '        'For Each oItem In AllInherited
        '        '    DC.Delete(oItem)
        '        'Next

        '        ''Dim AllApplyed As List(Of CommunityFileAssignment) = (From fa In DC.GetCurrentSession.Linq(Of CommunityFileAssignment)() _
        '        ''      Where ItemsId.Contains(fa.File.Id) AndAlso fa.Deny = Deny AndAlso Not fa.Inherited Select fa).ToList
        '        'If FatherID <= 0 Then
        '        '    Me.AddCommunityAssignmentToItems(CreatedOn, CreatedBy, ItemsId, Deny, Permission, True)
        '        'Else
        '        '    Dim oFolder As CommunityFile = DC.GetCurrentSession.Get(Of CommunityFile)(FatherID)
        '        '    If Not IsNothing(oFolder) Then
        '        '        Dim AllApplyed As List(Of CommunityFileAssignment) = (From fa In DC.GetCurrentSession.Linq(Of CommunityFileAssignment)() _
        '        '     Where ItemsId.Contains(fa.File.Id) AndAlso fa.Deny = Deny AndAlso Not fa.Inherited Select fa).ToList
        '        '        For Each oItem In AllApplyed

        '        '        Next
        '        '    End If
        '        'End If
        '    Catch ex As Exception
        '        Throw New Exception
        '    End Try
        'End Sub



        'Private Sub RecursiveApplyAnalyzeInheritedAssignment(ByVal CreatedOn As DateTime, ByVal CreatedBy As Person, ByVal oFather As CommunityFile, ByVal ItemsID As List(Of Long), ByVal Deny As Boolean, ByVal ToSubitems As Boolean, ByVal Permission As Long)
        '    Try
        '        For Each ItemId In ItemsID
        '            Me.AnalyzeInheritedAssignment(CreatedOn, CreatedBy, oFather, ItemId, Deny, ToSubitems, Permission)
        '        Next
        '    Catch ex As Exception
        '        Throw New Exception
        '    End Try
        'End Sub
        'Private Sub AnalyzeInheritedAssignment(ByVal CreatedOn As DateTime, ByVal CreatedBy As Person, ByVal oFather As CommunityFile, ByVal ItemID As Long, ByVal Deny As Boolean, ByVal ToSubitems As Boolean, ByVal Permission As Long)

        'End Sub

        Public Function AddAssignmentToItems(ByVal ByUserID As Integer, ByVal ItemsId As List(Of Long), ByVal RolesID As List(Of Integer), ByVal MembersID As List(Of Integer), ByVal Deny As Boolean, ByVal ToSubitems As Boolean, ByVal Permission As Long) As Boolean
            Return SetOtherAssignmentToItems(ByUserID, ItemsId, RolesID, MembersID, Deny, False, ToSubitems, Permission)
        End Function
        Public Function AddPortalAssignmentToItems(ByVal ByUserID As Integer, ByVal ItemsId As List(Of Long), ByVal TypesID As List(Of Integer), ByVal MembersID As List(Of Integer), ByVal Deny As Boolean, ByVal ToSubitems As Boolean, ByVal Permission As Long) As Boolean
            Return SetOtherAssignmentToItems(ByUserID, ItemsId, TypesID, MembersID, Deny, True, ToSubitems, Permission)
        End Function

        Public Function UpdateFolderPermission(ByVal ByUserID As Integer, ByVal oItem As CommunityFile, ByVal ToSubitems As Boolean, ByVal Permission As Long) As Boolean
            Dim iResponse As Boolean = False
            Try
                DC.BeginTransaction()
                Dim EditOn As DateTime = Now
                Dim EditBy As Person = Me.DC.GetById(Of Person)(ByUserID)
                If Not IsNothing(EditBy) Then
                    iResponse = UpdateFoldersPermission(EditOn, EditBy, oItem, ToSubitems, Permission)
                End If
                DC.Commit()

            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                iResponse = False
            End Try
            Return iResponse
        End Function

        Private Function UpdateFoldersPermission(ByVal EditOn As DateTime, ByVal EditBy As Person, ByVal oItem As CommunityFile, ByVal ToSubitems As Boolean, ByVal Permission As Long) As Boolean
            Dim iResponse As Boolean = False
            Try
                Dim assigments As List(Of CommunityFileAssignment) = (From f In DC.GetCurrentSession.Linq(Of CommunityFileAssignment)() Where f.File Is oItem Select f).ToList()
                For Each assigment In assigments
                    assigment.Permission = Permission ' IIf(oItem.isFile, IIf( PermissionHelper.CheckPermissionSoft(Permission, ModuleCommunityRepository.Base2Permission
                    assigment.ModifiedOn = EditOn
                    assigment.ModifiedBy = EditBy
                    DC.SaveOrUpdate(assigment)
                Next
                iResponse = True
                If ToSubitems Then
                    Dim oItems As List(Of CommunityFile) = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)() Where f.FolderId = oItem.Id Select f).ToList

                    For Each oItem In oItems
                        iResponse = UpdateFoldersPermission(EditOn, EditBy, oItem, ToSubitems, Permission)
                    Next
                End If
            Catch ex As Exception
                iResponse = False
            End Try
            Return iResponse
        End Function


        Public Function ChangeItemAssignmentToCommunity(ByVal ByUserID As Integer, ByVal oItem As CommunityFile, ByVal Deny As Boolean, ByVal ToSubitems As Boolean, ByVal Permission As Long) As Boolean
            Dim oItems As New List(Of Long)
            oItems.Add(oItem.Id)
            Return Me.SetCommunityAssignmentToItems(ByUserID, oItem.FolderId, oItems, Deny, ToSubitems, Permission)
        End Function
        Public Function ChangeItemAssignmentToSome(ByVal ByUserID As Integer, ByVal oItem As CommunityFile, ByVal Deny As Boolean, ByVal ToSubitems As Boolean, ByVal Permission As Long) As Boolean
            Dim oItems As New List(Of Long)
            Dim Members As New List(Of Integer)
            oItems.Add(oItem.Id)
            Members.Add(ByUserID)
            Return Me.AddAssignmentToItems(ByUserID, oItems, New List(Of Integer), Members, Deny, ToSubitems, Permission)
        End Function
        Public Function ChangePortalItemAssignmentToSome(ByVal ByUserID As Integer, ByVal oItem As CommunityFile, ByVal Deny As Boolean, ByVal ToSubitems As Boolean, ByVal Permission As Long) As Boolean
            Dim oItems As New List(Of Long)
            Dim Members As New List(Of Integer)
            oItems.Add(oItem.Id)
            Members.Add(ByUserID)
            Return Me.AddPortalAssignmentToItems(ByUserID, oItems, New List(Of Integer), Members, Deny, ToSubitems, Permission)
        End Function

#Region "Add Assignment"
        Public Function SetOtherAssignmentToItems(ByVal ByUserID As Integer, ByVal ItemsId As List(Of Long), ByVal RolesID As List(Of Integer), ByVal MembersID As List(Of Integer), ByVal Deny As Boolean, ByVal ForPortal As Boolean, ByVal ToSubitems As Boolean, ByVal Permission As Long) As Boolean
            Dim iResponse As Boolean = False
            Try
                DC.BeginTransaction()

                Dim CreatedOn As DateTime = Now
                Dim CreatedBy As Person = Me.DC.GetById(Of Person)(ByUserID)
                Dim oItems As List(Of CommunityFile) = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)() Where ItemsId.Contains(f.Id) Select f).ToList
                If Not IsNothing(CreatedBy) Then
                    For Each oItem In oItems
                        Dim oFileItem As CommunityFile = oItem
                        Me.ApplyAssignmentToOthers(CreatedOn, CreatedBy, oItem.FolderId, oItem, RolesID, MembersID, Deny, ToSubitems, Permission)

                        Dim FolderID As Long = oItem.FolderId
                        Dim oAssignments As List(Of CommunityFileAssignment)

                        If FolderID = 0 Then
                            oAssignments = (From fa In DC.GetCurrentSession.Linq(Of CommunityFileAssignment)() Where fa.File Is oFileItem AndAlso fa.Inherited = False Select fa).ToList
                        Else
                            oAssignments = (From fa In DC.GetCurrentSession.Linq(Of CommunityFileAssignment)() Where fa.File.Id = FolderID AndAlso fa.Inherited Select fa).ToList
                        End If
                        Me.ApplyInheritedAssignment(CreatedOn, CreatedBy, oItem, oAssignments, Deny, Permission)
                    Next
                End If

                DC.Commit()
                iResponse = True
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
            Return iResponse
        End Function
        Private Sub ApplyAssignmentToOthers(ByVal CreatedOn As DateTime, ByVal CreatedBy As Person, ByVal FatherID As Long, ByVal oItem As CommunityFile, ByVal RolesID As List(Of Integer), ByVal MembersID As List(Of Integer), ByVal Deny As Boolean, ByVal ToSubitems As Boolean, ByVal Permission As Long)
            Try

                Dim OldAssigned As List(Of CommunityFileAssignment) = (From fa In DC.GetCurrentSession.Linq(Of CommunityFileAssignment)() _
                     Where fa.File Is oItem AndAlso (fa.Deny = Deny OrElse fa.CreatedBy Is Nothing OrElse fa.ModifiedBy Is Nothing) Select fa).ToList
                For Each oOldAssigned In OldAssigned
                    DC.Delete(oOldAssigned)
                Next
                If oItem.CommunityOwner Is Nothing Then
                    Me.AddPersonTypesAssignmentToItem(CreatedOn, CreatedBy, oItem.Id, RolesID, Deny, Permission, False)
                Else
                    Me.AddRolesAssignmentToItem(CreatedOn, CreatedBy, oItem.Id, RolesID, Deny, Permission, False)
                End If
                Me.AddPersonsAssignmentToItem(CreatedOn, CreatedBy, oItem.Id, MembersID, Deny, Permission, False)

                Dim oItemsToApply As New List(Of CommunityFile)
                If Not oItem.isFile AndAlso ToSubitems Then
                    oItemsToApply = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)() Where f.FolderId = oItem.Id Select f).ToList
                    If oItemsToApply.Count > 0 Then
                        RecursiveApplyAssignmentToOther(CreatedOn, CreatedBy, oItem.Id, oItemsToApply, RolesID, MembersID, Deny, ToSubitems, Permission)
                    End If
                End If
            Catch ex As Exception
                Throw New Exception
            End Try
        End Sub
        Private Sub RecursiveApplyAssignmentToOther(ByVal CreatedOn As DateTime, ByVal CreatedBy As Person, ByVal FatherID As Long, ByVal Items As List(Of CommunityFile), ByVal RolesID As List(Of Integer), ByVal MembersID As List(Of Integer), ByVal Deny As Boolean, ByVal ToSubitems As Boolean, ByVal Permission As Long)
            Try
                For Each Item In Items
                    Me.ApplyAssignmentToOthers(CreatedOn, CreatedBy, FatherID, Item, RolesID, MembersID, Deny, ToSubitems, Permission)
                Next
            Catch ex As Exception
                Throw New Exception
            End Try
        End Sub
        Private Function AddCommunityAssignmentToItems(ByVal CreatedOn As DateTime, ByVal CreatedBy As Person, ByVal ItemsId As List(Of Long), ByVal Deny As Boolean, ByVal Permission As Long, ByVal Inherited As Boolean) As List(Of CommunityFileCommunityAssignment)
            Dim iResponse As New List(Of CommunityFileCommunityAssignment)
            Try
                For Each ItemId In ItemsId
                    Dim oFile As CommunityFile = DC.GetById(Of CommunityFile)(ItemId)
                    If Not IsNothing(oFile) Then
                        Dim oAssignment As New CommunityFileCommunityAssignment

                        oAssignment.AssignedTo = oFile.CommunityOwner
                        oAssignment.CreatedBy = CreatedBy
                        oAssignment.CreatedOn = CreatedOn
                        oAssignment.Deny = Deny
                        oAssignment.File = oFile
                        oAssignment.ModifiedBy = CreatedBy
                        oAssignment.ModifiedOn = CreatedOn
                        oAssignment.Permission = Permission
                        oAssignment.Inherited = Inherited
                        DC.SaveOrUpdate(oAssignment)
                        iResponse.Add(oAssignment)
                    End If
                Next
            Catch ex As Exception
            End Try
            Return iResponse
        End Function
        Private Function AddCommunityAssignmentToItem(ByVal CreatedOn As DateTime, ByVal CreatedBy As Person, ByVal ItemId As Long, ByVal Deny As Boolean, ByVal Permission As Long, ByVal Inherited As Boolean) As List(Of CommunityFileCommunityAssignment)
            Dim iResponse As New List(Of CommunityFileCommunityAssignment)
            Try
                Dim oFile As CommunityFile = DC.GetById(Of CommunityFile)(ItemId)
                If Not IsNothing(oFile) Then
                    Dim oAssignment As New CommunityFileCommunityAssignment

                    oAssignment.AssignedTo = oFile.CommunityOwner
                    oAssignment.CreatedBy = CreatedBy
                    oAssignment.CreatedOn = CreatedOn
                    oAssignment.Deny = Deny
                    oAssignment.File = oFile
                    oAssignment.ModifiedBy = CreatedBy
                    oAssignment.ModifiedOn = CreatedOn
                    oAssignment.Permission = Permission
                    oAssignment.Inherited = Inherited
                    DC.SaveOrUpdate(oAssignment)
                    iResponse.Add(oAssignment)
                End If
            Catch ex As Exception
            End Try
            Return iResponse
        End Function
        Private Function AddRolesAssignmentToItem(ByVal CreatedOn As DateTime, ByVal CreatedBy As Person, ByVal ItemID As Long, ByVal RolesID As List(Of Integer), ByVal Deny As Boolean, ByVal Permission As Long, ByVal Inherited As Boolean) As List(Of CommunityFileRoleAssignment)
            Dim iResponse As New List(Of CommunityFileRoleAssignment)
            Try
                Dim oFile As CommunityFile = DC.GetById(Of CommunityFile)(ItemID)
                If Not IsNothing(oFile) Then
                    For Each RoleID In RolesID
                        Dim oAssignment As New CommunityFileRoleAssignment
                        Dim oRole As Role = DC.GetById(Of Role)(RoleID)
                        oAssignment.AssignedTo = oRole
                        oAssignment.CreatedBy = CreatedBy
                        oAssignment.CreatedOn = CreatedOn
                        oAssignment.Deny = Deny
                        oAssignment.File = oFile
                        oAssignment.ModifiedBy = CreatedBy
                        oAssignment.ModifiedOn = CreatedOn
                        oAssignment.Permission = Permission
                        oAssignment.Inherited = Inherited
                        DC.SaveOrUpdate(oAssignment)
                        iResponse.Add(oAssignment)
                    Next
                End If
            Catch ex As Exception
            End Try
            Return iResponse
        End Function

        Private Function AddPersonAssignmentToItem(ByVal CreatedOn As DateTime, ByVal CreatedBy As Person, ByVal oFile As CommunityFile, ByVal oPerson As Person, ByVal Deny As Boolean, ByVal Permission As Long, ByVal Inherited As Boolean) As CommunityFilePersonAssignment
            Dim oAssignment As New CommunityFilePersonAssignment
            oAssignment.AssignedTo = oPerson
            oAssignment.CreatedBy = CreatedBy
            oAssignment.CreatedOn = CreatedOn
            oAssignment.Deny = Deny
            oAssignment.File = oFile
            oAssignment.ModifiedBy = CreatedBy
            oAssignment.ModifiedOn = CreatedOn
            oAssignment.Permission = Permission
            oAssignment.Inherited = Inherited
            DC.SaveOrUpdate(oAssignment)

            Return oAssignment
        End Function
        Private Function AddPersonsAssignmentToItem(ByVal CreatedOn As DateTime, ByVal CreatedBy As Person, ByVal ItemId As Long, ByVal MembersID As List(Of Integer), ByVal Deny As Boolean, ByVal Permission As Long, ByVal Inherited As Boolean) As List(Of CommunityFilePersonAssignment)
            Dim iResponse As New List(Of CommunityFilePersonAssignment)
            Try
                Dim oFile As CommunityFile = DC.GetById(Of CommunityFile)(ItemId)
                If Not IsNothing(oFile) Then
                    For Each MemberID In MembersID
                        Dim oAssignment As New CommunityFilePersonAssignment
                        Dim oPerson As Person = DC.GetById(Of Person)(MemberID)
                        oAssignment.AssignedTo = oPerson
                        oAssignment.CreatedBy = CreatedBy
                        oAssignment.CreatedOn = CreatedOn
                        oAssignment.Deny = Deny
                        oAssignment.File = oFile
                        oAssignment.ModifiedBy = CreatedBy
                        oAssignment.ModifiedOn = CreatedOn
                        oAssignment.Permission = Permission
                        oAssignment.Inherited = Inherited
                        DC.SaveOrUpdate(oAssignment)
                        iResponse.Add(oAssignment)
                    Next
                End If
            Catch ex As Exception
            End Try

            Return iResponse
        End Function
        Private Function AddPersonTypesAssignmentToItem(ByVal CreatedOn As DateTime, ByVal CreatedBy As Person, ByVal ItemId As Long, ByVal TypesID As List(Of Integer), ByVal Deny As Boolean, ByVal Permission As Long, ByVal Inherited As Boolean) As List(Of CommunityFilePersonTypeAssignment)
            Dim iResponse As New List(Of CommunityFilePersonTypeAssignment)
            Try
                Dim oFile As CommunityFile = DC.GetById(Of CommunityFile)(ItemId)
                If Not IsNothing(oFile) Then
                    For Each TypeID In TypesID
                        Dim oAssignment As New CommunityFilePersonTypeAssignment
                        oAssignment.AssignedTo = TypeID
                        oAssignment.CreatedBy = CreatedBy
                        oAssignment.CreatedOn = CreatedOn
                        oAssignment.Deny = Deny
                        oAssignment.File = oFile
                        oAssignment.ModifiedBy = CreatedBy
                        oAssignment.ModifiedOn = CreatedOn
                        oAssignment.Permission = Permission
                        oAssignment.Inherited = Inherited
                        DC.SaveOrUpdate(oAssignment)
                        iResponse.Add(oAssignment)
                    Next
                End If
            Catch ex As Exception

            End Try
            Return iResponse
        End Function
#End Region



        Public Function GetRepositorySize(ByVal CommunityID As Integer, ByVal Personal As Boolean, ByVal EvaluateDeletedFiles As Boolean) As Long
            Dim RepositorySize As Long = 0
            Try
                DC.BeginTransaction()
                Dim oCommunity As Community = DC.GetCurrentSession.Get(Of Community)(CommunityID)
                RepositorySize = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)() Where f.CommunityOwner Is oCommunity _
                        AndAlso ((Not EvaluateDeletedFiles AndAlso f.isDeleted = False) OrElse EvaluateDeletedFiles) _
                        AndAlso (Personal = f.isPersonal) AndAlso f.isFile _
                        Select f.Size).Sum
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try

            Return RepositorySize
        End Function

        Public Function ExistInFolder(ByVal FolderId As Long, ByVal oCommunity As Community, ByVal ItemId As Long, ByVal SearchDisplayName As String, ByVal isFile As Boolean) As Boolean
            Dim oResponse As Boolean = False
            Try
                DC.BeginTransaction()
                Dim oFileNames As List(Of String) = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)() _
                                                     Where f.CommunityOwner Is oCommunity AndAlso f.FolderId = FolderId AndAlso f.DisplayName.StartsWith(SearchDisplayName, StringComparison.InvariantCultureIgnoreCase) AndAlso f.Id <> ItemId AndAlso f.isFile = isFile Select f.DisplayName).ToList
                oResponse = (From name In oFileNames Where name.ToLower = SearchDisplayName.ToLower).Any
                DC.Commit()
            Catch ex As Exception
                oResponse = True
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
            Return oResponse
        End Function

        Public Function SaveItem(ByVal oItem As CommunityFile, ByVal PersonID As Integer) As CommunityFile
            Dim oResponse As CommunityFile = Nothing
            Try
                DC.BeginTransaction()
                Dim oPerson As Person = DC.GetById(Of Person)(PersonID)

                If Not IsNothing(oPerson) Then
                    oItem.ModifiedOn = Now
                    oItem.ModifiedBy = oPerson
                    DC.GetCurrentSession.SaveOrUpdate(oItem)
                    oResponse = oItem
                Else
                    oResponse = Nothing
                End If
                DC.Commit()
            Catch ex As Exception
                oResponse = Nothing
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
            Return oResponse
        End Function

        Public Sub AddDownloadToFile(ByVal oFile As BaseCommunityFile, ByVal DownloaderID As Integer, ByVal Link As ModuleLink, ByVal serviceCode As String)
            Try
                DC.BeginTransaction()
                Dim oDownloader As Person = DC.GetById(Of Person)(DownloaderID)
                Dim oDownloadInfo As New FileDownloadInfo
                oDownloadInfo.CommunityOwner = oFile.CommunityOwner
                oDownloadInfo.Downloader = oDownloader
                oDownloadInfo.File = oFile
                oDownloadInfo.UniqueID = oFile.UniqueID
                oDownloadInfo.RepositoryItemType = oFile.RepositoryItemType
                oDownloadInfo.Link = Link
                oDownloadInfo.ServiceCode = serviceCode
                DC.GetCurrentSession.SaveOrUpdate(oDownloadInfo)

                oFile.Downloads = oFile.Downloads + 1
                DC.GetCurrentSession.SaveOrUpdate(oFile)
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
        End Sub

        Public Function GetFolderFatherOf(ByVal ItemId As Long) As CommunityFile
            Dim oFolder As CommunityFile = Nothing
            Try
                DC.BeginTransaction()
                Dim FatherID As Long = 0
                FatherID = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)() Where f.Id = ItemId Select f.FolderId).FirstOrDefault
                If FatherID <> 0 Then
                    oFolder = DC.GetById(Of CommunityFile)(FatherID)
                End If
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try

            Return oFolder
        End Function
        Public Sub SaveItemsVisibility(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal Visibility As Boolean)
            Dim oResponse As CommunityFile = Nothing
            Try
                DC.BeginTransaction()
                Dim oPerson As Person = DC.GetById(Of Person)(PersonID)
                Dim oCommunity As Community = DC.GetById(Of Community)(CommunityID)
                If Not IsNothing(oPerson) AndAlso Not (CommunityID > 0 AndAlso IsNothing(oCommunity)) Then
                    Dim oFiles As List(Of CommunityFile) = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)() Where f.CommunityOwner Is oCommunity AndAlso f.isFile AndAlso f.FolderId = FolderID AndAlso f.isVisible <> Visibility Select f).ToList
                    For Each oFile In oFiles
                        oFile.ModifiedOn = Now
                        oFile.ModifiedBy = oPerson
                        oFile.isVisible = Visibility
                        DC.GetCurrentSession.SaveOrUpdate(oFile)
                    Next
                End If
                DC.Commit()
            Catch ex As Exception
                oResponse = Nothing
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
        End Sub

        Public Function DeleteFile(ByVal oFile As CommunityFile, ByVal CommunityPath As String) As Boolean
            Dim oResponse As Boolean = False
            Try
                DC.BeginTransaction()
                Dim FileName As String = oFile.UniqueID.ToString & ".stored"
                If oFile.isFile AndAlso oFile.RepositoryItemType <> RepositoryItemType.FileStandard Then
                    'Dim oScorm As ScormFile = (From sf In DC.GetCurrentSession.Linq(Of ScormFile)() Where sf.File Is oFile AndAlso sf.FileUniqueID = oFile.UniqueID Select sf).FirstOrDefault
                    'If Not IsNothing(oScorm) Then
                    '    oScorm.Status = ScormStatus.Deleting
                    '    DC.GetCurrentSession.SaveOrUpdate(oScorm)
                    'End If
                    DeleteFileTransfer(oFile)
                End If
                Dim oAssigments As List(Of CommunityFileAssignment) = (From fa In DC.GetCurrentSession().Linq(Of CommunityFileAssignment)() Where fa.File Is oFile Select fa).ToList
                For Each Assigment In oAssigments
                    DC.GetCurrentSession.Delete(Assigment)
                Next

                'Dim oDiaryFiles As List(Of EventCommunityFile) = (From ef In DC.GetCurrentSession().Linq(Of EventCommunityFile)() Where ef Is oFile Select ef).ToList

                'For Each oDiaryFile In oDiaryFiles
                '    DC.GetCurrentSession.Delete(oDiaryFile)
                'Next

                Dim oWorkBookFiles As List(Of WorkBookCommunityFile) = (From ef In DC.GetCurrentSession().Linq(Of WorkBookCommunityFile)() Where ef.FileCommunity Is oFile Select ef).ToList

                For Each oWorkBookFile In oWorkBookFiles
                    DC.GetCurrentSession.Delete(oWorkBookFile)
                Next

                DC.Delete(oFile)
                DC.Commit()
                If Delete.File(CommunityPath & FileName) = ItemRepositoryStatus.Deleted Then
                    oResponse = True
                Else
                    oResponse = Nothing
                    If DC.isInTransaction Then
                        DC.Rollback()
                    End If
                End If

            Catch ex As Exception
                oResponse = Nothing
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
            Return oResponse
        End Function
        Public Function DeleteFiles(ByVal oFileID As List(Of Long), ByVal CommunityPath As String) As Boolean
            Dim oResponse As Boolean = False
            Try
                Dim oFiles As List(Of BaseCommunityFile) = (From f In Me.DC.GetCurrentSession.Linq(Of BaseCommunityFile)() _
                                Where oFileID.Contains(f.Id) Select f).ToList
                If oFiles.Count > 0 Then
                    Dim FilesName As List(Of String) = (From f In oFiles Select CommunityPath & f.UniqueID.ToString & ".stored").ToList
                    DC.BeginTransaction()

                    DeleteFilesTransfer((From f In oFiles Where f.isFile AndAlso f.RepositoryItemType <> RepositoryItemType.FileStandard Select f).ToList())
                    For Each oItem As BaseCommunityFile In oFiles
                        Dim oFile As BaseCommunityFile = oItem
                        'If oFile.isSCORM Then
                        '    Dim oScorm As ScormFile = (From sf In DC.GetCurrentSession.Linq(Of ScormFile)() Where sf.File Is oFile AndAlso sf.FileUniqueID = oFile.UniqueID Select sf).FirstOrDefault
                        '    If Not IsNothing(oScorm) Then
                        '        oScorm.Status = ScormStatus.Deleting
                        '        DC.GetCurrentSession.SaveOrUpdate(oScorm)
                        '    End If
                        'End If
                        If TypeOf oItem Is CommunityFile Then
                            'Dim oDiaryFiles As List(Of EventCommunityFile) = (From ef In DC.GetCurrentSession().Linq(Of EventCommunityFile)() Where ef.FileCommunity Is oFile Select ef).ToList

                            'For Each oDiaryFile In oDiaryFiles
                            '    DC.GetCurrentSession.Delete(oDiaryFile)
                            'Next

                            Dim oWorkBookFiles As List(Of WorkBookCommunityFile) = (From ef In DC.GetCurrentSession().Linq(Of WorkBookCommunityFile)() Where ef.FileCommunity Is oFile Select ef).ToList

                            For Each oWorkBookFile In oWorkBookFiles
                                DC.GetCurrentSession.Delete(oWorkBookFile)
                            Next
                        End If
                        DC.Delete(oFile)
                    Next

                    Dim oAssigments As List(Of CommunityFileAssignment) = (From fa In DC.GetCurrentSession().Linq(Of CommunityFileAssignment)() Where oFiles.Contains(fa.File) Select fa).ToList
                    For Each Assigment In oAssigments
                        DC.GetCurrentSession.Delete(Assigment)
                    Next

                    DC.Commit()

                    Delete.Files(FilesName)
                End If
                oResponse = True
            Catch ex As Exception
                oResponse = Nothing
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
            Return oResponse
        End Function
        Public Function DeleteFolder(ByVal oFolder As CommunityFile, ByVal CommunityPath As String) As Boolean
            Dim oResponse As Boolean = False
            Try
                DC.BeginTransaction()
                Dim oFileNames As New List(Of String)
                DeleteFolderContent(oFolder, CommunityPath, oFileNames)

                Dim oAssigments As List(Of CommunityFileAssignment) = (From fa In DC.GetCurrentSession().Linq(Of CommunityFileAssignment)() Where fa.File Is oFolder Select fa).ToList
                For Each Assigment In oAssigments
                    DC.GetCurrentSession.Delete(Assigment)
                Next
                DC.GetCurrentSession.Delete(oFolder)
                DC.Commit()
                Delete.Files(oFileNames)
                oResponse = True
            Catch ex As Exception
                oResponse = Nothing
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
            Return oResponse
        End Function
        Private Sub DeleteFolderContent(ByVal oFolder As CommunityFile, ByVal CommunityPath As String, ByVal FileName As List(Of String))
            Dim oItems As List(Of CommunityFile)
            oItems = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)() Where f.FolderId = oFolder.Id AndAlso f.isPersonal = oFolder.isPersonal AndAlso f.CommunityOwner Is oFolder.CommunityOwner Select f).ToList

            Dim oAssigments As List(Of CommunityFileAssignment) = (From fa In DC.GetCurrentSession().Linq(Of CommunityFileAssignment)() Where oItems.Contains(fa.File) Select fa).ToList
            For Each Assigment In oAssigments
                DC.GetCurrentSession.Delete(Assigment)
            Next

            DeleteFilesTransfer((From f In oItems Where f.isFile AndAlso f.RepositoryItemType <> RepositoryItemType.FileStandard Select f).ToList())

            For Each oItem In (From f In oItems Where f.isFile Select f).ToList
                Dim oFile As CommunityFile = oItem
                FileName.Add(CommunityPath & oFile.UniqueID.ToString & ".stored")
                'If oFile.isSCORM Then
                '    Dim oScorm As ScormFile = (From sf In DC.GetCurrentSession.Linq(Of ScormFile)() Where sf.File Is oFile AndAlso sf.FileUniqueID = oFile.UniqueID Select sf).FirstOrDefault
                '    If Not IsNothing(oScorm) Then
                '        oScorm.Status = ScormStatus.Deleting
                '        DC.GetCurrentSession.SaveOrUpdate(oScorm)
                '    End If
                'End If

                'Dim oDiaryFiles As List(Of EventCommunityFile) = (From ef In DC.GetCurrentSession().Linq(Of EventCommunityFile)() Where ef.FileCommunity Is oFile Select ef).ToList

                'For Each oDiaryFile In oDiaryFiles
                '    DC.GetCurrentSession.Delete(oDiaryFile)
                'Next

                Dim oWorkBookFiles As List(Of WorkBookCommunityFile) = (From ef In DC.GetCurrentSession().Linq(Of WorkBookCommunityFile)() Where ef.FileCommunity Is oFile Select ef).ToList

                For Each oWorkBookFile In oWorkBookFiles
                    DC.GetCurrentSession.Delete(oWorkBookFile)
                Next

                DC.GetCurrentSession.Delete(oFile)
            Next

            For Each oFolder In (From f In oItems Where Not f.isFile Select f).ToList
                DeleteFolderContent(oFolder, CommunityPath, FileName)
                DC.GetCurrentSession.Delete(oFolder)
            Next

        End Sub

        Public Function DeleteRepository(ByVal isPersonal As Boolean, ByVal oCommunity As Community, ByVal CommunityPath As String) As List(Of dtoDeletedItem)
            Dim oResponse As New List(Of dtoDeletedItem)
            Try
                DC.BeginTransaction()
                oResponse = DeleteCommunityRepository(isPersonal, oCommunity, CommunityPath)
            Catch ex As Exception
                oResponse.Clear()
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
            Return oResponse
        End Function
        Private Function DeleteCommunityRepository(ByVal isPersonal As Boolean, ByVal oCommunity As Community, ByVal CommunityPath As String) As List(Of dtoDeletedItem)
            Dim oResponse As New List(Of dtoDeletedItem)
            Try
                Dim oItems As List(Of CommunityFile)
                oItems = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)() Where f.CommunityOwner Is oCommunity AndAlso f.isPersonal = isPersonal Select f).ToList

                Dim FilesName As List(Of String) = (From f In oItems Where f.isFile Select CommunityPath & f.UniqueID.ToString & ".stored").ToList

                Dim list As List(Of System.Guid) = (From f In oItems Where f.isFile AndAlso f.RepositoryItemType <> RepositoryItemType.FileStandard Select f.UniqueID).ToList
                'Dim Query = (From sf In DC.GetCurrentSession.Linq(Of ScormFile)() _
                '             Where ScormsID.Contains(sf.Id) Select sf)

                oResponse.AddRange((From f In oItems Where f.FolderId = 0 Select New dtoDeletedItem(f.Id, f.DisplayName, f.isFile, f.FolderId, "", f.RepositoryItemType)).ToList)
                DeleteFilesTransfer(list)
                'For Each oItem As ScormFile In Query.ToList
                '    oItem.Status = ScormStatus.Deleting
                '    DC.GetCurrentSession.SaveOrUpdate(oItem)
                'Next

                Dim oAssigments As List(Of CommunityFileAssignment) = (From fa In DC.GetCurrentSession().Linq(Of CommunityFileAssignment)() Where oItems.Contains(fa.File) Select fa).ToList
                For Each Assigment In oAssigments
                    DC.GetCurrentSession.Delete(Assigment)
                Next

                Dim FileItems As List(Of CommunityFile) = (From f In oItems Where f.isFile Select f).ToList
                'Dim oDiaryFiles As List(Of EventCommunityFile) = (From ef In DC.GetCurrentSession().Linq(Of EventCommunityFile)() Where FileItems.Contains(ef.FileCommunity) Select ef).ToList
                'For Each oDiaryFile In oDiaryFiles
                '    DC.GetCurrentSession.Delete(oDiaryFile)
                'Next

                Dim oWorkBookFiles As List(Of WorkBookCommunityFile) = (From ef In DC.GetCurrentSession().Linq(Of WorkBookCommunityFile)() Where FileItems.Contains(ef.FileCommunity) Select ef).ToList
                For Each oWorkBookFile In oWorkBookFiles
                    DC.GetCurrentSession.Delete(oWorkBookFile)
                Next

                For Each oItem As CommunityFile In oItems
                    DC.GetCurrentSession.Delete(oItem)
                Next
                DC.Commit()

                Delete.Files(FilesName)
            Catch ex As Exception
                Throw New Exception
            End Try
            Return oResponse
        End Function
        Public Function ExistItem(ByVal ItemId As Long) As Boolean
            Dim iResponse As Boolean = False
            Try
                DC.BeginTransaction()
                iResponse = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)() Where f.Id = ItemId).Any
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try

            Return iResponse
        End Function

        Public Function DeleteItems(ByVal ItemsID As List(Of Long), ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal CommunityPath As String) As List(Of dtoDeletedItem)
            Dim oResponse As New List(Of dtoDeletedItem)
            Try
                DC.BeginTransaction()
                Dim oCommunity As Community = Me.DC.GetById(Of Community)(CommunityID)
                If ItemsID.Contains(0) AndAlso Not IsNothing(oCommunity) AndAlso CommunityID > 0 Then
                    oResponse = Me.DeleteCommunityRepository(isPersonal, oCommunity, CommunityPath)
                Else
                    Dim oAssigments As New List(Of CommunityFileAssignment)
                    Dim oFiles As List(Of CommunityFile) = (From f In Me.DC.GetCurrentSession.Linq(Of CommunityFile)() _
                                Where ItemsID.Contains(f.Id) AndAlso f.isFile Select f).ToList

                    Dim FilesName As List(Of String) = (From f In oFiles Select CommunityPath & f.UniqueID.ToString & ".stored").ToList
                    oAssigments.AddRange((From fa In DC.GetCurrentSession().Linq(Of CommunityFileAssignment)() Where oFiles.Contains(fa.File) Select fa).ToList)

                    Dim oFolders As List(Of CommunityFile) = (From f In Me.DC.GetCurrentSession.Linq(Of CommunityFile)() _
                                Where ItemsID.Contains(f.Id) AndAlso Not f.isFile Select f).ToList

                    Dim idFolderFathers As List(Of Long) = (From f In Me.DC.GetCurrentSession.Linq(Of CommunityFile)() Where ItemsID.Contains(f.Id) Distinct Select f.FolderId).ToList()

                    Dim queryFolderName = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)()
                                 Where idFolderFathers.Contains(f.Id) Select New With {.Id = f.Id, .Name = f.Name}).ToList()

                    oResponse.AddRange((From f In oFolders Select New dtoDeletedItem(f.Id, f.Name, f.isFile, f.FolderId, queryFolderName.Where(Function(fa) fa.Id = f.FolderId).Select(Function(fa) fa.Name).FirstOrDefault, f.isSCORM)).ToList)
                    oResponse.AddRange((From f In oFiles Where (From fo In oFolders Select fo.Id).ToList.Contains(f.Id) _
                                        Select New dtoDeletedItem(f.Id, f.DisplayName, f.isFile, f.FolderId, queryFolderName.Where(Function(fa) fa.Id = f.FolderId).Select(Function(fa) fa.Name).FirstOrDefault, f.isSCORM)).ToList)
                    ', f.FolderId, (From folder In oItems Where folder.Id = f.FolderId Select folder.Name).FirstOrDefault)
                    ' oResponse.AddRange((From f In oFiles Select New dtoDeletedItem(f.Id, f.Name, f.isFile, f.FolderId, )).ToList)
                    For Each oItem As CommunityFile In oFiles
                        Dim oFile As CommunityFile = oItem
                        'If oFile.isSCORM Then
                        '    Dim oScorm As ScormFile = (From sf In DC.GetCurrentSession.Linq(Of ScormFile)() Where sf.File Is oFile AndAlso sf.FileUniqueID = oFile.UniqueID Select sf).FirstOrDefault
                        '    If Not IsNothing(oScorm) Then
                        '        oScorm.Status = ScormStatus.Deleting
                        '        DC.GetCurrentSession.SaveOrUpdate(oScorm)
                        '    End If
                        'End If
                        If oFile.isFile AndAlso oFile.RepositoryItemType <> RepositoryItemType.FileStandard Then
                            DeleteFileTransfer(oFile)
                        End If
                        DC.Delete(oFile)
                    Next

                    oAssigments.AddRange((From fa In DC.GetCurrentSession().Linq(Of CommunityFileAssignment)() Where oFolders.Contains(fa.File) Select fa).ToList)

                    For Each oItem As CommunityFile In oFolders
                        Dim oFolder As CommunityFile = oItem
                        DC.Delete(oFolder)
                    Next

                    For Each Assigment In oAssigments
                        DC.Delete(Assigment)
                    Next
                    DC.Commit()
                    Delete.Files(FilesName)
                End If
            Catch ex As Exception
                oResponse = New List(Of dtoDeletedItem)
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
            Return oResponse
        End Function



        Private Sub DeleteFileTransfer(file As BaseCommunityFile)
            Dim list As List(Of FileTransfer) = (From f In DC.GetCurrentSession.Linq(Of FileTransfer)()
                                                           Where f.File Is file AndAlso f.FileUniqueID = file.UniqueID Select f).ToList

            For Each fileTransfer As FileTransfer In list
                fileTransfer.Status = TransferStatus.Deleting
                DC.GetCurrentSession.SaveOrUpdate(fileTransfer)
            Next
        End Sub
        Private Sub DeleteFilesTransfer(files As List(Of CommunityFile))
            DeleteFilesTransfer((From f In files Where f.UniqueID <> Guid.Empty Select f.UniqueID).ToList)
        End Sub
        Private Sub DeleteFilesTransfer(files As List(Of BaseCommunityFile))
            DeleteFilesTransfer((From f In files Where f.UniqueID <> Guid.Empty Select f.UniqueID).ToList)
        End Sub
        Private Sub DeleteFilesTransfer(filesId As List(Of System.Guid))
            Dim list As List(Of FileTransfer) = (From f In DC.GetCurrentSession.Linq(Of FileTransfer)()
                                                         Where filesId.Contains(f.FileUniqueID) Select f).ToList

            For Each fileTransfer As FileTransfer In list
                fileTransfer.Status = TransferStatus.Deleting
                DC.GetCurrentSession.SaveOrUpdate(fileTransfer)
            Next
        End Sub

        Public Function FindAllFolderItems(ByVal FolderID As Long, ByVal oCommunity As Community, ByVal Personal As Boolean) As List(Of CommunityFile)
            Dim iResponse As New List(Of CommunityFile)
            Try
                DC.BeginTransaction()
                iResponse = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)() Where f.isPersonal = Personal AndAlso f.CommunityOwner Is oCommunity AndAlso f.FolderId = FolderID AndAlso f.isDeleted = False Select f).ToList
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try

            Return iResponse
        End Function
        Public Function FindAllFolderFileNames(ByVal FolderID As Long, ByVal oCommunity As Community, ByVal Personal As Boolean) As List(Of String)
            Dim iResponse As New List(Of String)
            Try
                DC.BeginTransaction()
                iResponse = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)() Where f.isFile AndAlso f.isPersonal = Personal AndAlso f.CommunityOwner Is oCommunity AndAlso f.FolderId = FolderID AndAlso f.isDeleted = False Select f.DisplayName).ToList
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
            Return iResponse
        End Function
        Public Function FindAllFolderSubFolderNames(ByVal FolderID As Long, ByVal oCommunity As Community, ByVal Personal As Boolean) As List(Of String)
            Dim iResponse As New List(Of String)
            Try
                DC.BeginTransaction()
                iResponse = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)() Where Not f.isFile AndAlso f.isPersonal = Personal AndAlso f.CommunityOwner Is oCommunity AndAlso f.FolderId = FolderID AndAlso f.isDeleted = False Select f.Name).ToList
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
            Return iResponse
        End Function

        Public Function TransferItemsToFolder(ByVal oContext As FileTransferContext, ByVal oFilesToImport As List(Of dtoGenericFile), ByVal oFoldersToImport As List(Of dtoFileFolder)) As Boolean
            Dim TransferedFiles As New List(Of String)
            Dim iResponse As Boolean = False
            Try
                DC.BeginTransaction()
                Dim oCommunity As Community = Me.DC.GetById(Of Community)(oContext.CommunityID)
                Dim oOwner As Person = Me.DC.GetById(Of Person)(oContext.OwnerID)
                If (oContext.CommunityID > 0 AndAlso oCommunity Is Nothing) OrElse IsNothing(oOwner) Then
                    iResponse = False
                    DC.Commit()
                Else
                    Dim SourceItem As CommunityFile
                    Dim TransferedFile As CommunityFile

                    If Not lm.Comol.Core.File.Exists.Directory(oContext.DestinationRepositoryFolder) Then
                        lm.Comol.Core.File.Create.Directory(oContext.DestinationRepositoryFolder)
                    End If
                    For Each oDtoFile In oFilesToImport
                        SourceItem = Me.DC.GetById(Of CommunityFile)(oDtoFile.ID)
                        If Not IsNothing(SourceItem) Then
                            TransferedFile = Me.TransferFile(oContext, Duplicate(SourceItem, oDtoFile.Name, oOwner, oCommunity, oContext.Visible, oContext.isPersonal, oContext.DestinationFolderID), TransferedFiles)
                        End If
                    Next
                    For Each oDtoFolder In oFoldersToImport
                        ParseFolder(oDtoFolder, oContext, oContext.DestinationFolderID, oOwner, oCommunity, TransferedFiles)
                    Next
                    DC.Commit()
                    iResponse = True
                End If
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
            If Not iResponse AndAlso TransferedFiles.Count > 0 Then

            End If
            Return iResponse
        End Function


        Private Function ParseFolder(ByVal oDtoFolder As dtoFileFolder, ByVal oContext As FileTransferContext, ByVal DestinationFolderID As Long, ByVal oOwner As Person, ByVal oCommunity As Community, ByVal TransferedFiles As List(Of String)) As CommunityFile
            Dim oResponse As CommunityFile = Nothing
            Try
                Dim SourceItem As CommunityFile
                Dim TransferedFolder As CommunityFile
                SourceItem = Me.DC.GetById(Of CommunityFile)(oDtoFolder.ID)

                If Not IsNothing(SourceItem) Then
                    TransferedFolder = Me.TransferFolder(oContext, Duplicate(SourceItem, oDtoFolder.Name, oOwner, oCommunity, oContext.Visible, oContext.isPersonal, DestinationFolderID))
                    If Not IsNothing(TransferedFolder) Then
                        Dim SourceFileItem As CommunityFile
                        Dim TransferedFile As CommunityFile
                        For Each oDtoFile In oDtoFolder.Files
                            SourceFileItem = Me.DC.GetById(Of CommunityFile)(oDtoFile.ID)
                            If Not IsNothing(SourceFileItem) Then
                                TransferedFile = Me.TransferFile(oContext, Duplicate(SourceFileItem, oDtoFile.Name, oOwner, oCommunity, oContext.Visible, oContext.isPersonal, TransferedFolder.Id), TransferedFiles)
                            End If
                        Next
                        For Each SubFolder In oDtoFolder.SubFolders
                            ParseFolder(SubFolder, oContext, TransferedFolder.Id, oOwner, oCommunity, TransferedFiles)
                        Next
                    End If
                End If
            Catch ex As Exception
                oResponse = Nothing
            End Try
            Return oResponse
        End Function
        Private Function TransferFile(ByVal oContext As FileTransferContext, ByVal oFile As CommunityFile, ByVal TransferedFiles As List(Of String)) As CommunityFile
            Dim oResponse As CommunityFile = Nothing
            Dim FileNameToSave As String = oContext.DestinationRepositoryFolder & oFile.UniqueID.ToString & ".stored"
            Dim OldFileName As String = oContext.SourceRepositoryFolder & oFile.CloneUniqueID.ToString & ".stored"
            Try
                If lm.Comol.Core.File.Exists.File(OldFileName) Then
                    VerifyAndUpdateItemName(oFile)
                    DC.GetCurrentSession.SaveOrUpdate(oFile)

                    lm.Comol.Core.File.Create.CopyFile(OldFileName, FileNameToSave)
                    TransferItemPermission(oContext.DownlodableByCommunity, oFile)

                    If oFile.isFile AndAlso oFile.RepositoryItemType <> RepositoryItemType.FileStandard Then
                        AddFileForTransfer(oFile, oContext.DestinationRepositoryFolder, oFile.CloneUniqueID)
                    End If
                    'If oFile.isSCORM Then
                    '    Dim oScorm As New ScormFile()
                    '    oScorm.FileUniqueID = oFile.UniqueID
                    '    oScorm.File = oFile
                    '    If Not oContext.DestinationRepositoryFolder.EndsWith("\") Then
                    '        oScorm.Path =  & "\"
                    '    Else
                    '        oScorm.Path = oContext.DestinationRepositoryFolder
                    '    End If
                    '    oScorm.FileName = oFile.UniqueID.ToString & ".stored"
                    '    oScorm.CloneID = oFile.CloneUniqueID
                    '    DC.GetCurrentSession.SaveOrUpdate(oScorm)
                    'End If
                    oResponse = oFile
                    TransferedFiles.Add(FileNameToSave)
                End If
            Catch ex As Exception
                Delete.File(FileNameToSave)
                oResponse = Nothing
            End Try
            Return oResponse
        End Function

        Private Function TransferFolder(ByVal oContext As FileTransferContext, ByVal oFolder As CommunityFile) As CommunityFile
            Dim oResponse As CommunityFile = Nothing
            Try
                VerifyAndUpdateItemName(oFolder)
                DC.GetCurrentSession.SaveOrUpdate(oFolder)

                TransferItemPermission(oContext.DownlodableByCommunity, oFolder)
                oResponse = oFolder
            Catch ex As Exception
                oResponse = Nothing
            End Try
            Return oResponse
        End Function
        Private Sub TransferItemPermission(ByVal DownlodableByCommunity As Boolean, ByVal oItem As CommunityFile)
            Dim FolderId As Long = oItem.FolderId
            Dim Permission As Long = COL_BusinessLogic_v2.UCServices.Services_File.Base2Permission.DownloadFile

            If DownlodableByCommunity Then
                Me.AddCommunityAssignmentToItem(oItem.CreatedOn, oItem.Owner, oItem.Id, False, Permission, False)
            Else
                Me.AddPersonAssignmentToItem(oItem.CreatedOn, oItem.Owner, oItem, oItem.Owner, False, Permission, False)
            End If

            If FolderId = 0 Then
                If DownlodableByCommunity Then
                    Me.AddCommunityAssignmentToItem(oItem.CreatedOn, oItem.Owner, oItem.Id, False, Permission, True)
                Else
                    Me.AddPersonAssignmentToItem(oItem.CreatedOn, oItem.Owner, oItem, oItem.Owner, False, Permission, True)
                End If
            Else
                Dim oFatherAssignments As List(Of CommunityFileAssignment) = (From fa In DC.GetCurrentSession.Linq(Of CommunityFileAssignment)() Where fa.File.Id = FolderId AndAlso fa.Inherited Select fa).ToList

                Me.ApplyInheritedAssignment(oItem.CreatedOn, oItem.Owner, oItem, oFatherAssignments, False, Permission)
            End If

            'Me.ApplyInheritedAssignment(oFolder.CreatedOn, oFolder.Owner, oFolder, (From fa In DC.GetCurrentSession.Linq(Of CommunityFileAssignment)() Where fa.File.Id = oFolder.FolderId AndAlso fa.Inherited Select fa).ToList, False, Permission)
            'If DownlodableByCommunity Then
            '    Dim oAssignment As New CommunityFileCommunityAssignment
            '    oAssignment.AssignedTo = oItem.CommunityOwner
            '    oAssignment.Deny = False
            '    oAssignment.File = oItem
            '    oAssignment.Permission = 10
            '    oAssignment.CreatedBy = oItem.Owner
            '    oAssignment.CreatedOn = oItem.CreatedOn
            '    DC.GetCurrentSession.SaveOrUpdate(oAssignment)
            'Else
            '    Dim oAssignment As New CommunityFilePersonAssignment
            '    oAssignment.AssignedTo = oItem.Owner
            '    oAssignment.Deny = False
            '    oAssignment.File = oItem
            '    oAssignment.Permission = 10
            '    oAssignment.CreatedBy = oItem.Owner
            '    oAssignment.CreatedOn = oItem.CreatedOn
            '    DC.GetCurrentSession.SaveOrUpdate(oAssignment)
            'End If
        End Sub

        'Private Sub UpdateCommunityItemName(ByVal oItem As CommunityFile)
        '    Dim ProposedName As String = oItem.Name
        '    Dim oQuery = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)() Where f.FolderId = oItem.FolderId AndAlso f.CommunityOwner Is oItem.CommunityOwner AndAlso f.DisplayName.Contains(oItem.DisplayName) Select f).ToList

        '    Dim Names As List(Of String) = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)() Where f.FolderId = oItem.FolderId AndAlso f.CommunityOwner Is oItem.CommunityOwner AndAlso f.isPersonal = oItem.isPersonal AndAlso f.isFile = oItem.isFile AndAlso f.DisplayName.Contains(oItem.DisplayName) Select f.Name).ToList

        '    Dim i As Integer = 1
        '    While Names.Contains(ProposedName & oItem.Extension)
        '        ProposedName = oItem.Name & " (" & i.ToString & ")"
        '        i += 1
        '    End While
        '    oItem.Name = ProposedName
        'End Sub

        Private Sub VerifyAndUpdateFolderName(ByVal folder As CommunityFile)
            Dim ProposedName As String = folder.Name
            Dim Names As List(Of String) = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)() _
                                            Where f.FolderId = folder.FolderId AndAlso f.CommunityOwner Is folder.CommunityOwner AndAlso f.isPersonal = folder.isPersonal AndAlso f.isFile = folder.isFile _
                                            AndAlso f.Name.Contains(folder.Name) Select f.Name).ToList

            Dim i As Integer = 1
            While Names.Contains(ProposedName)
                ProposedName = folder.Name & " [" & i.ToString & "]"
                i += 1
            End While
            folder.Name = ProposedName
        End Sub
        Private Sub VerifyAndUpdateItemName(ByVal oItem As CommunityFile)
            Dim ProposedName As String = oItem.Name
            Dim Names As List(Of String) = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)() _
                                            Where f.FolderId = oItem.FolderId AndAlso f.CommunityOwner Is oItem.CommunityOwner AndAlso f.isPersonal = oItem.isPersonal AndAlso f.isFile = oItem.isFile _
                                            AndAlso f.Extension.Contains(oItem.Extension) AndAlso f.Name.Contains(oItem.Name) Select f.Name).ToList

            Dim i As Integer = 1
            While Names.Contains(ProposedName)
                ProposedName = oItem.Name & " [" & i.ToString & "]"
                i += 1
            End While
            oItem.Name = ProposedName
        End Sub

        Private Function Duplicate(ByVal oItem As CommunityFile, ByVal NewName As String, ByVal oOwner As Person, ByVal oCommunity As Community, ByVal isVisible As Boolean, ByVal isPersonal As Boolean, ByVal FolderID As Long) As CommunityFile
            Dim oCommunityFile As New CommunityFile
            With oCommunityFile
                .CloneID = oItem.Id
                .CommunityOwner = oCommunity
                .ContentType = oItem.ContentType
                .Description = oItem.Description
                .Downloads = 0
                .Extension = oItem.Extension
                .FileCategoryID = oItem.FileCategoryID
                .FilePath = ""
                .FolderId = FolderID
                .isDeleted = False
                .IsDownloadable = oItem.IsDownloadable
                .isFile = oItem.isFile
                .isPersonal = isPersonal
                .isSCORM = oItem.isSCORM
                .isVideocast = oItem.isVideocast
                .isVirtual = oItem.isVirtual
                .isVisible = isVisible
                .Level = oItem.Level
                .Name = NewName
                .Owner = oOwner
                .Size = oItem.Size
                .CloneUniqueID = oItem.UniqueID
                .RepositoryItemType = oItem.RepositoryItemType
                .ModifiedBy = oOwner
                .ModifiedOn = .CreatedOn
            End With
            Return oCommunityFile
        End Function
        ' DC.BeginTransaction()
        'Dim FilesName As List(Of System.Guid) = (From f In Me.DC.GetCurrentSession.Linq(Of WorkBookInternalFile)() _
        '                                         Where f.ItemOwner Is oItem Select f.File.Id).ToList

        '            For Each oFile As WorkBookFile In oItem.Files
        '                DC.Delete(oFile)
        '            Next
        '            DC.Delete(oItem)
        '            DC.Commit()

        '           




        Public Function GetRepositoryScormFiles(ByVal CommunityID As Long, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean, ByVal oPerson As Person) As List(Of CommunityFile)
            Dim oCommunityFiles As New List(Of CommunityFile)
            Try
                Dim oCommunity As Community = Me.GetCommunity(CommunityID)
                Dim oRole As Role = (From s As Subscription In DC.GetCurrentSession.Linq(Of Subscription)() Where s.Community Is oCommunity AndAlso s.Person Is oPerson Select s.Role).FirstOrDefault
                Dim Query = (From file In DC.GetCurrentSession.Linq(Of CommunityFile)() _
                                   Where file.CommunityOwner Is oCommunity AndAlso file.isFile AndAlso file.isSCORM _
                                   AndAlso (ShowHiddenItems = True OrElse (file.isVisible OrElse file.Owner Is oPerson)))

                If AdminPurpose Then
                    oCommunityFiles = (From f In Query Select f).ToList
                Else
                    Dim FilesID As List(Of Long) = (From f In Query Select f.Id).ToList
                    oCommunityFiles = (From fa In DC.GetCurrentSession.Linq(Of CommunityFileCommunityAssignment)() Where FilesID.Contains(fa.File.Id) AndAlso fa.Inherited AndAlso fa.AssignedTo Is oCommunity AndAlso Not fa.Deny Select fa.File).ToList
                    If CommunityID = 0 Then
                        oCommunityFiles.AddRange((From fa In DC.GetCurrentSession.Linq(Of CommunityFilePersonTypeAssignment)() Where Not oCommunityFiles.Contains(fa.File) AndAlso FilesID.Contains(fa.File.Id) AndAlso fa.Inherited AndAlso fa.AssignedTo = oPerson.TypeID AndAlso Not fa.Deny Select fa.File).ToList)
                    Else
                        oCommunityFiles.AddRange((From fa In DC.GetCurrentSession.Linq(Of CommunityFileRoleAssignment)() Where Not oCommunityFiles.Contains(fa.File) AndAlso FilesID.Contains(fa.File.Id) AndAlso fa.Inherited AndAlso fa.AssignedTo Is oRole AndAlso Not fa.Deny Select fa.File).ToList)
                    End If
                    oCommunityFiles.AddRange((From fa In DC.GetCurrentSession.Linq(Of CommunityFilePersonAssignment)() Where Not oCommunityFiles.Contains(fa.File) AndAlso FilesID.Contains(fa.File.Id) AndAlso fa.Inherited AndAlso fa.AssignedTo Is oPerson AndAlso Not fa.Deny Select fa.File).ToList)
                End If
            Catch ex As Exception

            End Try
            Return oCommunityFiles
        End Function
        Public Function GetRepositoryScormFilesID(ByVal CommunityID As Long, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean, ByVal oPerson As Person) As List(Of Long)
            Dim oCommunityFiles As New List(Of Long)
            Try
                Dim oCommunity As Community = Me.GetCommunity(CommunityID)
                Dim oRole As Role = (From s As Subscription In DC.GetCurrentSession.Linq(Of Subscription)() Where s.Community Is oCommunity AndAlso s.Person Is oPerson Select s.Role).FirstOrDefault
                Dim Query = (From file In DC.GetCurrentSession.Linq(Of CommunityFile)() _
                                   Where file.CommunityOwner Is oCommunity AndAlso file.isFile AndAlso file.isSCORM _
                                   AndAlso (ShowHiddenItems = True OrElse (file.isVisible OrElse file.Owner Is oPerson)))

                If AdminPurpose Then
                    oCommunityFiles = (From f In Query Select f.Id).ToList
                Else
                    Dim FilesID As List(Of Long) = (From f In Query Select f.Id).ToList
                    oCommunityFiles = (From fa In DC.GetCurrentSession.Linq(Of CommunityFileCommunityAssignment)() Where FilesID.Contains(fa.File.Id) AndAlso fa.Inherited AndAlso fa.AssignedTo Is oCommunity AndAlso Not fa.Deny Select fa.File.Id).ToList
                    If CommunityID = 0 Then
                        oCommunityFiles.AddRange((From fa In DC.GetCurrentSession.Linq(Of CommunityFilePersonTypeAssignment)() Where Not oCommunityFiles.Contains(fa.File.Id) AndAlso FilesID.Contains(fa.File.Id) AndAlso fa.Inherited AndAlso fa.AssignedTo = oPerson.TypeID AndAlso Not fa.Deny Select fa.File.Id).ToList)
                    Else
                        oCommunityFiles.AddRange((From fa In DC.GetCurrentSession.Linq(Of CommunityFileRoleAssignment)() Where Not oCommunityFiles.Contains(fa.File.Id) AndAlso FilesID.Contains(fa.File.Id) AndAlso fa.Inherited AndAlso fa.AssignedTo Is oRole AndAlso Not fa.Deny Select fa.File.Id).ToList)
                    End If
                    oCommunityFiles.AddRange((From fa In DC.GetCurrentSession.Linq(Of CommunityFilePersonAssignment)() Where Not oCommunityFiles.Contains(fa.File.Id) AndAlso FilesID.Contains(fa.File.Id) AndAlso fa.Inherited AndAlso fa.AssignedTo Is oPerson AndAlso Not fa.Deny Select fa.File.Id).ToList)
                End If
            Catch ex As Exception

            End Try
            Return oCommunityFiles
        End Function

        Public Function GetRepositoryDTOscormFiles(ByVal CommunityID As Long, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean, ByVal PersonID As Integer) As List(Of dtoFileScorm)
            Dim oList As New List(Of dtoFileScorm)
            Try
                Dim oPerson As Person = Me.DC.GetById(Of Person)(PersonID)
                Dim ScormFilesID As List(Of Long) = Me.GetRepositoryScormFilesID(CommunityID, ShowHiddenItems, AdminPurpose, oPerson)
                Dim Query = (From f In DC.GetCurrentSession.Linq(Of BaseCommunityFile)() Where ScormFilesID.Contains(f.Id) Select f)
                Dim QueryNumPlay = (From np In DC.GetCurrentSession.Linq(Of FilePlayInfo)())

                oList = (From f In Query.ToList Select New dtoFileScorm(f, (From np In QueryNumPlay Where np.File Is f).Count)).ToList '

            Catch ex As Exception

            End Try
            Return oList
        End Function

        Public Function GetTransferStatus(ByVal FileUniqueID As System.Guid) As TransferStatus
            Dim iResponse As TransferStatus = TransferStatus.FileTypeError

            Try
                iResponse = (From f In DC.GetCurrentSession.Linq(Of FileTransfer)() Where f.FileUniqueID = FileUniqueID Select f.Status).FirstOrDefault
            Catch ex As Exception
                iResponse = TransferStatus.Error
            End Try
            Return iResponse
        End Function
        Public Sub SaveUserAccessToFile(ByVal WorkingSessionID As Guid, ByVal PersonID As Integer, ByVal oFile As BaseCommunityFile, ByVal LinkID As Long)

            Try
                Dim playInfo As New FilePlayInfo
                Dim oUser As Person = DC.GetById(Of Person)(PersonID)

                If Not IsNothing(oUser) Then
                    With playInfo
                        .WorkingSessionID = WorkingSessionID.ToString
                        .Owner = oUser
                        .FileUniqueID = oFile.UniqueID
                        .File = oFile
                        .CreatedOn = Now
                        .CommunityOwner = oFile.CommunityOwner
                        .DateZone = 1
                        .IdAction = 1
                        .RepositoryItemType = oFile.RepositoryItemType
                    End With
                    DC.SaveOrUpdate(playInfo)

                    If (oFile.RepositoryItemType = RepositoryItemType.ScormPackage) Then
                        Dim oScormToEvaluate As ScormPackageToEvaluate = (From e As ScormPackageToEvaluate In DC.GetCurrentSession.Linq(Of ScormPackageToEvaluate)() _
                                                                   Where e.FileUniqueID = oFile.UniqueID AndAlso e.IdPerson = PersonID AndAlso e.IdLink = LinkID Select e).FirstOrDefault
                        If oScormToEvaluate Is Nothing Then
                            oScormToEvaluate = New ScormPackageToEvaluate
                            With oScormToEvaluate
                                .FileUniqueID = oFile.UniqueID
                                .IdFile = oFile.Id
                                .IdPerson = PersonID
                                .IdLink = LinkID
                                .IsPlaying = False
                                .ToUpdate = False
                                .Deleted = BaseStatusDeleted.None
                                .ModifiedOn = DateTime.Now
                            End With
                            DC.SaveOrUpdate(oScormToEvaluate)
                        End If
                    End If
                End If
            Catch ex As Exception
            End Try
        End Sub
        'Public Function GetRepositoryScormFiles(ByVal CommunityID As Integer) As List(Of CommunityFile)
        '    Dim oFiles As New List(Of CommunityFile)
        '    Try
        '        Dim oCommunity As Community = DC.GetById(Of Community)(CommunityID)
        '        If Not IsNothing(oCommunity) OrElse (oCommunity Is Nothing AndAlso CommunityID = 0) Then
        '            oFiles = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)() _
        '                      Where f.isFile AndAlso f.CommunityOwner Is oCommunity AndAlso f.isSCORM Select f).ToList

        '        End If
        '    Catch ex As Exception
        '        oFiles = New List(Of CommunityFile)
        '    End Try
        '    Return oFiles
        'End Function
        'Public Function GetRepositoryScormFilesID(ByVal CommunityID As Integer) As List(Of Long)
        '    Dim oFiles As New List(Of Long)
        '    Try
        '        Dim oCommunity As Community = DC.GetById(Of Community)(CommunityID)
        '        If Not IsNothing(oCommunity) OrElse (oCommunity Is Nothing AndAlso CommunityID = 0) Then
        '            oFiles = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)() _
        '                      Where f.isFile AndAlso f.CommunityOwner Is oCommunity AndAlso f.isSCORM Select f.Id).ToList

        '        End If
        '    Catch ex As Exception
        '        oFiles = New List(Of Long)
        '    End Try
        '    Return oFiles
        'End Function
        'Public Function GetRepositoryScormFiles(ByVal CommunityID As Integer) As List(Of CommunityFile)
        '    Dim oFiles As New List(Of CommunityFile)
        '    Try
        '        Dim oCommunity As Community = DC.GetById(Of Community)(CommunityID)
        '        If Not IsNothing(oCommunity) OrElse (oCommunity Is Nothing AndAlso CommunityID = 0) Then
        '            oFiles = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)() _
        '                      Where f.isFile AndAlso f.CommunityOwner Is oCommunity AndAlso f.isSCORM Select f).ToList

        '        End If
        '    Catch ex As Exception
        '        oFiles = New List(Of CommunityFile)
        '    End Try
        '    Return oFiles
        'End Function

        Public Function AddModuleLongInternalFile(ByVal file As ModuleLongInternalFile, ByVal oPerson As Person, ByVal CommunityPath As String, ByVal Permission As Long, ByRef Status As ItemRepositoryStatus) As ModuleLongInternalFile
            Dim oResponse As ModuleLongInternalFile = Nothing
            Try
                DC.BeginTransaction()

                DC.GetCurrentSession.SaveOrUpdate(file)

                'If oFile.isSCORM Then
                '    Dim oScorm As New ScormFile()
                '    oScorm.FileUniqueID = oFile.UniqueID
                '    oScorm.File = oFile
                '    If Not CommunityPath.EndsWith("\") Then
                '        oScorm.Path = CommunityPath & "\"
                '    Else
                '        oScorm.Path = CommunityPath
                '    End If
                '    oScorm.FileName = oFile.UniqueID.ToString & ".stored"
                '    DC.GetCurrentSession.SaveOrUpdate(oScorm)
                'End If
                If (file.isFile AndAlso file.RepositoryItemType <> RepositoryItemType.FileStandard) Then
                    AddFileForTransfer(file, CommunityPath)
                End If
                Status = ItemRepositoryStatus.FileUploaded
                oResponse = file

                DC.Commit()
            Catch ex As Exception
                oResponse = Nothing
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                If Status <> ItemRepositoryStatus.FileExist Then
                    Status = ItemRepositoryStatus.CreationError
                End If
            End Try
            Return oResponse
        End Function
        Public Function AddModuleGuidInternalFile(ByVal file As ModuleGuidInternalFile, ByVal oPerson As Person, ByVal CommunityPath As String, ByVal Permission As Long, ByRef Status As ItemRepositoryStatus) As ModuleGuidInternalFile
            Dim oResponse As ModuleGuidInternalFile = Nothing
            Try
                DC.BeginTransaction()

                DC.GetCurrentSession.SaveOrUpdate(file)

                'If oFile.isSCORM Then
                '    Dim oScorm As New ScormFile()
                '    oScorm.FileUniqueID = oFile.UniqueID
                '    oScorm.File = oFile
                '    If Not CommunityPath.EndsWith("\") Then
                '        oScorm.Path = CommunityPath & "\"
                '    Else
                '        oScorm.Path = CommunityPath
                '    End If
                '    oScorm.FileName = oFile.UniqueID.ToString & ".stored"
                '    DC.GetCurrentSession.SaveOrUpdate(oScorm)
                'End If
                If (file.isFile AndAlso file.RepositoryItemType <> RepositoryItemType.FileStandard) Then
                    AddFileForTransfer(file, CommunityPath)
                End If
                Status = ItemRepositoryStatus.FileUploaded
                oResponse = file

                DC.Commit()
            Catch ex As Exception
                oResponse = Nothing
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                If Status <> ItemRepositoryStatus.FileExist Then
                    Status = ItemRepositoryStatus.CreationError
                End If
            End Try
            Return oResponse
        End Function
        Public Function GetFilesForModuleLinkDefaultAction(ByVal oList As List(Of Long), ByVal Permission As Integer, ByVal ModuleID As Integer) As List(Of ModuleLink)
            Dim oLinks As New List(Of ModuleLink)
            Try
                Dim oQuery = (From file In DC.GetCurrentSession.Linq(Of CommunityFile)() _
                              Where file.isFile AndAlso oList.Contains(file.Id) _
                              Select file)

                oLinks = (From item In oQuery.ToList Select New ModuleLink(Permission, GetDefaultAction(item.RepositoryItemType)) With {.DestinationItem = CreateModuleObject(item, ModuleID), .EditEnabled = False}).ToList

            Catch ex As Exception
            End Try

            Return oLinks
        End Function
        Public Function GetModuleActionLinkItems(ByVal IdFiles As List(Of Long), ByVal Permission As Integer, ByVal ModuleID As Integer) As List(Of ModuleActionLink)
            Dim oLinks As New List(Of ModuleActionLink)
            Try
                Dim oQuery = (From file In DC.GetCurrentSession.Linq(Of CommunityFile)() _
                              Where file.isFile AndAlso IdFiles.Contains(file.Id) _
                              Select file)

                oLinks = (From item In oQuery.ToList Select New ModuleActionLink(Permission, GetDefaultAction(item.RepositoryItemType)) With {.ModuleObject = CreateModuleObject(item, ModuleID), .EditEnabled = False}).ToList

            Catch ex As Exception
            End Try

            Return oLinks
        End Function



        Public Sub UpdateModuleInternalFile(ByVal items As List(Of ModuleLink))
            Try
                DC.BeginTransaction()
                For Each item As ModuleLink In items
                    Dim FileID As Long = item.DestinationItem.ObjectLongID
                    Dim oFile As ModuleInternalFile = (From f In DC.GetCurrentSession.Linq(Of ModuleInternalFile)() Where f.Id = FileID).FirstOrDefault

                    If TypeOf oFile Is ModuleLongInternalFile Then
                        oFile.ServiceOwner = item.SourceItem.ServiceCode
                        oFile.ObjectTypeID = item.SourceItem.ObjectTypeID
                        oFile.ObjectOwner = item.SourceItem.ObjectOwner
                        DC.SaveOrUpdate(oFile)
                    ElseIf TypeOf oFile Is ModuleGuidInternalFile Then
                        oFile.ServiceOwner = item.SourceItem.ServiceCode
                        oFile.ObjectTypeID = item.SourceItem.ObjectTypeID
                        oFile.ObjectOwner = item.SourceItem
                        DC.SaveOrUpdate(oFile)
                    End If
                Next
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
        End Sub

        Public Function CreateModuleLink(ByVal Source As ModuleObject, ByVal oFile As BaseCommunityFile, ByVal Permission As Integer, ByVal ModuleID As Integer) As ModuleLink
            Return New ModuleLink(Permission, ItemDefaultAction(oFile)) With {.DestinationItem = CreateModuleObject(oFile, ModuleID), .SourceItem = Source}
        End Function
        Public Function CreateModuleLink(ByVal oFile As BaseCommunityFile, ByVal Permission As Integer, ByVal ModuleID As Integer) As ModuleLink
            Return New ModuleLink(Permission, ItemDefaultAction(oFile)) With {.DestinationItem = CreateModuleObject(oFile, ModuleID)}
        End Function
        Public Function CreateModuleActionLink(ByVal oFile As BaseCommunityFile, ByVal Permission As Integer, ByVal ModuleID As Integer) As ModuleActionLink
            Return New ModuleActionLink(Permission, ItemDefaultAction(oFile)) With {.ModuleObject = CreateModuleObject(oFile, ModuleID)}
        End Function
        Public Function CreateModuleActionLink(ByVal oFile As ModuleInternalFile, ByVal Permission As Integer, ByVal ModuleID As Integer) As ModuleActionLink
            Return New ModuleActionLink(Permission, ItemDefaultAction(oFile)) With {.ModuleObject = CreateModuleObject(oFile, ModuleID), .EditEnabled = False}
        End Function
        'Public Function CreateModuleLink(ByVal Source As ModuleObject, ByVal oFile As BaseCommunityFile, ByVal Permission As Integer, ByVal Action As Integer, ByVal ModuleID As Integer) As ModuleLink
        '    Return New ModuleLink(Permission, Action) With {.DestinationItem = CreateModuleObject(oFile, ModuleID), .SourceItem = Source}
        'End Function
        'Public Function CreateModuleLink(ByVal oFile As BaseCommunityFile, ByVal Permission As Integer, ByVal Action As Integer, ByVal ModuleID As Integer) As ModuleLink
        '    Return New ModuleLink(Permission, Action) With {.DestinationItem = CreateModuleObject(oFile, ModuleID)}
        'End Function

        'Public Function CreateModuleActionLink(ByVal oFile As BaseCommunityFile, ByVal Permission As Integer, ByVal Action As Integer, ByVal ModuleID As Integer) As ModuleActionLink
        '    Return New ModuleActionLink(Permission, Action) With {.ModuleObject = CreateModuleObject(oFile, ModuleID)}
        'End Function
        'Public Function CreateModuleActionLink(ByVal oFile As ModuleInternalFile, ByVal Permission As Integer, ByVal Action As Integer, ByVal ModuleID As Integer) As ModuleActionLink
        '    Return New ModuleActionLink(Permission, Action) With {.ModuleObject = CreateModuleObject(oFile, ModuleID), .EditEnabled = False}
        'End Function

        Public Function CreateModuleObject(ByVal oFile As BaseCommunityFile, ByVal ModuleID As Integer) As ModuleObject
            Dim Item As New ModuleObject
            Item.FQN = oFile.GetType.FullName
            Item.ObjectLongID = oFile.Id
            Item.ObjectOwner = oFile
            'If oFile.isSCORM Then
            '    Item.ObjectTypeID = COL_BusinessLogic_v2.UCServices.Services_File.ObjectType.FileScorm
            'ElseIf oFile.isFile Then
            '    Item.ObjectTypeID = COL_BusinessLogic_v2.UCServices.Services_File.ObjectType.File
            'Else
            '    Item.ObjectTypeID = COL_BusinessLogic_v2.UCServices.Services_File.ObjectType.Folder
            'End If
            Item.ObjectTypeID = oFile.RepositoryItemType
            Item.ServiceCode = COL_BusinessLogic_v2.UCServices.Services_File.Codex
            Item.ServiceID = ModuleID
            If oFile.CommunityOwner Is Nothing Then
                Item.CommunityID = 0
            Else
                Item.CommunityID = oFile.CommunityOwner.Id
            End If

            Return Item
        End Function
        Private Function GetDefaultAction(type As RepositoryItemType) As CoreModuleRepository.ActionType
            Select Case type
                Case RepositoryItemType.FileStandard
                    Return CoreModuleRepository.ActionType.DownloadFile
                Case RepositoryItemType.Multimedia
                    Return CoreModuleRepository.ActionType.PlayFile
                Case RepositoryItemType.ScormPackage
                    Return CoreModuleRepository.ActionType.PlayFile
                Case RepositoryItemType.VideoStreaming
                    Return CoreModuleRepository.ActionType.PlayFile
                Case Else
                    Return CoreModuleRepository.ActionType.None
            End Select
        End Function
        Private Function ItemDefaultAction(item As BaseCommunityFile) As Integer
            If (item.RepositoryItemType = RepositoryItemType.FileStandard OrElse item.RepositoryItemType = RepositoryItemType.Folder OrElse item.RepositoryItemType = RepositoryItemType.None) Then
                Return CInt(CoreModuleRepository.ActionType.DownloadFile)
            Else
                Return CInt(CoreModuleRepository.ActionType.PlayFile)
            End If
        End Function

#Region "iLinkedNHibernateService"
        Public Function GetAllowedStandardAction(source As ModuleObject, destination As ModuleObject, idUser As Integer, idRole As Integer, idCommunity As Integer, Optional moduleUserLong As Dictionary(Of String, Long) = Nothing, Optional moduleUserString As Dictionary(Of String, String) = Nothing) As List(Of StandardActionType) Implements iLinkedService.GetAllowedStandardAction
            Dim item As BaseCommunityFile = Nothing
            Dim actions As New List(Of StandardActionType)
            If (source.ObjectTypeID = CoreModuleRepository.ObjectType.File) Then

            End If
            Return actions
        End Function

        Public Function EvaluateModuleLink(link As ModuleLink, idUser As Integer, Optional moduleUserLong As Dictionary(Of String, Long) = Nothing, Optional moduleUserString As Dictionary(Of String, String) = Nothing) As dtoEvaluation Implements iLinkedService.EvaluateModuleLink
            Dim oDto As New dtoEvaluation
            Dim oBaseFile As BaseCommunityFile = DC.GetById(Of BaseCommunityFile)(link.DestinationItem.ObjectLongID)

            If Not IsNothing(oBaseFile) Then
                Select Case (link.Action)
                    Case COL_BusinessLogic_v2.UCServices.Services_File.ActionType.CreateFolder

                    Case COL_BusinessLogic_v2.UCServices.Services_File.ActionType.DownloadFile
                        If (From f In DC.GetCurrentSession.Linq(Of FileDownloadInfo)() Where f.File Is oBaseFile AndAlso f.Downloader.Id = idUser).Any Then
                            oDto.isCompleted = True
                            oDto.Completion = 100
                            oDto.Mark = 100
                        End If
                    Case COL_BusinessLogic_v2.UCServices.Services_File.ActionType.UploadFile

                    Case COL_BusinessLogic_v2.UCServices.Services_File.ActionType.PlayFile
                        If oBaseFile.RepositoryItemType = RepositoryItemType.ScormPackage Then

                        ElseIf (From f In DC.GetCurrentSession.Linq(Of FilePlayInfo)() Where f.File Is oBaseFile AndAlso f.Owner.Id = idUser).Any Then
                            oDto.isCompleted = True
                            oDto.Completion = 100
                            oDto.Mark = 100

                        End If

                End Select
            End If

            Return oDto
        End Function


        Public Function EvaluateModuleLinks(links As List(Of ModuleLink), idUser As Integer, Optional moduleUserLong As Dictionary(Of String, Long) = Nothing, Optional moduleUserString As Dictionary(Of String, String) = Nothing) As List(Of dtoItemEvaluation(Of Long)) Implements iLinkedService.EvaluateModuleLinks
            Dim evaluations As New List(Of dtoItemEvaluation(Of Long))
            Dim baseQuery = (From l In links Where l.DestinationItem.FQN = GetType(BaseCommunityFile).FullName OrElse l.DestinationItem.FQN = GetType(CommunityFile).FullName OrElse l.DestinationItem.FQN = GetType(ModuleLongInternalFile).FullName OrElse l.DestinationItem.FQN = GetType(ModuleGuidInternalFile).FullName)
            Dim user As Person = DC.GetById(Of Person)(idUser)

            Dim idFiles As List(Of Long) = (From i In baseQuery Where i.Action = COL_BusinessLogic_v2.UCServices.Services_File.ActionType.DownloadFile Select i.DestinationItem.ObjectLongID).ToList
            Dim idDownloadedFiles As List(Of Long) = (From info In DC.GetCurrentSession.Linq(Of FileDownloadInfo)() Where info.Downloader Is user AndAlso info.File IsNot Nothing AndAlso idFiles.Contains(info.File.Id) _
                 Distinct Select info.File.Id).ToList

            evaluations.AddRange((From d In idDownloadedFiles Select New dtoItemEvaluation(Of Long) With {.Item = d, .isCompleted = True, .isPassed = True, .isStarted = True, .Completion = 100}).ToList)

            Dim linkToPlayer = (From i In baseQuery Where i.Action = COL_BusinessLogic_v2.UCServices.Services_File.ActionType.PlayFile Select i)

            If linkToPlayer.Any Then
                Dim IdfileToPlayer As List(Of Long) = (From l In linkToPlayer Select l.DestinationItem.ObjectLongID).ToList

                Dim selectedFiles As List(Of BaseCommunityFile) = (From f In DC.GetCurrentSession.Linq(Of BaseCommunityFile)() _
                                                     Where IdfileToPlayer.Contains(f.Id) AndAlso f.isFile Select f).ToList()

                Dim idScormFiles As List(Of Long) = (From f In selectedFiles _
                                                     Where (f.RepositoryItemType = RepositoryItemType.ScormPackage) Select f.Id).ToList()

                Dim idPlayerFiles As List(Of System.Guid) = (From f In selectedFiles _
                                                    Where f.RepositoryItemType <> RepositoryItemType.ScormPackage AndAlso f.RepositoryItemType <> RepositoryItemType.FileStandard _
                                                    Select f.UniqueID).ToList()
                If idScormFiles.Any Then

                End If
                If idPlayerFiles.Any Then
                    evaluations.AddRange((From pf In DC.GetCurrentSession.Linq(Of FilePlayInfo)()
                                          Where idPlayerFiles.Contains(pf.FileUniqueID)
                                          Select New dtoItemEvaluation(Of Long) With {.Item = pf.File.Id, .isCompleted = True, .isPassed = True, .isStarted = True, .Completion = 100}).ToList)

                End If
            End If


            Return evaluations
        End Function

        Public Function AllowActionExecution(link As ModuleLink, idUser As Integer, idCommunity As Integer, idRole As Integer, Optional moduleUserLong As Dictionary(Of String, Long) = Nothing, Optional moduleUserString As Dictionary(Of String, String) = Nothing) As Boolean Implements iLinkedService.AllowActionExecution
            Return True
        End Function

        Public Function AllowStandardAction(actionType As StandardActionType, source As ModuleObject, destination As ModuleObject, idUser As Integer, idRole As Integer, Optional moduleUserLong As Dictionary(Of String, Long) = Nothing, Optional moduleUserString As Dictionary(Of String, String) = Nothing) As Boolean Implements iLinkedService.AllowStandardAction
            Return True
        End Function
        Public Function GetObjectItemFilesForStatistics(objectId As Long, objectTypeId As Integer, translations As Dictionary(Of Integer, String), idCommunity As Integer, idUser As Integer, Optional moduleUserLong As Dictionary(Of String, Long) = Nothing, Optional moduleUserString As Dictionary(Of String, String) = Nothing) As StatTreeNode(Of StatFileTreeLeaf) Implements iLinkedService.GetObjectItemFilesForStatistics
            Return Nothing
        End Function


        Public Sub PhisicalDeleteRepositoryItem(idFileItem As Long, idCommunity As Integer, idUser As Integer, Optional moduleUserLong As Dictionary(Of String, Long) = Nothing, Optional moduleUserString As Dictionary(Of String, String) = Nothing) Implements iLinkedService.PhisicalDeleteRepositoryItem

        End Sub
        Public Sub SaveActionExecution(link As ModuleLink, isStarted As Boolean, isPassed As Boolean, Completion As Short, isCompleted As Boolean, mark As Short, idUser As Integer, alreadyCompleted As Boolean, Optional moduleUserLong As Dictionary(Of String, Long) = Nothing, Optional moduleUserString As Dictionary(Of String, String) = Nothing) Implements iLinkedService.SaveActionExecution

        End Sub
        Public Sub SaveActionsExecution(evaluatedLinks As List(Of dtoItemEvaluation(Of ModuleLink)), idUser As Integer, Optional moduleUserLong As Dictionary(Of String, Long) = Nothing, Optional moduleUserString As Dictionary(Of String, String) = Nothing) Implements iLinkedService.SaveActionsExecution

        End Sub
#End Region

        Public Function GetSettings(ByVal idPerson As Integer, ByVal IdCommunity As Integer) As lm.Comol.Core.DomainModel.Repository.RepositorySettings
            Dim settings As New lm.Comol.Core.DomainModel.Repository.RepositorySettings
            Try
                DC.BeginTransaction()
                Dim person As Person = DC.GetById(Of Person)(idPerson)
                Dim isPortal As Boolean = False
                Dim community As Community = DC.GetById(Of Community)(IdCommunity)
                If IdCommunity = 0 Then
                    isPortal = True
                End If
                If Not IsNothing(person) AndAlso (IdCommunity = 0 OrElse (IdCommunity > 0 AndAlso Not IsNothing(community))) Then
                    settings = (From r In DC.GetCurrentSession.Linq(Of lm.Comol.Core.DomainModel.Repository.RepositorySettings)() _
                                                                                               Where (r.Person Is person) AndAlso isPortal = r.isPortal AndAlso (isPortal OrElse (r.Community Is community)) Select r).Skip(0).Take(1).ToList.FirstOrDefault()
                    If IsNothing(settings) Then
                        settings = New lm.Comol.Core.DomainModel.Repository.RepositorySettings With {.Community = community, .Person = person, .isPortal = isPortal}
                    End If
                End If
                DC.Commit()

            Catch ex As Exception
                settings = New lm.Comol.Core.DomainModel.Repository.RepositorySettings
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
            Return settings
        End Function
        Public Function SaveSettings(ByVal idPerson As Integer, ByVal IdCommunity As Integer, ByVal displayDescriptions As Boolean) As Boolean
            Dim result As Boolean = False
            Try
                DC.BeginTransaction()
                Dim person As Person = DC.GetById(Of Person)(idPerson)
                Dim isPortal As Boolean = False
                Dim community As Community = DC.GetById(Of Community)(IdCommunity)
                If IdCommunity = 0 Then
                    isPortal = True
                End If
                If Not IsNothing(person) AndAlso (IdCommunity = 0 OrElse (IdCommunity > 0 AndAlso Not IsNothing(community))) Then
                    result = True
                    Dim settings As lm.Comol.Core.DomainModel.Repository.RepositorySettings = (From r In DC.GetCurrentSession.Linq(Of lm.Comol.Core.DomainModel.Repository.RepositorySettings)() _
                                                                                               Where (r.Person Is person) AndAlso isPortal = r.isPortal AndAlso (isPortal OrElse (r.Community Is community)) Select r).Skip(0).Take(1).ToList.FirstOrDefault()
                    If IsNothing(settings) Then
                        settings = New lm.Comol.Core.DomainModel.Repository.RepositorySettings With {.Community = community, .Person = person, .isPortal = isPortal}
                    End If
                    settings.DisplayDescriptions = displayDescriptions
                    DC.SaveOrUpdate(settings)
                End If
                DC.Commit()

            Catch ex As Exception
                result = False
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
            Return result
        End Function


        Public Sub PhisicalDeleteCommunity(idCommunity As Integer, idUser As Integer, baseFilePath As String, baseThumbnailPath As String) Implements iLinkedService.PhisicalDeleteCommunity

        End Sub
    End Class
End Namespace