Public Class UC_PredecessorsHelper
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
            .setLiteral(LTpredecessorsHelpLinkTypes)

            .setLabel(LBpredecessorsHelpLink_t)
            .setLabel(LBpredecessorsHelpLink)
            .setLabel(LBpredecessorsHelpActivityId_t)
            .setLabel(LBpredecessorsHelpActivityId)
            .setLabel(LBpredecessorsHelpType_t)
            .setLabel(LBpredecessorsHelpType)
            .setLabel(LBpredecessorsHelpFS_t)
            .setLabel(LBpredecessorsHelpFS)
            .setLabel(LBpredecessorsHelpSS_t)
            .setLabel(LBpredecessorsHelpSS)
            .setLabel(LBpredecessorsHelpFF_t)
            .setLabel(LBpredecessorsHelpFF)
            .setLabel(LBpredecessorsHelpFS_t)
            .setLabel(LBpredecessorsHelpFF)
            .setLabel(LBpredecessorsHelpSF_t)
            .setLabel(LBpredecessorsHelpSF)

            .setLabel(LBpredecessorsHelpLead_t)
            .setLabel(LBpredecessorsHelpLead)

        End With
    End Sub
#End Region

End Class