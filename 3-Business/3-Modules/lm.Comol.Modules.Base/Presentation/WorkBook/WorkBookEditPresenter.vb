Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic

Namespace lm.Comol.Modules.Base.Presentation
    Public Class WorkBookEditPresenter
        Inherits DomainPresenter

#Region "PERMESSI"
        Private _Permission As ModuleWorkBook
        Private _CommunitiesPermission As IList(Of WorkBookCommunityPermission)
        Private ReadOnly Property Permission(Optional ByVal CommunityID As Integer = 0) As ModuleWorkBook
            Get
                If IsNothing(_Permission) AndAlso CommunityID = 0 Then
                    _Permission = Me.View.ModulePermission
                    Return _Permission
                ElseIf CommunityID > 0 Then
                    Return (From o In CommunitiesPermission Where o.ID = CommunityID Select o.Permissions).FirstOrDefault
                Else
                    Return _Permission
                End If
            End Get
        End Property
        Private ReadOnly Property CommunitiesPermission() As IList(Of WorkBookCommunityPermission)
            Get
                If IsNothing(_CommunitiesPermission) Then
                    _CommunitiesPermission = Me.View.CommunitiesPermission()
                End If
                Return _CommunitiesPermission
            End Get
        End Property
#End Region

#Region "Standard"
        Public Overloads Property CurrentManager() As ManagerWorkBook
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As ManagerWorkBook)
                _CurrentManager = value
            End Set
        End Property
        Public Overloads ReadOnly Property View() As IviewWorkBookEdit
            Get
                Return MyBase.View
            End Get
        End Property
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerWorkBook(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IviewWorkBookEdit)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerWorkBook(MyBase.AppContext)
        End Sub

        Public Sub Init()

        End Sub
#End Region

        Public Sub InitView()
            If IsNothing(Me.UserContext.CurrentUser) OrElse Me.UserContext.isAnonymous OrElse Me.UserContext.CurrentUser.Id <= 0 Then
                Me.View.ShowError(My.Resources.ModuleBaseResource.NotPermission)
            Else
                Dim oWorkBook As WorkBook = Me.CurrentManager.GetWorkBook(Me.View.CurrentWorkBookID)

                If IsNothing(oWorkBook) Then
                    Me.View.ShowError(My.Resources.ModuleBaseResource.WorkBookNotFound)
                Else
                    Dim ModulePermission As ModuleWorkBook
                    If oWorkBook.CommunityOwner Is Nothing Then : ModulePermission = ModuleWorkBook.CreatePortalmodule
                    Else : ModulePermission = (From p In Me.View.CommunitiesPermission Where p.ID = oWorkBook.CommunityOwner.Id Select p.Permissions).FirstOrDefault
                    End If
                    Dim oPermission As WorkBookPermission = Me.CurrentManager.GetWorkBookPermission(Me.UserContext.CurrentUser.Id, oWorkBook, ModulePermission)
                    Dim oAuthor As iPerson = Me.CurrentManager.GetPerson(Me.UserContext.CurrentUser.Id)
                    Me.View.AllowShowItems = oPermission.AddItems
                    Me.View.AllowAddAuthors = (oWorkBook.Type <> WorkBookType.Personal AndAlso oWorkBook.Type <> WorkBookType.PersonalCommunity AndAlso oWorkBook.Type <> WorkBookType.Community) AndAlso (oPermission.Admin OrElse oWorkBook.Owner.Equals(oAuthor))

                    Me.View.AllowManagementAuthors = (oPermission.Admin OrElse oPermission.EditWorkBook OrElse oWorkBook.Owner.Equals(oAuthor) OrElse oWorkBook.Authors.Contains(oAuthor)) AndAlso (oWorkBook.Type <> WorkBookType.Personal AndAlso oWorkBook.Type <> WorkBookType.PersonalCommunity AndAlso oWorkBook.Type <> WorkBookType.Community)

                    Me.View.AllowSelectOwner = (oWorkBook.Type <> WorkBookType.Personal AndAlso oWorkBook.Type <> WorkBookType.PersonalCommunity AndAlso oWorkBook.Type <> WorkBookType.Community) AndAlso oPermission.Admin

                    If oPermission.ReadWorkBook Then
                        Me.ShowWorkBook(oWorkBook, oPermission, ModulePermission)
                    Else
                        Me.View.ShowError(My.Resources.ModuleBaseResource.NotPermission)
                    End If
                End If
            End If
        End Sub
        Private Sub ShowWorkBook(ByVal oWorkBook As WorkBook, ByVal oPermission As WorkBookPermission, ByVal oModulePermission As ModuleWorkBook)
            Dim oDTOworkgroup As New dtoWorkBook With {.WorkBook = oWorkBook, .Permission = oPermission}

            Me.View.CurrentStep = IviewWorkBookEdit.ViewStep.ChangeData

            Me.View.AllowStatusChange = oPermission.ChangeApprovation

            Dim oEditingPermission As EditingPermission = Me.CurrentManager.GetWorkBookAvailablePermission(Me.UserContext.CurrentUserID, oWorkBook, oModulePermission)
            Dim oAvailableStatus As List(Of TranslatedItem(Of Integer)) = Me.CurrentManager.GetTranslatedWorkBookStatusList(Me.UserContext.Language, oEditingPermission)
            If (From o In oAvailableStatus Where o.Id = oWorkBook.Status.Id).Count = 0 Then
                Dim oCurrentStatus As TranslatedItem(Of Integer) = Me.CurrentManager.GetTranslatedWorkBookStatus(Me.UserContext.Language, oWorkBook.Status.Id)
                oAvailableStatus.Add(oCurrentStatus)
                Me.View.AllowStatusChange = False
            End If


            Me.LoadEditingValues(oEditingPermission, oWorkBook.Editing)
            Me.View.LoadWorkBook(oDTOworkgroup, oAvailableStatus)
        End Sub

        Private Sub LoadEditingValues(ByVal oEditingPermission As EditingPermission, ByVal oItemEditing As EditingSettings)
            Dim oAvailableEditing As New List(Of TranslatedItem(Of Integer))

            If (oEditingPermission And EditingSettings.OnlyWorkbookResponsible) > 0 Then
                oAvailableEditing.Add(New TranslatedItem(Of Integer) With {.Id = EditingSettings.OnlyWorkbookResponsible, .Translation = Me.View.GetEditingTranslation(EditingSettings.OnlyWorkbookResponsible)})
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

        Public Sub LoadWorkBook()
            Dim oWorkBook As WorkBook = Me.CurrentManager.GetWorkBook(Me.View.CurrentWorkBookID)
            Dim ModulePermission As ModuleWorkBook
            If oWorkBook.CommunityOwner Is Nothing Then : ModulePermission = ModuleWorkBook.CreatePortalmodule
            Else : ModulePermission = (From p In Me.View.CommunitiesPermission Where p.ID = oWorkBook.CommunityOwner.Id Select p.Permissions).FirstOrDefault
            End If

            Dim oPermission As WorkBookPermission = Me.CurrentManager.GetWorkBookPermission(Me.UserContext.CurrentUser.Id, oWorkBook, ModulePermission)

            Me.ShowWorkBook(oWorkBook, oPermission, ModulePermission)
        End Sub
        Public Sub LoadWorkBookAuthors()
            Dim oWorkBook As WorkBook = Me.CurrentManager.GetWorkBook(Me.View.CurrentWorkBookID)
            Dim ModulePermission As ModuleWorkBook
            If oWorkBook.CommunityOwner Is Nothing Then : ModulePermission = ModuleWorkBook.CreatePortalmodule
            Else : ModulePermission = (From p In Me.View.CommunitiesPermission Where p.ID = oWorkBook.CommunityOwner.Id Select p.Permissions).FirstOrDefault
            End If
            Dim oPermission As WorkBookPermission = Me.CurrentManager.GetWorkBookPermission(Me.UserContext.CurrentUser.Id, oWorkBook, ModulePermission)

            If Not oWorkBook.Id = System.Guid.Empty Then
                If Me.View.CurrentStep = IviewWorkBookEdit.ViewStep.ChangeData Then
                    oWorkBook = Me.SaveWorkBook()
                End If

                Dim oList As IList(Of WorkBookAuthor) = Nothing
                If oPermission.CreateWorkBook OrElse oPermission.EditWorkBook Then
                    oList = oWorkBook.WorkBookAuthors
                Else
                    oList = (From o In oWorkBook.WorkBookAuthors Where o.isDeleted = False).ToList
                End If
                If IsNothing(oList) Then
                    oList = New List(Of WorkBookAuthor)
                End If
                Me.View.CurrentStep = IviewWorkBookEdit.ViewStep.ManagementAuthors

                Dim oListAuthors As New List(Of dtoWorkBookAuthor)
                For Each oAuthor In oList
                    oListAuthors.Add(New dtoWorkBookAuthor() With {.Author = oAuthor, .Permission = oPermission})
                Next

                Me.View.LoadAuthors(oListAuthors)
            Else
                Me.View.ShowError(My.Resources.ModuleBaseResource.WorkBookNotFound)
            End If
        End Sub

        Public Sub LoadSearchAuthors()
            Dim oWorkBook As WorkBook = Me.CurrentManager.GetWorkBook(Me.View.CurrentWorkBookID)
            If Not IsNothing(oWorkBook) Then
                Dim oList As New List(Of Integer)
                oList = (From o In oWorkBook.Authors Select o.Id).ToList
                If IsNothing(oList) Then
                    oList = New List(Of Integer)
                End If
                Dim oCommunityList As New List(Of Integer)
                If Not IsNothing(oWorkBook.CommunityOwner) Then
                    oCommunityList.Add(oWorkBook.CommunityOwner.Id)
                Else
                    oCommunityList = (From o In Me.CommunitiesPermission Select o.ID).ToList
                End If
                Me.View.CurrentStep = IviewWorkBookEdit.ViewStep.AddAuthors
                Me.View.LoadSearchUser(oCommunityList, (oWorkBook.Type = WorkBookType.CommunityShared OrElse oWorkBook.Type = WorkBookType.PersonalShared OrElse oWorkBook.Type = WorkBookType.PersonalSharedCommunity), oList)
            Else
                Me.View.ShowError(My.Resources.ModuleBaseResource.WorkBookNotFound)
            End If
        End Sub
        Public Sub LoadSelectOwner()
            Dim oWorkBook As WorkBook = Me.CurrentManager.GetWorkBook(Me.View.CurrentWorkBookID)
            If Not IsNothing(oWorkBook) Then
                Dim oList As New List(Of iPerson)
                oList = oWorkBook.Authors
                If IsNothing(oList) Then
                    oList = New List(Of iPerson)
                End If
                Me.View.CurrentStep = IviewWorkBookEdit.ViewStep.SelectOwner
                Dim ModulePermission As ModuleWorkBook
                If oWorkBook.CommunityOwner Is Nothing Then : ModulePermission = ModuleWorkBook.CreatePortalmodule
                Else : ModulePermission = (From p In Me.View.CommunitiesPermission Where p.ID = oWorkBook.CommunityOwner.Id Select p.Permissions).FirstOrDefault
                End If
                Me.View.LoadOwner(oWorkBook.Owner.Id, oList, Me.CurrentManager.GetWorkBookPermission(Me.UserContext.CurrentUser.Id, oWorkBook, ModulePermission))
            Else
                Me.View.ShowError(My.Resources.ModuleBaseResource.WorkBookNotFound)
            End If
        End Sub


        Public Sub SaveAndGoToList()
            Dim oWorkBook As WorkBook = Me.SaveWorkBook()
            Me.View.LoadWorkBookList()
        End Sub

        Public Function SaveWorkBook() As WorkBook
            Dim oUnsavedWorkBook As WorkBook = Me.View.GetWorkBook
            Dim oSaved As WorkBook = Nothing

            If Not IsNothing(oUnsavedWorkBook) Then
                oSaved = Me.CurrentManager.SaveWorkBook(Me.UserContext.CurrentUser.Id, oUnsavedWorkBook)
                If Not IsNothing(oSaved) Then
                    Dim Authors As List(Of Integer) = (From au In oSaved.Authors Select au.Id).ToList
                    Dim CommunityID As Integer = 0
                    If Not oSaved.CommunityOwner Is Nothing Then
                        CommunityID = oSaved.CommunityOwner.Id
                    End If

                    If oSaved.isPersonal Then
                        Me.View.NotifyPersonalEdit(CommunityID, oSaved.Id, oSaved.Title, Me.UserContext.CurrentUser.SurnameAndName, Authors)
                    Else
                        Me.View.NotifyCommunityEdit(CommunityID, oSaved.Id, oSaved.Title, Me.UserContext.CurrentUser.SurnameAndName, Authors)
                    End If
                End If
            End If
            Return oSaved
        End Function

        Public Sub SaveOwner(ByVal OwnerID As Integer, ByVal LoadList As Boolean)
            Dim oWorkBook As WorkBook = Me.CurrentManager.SaveOwner(Me.UserContext.CurrentUser.Id, OwnerID, Me.View.CurrentWorkBookID)

            If IsNothing(oWorkBook) Then
                Me.View.CurrentStep = IviewWorkBookEdit.ViewStep.ChangeData
            ElseIf LoadList Then
                Me.View.LoadWorkBookList()
            Else
                Me.View.CurrentStep = IviewWorkBookEdit.ViewStep.ChangeData
                Me.LoadWorkBook()
            End If
        End Sub

        'Private Function GetWorkBookPermission(ByVal oWorkBook As WorkBook, ByVal CurrentUserID As Integer) As WorkBookPermission
        '    Dim oPerson As iPerson = Me.CurrentManager.GetPerson(CurrentUserID)
        '    Dim CommunityPermission As ModuleWorkBook = Nothing
        '    Dim isWorkBookOwner As Boolean = (oWorkBook.Owner.Equals(oPerson))
        '    Dim isWorkBookAuthor As Boolean = (oWorkBook.Authors.Contains(oPerson))
        '    Dim ItemStatus As Boolean = Not (oWorkBook.MetaInfo.Approvation = MetaApprovationStatus.NotApproved OrElse oWorkBook.MetaInfo.Approvation = MetaApprovationStatus.Approved)
        '    Dim WorkBookEditable As Boolean = oWorkBook.MetaInfo.canModify AndAlso ItemStatus

        '    If Not IsNothing(oWorkBook.CommunityOwner) AndAlso oWorkBook.CommunityOwner.Id > 0 Then
        '        CommunityPermission = Me.Permission(oWorkBook.CommunityOwner.Id)
        '    End If

        '    Dim oPermission As New WorkBookPermission
        '    If oWorkBook.isPersonal Then
        '        With oPermission
        '            .AddItems = Not oWorkBook.MetaInfo.isDeleted AndAlso WorkBookEditable AndAlso (isWorkBookAuthor OrElse isWorkBookOwner)
        '            .ChangeApprovation = False
        '            .EditWorkBook = isWorkBookOwner AndAlso Not oWorkBook.MetaInfo.isDeleted
        '            .CreateWorkBook = True
        '            .DeleteWorkBook = isWorkBookOwner AndAlso WorkBookEditable
        '            ' VERIFICARE DI CERTO
        '            .ReadWorkBook = (isWorkBookOwner OrElse isWorkBookAuthor)
        '            .UndeleteWorkBook = isWorkBookOwner
        '            .Admin = False
        '        End With
        '    ElseIf Not CommunityPermission Is Nothing Then
        '        With oPermission
        '            .AddItems = (WorkBookEditable AndAlso isWorkBookAuthor) OrElse (CommunityPermission.Administration) OrElse CommunityPermission.AddItemsToOther
        '            .ChangeApprovation = Not oWorkBook.MetaInfo.isDeleted AndAlso (CommunityPermission.ChangeApprovation OrElse CommunityPermission.Administration)
        '            .CreateWorkBook = CommunityPermission.CreateGroupWorkBook OrElse CommunityPermission.CreateWorkBook OrElse CommunityPermission.Administration
        '            .EditWorkBook = Not oWorkBook.MetaInfo.isDeleted AndAlso ((ItemStatus AndAlso (isWorkBookOwner OrElse isWorkBookAuthor)) OrElse (CommunityPermission.Administration OrElse CommunityPermission.ChangeApprovation))
        '            .DeleteWorkBook = (ItemStatus AndAlso isWorkBookOwner) OrElse (CommunityPermission.Administration)
        '            .ReadWorkBook = (isWorkBookOwner OrElse isWorkBookAuthor) OrElse CommunityPermission.Administration OrElse CommunityPermission.ChangeApprovation
        '            .UndeleteWorkBook = isWorkBookOwner OrElse CommunityPermission.Administration
        '            .Admin = CommunityPermission.Administration
        '        End With
        '    End If
        '    Return oPermission
        'End Function

		Public Sub AddAuthors()
			Dim oWorkBook As WorkBook = Me.CurrentManager.AddAuthorsToWorkBook(Me.View.CurrentWorkBookID, Me.View.SelectedUsers, Me.UserContext.CurrentUser.Id)

			If Not IsNothing(oWorkBook) Then
				Me.LoadWorkBookAuthors()
			Else
				Me.View.ShowError(My.Resources.ModuleBaseResource.WorkBookNotFound)
			End If
		End Sub

		Public Sub RemoveAuthor(ByVal WorkBookAuthorID As System.Guid)
			If WorkBookAuthorID <> System.Guid.Empty Then
				Me.CurrentManager.RemoveAuthor(WorkBookAuthorID)
			End If
			Me.LoadWorkBookAuthors()
		End Sub
		Public Sub VirtualDeleteAuthor(ByVal WorkBookAuthorID As System.Guid)
			If WorkBookAuthorID <> System.Guid.Empty Then
				Me.CurrentManager.RemoveVirtualAuthor(WorkBookAuthorID)
			End If
			Me.LoadWorkBookAuthors()
		End Sub
		Public Sub UnDeleteAuthor(ByVal WorkBookAuthorID As System.Guid)
			If WorkBookAuthorID <> System.Guid.Empty Then
				Me.CurrentManager.UnDeleteVirtualAuthor(WorkBookAuthorID)
			End If
			Me.LoadWorkBookAuthors()
		End Sub
	End Class
End Namespace