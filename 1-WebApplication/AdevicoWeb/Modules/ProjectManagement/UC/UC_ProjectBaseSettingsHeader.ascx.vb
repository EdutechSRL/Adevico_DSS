Public Class UC_ProjectBaseSettingsHeader
    Inherits BaseControl

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region
#Region "Control"
    Protected Function GetTranslateDeadlineDateError() As String
        Return Resource.getValue("GetTranslateDeadlineDateError")
    End Function
    Protected Function GetTranslateLastDateError() As String
        Return Resource.getValue("GetTranslateLastDateError")
    End Function

    Protected ReadOnly Property SelectOwnerFromCommunityDialogTitleTranslation() As String
        Get
            Return Resource.getValue("SelectOwnerFromCommunityDialogTitleTranslation")
        End Get
    End Property
    Protected ReadOnly Property SelectOwnerFromMyCommunitiesDialogTitleTranslation() As String
        Get
            Return Resource.getValue("SelectOwnerFromMyCommunitiesDialogTitleTranslation")
        End Get
    End Property
    Protected ReadOnly Property SelectOwnerFromPortalDialogTitleTranslation() As String
        Get
            Return Resource.getValue("SelectOwnerFromPortalDialogTitleTranslation")
        End Get
    End Property
    Protected ReadOnly Property SelectOwnerFromProjectResourceslDialogTitleTranslation() As String
        Get
            Return Resource.getValue("SelectOwnerFromProjectResourceslDialogTitleTranslation")
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

    End Sub
#End Region

End Class