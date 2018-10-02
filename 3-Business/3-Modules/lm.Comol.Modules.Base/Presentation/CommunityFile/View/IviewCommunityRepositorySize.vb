Namespace lm.Comol.Modules.Base.Presentation
    Public Interface IviewCommunityRepositorySize
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView
        ReadOnly Property DrivePath() As String
        WriteOnly Property UsageRate25() As Integer
        WriteOnly Property UsageRate50() As Integer
        WriteOnly Property UsageRate75() As Integer
        WriteOnly Property UsageRate100() As Integer
        WriteOnly Property UsageRate150() As Integer
        WriteOnly Property TestoSezioneOver() As String
        Sub SetDisplayInfo(ByVal SizeUsed As Double, ByVal SizeAvailable As Long)

        ReadOnly Property BaseUnit() As Integer

        Property ChachedSize() As Long
        Property isPersonalRepository() As Boolean
        Property EvaluateDeletedFiles() As Boolean
        Property RepositoryCommunityID() As Integer
        ReadOnly Property AvailableMB() As Long
    End Interface
End Namespace