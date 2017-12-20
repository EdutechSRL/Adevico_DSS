Imports lm.Comol.Core.Statistiche

Partial Public Class UC_Void_StatPers
    Inherits System.Web.UI.UserControl
    Implements IpersonalContainer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub Bind(ByVal Community_Id As Integer, ByVal User_Id As Integer) Implements lm.Comol.Core.Statistiche.IpersonalContainer.Bind

    End Sub

    Public Property CommunityID() As Integer Implements lm.Comol.Core.Statistiche.IpersonalContainer.CommunityID
        Get

        End Get
        Set(ByVal value As Integer)

        End Set
    End Property

    Public Property UserID() As Integer Implements lm.Comol.Core.Statistiche.IpersonalContainer.UserID
        Get

        End Get
        Set(ByVal value As Integer)

        End Set
    End Property
End Class