Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.Base.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    Public Interface IviewScormFileSelector
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView
        Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleCommunityRepository

        Sub InitializeControl(ByVal CommunityID As Integer, ByVal UserID As Integer, ByVal PreSelectID As List(Of Long))
        Sub InitializeControlForModule(ByVal CommunityID As Integer, ByVal UserID As Integer, ByVal PreSelectID As List(Of Long), ByVal ModuleID As Integer, ByVal ItemID As String, ByVal ItemType As String)

        Property SelectorItemTypeID() As String
        Property SelectorItemID() As String
        Property SelectorModuleID() As Integer
        Property SelectorModuleCode() As String
        Property SelectorCommunityID() As Integer
        Property SelectedFilesID() As List(Of Long)
        Property TemporaryFilesID() As List(Of Long)
        ReadOnly Property PageSize() As Integer

        Sub LoadFiles(ByVal Files As List(Of dtoFileScorm))
    End Interface
End Namespace