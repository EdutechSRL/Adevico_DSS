Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports COL_BusinessLogic_v2
Imports System.Linq
Imports lm.Comol.Core.DataLayer.LinqExtension

Namespace lm.Comol.Modules.Base.Presentation
    Public Class CRmanagementPresenter
        Inherits DomainPresenter

        Private _CommonManager As ManagerCommon
        Private _ModuleContext As PermissionContext
        Private _ModuleID As Integer


        Private ReadOnly Property ModuleID() As Integer
            Get
                If _ModuleID <= 0 Then
                    _ModuleID = Me.CommonManager.GetModuleID(UCServices.Services_File.Codex)
                End If
                Return _ModuleID
            End Get
        End Property
        Private ReadOnly Property ModuleContext(ByVal CommunityId As Integer) As PermissionContext
            Get
                If _ModuleContext Is Nothing Then
                    _ModuleContext = New PermissionContext
                    _ModuleContext.Admin = HasModulePermissionToAdmin(CommunityId)
                    _ModuleContext.Upload = HasModulePermissionToUpload(CommunityId)
                    _ModuleContext.List = HasModulePermissionToSee(CommunityId)
                End If
                Return _ModuleContext
            End Get
        End Property
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

        Public Overloads ReadOnly Property View() As IViewManagementCommunityRepository
            Get
                Return MyBase.View
            End Get
        End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewManagementCommunityRepository)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub

        Public Sub InitView()
            Dim CommunityId As Integer = Me.View.PreLoadedCommunityID
            If CommunityId = 0 Then
                CommunityId = Me.UserContext.CurrentCommunityID
            End If
            Me.View.RepositoryModuleID = Me.ModuleID
            Me.View.RepositoryCommunityID = CommunityId
            If Not Me.UserContext.isAnonymous Then
                Dim oCommunity As Community = Me.CurrentManager.GetCommunity(CommunityId)
                If oCommunity Is Nothing AndAlso Not HasModulePermissionToSee(CommunityId) Then
                    Me.View.TitleCommunity = Me.View.Portalname
                    Me.View.NoPermission(CommunityId)
                Else
                    Dim CommunityName As String = ""
                    Dim FolderID As Long = Me.View.PreLoadedFolder
                    Dim oPermission As ModuleCommunityRepository

                    If FolderID > 0 Then
                        Dim oFolder As CommunityFile = Me.CurrentManager.GetFileItemById(Me.View.PreLoadedFolder)


                        If oFolder Is Nothing Then
                            FolderID = 0
                            If CommunityId = 0 Then
                                CommunityName = Me.View.Portalname
                            ElseIf Not oCommunity Is Nothing Then
                                CommunityName = oCommunity.Name
                            End If
                        Else
                            If oFolder.CommunityOwner Is Nothing Then
                                CommunityId = 0
                                CommunityName = Me.View.Portalname
                            Else
                                CommunityId = oFolder.CommunityOwner.Id
                                CommunityName = oFolder.CommunityOwner.Name
                            End If
                        End If
                        oPermission = Me.View.CommunityRepositoryPermission(CommunityId)
                        If Not Me.CurrentManager.HasPermissionToSeeItem(FolderID, oPermission.Administration, oPermission.Administration, Me.UserContext.CurrentUserID) Then
                            FolderID = 0
                        End If
                    Else
                        FolderID = 0
                        oPermission = Me.View.CommunityRepositoryPermission(CommunityId)
                        If CommunityId = 0 Then
                            CommunityName = Me.View.Portalname
                        ElseIf Not oCommunity Is Nothing Then
                            CommunityName = oCommunity.Name
                        End If
                    End If
                    Me.View.TitleCommunity = CommunityName
                    Me.View.AllowMultipleDelete(FolderID, CommunityId, 0) = ModuleContext(CommunityId).Admin
                    Me.View.AllowImport(FolderID, CommunityId, 0) = ModuleContext(CommunityId).Admin OrElse ModuleContext(CommunityId).Upload
                    Me.View.AllowDownload(FolderID, CommunityId, 0) = ModuleContext(CommunityId).List
                    'Me.View.AllowAddItem(FolderID, CommunityId) = ModuleContext(CommunityId).Upload
                    'Me.View.AllowHideItems = ModuleContext(CommunityId).Admin
                    Me.View.Ascending = True ' False
                    Me.View.OrderBy = CommunityFileOrder.Name ' CommunityFileOrder.DateUpload
                    Me.View.CurrentView = Me.View.PreLoadedView
                    Me.View.RepositoryCommunityID = CommunityId

                    Dim settings As lm.Comol.Core.DomainModel.Repository.RepositorySettings = CurrentManager.GetSettings(Me.UserContext.CurrentUserID, CommunityId)
                    Me.View.ShowDescription = settings.DisplayDescriptions

                    Me.View.LoadFolderSelector(-1, FolderID, CommunityId, oPermission.Administration, oPermission.Administration)
                    Me.View.LoadFolder(CommunityId, FolderID, ModuleContext(CommunityId).Admin, ModuleContext(CommunityId).Admin)

                    'Dim ExludeFolderID As Long = -1
                    'If oItem.FolderId = 0 Then
                    '    Me.View.ItemPath = Me.View.BaseFolder & "/"
                    'Else
                    '    If Not oItem.isFile Then
                    '        ExludeFolderID = oItem.Id
                    '    End If
                    '    Me.View.ItemPath = Me.View.BaseFolder & Me.CurrentManager.GetFolderPathName(oItem.FolderId)
                    'End If
                    'Me.View.LoadFolderSelector(ExludeFolderID, oItem.FolderId, RepositoryCommunityID, oPermission.Administration, oPermission.Administration)
                End If
            Else
                Me.View.NoPermission(CommunityId)
            End If
        End Sub
        Public Sub SaveDescriptionSettings(ByVal showDescription As Boolean, ByVal idFolder As Long)
            Me.CurrentManager.SaveSettings(UserContext.CurrentUserID, Me.View.RepositoryCommunityID, showDescription)
            Me.RetrieveFolderContent(idFolder)
        End Sub

        Public Sub RetrieveFolderContent(ByVal FolderID As Long)
            Dim oCommunityFiles As New List(Of CommunityFile)
            Dim CommunityId As Integer = Me.View.PreLoadedCommunityID
            If CommunityId = 0 Then
                CommunityId = Me.UserContext.CurrentCommunityID
            End If
            Me.SetStartPager(FolderID, CommunityId, ModuleContext(CommunityId).Admin)
            oCommunityFiles = Me.CurrentManager.GetFolderContent(CommunityId, FolderID, ModuleContext(CommunityId).Admin, ModuleContext(CommunityId).Admin, Me.UserContext.CurrentUserID)

            Dim oList As New List(Of dtoCommunityItemRepository)

            Dim oOrder As CommunityFileOrder = Me.View.OrderBy
            Dim Ascending As Boolean = Me.View.Ascending
            Dim oPermission As ModuleCommunityRepository = Me.View.CommunityRepositoryPermission(CommunityId)
            Dim Query = (From f In oCommunityFiles Select New dtoCommunityItemRepository(f, oPermission, Me.UserContext.CurrentUserID, Me.CurrentManager.HasCommunityAssignment(f.Id, False)))
            If Ascending AndAlso oOrder = CommunityFileOrder.Name Then
                Query = Query.OrderBy(Function(f) f.File.Name).ThenBy(Function(f) f.File.Extension).ThenByDescending(Function(f) f.File.CreatedOn)
            ElseIf Ascending AndAlso oOrder = CommunityFileOrder.DateUpload Then
                Query = Query.OrderBy(Function(f) f.File.CreatedOn).ThenBy(Function(f) f.File.Name).ThenBy(Function(f) f.File.Extension)
            ElseIf Not Ascending AndAlso oOrder = CommunityFileOrder.Name Then
                Query = Query.OrderByDescending(Function(f) f.File.Name).ThenByDescending(Function(f) f.File.Extension).OrderBy(Function(f) f.File.CreatedOn)
            ElseIf Not Ascending AndAlso oOrder = CommunityFileOrder.DateUpload Then
                Query = Query.OrderByDescending(Function(f) f.File.CreatedOn).ThenBy(Function(f) f.File.Name).ThenBy(Function(f) f.File.Extension)
            End If
            oList = (From f In Query Where f.File.isFile = False Select f).ToList
            oList.AddRange((From f In Query Where f.File.isFile Select f).ToList)

            'oList = (From f In oCommunityFiles Where f.isFile = False Order By f.Name, f.CreatedOn Descending Select New dtoCommunityItemRepository(f)).ToList
            'oList.AddRange((From f In oCommunityFiles Where f.isFile Order By f.Name, f.CreatedOn Descending Select New dtoCommunityItemRepository(f)).ToList)

            Dim oFolder As CommunityFile = Me.CurrentManager.GetFolderFatherOf(FolderID)
            If IsNothing(oFolder) AndAlso FolderID > 0 Then
                oFolder = New CommunityFile() With {.Name = "", .RepositoryItemType = Repository.RepositoryItemType.Folder}
            End If
            Dim oFolderUp As dtoCommunityItemRepository = Nothing
            If Not IsNothing(oFolder) Then
                oFolderUp = New dtoCommunityItemRepository(oFolder, oPermission, Me.UserContext.CurrentUserID, False)
                oFolderUp.Virtual = True
                oFolderUp.DisplayName = ".."
                oList.Insert(0, oFolderUp)
            End If

            Dim oPager As PagerBase = Me.View.CurrentPager
            If Not oPager Is Nothing Then
                Me.View.LoadFolderContent(oList.Skip(oPager.Skip).Take(oPager.PageSize).ToList)
            Else
                Me.View.LoadFolderContent(oList)
            End If
            Me.View.RepositoryFolderID = FolderID
            Me.View.AllowAddItem(FolderID, CommunityId) = ModuleContext(CommunityId).Upload
            Me.View.AllowHideItems = ModuleContext(CommunityId).Admin
            Me.View.AllowMultipleDelete(FolderID, CommunityId, Me.View.CurrentPager.PageIndex) = ModuleContext(CommunityId).Admin
            Me.View.AllowImport(FolderID, CommunityId, Me.View.CurrentPager.PageIndex) = ModuleContext(CommunityId).Admin OrElse ModuleContext(CommunityId).Upload

            Me.View.DefineUrlSelector(FolderID, CommunityId)
            Me.View.LoadRepositoryAction(CommunityId, Me.ModuleID)
        End Sub
        Public Sub UpdatePathSelector(ByVal FolderID As Long, ByVal FolderFilters As List(Of FilterElement))
            Dim oCommunityFiles As New List(Of CommunityFile)
            Dim CommunityId As Integer = Me.View.PreLoadedCommunityID
            If CommunityId = 0 Then
                CommunityId = Me.UserContext.CurrentCommunityID
            End If
            Me.View.RepositoryFolderID = FolderID
            Me.View.UpdatePathSelector(FolderFilters, CommunityId)
        End Sub
        Private Sub SetStartPager(ByVal FolderID As Long, ByVal CommunityId As Integer, ByVal ShowAllFiles As Boolean)
            Dim oPager As New lm.Comol.Core.DomainModel.PagerBase
            oPager.PageSize = Me.View.CurrentPageSize
            oPager.Count = Me.CurrentManager.GetFolderContentCount(CommunityId, FolderID, ShowAllFiles, ModuleContext(CommunityId).Admin, Me.UserContext.CurrentUserID)
            oPager.Count -= 1

            If Me.View.CurrentPager Is Nothing Then
                oPager.PageIndex = Me.View.PreLoadedPageIndex
            Else
                oPager.PageIndex = Me.View.CurrentPager.PageIndex
            End If
            Me.View.CurrentPager = oPager
        End Sub
        Private Function HasModulePermissionToSee(ByVal CommunityId As Integer) As Boolean
            Dim oPermission As ModuleCommunityRepository = Me.View.CommunityRepositoryPermission(CommunityId)
            Return (oPermission.Administration OrElse oPermission.DownLoad OrElse oPermission.ListFiles)
        End Function

        Private Function HasModulePermissionToAdmin(ByVal CommunityId As Integer) As Boolean
            Dim oPermission As ModuleCommunityRepository = Me.View.CommunityRepositoryPermission(CommunityId)
            Return (oPermission.Administration)
        End Function
        Private Function HasModulePermissionToUpload(ByVal CommunityId As Integer) As Boolean
            Dim oPermission As ModuleCommunityRepository = Me.View.CommunityRepositoryPermission(CommunityId)
            Return (oPermission.UploadFile OrElse oPermission.Administration)
        End Function

        Private Class PermissionContext
            Public Upload As Boolean
            Public Admin As Boolean
            Public List As Boolean
        End Class


        Public Sub UpdateItemVisibility(ByVal ItemID As Long)
            Dim oItem As CommunityFile = Me.CurrentManager.GetFileItemById(ItemID)
            If Not IsNothing(oItem) Then
                oItem.isVisible = Not oItem.isVisible
                If Not Me.CurrentManager.SaveItem(oItem, Me.UserContext.CurrentUserID) Is Nothing Then
                    Dim FolderName As String = Me.View.BaseFolder
                    Dim CommunityID As Integer = 0
                    If Not oItem.CommunityOwner Is Nothing Then
                        CommunityID = oItem.CommunityOwner.Id
                        If oItem.FolderId > 0 Then
                            FolderName = Me.CurrentManager.GetFolderName(oItem.FolderId)
                        End If
                    End If
                    Me.View.NotifyVisibilityChange(CommunityID, Me.ModuleID, oItem.DisplayName, oItem.Id, oItem.FolderId, FolderName, oItem.isFile, oItem.UniqueID, oItem.isVisible, oItem.RepositoryItemType)
                End If
                Me.RetrieveFolderContent(oItem.FolderId)
            End If

        End Sub
        Public Sub HideAllFiles(ByVal FolderID As Long)
            Me.CurrentManager.SaveItemsVisibility(FolderID, Me.View.RepositoryCommunityID, Me.UserContext.CurrentUserID, False)
            Me.RetrieveFolderContent(FolderID)
        End Sub
        Public Sub LoadMoveToView(ByVal ItemID As Long)
            Dim oItem As CommunityFile = Me.CurrentManager.GetFileItemById(ItemID)
            If Not IsNothing(oItem) Then
                Dim ExludeFolderID As Long = -1
                If Not oItem.isFile Then
                    ExludeFolderID = oItem.Id
                End If
                Dim oPermission As ModuleCommunityRepository
                Dim RepositoryCommunityID As Integer = 0
                If Not IsNothing(oItem.CommunityOwner) Then
                    RepositoryCommunityID = oItem.CommunityOwner.Id
                End If
                Me.View.RepositoryItemID = ItemID
                oPermission = Me.View.CommunityRepositoryPermission(RepositoryCommunityID)
                Me.View.LoadFolderSelector(ExludeFolderID, oItem.FolderId, RepositoryCommunityID, oPermission.Administration, oPermission.Administration)
            Else
                Me.RetrieveFolderContent(Me.View.RepositoryFolderID)
            End If
        End Sub
        Public Sub Verify(ByVal FolderId As Long, ByVal FolderName As String)
            Dim oItem As CommunityFile = Me.CurrentManager.GetFileItemById(Me.View.RepositoryItemID)
            Dim oFolder As CommunityFile = Me.CurrentManager.GetFileItemById(FolderId)
            If IsNothing(oItem) Then
                Me.View.ShowFolderDoesntExist(FolderName)
            ElseIf IsNothing(oFolder) AndAlso FolderId > 0 Then
                Me.View.ShowFolderDoesntExist(FolderName)
            Else
                If Me.CurrentManager.ExistInFolder(FolderId, oItem.CommunityOwner, oItem.Id, oItem.DisplayName, oItem.isFile) Then
                    Me.View.SelectedDestinationFolderID = oItem.FolderId
                    If oItem.isFile Then
                        Me.View.ShowFileNameExist(FolderName, oItem.DisplayName)
                    Else
                        Me.View.ShowFolderExist(FolderName, oItem.Name)
                    End If
                Else
                    Dim OldFolderName As String = ""
                    Dim NewFolder As String = ""
                    If oItem.FolderId = 0 Then
                        OldFolderName = Me.View.BaseFolder
                    Else
                        Dim oOldFolder As CommunityFile = Me.CurrentManager.GetFileItemById(oItem.FolderId)
                        OldFolderName = oOldFolder.Name
                    End If

                    If FolderId = 0 Then
                        NewFolder = Me.View.BaseFolder
                    Else
                        NewFolder = oFolder.Name
                    End If

                    Me.View.ShowConfirmFolder(OldFolderName, NewFolder, oItem.DisplayName)
                    'Me.View.RepositoryFolderID = FolderId
                    'Me.View.ItemFolderChanged()
                End If
            End If
        End Sub
        Public Sub MoveItem(ByVal ItemID As Long, ByVal FolderID As Long)
            Dim oItem As CommunityFile = Me.CurrentManager.GetFileItemById(ItemID)
            Dim oldFolderID As Integer = oItem.FolderId
            Dim oldFolderName As String = IIf(oldFolderID = 0, Me.View.BaseFolder, Me.CurrentManager.GetFolderName(oldFolderID))

            oItem.FolderId = FolderID
            If IsNothing(Me.CurrentManager.SaveItem(oItem, Me.UserContext.CurrentUserID)) Then
                Me.View.RepositoryFolderID = FolderID
                Me.RetrieveFolderContent(FolderID)
            Else
                Dim oPermission As ModuleCommunityRepository
                Dim RepositoryCommunityID As Integer = Me.View.RepositoryCommunityID
                oPermission = Me.View.CommunityRepositoryPermission(RepositoryCommunityID)
                Me.View.LoadFolder(RepositoryCommunityID, FolderID, oPermission.Administration, oPermission.Administration)

                Dim FolderName As String = Me.View.BaseFolder
                Dim CommunityID As Integer = 0
                If Not oItem.CommunityOwner Is Nothing Then
                    CommunityID = oItem.CommunityOwner.Id
                End If
                Dim FolderVisible As Boolean = True
                If oItem.FolderId > 0 Then
                    FolderName = Me.CurrentManager.GetFolderName(oItem.FolderId)
                    'Dim oFolder As CommunityFile = Me.CurrentManager.GetFileItemById(oItem.FolderId)
                    'If Not IsNothing(oFolder) Then
                    '    FolderVisible = oFolder.isVisible
                    'End If
                End If

                If oItem.isFile Then
                    Me.View.NotifyItemFileMoved(CommunityID, Me.ModuleID, FolderID, oItem.Id, oItem.UniqueID, oItem.DisplayName, oldFolderName, FolderName, oItem.RepositoryItemType)
                    '    Me.View.NotifyScormFileMoved(CommunityID, Me.ModuleID, FolderID, oItem.Id, oItem.UniqueID, oItem.DisplayName, oldFolderName, FolderName)
                    'ElseIf oItem.isFile Then
                    '    Me.View.NotifyFileMoved(CommunityID, Me.ModuleID, FolderID, oItem.Id, oItem.DisplayName, oldFolderName, FolderName)
                Else
                    Me.View.NotifyFolderMoved(CommunityID, Me.ModuleID, FolderID, oItem.Id, oItem.DisplayName, oldFolderName, FolderName)
                End If
            End If

        End Sub
        Public Sub DeleteItem(ByVal ItemID As Long, ByVal CommunityPath As String)
            Dim oItem As CommunityFile = Me.CurrentManager.GetFileItemById(ItemID)
            Dim idFolder As Long = Me.View.RepositoryFolderID
            If Not String.IsNullOrEmpty(CommunityPath) Then
                If Not CommunityPath.EndsWith("\") Then : CommunityPath &= "\"

                End If
                Dim FolderName As String = Me.View.BaseFolder
                Dim CommunityID As Integer = 0
                If oItem.CommunityOwner Is Nothing Then
                    CommunityPath &= "0\"
                Else
                    CommunityID = oItem.CommunityOwner.Id
                    If oItem.FolderId > 0 Then
                        FolderName = Me.CurrentManager.GetFolderName(oItem.FolderId)
                    End If
                    CommunityPath &= CommunityID & "\"
                End If
                Dim FolderID As Long = oItem.FolderId
                Dim DisplayName As String = oItem.DisplayName
                Dim type As Repository.RepositoryItemType = oItem.RepositoryItemType

                If oItem.isFile Then
                    If Me.CurrentManager.DeleteFile(oItem, CommunityPath) Then
                        Me.View.NotifyFileDeleted(CommunityID, Me.ModuleID, FolderID, ItemID, DisplayName, FolderName, type)
                    End If
                Else
                    If Me.CurrentManager.DeleteFolder(oItem, CommunityPath) Then
                        Me.View.NotifyFolderDeleted(CommunityID, Me.ModuleID, FolderID, ItemID, DisplayName, FolderName)
                    End If
                End If

                Me.View.UpdateFolderTree(CommunityID, idFolder, ModuleContext(CommunityID).Admin, ModuleContext(CommunityID).Admin)
            End If

            Me.RetrieveFolderContent(idFolder)
       
        End Sub

    End Class
End Namespace