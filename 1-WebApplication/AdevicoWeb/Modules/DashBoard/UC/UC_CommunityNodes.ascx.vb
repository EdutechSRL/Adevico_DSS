Public Class UC_CommunityNodes
    Inherits DBbaseControl

#Region "Internal"
    Public Event AccessTo(ByVal idCommunity As Integer, path As String)
    Public Event EnrollTo(ByVal idCommunity As Integer, path As String)
    Public Event UnsubscribeFrom(ByVal idCommunity As Integer, path As String)
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

#Region "Internal"
    Private internal As Dictionary(Of String, String)
    Public Sub InitializeControl(nodes As List(Of lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityNodeItem), n As Dictionary(Of String, String))
        internal = n
        RPTchildren.DataSource = nodes
        RPTchildren.DataBind()
    End Sub
    Protected Sub RPTchildren_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim item As lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityNodeItem = e.Item.DataItem
                Dim oControl As UC_CommunityNode = e.Item.FindControl("CTRLnode")
                If Not IsNothing(item) Then
                    If item.HasCurrent Then
                        oControl.AutoOpenCssClass = LTtreeKeepAutoOpenCssClass.Text
                    End If
                    oControl.InitializeControl(item, internal)
                End If
        End Select
    End Sub

    Protected Friend Sub CTRLmenu_AccessTo(idCommunity As Integer, path As String)
        RaiseEvent EnrollTo(idCommunity, path)
    End Sub
    Protected Friend Sub CTRLmenu_EnrollTo(idCommunity As Integer, path As String)
        RaiseEvent EnrollTo(idCommunity, path)
    End Sub
    Protected Friend Sub CTRLmenu_UnsubscribeFrom(idCommunity As Integer, path As String)
        RaiseEvent UnsubscribeFrom(idCommunity, path)
    End Sub
#End Region

  
    Private Sub RPTchildren_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTchildren.ItemCommand

    End Sub
End Class