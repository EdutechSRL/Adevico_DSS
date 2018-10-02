Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports COL_BusinessLogic_v2

Namespace lm.Comol.Modules.Base.Presentation
    Public Interface IviewCommunityFolderSelect
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        WriteOnly Property ShowNoFolderFound() As Boolean
        Property SelectedFolder() As Long
        ReadOnly Property SelectedFolderName() As String
        Property SelectionMode() As Web.UI.WebControls.ListSelectionMode
        Property AjaxEnabled() As Boolean
        Property ForUpload As Boolean
        Sub LoadTree(ByVal oCommuntyRepository As dtoFileFolder, isForManagement As Boolean)

        ReadOnly Property SelectedFolderPath() As List(Of FilterElement)
        ReadOnly Property SelectedFolderPathName() As String
    End Interface
End Namespace