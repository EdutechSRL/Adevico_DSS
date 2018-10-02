Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports COL_BusinessLogic_v2

Namespace lm.Comol.Modules.Base.Presentation
    Public Class CRimportPresenter
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

        Public Overloads ReadOnly Property View() As IviewImportItemsIntoRepository
            Get
                Return MyBase.View
            End Get
        End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IviewImportItemsIntoRepository)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub

        Public Sub InitView()
            Dim CommunityId As Integer = Me.View.PreLoadedCommunityID
            If CommunityId = 0 Then
                CommunityId = Me.UserContext.CurrentCommunityID
            End If
            If Not Me.UserContext.isAnonymous Then
                Dim oCommunity As Community = Me.CurrentManager.GetCommunity(CommunityId)
                Dim oPermission As ModuleCommunityRepository
                oPermission = Me.View.CommunityRepositoryPermission(CommunityId)

                Me.View.AllowManagement(Me.View.PreLoadedFolderID, Me.View.PreLoadedCommunityID, Me.View.PreLoadedView) = oPermission.Administration
                If oCommunity Is Nothing AndAlso Not (oPermission.Administration) Then
                    Me.View.TitleCommunity = Me.View.Portalname
                    Me.View.NoPermissionToImportItems(CommunityId)
                Else
                    Dim CommunityName As String = ""

                    If CommunityId = 0 Then
                        CommunityName = Me.View.Portalname
                    ElseIf Not oCommunity Is Nothing Then
                        CommunityName = oCommunity.Name
                    End If
                    Me.View.DestinationCommunityID = CommunityId
                    Me.View.DestinationCommunityName = CommunityName
                    Me.View.TitleCommunity = CommunityName
                    Me.View.SourceCommunityName = CommunityName
                    If oPermission.Administration Then
                        If Me.View.SelectedFolder = -1 Then
                            Dim FolderPath As String = Me.View.BaseFolder
                            Dim FolderID As Long = Me.View.PreLoadedFolderID

                            If FolderID > 0 Then
                                Dim oFolder As CommunityFile = Me.CurrentManager.GetFileItemById(FolderID)
                                If oFolder.CommunityOwner.Id = CommunityId Then
                                    FolderPath &= Me.CurrentManager.GetFolderPathName(FolderID)
                                End If
                            Else
                                FolderID = 0
                                FolderPath &= "/"
                            End If
                            Me.View.FilePath = FolderPath
                            Me.View.InitializeFolderSelector(CommunityId, FolderID, oPermission.Administration)
                        End If
                        Me.View.InitCommunitySelection(CommunityId)
                        Me.View.SendActionInit(CommunityId, Me.CommonManager.GetModuleID(UCServices.Services_File.Codex))
                    Else
                        Me.View.NoPermissionToImportItems(CommunityId)
                    End If
                End If
            Else
                Me.View.NoPermissionToAccessPage(CommunityId)
            End If
        End Sub

        Public Sub LoadSourceItems(ByVal SourceID As Integer)
            Dim oPermission As ModuleCommunityRepository
            oPermission = Me.View.CommunityRepositoryPermission(SourceID)

            Dim oCommunity As Community = Me.CurrentManager.GetCommunity(SourceID)
            Dim CommunityName As String = ""
            If SourceID = 0 Then
                CommunityName = Me.View.Portalname
            ElseIf Not oCommunity Is Nothing Then
                CommunityName = oCommunity.Name
            End If
            Me.View.SourceCommunityID = SourceID
          
            Me.View.InitializeSourceItemsSelector(SourceID, oPermission.Administration, oPermission.Administration)
        End Sub
        Public Sub SelectDestinationFolder()
          
            Me.View.ShowFoldersList()
        End Sub

        Public Sub TryToImport(ByVal FolderID As Long, ByVal StructureToImport As dtoFileFolder, ByVal RepositoryFolder As String)
            If StructureToImport.Files.Count > 0 OrElse StructureToImport.SubFolders.Count > 0 Then
                Dim oFilesToChange As New List(Of dtoFileExist(Of Long))

                If StructureToImport.SubFolders.Count = 1 AndAlso StructureToImport.Files.Count = 0 AndAlso StructureToImport.SubFolders(0).ID = 0 Then
                    oFilesToChange = Me.FindItemsToRename(FolderID, StructureToImport.SubFolders(0).Files, StructureToImport.SubFolders(0).SubFolders, New List(Of dtoFileExist(Of Long)))
                Else
                    oFilesToChange = Me.FindItemsToRename(FolderID, StructureToImport.Files, StructureToImport.SubFolders, New List(Of dtoFileExist(Of Long)))
                End If

                If oFilesToChange.Count > 0 Then
                    Me.View.LoadMultipleFileName(oFilesToChange)
                Else
                    ImportIntoCommunity(FolderID, StructureToImport, oFilesToChange, RepositoryFolder)
                End If
            Else
                Me.View.ShowFileList()
            End If
        End Sub
        Public Sub EvaluateNameChanged(ByVal FolderID As Long, ByVal StructureToImport As dtoFileFolder, ByVal RepositoryFolder As String)
            If StructureToImport.Files.Count > 0 OrElse StructureToImport.SubFolders.Count > 0 Then
                Dim oFilesToChange As New List(Of dtoFileExist(Of Long))

                If StructureToImport.SubFolders.Count = 1 AndAlso StructureToImport.Files.Count = 0 AndAlso StructureToImport.SubFolders(0).ID = 0 Then
                    oFilesToChange = Me.FindItemsToRename(FolderID, StructureToImport.SubFolders(0).Files, StructureToImport.SubFolders(0).SubFolders, Me.View.GetChangedFileName)
                Else
                    oFilesToChange = Me.FindItemsToRename(FolderID, StructureToImport.Files, StructureToImport.SubFolders, Me.View.GetChangedFileName)
                End If

                If (From item In oFilesToChange Where item.HighLight).Count > 0 Then
                    Me.View.LoadMultipleFileName(oFilesToChange)
                Else
                    ImportIntoCommunity(FolderID, StructureToImport, oFilesToChange, RepositoryFolder)
                End If
            Else
                Me.View.ShowFileList()
            End If
        End Sub

        Private Function FindItemsToRename(ByVal FolderID As Long, ByVal Files As List(Of dtoGenericFile), ByVal Folders As List(Of dtoFileFolder), ByVal RenamedItems As List(Of dtoFileExist(Of Long))) As List(Of dtoFileExist(Of Long))
            Dim oFilesToChange As New List(Of dtoFileExist(Of Long))
            Dim ItemsToEvaluate As List(Of Long) = Nothing
            ItemsToEvaluate = (From f In Files Select f.ID).ToList
            ItemsToEvaluate.AddRange((From f In Folders Select f.ID).ToList)

            Dim Items As List(Of CommunityFile) = Me.CurrentManager.GetCommunityFilesById(ItemsToEvaluate)
            Dim oCommunity As Community = Me.CurrentManager.GetCommunity(Me.View.DestinationCommunityID)


            ' Analyze folder
            Dim oDestinationFoldersNames As List(Of String) = Me.CurrentManager.FindAllFolderSubFolderNames(FolderID, oCommunity, False)
            For Each Item As CommunityFile In (From f In Items Where Not f.isFile Order By f.DisplayName Select f).ToList
                Dim FolderName As String = Item.Name
                Dim ItemID As Long = Item.Id
                Dim RenamedItem As dtoFileExist(Of Long) = (From rf In RenamedItems Where rf.Id = ItemID Select rf).FirstOrDefault

                If Not IsNothing(RenamedItem) Then
                    FolderName = RenamedItem.ProposedFileName
                    RenamedItem.Extension = Item.Extension
                End If
                Dim ProposedFolderName As String = Me.FindItemName(FolderName, "", oDestinationFoldersNames)
                If ProposedFolderName <> FolderName Then
                    Dim oDto As dtoFileExist(Of Long) = New dtoFileExist(Of Long) With {.Id = Item.Id, .Extension = -1, .ExistFileName = Item.DisplayName, .ProposedFileName = ProposedFolderName}
                    If Not IsNothing(RenamedItem) Then
                        oDto.HighLight = True
                    End If
                    oFilesToChange.Add(oDto)
                    oDestinationFoldersNames.Add(ProposedFolderName)
                ElseIf Not IsNothing(RenamedItem) Then
                    Dim oDto As dtoFileExist(Of Long) = New dtoFileExist(Of Long) With {.Id = Item.Id, .Extension = Item.Extension, .ExistFileName = Item.DisplayName, .ProposedFileName = ProposedFolderName}
                    oFilesToChange.Add(oDto)
                End If
                oDestinationFoldersNames.Add(ProposedFolderName)
            Next

            ' Analyze file
            Dim oDestinationFileNames As List(Of String) = Me.CurrentManager.FindAllFolderFileNames(FolderID, oCommunity, False)
            For Each Item As CommunityFile In (From f In Items Where f.isFile Order By f.DisplayName Select f).ToList
                Dim ItemName As String = Item.Name
                Dim ItemID As Long = Item.Id
                Dim RenamedItem As dtoFileExist(Of Long) = (From rf In RenamedItems Where rf.Id = ItemID Select rf).FirstOrDefault

                If Not IsNothing(RenamedItem) Then
                    ItemName = RenamedItem.ProposedFileName
                    RenamedItem.Extension = Item.Extension
                End If
                Dim ProposedName As String = Me.FindItemName(ItemName, Item.Extension, oDestinationFileNames)
                If ProposedName <> ItemName Then
                    Dim oDto As dtoFileExist(Of Long) = New dtoFileExist(Of Long) With {.Id = Item.Id, .Extension = Item.Extension, .ExistFileName = Item.DisplayName, .ProposedFileName = ProposedName}
                    If Not IsNothing(RenamedItem) Then
                        oDto.HighLight = True
                    End If
                    oFilesToChange.Add(oDto)
                ElseIf Not IsNothing(RenamedItem) Then
                    Dim oDto As dtoFileExist(Of Long) = New dtoFileExist(Of Long) With {.Id = Item.Id, .Extension = Item.Extension, .ExistFileName = Item.DisplayName, .ProposedFileName = ProposedName}
                    oFilesToChange.Add(oDto)
                End If
                oDestinationFileNames.Add(ProposedName & Item.Extension)
            Next
            Return oFilesToChange
        End Function
        Private Function FindItemName(ByVal ItemName As String, ByVal Extension As String, ByVal Names As List(Of String)) As String
            Dim ProposedName As String = ItemName
            Dim i As Integer = 1
            While Names.Contains(ProposedName & Extension)
                ProposedName = ItemName & " (" & i.ToString & ")"
                i += 1
            End While
            Return ProposedName
        End Function

        Private Sub ImportIntoCommunity(ByVal FolderID As Long, ByVal StructureToImport As dtoFileFolder, ByVal RenamedItems As List(Of dtoFileExist(Of Long)), ByVal RepositoryFolder As String)
            Dim CommunityID As Integer = Me.View.DestinationCommunityID
            Dim oContextImported As New dtoImportedItem()
            If CommunityID >= 0 Then
                Dim oFilesToImport As List(Of dtoGenericFile)
                Dim oFoldersToImport As List(Of dtoFileFolder)

                If StructureToImport.SubFolders.Count = 1 AndAlso StructureToImport.Files.Count = 0 AndAlso StructureToImport.SubFolders(0).ID = 0 Then
                    oFilesToImport = StructureToImport.SubFolders(0).Files
                    oFoldersToImport = StructureToImport.SubFolders(0).SubFolders
                Else
                    oFilesToImport = StructureToImport.Files
                    oFoldersToImport = StructureToImport.SubFolders
                End If

                oContextImported.FolderCount = oFoldersToImport.Count
                oContextImported.FileCount = oFilesToImport.Count
                oContextImported.FolderID = FolderID
                oContextImported.FolderName = IIf(FolderID = 0, Me.View.BaseFolder, Me.CurrentManager.GetFolderName(FolderID))

                For Each oItem In RenamedItems
                    Dim ItemID As Long = oItem.Id
                    Dim oFile As dtoGenericFile = (From f In oFilesToImport Select f Where f.ID = ItemID).FirstOrDefault
                    If Not IsNothing(oFile) Then
                        oFile.Name = oItem.ProposedFileName
                    Else
                        Dim oFolder As dtoFileFolder = (From f In oFoldersToImport Select f Where f.ID = ItemID).FirstOrDefault
                        If Not IsNothing(oFolder) Then
                            oFolder.Name = oItem.ProposedFileName
                        End If
                    End If
                Next
                Dim oContext As New FileTransferContext
                With oContext
                    .CommunityID = CommunityID
                    .DestinationFolderID = FolderID
                    .DownlodableByCommunity = True
                    .OwnerID = Me.UserContext.CurrentUserID
                    .SourceRepositoryFolder = RepositoryFolder & Me.View.SourceCommunityID & "\"
                    .DestinationRepositoryFolder = RepositoryFolder & CommunityID & "\"
                    .Visible = Me.View.VisibleToAll
                End With
             
                Me.CurrentManager.TransferItemsToFolder(oContext, oFilesToImport, oFoldersToImport)
            End If

            Me.View.SendActionImportCompleted(CommunityID, Me.CommonManager.GetModuleID(UCServices.Services_File.Codex))
            Me.View.NotifyImportedItems(CommunityID, oContextImported)
            Me.View.ReturnToFileManagement(FolderID, CommunityID, Me.View.PreLoadedView)
        End Sub

    End Class
End Namespace