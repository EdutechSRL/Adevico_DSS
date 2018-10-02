Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports COL_BusinessLogic_v2
Imports System.Linq
Imports lm.Comol.Core.DataLayer.LinqExtension

Namespace lm.Comol.Modules.Base.Presentation
    Public Class CRexplorePresenter
        Inherits DomainPresenter

        Private _CommonManager As ManagerCommon
        Private _ModuleContext As PermissionContext
        Private _ModuleID As Integer
        Private _ModuleRepository As New Dictionary(Of Integer, ModuleCommunityRepository)

        Private ReadOnly Property ModuleRepository(ByVal CommunityId As Integer) As ModuleCommunityRepository
            Get
                If Not _ModuleRepository.ContainsKey(CommunityId) Then
                    _ModuleRepository.Add(CommunityId, Me.View.CommunityRepositoryPermission(CommunityId))
                End If
                Return _ModuleRepository.Item(CommunityId)
            End Get
        End Property

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

        Public Overloads ReadOnly Property View() As IViewExploreCommunityRepository
            Get
                Return MyBase.View
            End Get
        End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewExploreCommunityRepository)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub

        Public Sub InitView()
            Dim CommunityId As Integer = Me.View.PreLoadedCommunityID
            If CommunityId = 0 Then
                CommunityId = Me.UserContext.CurrentCommunityID
            End If
            Me.View.RepositoryModuleID = Me.CommonManager.GetModuleID(UCServices.Services_File.Codex)
            Me.View.RepositoryCommunityID = CommunityId
            If Not Me.UserContext.isAnonymous Then
                Dim oCommunity As Community = Me.CurrentManager.GetCommunity(CommunityId)
                If oCommunity Is Nothing AndAlso Not HasModulePermissionToSee(CommunityId) Then
                    Me.View.TitleCommunity = Me.View.Portalname
                    Me.View.NoPermission(CommunityId)
                Else
                    Dim FolderID As Long = Me.View.PreLoadedFolder
                    Dim CommunityName As String = ""
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
                        Dim oPermission As ModuleCommunityRepository = Me.ModuleRepository(CommunityId)
                        If Not Me.CurrentManager.HasPermissionToSeeItem(FolderID, oPermission.Administration, oPermission.Administration, Me.UserContext.CurrentUserID) Then
                            FolderID = 0
                        End If
                    Else
                        FolderID = 0
                        If CommunityId = 0 Then
                            CommunityName = Me.View.Portalname
                        ElseIf Not oCommunity Is Nothing Then
                            CommunityName = oCommunity.Name
                        End If
                    End If
                    View.RepositoryCommunityID = CommunityId
                    View.TitleCommunity = CommunityName
                    'Me.View.AllowAdvancedManagement(CommunityId, 0) = ModuleContext(CommunityId).Admin
                    View.AllowManagement(FolderID, CommunityId, 0) = ModuleContext(CommunityId).Admin
                    '  Me.View.AllowUploadFile(FolderID, CommunityId) = Me.CurrentManager.HasPermissionToUploadIntoFolder(FolderID, Me.UserContext.CurrentUserID, Me.ModuleRepository(CommunityId))

                    View.Ascending = True 'False
                    View.OrderBy = CommunityFileOrder.Name 'CommunityFileOrder.DateUpload
                    View.CurrentView = Me.View.PreLoadedView
                    Dim settings As lm.Comol.Core.DomainModel.Repository.RepositorySettings = CurrentManager.GetSettings(Me.UserContext.CurrentUserID, CommunityId)
                    View.ShowDescription = settings.DisplayDescriptions
                    View.LoadFolder(CommunityId, FolderID, ModuleContext(CommunityId).Admin, ModuleContext(CommunityId).Admin)
                End If
            Else
                Me.View.NoPermission(CommunityId)
            End If
        End Sub
        Public Sub RetrieveFolderContent(ByVal FolderID As Long)
            Dim oCommunityFiles As New List(Of CommunityFile)
            Dim CommunityId As Integer = Me.View.RepositoryCommunityID
            'Dim CommunityId As Integer = Me.View.PreLoadedCommunityID
            'If CommunityId = 0 Then
            '    CommunityId = Me.UserContext.CurrentCommunityID
            'End If
            Me.SetStartPager(FolderID, CommunityId, ModuleContext(CommunityId).Admin)
            oCommunityFiles = Me.CurrentManager.GetFolderContent(CommunityId, FolderID, ModuleContext(CommunityId).Admin, ModuleContext(CommunityId).Admin, Me.UserContext.CurrentUserID)

            Dim oList As New List(Of dtoCommunityItemRepository)

            Dim oOrder As CommunityFileOrder = Me.View.OrderBy
            Dim Ascending As Boolean = Me.View.Ascending
            Dim oPermission As ModuleCommunityRepository = Me.ModuleRepository(CommunityId)
            Dim Query = (From f In oCommunityFiles Select New dtoCommunityItemRepository(f, oPermission, Me.UserContext.CurrentUserID, Me.CurrentManager.HasCommunityAssignment(f.Id, False)))
            If Ascending AndAlso oOrder = CommunityFileOrder.Name Then
                Query = Query.OrderBy(Function(f) f.File.Name).ThenBy(Function(f) f.File.Extension).ThenByDescending(Function(f) f.File.CreatedOn)
            ElseIf Ascending AndAlso oOrder = CommunityFileOrder.DateUpload Then
                Query = Query.OrderBy(Function(f) f.File.CreatedOn).ThenBy(Function(f) f.File.Name).ThenBy(Function(f) f.File.Extension)
            ElseIf Not Ascending AndAlso oOrder = CommunityFileOrder.Name Then
                Query = Query.OrderByDescending(Function(f) f.File.Name).ThenByDescending(Function(f) f.File.Extension).ThenBy(Function(f) f.File.CreatedOn)
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
            Me.View.AllowManagement(FolderID, CommunityId, 0) = ModuleContext(CommunityId).Admin
            Me.View.AllowUploadFile(FolderID, CommunityId) = Me.CurrentManager.HasPermissionToUploadIntoFolder(FolderID, Me.UserContext.CurrentUserID, oPermission)

            ' ModuleContext(CommunityId).Upload
            Me.View.DefineUrlSelector(FolderID, CommunityId)
            Me.View.LoadRepositoryAction(CommunityId, Me.ModuleID)
        End Sub
        Public Sub SaveDescriptionSettings(ByVal showDescription As Boolean, ByVal idFolder As Long)
            View.ShowDescription = showDescription
            Me.CurrentManager.SaveSettings(UserContext.CurrentUserID, Me.View.RepositoryCommunityID, showDescription)
            Me.RetrieveFolderContent(idFolder)
        End Sub
        Public Sub UpdatePathSelector(ByVal FolderID As Long, ByVal FolderFilters As List(Of FilterElement))
            Dim oCommunityFiles As New List(Of CommunityFile)
            Dim CommunityId As Integer = Me.View.PreLoadedCommunityID
            If CommunityId = 0 Then
                CommunityId = Me.UserContext.CurrentCommunityID
            End If
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
        'Private Sub LoadFiles(ByVal oCommunity As Community, ByVal SelectedFiles As List(Of Long), ByVal ShowAllFiles As Boolean)
        '    Dim oCommunityFiles As New List(Of CommunityFile)
        '    Dim oCommuntyRepository As New dtoFileFolder(0, "", True)


        '    oCommunityFiles = Me.CurrentManager.GetFiles(oCommunity, 0, ShowAllFiles, Me.UserContext.CurrentUser)

        '    For Each oCommunityFolder As CommunityFile In (From folder In oCommunityFiles Where folder.isFile = False Select folder).ToList
        '        Dim oSubfolder As New dtoFileFolder(oCommunityFolder.Id, oCommunityFolder.Name, oCommunityFolder.isVisibile)

        '        Me.RecursivelyCreateFolder(oSubfolder, oCommunity, SelectedFiles, ShowAllFiles)

        '        oCommuntyRepository.SubFolders.Add(oSubfolder)
        '    Next
        '    oCommuntyRepository.Files = (From file In oCommunityFiles Where file.isFile Select New dtoCommunityFile(file.Id, file.Name, file.isVisibile, SelectedFiles.Contains(file.Id))).ToList
        '    Me.View.LoadTree(oCommuntyRepository)
        'End Sub

        Private Function HasModulePermissionToSee(ByVal CommunityId As Integer) As Boolean
            Dim oPermission As ModuleCommunityRepository = Me.ModuleRepository(CommunityId)
            Return (oPermission.Administration OrElse oPermission.DownLoad OrElse oPermission.ListFiles)
        End Function

        Private Function HasModulePermissionToAdmin(ByVal CommunityId As Integer) As Boolean
            Dim oPermission As ModuleCommunityRepository = Me.ModuleRepository(CommunityId)
            Return (oPermission.Administration)
        End Function
        Private Function HasModulePermissionToUpload(ByVal CommunityId As Integer) As Boolean
            Dim oPermission As ModuleCommunityRepository = Me.ModuleRepository(CommunityId)
            Return (oPermission.UploadFile OrElse oPermission.Administration)
        End Function

        Private Class PermissionContext
            Public Upload As Boolean
            Public Admin As Boolean
            Public List As Boolean
        End Class
    End Class
End Namespace