Imports lm.Comol.Core.DomainModel
Imports lm.Modules.NotificationSystem.Presentation

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoSubscription
        Inherits DomainObject(Of Integer)

        Public CommunityID As Integer
        Public CommunityName As String
        Public CommunityLogo As String
        Public Role As iRole
        Public LastAccessOn As DateTime?
        Public ReadOnly Property HasNews() As Boolean
            Get
                Return NewsInfo.Count > 0
            End Get
        End Property
        Public Enabled As Boolean
        Public NewsInfo As dtoCommunityNewsCount
        Sub New()
            NewsInfo = New dtoCommunityNewsCount() With {.Count = 0}
        End Sub

        Sub New(ByVal s As Subscription)
            NewsInfo = New dtoCommunityNewsCount() With {.Count = 0}
            CommunityID = s.Community.Id
            CommunityName = s.Community.Name
            Role = s.Role
            LastAccessOn = s.LastAccessOn
            CommunityLogo = s.Community.TypeOfCommunity.Logo
            Enabled = s.Accepted AndAlso s.Enabled
        End Sub

        Public Function UpdateNewsInfo(ByVal oNews As dtoCommunityNewsCount) As dtoSubscription
            Me.NewsInfo = oNews
            Return Me
        End Function
    End Class
End Namespace