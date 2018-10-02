Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports COL_BusinessLogic_v2

Namespace lm.Comol.Modules.Base.Presentation
    Public Class WorkBookItemPresenter
        Inherits DomainPresenter

        Public Overloads Property CurrentManager() As ManagerWorkBook
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As ManagerWorkBook)
                _CurrentManager = value
            End Set
        End Property
        Public Overloads ReadOnly Property View() As IviewWorkBookItem
            Get
                Return MyBase.View
            End Get
        End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerWorkBook(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IviewWorkBookItem)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerWorkBook(MyBase.AppContext)
        End Sub

        Public Sub InitView()
            If Not Me.UserContext.isAnonymous Then
                Dim oWorkBook As WorkBook = Me.CurrentManager.GetWorkBook(Me.View.PreloadedWorkBookID)
                If IsNothing(oWorkBook) OrElse (Not Me.View.PreloadedIsInsertMode AndAlso Me.View.PreloadedItemID = System.Guid.Empty) Then
                    Me.View.NoPermission(Me.UserContext.CurrentCommunityID)
                Else
                    Dim oItem As WorkBookItem = Me.CurrentManager.GetWorkBookItem(Me.View.PreloadedItemID)
                    If IsNothing(oItem) AndAlso Not Me.View.PreloadedIsInsertMode Then
                        Me.View.NoItemWithThisID(Me.UserContext.CurrentCommunityID)
                    Else
                        Dim ModulePermission As ModuleWorkBook
                        If oWorkBook.CommunityOwner Is Nothing Then : ModulePermission = ModuleWorkBook.CreatePortalmodule
                        Else : ModulePermission = (From p In Me.View.CommunitiesPermission Where p.ID = oWorkBook.CommunityOwner.Id Select p.Permissions).FirstOrDefault
                        End If
                        Dim oEditingPermission As EditingPermission = Me.CurrentManager.GetItemAvailablePermission(oWorkBook, ModulePermission)

                        If Me.View.PreloadedItemID = System.Guid.Empty Then
                            oItem = New WorkBookItem
                            oItem.Title = ""
                            oItem.Note = ""
                            oItem.isDraft = False
                            oItem.Body = ""
                            oItem.Owner = Me.UserContext.CurrentUser
                            oItem.CreatedBy = Me.UserContext.CurrentUser
                            oItem.CreatedOn = Now
                            oItem.StartDate = Now.Date
                            oItem.Status = Me.CurrentManager.GetDefaultWorkBookStatus()
                            If oWorkBook.Owner.Id = Me.UserContext.CurrentUserID And (oEditingPermission And EditingPermission.Responsible) > 0 Then
                                oItem.Editing = EditingSettings.OnlyWorkbookResponsible
                            ElseIf (oEditingPermission And EditingPermission.Authors) > 0 Then
                                oItem.Editing = EditingSettings.AllAuthors
                            Else
                                oItem.Editing = EditingSettings.OnlyAuthor
                            End If
                        End If

                        Dim oWKPermission As WorkBookPermission = GetWorkBookPermission(oWorkBook)
                        Dim oFilePermission As New ModuleCommunityRepository

                        If Not IsNothing(oWorkBook.CommunityOwner) Then
                            oFilePermission = Me.View.CommunityRepositoryPermission(oWorkBook.CommunityOwner.Id)
                        End If

                        Dim oItemPermission As WorkBookItemPermission = Nothing
                        If Me.View.PreloadedItemID <> System.Guid.Empty Then
                            oItemPermission = GetWorkBookItemPermission(oItem)
                            If oItemPermission.Write OrElse oItemPermission.Read Then
                                Me.View.AllowEdit = oItemPermission.Write
                                Me.View.AllowFileManagement = oItemPermission.Write
                                Dim CommunityID As Integer = 0
                                If Not oWorkBook.CommunityOwner Is Nothing Then
                                    CommunityID = oWorkBook.CommunityOwner.Id
                                End If

                                Me.View.LoadFilesToManage(oItem.Id, CommunityID, oItemPermission.Write, oItemPermission, oFilePermission, Me.View.AllowPublish)
                            Else
                                Me.View.AllowEdit = False
                                Me.View.AllowFileManagement = False
                            End If
                            Me.View.AllowStatusChange = oItemPermission.ChangeApprovation
                        Else
                            Me.View.AllowStatusChange = oWKPermission.ChangeApprovation
                            Me.View.AllowEdit = oWKPermission.AddItems
                            Me.View.AllowFileManagement = oWKPermission.AddItems
                            ' Me.View.LoadFilesToManage(oItem.Id, oWKPermission.AddItems, oItemPermission, oFilePermission)
                        End If


                        'If Me.View.PreloadedItemID = System.Guid.Empty Then
                        '    oEditingPermission = GetNewItemEditingPermission(oWorkBook)
                        'Else
                        '    oEditingPermission = GetItemEditingPermission(oWorkBook, oItemPermission)
                        'End If
                        Dim oAvailableStatus As List(Of TranslatedItem(Of Integer)) = Me.CurrentManager.GetTranslatedWorkBookStatusList(Me.UserContext.Language, oEditingPermission)
                        If (From o In oAvailableStatus Where o.Id = oItem.Status.Id).Count = 0 Then
                            Dim oCurrentStatus As TranslatedItem(Of Integer) = Me.CurrentManager.GetTranslatedWorkBookStatus(Me.UserContext.Language, oItem.Status.Id)
                            oAvailableStatus.Add(oCurrentStatus)
                            Me.View.AllowStatusChange = False
                        End If
                        Me.LoadEditingValues(oEditingPermission, oItem)
                        'Dim oCurrentStatus As TranslatedItem(Of Integer) = Me.CurrentManager.GetTranslatedWorkBookStatus(Me.UserContext.Language, oItem.Status.Id)
                        'If Not oAvailableStatus.Contains(oCurrentStatus) Then
                        '    oAvailableStatus.Add(oCurrentStatus)
                        '    oAvailableStatus = (From o In oAvailableStatus Select o Order By o.Translation).ToList
                        'End If
                        Me.View.LoadItem(oItem, oAvailableStatus)
                    End If
                    Me.View.SetBackToWorkbookURL(Me.View.PreloadedWorkBookID, Me.View.PreviousWorkBookView)
                End If
            Else
                Me.View.NoPermission(Me.UserContext.CurrentCommunityID)
            End If
        End Sub
        Private Function GetWorkBookItemPermission(ByVal oItem As WorkBookItem) As WorkBookItemPermission
            Dim ModulePermission As ModuleWorkBook
            If oItem.WorkBookOwner.CommunityOwner Is Nothing Then : ModulePermission = ModuleWorkBook.CreatePortalmodule
            Else : ModulePermission = (From p In Me.View.CommunitiesPermission Where p.ID = oItem.WorkBookOwner.CommunityOwner.Id Select p.Permissions).FirstOrDefault
            End If

            Dim oPermission As WorkBookItemPermission = Me.CurrentManager.GetWorkBookItemPermission(Me.UserContext.CurrentUserID, oItem, ModulePermission)

            If IsNothing(oPermission) Then
                Return New WorkBookItemPermission
            Else
                Return oPermission
            End If
        End Function
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

        Public Sub ReloadManagementFileView()
            If Not Me.UserContext.isAnonymous Then
                Dim oItem As WorkBookItem = Me.CurrentManager.GetWorkBookItem(Me.View.PreloadedItemID)
                If IsNothing(oItem) AndAlso Me.View.PreloadedIsInsertMode = False Then
                    Me.View.NoItemWithThisID(Me.UserContext.CurrentCommunityID)
                ElseIf Me.View.PreloadedIsInsertMode = False Then
                    Dim oPermission As WorkBookItemPermission = GetWorkBookItemPermission(oItem)
                    If oPermission.Write OrElse oPermission.Read Then
                        Dim oFilePermission As New ModuleCommunityRepository
                        If Not oItem.WorkBookOwner.CommunityOwner Is Nothing Then
                            oFilePermission = Me.View.CommunityRepositoryPermission(oItem.WorkBookOwner.CommunityOwner.Id)
                        End If
                        Dim CommunityID As Integer = 0
                        If Not oItem.WorkBookOwner.CommunityOwner Is Nothing Then
                            CommunityID = oItem.WorkBookOwner.CommunityOwner.Id
                        End If
                        Me.View.LoadFilesToManage(oItem.Id, CommunityID, oPermission.Write, oPermission, oFilePermission, Me.View.AllowPublish)
                    Else
                        Me.View.SendToItemsList(Me.View.PreloadedWorkBookID, Me.View.PreviousWorkBookView, oItem.Id)
                    End If
                End If
            Else
                Me.View.NoPermission(Me.UserContext.CurrentCommunityID)
            End If
        End Sub

        Public Sub GoToFileManagement()
            Dim oItem As WorkBookItem = Me.SaveWorkBookItem(Me.View.CurrentItem)
            If IsNothing(oItem) Then
                Me.View.SetBackToWorkbookURL(Me.View.PreloadedWorkBookID, Me.View.PreviousWorkBookView)
            Else
                Me.View.SendToFileManagement(oItem.Id, Me.View.PreviousWorkBookView)
            End If
        End Sub
        Public Sub SaveItem()
            Dim oItem As WorkBookItem = Me.SaveWorkBookItem(Me.View.CurrentItem)
            Me.View.SendToItemsList(Me.View.PreloadedWorkBookID, Me.View.PreviousWorkBookView, oItem.Id)
        End Sub
        Private Function SaveWorkBookItem(ByVal oItem As WorkBookItem) As WorkBookItem
            Dim oSavedItem As WorkBookItem = Nothing
            Dim oPreviousItem As WorkBookItem = Nothing
            Dim NotifyEdit As Boolean = False
            If Not Me.View.PreloadedItemID = System.Guid.Empty Then
                oItem.Id = View.PreloadedItemID
                oPreviousItem = Me.CurrentManager.GetWorkBookItem(Me.View.PreloadedItemID)
                NotifyEdit = oPreviousItem.isDraft <> oItem.isDraft OrElse oPreviousItem.Note <> oItem.Note OrElse oPreviousItem.Body <> oItem.Body OrElse oPreviousItem.Title <> oItem.Title OrElse oPreviousItem.StartDate <> oItem.StartDate
                If oItem.Status Is Nothing Then
                    oItem.Status = oPreviousItem.Status
                End If
            ElseIf oItem.Status Is Nothing Then
                oItem.Status = Me.CurrentManager.GetDefaultWorkBookStatus
            End If

            oSavedItem = Me.CurrentManager.NEW_SaveWorkBookItem(Me.UserContext.CurrentUser.Id, Me.UserContext.CurrentUser.Id, Me.UserContext.CurrentCommunityID, Me.View.PreloadedWorkBookID, oItem)

            If Not IsNothing(oSavedItem) Then
                Dim oWorkBook As WorkBook = Me.CurrentManager.NEW_GetWorkBook(Me.View.PreloadedWorkBookID)
                Dim Authors As List(Of Integer) = (From au In oWorkBook.Authors Select au.Id).ToList
                Dim CommunityID As Integer = 0
                Dim isPersonal As Boolean = oSavedItem.WorkBookOwner.isPersonal
                If Not oSavedItem.WorkBookOwner.CommunityOwner Is Nothing Then
                    CommunityID = oSavedItem.WorkBookOwner.CommunityOwner.Id
                End If

                If Me.View.PreloadedItemID = System.Guid.Empty Then
                    Me.View.SendAddAction(oSavedItem.Id)
                    If oSavedItem.isDraft Then
                        Authors.Clear()
                        Authors.Add(Me.UserContext.CurrentUserID)
                    End If
                    Me.View.NotifyAdd(isPersonal, CommunityID, Me.View.PreloadedWorkBookID, oSavedItem.WorkBookOwner.Title, oSavedItem.Id, oSavedItem.Title, oSavedItem.StartDate, Me.UserContext.CurrentUser.SurnameAndName, Authors)
                Else
                    Me.View.SendEditAction(oSavedItem.Id)
                    If oSavedItem.isDraft Then
                        Authors.Clear()
                        Authors.Add(Me.UserContext.CurrentUserID)
                    End If

                    If NotifyEdit Then
                        Me.View.NotifyEdit(isPersonal, CommunityID, Me.View.PreloadedWorkBookID, oSavedItem.WorkBookOwner.Title, oSavedItem.Id, oSavedItem.Title, oSavedItem.StartDate, Me.UserContext.CurrentUser.SurnameAndName, Authors)
                    End If
                End If
            End If
            Return oSavedItem
        End Function

        Private Sub LoadEditingValues(ByVal oEditingPermission As EditingPermission, ByVal oItem As WorkBookItem)
            Dim oAvailableEditing As New List(Of TranslatedItem(Of Integer))

            Dim oItemEditing As EditingPermission = oItem.Editing

            If (oEditingPermission And EditingSettings.OnlyWorkbookResponsible) > 0 Then
                oAvailableEditing.Add(New TranslatedItem(Of Integer) With {.Id = EditingSettings.OnlyWorkbookResponsible, .Translation = Me.View.GetEditingTranslation(EditingSettings.OnlyWorkbookResponsible)})
            End If

            If (oEditingPermission And EditingSettings.OnlyAuthor) > 0 Then
                Dim oTranslation As New TranslatedItem(Of Integer) With {.Id = EditingSettings.OnlyAuthor, .Translation = Me.View.GetEditingTranslationOwner(oItem.Owner.Id = Me.UserContext.CurrentUserID, EditingSettings.OnlyAuthor)}

                If oItem.Owner.Id <> Me.UserContext.CurrentUserID Then
                    oTranslation.Translation = String.Format(oTranslation.Translation, oItem.Owner.SurnameAndName)
                End If
                oAvailableEditing.Add(oTranslation)
            End If
            If (oEditingPermission And EditingSettings.AllAuthors) > 0 Then
                oAvailableEditing.Add(New TranslatedItem(Of Integer) With {.Id = EditingSettings.AllAuthors, .Translation = Me.View.GetEditingTranslation(EditingSettings.AllAuthors)})
            End If
            If (oEditingPermission And EditingSettings.OnlyWorkbooksAdministrators) > 0 Then
                oAvailableEditing.Add(New TranslatedItem(Of Integer) With {.Id = EditingSettings.OnlyWorkbooksAdministrators, .Translation = Me.View.GetEditingTranslation(EditingSettings.OnlyWorkbooksAdministrators)})
            End If
            If (From o In oAvailableEditing Select o.Id = oItemEditing).Count = 0 Then
                oAvailableEditing.Add(New TranslatedItem(Of Integer) With {.Id = oItemEditing, .Translation = Me.View.GetEditingTranslation(oItemEditing)})
            End If
            Me.View.SetEditing(oAvailableEditing.OrderBy(Function(c) c.Translation).ToList, oItemEditing)
            Me.View.AllowEditingChanging = ((oItemEditing And oEditingPermission) > 0)
        End Sub

    End Class
End Namespace