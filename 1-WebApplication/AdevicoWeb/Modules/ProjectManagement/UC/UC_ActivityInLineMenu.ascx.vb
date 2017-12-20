Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports System.Linq
Public Class UC_ActivityInLineMenu
    Inherits BaseUserControl

#Region "Internal"
    Protected ReadOnly Property PredecessorsHelperDialogTitleTranslation() As String
        Get
            Return Resource.getValue("PredecessorsHelperDialogTitleTranslation")
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProjectManagement", "Modules", "ProjectManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLinkButton(LNBeditActivity, False, True)
            .setLinkButton(LNBindentActivityToLeft, False, True)
            .setLinkButton(LNBindentActivityToRight, False, True)

            .setLabel(LBgroupAddChildren)
            LBgroupAddChildren.Text = String.Format(LBgroupAddChildren.Text, LTaddGroupTitle.Text)
            .setLabel(LBgroupAddAfter)
            LBgroupAddAfter.Text = String.Format(LBgroupAddAfter.Text, LTaddGroupTitle.Text)
            .setLabel(LBgroupAddBefore)
            LBgroupAddBefore.Text = String.Format(LBgroupAddBefore.Text, LTaddGroupTitle.Text)
            .setLabel(LBgroupAddLinkedAfter)
            LBgroupAddLinkedAfter.Text = String.Format(LBgroupAddLinkedAfter.Text, LTaddGroupTitle.Text)
            .setLinkButton(LNBvirtualDeleteActivity, False, True)
            .setLinkButton(LNBvirtualDeleteActivityWithChildren, False, True)


            .setLabel(LBgroupAddLinkedChildren)
            LBgroupAddLinkedChildren.Text = String.Format(LBgroupAddLinkedChildren.Text, LTaddGroupTitle.Text)
        End With
    End Sub
#End Region

    Public Sub InitializeControl(ByVal idActivity As Long, haschildren As Boolean, ByVal permission As dtoActivityPermission)
        SetInternazionalizzazione()
        For Each l As LinkButton In Controls.OfType(Of LinkButton)()
            l.CommandArgument = idActivity
        Next
        With permission
            LNBeditActivity.Visible = .Edit
            LTindentSeparator.Visible = .ToChild OrElse .ToFather
            LNBindentActivityToLeft.Visible = .ToFather
            LNBindentActivityToRight.Visible = .ToChild

            LTaddChildrenSeparator.Visible = .AddChildren
            LBgroupAddChildren.Visible = .AddChildren
            SPNaddChildren.Visible = .AddChildren

            LTaddLinkedChildrenSeparator.Visible = .AddLinkedChildren
            LBgroupAddLinkedChildren.Visible = .AddLinkedChildren
            SPNaddLinkedChildren.Visible = .AddLinkedChildren


            LTaddAfterSeparator.Visible = .AddActivityAfter
            LBgroupAddAfter.Visible = .AddActivityAfter
            SPNaddAfter.Visible = .AddActivityAfter


            LTaddLinkedAfterSeparator.Visible = .AddLinkedActivityAfter
            LBgroupAddLinkedAfter.Visible = .AddLinkedActivityAfter
            SPNaddLinkedAfter.Visible = .AddLinkedActivityAfter

            LTaddBeforeSeparator.Visible = .AddActivityBefore
            LBgroupAddBefore.Visible = .AddActivityBefore
            SPNaddBefore.Visible = .AddActivityBefore

            LTaddVirtualDeleteSeparator.Visible = .VirtualDelete

            LNBvirtualDeleteActivity.Visible = Not haschildren AndAlso .VirtualDelete
            LNBvirtualDeleteActivityWithChildren.Visible = haschildren AndAlso .VirtualDelete
        End With

    End Sub
End Class