Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    <CLSCompliant(True)> Public Interface IviewWorkBookAdd
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        ReadOnly Property ModulePermission() As ModuleWorkBook
        ReadOnly Property CommunitiesPermission() As IList(Of WorkBookCommunityPermission)
		Property CurrentWorkBookID() As System.Guid
		Property WorkBookCommunityID() As Integer
        Property CurrentCommunityID() As Integer
        Property CurrentWorkBookType() As WorkBookType
		Property CurrentOwner() As lm.Comol.Core.DomainModel.Person
        'WriteOnly Property CurrentAuthors() As List(Of iPerson)
        ReadOnly Property SelectedUsers() As List(Of Person)
        ReadOnly Property CreateForOther() As Boolean
        WriteOnly Property FirstStep() As viewStep
		Property LastStep() As ViewStep
		'Property AvailableStep() As IList(Of IviewWorkBookAdd.ViewStep)
		Property CurrentStep() As ViewStep
        Property AddCurrentUserToAuthors() As Boolean
        Property SelectOnlyCurrentUser() As Boolean
        Property AllowSelectCurrentUser() As Boolean
		ReadOnly Property GetCurrentWorkBook() As WorkBook
		Sub ShowError(ByVal ErrorString As String)
        Sub LoadWorkBook(ByVal oWorkBook As dtoWorkBook, ByVal oAvailableStatus As List(Of TranslatedItem(Of Integer)))

		Sub InitCommunitySelection()
		Sub LoadAvailableTypes(ByVal oList As List(Of viewWorkBookType))
		Sub LoadSearchAuthors()
		'Sub LoadAvailableAuthors(ByVal oList As List(Of iPerson))
        Sub LoadAuthorsToSelectOwner(ByVal oList As List(Of Person))
        Sub ShowCompleteMessage(ByVal iError As viewError)
		Sub LoadWorkBookList(ByVal oType As WorkBookType)

		Sub InitUsersList(ByVal oCommunityList As List(Of Integer), ByVal multiple As Boolean, ByVal ExceptUsers As List(Of Integer))

		'Property ReloadUsersList() As Boolean
        ReadOnly Property ListCurrentView() As WorkBookTypeFilter

        Enum ViewStep
            None
            SelectCommunity
            SelectType
            SelectData
            SelectAuthors
            SelectOwner
            FinalMessage
            FinalErrorMessage
        End Enum

        Enum viewError
            None = 0
            AuthorsNotFound = 1
            OwnerNotFound = 2
            CreateError = 3
		End Enum

		Enum viewWorkBookType
			None = -999
			Personal = 0
			PersonalShared = 1
			PersonalCommunity = 2
			Community = 3
			CommunityShared = 4
			PersonalSharedCommunity = 5
			OtherUser = 6
        End Enum

        Sub NotifyPersonal(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal Name As String, ByVal CreatorName As String, ByVal Authors As List(Of Integer))
        Sub NotifyCommunity(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal Name As String, ByVal CreatorName As String, ByVal Authors As List(Of Integer))


        WriteOnly Property AllowEditingChanging() As Boolean
        Sub SetEditing(ByVal oAvailableEditing As List(Of TranslatedItem(Of Integer)), ByVal ItemEditing As EditingPermission)
        ReadOnly Property GetEditingTranslation(ByVal Translation As Integer) As String
        Property AllowStatusChange() As Boolean
	End Interface
End Namespace
