Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    <CLSCompliant(True)> Public Interface IviewWorkBookEdit
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        ReadOnly Property ModulePermission() As ModuleWorkBook
        ReadOnly Property CommunitiesPermission() As IList(Of WorkBookCommunityPermission)
        ReadOnly Property CurrentWorkBookID() As System.Guid
        Property WorkBookCommunityID() As Integer
        Property CurrentWorkBookType() As WorkBookType
		Property CurrentStep() As ViewStep
		Property AllowShowItems() As Boolean
		Property AllowManagementAuthors() As Boolean
		Property AllowAddAuthors() As Boolean
		Property AllowSelectOwner() As Boolean

        ReadOnly Property ListCurrentView() As WorkBookTypeFilter

        Sub ShowError(ByVal ErrorString As String)
        Sub LoadWorkBook(ByVal oWorkBook As dtoWorkBook, ByVal oAvailableStatus As List(Of TranslatedItem(Of Integer)))
        Sub LoadAuthors(ByVal oList As IList(Of dtoWorkBookAuthor))
		Sub LoadOwner(ByVal OwnerID As Integer, ByVal oList As List(Of iPerson), ByVal oPermission As lm.Comol.Modules.Base.DomainModel.iWorkBookPermission)

		Sub LoadWorkBookList()
		Sub LoadSearchUser(ByVal oCommunity As List(Of Integer), ByVal multiple As Boolean, ByVal oExceptUsers As List(Of Integer))
		Function GetWorkBook() As WorkBook
		ReadOnly Property SelectedUsers() As List(Of Person)

        Enum ViewStep
            None
			ChangeData
            ManagementAuthors
            AddAuthors
            SelectOwner
            FinalMessage
            FinalErrorMessage
        End Enum

        Enum viewError
            None = 0
            AuthorsNotFound = 1
            OwnerNotFound = 2
            GenericError = 3
        End Enum




        Sub NotifyPersonalEdit(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal Name As String, ByVal CreatorName As String, ByVal Authors As List(Of Integer))
        Sub NotifyCommunityEdit(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal Name As String, ByVal CreatorName As String, ByVal Authors As List(Of Integer))
        'Sub NotifyPersonalEdit(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal Name As String, ByVal CreatorName As String, ByVal Authors As List(Of Integer))


        WriteOnly Property AllowEditingChanging() As Boolean
        Sub SetEditing(ByVal oAvailableEditing As List(Of TranslatedItem(Of Integer)), ByVal ItemEditing As EditingPermission)
        ReadOnly Property GetEditingTranslation(ByVal Translation As Integer) As String
        Property AllowStatusChange() As Boolean
    End Interface
End Namespace
