Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic

Namespace lm.Comol.Modules.Base.Presentation
    Public Class WKworkbookListPresenter
        Inherits DomainPresenter

        Public Overloads Property CurrentManager() As ManagerWorkBook
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As ManagerWorkBook)
                _CurrentManager = value
            End Set
        End Property
        Public Overloads ReadOnly Property View() As IWKworkBookList
            Get
                Return MyBase.View
            End Get
        End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerWorkBook(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IWKworkBookList)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerWorkBook(MyBase.AppContext)
        End Sub

        Public Sub InitView()
            If Not Me.UserContext.isAnonymous Then
                Me.SetupViews()
            Else
                Me.View.AllowCreateWorkBook = IWKworkBookList.AllowCreation.None
                Me.View.NoPermissionToAccessPage()
            End If
        End Sub


        Public Function GetEditingValues() As List(Of TranslatedItem(Of Integer))
            Dim oAvailableEditing As New List(Of TranslatedItem(Of Integer))
            'Dim oWorkBook As WorkBook = Me.CurrentManager.NEW_GetWorkBook(Me.View.PreloadedWorkBookID)
            'Dim oPermission As EditingPermission = Me.GetWorkBookAvailablePermission(oWorkBook)

            'If (oPermission And EditingPermission.Owner) > 0 Then
            'oAvailableEditing.Add(New TranslatedItem(Of Integer) With {.Id = EditingSettings.OnlyAuthor, .Translation = Me.View.GetEditingTranslation(EditingSettings.OnlyAuthor)})
            'End If
            ' If (oPermission And EditingPermission.Authors) > 0 Then
            oAvailableEditing.Add(New TranslatedItem(Of Integer) With {.Id = EditingSettings.AllAuthors, .Translation = Me.View.GetEditingTranslation(EditingSettings.AllAuthors)})
            'End If
            'If (oPermission And EditingPermission.ModuleManager) > 0 Then
            oAvailableEditing.Add(New TranslatedItem(Of Integer) With {.Id = EditingSettings.OnlyWorkbooksAdministrators, .Translation = Me.View.GetEditingTranslation(EditingSettings.OnlyWorkbooksAdministrators)})
            'End If
            ' If (oPermission And EditingPermission.Responsible) > 0 Then
            oAvailableEditing.Add(New TranslatedItem(Of Integer) With {.Id = EditingSettings.OnlyWorkbookResponsible, .Translation = Me.View.GetEditingTranslation(EditingSettings.OnlyWorkbookResponsible)})
            'End If
            Return oAvailableEditing
        End Function

        Public Function GetAvailableStatus() As List(Of TranslatedItem(Of Integer))
            'Dim oWorkBook As WorkBook = Me.CurrentManager.NEW_GetWorkBook(Me.View.PreloadedWorkBookID)
            'Return Me.CurrentManager.GetTranslatedWorkBookStatusList(Me.UserContext.Language, Me.GetWorkBookAvailablePermission(oWorkBook))
            Return Me.CurrentManager.GetTranslatedWorkBookStatusList(Me.UserContext.Language)
        End Function

        Private Sub SetupViews()
            Dim oList As List(Of dtoWorkBookListView) = Me.ListAvailableViews()
            Me.View.LoadAvailableView(oList)

            If (From o In oList Select o.TypeFilter).ToList.Contains(Me.View.PreLoadedView) Then
                Me.View.CurrentView = Me.View.PreLoadedView
            End If
            If Me.View.CurrentView = WorkBookTypeFilter.None Then
                If Me.UserContext.CurrentCommunityID > 0 Then
                    Me.View.CurrentView = WorkBookTypeFilter.AssignedWorkBook
                Else
                    Me.View.CurrentView = WorkBookTypeFilter.PersonalWorkBook
                End If
            End If
            Me.SetupFilters()
            Me.SetStartPager(Me.View.CurrentView)
            Me.View.ShowListView()
            Me.LoadWorkbooks()
        End Sub
        Private Function ListAvailableViews() As List(Of dtoWorkBookListView)
            Dim oList As New List(Of dtoWorkBookListView)

            If Me.AppContext.UserContext.CurrentUser.Id > 0 Then
                Dim oPersonalContext As New WorkBookContext With {.PageIndex = 0, .PageSize = Me.View.DefaultPageSize, .CommunityFilter = WorkBookCommunityFilter.AllCommunities, .Order = WorkBookOrder.Community, .View = WorkBookTypeFilter.PersonalWorkBook}
                Dim oAssignedContext As New WorkBookContext With {.PageIndex = 0, .PageSize = Me.View.DefaultPageSize, .CommunityFilter = WorkBookCommunityFilter.AllCommunities, .Order = WorkBookOrder.Community, .View = WorkBookTypeFilter.AssignedWorkBook}
                Dim oManagedContext As New WorkBookContext With {.PageIndex = 0, .PageSize = Me.View.DefaultPageSize, .CommunityFilter = WorkBookCommunityFilter.AllCommunities, .Order = WorkBookOrder.Community, .View = WorkBookTypeFilter.ManageWorkBook}
                If Me.UserContext.CurrentCommunityID > 0 Then
                    oPersonalContext.CommunityFilter = WorkBookCommunityFilter.CurrentCommunity
                    oPersonalContext.Order = WorkBookOrder.ChangedOn
                    oAssignedContext.CommunityFilter = WorkBookCommunityFilter.CurrentCommunity
                    oAssignedContext.Order = WorkBookOrder.ChangedOn
                    oManagedContext.CommunityFilter = WorkBookCommunityFilter.CurrentCommunity
                    oManagedContext.Order = WorkBookOrder.ChangedOn
                End If

                oList.Add(New dtoWorkBookListView() With {.TypeFilter = WorkBookTypeFilter.PersonalWorkBook, .Context = oPersonalContext})
                oList.Add(New dtoWorkBookListView() With {.TypeFilter = WorkBookTypeFilter.AssignedWorkBook, .Context = oAssignedContext})

                Dim oCommunitiesPermission As IList(Of WorkBookCommunityPermission) = Me.View.CommunitiesPermission

                If Me.UserContext.CurrentCommunityID > 0 Then
                    If (From cm In oCommunitiesPermission Where cm.Permissions.AddItemsToOther OrElse cm.Permissions.Administration OrElse cm.Permissions.ListOtherWorkBooks OrElse cm.Permissions.ChangeOtherWorkBook).Count > 0 Then
                        oList.Add(New dtoWorkBookListView() With {.TypeFilter = WorkBookTypeFilter.ManageWorkBook, .Context = oManagedContext})
                    End If
                ElseIf (From cm In oCommunitiesPermission Where cm.Permissions.AddItemsToOther OrElse cm.Permissions.Administration OrElse cm.Permissions.ListOtherWorkBooks OrElse cm.Permissions.ChangeOtherWorkBook).Count > 0 Then
                    oList.Add(New dtoWorkBookListView() With {.TypeFilter = WorkBookTypeFilter.ManageWorkBook, .Context = oManagedContext})
                End If
            End If
            Return oList
        End Function
        Private Sub SetupFilters()
            Dim oFilters As New List(Of WorkBookCommunityFilter)
            Dim oOrderBy As New List(Of WorkBookOrder)
            Dim DefaultFilter As WorkBookCommunityFilter = WorkBookCommunityFilter.None
            Dim DefaultOrder As WorkBookOrder = WorkBookOrder.None

            oOrderBy.Add(WorkBookOrder.ChangedOn)
            Select Case Me.View.CurrentView
                Case WorkBookTypeFilter.PersonalWorkBook
                    If Me.UserContext.CurrentCommunityID > 0 Then : oFilters.Add(WorkBookCommunityFilter.CurrentCommunity)

                    End If
                    oFilters.Add(WorkBookCommunityFilter.Portal)
                    If Me.CurrentManager.NEW_UserHasCommunitiesWorkBook(Me.UserContext.CurrentUserID, True) Then
                        oFilters.Add(WorkBookCommunityFilter.AllCommunities)
                        oOrderBy.Add(WorkBookOrder.Community)
                        DefaultOrder = WorkBookOrder.Community
                    Else
                        DefaultOrder = WorkBookOrder.ChangedOn
                    End If

                Case WorkBookTypeFilter.ManageWorkBook
                    oFilters.Add(WorkBookCommunityFilter.AllCommunities)
                    If Me.UserContext.CurrentCommunityID > 0 Then : oFilters.Add(WorkBookCommunityFilter.CurrentCommunity)

                    End If
                    oOrderBy.Add(WorkBookOrder.Community)
                    DefaultOrder = WorkBookOrder.Community
                Case WorkBookTypeFilter.AssignedWorkBook
                    oFilters.Add(WorkBookCommunityFilter.AllCommunities)
                    If Me.UserContext.CurrentCommunityID > 0 Then : oFilters.Add(WorkBookCommunityFilter.CurrentCommunity)

                    End If

                    If Me.CurrentManager.NEW_UserHasCommunitiesWorkBook(Me.UserContext.CurrentUserID, False) Then
                        oOrderBy.Add(WorkBookOrder.Community)
                        DefaultOrder = WorkBookOrder.Community
                    Else
                        DefaultOrder = WorkBookOrder.ChangedOn
                    End If
                Case Else

            End Select

            Me.View.LoadAvailableFilters(oFilters)
            Me.View.LoadAvailableOrderBy(oOrderBy)
            If (oFilters.Contains(Me.View.PreLoadedCommunityFilter)) Then
                Me.View.CurrentCommunityFilter = Me.View.PreLoadedCommunityFilter
            ElseIf DefaultFilter <> WorkBookCommunityFilter.None AndAlso (oFilters.Contains(DefaultFilter)) Then
                Me.View.CurrentCommunityFilter = DefaultFilter
            End If
            If (oOrderBy.Contains(Me.View.PreLoadedOrder)) Then
                Me.View.CurrentOrder = Me.View.PreLoadedOrder
            ElseIf DefaultOrder <> WorkBookOrder.None AndAlso (oOrderBy.Contains(DefaultOrder)) Then
                Me.View.CurrentOrder = DefaultOrder
            End If
            Me.View.CurrentPageSize = Me.View.PreLoadedPageSize
        End Sub

        Private Sub SetStartPager(ByVal oCurrentView As WorkBookTypeFilter)
            Dim oPager As New lm.Comol.Core.DomainModel.PagerBase
            oPager.PageIndex = Me.View.PreLoadedPageIndex
            oPager.PageSize = Me.View.CurrentPageSize

            Me.View.CurrentOrder = WorkBookOrder.Community
            Me.View.AllowCreateWorkBook = IWKworkBookList.AllowCreation.None

            Dim oCommunitiesID As New List(Of Integer)
            If Me.View.CurrentCommunityFilter = WorkBookCommunityFilter.AllCommunities AndAlso oCurrentView = WorkBookTypeFilter.ManageWorkBook Then
                Dim oCommunitiesPermission As IList(Of WorkBookCommunityPermission) = Me.View.CommunitiesPermission
                oCommunitiesID = (From cm In oCommunitiesPermission Where cm.Permissions.AddItemsToOther OrElse cm.Permissions.Administration OrElse cm.Permissions.ListOtherWorkBooks OrElse cm.Permissions.ChangeOtherWorkBook Select cm.ID).ToList
            ElseIf Me.View.CurrentCommunityFilter = WorkBookCommunityFilter.CurrentCommunity Then
                oCommunitiesID.Add(Me.UserContext.CurrentCommunityID)
            End If

           oPager.Count = Me.CurrentManager.NEW_WorkBookCount(Me.UserContext.CurrentUserID, oCommunitiesID, oCurrentView, Me.View.CurrentCommunityFilter, ObjectStatus.All)
            oPager.Count -= 1
            Me.View.CurrentPager = oPager
        End Sub

        Public Sub LoadWorkbooks()
            Dim oCommunitiesID As New List(Of Integer)
            If Me.View.CurrentCommunityFilter = WorkBookCommunityFilter.AllCommunities AndAlso Me.View.CurrentView = WorkBookTypeFilter.ManageWorkBook Then
                Dim oCommunitiesPermission As IList(Of WorkBookCommunityPermission) = Me.View.CommunitiesPermission
                oCommunitiesID = (From cm In oCommunitiesPermission Where cm.Permissions.AddItemsToOther OrElse cm.Permissions.Administration OrElse cm.Permissions.ListOtherWorkBooks OrElse cm.Permissions.ChangeOtherWorkBook Select cm.ID).ToList
            ElseIf Me.View.CurrentCommunityFilter = WorkBookCommunityFilter.CurrentCommunity Then
                oCommunitiesID.Add(Me.UserContext.CurrentCommunityID)
            End If

            Dim oPager As lm.Comol.Core.DomainModel.PagerBase = Me.View.CurrentPager

            oPager.PageIndex = Me.View.PreLoadedPageIndex
            If oPager.PageSize <> Me.View.CurrentPageSize Then
                oPager.PageSize = Me.View.CurrentPageSize
            End If
            'oPager.PageIndex = 
            '            

            Dim oList As List(Of dtoWorkbooks) = Me.CurrentManager.NEW_GetWorkBooksListWithPermission(Me.UserContext.CurrentUserID, Me.View.GetPortalName, Me.View.GetAllCommunitiesName, oCommunitiesID, Me.View.CurrentView, Me.View.CurrentOrder, Me.View.CurrentCommunityFilter, oPager, (From o In Me.View.CommunitiesPermission Select New ModuleCommunityPermission(Of ModuleWorkBook) With {.ID = o.ID, .Permissions = o.Permissions}).ToList, Me.UserContext.Language)


            Me.View.AllowChangeStatusEditing = ((From o In oList Where (From oItem In o.Items Where oItem.isDeleted = False AndAlso oItem.isEditable).Count > 0).Count > 0)

            If Me.UserContext.CurrentCommunityID > 0 OrElse Me.View.CurrentView = WorkBookTypeFilter.PersonalWorkBook Then
                Me.View.AllowCreateWorkBook = IWKworkBookList.AllowCreation.Current
            Else
                Me.View.AllowCreateWorkBook = IWKworkBookList.AllowCreation.ForOther
            End If

            Me.View.LoadWorkBooks(oList)
            Me.View.NavigationUrl(GetCurrentContext)
        End Sub

        Private Function GetCurrentContext() As WorkBookContext
            Dim oContext As New WorkBookContext

            oContext.CommunityFilter = Me.View.CurrentCommunityFilter
            oContext.Order = Me.View.CurrentOrder
            oContext.PageSize = Me.View.CurrentPageSize
            oContext.View = Me.View.CurrentView
            Return oContext
        End Function


        Public Sub DeleteWorkBook(ByVal WorkBookID As System.Guid, ByVal BaseUserRepositoryPath As String)
            Dim oWorkBook As WorkBook = CurrentManager.NEW_GetWorkBook(WorkBookID)
            If Not IsNothing(oWorkBook) Then

                Dim Authors As List(Of Integer) = (From au In oWorkBook.Authors Select au.Id).ToList
                Dim CommunityID As Integer = 0
                Dim isPersonal As Boolean = oWorkBook.isPersonal
                Dim Title As String = oWorkBook.Title
                If Not oWorkBook.CommunityOwner Is Nothing Then
                    CommunityID = oWorkBook.CommunityOwner.Id
                End If
                If Me.CurrentManager.NEW_DeleteWorkBook(WorkBookID, BaseUserRepositoryPath) Then
                    If isPersonal Then
                        Me.View.NotifyPersonalDelete(CommunityID, WorkBookID, Title, Me.UserContext.CurrentUser.SurnameAndName, Authors)
                    Else
                        Me.View.NotifyCommunityDelete(CommunityID, WorkBookID, Title, Me.UserContext.CurrentUser.SurnameAndName, Authors)
                    End If
                End If
            End If

            Me.SetPager()

            Me.LoadWorkBooks()
        End Sub
        Public Sub VirtualDeleteWorkBook(ByVal WorkBookID As System.Guid)
            Dim oWorkBook As WorkBook = CurrentManager.NEW_GetWorkBook(WorkBookID)
            If Not IsNothing(oWorkBook) Then
                Dim Authors As List(Of Integer) = (From au In oWorkBook.Authors Select au.Id).ToList
                Dim CommunityID As Integer = 0
                Dim isPersonal As Boolean = oWorkBook.isPersonal
                Dim Title As String = oWorkBook.Title
                If Not oWorkBook.CommunityOwner Is Nothing Then
                    CommunityID = oWorkBook.CommunityOwner.Id
                End If

                If IsNothing(Me.CurrentManager.NEW_VirtualDeleteWorkBook(WorkBookID, Me.UserContext.CurrentUserID)) Then
                    If isPersonal Then
                        Me.View.NotifyPersonalVirtualDelete(CommunityID, WorkBookID, Title, Me.UserContext.CurrentUser.SurnameAndName, Authors)
                    Else
                        Me.View.NotifyCommunityVirtualDelete(CommunityID, WorkBookID, Title, Me.UserContext.CurrentUser.SurnameAndName, Authors)
                    End If
                End If
            End If
            Me.LoadWorkbooks()
        End Sub
        Public Sub UnDeleteWorkBook(ByVal WorkBookID As System.Guid)
            Dim oWorkBook As WorkBook = CurrentManager.NEW_GetWorkBook(WorkBookID)
            If Not IsNothing(oWorkBook) Then
                Dim Authors As List(Of Integer) = (From au In oWorkBook.Authors Select au.Id).ToList
                Dim CommunityID As Integer = 0
                Dim isPersonal As Boolean = oWorkBook.isPersonal
                Dim Title As String = oWorkBook.Title
                If Not oWorkBook.CommunityOwner Is Nothing Then
                    CommunityID = oWorkBook.CommunityOwner.Id
                End If

                If Not IsNothing(Me.CurrentManager.NEW_UnDeleteVirtuaWorkBook(WorkBookID, Me.UserContext.CurrentUserID)) Then
                    If isPersonal Then
                        Me.View.NotifyPersonalVirtualUnDelete(CommunityID, WorkBookID, Title, Me.UserContext.CurrentUser.SurnameAndName, Authors)
                    Else
                        Me.View.NotifyCommunityVirtualUnDelete(CommunityID, WorkBookID, Title, Me.UserContext.CurrentUser.SurnameAndName, Authors)
                    End If
                End If
            End If
            Me.LoadWorkBooks()
        End Sub
        Public Sub UpdateWorkbooks()
            Dim oList As List(Of dtoItemStatusEditing) = Me.View.GetItemsStatusEditing
            Me.CurrentManager.SaveWorkBooksStatus((From olfo In oList Where olfo.StatusId > -1 Select New GenericItemStatus(Of System.Guid, Integer) With {.Id = olfo.ItemId, .Status = olfo.StatusId}).ToList, Me.UserContext.CurrentUserID)
            Me.CurrentManager.SaveWorkBooksEditing((From olfo In oList Where olfo.Editing <> EditingPermission.None Select New GenericItemStatus(Of System.Guid, Integer) With {.Id = olfo.ItemId, .Status = olfo.Editing}).ToList, Me.UserContext.CurrentUserID)
            Me.LoadWorkbooks()
        End Sub

        Private Sub SetPager()
            Dim oPager As lm.Comol.Core.DomainModel.PagerBase = Me.View.CurrentPager
            Dim oCurrentView As WorkBookTypeFilter = Me.View.CurrentView
            oPager.PageSize = Me.View.CurrentPageSize

            Dim oCommunitiesID As New List(Of Integer)
            If Me.View.CurrentCommunityFilter = WorkBookCommunityFilter.AllCommunities AndAlso oCurrentView = WorkBookTypeFilter.ManageWorkBook Then
                Dim oCommunitiesPermission As IList(Of WorkBookCommunityPermission) = Me.View.CommunitiesPermission
                oCommunitiesID = (From cm In oCommunitiesPermission Where cm.Permissions.AddItemsToOther OrElse cm.Permissions.Administration OrElse cm.Permissions.ListOtherWorkBooks OrElse cm.Permissions.ChangeOtherWorkBook Select cm.ID).ToList
            ElseIf Me.View.CurrentCommunityFilter = WorkBookCommunityFilter.CurrentCommunity Then
                oCommunitiesID.Add(Me.UserContext.CurrentCommunityID)
            End If

            oPager.Count = Me.CurrentManager.NEW_WorkBookCount(Me.UserContext.CurrentUserID, oCommunitiesID, oCurrentView, Me.View.CurrentCommunityFilter, ObjectStatus.All)
            oPager.Count -= 1
            Me.View.CurrentPager = oPager
        End Sub
    End Class


End Namespace