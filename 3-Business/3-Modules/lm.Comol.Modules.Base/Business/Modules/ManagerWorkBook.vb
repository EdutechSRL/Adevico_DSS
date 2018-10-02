Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.Base.DomainModel

Imports NHibernate
Imports NHibernate.Criterion
Imports NHibernate.Linq
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Core.File

Namespace lm.Comol.Modules.Base.BusinessLogic
    Public Class ManagerWorkBook
        Implements lm.Comol.Core.DomainModel.Common.iDomainManager

#Region "Private property"
        Private _UserContext As iUserContext
        Private _Datacontext As iDataContext
#End Region

#Region "Public property"
        Private ReadOnly Property DC() As iDataContext
            Get
                Return _Datacontext
            End Get
        End Property
        Private ReadOnly Property CurrentUserContext() As iUserContext
            Get
                Return _UserContext
            End Get
        End Property
#End Region

        Public Sub New()
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext)
            Me._UserContext = oContext.UserContext
            Me._Datacontext = oContext.DataContext
        End Sub
        Public Sub New(ByVal oUserContext As iUserContext, ByVal oDatacontext As iDataContext)
            Me._UserContext = oUserContext
            Me._Datacontext = oDatacontext
        End Sub

        Public Function GetWorkBook(ByVal WorkBookID As System.Guid) As WorkBook
            Dim oWorkBook As WorkBook = Nothing
            Try
                oWorkBook = DC.GetById(Of WorkBook)(WorkBookID)
            Catch ex As Exception
                Debug.Write(ex.ToString)

                Return Nothing
            End Try
            Return oWorkBook
        End Function
        Public Function GetPerson(ByVal PersonID As Integer) As iPerson
            Dim oPerson As Person = Nothing

            Try
                oPerson = _Datacontext.GetById(Of Person)(PersonID)
            Catch ex As Exception
                Debug.Write(ex.ToString)
            End Try
            If IsNothing(oPerson) Then
                oPerson = New Person
            End If
            Return oPerson
        End Function
        Public Function GetCommunity(ByVal CommunityID As Integer) As Community
            Dim oCommunity As Community = Nothing

            Try
                oCommunity = _Datacontext.GetById(Of Community)(CommunityID)
            Catch ex As Exception
                Debug.Write(ex.ToString)
            End Try
            If IsNothing(oCommunity) Then
                oCommunity = New Community
            End If
            Return oCommunity
        End Function
        Public Function GetModule(ByVal ModuleCode As String) As ModuleDefinition
            Dim oModuleDefinition As ModuleDefinition = Nothing

            Try
                DC.BeginTransaction()
                oModuleDefinition = (From m As ModuleDefinition In _Datacontext.GetCurrentSession.Linq(Of ModuleDefinition)() Where m.Code = ModuleCode Select m).FirstOrDefault
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                Debug.Write(ex.ToString)
            End Try
            Return oModuleDefinition
        End Function

#Region "WorkBook"
        Public Function SaveUserDiary(ByVal CreatedByID As Integer, ByVal OwnerID As Integer, ByVal CommunityID As Integer, ByVal oUnsavedDiary As WorkBook) As WorkBook
            Dim oWorkBook As New WorkBook
            Dim oMeta As New MetaData

            If oUnsavedDiary.Id <> System.Guid.Empty Then
                oWorkBook = Me.GetWorkBook(oUnsavedDiary.Id)
            End If
            Try
                DC.BeginTransaction()
                Dim oCreatedBy As Person = DC.GetById(Of Person)(CreatedByID)
                Dim oCommunity As Community = DC.GetById(Of Community)(CommunityID)
                Dim oOwner As Person = DC.GetById(Of Person)(OwnerID)
                If oUnsavedDiary.Id = System.Guid.Empty Then
                    oWorkBook.Owner = oCreatedBy
                    oWorkBook.CreatedBy = oCreatedBy
                    oWorkBook.CreatedOn = Now
                    oWorkBook.ModifiedBy = oCreatedBy
                    oWorkBook.ModifiedOn = oWorkBook.CreatedOn

                    oWorkBook.isDeleted = False
                    oWorkBook.isDraft = oUnsavedDiary.isDraft
                Else
                    oWorkBook = DC.GetById(Of WorkBook)(oUnsavedDiary.Id)
                    oWorkBook.ModifiedBy = oCreatedBy
                    oWorkBook.ModifiedOn = Now

                    If oWorkBook.Status.Id <> oUnsavedDiary.Status.Id Then
                        oWorkBook.ApprovedBy = oCreatedBy
                        oWorkBook.ApprovedOn = oWorkBook.ModifiedOn
                    End If
                End If
                oWorkBook.Editing = oUnsavedDiary.Editing
                With oWorkBook
                    If oUnsavedDiary.Id = System.Guid.Empty Then
                        .CommunityOwner = oCommunity
                    End If
                    .Owner = oOwner
                    .Note = oUnsavedDiary.Note
                    .Text = oUnsavedDiary.Text
                    .Title = oUnsavedDiary.Title
                    Dim oStatus As WorkBookStatus = _Datacontext.GetCurrentSession.Get(Of WorkBookStatus)(CInt(oUnsavedDiary.Status.Id))
                    .Status = oStatus
                End With

                If oWorkBook.WorkBookAuthors Is Nothing OrElse oWorkBook.WorkBookAuthors.Count = 0 Then
                    oWorkBook.WorkBookAuthors = New List(Of WorkBookAuthor)
                    Dim oAuthor As New WorkBookAuthor
                    oAuthor.Author = oOwner
                    oAuthor.WorkBookOwner = oWorkBook
                    oAuthor.IsOwner = True
                    oAuthor.CreatedBy = oCreatedBy
                    oAuthor.CreatedOn = oWorkBook.CreatedOn
                    oWorkBook.WorkBookAuthors.Add(oAuthor)

                    DC.SaveOrUpdate(oWorkBook)
                    DC.SaveOrUpdate(oAuthor)
                    DC.Commit()
                Else
                    DC.SaveOrUpdate(oWorkBook)
                    DC.Commit()
                End If
            Catch ex As Exception
                Debug.Write(ex.ToString)
                DC.Rollback()
                Return Nothing
            End Try
            Return oWorkBook
        End Function
        Public Function CreateWorkBook(ByVal CreatedByID As Integer, ByVal oUnsavedWorkBook As WorkBook) As WorkBook
            Dim oWorkBook As New WorkBook
            Dim oMeta As New MetaData

            If oUnsavedWorkBook.Id = System.Guid.Empty Then
                Try
                    DC.BeginTransaction()
                    Dim oCreatedBy As Person = DC.GetById(Of Person)(CreatedByID)
                    Dim oOwner As Person = DC.GetById(Of Person)(oUnsavedWorkBook.Owner.Id)

                    oWorkBook.CreatedBy = oCreatedBy
                    oWorkBook.CreatedOn = Now
                    oWorkBook.ModifiedBy = oCreatedBy
                    oWorkBook.ModifiedOn = oWorkBook.CreatedOn
                    With oWorkBook
                        .CommunityOwner = oUnsavedWorkBook.CommunityOwner
                        .Owner = oOwner
                        .Note = oUnsavedWorkBook.Note
                        .Text = oUnsavedWorkBook.Text
                        .Title = oUnsavedWorkBook.Title
                        .isPersonal = oUnsavedWorkBook.isPersonal
                        .Type = oUnsavedWorkBook.Type

                        Dim oStatus As WorkBookStatus = _Datacontext.GetCurrentSession.Get(Of WorkBookStatus)(CInt(oUnsavedWorkBook.Status.Id))
                        .Status = oStatus
                        .ApprovedOn = Now
                        .ApprovedBy = Me.CurrentUserContext.CurrentUser
                        .isDraft = oUnsavedWorkBook.isDraft
                        .Editing = oUnsavedWorkBook.Editing
                    End With

                    oWorkBook.WorkBookAuthors = New List(Of WorkBookAuthor)
                    DC.SaveOrUpdate(oWorkBook)

                    For Each oUnsavedAuthor In oUnsavedWorkBook.WorkBookAuthors
                        Dim oAuthor As New WorkBookAuthor
                        oAuthor.Author = oUnsavedAuthor.Author
                        oAuthor.WorkBookOwner = oWorkBook
                        oAuthor.IsOwner = oUnsavedAuthor.IsOwner
                        oAuthor.CreatedBy = oCreatedBy
                        oAuthor.CreatedOn = oWorkBook.CreatedOn
                        oWorkBook.WorkBookAuthors.Add(oAuthor)
                        DC.SaveOrUpdate(oAuthor)
                    Next
                    DC.SaveOrUpdate(oWorkBook)
                    DC.Commit()
                Catch ex As Exception
                    EventLog.WriteEntry("workbook", ex.Message)
                    Debug.Write(ex.ToString)
                    DC.Rollback()
                    Return Nothing
                End Try
            Else
                oWorkBook = Nothing
            End If

            Return oWorkBook
        End Function
        Public Function SaveWorkBook(ByVal ModifiedByID As Integer, ByVal oUnsavedDiary As WorkBook) As WorkBook
            Dim oWorkBook As New WorkBook
            Dim oMeta As New MetaData

            If oUnsavedDiary.Id <> System.Guid.Empty Then
                oWorkBook = Me.GetWorkBook(oUnsavedDiary.Id)
                If IsNothing(oWorkBook) Then
                    Return Nothing
                End If
            Else : Return Nothing
            End If
            Try
                DC.BeginTransaction()
                Dim oModifiedBy As Person = DC.GetById(Of Person)(ModifiedByID)
                Dim oOwner As Person = Nothing

                If Not IsNothing(oUnsavedDiary.Owner) Then
                    oOwner = DC.GetById(Of Person)(oUnsavedDiary.Owner.Id)
                Else
                    oOwner = oWorkBook.Owner
                End If

                Dim oModifiedOn As DateTime = Now

                oWorkBook = DC.GetById(Of WorkBook)(oUnsavedDiary.Id)
                If Not IsNothing(oOwner) Then
                    oWorkBook.Owner = oOwner
                End If
                oWorkBook.ModifiedBy = oModifiedBy
                oWorkBook.ModifiedOn = Now
                If oWorkBook.Status.Id <> oUnsavedDiary.Status.Id Then
                    oWorkBook.ApprovedBy = oModifiedBy
                    oWorkBook.ApprovedOn = oWorkBook.ModifiedOn
                End If
                With oWorkBook
                    If .CommunityOwner Is Nothing Then
                        .CommunityOwner = oUnsavedDiary.CommunityOwner
                    End If
                    .Owner = oOwner
                    .Note = oUnsavedDiary.Note
                    .Text = oUnsavedDiary.Text
                    .Title = oUnsavedDiary.Title

                    Dim oStatus As WorkBookStatus = _Datacontext.GetCurrentSession.Get(Of WorkBookStatus)(CInt(oUnsavedDiary.Status.Id))
                    .Status = oStatus
                    .isDraft = False
                    .Editing = oUnsavedDiary.Editing
                End With
                If IsNothing(oWorkBook.WorkBookAuthors) Then
                    oWorkBook.WorkBookAuthors = New List(Of WorkBookAuthor)
                End If

                DC.SaveOrUpdate(oWorkBook)
                If oWorkBook.WorkBookAuthors.Count = 0 And Not IsNothing(oUnsavedDiary.WorkBookAuthors) Then
                    For Each oUnsavedAuthor In oUnsavedDiary.WorkBookAuthors
                        Dim oAuthor As New WorkBookAuthor
                        oAuthor.Author = oOwner
                        oAuthor.WorkBookOwner = oWorkBook
                        oAuthor.IsOwner = True
                        oAuthor.CreatedBy = oModifiedBy
                        oAuthor.CreatedOn = oWorkBook.ModifiedOn
                        oWorkBook.WorkBookAuthors.Add(oAuthor)
                        DC.SaveOrUpdate(oAuthor)
                    Next
                    DC.SaveOrUpdate(oWorkBook)
                End If
                DC.Commit()
            Catch ex As Exception
                Debug.Write(ex.ToString)
                DC.Rollback()
                Return Nothing
            End Try
            Return oWorkBook
        End Function
        Public Function SaveOwner(ByVal ModifiedByID As Integer, ByVal OwnerID As Integer, ByVal WorkBookID As System.Guid) As WorkBook
            Dim oWorkBook As New WorkBook
            Dim oMeta As New MetaData

            If WorkBookID <> System.Guid.Empty Then
                Dim oModifiedBy As Person = DC.GetById(Of Person)(ModifiedByID)
                Dim oOwner As Person = DC.GetById(Of Person)(OwnerID)

                oWorkBook = Me.NEW_GetWorkBook(WorkBookID)
                If Not IsNothing(oWorkBook) AndAlso Not IsNothing(oModifiedBy) AndAlso Not IsNothing(oOwner) Then
                    Try
                        DC.BeginTransaction()

                        oWorkBook.Owner = oOwner
                        oWorkBook.ModifiedBy = oModifiedBy
                        oWorkBook.ModifiedOn = Now

                        DC.SaveOrUpdate(oWorkBook)
                        DC.Commit()
                    Catch ex As Exception
                        Debug.Write(ex.ToString)
                        DC.Rollback()
                        Return Nothing
                    End Try
                Else : Return Nothing
                End If
            Else : Return Nothing
            End If
            Return oWorkBook
        End Function
#End Region

        Private Function HasModuleWorkbookAdminPermission(ByVal ModulePermission As ModuleWorkBook) As Boolean
            Return ModulePermission.Administration OrElse ModulePermission.AddItemsToOther OrElse ModulePermission.ChangeOtherWorkBook
        End Function

#Region "NEW FUNCTION"
        Public Function NEW_UserHasPortalWorkBook(ByVal UserID As Integer) As Boolean
            Return NEW_UserHasWorkbook(UserID, 0, True)
        End Function
        Public Function NEW_UserHasCommunityWorkBook(ByVal UserID As Integer, ByVal CommunityID As Integer, ByVal Personal As Boolean) As Boolean
            Return NEW_UserHasWorkbook(UserID, CommunityID, Personal)
        End Function
        Public Function NEW_UserHasCommunitiesWorkBook(ByVal UserID As Integer, ByVal Personal As Boolean) As Boolean
            Return NEW_UserHasWorkbook(UserID, -1, Personal)
        End Function
        Private Function NEW_UserHasWorkbook(ByVal UserID As Integer, ByVal CommunityID As Integer, ByVal Personal As Boolean) As Boolean
            Dim iCount As Integer = 0
            Try
                DC.BeginTransaction()
                Dim oPerson As Person = Me.GetPerson(UserID)
                Dim oCommunity As Community = Me.GetCommunity(CommunityID)
                Dim QueryAuthors = (From au In DC.GetCurrentSession.Linq(Of WorkBookAuthor)() Where au.Author Is oPerson Select au.WorkBookOwner.Id)

                Dim Query = (From w In DC.GetCurrentSession.Linq(Of WorkBook)())
                Query = Query.Where(Function(w) QueryAuthors.ToList.Contains(w.Id) AndAlso w.isPersonal = Personal AndAlso ((CommunityID = -1) _
                        OrElse (CommunityID = 0 AndAlso w.CommunityOwner Is Nothing) _
                        OrElse (CommunityID > 0 AndAlso w.CommunityOwner Is oCommunity)))

                iCount = Query.Count
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                Debug.Write(ex.ToString)
            End Try
            Return (iCount > 0)
        End Function
        Public Function NEW_WorkBookCount(ByVal UserID As Integer, ByVal CommunitiesID As List(Of Integer), ByVal oType As WorkBookTypeFilter, ByVal oFilter As WorkBookCommunityFilter, ByVal Record As ObjectStatus) As Integer
            Dim iCount As Integer = 0
            Try
                DC.BeginTransaction()
                Dim oPerson As Person = Me.GetPerson(UserID)

                Dim QueryResponsible = (From au In DC.GetCurrentSession.Linq(Of WorkBookAuthor)() Where au.Author Is oPerson AndAlso au.IsOwner Select au.WorkBookOwner.Id)
                Dim QueryAuthors = (From au In DC.GetCurrentSession.Linq(Of WorkBookAuthor)() Where au.Author Is oPerson Select au.WorkBookOwner.Id)

                Dim Query = (From w In DC.GetCurrentSession.Linq(Of WorkBook)())
                Select Case oType
                    Case WorkBookTypeFilter.PersonalWorkBook
                        Query = Query.Where(Function(w) ((QueryAuthors.ToList.Contains(w.Id) AndAlso w.isDeleted = False) OrElse QueryResponsible.ToList.Contains(w.Id)) AndAlso w.isPersonal = True AndAlso ((oFilter = WorkBookCommunityFilter.Portal AndAlso w.CommunityOwner Is Nothing) _
                                    OrElse ((oFilter = WorkBookCommunityFilter.CurrentCommunity AndAlso CommunitiesID.Contains(w.CommunityOwner.Id)) _
                                    OrElse (oFilter = WorkBookCommunityFilter.AllCommunities))))
                    Case WorkBookTypeFilter.AssignedWorkBook
                        Query = Query.Where(Function(w) ((QueryAuthors.ToList.Contains(w.Id) AndAlso w.isDeleted = False) OrElse QueryResponsible.ToList.Contains(w.Id)) AndAlso w.isPersonal = False AndAlso (((oFilter = WorkBookCommunityFilter.CurrentCommunity AndAlso CommunitiesID.Contains(w.CommunityOwner.Id)) _
                                                          OrElse (oFilter = WorkBookCommunityFilter.AllCommunities))))
                    Case WorkBookTypeFilter.ManageWorkBook
                        ' AUTORE PROPRIETARIO

                        ' NON SONO AUTORE
                        QueryAuthors = (From au In DC.GetCurrentSession.Linq(Of WorkBookAuthor)() Where au.Author Is oPerson AndAlso Not au.IsOwner Select au.WorkBookOwner.Id)
                        Query = Query.Where(Function(w) (QueryResponsible.ToList.Contains(w.Id) OrElse Not QueryAuthors.ToList.Contains(w.Id)) AndAlso w.isPersonal = False AndAlso (((oFilter = WorkBookCommunityFilter.CurrentCommunity AndAlso CommunitiesID.Contains(w.CommunityOwner.Id)) _
                                                        OrElse (oFilter = WorkBookCommunityFilter.AllCommunities AndAlso CommunitiesID.Contains(w.CommunityOwner.Id)))))
                End Select




                iCount = Query.Count
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                Debug.Write(ex.ToString)
            End Try
            Return iCount
        End Function
        Public Function NEW_GetWorkBooksListWithPermission(ByVal UserID As Integer, ByVal PortalName As String, ByVal AllComunitiesName As String, ByVal CommunitiesID As List(Of Integer), ByVal oType As WorkBookTypeFilter, ByVal oOrder As WorkBookOrder, ByVal oFilter As WorkBookCommunityFilter, ByVal oPager As lm.Comol.Core.DomainModel.PagerBase, ByVal oModulePermissions As List(Of ModuleCommunityPermission(Of ModuleWorkBook)), ByVal oLanguage As Language) As List(Of dtoWorkbooks)
            Dim oReturnList As List(Of dtoWorkbooks) = New List(Of dtoWorkbooks)
            Dim oList As List(Of dtoCommunityWorkbook) = Me.NEW_GetWorkBooks(UserID, PortalName, AllComunitiesName, CommunitiesID, oType, oOrder, oFilter, oPager)
            If Not IsNothing(oList) AndAlso oList.Count > 0 Then
                Dim query = (From oDto In oList Group By ComID = oDto.CommunityID, ComName = oDto.CommunityName Into Workbooks = Group Select ComID, ComName, Workbooks)
                Dim oTranslatedStatus As List(Of TranslatedItem(Of Integer)) = Me.GetTranslatedWorkBookStatusList(oLanguage)
                If oOrder = WorkBookOrder.Community Then
                    oReturnList = (From oDto In query Select New dtoWorkbooks(oDto.ComID, oDto.ComName) With {.Items = (From w In oDto.Workbooks Select Me.GetDTOWorkBookItem(UserID, w.WorkBookObject, oModulePermissions, oTranslatedStatus, PortalName, AllComunitiesName)).OrderByDescending(Function(c) c.ModifiedOn).ToList}).ToList
                Else
                    oReturnList = (From oDto In query Select New dtoWorkbooks(oDto.ComID, oDto.ComName) With {.Items = (From w In oDto.Workbooks Select Me.GetDTOWorkBookItem(UserID, w.WorkBookObject, oModulePermissions, oTranslatedStatus, PortalName, AllComunitiesName)).ToList}).ToList
                End If
            End If
            Return oReturnList
        End Function
        Public Function GetDTOWorkBookItem(ByVal UserID As Integer, ByVal oWorkbook As WorkBook, ByVal oModulePermissions As List(Of ModuleCommunityPermission(Of ModuleWorkBook)), ByVal oTranslatedStatus As List(Of TranslatedItem(Of Integer)), ByVal PortalName As String, ByVal AllComunitiesName As String) As NEWdtoWorkbooks
            Dim iResponse As New NEWdtoWorkbooks
            Dim oPerson As iPerson = Me.GetPerson(UserID)
            Dim isWorkBookOwner As Boolean = (oWorkbook.Owner.Equals(oPerson))
            Dim isWorkBookAuthor As Boolean = oWorkbook.Authors.Contains(oPerson)
            Dim oPermission As New WorkBookPermission
            Dim Translated As String = ""
            Dim StatusID As Integer = 0

            Dim ModulePermission As ModuleWorkBook = Nothing
            If oWorkbook.CommunityOwner Is Nothing AndAlso oWorkbook.isPersonal Then
                ModulePermission = ModuleWorkBook.CreatePortalmodule
            ElseIf Not IsNothing(oWorkbook.CommunityOwner) Then
                ModulePermission = (From mp In oModulePermissions Where mp.ID = oWorkbook.CommunityOwner.Id Select mp.Permissions).FirstOrDefault
            End If
            If IsNothing(ModulePermission) Then
                ModulePermission = New ModuleWorkBook
            End If

            StatusID = oWorkbook.Status.Id
            Translated = (From o In oTranslatedStatus Where o.Id = StatusID Select o.Translation).FirstOrDefault
            Dim UserPermission As EditingPermission = EditingPermission.None
            If isWorkBookOwner Then
                UserPermission = UserPermission Or EditingPermission.Responsible
            End If
            If isWorkBookAuthor Then
                UserPermission = UserPermission Or EditingPermission.Authors
            End If
            If HasModuleWorkbookAdminPermission(ModulePermission) Then
                UserPermission = UserPermission Or EditingPermission.ModuleManager
                oPermission.ChangeEditing = True
            Else
                oPermission.ChangeEditing = ((UserPermission And oWorkbook.Editing) > 0)
            End If


            Dim isWorkBookEditing As Boolean = Not oWorkbook.isDeleted AndAlso ((UserPermission And oWorkbook.Editing) > 0)
            Dim isWorkBookStatusEnabled As Boolean = isWorkBookEditing
            If ModulePermission Is Nothing OrElse oWorkbook.isPersonal Then
                With oPermission
                    .AddItems = Not oWorkbook.isDeleted AndAlso isWorkBookEditing AndAlso (isWorkBookAuthor OrElse isWorkBookOwner)
                    .ChangeApprovation = (UserPermission > 0) ' MODIFICATO 31/03/2010 False
                    .EditWorkBook = isWorkBookOwner AndAlso Not oWorkbook.isDeleted
                    .CreateWorkBook = False
                    .DeleteWorkBook = isWorkBookOwner 'AndAlso isWorkBookEditing
                    ' VERIFICARE DI CERTO
                    .ReadWorkBook = (isWorkBookOwner OrElse isWorkBookAuthor)
                    .UndeleteWorkBook = isWorkBookOwner
                    .Admin = False
                End With
            Else
                With oPermission
                    .AddItems = (isWorkBookEditing AndAlso isWorkBookAuthor) OrElse (ModulePermission.Administration) OrElse ModulePermission.AddItemsToOther
                    .ChangeApprovation = Not oWorkbook.isDeleted AndAlso (ModulePermission.ChangeApprovation OrElse ModulePermission.Administration OrElse (UserPermission > 0))
                    .CreateWorkBook = ModulePermission.CreateGroupWorkBook OrElse ModulePermission.CreateWorkBook OrElse ModulePermission.Administration
                    .EditWorkBook = Not oWorkbook.isDeleted AndAlso ((isWorkBookStatusEnabled AndAlso isWorkBookOwner) OrElse (ModulePermission.Administration OrElse ModulePermission.ChangeApprovation))
                    .DeleteWorkBook = (isWorkBookStatusEnabled AndAlso isWorkBookOwner) OrElse (ModulePermission.Administration)
                    .ReadWorkBook = (isWorkBookOwner OrElse isWorkBookAuthor) OrElse ModulePermission.Administration OrElse ModulePermission.ChangeApprovation OrElse ModulePermission.ReadOtherWorkBook
                    .UndeleteWorkBook = isWorkBookOwner OrElse ModulePermission.Administration
                    .Admin = ModulePermission.Administration
                End With
            End If

            'iResponse.ChangeEditing = ((UserPermission And oItem.Editing) > 0) AndAlso (isItemOwner OrElse isWorkBookOwner)
            'Dim isItemEditing As Boolean = Not oItem.isDeleted AndAlso (oItem.Editing And UserPermission) > 0
            'Dim isItemStatusEnabled As Boolean = isItemEditing ' Not (oItem.MetaInfo.Approvation = MetaApprovationStatus.Approved OrElse oItem.MetaInfo.Approvation = MetaApprovationStatus.NotApproved)

            '' Dim isItemEditing As Boolean = Not oItem.isDeleted AndAlso oItem.MetaInfo.canModify

            iResponse = New NEWdtoWorkbooks(oPermission, oWorkbook, Translated, PortalName, GetWorkbookAvailableEditing(UserID, oWorkbook, ModulePermission), GetWorkbookAvailableStatus(UserID, Me.CurrentUserContext.Language, oWorkbook, ModulePermission))
            Return iResponse
        End Function
        Private Function NEW_GetWorkBooks(ByVal UserID As Integer, ByVal PortalName As String, ByVal AllComunitiesName As String, ByVal CommunitiesID As List(Of Integer), ByVal oType As WorkBookTypeFilter, ByVal oOrder As WorkBookOrder, ByVal oFilter As WorkBookCommunityFilter, ByVal oPager As lm.Comol.Core.DomainModel.PagerBase, Optional ByVal Record As ObjectStatus = ObjectStatus.Active) As List(Of dtoCommunityWorkbook)
            Dim oList As New List(Of dtoCommunityWorkbook)

            Try
                DC.BeginTransaction()
                Dim oPerson As Person = Me.GetPerson(UserID)
                ' AUTORE PROPRIETARIO
                Dim QueryResponsible = (From au In DC.GetCurrentSession.Linq(Of WorkBookAuthor)() Where au.Author Is oPerson AndAlso au.IsOwner AndAlso au.isDeleted = False Select au.WorkBookOwner.Id)
                ' AUTORE
                Dim QueryAuthors = (From au In DC.GetCurrentSession.Linq(Of WorkBookAuthor)() Where au.Author Is oPerson AndAlso au.isDeleted = False Select au.WorkBookOwner.Id)
                Dim Query = (From w In DC.GetCurrentSession.Linq(Of WorkBook)())
                Select Case oType
                    Case WorkBookTypeFilter.PersonalWorkBook
                        Query = Query.Where(Function(w) ((QueryAuthors.ToList.Contains(w.Id) AndAlso w.isDeleted = False) OrElse QueryResponsible.ToList.Contains(w.Id)) AndAlso w.isPersonal = True AndAlso ((oFilter = WorkBookCommunityFilter.Portal AndAlso w.CommunityOwner Is Nothing) _
                                    OrElse ((oFilter = WorkBookCommunityFilter.CurrentCommunity AndAlso CommunitiesID.Contains(w.CommunityOwner.Id)) _
                                    OrElse (oFilter = WorkBookCommunityFilter.AllCommunities))))
                    Case WorkBookTypeFilter.AssignedWorkBook
                        Query = Query.Where(Function(w) ((QueryAuthors.ToList.Contains(w.Id) AndAlso w.isDeleted = False) OrElse QueryResponsible.ToList.Contains(w.Id)) AndAlso w.isPersonal = False AndAlso (((oFilter = WorkBookCommunityFilter.CurrentCommunity AndAlso CommunitiesID.Contains(w.CommunityOwner.Id)) _
                                                        OrElse (oFilter = WorkBookCommunityFilter.AllCommunities))))
                    Case WorkBookTypeFilter.ManageWorkBook

                        ' NON SONO AUTORE
                        QueryAuthors = (From au In DC.GetCurrentSession.Linq(Of WorkBookAuthor)() Where au.Author Is oPerson AndAlso Not au.IsOwner Select au.WorkBookOwner.Id)
                        Query = Query.Where(Function(w) (QueryResponsible.ToList.Contains(w.Id) OrElse Not QueryAuthors.ToList.Contains(w.Id)) AndAlso w.isPersonal = False AndAlso (((oFilter = WorkBookCommunityFilter.CurrentCommunity AndAlso CommunitiesID.Contains(w.CommunityOwner.Id)) _
                                                        OrElse (oFilter = WorkBookCommunityFilter.AllCommunities AndAlso CommunitiesID.Contains(w.CommunityOwner.Id)))))
                End Select
                If oOrder = WorkBookOrder.Community Then
                    Dim oDtoCommunities = (From w In Query Select New dtoCommunityWorkbook(w.Id, w.CommunityOwner, PortalName)).ToList

                    Dim WorkbooksID As List(Of System.Guid) = (From o In oDtoCommunities Order By o.CommunityName Select o.WorkBookID).Skip(oPager.Skip).Take(oPager.PageSize).ToList
                    Dim Workbooks As List(Of WorkBook) = (From w In DC.GetCurrentSession.Linq(Of WorkBook)() Where WorkbooksID.Contains(w.Id) Select w).ToList

                    oList = (From w In Workbooks Join dto In oDtoCommunities On w.Id Equals dto.WorkBookID Order By dto.CommunityName, w.ModifiedOn Descending Select New dtoCommunityWorkbook(w, PortalName)).ToList
                Else
                    Select Case oOrder
                        Case WorkBookOrder.Title
                            Query = Query.OrderBy(Function(w) w.Title)
                        Case WorkBookOrder.CreatedOn
                            Query = Query.OrderByDescending(Function(w) w.CreatedOn)
                        Case WorkBookOrder.ChangedOn
                            Query = Query.OrderByDescending(Function(w) w.ModifiedOn)
                    End Select
                    oList = (From w In Query.Skip(oPager.Skip).Take(oPager.PageSize).ToList Select New dtoCommunityWorkbook(w, -999, AllComunitiesName)).ToList
                End If
                DC.Commit()
                Return oList
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                Debug.Write(ex.ToString)
            End Try

            Return oList
        End Function

        Private Function GetWorkbookAvailableEditing(ByVal UserID As Integer, ByVal oWorkBook As WorkBook, ByVal ModulePermission As ModuleWorkBook) As List(Of EditingSettings)
            Dim oAvailableEditing As New List(Of EditingSettings)
            Dim oPermission As EditingPermission = Me.GetWorkBookAvailablePermission(UserID, oWorkBook, ModulePermission)

            If (oPermission And EditingPermission.Authors) > 0 Then
                oAvailableEditing.Add(EditingSettings.AllAuthors)
            End If
            If (oPermission And EditingPermission.ModuleManager) > 0 Then
                oAvailableEditing.Add(EditingSettings.OnlyWorkbooksAdministrators)
            End If
            If (oPermission And EditingPermission.Responsible) > 0 Then
                oAvailableEditing.Add(EditingSettings.OnlyWorkbookResponsible)
            End If
            Return oAvailableEditing
        End Function

        Private Function GetWorkbookAvailableStatus(ByVal UserID As Integer, ByVal oLanguage As Language, ByVal oWorkBook As WorkBook, ByVal ModulePermission As ModuleWorkBook) As List(Of TranslatedItem(Of Integer))
            Return Me.GetTranslatedWorkBookStatusList(oLanguage, Me.GetWorkBookAvailablePermission(UserID, oWorkBook, ModulePermission))
        End Function
        ' MODIFICATO PER UTENTE COMPLETO AFFINCHE VEDA TUTTO
        Public Function GetWorkBookAvailablePermission(ByVal UserID As Integer, ByVal oWorkBook As WorkBook, ByVal ModulePermission As ModuleWorkBook) As EditingPermission
            Dim oEditingPermission As EditingPermission = EditingPermission.None
            Dim oPerson As iPerson = Me.GetPerson(UserID)
            Dim isWorkBookAuthor As Boolean = oWorkBook.Authors.Contains(oPerson)

            If isWorkBookAuthor OrElse (ModulePermission.AddItemsToOther OrElse ModulePermission.Administration) Then
                oEditingPermission = oEditingPermission Or EditingPermission.Authors
            End If

            If ModulePermission.AddItemsToOther OrElse ModulePermission.Administration Then
                oEditingPermission = oEditingPermission Or EditingPermission.ModuleManager
            End If
            If oWorkBook.Owner.Id = UserID OrElse (ModulePermission.AddItemsToOther OrElse ModulePermission.Administration) Then
                oEditingPermission = oEditingPermission Or EditingPermission.Responsible
            End If
            Return oEditingPermission
        End Function

        Public Function SaveWorkBooksStatus(ByVal oItems As List(Of GenericItemStatus(Of System.Guid, Integer)), ByVal ChangedBy As Integer) As List(Of GenericItemStatus(Of System.Guid, Integer))
            Dim oChangedList As New List(Of GenericItemStatus(Of System.Guid, Integer))
            Dim oChangedBy As Person = DC.GetById(Of Person)(ChangedBy)

            For Each oItemStatus As GenericItemStatus(Of System.Guid, Integer) In oItems
                Try
                    DC.BeginTransaction()
                    Dim oWorkBook As WorkBook = _Datacontext.GetCurrentSession.Get(Of WorkBook)(oItemStatus.Id)
                    If Not IsNothing(oWorkBook) AndAlso oWorkBook.Status.Id <> oItemStatus.Status Then
                        oWorkBook.ApprovedBy = oChangedBy
                        oWorkBook.ApprovedOn = Now
                        oWorkBook.ModifiedOn = oWorkBook.ApprovedOn
                        oWorkBook.ModifiedBy = oChangedBy
                        oWorkBook.Status = Me.DC.GetById(Of WorkBookStatus)(oItemStatus.Status)
                        DC.SaveOrUpdate(oWorkBook)
                    End If
                    DC.Commit()
                    If Not IsNothing(oWorkBook) Then
                        oChangedList.Add(oItemStatus)
                    End If
                Catch ex As Exception
                    If DC.isInTransaction Then
                        DC.Rollback()
                    End If
                End Try
            Next
            Return oChangedList
        End Function
        Public Function SaveWorkBooksEditing(ByVal oItems As List(Of GenericItemStatus(Of System.Guid, Integer)), ByVal ChangedBy As Integer) As List(Of GenericItemStatus(Of System.Guid, Integer))
            Dim oChangedList As New List(Of GenericItemStatus(Of System.Guid, Integer))
            Dim oChangedBy As Person = DC.GetById(Of Person)(ChangedBy)

            For Each oItemStatus As GenericItemStatus(Of System.Guid, Integer) In oItems
                Try
                    DC.BeginTransaction()
                    Dim oWorkBook As WorkBook = _Datacontext.GetCurrentSession.Get(Of WorkBook)(oItemStatus.Id)
                    If Not IsNothing(oWorkBook) AndAlso CInt(oWorkBook.Editing) <> oItemStatus.Status Then
                        oWorkBook.Editing = oItemStatus.Status
                        DC.SaveOrUpdate(oWorkBook)
                    End If
                    DC.Commit()
                    If Not IsNothing(oWorkBook) Then
                        oChangedList.Add(oItemStatus)
                    End If
                Catch ex As Exception
                    If DC.isInTransaction Then
                        DC.Rollback()
                    End If
                End Try
            Next
            Return oChangedList
        End Function

        Public Function NEW_VirtualDeleteWorkBook(ByVal WorkBookID As System.Guid, ByVal DeletedByID As Integer) As WorkBook
            Return SetVirtualDeleteToWorkBook(WorkBookID, DeletedByID, True)
        End Function
        Public Function NEW_UnDeleteVirtuaWorkBook(ByVal WorkBookID As System.Guid, ByVal DeletedByID As Integer) As WorkBook
            Return SetVirtualDeleteToWorkBook(WorkBookID, DeletedByID, False)
        End Function
        Public Sub NEW_VirtualDeleteWorkBooks(ByVal oWorkBooksID As List(Of System.Guid), ByVal DeletedByID As Integer)
            Me.SetVirtualDeleteToWorkBooks(oWorkBooksID, DeletedByID, True)
        End Sub
        Public Sub NEW_VirtualUndeleteItems(ByVal oWorkBooksID As List(Of System.Guid), ByVal DeletedByID As Integer)
            Me.SetVirtualDeleteToWorkBooks(oWorkBooksID, DeletedByID, False)
        End Sub
        Private Function SetVirtualDeleteToWorkBook(ByVal WorkBookID As System.Guid, ByVal DeletedByID As Integer, ByVal isdeleted As Boolean) As WorkBook
            Dim oPerson As Person = Me.GetPerson(DeletedByID)
            Dim oWorkBook As WorkBook = Me.NEW_GetWorkBook(WorkBookID)
            Try
                DC.BeginTransaction()
                If Not IsNothing(oPerson) AndAlso Not IsNothing(oWorkBook) Then
                    Dim DeletedOn As DateTime = Now
                    oWorkBook.ModifiedOn = DeletedOn
                    oWorkBook.ModifiedBy = oPerson
                    oWorkBook.isDeleted = isdeleted
                    DC.SaveOrUpdate(oWorkBook)
                End If
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                Return Nothing
            End Try
            Return oWorkBook
        End Function
        Private Sub SetVirtualDeleteToWorkBooks(ByVal oWorkBooksID As List(Of System.Guid), ByVal DeletedByID As Integer, ByVal isdeleted As Boolean)
            Dim oPerson As Person = Me.GetPerson(DeletedByID)
            Try
                DC.BeginTransaction()
                Dim WorkBooks As List(Of WorkBook) = (From wki As WorkBook In DC.GetCurrentSession.Linq(Of WorkBook)() Where oWorkBooksID.Contains(wki.Id) Select wki).ToList
                If Not IsNothing(oPerson) AndAlso Not IsNothing(WorkBooks) Then
                    Dim DeletedOn As DateTime = Now
                    For Each oItem In WorkBooks
                        oItem.ModifiedOn = DeletedOn
                        oItem.ModifiedBy = oPerson
                        oItem.isDeleted = isdeleted
                        DC.SaveOrUpdate(oItem)
                    Next
                End If
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
        End Sub

        Public Function NEW_DeleteWorkBook(ByVal WorkBookID As System.Guid, ByVal BaseUserRepositoryPath As String) As Boolean
            Dim oWorkBook As WorkBook = Me.NEW_GetWorkBook(WorkBookID)
            Return NEW_DeleteWorkBook(oWorkBook, BaseUserRepositoryPath)
        End Function
        Public Function NEW_DeleteWorkBook(ByVal oWorkBook As WorkBook, ByVal BaseUserRepositoryPath As String) As Boolean
            Dim iResponse As Boolean = False
            If Not IsNothing(oWorkBook) Then
                Try
                    DC.BeginTransaction()
                    Dim InternalFileNames As List(Of System.Guid) = (From f In Me.DC.GetCurrentSession.Linq(Of WorkBookInternalFile)() _
                                                                     Where f.WorkBookOwner Is oWorkBook Select f.File.Id).ToList
                    Dim AllFiles As List(Of WorkBookFile) = (From f In Me.DC.GetCurrentSession.Linq(Of WorkBookFile)() _
                                                             Where f.WorkBookOwner Is oWorkBook Select f).ToList

                    For Each oFile As WorkBookFile In AllFiles
                        DC.Delete(oFile)
                    Next
                    Dim AllItems As List(Of WorkBookItem) = (From f In Me.DC.GetCurrentSession.Linq(Of WorkBookItem)() _
                                                             Where f.WorkBookOwner Is oWorkBook Select f).ToList
                    For Each oItem As WorkBookItem In AllItems
                        DC.Delete(oItem)
                    Next
                    DC.Delete(oWorkBook)
                    DC.Commit()
                    iResponse = True
                    Delete.Files(BaseUserRepositoryPath, InternalFileNames, ".stored")
                Catch ex As Exception
                    Debug.Write(ex.ToString)
                    DC.Rollback()
                End Try
            End If
            Return iResponse
        End Function

#Region "Manage WorkBook"
        Public Function GetWorkBookPermission(ByVal PersonId As Integer, ByVal WorkBookID As System.Guid, ByVal ModulePermission As ModuleWorkBook) As WorkBookPermission
            Dim oWorkBook As WorkBook = Me.GetWorkBook(WorkBookID)
            If IsNothing(oWorkBook) Then
                Return New WorkBookPermission
            Else
                Return GetWorkBookPermission(PersonId, oWorkBook, ModulePermission)
            End If
        End Function
        Public Function GetWorkBookPermission(ByVal PersonId As Integer, ByVal oWorkBook As WorkBook, ByVal ModulePermission As ModuleWorkBook) As WorkBookPermission
            Dim iResponse As New WorkBookPermission
            Dim oPerson As iPerson = Me.GetPerson(PersonId)
            Dim isWorkBookOwner As Boolean = (oWorkBook.Owner.Equals(oPerson))
            Dim isWorkBookAuthor As Boolean = oWorkBook.Authors.Contains(oPerson)
            'Dim isWorkBookStatusEnabled As Boolean = Not (oWorkBook.MetaInfo.Approvation = MetaApprovationStatus.Approved OrElse oWorkBook.MetaInfo.Approvation = MetaApprovationStatus.NotApproved)
            'Dim isWorkBookEditing As Boolean = oWorkBook.MetaInfo.canModify AndAlso Not oWorkBook.MetaInfo.isDeleted

            Dim UserPermission As EditingPermission = EditingPermission.None
            If isWorkBookOwner Then
                UserPermission = UserPermission Or EditingPermission.Responsible
            End If
            If isWorkBookAuthor Then
                UserPermission = UserPermission Or EditingPermission.Authors
            End If
            If HasModuleWorkbookAdminPermission(ModulePermission) Then
                UserPermission = UserPermission Or EditingPermission.ModuleManager
                iResponse.ChangeEditing = True
            Else
                iResponse.ChangeEditing = ((UserPermission And oWorkBook.Editing) > 0)
            End If

            Dim isWorkBookStatusEnabled As Boolean = Not oWorkBook.isDeleted AndAlso (oWorkBook.Editing And UserPermission) > 0
            Dim isWorkBookEditing As Boolean = isWorkBookStatusEnabled ' Not (oItem.MetaInfo.Approvation = MetaApprovationStatus.Approved OrElse oItem.MetaInfo.Approvation = MetaApprovationStatus.NotApproved)
            If ModulePermission Is Nothing OrElse oWorkBook.isPersonal Then
                With iResponse
                    .AddItems = Not oWorkBook.isDeleted AndAlso isWorkBookEditing AndAlso (isWorkBookAuthor OrElse isWorkBookOwner)
                    .ChangeApprovation = (UserPermission > 0) ' MODIFICATO 31/03/2010 False
                    .EditWorkBook = isWorkBookOwner AndAlso Not oWorkBook.isDeleted
                    .CreateWorkBook = False
                    .DeleteWorkBook = isWorkBookOwner AndAlso isWorkBookEditing
                    ' VERIFICARE DI CERTO
                    .ReadWorkBook = (isWorkBookOwner OrElse isWorkBookAuthor)
                    .UndeleteWorkBook = isWorkBookOwner
                    .Admin = False
                End With
            Else
                With iResponse
                    .AddItems = (isWorkBookEditing AndAlso isWorkBookAuthor) OrElse (ModulePermission.Administration) OrElse ModulePermission.AddItemsToOther
                    .ChangeApprovation = Not oWorkBook.isDeleted AndAlso (ModulePermission.ChangeApprovation OrElse ModulePermission.Administration OrElse (UserPermission > 0))
                    .CreateWorkBook = ModulePermission.CreateGroupWorkBook OrElse ModulePermission.CreateWorkBook OrElse ModulePermission.Administration
                    .EditWorkBook = Not oWorkBook.isDeleted AndAlso ((isWorkBookStatusEnabled AndAlso isWorkBookOwner) OrElse (ModulePermission.Administration OrElse ModulePermission.ChangeApprovation))
                    .DeleteWorkBook = (isWorkBookStatusEnabled AndAlso isWorkBookOwner) OrElse (ModulePermission.Administration)
                    .ReadWorkBook = (isWorkBookOwner OrElse isWorkBookAuthor) OrElse ModulePermission.Administration OrElse ModulePermission.ChangeApprovation OrElse ModulePermission.ReadOtherWorkBook
                    .UndeleteWorkBook = isWorkBookOwner OrElse ModulePermission.Administration
                    .Admin = ModulePermission.Administration
                End With
            End If
            Return iResponse
        End Function


        'Public Function GetWorkBookAvailablePermission(ByVal PersonId As Integer, ByVal oWorkBook As WorkBook, ByVal ModulePermission As ModuleWorkBook) As EditingPermission
        '    Dim oPerson As iPerson = Me.GetPerson(PersonId)
        '    Dim isWorkBookOwner As Boolean = (oWorkBook.Owner.Equals(oPerson))
        '    Dim isWorkBookAuthor As Boolean = oWorkBook.Authors.Contains(oPerson)

        '    Dim UserPermission As EditingPermission = EditingPermission.None
        '    If isWorkBookOwner Then
        '        UserPermission = UserPermission Or EditingPermission.Responsible
        '    End If
        '    If isWorkBookAuthor Then
        '        UserPermission = UserPermission Or EditingPermission.Authors
        '    End If
        '    If HasModuleWorkbookAdminPermission(ModulePermission) Then
        '        UserPermission = UserPermission Or EditingPermission.ModuleManager
        '    End If

        '    Return UserPermission
        'End Function
#End Region
#Region "Manage File"
        Public Function NEW_WorkbookItemCommunityFiles(ByVal oItem As WorkBookItem) As List(Of WorkBookCommunityFile)
            Dim oList As New List(Of WorkBookCommunityFile)

            Try
                DC.BeginTransaction()

                oList = (From file In DC.GetCurrentSession.Linq(Of WorkBookCommunityFile)() Where file.ItemOwner Is oItem Select file Order By file.FileCommunity.Name).ToList

                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                Debug.Write(ex.ToString)
            End Try
            Return oList
        End Function
        Public Function NEW_WorkbookItemInternalFiles(ByVal oItem As WorkBookItem) As List(Of WorkBookInternalFile)
            Dim oList As New List(Of WorkBookInternalFile)

            Try
                DC.BeginTransaction()

                oList = (From file In DC.GetCurrentSession.Linq(Of WorkBookInternalFile)() Where file.ItemOwner Is oItem Select file).ToList

                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                Debug.Write(ex.ToString)
            End Try
            Return oList
        End Function
        Public Function NEW_WorkbookItemInternalFilesById(ByVal oFilesID As List(Of System.Guid)) As List(Of WorkBookInternalFile)
            Dim oList As New List(Of WorkBookInternalFile)

            Try
                DC.BeginTransaction()

                oList = (From file In DC.GetCurrentSession.Linq(Of WorkBookInternalFile)() Where oFilesID.Contains(file.Id) Select file).ToList

                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                Debug.Write(ex.ToString)
            End Try
            Return oList
        End Function
        Public Function NEW_WorkbookItemInternalFile(ByVal FileID As System.Guid) As WorkBookInternalFile
            Dim oFile As WorkBookInternalFile = Nothing

            Try
                DC.BeginTransaction()

                oFile = DC.GetCurrentSession.Get(Of WorkBookInternalFile)(FileID)

                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                Debug.Write(ex.ToString)
            End Try
            Return oFile
        End Function
        Public Function NEW_GetWorkBookItemDTOFiles(ByVal oItem As WorkBookItem, ByVal OnlyVisibleFiles As Boolean, ByVal oPermission As WorkBookItemPermission, ByVal oModule As ModuleCommunityRepository) As List(Of dtoWorkBookFile)
            Dim oList As New List(Of dtoWorkBookFile)

            Try
                Dim oInternalList As List(Of WorkBookInternalFile) = (From f In DC.GetCurrentSession.Linq(Of WorkBookInternalFile)() Where f.ItemOwner Is oItem AndAlso (Not OnlyVisibleFiles OrElse (OnlyVisibleFiles AndAlso f.isDeleted = False)) Select f).ToList

                If oInternalList.Count > 0 Then
                    oList = (From f In oInternalList Select New dtoWorkBookFile(f, oPermission, oItem.isDeleted, Not (oItem.WorkBookOwner.CommunityOwner Is Nothing))).ToList
                End If

                Dim oCommunityFiles As List(Of WorkBookCommunityFile) = (From f In DC.GetCurrentSession.Linq(Of WorkBookCommunityFile)() Where f.ItemOwner Is oItem AndAlso (Not OnlyVisibleFiles OrElse (OnlyVisibleFiles AndAlso f.isDeleted = False)) Select f).ToList
                If oCommunityFiles.Count > 0 Then
                    oList.AddRange((From f In oCommunityFiles Select New dtoWorkBookFile(f, oPermission, oModule, oItem.isDeleted)).ToList)
                End If
            Catch ex As Exception
                Debug.Write(ex.ToString)
            End Try
            'If oList.Count > 0 Then
            '    oList = (From o In oList Order By o.Mg odifiedOn descendin
            'End If
            Return oList.OrderByDescending(Function(dto) dto.ModifiedBy).ThenBy(Function(dto) dto.Name).ToList
        End Function
        Public Function NEW_GetWorkbookItemFile(ByVal WorkbookItemFileID As System.Guid) As WorkBookFile
            Dim oFile As WorkBookFile = Nothing

            Try
                DC.BeginTransaction()
                oFile = DC.GetById(Of WorkBookFile)(WorkbookItemFileID)

                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                Debug.Write(ex.ToString)
            End Try
            Return oFile
        End Function

        Public Function AddCommunityFilesToItem(ByVal ItemID As System.Guid, ByVal Files As List(Of Long), ByVal CreatedByID As Integer) As List(Of WorkBookCommunityFile)
            Dim oAddedFiles As New List(Of WorkBookCommunityFile)
            Dim oItem As WorkBookItem = Me.GetWorkBookItem(ItemID)
            Dim oPerson As Person = Me.GetPerson(CreatedByID)
            If Not IsNothing(oItem) Then
                Dim oModifyedOn As DateTime = Now
                For Each FileID As Long In Files
                    Try
                        DC.BeginTransaction()
                        Dim oFile As CommunityFile = DC.GetCurrentSession.Get(Of CommunityFile)(FileID)

                        Dim oWorkbookFile As New WorkBookCommunityFile
                        oWorkbookFile.FileCommunity = oFile
                        oWorkbookFile.Owner = oPerson
                        oWorkbookFile.CreatedBy = oPerson
                        oWorkbookFile.CreatedOn = oModifyedOn
                        oWorkbookFile.ItemOwner = oItem
                        oWorkbookFile.WorkBookOwner = oItem.WorkBookOwner
                        oWorkbookFile.Approvation = ApprovationStatus.Waiting
                        oWorkbookFile.isDeleted = False
                        DC.SaveOrUpdate(oWorkbookFile)
                        DC.Commit()
                        oAddedFiles.Add(oWorkbookFile)
                    Catch ex As Exception
                        If DC.isInTransaction Then
                            DC.Rollback()
                        End If
                    End Try
                Next
                oItem.ModifiedBy = oPerson
                oItem.ModifiedOn = oModifyedOn
                DC.SaveOrUpdate(oItem)
                oItem.WorkBookOwner.ModifiedOn = oModifyedOn
                oItem.WorkBookOwner.ModifiedBy = oPerson
                DC.SaveOrUpdate(oItem)
            End If
            Return oAddedFiles
        End Function
        Public Function AddInternalFilesToItem(ByVal ItemID As System.Guid, ByVal oFiles As List(Of lm.Comol.Core.DomainModel.BaseFile), ByVal CreatedByID As Integer) As List(Of WorkBookInternalFile)
            Dim oAddedFiles As New List(Of WorkBookInternalFile)
            Dim oItem As WorkBookItem = Me.GetWorkBookItem(ItemID)
            Dim oPerson As Person = Me.GetPerson(CreatedByID)
            If Not IsNothing(oItem) Then
                Dim oModifyedOn As DateTime = Now
                For Each oFile As BaseFile In oFiles
                    Try
                        DC.BeginTransaction()

                        Dim oWorkbookFile As New WorkBookInternalFile
                        oWorkbookFile.File = oFile
                        oWorkbookFile.Owner = oPerson
                        oWorkbookFile.CreatedBy = oPerson
                        oWorkbookFile.CreatedOn = oModifyedOn
                        oWorkbookFile.ItemOwner = oItem
                        oWorkbookFile.WorkBookOwner = oItem.WorkBookOwner
                        oWorkbookFile.Approvation = ApprovationStatus.Waiting
                        oWorkbookFile.isDeleted = False
                        DC.SaveOrUpdate(oWorkbookFile)
                        DC.Commit()
                        oAddedFiles.Add(oWorkbookFile)
                    Catch ex As Exception
                        If DC.isInTransaction Then
                            DC.Rollback()
                        End If
                    End Try
                Next
                oItem.ModifiedBy = oPerson
                oItem.ModifiedOn = oModifyedOn
                DC.SaveOrUpdate(oItem)
                oItem.WorkBookOwner.ModifiedOn = oModifyedOn
                oItem.WorkBookOwner.ModifiedBy = oPerson
                DC.SaveOrUpdate(oItem)
            End If
            Return oAddedFiles
        End Function

        Public Sub UnLinkToCommunityFileFromItem(ByVal WorkbookItemFileID As System.Guid)
            Try
                DC.BeginTransaction()

                Dim oFile As WorkBookCommunityFile = DC.GetById(Of WorkBookCommunityFile)(WorkbookItemFileID)
                If Not (IsNothing(oFile)) Then
                    DC.GetCurrentSession.Delete(oFile)
                End If
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
        End Sub
        Public Sub UnLinkToCommunityFileFromItem(ByVal ItemId As System.Guid, ByVal oFilesToRemove As List(Of WorkBookCommunityFile))
            Try
                For Each oFile As WorkBookCommunityFile In oFilesToRemove
                    Try
                        DC.BeginTransaction()
                        DC.GetCurrentSession.Delete(oFile)
                        DC.Commit()
                    Catch ex As Exception
                        If DC.isInTransaction Then
                            DC.Rollback()
                        End If
                    End Try
                Next
            Catch ex As Exception

            End Try
        End Sub

        Public Sub RemoveFileFromItem(ByVal FileID As System.Guid, ByVal BaseUserRepositoryPath As String)
            Dim oItemFile As WorkBookFile = Me.NEW_GetWorkbookItemFile(FileID)
            If Not IsNothing(oItemFile) Then
                Try
                    Dim isInternal As Boolean = False
                    Dim FileSystemPath As String = BaseUserRepositoryPath
                    DC.BeginTransaction()
                    If TypeOf oItemFile Is WorkBookCommunityFile Then
                        DC.GetCurrentSession.Delete(oItemFile)
                    Else
                        isInternal = True
                        FileSystemPath = DirectCast(oItemFile, WorkBookInternalFile).File.Id.ToString & ".stored"

                        DC.GetCurrentSession.Delete(DirectCast(oItemFile, WorkBookInternalFile).File)
                        DC.GetCurrentSession.Delete(oItemFile)
                    End If
                    DC.Commit()

                    If isInternal Then
                        Delete.File(FileSystemPath)
                    End If
                Catch ex As Exception
                    If DC.isInTransaction Then
                        DC.Rollback()
                    End If
                End Try
            End If
        End Sub
        Private Sub RemoveFilesFromItem(ByVal oFiles As List(Of WorkBookFile), ByVal BaseUserRepositoryPath As String)
            Dim RemoveFiles As New List(Of String)

            Try
                DC.BeginTransaction()
                For Each oItemFile As WorkBookFile In oFiles
                    Dim isInternal As Boolean = False
                    Dim FileSystemPath As String = BaseUserRepositoryPath

                    If TypeOf oItemFile Is WorkBookCommunityFile Then
                        DC.GetCurrentSession.Delete(oItemFile)
                    Else
                        isInternal = True
                        RemoveFiles.Add(DirectCast(oItemFile, WorkBookInternalFile).File.Id.ToString & ".stored")

                        DC.GetCurrentSession.Delete(DirectCast(oItemFile, WorkBookInternalFile).File)
                        DC.GetCurrentSession.Delete(oItemFile)
                    End If
                Next
                DC.Commit()
                Delete.Files(RemoveFiles)
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
        End Sub

        Private Function GetGenericWorkBookFile(ByVal FileID As System.Guid) As WorkBookFile
            Dim oFile As WorkBookFile
            Try
                oFile = _Datacontext.GetById(Of WorkBookFile)(FileID)
            Catch ex As Exception
                Debug.Write(ex.ToString)

                Return Nothing
            End Try
            Return oFile
        End Function

        Public Sub VirtualDeleteFileFromItem(ByVal ItemFileID As System.Guid, ByVal DeletedByID As Integer)
            SetVirtualDeleteToFile(ItemFileID, DeletedByID, True)
        End Sub
        Public Sub VirtualUnDeleteFileFromItem(ByVal ItemFileID As System.Guid, ByVal DeletedByID As Integer)
            SetVirtualDeleteToFile(ItemFileID, DeletedByID, False)
        End Sub
        Public Sub VirtualDeleteFilesFromItem(ByVal oFilesID As List(Of System.Guid), ByVal DeletedByID As Integer)
            Me.SetVirtualDeleteToFiles(oFilesID, DeletedByID, True)
        End Sub
        Public Sub VirtualFilesFromItem(ByVal oFilesID As List(Of System.Guid), ByVal DeletedByID As Integer)
            Me.SetVirtualDeleteToFiles(oFilesID, DeletedByID, False)
        End Sub
        Private Sub SetVirtualDeleteToFile(ByVal ItemFileID As System.Guid, ByVal DeletedByID As Integer, ByVal isdeleted As Boolean)
            Dim oPerson As Person = Me.GetPerson(DeletedByID)
            Dim oFile As WorkBookFile = Me.GetGenericWorkBookFile(ItemFileID)
            Try
                DC.BeginTransaction()
                If Not IsNothing(oPerson) AndAlso Not IsNothing(oFile) Then
                    Dim DeletedOn As DateTime = Now
                    oFile.ModifiedOn = DeletedOn
                    oFile.ModifiedBy = oPerson
                    oFile.isDeleted = isdeleted
                    DC.SaveOrUpdate(oFile)
                End If
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
        End Sub
        Private Sub SetVirtualDeleteToFiles(ByVal oFilesID As List(Of System.Guid), ByVal DeletedByID As Integer, ByVal isdeleted As Boolean)
            Dim oPerson As Person = Me.GetPerson(DeletedByID)
            Try
                DC.BeginTransaction()
                Dim oFiles As List(Of WorkBookFile) = (From f As WorkBookFile In DC.GetCurrentSession.Linq(Of WorkBookFile)() Where oFilesID.Contains(f.Id) Select f).ToList
                If Not IsNothing(oPerson) AndAlso Not IsNothing(oFiles) Then
                    Dim DeletedOn As DateTime = Now
                    For Each oFile In oFiles
                        oFile.ModifiedOn = DeletedOn
                        oFile.ModifiedBy = oPerson
                        oFile.isDeleted = isdeleted
                        DC.SaveOrUpdate(oFile)
                    Next
                End If
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
        End Sub
#End Region

        Public Function GetItemAvailablePermission(ByVal oWorkBook As WorkBook, ByVal oModulePermission As ModuleWorkBook) As EditingPermission
            Dim oEditingPermission As EditingPermission
            oEditingPermission = EditingPermission.Authors Or EditingPermission.Owner

            If oModulePermission.AddItemsToOther OrElse oModulePermission.Administration Then
                oEditingPermission = oEditingPermission Or EditingPermission.ModuleManager
            End If

            If oWorkBook.Owner.Id = _UserContext.CurrentUserID Then
                oEditingPermission = oEditingPermission Or EditingPermission.Responsible
            End If
            Return oEditingPermission
        End Function
#Region "Manage Items"

        Public Function GetWorkBookItem(ByVal ItemID As System.Guid) As WorkBookItem
            Dim oItem As WorkBookItem = Nothing
            Try
                DC.BeginTransaction()
                oItem = _Datacontext.GetCurrentSession.Get(Of WorkBookItem)(ItemID)
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
            Return oItem
        End Function
        Public Function GetWorkBookItemPermission(ByVal PersonId As Integer, ByVal oItem As WorkBookItem, ByVal ModulePermission As ModuleWorkBook) As WorkBookItemPermission
            Dim iResponse As New WorkBookItemPermission
            Dim oWorkBook As WorkBook = oItem.WorkBookOwner
            Dim oPerson As iPerson = Me.GetPerson(PersonId)

            Dim isWorkBookOwner As Boolean = (oWorkBook.Owner.Equals(oPerson))
            Dim isWorkBookAuthor As Boolean = oWorkBook.Authors.Contains(oPerson)
            'Dim isWorkBookStatusEnabled As Boolean = Not (oWorkBook.MetaInfo.Approvation = MetaApprovationStatus.Approved OrElse oWorkBook.MetaInfo.Approvation = MetaApprovationStatus.NotApproved)
            'Dim isWorkBookEditing As Boolean = oWorkBook.MetaInfo.canModify AndAlso Not oWorkBook.isDeleted

            Dim isWorkBookEditing As Boolean = ((Me.GetWorkBookAvailablePermission(PersonId, oWorkBook, ModulePermission) And oWorkBook.Editing) > 0) AndAlso Not oWorkBook.isDeleted
            Dim isWorkBookStatusEnabled As Boolean = isWorkBookEditing

            Dim WorkBookOwner As Person = oWorkBook.Owner
            '  Dim isItemStatusEnabled As Boolean = Not (oItem.MetaInfo.Approvation = MetaApprovationStatus.Approved OrElse oItem.MetaInfo.Approvation = MetaApprovationStatus.NotApproved)
            Dim isItemOwner As Boolean = False
            If IsNothing(oItem.Owner) = False Then
                isItemOwner = oItem.Owner.Equals(oPerson)
            End If
            ' Dim isItemEditing As Boolean = Not oItem.isDeleted AndAlso oItem.MetaInfo.canModify

            Dim UserPermission As EditingPermission = EditingPermission.None
            If isWorkBookOwner Then
                UserPermission = UserPermission Or EditingPermission.Responsible
            End If
            If isItemOwner Then
                UserPermission = UserPermission Or EditingPermission.Owner
            End If
            If isWorkBookAuthor Then
                UserPermission = UserPermission Or EditingPermission.Authors
            End If
            If HasModuleWorkbookAdminPermission(ModulePermission) Then
                UserPermission = UserPermission Or EditingPermission.ModuleManager
                iResponse.ChangeEditing = ((UserPermission And oItem.Editing) > 0)
            ElseIf oItem.Owner.Equals(WorkBookOwner) AndAlso Not isItemOwner Then
                iResponse.ChangeEditing = False
            Else
                iResponse.ChangeEditing = ((UserPermission And oItem.Editing) > 0)
            End If

            Dim isItemEditing As Boolean = Not oItem.isDeleted AndAlso (oItem.Editing And UserPermission) > 0
            Dim isItemStatusEnabled As Boolean = isItemEditing ' Not (oItem.MetaInfo.Approvation = MetaApprovationStatus.Approved OrElse oItem.MetaInfo.Approvation = MetaApprovationStatus.NotApproved)

            iResponse.ChangeEditing = ((UserPermission And oItem.Editing) > 0)
            If oWorkBook.isPersonal Then
                '' Posso vedere la voce 
                'iResponse.Read = (isWorkBookOwner OrElse isWorkBookAuthor) 'OrElse ModulePermission.ReadOtherWorkBook OrElse ModulePermission.Administration OrElse ModulePermission.ChangeApprovation

                '' Posso scaricare i file
                'iResponse.DownLoadFile = Not oItem.isDeleted AndAlso (isWorkBookOwner OrElse isWorkBookAuthor) ' OrElse ModulePermission.DownLoadItemFiles OrElse ModulePermission.Administration OrElse ModulePermission.ChangeApprovation)

                '' Modifica Elemento
                ''1) deve essere possibile l'editing e se sono il proprietario
                'iResponse.Write = Not oItem.isDeleted AndAlso (isItemStatusEnabled AndAlso isWorkBookStatusEnabled AndAlso isWorkBookEditing) AndAlso (isWorkBookOwner OrElse (isWorkBookAuthor AndAlso oItem.Owner.Equals(oWorkBook.Owner) = False))
                ''2) deve essere possibile se amministro
                '' oPermission.Write = oPermission.Write OrElse ModulePermission.Administration

                '' CANCELLA elemento
                'iResponse.Delete = (isWorkBookStatusEnabled AndAlso isItemStatusEnabled) AndAlso (isWorkBookAuthor OrElse isItemOwner) 'OrElse ModulePermission.DeleteItemsFromOther) OrElse (ModulePermission.Administration AndAlso isWorkBookStatusEnabled AndAlso isWorkBookEditing)
                '' Cambia Approvazione
                'iResponse.ChangeApprovation = Not oItem.isDeleted AndAlso (isWorkBookEditing AndAlso isWorkBookStatusEnabled AndAlso ModulePermission.ChangeApprovation) ' OrElse ModulePermission.Administration)
                'iResponse.UnDelete = ((isItemOwner OrElse isWorkBookOwner) AndAlso isWorkBookStatusEnabled AndAlso isWorkBookEditing) ' OrElse (ModulePermission.Administration AndAlso isWorkBookStatusEnabled AndAlso isWorkBookEditing)

                'iResponse.ViewPersonalNote = True 'isItemOwner
                ' Posso vedere la voce 
                iResponse.Read = (isWorkBookOwner OrElse isWorkBookAuthor) 'OrElse ModulePermission.ReadOtherWorkBook OrElse ModulePermission.Administration OrElse ModulePermission.ChangeApprovation

                ' Posso scaricare i file
                iResponse.DownLoadFile = Not oItem.isDeleted AndAlso (isWorkBookOwner OrElse isWorkBookAuthor) ' OrElse ModulePermission.DownLoadItemFiles OrElse ModulePermission.Administration OrElse ModulePermission.ChangeApprovation)

                ' Modifica Elemento
                '1) deve essere possibile l'editing e se sono il proprietario
                iResponse.Write = Not oItem.isDeleted AndAlso (isItemStatusEnabled AndAlso isWorkBookStatusEnabled AndAlso isWorkBookEditing) AndAlso (isWorkBookOwner OrElse (isWorkBookAuthor AndAlso oItem.Owner.Equals(WorkBookOwner) = False))
                '2) deve essere possibile se amministro
                ' oPermission.Write = oPermission.Write OrElse ModulePermission.Administration

                ' CANCELLA elemento
                iResponse.Delete = (isWorkBookStatusEnabled AndAlso isItemStatusEnabled) AndAlso ((isWorkBookAuthor AndAlso oItem.Owner.Equals(WorkBookOwner) = False) OrElse isItemOwner) 'OrElse ModulePermission.DeleteItemsFromOther) OrElse (ModulePermission.Administration AndAlso isWorkBookStatusEnabled AndAlso isWorkBookEditing)
                ' Cambia Approvazione
                iResponse.ChangeApprovation = Not oItem.isDeleted AndAlso (isWorkBookEditing AndAlso isWorkBookStatusEnabled) AndAlso iResponse.ChangeEditing AndAlso (oItem.Status.AvailableFor And UserPermission) > 0 ' RIMOSSO MARZO 2010 AndAlso ModulePermission.ChangeApprovation)


                ' OrElse ModulePermission.Administration)
                iResponse.UnDelete = ((isItemOwner OrElse isWorkBookOwner) AndAlso isWorkBookStatusEnabled AndAlso isWorkBookEditing) ' OrElse (ModulePermission.Administration AndAlso isWorkBookStatusEnabled AndAlso isWorkBookEditing)

                iResponse.ViewPersonalNote = True ' isItemOwner
            Else
                ' Posso vedere la voce 
                iResponse.Read = (isWorkBookOwner OrElse isWorkBookAuthor) OrElse ModulePermission.ReadOtherWorkBook OrElse ModulePermission.Administration OrElse ModulePermission.ChangeApprovation

                ' Posso scaricare i file
                iResponse.DownLoadFile = Not oItem.isDeleted AndAlso (isWorkBookOwner OrElse isWorkBookAuthor OrElse ModulePermission.DownLoadItemFiles OrElse ModulePermission.Administration OrElse ModulePermission.ChangeApprovation)

                ' Modifica Elemento
                '1) deve essere possibile l'editing e se sono il proprietario
                iResponse.Write = Not oItem.isDeleted AndAlso (isItemStatusEnabled AndAlso isWorkBookStatusEnabled AndAlso isWorkBookEditing) AndAlso (isWorkBookOwner OrElse (isWorkBookAuthor AndAlso oItem.Owner.Equals(WorkBookOwner) = False)) '(isWorkBookAuthor AndAlso oItem.Owner.Equals(WorkBookOwner) = False))
                '2) deve essere possibile se amministro
                iResponse.Write = iResponse.Write OrElse ModulePermission.Administration

                ' CANCELLA elemento
                iResponse.Delete = (isWorkBookStatusEnabled AndAlso isItemStatusEnabled) AndAlso ((isWorkBookAuthor AndAlso oItem.Owner.Equals(WorkBookOwner) = False) OrElse isItemOwner OrElse ModulePermission.DeleteItemsFromOther) OrElse (ModulePermission.Administration AndAlso isWorkBookStatusEnabled AndAlso isWorkBookEditing)
                ' Cambia Approvazione
                ' VERSIONE MARZO 2010 iResponse.ChangeApprovation = Not oItem.isDeleted AndAlso ((isWorkBookEditing AndAlso isWorkBookStatusEnabled AndAlso ModulePermission.ChangeApprovation) OrElse ModulePermission.Administration)

                iResponse.ChangeApprovation = Not oItem.isDeleted AndAlso ((isWorkBookEditing AndAlso isWorkBookStatusEnabled) OrElse ModulePermission.Administration) AndAlso iResponse.ChangeEditing AndAlso (oItem.Status.AvailableFor And UserPermission) > 0

                iResponse.UnDelete = ((isItemOwner OrElse isWorkBookOwner OrElse ModulePermission.DeleteItemsFromOther) AndAlso isWorkBookStatusEnabled AndAlso isWorkBookEditing) OrElse (ModulePermission.Administration AndAlso isWorkBookStatusEnabled AndAlso isWorkBookEditing)
                iResponse.ViewPersonalNote = True ' isItemOwner
            End If
            Return iResponse
        End Function
        Public Function GetDTOWorkBookItem(ByVal oPerson As iPerson, ByVal isWorkBookOwner As Boolean, ByVal isWorkBookAuthor As Boolean, ByVal isWorkBookStatusEnabled As Boolean, ByVal isWorkBookEditing As Boolean, ByVal isWorkBookPersonal As Boolean, ByVal WorkBookOwner As Person, ByVal oItem As WorkBookItem, ByVal ModulePermission As ModuleWorkBook, ByVal iStatusTranslated As String) As dtoWorkBookItem
            Dim iResponse As New WorkBookItemPermission
            '' Dim isItemStatusEnabled As Boolean = Not (oItem.MetaInfo.Approvation = MetaApprovationStatus.Approved OrElse oItem.MetaInfo.Approvation = MetaApprovationStatus.NotApproved)
            Dim isItemOwner As Boolean = False
            If IsNothing(oItem.Owner) = False Then
                isItemOwner = oItem.Owner.Equals(oPerson)
            End If



            Dim UserPermission As EditingPermission = EditingPermission.None
            If isWorkBookOwner Then
                UserPermission = UserPermission Or EditingPermission.Responsible
            End If
            If isItemOwner Then
                UserPermission = UserPermission Or EditingPermission.Owner
            End If
            If isWorkBookAuthor Then
                UserPermission = UserPermission Or EditingPermission.Authors
            End If
            ' MODIFICATO PER SARA
            If HasModuleWorkbookAdminPermission(ModulePermission) Then
                UserPermission = UserPermission Or EditingPermission.ModuleManager
                iResponse.ChangeEditing = ((UserPermission And oItem.Editing) > 0)
            ElseIf oItem.Owner.Equals(WorkBookOwner) AndAlso Not isItemOwner Then
                iResponse.ChangeEditing = False
            Else
                iResponse.ChangeEditing = ((UserPermission And oItem.Editing) > 0)
            End If
            iResponse.ChangeEditing = iResponse.ChangeEditing AndAlso isWorkBookEditing
            ' iResponse.ChangeEditing = ((UserPermission And oItem.Editing) > 0) 'AndAlso (isItemOwner OrElse isWorkBookOwner)
            Dim isItemEditing As Boolean = Not oItem.isDeleted AndAlso (oItem.Editing And UserPermission) > 0
            Dim isItemDeleting As Boolean = oItem.isDeleted AndAlso (oItem.Editing And UserPermission) > 0
            Dim isItemStatusEnabled As Boolean = isItemEditing '' Not (oItem.MetaInfo.Approvation = MetaApprovationStatus.Approved OrElse oItem.MetaInfo.Approvation = MetaApprovationStatus.NotApproved)


            '' Dim isItemEditing As Boolean = Not oItem.isDeleted AndAlso oItem.MetaInfo.canModify

            If isWorkBookPersonal Then
                ' Posso vedere la voce 
                iResponse.Read = (isWorkBookOwner OrElse isWorkBookAuthor) ''OrElse ModulePermission.ReadOtherWorkBook OrElse ModulePermission.Administration OrElse ModulePermission.ChangeApprovation

                ' Posso scaricare i file
                iResponse.DownLoadFile = Not oItem.isDeleted AndAlso (isWorkBookOwner OrElse isWorkBookAuthor) '' OrElse ModulePermission.DownLoadItemFiles OrElse ModulePermission.Administration OrElse ModulePermission.ChangeApprovation)

                ' Modifica Elemento
                '1) deve essere possibile l'editing e se sono il proprietario
                iResponse.Write = Not oItem.isDeleted AndAlso (isItemStatusEnabled AndAlso isWorkBookStatusEnabled AndAlso isWorkBookEditing) AndAlso (isWorkBookOwner OrElse isWorkBookAuthor) ' MODIFICA SARA (isWorkBookAuthor AndAlso oItem.Owner.Equals(WorkBookOwner) = False))
                '2) deve essere possibile se amministro
                ' oPermission.Write = oPermission.Write OrElse ModulePermission.Administration

                ' CANCELLA elemento
                iResponse.Delete = (isWorkBookStatusEnabled AndAlso isItemDeleting) AndAlso ((isWorkBookAuthor AndAlso oItem.Owner.Equals(WorkBookOwner) = False) OrElse isItemOwner) ''OrElse ModulePermission.DeleteItemsFromOther) OrElse (ModulePermission.Administration AndAlso isWorkBookStatusEnabled AndAlso isWorkBookEditing)
                ' Cambia Approvazione
                iResponse.ChangeApprovation = Not oItem.isDeleted AndAlso (isWorkBookEditing AndAlso isWorkBookStatusEnabled) AndAlso iResponse.ChangeEditing AndAlso (oItem.Status.AvailableFor And UserPermission) > 0 '' RIMOSSO MARZO 2010 AndAlso ModulePermission.ChangeApprovation)


                ' OrElse ModulePermission.Administration)
                iResponse.UnDelete = ((isItemOwner OrElse isWorkBookOwner) AndAlso isWorkBookStatusEnabled AndAlso isWorkBookEditing) '' OrElse (ModulePermission.Administration AndAlso isWorkBookStatusEnabled AndAlso isWorkBookEditing)

                iResponse.ViewPersonalNote = True ' isItemOwner

            Else
                ' Posso vedere la voce 
                iResponse.Read = (isWorkBookOwner OrElse isWorkBookAuthor) OrElse ModulePermission.ReadOtherWorkBook OrElse ModulePermission.Administration OrElse ModulePermission.ChangeApprovation

                ' Posso scaricare i file
                iResponse.DownLoadFile = Not oItem.isDeleted AndAlso (isWorkBookOwner OrElse isWorkBookAuthor OrElse ModulePermission.DownLoadItemFiles OrElse ModulePermission.Administration OrElse ModulePermission.ChangeApprovation)

                ' Modifica Elemento
                '1) deve essere possibile l'editing e se sono il proprietario
                iResponse.Write = Not oItem.isDeleted AndAlso (isItemStatusEnabled AndAlso isWorkBookStatusEnabled AndAlso isWorkBookEditing) AndAlso (isWorkBookOwner OrElse isWorkBookAuthor) ' MODIFICA SARA(isWorkBookAuthor AndAlso oItem.Owner.Equals(WorkBookOwner) = False)) ''(isWorkBookAuthor AndAlso oItem.Owner.Equals(WorkBookOwner) = False))
                '2) deve essere possibile se amministro
                iResponse.Write = iResponse.Write OrElse ModulePermission.Administration

                ' CANCELLA elemento
                iResponse.Delete = (isWorkBookStatusEnabled AndAlso isItemDeleting) AndAlso ((isWorkBookAuthor AndAlso oItem.Owner.Equals(WorkBookOwner) = False) OrElse isItemOwner OrElse ModulePermission.DeleteItemsFromOther) OrElse (ModulePermission.Administration AndAlso isWorkBookStatusEnabled AndAlso isWorkBookEditing)
                ' Cambia Approvazione
                ' VERSIONE MARZO 2010 iResponse.ChangeApprovation = Not oItem.isDeleted AndAlso ((isWorkBookEditing AndAlso isWorkBookStatusEnabled AndAlso ModulePermission.ChangeApprovation) OrElse ModulePermission.Administration)

                iResponse.ChangeApprovation = Not oItem.isDeleted AndAlso ((isWorkBookEditing AndAlso isWorkBookStatusEnabled) OrElse ModulePermission.Administration) AndAlso iResponse.ChangeEditing AndAlso (oItem.Status.AvailableFor And UserPermission) > 0

                iResponse.UnDelete = ((isItemOwner OrElse isWorkBookOwner OrElse ModulePermission.DeleteItemsFromOther) AndAlso isWorkBookStatusEnabled AndAlso isWorkBookEditing) OrElse (ModulePermission.Administration AndAlso isWorkBookStatusEnabled AndAlso isWorkBookEditing)
                iResponse.ViewPersonalNote = True ' isItemOwner
            End If
            Return New dtoWorkBookItem(iResponse, oItem, iStatusTranslated) 'oHeaderTitle,
        End Function
        Public Function GetWorkBookItemsListWithPermission(ByVal oUser As iPerson, ByVal oWorkBook As WorkBook, ByVal ModulePermission As ModuleWorkBook, ByVal Ascending As Boolean, ByVal ItemVisibility As ObjectStatus, ByVal oLanguage As Language) As List(Of dtoWorkBookItem)
            Dim oPerson As iPerson = Me.GetPerson(oUser.Id)
            Dim oReturnList As List(Of dtoWorkBookItem) = New List(Of dtoWorkBookItem)
            Dim oList As List(Of WorkBookItem) = Me.NEW_GetWorkBookItems(oWorkBook, Ascending, oUser, ItemVisibility)
            If Not IsNothing(oList) AndAlso oList.Count > 0 Then
                Dim isWorkBookOwner As Boolean = (oWorkBook.Owner.Equals(oPerson))
                Dim isWorkBookAuthor As Boolean = oWorkBook.Authors.Contains(oPerson)
                'Dim isWorkBookStatusEnabled As Boolean = Not (oWorkBook.MetaInfo.Approvation = MetaApprovationStatus.Approved OrElse oWorkBook.MetaInfo.Approvation = MetaApprovationStatus.NotApproved)
                'Dim isWorkBookEditing As Boolean = oWorkBook.MetaInfo.canModify AndAlso Not oWorkBook.MetaInfo.isDeleted


                Dim isWorkBookEditing As Boolean = ((Me.GetWorkBookAvailablePermission(oUser.Id, oWorkBook, ModulePermission) And oWorkBook.Editing) > 0) AndAlso Not oWorkBook.isDeleted
                Dim isWorkBookStatusEnabled As Boolean = isWorkBookEditing



                '  Dim oWorkBookEditing As EditingPermission = Me.GetItemAvailablePermission(oWorkBook, ModulePermission)
                Dim oTranslatedStatus As List(Of TranslatedItem(Of Integer)) = Me.GetTranslatedWorkBookStatusList(oLanguage)
                Dim Translated As String = ""
                Dim StatusID As Integer = 0
                For Each oItem In oList
                    StatusID = oItem.Status.Id
                    Translated = (From o In oTranslatedStatus Where o.Id = StatusID Select o.Translation).FirstOrDefault
                    oReturnList.Add(Me.GetDTOWorkBookItem(oPerson, isWorkBookOwner, isWorkBookAuthor, isWorkBookStatusEnabled, isWorkBookEditing, oWorkBook.isPersonal, oWorkBook.Owner, oItem, ModulePermission, Translated))
                Next
            End If
            Return oReturnList
        End Function
        Private Function NEW_GetWorkBookItems(ByVal oWorkBook As WorkBook, ByVal Ascending As Boolean, ByVal oUser As iPerson, ByVal ItemVisibility As ObjectStatus) As List(Of WorkBookItem)
            Dim oList As List(Of WorkBookItem) = Nothing
            Try
                DC.BeginTransaction()

                Dim Query = (From wki In DC.GetCurrentSession.Linq(Of WorkBookItem)() Where wki.WorkBookOwner Is oWorkBook _
                             AndAlso ((ItemVisibility = ObjectStatus.All AndAlso (wki.isDraft = False OrElse (wki.isDraft AndAlso wki.Owner.Id = oUser.Id))) _
                             OrElse (ItemVisibility = ObjectStatus.Active AndAlso (wki.isDeleted = False) AndAlso (wki.isDraft = False OrElse (wki.isDraft AndAlso wki.Owner.Id = oUser.Id))) _
                             OrElse (ItemVisibility = ObjectStatus.AllbutOnlyMyDeleted AndAlso (wki.isDeleted = False OrElse (wki.isDeleted AndAlso wki.Owner.Id = oUser.Id)) AndAlso (wki.isDraft = False OrElse (wki.isDraft AndAlso wki.Owner.Id = oUser.Id))) _
                             ) Select wki)
                If Ascending Then
                    Query = Query.OrderBy(Function(c) c.StartDate).ThenBy(Function(c) c.CreatedOn)
                Else
                    Query = Query.OrderByDescending(Function(c) c.StartDate).ThenByDescending(Function(c) c.CreatedOn)
                End If
                oList = Query.ToList
                DC.Commit()

            Catch ex As Exception
                Debug.Write(ex.ToString)
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                oList = New List(Of WorkBookItem)
            End Try
            Return oList
        End Function
        Public Function NEW_SaveWorkBookItem(ByVal CreatedByID As Integer, ByVal OwnerID As Integer, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal oUnsavedItem As WorkBookItem) As WorkBookItem
            Dim oWorkBookItem As New WorkBookItem
            Dim oMeta As New MetaData

            Try
                DC.BeginTransaction()
                Dim oCreatedBy As Person = DC.GetById(Of Person)(CreatedByID)
                Dim oCommunity As Community = DC.GetById(Of Community)(CommunityID)
                Dim oOwner As Person = DC.GetById(Of Person)(OwnerID)
                Dim oWorkBook As WorkBook = DC.GetById(Of WorkBook)(WorkBookID)

                If Not IsNothing(oOwner) AndAlso Not IsNothing(oCreatedBy) AndAlso Not IsNothing(oWorkBook) Then
                    If oUnsavedItem.Id = System.Guid.Empty Then
                        oWorkBookItem.Owner = oOwner
                        oWorkBookItem.CreatedBy = oCreatedBy
                        oWorkBookItem.CreatedOn = Now
                        oWorkBookItem.ModifiedBy = oCreatedBy
                        oWorkBookItem.ModifiedOn = oWorkBookItem.CreatedOn
                        oWorkBookItem.WorkBookOwner = oWorkBook
                    Else
                        oWorkBookItem = DC.GetById(Of WorkBookItem)(oUnsavedItem.Id)
                        oWorkBookItem.ModifiedBy = oCreatedBy
                        oWorkBookItem.ModifiedOn = Now
                    End If

                    With oWorkBookItem
                        '.CommunityOwner = oCommunity
                        .Owner = oOwner
                        .Note = oUnsavedItem.Note
                        .Body = oUnsavedItem.Body
                        .Title = oUnsavedItem.Title
                        .Status = oUnsavedItem.Status
                        .Editing = oUnsavedItem.Editing

                        .StartDate = oUnsavedItem.StartDate
                        .EndDate = oUnsavedItem.EndDate
                        .isDraft = oUnsavedItem.isDraft
                    End With

                    If oWorkBook.Items Is Nothing Then
                        oWorkBook.Items = New List(Of WorkBookItem)
                    End If
                    oWorkBook.Items.Add(oWorkBookItem)
                    'c'è un problema di salvataggio dei metadata, forse qualche rogna con i null o date etc
                    'dobbiamo cmq provare a creare prima il meta info come variabile a se, e poi associarlo al diario
                    'cmq le transaction sembra funzionino, se commento la riga sul metainfo, il diario viene salvato su db
                    'mentre se c'è errore sulla riga del metainfo, errore che appare solo al commit, giustamente il diario
                    'non viene salvato

                    oWorkBook.ModifiedBy = oWorkBookItem.ModifiedBy
                    oWorkBook.ModifiedOn = oWorkBookItem.ModifiedOn
                    DC.SaveOrUpdate(oWorkBookItem)
                    DC.SaveOrUpdate(oWorkBook)
                Else
                    oWorkBookItem = Nothing
                End If

                DC.Commit()
            Catch ex As Exception
                Debug.Write(ex.ToString)
                DC.Rollback()
                Return Nothing
            End Try
            Return oWorkBookItem
        End Function
        Public Function NEW_GetWorkBook(ByVal WorkBookID As System.Guid) As WorkBook
            Dim oWorkBook As WorkBook = Nothing
            Try
                DC.BeginTransaction()
                oWorkBook = DC.GetById(Of WorkBook)(WorkBookID)
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                Debug.Write(ex.ToString)

                Return Nothing
            End Try
            Return oWorkBook
        End Function
        Public Function NEW_VirtualDeleteWorkBookItem(ByVal ItemID As System.Guid, ByVal DeletedByID As Integer) As WorkBookItem
            Return SetVirtualDeleteToItem(ItemID, DeletedByID, True)
        End Function
        Public Function NEW_UnDeleteVirtuaWorkBookItem(ByVal ItemID As System.Guid, ByVal DeletedByID As Integer) As WorkBookItem
            Return SetVirtualDeleteToItem(ItemID, DeletedByID, False)
        End Function
        Public Sub NEW_VirtualDeleteItems(ByVal WorkBookId As System.Guid, ByVal oItemsID As List(Of System.Guid), ByVal DeletedByID As Integer)
            Me.SetVirtualDeleteToItems(WorkBookId, oItemsID, DeletedByID, True)
        End Sub
        Public Sub NEW_VirtualUndeleteItems(ByVal WorkBookId As System.Guid, ByVal oItemsID As List(Of System.Guid), ByVal DeletedByID As Integer)
            Me.SetVirtualDeleteToItems(WorkBookId, oItemsID, DeletedByID, False)
        End Sub
        Private Function SetVirtualDeleteToItem(ByVal Item As System.Guid, ByVal DeletedByID As Integer, ByVal isdeleted As Boolean) As WorkBookItem
            Dim oPerson As Person = Me.GetPerson(DeletedByID)
            Dim oItem As WorkBookItem = Me.GetWorkBookItem(Item)
            Try
                DC.BeginTransaction()
                If Not IsNothing(oPerson) AndAlso Not IsNothing(oItem) Then
                    Dim DeletedOn As DateTime = Now
                    oItem.ModifiedOn = DeletedOn
                    oItem.ModifiedBy = oPerson
                    oItem.isDeleted = isdeleted
                    DC.SaveOrUpdate(oItem)
                End If
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                Return Nothing
            End Try
            Return oItem
        End Function
        Private Sub SetVirtualDeleteToItems(ByVal WorkBookId As System.Guid, ByVal oItemsID As List(Of System.Guid), ByVal DeletedByID As Integer, ByVal isdeleted As Boolean)
            Dim oPerson As Person = Me.GetPerson(DeletedByID)
            Try
                Dim oWorkBook As WorkBook = Me.NEW_GetWorkBook(WorkBookId)
                DC.BeginTransaction()
                Dim Items As List(Of WorkBookItem) = (From wki As WorkBookItem In DC.GetCurrentSession.Linq(Of WorkBookItem)() Where wki.WorkBookOwner Is oWorkBook AndAlso oItemsID.Contains(wki.Id) Select wki).ToList
                If Not IsNothing(oPerson) AndAlso Not IsNothing(Items) Then
                    Dim DeletedOn As DateTime = Now
                    For Each oItem In Items
                        oItem.ModifiedOn = DeletedOn
                        oItem.ModifiedBy = oPerson
                        oItem.isDeleted = isdeleted
                        DC.SaveOrUpdate(oItem)
                    Next
                End If
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
        End Sub

        Public Function NEW_DeleteWorkBookItem(ByVal ItemID As System.Guid, ByVal BaseUserRepositoryPath As String) As Boolean
            Dim oItem As WorkBookItem = Me.GetWorkBookItem(ItemID)
            Return NEW_DeleteWorkBookItem(oItem, BaseUserRepositoryPath)
        End Function
        Public Function NEW_DeleteWorkBookItem(ByVal oItem As WorkBookItem, ByVal BaseUserRepositoryPath As String) As Boolean
            Dim iResponse As Boolean = False
            If Not IsNothing(oItem) Then
                Try
                    DC.BeginTransaction()
                    Dim FilesName As List(Of System.Guid) = (From f In Me.DC.GetCurrentSession.Linq(Of WorkBookInternalFile)() _
                                                             Where f.ItemOwner Is oItem Select f.File.Id).ToList

                    For Each oFile As WorkBookFile In oItem.Files
                        DC.Delete(oFile)
                    Next
                    DC.Delete(oItem)
                    DC.Commit()
                    Delete.Files(BaseUserRepositoryPath, FilesName, ".stored")
                    iResponse = True
                Catch ex As Exception
                    Debug.Write(ex.ToString)
                    DC.Rollback()
                End Try
            End If
            Return iResponse
        End Function
#End Region

#Region "WorkBook Status"
        Public Function GetDefaultWorkBookStatus() As WorkBookStatus
            Dim oStatus As WorkBookStatus = Nothing
            Try
                DC.BeginTransaction()
                oStatus = (From ws In _Datacontext.GetCurrentSession.Linq(Of WorkBookStatus)() Where ws.IsDefault Select ws).FirstOrDefault
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
            Return oStatus
        End Function
        Public Function GetListOfWorkBookStatus() As List(Of WorkBookStatus)
            Dim oList As List(Of WorkBookStatus)
            Try
                DC.BeginTransaction()
                oList = (From ws In _Datacontext.GetCurrentSession.Linq(Of WorkBookStatus)() Select ws).ToList
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                oList = New List(Of WorkBookStatus)
            End Try
            Return oList
        End Function
        Public Function GetTranslatedWorkBookStatusList(ByVal oLanguage As Language) As List(Of TranslatedItem(Of Integer))
            Dim oTranslatedList As List(Of TranslatedItem(Of Integer))
            Try
                DC.BeginTransaction()
                Dim oDefaultLanguage As Language = (From l In _Datacontext.GetCurrentSession.Linq(Of Language)() Where l.isDefault Select l).FirstOrDefault
                Dim oDefaultTranslatedList = (From ws In _Datacontext.GetCurrentSession.Linq(Of WorkBookStatusTraslations)() _
                        Where ws.SelectedLanguage Is oDefaultLanguage Select New TranslatedItem(Of Integer) With {.Id = ws.Status.Id, .Translation = ws.Translation}).ToList


                oTranslatedList = (From ws In _Datacontext.GetCurrentSession.Linq(Of WorkBookStatusTraslations)() _
                         Where ws.SelectedLanguage Is oLanguage Select New TranslatedItem(Of Integer) With {.Id = ws.Status.Id, .Translation = ws.Translation}).ToList

                If oTranslatedList.Count < oDefaultTranslatedList.Count Then
                    Dim oSelectedID As List(Of Integer) = (From o In oTranslatedList Select o.Id).ToList
                    Dim oOtherStatus As List(Of TranslatedItem(Of Integer)) = (From o In oDefaultTranslatedList Where oSelectedID.Contains(o.Id) = False Select o).ToList
                    If oOtherStatus.Count > 0 Then
                        oTranslatedList.AddRange(oOtherStatus)
                    End If
                End If

                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                oTranslatedList = New List(Of TranslatedItem(Of Integer))
            End Try
            Return oTranslatedList
        End Function
        Public Function GetTranslatedWorkBookStatusList(ByVal oLanguage As Language, ByVal RequiredPermission As EditingPermission) As List(Of TranslatedItem(Of Integer))
            Dim oTranslatedList As List(Of TranslatedItem(Of Integer))
            Try
                DC.BeginTransaction()

                Dim oDefaultLanguage As Language = (From l In _Datacontext.GetCurrentSession.Linq(Of Language)() Where l.isDefault Select l).FirstOrDefault
                Dim oAvailableStatus As List(Of WorkBookStatusTraslations) = (From ws In _Datacontext.GetCurrentSession.Linq(Of WorkBookStatusTraslations)() _
                     Where ws.SelectedLanguage Is oDefaultLanguage OrElse ws.SelectedLanguage.Id = oLanguage.Id).ToList

                oAvailableStatus = (From ws In oAvailableStatus Where (ws.Status.AvailableFor And RequiredPermission) > 0 Select ws).ToList

                ' New TranslatedItem(Of Integer) With {.Id = ws.Status.Id, .Translation = ws.Translation}).ToList


                oTranslatedList = (From ws In oAvailableStatus Where ws.SelectedLanguage.Id = oLanguage.Id Select New TranslatedItem(Of Integer) With {.Id = ws.Status.Id, .Translation = ws.Translation}).ToList

                If oTranslatedList.Count < (oAvailableStatus.Count / 2) AndAlso Not (oDefaultLanguage Is oLanguage) Then
                    Dim oSelectedID As List(Of Integer) = (From o In oTranslatedList Select o.Id).ToList
                    Dim oOtherStatus As List(Of TranslatedItem(Of Integer)) = (From o In oAvailableStatus Where o.SelectedLanguage Is oDefaultLanguage AndAlso oSelectedID.Contains(o.Id) = False Select New TranslatedItem(Of Integer) With {.Id = o.Status.Id, .Translation = o.Translation}).ToList
                    If oOtherStatus.Count > 0 Then
                        oTranslatedList.AddRange(oOtherStatus)
                    End If
                End If
                oTranslatedList = oTranslatedList.OrderBy(Function(c) c.Translation).ToList
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                oTranslatedList = New List(Of TranslatedItem(Of Integer))
            End Try
            Return oTranslatedList
        End Function
        Public Function GetTranslatedWorkBookStatus(ByVal oLanguage As Language, ByVal StatusID As Integer) As TranslatedItem(Of Integer)
            Dim oTranslated As TranslatedItem(Of Integer)
            Try
                DC.BeginTransaction()
                oTranslated = (From ws In _Datacontext.GetCurrentSession.Linq(Of WorkBookStatusTraslations)() _
                         Where ws.SelectedLanguage Is oLanguage AndAlso ws.Status.Id = StatusID Select New TranslatedItem(Of Integer) With {.Id = ws.Status.Id, .Translation = ws.Translation}).FirstOrDefault

                If IsNothing(oTranslated) Then
                    Dim oDefaultLanguage As Language = (From l In _Datacontext.GetCurrentSession.Linq(Of Language)() Where l.isDefault Select l).FirstOrDefault
                    Dim oDefaultTranslated = (From ws In _Datacontext.GetCurrentSession.Linq(Of WorkBookStatusTraslations)() _
                            Where ws.SelectedLanguage Is oDefaultLanguage AndAlso ws.Status.Id = StatusID Select New TranslatedItem(Of Integer) With {.Id = ws.Status.Id, .Translation = ws.Translation}).FirstOrDefault

                    oTranslated = oDefaultTranslated
                End If
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                oTranslated = New TranslatedItem(Of Integer)
            End Try
            Return oTranslated
        End Function
        Public Function GetTranslationWorkBookStatus(ByVal oLanguage As Language, ByVal StatusID As Integer) As String
            Dim iResponse As String = ""
            Try
                iResponse = (From ws In _Datacontext.GetCurrentSession.Linq(Of WorkBookStatusTraslations)() _
                         Where ws.SelectedLanguage Is oLanguage AndAlso ws.Status.Id = StatusID Select ws.Translation).FirstOrDefault

                If String.IsNullOrEmpty(iResponse) Then
                    Dim oDefaultLanguage As Language = (From l In _Datacontext.GetCurrentSession.Linq(Of Language)() Where l.isDefault Select l).FirstOrDefault
                    Dim oDefaultTranslated = (From ws In _Datacontext.GetCurrentSession.Linq(Of WorkBookStatusTraslations)() _
                            Where ws.SelectedLanguage Is oDefaultLanguage AndAlso ws.Status.Id = StatusID Select ws.Translation).FirstOrDefault

                    iResponse = oDefaultTranslated
                End If
            Catch ex As Exception
            End Try
            Return iResponse
        End Function
        Public Function GetWorkBookStatus(ByVal StatusID As Integer) As WorkBookStatus
            Dim oStatus As WorkBookStatus = Nothing
            Try
                DC.BeginTransaction()
                oStatus = _Datacontext.GetCurrentSession.Get(Of WorkBookStatus)(StatusID)
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
            Return oStatus
        End Function
        Public Function GetItemCountByStatus(ByVal StatusID As Integer) As Long
            Dim iCount As Long = 0
            Try
                iCount = (From oItem In DC.GetCurrentSession.Linq(Of WorkBookItem)() Where oItem.Status.Id = StatusID).Count
            Catch ex As Exception
            End Try
            Return iCount
        End Function
        Public Function GetWorkBookCountByStatus(ByVal StatusID As Integer) As Long
            Dim iCount As Long = 0
            Try
                iCount = (From oItem In DC.GetCurrentSession.Linq(Of WorkBook)() Where oItem.Status.Id = StatusID).Count
            Catch ex As Exception
            End Try
            Return iCount
        End Function

        Public Function SaveItemsStatus(ByVal WorkbookID As System.Guid, ByVal oItems As List(Of GenericItemStatus(Of System.Guid, Integer)), ByVal ChangedBy As Integer) As List(Of GenericItemStatus(Of System.Guid, Integer))
            Dim oChangedList As New List(Of GenericItemStatus(Of System.Guid, Integer))
            Dim oChangedBy As Person = DC.GetById(Of Person)(ChangedBy)
            Dim oChangedOn As DateTime = New DateTime
            Dim isChanged As Boolean = False
            For Each oItemStatus As GenericItemStatus(Of System.Guid, Integer) In oItems
                Try
                    DC.BeginTransaction()
                    Dim oItem As WorkBookItem = _Datacontext.GetCurrentSession.Get(Of WorkBookItem)(oItemStatus.Id)
                    If Not IsNothing(oItem) AndAlso oItem.Status.Id <> oItemStatus.Status Then
                        If oChangedOn.Equals(New DateTime) Then
                            oChangedOn = Now
                        End If
                        oItem.ModifiedBy = oChangedBy
                        oItem.ModifiedOn = oChangedOn
                        oItem.ApprovedBy = oChangedBy
                        oItem.ApprovedOn = oChangedOn
                        oItem.Status = Me.DC.GetById(Of WorkBookStatus)(oItemStatus.Status)
                        isChanged = True
                        DC.SaveOrUpdate(oItem)
                    End If
                    DC.Commit()
                    If Not IsNothing(oItem) Then
                        oChangedList.Add(oItemStatus)
                    End If
                Catch ex As Exception
                    If DC.isInTransaction Then
                        DC.Rollback()
                    End If
                End Try
            Next
            If isChanged AndAlso Not oChangedOn.Equals(New DateTime) Then
                Try
                    DC.BeginTransaction()
                    Dim oWorkBook As WorkBook = _Datacontext.GetCurrentSession.Get(Of WorkBook)(WorkbookID)
                    oWorkBook.ModifiedBy = oChangedBy
                    oWorkBook.ModifiedOn = oChangedOn
                    DC.SaveOrUpdate(oWorkBook)
                    DC.Commit()
                Catch ex As Exception
                    If DC.isInTransaction Then
                        DC.Rollback()
                    End If
                End Try
            End If


            Return oChangedList
        End Function
        Public Function SaveItemsEditing(ByVal WorkbookID As System.Guid, ByVal oItems As List(Of GenericItemStatus(Of System.Guid, Integer)), ByVal ChangedBy As Integer) As List(Of GenericItemStatus(Of System.Guid, Integer))
            Dim oChangedList As New List(Of GenericItemStatus(Of System.Guid, Integer))
            Dim oChangedBy As Person = DC.GetById(Of Person)(ChangedBy)
            Dim oChangedOn As DateTime = New DateTime
            Dim isChanged As Boolean = False
            For Each oItemStatus As GenericItemStatus(Of System.Guid, Integer) In oItems
                Try
                    DC.BeginTransaction()
                    Dim oItem As WorkBookItem = _Datacontext.GetCurrentSession.Get(Of WorkBookItem)(oItemStatus.Id)
                    If Not IsNothing(oItem) AndAlso oItem.Editing <> oItemStatus.Status Then
                        If oChangedOn.Equals(New DateTime) Then
                            oChangedOn = Now
                        End If
                        oItem.ModifiedBy = oChangedBy
                        oItem.ModifiedOn = oChangedOn
                        oItem.Editing = oItemStatus.Status
                        isChanged = True
                        DC.SaveOrUpdate(oItem)
                    End If
                    DC.Commit()
                    If Not IsNothing(oItem) Then
                        oChangedList.Add(oItemStatus)
                    End If
                Catch ex As Exception
                    If DC.isInTransaction Then
                        DC.Rollback()
                    End If
                End Try
            Next
            If isChanged AndAlso Not oChangedOn.Equals(New DateTime) Then
                Try
                    DC.BeginTransaction()
                    Dim oWorkBook As WorkBook = _Datacontext.GetCurrentSession.Get(Of WorkBook)(WorkbookID)
                    oWorkBook.ModifiedBy = oChangedBy
                    oWorkBook.ModifiedOn = oChangedOn
                    DC.SaveOrUpdate(oWorkBook)
                    DC.Commit()
                Catch ex As Exception
                    If DC.isInTransaction Then
                        DC.Rollback()
                    End If
                End Try
            End If
            Return oChangedList
        End Function
        Private Function HasEditingPermisson(ByVal Permission As EditingPermission, ByVal requiredPermission As EditingPermission) As Boolean
            Return Permission And requiredPermission
        End Function



        Public Function GetManagementWorkBookStatusList(ByVal oLanguage As Language, ByVal oPager As lm.Comol.Core.DomainModel.PagerBase) As List(Of dtoWorkBookStatus)
            Dim oList As List(Of dtoWorkBookStatus) = Nothing
            Try
                DC.BeginTransaction()
                oList = (From ws In _Datacontext.GetCurrentSession.Linq(Of WorkBookStatus)() Select New dtoWorkBookStatus() With {.ID = ws.Id, .isDefault = ws.IsDefault, .AvailableForPermission = ws.AvailableFor}).Skip(oPager.Skip).Take(oPager.PageSize).ToList

                '.Name = GetTranslationWorkBookStatus(oLanguage, ws),
                ', .ItemsCount = Me.GetItemCountByStatus(ws), .WorkbookCount = Me.GetWorkBookCountByStatus(ws)
                For Each o In oList
                    o.ItemsCount = Me.GetItemCountByStatus(o.ID)
                    o.WorkbookCount = Me.GetWorkBookCountByStatus(o.ID)
                    o.Name = GetTranslationWorkBookStatus(oLanguage, o.ID)
                Next
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                oList = New List(Of dtoWorkBookStatus)
            End Try
            Return oList
        End Function
        Public Function GetManagementWorkBookStatusCount() As Long
            Dim iResponse As Long = 0
            Try
                DC.BeginTransaction()
                iResponse = (From ws In _Datacontext.GetCurrentSession.Linq(Of WorkBookStatus)()).Count
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
            Return iResponse
        End Function

        Public Function SaveStatus(ByVal StatusID As Integer, ByVal oStatus As dtoWorkBookStatus, ByVal oList As List(Of dtoWorkBookStatusTranslation)) As WorkBookStatus
            Dim iResponse As WorkBookStatus = Nothing
            Dim oDefaultLanguage As Language = (From l In _Datacontext.GetCurrentSession.Linq(Of Language)() Where l.isDefault Select l).FirstOrDefault



            Try
                DC.BeginTransaction()

                Dim oSavedStatus As WorkBookStatus = Nothing
                If StatusID > 0 Then
                    oSavedStatus = DC.GetCurrentSession.Get(Of WorkBookStatus)(StatusID)

                    If oSavedStatus.IsDefault <> oStatus.isDefault Then
                        Dim oDefault As WorkBookStatus = (From ws In DC.GetCurrentSession.Linq(Of WorkBookStatus)() Where ws.IsDefault Select ws).FirstOrDefault
                        If IsNothing(oDefault) = False Then
                            oDefault.IsDefault = False
                            DC.SaveOrUpdate(oDefault)
                        End If
                    End If

                    oSavedStatus.IsDefault = oStatus.isDefault
                    oSavedStatus.AvailableFor = oStatus.AvailableForPermission
                    oSavedStatus.Name = (From o In oList Where o.LanguageID = oDefaultLanguage.Id Select o.Translation).FirstOrDefault
                Else
                    oSavedStatus = New WorkBookStatus
                    With oSavedStatus
                        .IsDefault = oStatus.isDefault
                        .AvailableFor = oStatus.AvailableForPermission
                        .Id = oStatus.ID
                        .Name = (From o In oList Where o.LanguageID = oDefaultLanguage.Id Select o.Translation).FirstOrDefault
                        .Translations = New List(Of WorkBookStatusTraslations)
                    End With
                End If
                DC.SaveOrUpdate(oSavedStatus)
                For Each o In oList
                    Dim oTranslation As WorkBookStatusTraslations = Nothing
                    If o.UniqueID > 0 AndAlso o.StatusId = StatusID Then
                        oTranslation = DC.GetById(Of WorkBookStatusTraslations)(o.UniqueID)
                        If Not IsNothing(oTranslation) Then
                            oTranslation.Translation = o.Translation
                            DC.SaveOrUpdate(oTranslation)
                        End If
                    ElseIf o.UniqueID = 0 Then
                        oTranslation = New WorkBookStatusTraslations
                        oTranslation.SelectedLanguage = DC.GetById(Of Language)(o.LanguageID)
                        oTranslation.Status = oSavedStatus
                        oTranslation.Translation = o.Translation
                        DC.SaveOrUpdate(oTranslation)
                    End If
                Next
                DC.Commit()
                iResponse = oSavedStatus
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
            Return iResponse
        End Function
        Public Function LoadStatusTranslation(ByVal oStatus As WorkBookStatus) As List(Of dtoWorkBookStatusTranslation)
            Dim mList As List(Of dtoWorkBookStatusTranslation)
            Try
                DC.BeginTransaction()
                If oStatus Is Nothing Then
                    mList = (From l In Me.DC.GetCurrentSession.Linq(Of Language)() Select New dtoWorkBookStatusTranslation() With {.LanguageID = l.Id, .LanguageName = l.Name, .StatusId = 0, .Translation = "", .UniqueID = 0}).ToList
                Else
                    Dim oLanguages As List(Of Language) = (Me.DC.GetCurrentSession.Linq(Of Language)()).ToList()
                    Dim oTranslations As List(Of WorkBookStatusTraslations) = (From o In Me.DC.GetCurrentSession.Linq(Of WorkBookStatusTraslations)() Where o.Status Is oStatus Select o).ToList

                    mList = (From lang In oLanguages Group Join t In oTranslations On lang.Id Equals t.SelectedLanguage.Id Into children = Group _
                             From child In children.DefaultIfEmpty(New WorkBookStatusTraslations() With {.Id = 0, .SelectedLanguage = lang, .Status = oStatus, .Translation = ""}) Select New dtoWorkBookStatusTranslation() With {.LanguageID = lang.Id, .LanguageName = lang.Name, .Translation = child.Translation, .UniqueID = child.Id, .StatusId = child.Status.Id}).tolist
                End If
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                mList = New List(Of dtoWorkBookStatusTranslation)
            End Try

            Return mList
        End Function
        Public Sub DeleteWorkBookStatus(ByVal StatusID As Integer)
            Dim oStatus As WorkBookStatus = Nothing
            Try
                DC.BeginTransaction()
                oStatus = _Datacontext.GetCurrentSession.Get(Of WorkBookStatus)(StatusID)
                For Each o In oStatus.Translations
                    DC.Delete(o)
                Next
                DC.Delete(oStatus)
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
        End Sub
#End Region
#End Region

#Region "WorkBook AUTHOR"
        Public Function GetWorkBookAuthor(ByVal AuthorID As System.Guid) As WorkBookAuthor
            Dim oWorkBookAuthor As WorkBookAuthor
            Try
                oWorkBookAuthor = _Datacontext.GetById(Of WorkBookAuthor)(AuthorID)
            Catch ex As Exception
                Debug.Write(ex.ToString)

                Return Nothing
            End Try
            Return oWorkBookAuthor
        End Function
        Public Function RemoveVirtualAuthor(ByVal AuthorID As System.Guid) As WorkBookAuthor
            Dim oWorkBookAuthor As WorkBookAuthor = Me.GetWorkBookAuthor(AuthorID)
            Dim oMeta As New MetaData

            Try
                DC.BeginTransaction()
                Dim oPerson As Person = DC.GetById(Of Person)(Me.CurrentUserContext.CurrentUser.Id)
                If Not IsNothing(oWorkBookAuthor) AndAlso IsNothing(oPerson) = False Then
                    oWorkBookAuthor.ModifiedOn = Now
                    oWorkBookAuthor.ModifiedBy = oPerson
                    oWorkBookAuthor.isDeleted = True
                    DC.SaveOrUpdate(oWorkBookAuthor)
                End If
                DC.Commit()
                Return oWorkBookAuthor
            Catch ex As Exception
                Debug.Write(ex.ToString)
                DC.Rollback()
                Return oWorkBookAuthor
            End Try
            Return oWorkBookAuthor
        End Function
        Public Function UnDeleteVirtualAuthor(ByVal AuthorID As System.Guid) As WorkBookAuthor
            Dim oWorkBookAuthor As WorkBookAuthor = Me.GetWorkBookAuthor(AuthorID)

            Try
                DC.BeginTransaction()
                If Not IsNothing(oWorkBookAuthor) Then
                    Dim oPerson As Person = Me.DC.GetById(Of Person)(Me.CurrentUserContext.CurrentUserID)
                    oWorkBookAuthor.ModifiedBy = oPerson
                    oWorkBookAuthor.ModifiedOn = Now
                    oWorkBookAuthor.isDeleted = False
                    DC.SaveOrUpdate(oWorkBookAuthor)
                End If
                DC.Commit()
                Return oWorkBookAuthor
            Catch ex As Exception
                Debug.Write(ex.ToString)
                DC.Rollback()
                Return oWorkBookAuthor
            End Try
            Return oWorkBookAuthor
        End Function
        Public Function RemoveAuthor(ByVal AuthorID As System.Guid) As WorkBookAuthor
            Dim oWorkBookAuthor As WorkBookAuthor = Me.GetWorkBookAuthor(AuthorID)
            Dim oMeta As New MetaData

            Try
                DC.BeginTransaction()
                DC.Delete(oWorkBookAuthor)
                DC.Commit()
                Return oWorkBookAuthor
            Catch ex As Exception
                Debug.Write(ex.ToString)
                DC.Rollback()
                Return oWorkBookAuthor
            End Try
            Return oWorkBookAuthor
        End Function
        Public Function AddAuthorsToWorkBook(ByVal WorkBookID As System.Guid, ByVal oList As List(Of Person), ByVal UserID As Integer) As WorkBook
            Dim oWorkBook As WorkBook = Me.GetWorkBook(WorkBookID)
            Try
                DC.BeginTransaction()

                oWorkBook = _Datacontext.GetById(Of WorkBook)(WorkBookID)
                If IsNothing(oWorkBook) Then
                    Return Nothing
                Else
                    Dim oPerson As iPerson = Me.GetPerson(UserID)
                    Dim CreatedOn As DateTime = Now
                    For Each oTempUser In oList
                        Dim oAuthor As WorkBookAuthor = Nothing
                        Dim TempUserID As Integer = oTempUser.Id
                        oAuthor = (From o In oWorkBook.WorkBookAuthors Where o.Author.Id = TempUserID).FirstOrDefault
                        If IsNothing(oAuthor) Then
                            oAuthor = New WorkBookAuthor
                            oAuthor.Author = Me.GetPerson(oTempUser.Id)
                            oAuthor.WorkBookOwner = oWorkBook
                            oAuthor.IsOwner = False
                            oAuthor.CreatedBy = oPerson
                            oAuthor.CreatedOn = CreatedOn
                            oWorkBook.WorkBookAuthors.Add(oAuthor)

                            DC.SaveOrUpdate(oWorkBook)
                            DC.SaveOrUpdate(oAuthor)
                        ElseIf oAuthor.isDeleted Then
                            oAuthor.isDeleted = False
                            oAuthor.ModifiedBy = oPerson
                            oAuthor.ModifiedOn = CreatedOn
                            DC.SaveOrUpdate(oAuthor)
                        End If

                        DC.SaveOrUpdate(oWorkBook)
                    Next
                End If
                DC.Commit()
                Return oWorkBook
            Catch ex As Exception
                Debug.Write(ex.ToString)

                Return Nothing
            End Try
            Return oWorkBook
        End Function
#End Region

        Public Function DeleteCommunityWorkBook(ByVal CommunityID As Integer, ByVal BaseUserRepositoryPath As String) As Boolean
            Dim iResponse As Boolean = True

            Try
                Dim oCommunity As Community = DC.GetById(Of Community)(CommunityID)
                If oCommunity Is Nothing AndAlso CommunityID > 0 Then
                    iResponse = False
                Else
                    Dim InternalFileNames As New List(Of System.Guid)
                    Dim oList As List(Of WorkBook) = (From w In DC.GetCurrentSession.Linq(Of WorkBook)() Where w.CommunityOwner Is oCommunity Select w).ToList
                    DC.BeginTransaction()
                    For Each oWorkbook In oList
                        InternalFileNames.AddRange(Me.DeleteCommunityWorkBook(oWorkbook))
                    Next

                    DC.Commit()
                    iResponse = True
                    Delete.Files(BaseUserRepositoryPath, InternalFileNames, ".stored")
                End If
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try

            Return iResponse
        End Function

        Public Function DeleteCommunityWorkBook(ByVal oWorkBook As WorkBook) As List(Of System.Guid)
            Dim iResponse As New List(Of System.Guid)
            If Not IsNothing(oWorkBook) Then
                Try
                    If oWorkBook.isPersonal AndAlso Not IsNothing(oWorkBook.CommunityOwner) Then
                        Dim CommunityFileNames As List(Of WorkBookCommunityFile) = (From f In Me.DC.GetCurrentSession.Linq(Of WorkBookCommunityFile)() _
                                                               Where f.WorkBookOwner Is oWorkBook Select f).ToList
                        For Each oFile As WorkBookCommunityFile In CommunityFileNames
                            DC.Delete(oFile)
                        Next
                        oWorkBook.CommunityOwner = Nothing
                        DC.Update(oWorkBook)
                    Else
                        Dim InternalFileNames As List(Of System.Guid) = (From f In Me.DC.GetCurrentSession.Linq(Of WorkBookInternalFile)() _
                                                                   Where f.WorkBookOwner Is oWorkBook Select f.File.Id).ToList
                        Dim AllFiles As List(Of WorkBookFile) = (From f In Me.DC.GetCurrentSession.Linq(Of WorkBookFile)() _
                                                                 Where f.WorkBookOwner Is oWorkBook Select f).ToList

                        For Each oFile As WorkBookFile In AllFiles
                            DC.Delete(oFile)
                        Next
                        Dim AllItems As List(Of WorkBookItem) = (From f In Me.DC.GetCurrentSession.Linq(Of WorkBookItem)() _
                                                                 Where f.WorkBookOwner Is oWorkBook Select f).ToList
                        For Each oItem As WorkBookItem In AllItems
                            DC.Delete(oItem)
                        Next
                        DC.Delete(oWorkBook)
                        iResponse = InternalFileNames
                    End If

                Catch ex As Exception
                    Throw New Exception("unable to delete workbook", ex)
                End Try
            End If
            Return iResponse
        End Function

    End Class
End Namespace