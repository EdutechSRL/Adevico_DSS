Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports COL_BusinessLogic_v2

Namespace lm.Comol.Modules.Base.Presentation
    Public Class CommunityFolderSelectPresenter
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

        Public Overloads ReadOnly Property View() As IviewCommunityFolderSelect
            Get
                Return MyBase.View
            End Get
        End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IviewCommunityFolderSelect)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub


        Public Sub InitView(ByVal isForManagement As Boolean, ByVal CommunityID As Integer, ByVal SelectedFolderID As Long, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean, ByVal ExludeFolderID As Long) 'Optional ByVal ExludeFolderID As Long = -1)
            If Not Me.UserContext.isAnonymous Then
                Dim oCommunity As Community = Me.CurrentManager.GetCommunity(CommunityID)
                If oCommunity Is Nothing AndAlso CommunityID > 0 Then
                    Me.View.ShowNoFolderFound = True
                Else
                    Me.View.ShowNoFolderFound = False
                    If View.ForUpload Then
                        Me.LoadFoldersForUpload(oCommunity, ShowHiddenItems, AdminPurpose, ExludeFolderID)
                    Else
                        Me.LoadFolders(isForManagement, oCommunity, ShowHiddenItems, AdminPurpose, ExludeFolderID)
                    End If
                    Me.View.SelectedFolder = SelectedFolderID
                End If
            Else
                Me.View.ShowNoFolderFound = True
            End If
        End Sub

        Private Sub LoadFoldersForUpload(ByVal oCommunity As Community, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean, Optional ByVal ExludeFolderID As Long = -1)
            Dim folders As New List(Of CommunityFile)
            Dim repository As New dtoFileFolder(0, "", False)



            Dim oModule As CoreModuleRepository
            If IsNothing(oCommunity) Then
                oModule = CoreModuleRepository.CreatePortalmodule(Me.UserContext.UserTypeID)
            Else
                oModule = New CoreModuleRepository(CommonManager.GetModulePermission(Me.UserContext.CurrentUserID, oCommunity.Id, CurrentManager.ModuleId))
            End If
            repository.Selectable = Not (ExludeFolderID = 0) AndAlso (oModule.Administration OrElse oModule.UploadFile)

            folders = Me.CurrentManager.GetFoldersForUpload(oCommunity, ExludeFolderID, ShowHiddenItems, AdminPurpose, Me.UserContext.CurrentUser)
            For Each folder As CommunityFile In (From f In folders Where f.FolderId = 0 Order By f.Name Select f).ToList
                If folder.Id <> ExludeFolderID Then
                    Dim oSubfolder As New dtoFileFolder(folder.Id, folder.Name, folder.isVisible) With {.Selectable = (folder.AllowUpload OrElse oModule.UploadFile OrElse oModule.Administration)}

                    Me.RecursivelyCreateFolderForUpload(oModule, folders, oSubfolder, oCommunity, ShowHiddenItems, AdminPurpose, ExludeFolderID)
                    repository.SubFolders.Add(oSubfolder)
                End If
            Next
            Me.View.LoadTree(repository, AdminPurpose)
        End Sub

        Private Sub RecursivelyCreateFolderForUpload(ByVal oModule As CoreModuleRepository, ByVal folders As List(Of CommunityFile), ByVal dtoFather As dtoFileFolder, ByVal oCommunity As Community, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean, Optional ByVal ExludeFolderID As Long = -1)
            Dim subFolders As List(Of CommunityFile) = (From f In folders Where f.FolderId = dtoFather.ID Order By f.Name Select f).ToList()

            For Each folder As CommunityFile In subFolders
                If folder.Id <> ExludeFolderID Then
                    Dim oSubfolder As New dtoFileFolder(folder.Id, folder.Name, folder.isVisible) With {.Selectable = (folder.AllowUpload OrElse oModule.UploadFile OrElse oModule.Administration)}

                    Me.RecursivelyCreateFolderForUpload(oModule, folders, oSubfolder, oCommunity, ShowHiddenItems, AdminPurpose)

                    dtoFather.SubFolders.Add(oSubfolder)
                End If
            Next
        End Sub

        Private Sub LoadFolders(ByVal isForManagement As Boolean, ByVal oCommunity As Community, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean, Optional ByVal ExludeFolderID As Long = -1)
            Dim oCommunityFiles As New List(Of CommunityFile)
            Dim oCommuntyRepository As New dtoFileFolder(0, "", True) With {.Selectable = True}


            oCommunityFiles = Me.CurrentManager.GetFolders(oCommunity, 0, ShowHiddenItems, AdminPurpose, Me.UserContext.CurrentUser)

            For Each oCommunityFolder As CommunityFile In (From f In oCommunityFiles Order By f.Name Select f).ToList
                If oCommunityFolder.Id <> ExludeFolderID Then
                    Dim oSubfolder As New dtoFileFolder(oCommunityFolder.Id, oCommunityFolder.Name, oCommunityFolder.isVisible) With {.Selectable = True}

                    Me.RecursivelyCreateFolder(oSubfolder, oCommunity, ShowHiddenItems, AdminPurpose, ExludeFolderID)
                    oCommuntyRepository.SubFolders.Add(oSubfolder)
                End If
            Next
            View.LoadTree(oCommuntyRepository, isForManagement)
        End Sub

        Private Sub RecursivelyCreateFolder(ByVal dtoFather As dtoFileFolder, ByVal oCommunity As Community, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean, Optional ByVal ExludeFolderID As Long = -1)
            Dim oCommunityFiles As List(Of CommunityFile) = Me.CurrentManager.GetFolders(oCommunity, dtoFather.ID, ShowHiddenItems, AdminPurpose, Me.UserContext.CurrentUser)

            For Each oCommunityFolder As CommunityFile In (From f In oCommunityFiles Order By f.Name Select f).ToList
                If oCommunityFolder.Id <> ExludeFolderID Then
                    Dim oSubfolder As New dtoFileFolder(oCommunityFolder.Id, oCommunityFolder.Name, oCommunityFolder.isVisible) With {.Selectable = True}

                    Me.RecursivelyCreateFolder(oSubfolder, oCommunity, ShowHiddenItems, AdminPurpose)

                    dtoFather.SubFolders.Add(oSubfolder)
                End If
            Next
        End Sub
    End Class
End Namespace