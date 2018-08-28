Imports lm.Comol.Core.DomainModel.Common

<CLSCompliant(True)> Public Interface ImoduleToQuizAction
    Inherits iModuleActionView

    Property ServiceCode As String
    Property ServiceID As Integer
    Property IsInline As Boolean
    Sub DisplayNoAction()
    Sub DisplayAction()

    'DA ADATTARE AI QUIZ!!!!!!!!!!!!!!!!!!!!!!
    'Sub ActionForDownload(ByVal DescriptionOnly As Boolean, ByVal LinkID As Long, ByVal FileID As Long, ByVal FileName As String, ByVal Extension As String, ByVal Size As Long)
    'Sub ActionForPlay(ByVal DescriptionOnly As Boolean, ByVal LinkID As Long, ByVal FileID As Long, ByVal FileName As String, ByVal Extension As String, ByVal Size As Long, ByVal UniqueID As System.Guid, ByVal CommunityID As Integer)
    'Sub ActionForUpload(ByVal DescriptionOnly As Boolean, ByVal LinkID As Long, ByVal FolderID As Long, ByVal FolderName As String, ByVal CommunityID As String)
    'Sub ActionForCreateFolder(ByVal DescriptionOnly As Boolean, ByVal LinkID As Long, ByVal FolderID As Long, ByVal FolderName As String, ByVal CommunityID As Integer)
    Sub ActionForCreate(ByVal idActivity As Int64)
    Sub ActionForUpdate(ByVal idActivity As Int64, ByVal idQuiz As Int64)

    ReadOnly Property GetUrlForCompile(ByVal SRCtypeID As Integer, ByVal idActivity As Int64, ByVal idQuiz As Int64, ByVal idLink As Long) As String
    ReadOnly Property GetUrlForCreate(ByVal idActivity As Int64, ByVal ownerType As Int64) As String
    ReadOnly Property GetUrlForUpdate(ByVal idActivity As Int64, ByVal idQuiz As Int64, ByVal ownerType As Int64) As String
    ReadOnly Property GetUrlForUserStat(ByVal idActivity As Int64, ByVal idQuiz As Int64, ByVal ownerType As Int64, ByVal idUser As Int64) As String
    ReadOnly Property GetUrlForAdvancedStat(ByVal idActivity As Int64, ByVal idQuiz As Int64, ByVal ownerType As Int64, ByVal idUser As Int64) As String

    Sub ActionForCompile(ByVal SRCtypeID As Integer, ByVal idActivity As Long, ByVal idQuiz As Long, ByVal idLink As Long)
    Property ownerPermission As lm.Comol.Modules.EduPath.Domain.PermissionEP_Enum
End Interface
