Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic

Namespace lm.Comol.Modules.Base.Presentation
	Public Class WorkBookAddPresenter
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
		Public Overloads ReadOnly Property View() As IviewWorkBookAdd
			Get
				Return MyBase.View
			End Get
		End Property
		Public Sub New(ByVal oContext As iApplicationContext)
			MyBase.New(oContext)
			MyBase.CurrentManager = New ManagerWorkBook(MyBase.AppContext)
		End Sub
		Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IviewWorkBookAdd)
			MyBase.New(oContext, view)
			MyBase.CurrentManager = New ManagerWorkBook(MyBase.AppContext)
		End Sub
#End Region

		Public Sub InitView()
			If IsNothing(Me.UserContext.CurrentUser) OrElse Me.UserContext.isAnonymous OrElse Me.UserContext.CurrentUser.Id <= 0 Then
				Me.View.ShowError(My.Resources.ModuleBaseResource.NotPermission)
			ElseIf Me.View.CreateForOther Then
				Me.View.FirstStep = IviewWorkBookAdd.ViewStep.SelectCommunity
				Me.View.InitCommunitySelection()
			Else
				Me.View.CurrentCommunityID = Me.UserContext.CurrentCommunityID
				Me.View.FirstStep = IviewWorkBookAdd.ViewStep.SelectType
				Me.LoadWorkBookType()
			End If
		End Sub

		Public Sub LoadWorkBookType(ByVal ComunityID As Integer)
			Me.View.CurrentCommunityID = ComunityID
			Me.LoadWorkBookType()
		End Sub
		Public Sub LoadWorkBookType()
			Me.View.LoadAvailableTypes(Me.WorkBookTypeAvailable)
		End Sub

		Public Sub LoadWorkBookDataEntry(ByVal oViewType As IviewWorkBookAdd.viewWorkBookType)
            Dim oWorkBook As New WorkBook With {.Id = System.Guid.Empty}
			Dim oType As WorkBookType

			Select Case oViewType
				Case IviewWorkBookAdd.viewWorkBookType.Community
					oType = WorkBookType.Community
				Case IviewWorkBookAdd.viewWorkBookType.CommunityShared
					oType = WorkBookType.CommunityShared
				Case IviewWorkBookAdd.viewWorkBookType.OtherUser
					oType = WorkBookType.Community
				Case IviewWorkBookAdd.viewWorkBookType.Personal
					oType = WorkBookType.Personal
				Case IviewWorkBookAdd.viewWorkBookType.PersonalCommunity
					oType = WorkBookType.PersonalCommunity
				Case IviewWorkBookAdd.viewWorkBookType.PersonalShared
					oType = WorkBookType.PersonalShared
				Case IviewWorkBookAdd.viewWorkBookType.PersonalSharedCommunity
					oType = WorkBookType.PersonalSharedCommunity
			End Select

			Me.View.WorkBookCommunityID = 0
			Me.View.CurrentWorkBookType = oType
			'Me.View.AvailableStep = ViewStepAvailable(oWorkBook)
            oWorkBook.Type = oType
            oWorkBook.isPersonal = (oType = WorkBookType.Personal OrElse oType = WorkBookType.PersonalShared OrElse oType = WorkBookType.PersonalCommunity OrElse oType = WorkBookType.PersonalSharedCommunity)
			If oWorkBook.Type = WorkBookType.Personal OrElse oWorkBook.Type = WorkBookType.PersonalCommunity Then
				Me.View.LastStep = IviewWorkBookAdd.ViewStep.SelectData
				Dim oAuthors As New List(Of Person)
				oAuthors.Add(Me.UserContext.CurrentUser)
				Me.View.LoadAuthorsToSelectOwner(oAuthors)
				Me.View.CurrentOwner = Me.UserContext.CurrentUser
			Else
				Me.View.LastStep = IviewWorkBookAdd.ViewStep.SelectOwner
			End If


			If (oViewType = IviewWorkBookAdd.viewWorkBookType.OtherUser) Then
				Me.View.AddCurrentUserToAuthors = False
			Else
				Me.View.AddCurrentUserToAuthors = True
			End If

            Dim oPermission As WorkBookPermission = Me.CreateWorkBookPermission(oType)
            Dim oCommunity As iCommunity = Me.CurrentManager.GetCommunity(Me.View.CurrentCommunityID)
            Dim oEditingPermission As EditingPermission = EditingPermission.Authors Or EditingPermission.Responsible

            oWorkBook.Note = ""
            oWorkBook.isDraft = False
            oWorkBook.Text = ""
            oWorkBook.Owner = Me.UserContext.CurrentUser
            oWorkBook.CreatedBy = Me.UserContext.CurrentUser
            oWorkBook.CreatedOn = Now
            oWorkBook.Status = Me.CurrentManager.GetDefaultWorkBookStatus()
            oWorkBook.Editing = EditingSettings.AllAuthors
            oWorkBook.CommunityOwner = oCommunity

            Dim ModulePermission As ModuleWorkBook
            If oWorkBook.CommunityOwner Is Nothing Then : ModulePermission = ModuleWorkBook.CreatePortalmodule
            Else : ModulePermission = (From p In Me.View.CommunitiesPermission Where p.ID = oWorkBook.CommunityOwner.Id Select p.Permissions).FirstOrDefault
            End If

            If Not IsNothing(oCommunity) AndAlso Not oWorkBook.isPersonal AndAlso (ModulePermission.Administration OrElse ModulePermission.CreateGroupWorkBook) Then
                oEditingPermission = oEditingPermission Or EditingPermission.ModuleManager
            End If

			Select Case oType
				Case WorkBookType.Community
					oWorkBook.Title = String.Format(My.Resources.ModuleBaseResource.WKcommunitylTitle, oCommunity.Name)
					Me.View.WorkBookCommunityID = Me.View.CurrentCommunityID
					Me.View.SelectOnlyCurrentUser = Not (oViewType = IviewWorkBookAdd.viewWorkBookType.OtherUser)
                    Me.View.AllowSelectCurrentUser = True
                Case WorkBookType.CommunityShared
                    oWorkBook.Title = String.Format(My.Resources.ModuleBaseResource.WKcommunitySharedlTitle, oCommunity.Name)
                    Me.View.WorkBookCommunityID = Me.View.CurrentCommunityID
                    Me.View.SelectOnlyCurrentUser = False
                    Me.View.AllowSelectCurrentUser = True
				Case WorkBookType.Personal
                    oWorkBook.Title = String.Format(My.Resources.ModuleBaseResource.WKpersonalTitle, Me.UserContext.CurrentUser.SurnameAndName)
                    Me.View.SelectOnlyCurrentUser = True
                    Me.View.AllowSelectCurrentUser = False
                Case WorkBookType.PersonalCommunity
                    oWorkBook.Title = String.Format(My.Resources.ModuleBaseResource.WKpersonalCommunitySharedlTitle, oCommunity.Name)
                    Me.View.WorkBookCommunityID = Me.View.CurrentCommunityID
                    Me.View.SelectOnlyCurrentUser = True
                    Me.View.AllowSelectCurrentUser = False
                Case WorkBookType.PersonalShared
                    oWorkBook.Title = String.Format(My.Resources.ModuleBaseResource.WKpersonalSharedlTitle, Me.UserContext.CurrentUser.SurnameAndName)
                    Me.View.SelectOnlyCurrentUser = False
                    Me.View.AllowSelectCurrentUser = False
                Case WorkBookType.PersonalSharedCommunity
                    oWorkBook.Title = String.Format(My.Resources.ModuleBaseResource.WKpersonalCommunitySharedlTitle, oCommunity.Name)

                    Me.View.WorkBookCommunityID = Me.View.CurrentCommunityID
                    Me.View.SelectOnlyCurrentUser = False
                    Me.View.AllowSelectCurrentUser = False
            End Select




            Dim oDTOworkgroup As New dtoWorkBook With {.WorkBook = oWorkBook, .Permission = oPermission}


            Me.View.CurrentStep = IviewWorkBookAdd.ViewStep.SelectData

            Me.View.AllowStatusChange = oPermission.ChangeApprovation

            Dim oAvailableStatus As List(Of TranslatedItem(Of Integer)) = Me.CurrentManager.GetTranslatedWorkBookStatusList(Me.UserContext.Language, oEditingPermission)
            If (From o In oAvailableStatus Where o.Id = oWorkBook.Status.Id).Count = 0 Then
                Dim oCurrentStatus As TranslatedItem(Of Integer) = Me.CurrentManager.GetTranslatedWorkBookStatus(Me.UserContext.Language, oWorkBook.Status.Id)
                oAvailableStatus.Add(oCurrentStatus)
                Me.View.AllowStatusChange = False
            End If
            Me.LoadEditingValues(oEditingPermission, oWorkBook.Editing)
            Me.View.LoadWorkBook(oDTOworkgroup, oAvailableStatus)
            Me.SetupUserSearchList(oWorkBook)
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
        Private Sub SetupUserSearchList(ByVal oWorkBook As WorkBook)
            If Not Me.View.SelectOnlyCurrentUser Then
                Dim ExceptUsers As New List(Of Integer)
                Dim oCommunityID As New List(Of Integer)

                If Not IsNothing(oWorkBook.CommunityOwner) AndAlso Not (oWorkBook.CommunityOwner.Id = 0 AndAlso oWorkBook.isPersonal) Then
                    oCommunityID.Add(oWorkBook.CommunityOwner.Id)
                Else
                    oCommunityID = (From o In Me.CommunitiesPermission Select o.ID).ToList
                End If
                If IsNothing(oCommunityID) Then
                    oCommunityID = New List(Of Integer)
                End If
                Dim oType As WorkBookType = Me.View.CurrentWorkBookType
                ExceptUsers.Add(Me.UserContext.CurrentUser.Id)

                Me.View.InitUsersList(oCommunityID, (oType = WorkBookType.CommunityShared OrElse oType = WorkBookType.PersonalShared OrElse oType = WorkBookType.PersonalSharedCommunity), ExceptUsers)
            End If
        End Sub

        Public Sub LoadNextStep()
            If Me.View.CurrentStep = IviewWorkBookAdd.ViewStep.SelectData Then
                If Me.View.SelectOnlyCurrentUser Then
                    Me.CreateWokbook()
                Else
                    Me.View.LoadSearchAuthors()
                End If
            ElseIf Me.View.CurrentStep = IviewWorkBookAdd.ViewStep.SelectAuthors Then
                Dim SelectedAuthors As List(Of Person) = Me.View.SelectedUsers
                If IsNothing(SelectedAuthors) Then
                    SelectedAuthors = New List(Of Person)
                End If
                If Me.View.AddCurrentUserToAuthors Then
                    SelectedAuthors.Add(Me.UserContext.CurrentUser)
                End If
                If SelectedAuthors.Count > 0 Then
                    Me.View.CurrentStep = IviewWorkBookAdd.ViewStep.SelectOwner
                    Me.View.LoadAuthorsToSelectOwner((From o In SelectedAuthors Order By o.SurnameAndName, o.Name Select o).ToList)
                    If Me.View.AddCurrentUserToAuthors Then
                        Me.View.CurrentOwner = Me.UserContext.CurrentUser
                    End If
                Else
                    Me.View.CurrentStep = IviewWorkBookAdd.ViewStep.SelectAuthors
                End If
            ElseIf Me.View.CurrentStep = IviewWorkBookAdd.ViewStep.SelectOwner Then
                Dim SelectedAuthors As List(Of Person) = Me.View.SelectedUsers
                If IsNothing(SelectedAuthors) Then
                    SelectedAuthors = New List(Of Person)
                End If
                If Me.View.AddCurrentUserToAuthors Then
                    SelectedAuthors.Add(Me.UserContext.CurrentUser)
                End If
                If (SelectedAuthors.Count = 0) Then
                    Me.View.ShowCompleteMessage(IviewWorkBookAdd.viewError.AuthorsNotFound)
                ElseIf (Me.View.CurrentOwner Is Nothing) Then
                    Me.View.ShowCompleteMessage(IviewWorkBookAdd.viewError.OwnerNotFound)
                Else
                    Me.CreateWokbook()
                End If
            End If
        End Sub
        Public Sub LoadPreviousStep()
            If Me.View.CurrentStep = IviewWorkBookAdd.ViewStep.SelectData Then
                Me.View.LoadAvailableTypes(Me.WorkBookTypeAvailable)
            ElseIf Me.View.CurrentStep = IviewWorkBookAdd.ViewStep.SelectType AndAlso Me.View.CreateForOther Then
                Me.View.InitCommunitySelection()
            ElseIf Me.View.CurrentStep = IviewWorkBookAdd.ViewStep.SelectAuthors Then
                Me.View.CurrentStep = IviewWorkBookAdd.ViewStep.SelectData
            ElseIf Me.View.CurrentStep = IviewWorkBookAdd.ViewStep.SelectOwner Then
                Me.View.CurrentStep = IviewWorkBookAdd.ViewStep.SelectAuthors
            End If
        End Sub

        Private Function GenerateWorkBook() As WorkBook
            Dim oWorkBook As New WorkBook With {.Id = System.Guid.Empty}
            Dim oType As WorkBookType = Me.View.CurrentWorkBookType

            oWorkBook = Me.View.GetCurrentWorkBook
            If Not IsNothing(oWorkBook) Then
                If Me.View.WorkBookCommunityID > 0 Then
                    oWorkBook.CommunityOwner = New Community() With {.Id = Me.View.WorkBookCommunityID}
                End If

                If Me.View.SelectOnlyCurrentUser Then
                    oWorkBook.Owner = Me.UserContext.CurrentUser
                Else
                    oWorkBook.Owner = Me.View.CurrentOwner
                End If
                oWorkBook.isPersonal = (oType = WorkBookType.Personal OrElse oType = WorkBookType.PersonalShared OrElse oType = WorkBookType.PersonalCommunity OrElse oType = WorkBookType.PersonalSharedCommunity)
                oWorkBook.Type = oType
                oWorkBook.WorkBookAuthors = New List(Of WorkBookAuthor)

                Dim SelectedAuthors As List(Of Person) = Me.View.SelectedUsers
                If IsNothing(SelectedAuthors) Then
                    SelectedAuthors = New List(Of Person)
                End If
                If Me.View.AddCurrentUserToAuthors Then
                    SelectedAuthors.Add(Me.UserContext.CurrentUser)
                End If

                For Each oAuthor In SelectedAuthors
                    oWorkBook.WorkBookAuthors.Add(New WorkBookAuthor() With {.Author = oAuthor, .IsOwner = (oWorkBook.Owner.Id = oAuthor.Id)})
                Next
            End If

            Return oWorkBook
        End Function
        Private Function WorkBookTypeAvailable() As List(Of IviewWorkBookAdd.viewWorkBookType)
            Dim oList As New List(Of IviewWorkBookAdd.viewWorkBookType)
            If Not Me.UserContext.isAnonymous AndAlso Me.UserContext.CurrentUser.Id > 0 Then
                oList.Add(IviewWorkBookAdd.viewWorkBookType.Personal)
                oList.Add(IviewWorkBookAdd.viewWorkBookType.PersonalShared)
            End If

            Dim oModule As ModuleWorkBook = Me.Permission(Me.View.CurrentCommunityID)

            If Not Me.UserContext.isAnonymous AndAlso Not IsNothing(oModule) Then
                If Me.View.CurrentCommunityID > 0 Then
                    If oModule.CreateWorkBook OrElse oModule.Administration Then
                        oList.Add(IviewWorkBookAdd.viewWorkBookType.Community)
                    End If
                    If oModule.CreateGroupWorkBook OrElse oModule.Administration Then
                        oList.Add(IviewWorkBookAdd.viewWorkBookType.CommunityShared)
                        oList.Add(IviewWorkBookAdd.viewWorkBookType.OtherUser)
                    End If
                    oList.Add(IviewWorkBookAdd.viewWorkBookType.PersonalCommunity)
                End If
            End If
            Return oList
        End Function
        Private Function ViewStepAvailable(ByVal oWorkBook As WorkBook) As List(Of IviewWorkBookAdd.ViewStep)
            Dim oList As New List(Of IviewWorkBookAdd.ViewStep)

            oList.Add(IviewWorkBookAdd.ViewStep.SelectData)

            If oWorkBook.Type = WorkBookType.CommunityShared OrElse oWorkBook.Type = WorkBookType.PersonalShared OrElse oWorkBook.Type = WorkBookType.PersonalSharedCommunity Then
                oList.Add(IviewWorkBookAdd.ViewStep.SelectOwner)
                oList.Add(IviewWorkBookAdd.ViewStep.SelectAuthors)
            End If
            Return oList
        End Function

        Private Sub CreateWokbook()
            Dim oSavedWorkBook As WorkBook = Nothing
            Dim oWorkBook As WorkBook = GenerateWorkBook()
            If Not IsNothing(oWorkBook) Then
                oSavedWorkBook = Me.CurrentManager.CreateWorkBook(Me.UserContext.CurrentUser.Id, oWorkBook)
            End If

            If oSavedWorkBook Is Nothing Then
                Me.View.ShowCompleteMessage(IviewWorkBookAdd.viewError.CreateError)
            Else
                Dim Authors As List(Of Integer) = (From au In oSavedWorkBook.Authors Select au.Id).ToList
                Dim CommunityID As Integer = 0
                If Not oSavedWorkBook.CommunityOwner Is Nothing Then
                    CommunityID = oSavedWorkBook.CommunityOwner.Id
                End If

                If oSavedWorkBook.isPersonal Then
                    Me.View.NotifyPersonal(CommunityID, oSavedWorkBook.Id, oSavedWorkBook.Title, Me.UserContext.CurrentUser.SurnameAndName, Authors)
                Else
                    Me.View.NotifyCommunity(CommunityID, oSavedWorkBook.Id, oSavedWorkBook.Title, Me.UserContext.CurrentUser.SurnameAndName, Authors)
                End If
                Me.View.LoadWorkBookList(oWorkBook.Type)
            End If
        End Sub
		'Public Function CreateWorkBook(ByVal oWorkBook As WorkBook, ByVal CommunityID As Integer, ByVal oOwner As iPerson) As WorkBook
		'	Dim oCommunity As iCommunity = Nothing
		'	If Not (Me.View.CurrentWorkBookType = WorkBookType.Personal OrElse Me.View.CurrentWorkBookType = WorkBookType.PersonalShared) Then
		'		oCommunity = Me.CurrentManager.GetCommunity(CommunityID)
		'	End If
		'	oWorkBook.CommunityOwner = oCommunity
		'	oWorkBook.Owner = Me.CurrentManager.GetPerson(oOwner.Id)
		'	Return Me.CurrentManager.SaveWorkBook(Me.UserContext.CurrentUser.Id, oWorkBook)

		'End Function
		'Public Function SaveWorkBook(ByVal oWorkBook As WorkBook, ByVal CommunityID As Integer, ByVal oOwner As iPerson) As WorkBook
		'	Dim oCommunity As iCommunity = Me.CurrentManager.GetCommunity(CommunityID)
		'	oWorkBook.CommunityOwner = oCommunity
		'	oWorkBook.Owner = Me.CurrentManager.GetPerson(oOwner.Id)
		'	Return Me.CurrentManager.SaveWorkBook(Me.UserContext.CurrentUser.Id, oWorkBook)

		'End Function

        Private Function CreateWorkBookPermission(ByVal oType As WorkBookType) As WorkBookPermission
            Dim CommunityPermission As ModuleWorkBook = Me.Permission(Me.View.CurrentCommunityID)
            Dim oPermission As New WorkBookPermission

            If oType = WorkBookType.Personal OrElse oType = WorkBookType.PersonalCommunity OrElse oType = WorkBookType.PersonalShared OrElse oType = WorkBookType.PersonalSharedCommunity Then
                oPermission.AddItems = True
                oPermission.ChangeApprovation = True
                oPermission.EditWorkBook = True
                oPermission.CreateWorkBook = True
                oPermission.DeleteWorkBook = True
                oPermission.ReadWorkBook = True
                oPermission.UndeleteWorkBook = True
                oPermission.Admin = False
            ElseIf oType = WorkBookType.Community Then
                oPermission.AddItems = True
                oPermission.ChangeApprovation = CommunityPermission.ChangeApprovation
                oPermission.EditWorkBook = True
                oPermission.CreateWorkBook = CommunityPermission.CreateWorkBook
                oPermission.DeleteWorkBook = True
                oPermission.ReadWorkBook = True
                oPermission.UndeleteWorkBook = True
                oPermission.Admin = CommunityPermission.Administration
            End If
            Return oPermission
        End Function
	End Class
End Namespace