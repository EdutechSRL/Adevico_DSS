Imports lm.Comol.Core.Dashboard.Domain
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Public Class UC_CommunityNode
    Inherits DBbaseControl

#Region "Internal"
    Private _AutoOpenCssClass As String
    Public Property AutoOpenCssClass As String
        Get
            Return _AutoOpenCssClass
        End Get
        Set(value As String)
            _AutoOpenCssClass = value
        End Set
    End Property
    Private _CssClass As String
    Public Property CssClass As String
        Get
            Return _CssClass
        End Get
        Set(value As String)
            _CssClass = value
        End Set
    End Property
    Public ReadOnly Property Displayname As String
        Get
            Return LBnodeName.Text
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

#Region "Internal"
    Public Sub InitializeControl(item As lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityNodeItem)
        CBselect.Visible = Not (item.Type = lm.Comol.Core.BaseModules.Dashboard.Domain.NodeType.Virtual)
        Select Case item.Type
            Case lm.Comol.Core.BaseModules.Dashboard.Domain.NodeType.Virtual
                LBnodeName.Text = item.Displayname
            Case Else
                LBnodeName.Text = item.Displayname
                LNBnodeName.Text = String.Format(LNBnodeName.Text, item.Displayname)
                LNBnodeName.CommandArgument = item.IdCommunity & "," & item.CurrentPath
                LBnodeName.Visible = Not item.Details.Permissions.AccessTo
                LNBnodeName.Visible = item.Details.Permissions.AccessTo
                CTRLmenu.InitializeControl(item)
        End Select
    End Sub
    Public Sub HideItems()
        LBnodeName.Visible = True
        LNBnodeName.Visible = False
        CTRLmenu.Visible = False
    End Sub
#End Region

End Class