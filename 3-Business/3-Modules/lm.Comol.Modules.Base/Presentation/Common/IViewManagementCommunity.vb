Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports COL_BusinessLogic_v2
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    Public Interface IViewCommonSessionExpired
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        ReadOnly Property PreloadedForUserID() As Integer
        ReadOnly Property PreloadedCommunityID() As Integer
        ReadOnly Property PreservePreviousUrl() As Boolean
        ReadOnly Property PreloadedLanguageCode() As String
        ReadOnly Property PreloadedInPopupWindow() As Boolean
        ReadOnly Property PreloadedForDownload As Boolean

        Property CurrentForUserID() As Integer
        Property CurrentCommunityID() As Integer

        Property PreservedDownloadUrl() As String
        Property CurrentInPopupWindow() As Boolean
        Property CurrentForDownload() As Boolean

        Sub InitializeView(ByVal CommunityID As Integer)
        Sub LoadStandardLoginPage()
        Sub LoadInternalLoginPage(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal PersonLogin As String, ByVal PostLoadDownloadPage As String)
        Sub LoadExternalLoginPage(ByVal AuthenticationTypeID As Integer, ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal PersonLogin As String, ByVal PostLoadDownloadPage As String)
        Sub LoadLanguage(ByVal oLanguage As Language)
    End Interface
End Namespace