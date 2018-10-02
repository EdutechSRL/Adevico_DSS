Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic

Namespace lm.Comol.Modules.Base.Presentation
    Public Class WKItemsListPresenter
        Inherits DomainPresenter

        Private _ModuleID As Integer
        Private _CommonManager As ManagerCommon
        Private ReadOnly Property ModuleID() As Integer
            Get
                If _ModuleID <= 0 Then
                    _ModuleID = Me.CommonManager.GetModuleID(COL_BusinessLogic_v2.UCServices.Services_WorkBook.Codex)
                End If
                Return _ModuleID
            End Get
        End Property
        Private Overloads Property CommonManager() As ManagerCommon
            Get
                Return _CommonManager
            End Get
            Set(ByVal value As ManagerCommon)
                _CommonManager = value
            End Set
        End Property
        Public Overloads Property CurrentManager() As ManagerWorkBook
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As ManagerWorkBook)
                _CurrentManager = value
            End Set
        End Property
        Public Overloads ReadOnly Property View() As IWKitemsList
            Get
                Return MyBase.View
            End Get
        End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerWorkBook(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IWKitemsList)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerWorkBook(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub

        Public Sub InitView()
            Me.View.WorkBookModuleID = Me.ModuleID

            If Not Me.UserContext.isAnonymous Then
                Me.View.WorkBookCommunityID = Me.UserContext.CurrentCommunityID
                Dim oWorkBook As WorkBook = Me.CurrentManager.NEW_GetWorkBook(Me.View.PreloadedWorkBookID)
                If IsNothing(oWorkBook) Then
                    Me.View.SetWorkBookManagementItemUrl(Me.View.PreviousWorkBookView)
                    Me.View.NoWorkBookWithThisID()
                Else
                    If oWorkBook.CommunityOwner Is Nothing Then
                        Me.View.WorkBookCommunityID = 0
                    Else
                        Me.View.WorkBookCommunityID = oWorkBook.CommunityOwner.Id
                    End If
                    Dim oPermission As WorkBookPermission = GetWorkBookPermission(oWorkBook)
                    If PermissionToSee(oPermission) Then
                        Me.View.DisplayOrderAscending = Me.View.PreloadedAscending
                        Me.LoadWorkBookItems(oWorkBook)
                    Else
                        Me.View.ReturnToWorkBookManagement(Me.View.PreviousWorkBookView)
                    End If
                End If
            Else
                Me.View.WorkBookCommunityID = 0
                Me.View.NoPermissionToViewItems()
            End If
        End Sub

        Public Sub LoadWorkBookItems()
            Dim oWorkBook As WorkBook = Me.CurrentManager.NEW_GetWorkBook(Me.View.PreloadedWorkBookID)
            If IsNothing(oWorkBook) OrElse oWorkBook.Id = System.Guid.Empty Then
                Me.View.SetWorkBookManagementItemUrl(Me.View.PreviousWorkBookView)
                Me.View.NoWorkBookWithThisID()
            Else
                Me.LoadWorkBookItems(oWorkBook)
            End If
        End Sub
        Public Sub LoadWorkBookItems(ByVal oWorkBook As WorkBook)
            Dim oPermission As WorkBookPermission = GetWorkBookPermission(oWorkBook)
            Dim ModulePermission As ModuleWorkBook
            If oWorkBook.CommunityOwner Is Nothing Then : ModulePermission = ModuleWorkBook.CreatePortalmodule
            Else : ModulePermission = (From p In Me.View.CommunitiesPermission Where p.ID = oWorkBook.CommunityOwner.Id Select p.Permissions).FirstOrDefault
            End If


            Dim oList As List(Of dtoWorkBookItem) = Me.CurrentManager.GetWorkBookItemsListWithPermission(Me.UserContext.CurrentUser, oWorkBook, ModulePermission, Me.View.DisplayOrderAscending, ObjectStatus.AllbutOnlyMyDeleted, Me.UserContext.Language)


            Dim oEditingPermission = GetWorkBookAvailablePermission(oWorkBook)


            Dim NotDeletedItems As Integer = (From wki In oList Where wki.Item.isDeleted = False AndAlso ((wki.Editing And oEditingPermission) > 0) AndAlso ((wki.Item.Status.AvailableFor And oEditingPermission) > 0)).Count
            Dim DeletedItems As Integer = (From wki In oList Where wki.Item.isDeleted AndAlso ((wki.Editing And oEditingPermission) > 0) AndAlso ((wki.Item.Status.AvailableFor And oEditingPermission) > 0)).Count

            Dim AllowMultipleDelete, AllowPrint As Boolean
            AllowMultipleDelete = (oPermission.Admin OrElse oPermission.EditWorkBook OrElse oPermission.AddItems) AndAlso NotDeletedItems > 0
            AllowPrint = oPermission.ReadWorkBook AndAlso NotDeletedItems > 0

            Dim itemsEditableCount = (From o In oList Where o.Permission.ChangeApprovation OrElse o.Permission.ChangeEditing).Count
            Me.View.AllowAddItem = oPermission.AddItems OrElse oPermission.Admin
            Me.View.AllowChangeApprovation = NotDeletedItems > 0 AndAlso oPermission.ChangeApprovation AndAlso itemsEditableCount > 0
            Me.View.AllowPrint = AllowPrint
            Me.View.AllowMultipleDelete = AllowMultipleDelete
            Me.View.AllowItemsSelection = (oList.Count > 1) AndAlso (oPermission.Admin OrElse oPermission.EditWorkBook OrElse oPermission.AddItems)
            'If AllowPrint Then
            '    Me.View.SetPrintItemUrl(Me.View.PreloadedWorkBookID, Me.View.DisplayOrderAscending)
            'End If
            'If PermissionToEdit(oPermission) Then
            '    Me.View.SetAddItemUrl(Me.View.PreloadedWorkBookID)
            'End If
            'Me.View.SetWorkBookManagementItemUrl(Me.View.PreviousWorkBookView)

            Me.SetupUrl()
            Me.View.SetItemsListUrl(System.Guid.Empty, Me.View.PreviousWorkBookView)

            Me.View.LoadItems(oList)

            Dim CommunityID As Integer = 0
            If Not oWorkBook.CommunityOwner Is Nothing Then
                CommunityID = oWorkBook.CommunityOwner.Id
            End If
            Me.View.SendActionList(CommunityID, oWorkBook.Id)
        End Sub

        Public Sub VirtualDeleteItem(ByVal ItemID As System.Guid)
            Dim oItem As WorkBookItem = Me.CurrentManager.NEW_VirtualDeleteWorkBookItem(ItemID, Me.UserContext.CurrentUserID)
            VirtualDeleteUndeleteItem(oItem, True)
        End Sub
        Public Sub VirtualUnDeleteItem(ByVal ItemID As System.Guid)
            Dim oItem As WorkBookItem = Me.CurrentManager.NEW_UnDeleteVirtuaWorkBookItem(ItemID, Me.UserContext.CurrentUserID)
            VirtualDeleteUndeleteItem(oItem, False)
        End Sub
        Private Sub VirtualDeleteUndeleteItem(ByVal oItem As WorkBookItem, ByVal Delete As Boolean)
            If IsNothing(oItem) Then
                Me.View.AllowAddItem = False
                Me.View.AllowPrint = False
                Me.SetupUrl()
                Me.View.SetItemsListUrl(Me.View.PreloadedWorkBookID, Me.View.PreviousWorkBookView)
                Me.View.NoWorkBookItemWithThisID()
            Else
                Dim oWorkBook As WorkBook = oItem.WorkBookOwner
                Dim Authors As List(Of Integer) = (From au In oWorkBook.Authors Select au.Id).ToList
                Dim CommunityID As Integer = 0
                Dim isPersonal As Boolean = oWorkBook.isPersonal
                If Not oWorkBook.CommunityOwner Is Nothing Then
                    CommunityID = oWorkBook.CommunityOwner.Id
                End If
                If Delete Then
                    Me.View.SendActionVirtualDelete(CommunityID, oWorkBook.Id, oItem.Id)
                    Me.View.NotifyVirtualDelete(isPersonal, CommunityID, oItem.WorkBookOwner.Id, oWorkBook.Title, oItem.Id, oItem.Title, oItem.StartDate, Me.UserContext.CurrentUser.SurnameAndName, Authors)
                Else
                    Me.View.SendActionVirtualUnDelete(CommunityID, oWorkBook.Id, oItem.Id)
                    Me.View.NotifyVirtualUnDelete(isPersonal, CommunityID, oItem.WorkBookOwner.Id, oWorkBook.Title, oItem.Id, oItem.Title, oItem.StartDate, Me.UserContext.CurrentUser.SurnameAndName, Authors)
                End If

                Me.LoadWorkBookItems(oWorkBook)
            End If
        End Sub


        Public Function GetAvailableStatus() As List(Of TranslatedItem(Of Integer))
            Dim oWorkBook As WorkBook = Me.CurrentManager.NEW_GetWorkBook(Me.View.PreloadedWorkBookID)
            Return Me.CurrentManager.GetTranslatedWorkBookStatusList(Me.UserContext.Language, Me.GetWorkBookAvailablePermission(oWorkBook))
        End Function


        Public Sub SaveItemsStatus()
            Dim oList As List(Of dtoItemStatusEditing) = Me.View.GetItemsStatusEditing

            Me.CurrentManager.SaveItemsStatus(Me.View.PreloadedWorkBookID, (From olfo In oList Where olfo.StatusId > -1 Select New GenericItemStatus(Of System.Guid, Integer) With {.Id = olfo.ItemId, .Status = olfo.StatusId}).ToList, Me.UserContext.CurrentUserID)

            Me.CurrentManager.SaveItemsEditing(Me.View.PreloadedWorkBookID, (From olfo In oList Where olfo.Editing <> EditingPermission.None Select New GenericItemStatus(Of System.Guid, Integer) With {.Id = olfo.ItemId, .Status = olfo.Editing}).ToList, Me.UserContext.CurrentUserID)

            Dim oSelectedItems As List(Of System.Guid) = Me.View.SelectedItems
            Me.LoadWorkBookItems()
            Me.View.SelectedItems = oSelectedItems
        End Sub

        Public Sub DeleteItems(ByVal FilePath As String)
            Dim oSelectedItems As New List(Of System.Guid)
            Dim NotRemovedItems As New List(Of System.Guid)
            oSelectedItems = Me.View.SelectedItems

            For Each ItemID As System.Guid In oSelectedItems
                Dim oItem As WorkBookItem = Me.CurrentManager.GetWorkBookItem(ItemID)
                If Not IsNothing(oItem) Then
                    If oItem.isDeleted Then
                        Me.CurrentManager.NEW_DeleteWorkBookItem(ItemID, FilePath)
                    Else
                        If IsNothing(Me.CurrentManager.NEW_VirtualDeleteWorkBookItem(ItemID, Me.UserContext.CurrentUserID)) Then
                            NotRemovedItems.Add(ItemID)
                        End If
                        '            If oItem.MetaInfo.Approvation = MetaApprovationStatus.Approved Or oItem.MetaInfo.Approvation = MetaApprovationStatus.NotApproved Then
                        '                If Me.Permission.Administration Then
                        '                    If IsNothing(Me.CurrentManager.DeleteVirtualWorkBookItem(ItemID)) Then
                        '                        NotRemovedItems.Add(ItemID)
                        '                    End If
                        '                Else
                        '                    NotRemovedItems.Add(ItemID)
                        '                End If
                        '            Else
                        '                If IsNothing(Me.CurrentManager.DeleteVirtualWorkBookItem(ItemID)) Then
                        '                    NotRemovedItems.Add(ItemID)
                        '                End If
                        'End If
                    End If
                End If
            Next

            ''Me.View.LoadItems(Me.GetDiaryItems(Me.View.CurrentDiaryID, Me.View.OrderAscending))
            Me.LoadWorkBookItems()
            If NotRemovedItems.Count > 0 Then
                Me.View.SelectedItems = NotRemovedItems
            End If
        End Sub

        Public Function GetItemFiles(ByVal ItemID As System.Guid, ByVal oPermission As WorkBookItemPermission) As List(Of dtoWorkBookFile)
            Dim oFiles As List(Of dtoWorkBookFile) = Nothing
            Dim oItem As WorkBookItem = Me.CurrentManager.GetWorkBookItem(ItemID)
            Dim oFilePermission As New ModuleCommunityRepository
            If Not oItem.WorkBookOwner.CommunityOwner Is Nothing Then
                oFilePermission = Me.View.CommunityRepositoryPermission(oItem.WorkBookOwner.CommunityOwner.Id)
            End If

            oFiles = Me.CurrentManager.NEW_GetWorkBookItemDTOFiles(oItem, True, oPermission, oFilePermission)
            If oFiles Is Nothing Then
                oFiles = New List(Of dtoWorkBookFile)
            End If
            Return oFiles
        End Function

        Public Sub DeleteItem(ByVal ItemID As System.Guid, ByVal FilePath As String)
            Dim oItem As WorkBookItem = Me.CurrentManager.GetWorkBookItem(ItemID)
            Dim oWorkBook As WorkBook = oItem.WorkBookOwner
            Dim Authors As List(Of Integer) = (From au In oWorkBook.Authors Select au.Id).ToList
            Dim CommunityID As Integer = 0
            Dim isPersonal As Boolean = oWorkBook.isPersonal
            If Not oWorkBook.CommunityOwner Is Nothing Then
                CommunityID = oWorkBook.CommunityOwner.Id
            End If
            Dim Title As String = oItem.Title
            Dim StartDate As DateTime = oItem.StartDate

            If Me.CurrentManager.NEW_DeleteWorkBookItem(ItemID, FilePath) Then
                Me.View.NotifyDelete(isPersonal, CommunityID, oWorkBook.Id, oWorkBook.Title, ItemID, Title, StartDate, Me.UserContext.CurrentUser.SurnameAndName, Authors)
                Me.View.SendActionDelete(CommunityID, oWorkBook.Id, ItemID)
            End If

            Me.LoadWorkBookItems()
        End Sub

        'Private Sub DeleteItem(ByVal oItem As WorkBookItem, ByVal FilePath As String)
        '    'Dim NotRemovedItems As New List(Of System.Guid)

        '    'If Not IsNothing(oItem) Then
        '    '    If oItem.MetaInfo.Approvation = MetaApprovationStatus.Approved Or oItem.MetaInfo.Approvation = MetaApprovationStatus.NotApproved Then
        '    '        If Me.Permission.Administration Then
        '    '            If IsNothing(Me.CurrentManager.DeleteWorkBookItem(oItem.Id, FilePath)) Then
        '    '                NotRemovedItems.Add(oItem.Id)
        '    '            End If
        '    '        Else
        '    '            NotRemovedItems.Add(oItem.Id)
        '    '        End If
        '    '    Else
        '    '        If IsNothing(Me.CurrentManager.DeleteWorkBookItem(oItem.Id, FilePath)) Then
        '    '            NotRemovedItems.Add(oItem.Id)
        '    '        End If
        '    '    End If
        '    'End If
        '    'Me.LoadWorkBookItems(Me.View.CurrentWorkBookID)
        'End Sub

        Public Sub ChangeOrderBy()
            Dim oSelectedItems As List(Of System.Guid) = Me.View.SelectedItems
            'Me.View.SetRedirectToItemList(Me.View.PreloadedWorkBookID, Me.View.PreviousWorkBookView, Me.View.DisplayOrderAscending)
            Me.LoadWorkBookItems()
            Me.View.SelectedItems = oSelectedItems
        End Sub

        Private Function GetWorkBookAvailablePermission(ByVal oWorkBook As WorkBook) As EditingPermission
            Dim ModulePermission As ModuleWorkBook
            If oWorkBook.CommunityOwner Is Nothing Then : ModulePermission = ModuleWorkBook.CreatePortalmodule
            Else : ModulePermission = (From p In Me.View.CommunitiesPermission Where p.ID = oWorkBook.CommunityOwner.Id Select p.Permissions).FirstOrDefault
            End If

            Dim oEditingPermission As EditingPermission
            oEditingPermission = EditingPermission.Authors Or EditingPermission.Owner

            If ModulePermission.AddItemsToOther OrElse ModulePermission.Administration Then
                oEditingPermission = oEditingPermission Or EditingPermission.ModuleManager
            End If
            'If (From a In oWorkBook.Authors Select a.Id).Contains(UserContext.CurrentUserID) Then
            '    oEditingPermission = oEditingPermission Or EditingPermission.Authors
            'End If

            If oWorkBook.Owner.Id = UserContext.CurrentUserID Then
                oEditingPermission = oEditingPermission Or EditingPermission.Responsible
            End If
            Return oEditingPermission
        End Function

#Region "Permission"
        Private Function GetWorkBookPermission(ByVal oWorkBook As WorkBook) As WorkBookPermission
            Dim ModulePermission As ModuleWorkBook
            If oWorkBook.CommunityOwner Is Nothing Then : ModulePermission = ModuleWorkBook.CreatePortalmodule
            Else : ModulePermission = (From p In Me.View.CommunitiesPermission Where p.ID = oWorkBook.CommunityOwner.Id Select p.Permissions).FirstOrDefault
            End If

            Dim oPermission As WorkBookPermission = Me.CurrentManager.GetWorkBookPermission(Me.UserContext.CurrentUserID, oWorkBook, ModulePermission)
            If IsNothing(oPermission) Then
                Return New WorkBookPermission
            Else
                Return oPermission
            End If
        End Function
        Private Function PermissionToSee(ByVal oPermission As WorkBookPermission) As Boolean
            Return oPermission.AddItems OrElse oPermission.Admin OrElse oPermission.EditWorkBook OrElse oPermission.ReadWorkBook
        End Function
        Private Function PermissionToEdit(ByVal oPermission As WorkBookPermission) As Boolean
            Return oPermission.AddItems OrElse oPermission.Admin OrElse oPermission.EditWorkBook
        End Function
#End Region

        Private Sub SetupUrl()
            If Me.View.AllowAddItem Then
                Me.View.SetAddItemUrl(Me.View.PreloadedWorkBookID)
            End If
            If Me.View.AllowPrint Then
                Me.View.SetPrintItemUrl(Me.View.PreloadedWorkBookID, Me.View.DisplayOrderAscending)
            End If
            Me.View.SetWorkBookManagementItemUrl(Me.View.PreviousWorkBookView)
        End Sub

        Public Function GetEditingValues() As List(Of TranslatedItem(Of Integer))
            Dim oAvailableEditing As New List(Of TranslatedItem(Of Integer))
            Dim oWorkBook As WorkBook = Me.CurrentManager.NEW_GetWorkBook(Me.View.PreloadedWorkBookID)
            Dim oPermission As EditingPermission = Me.GetWorkBookAvailablePermission(oWorkBook)

            If (oPermission And EditingPermission.Owner) > 0 Then
                oAvailableEditing.Add(New TranslatedItem(Of Integer) With {.Id = EditingSettings.OnlyAuthor, .Translation = Me.View.GetEditingTranslation(EditingSettings.OnlyAuthor)})
            End If
            If (oPermission And EditingPermission.Authors) > 0 Then
                oAvailableEditing.Add(New TranslatedItem(Of Integer) With {.Id = EditingSettings.AllAuthors, .Translation = Me.View.GetEditingTranslation(EditingSettings.AllAuthors)})
            End If
            If (oPermission And EditingPermission.ModuleManager) > 0 Then
                oAvailableEditing.Add(New TranslatedItem(Of Integer) With {.Id = EditingSettings.OnlyWorkbooksAdministrators, .Translation = Me.View.GetEditingTranslation(EditingSettings.OnlyWorkbooksAdministrators)})
            End If
            If (oPermission And EditingPermission.Responsible) > 0 Then
                oAvailableEditing.Add(New TranslatedItem(Of Integer) With {.Id = EditingSettings.OnlyWorkbookResponsible, .Translation = Me.View.GetEditingTranslation(EditingSettings.OnlyWorkbookResponsible)})
            End If
            Return oAvailableEditing '.OrderBy(Function(c) c.Translation).ToList
        End Function

    End Class
End Namespace