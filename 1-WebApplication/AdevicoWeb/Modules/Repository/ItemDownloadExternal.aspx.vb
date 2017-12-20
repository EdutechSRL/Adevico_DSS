Public Class ItemDownloadExternal
    Inherits FRdownloadErrorPageBase
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Not Page.IsPostBack Then
        '    InitializeMaster()
        'End If
    End Sub

#Region "inherits"
    Public Overrides Sub BindNoPermessi()

    End Sub
    Protected Friend Overrides Sub DisplaySessionTimeout()

    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setHyperLink(HYPbackToPreviousUrl, False, True)
        End With
    End Sub

    Protected Overrides Sub InitializeMaster(context As lm.Comol.Core.DomainModel.Helpers.ExternalPageContext)
        Me.Master.Page_Title = Resource.getValue("Repository.Download.PageTitle")
        If Not IsNothing(context) Then
            context.ShowDocType = True
            Master.InitializeMaster(context)
        End If
    End Sub
    Protected Overrides Sub DisplayBackUrl(url As String)
        If String.IsNullOrWhiteSpace(url) Then
            DVmenu.Visible = False
        Else
            DVmenu.Visible = True
            HYPbackToPreviousUrl.NavigateUrl = url
        End If
    End Sub
    Protected Overrides Sub DisplayMessage(message As String, errorType As lm.Comol.Core.FileRepository.Domain.DownloadErrorType)
        Dim t As lm.Comol.Core.DomainModel.Helpers.MessageType = lm.Comol.Core.DomainModel.Helpers.MessageType.error
        Select Case errorType
            Case lm.Comol.Core.FileRepository.Domain.DownloadErrorType.notLoggedIn
                t = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
            Case lm.Comol.Core.FileRepository.Domain.DownloadErrorType.noPermissions
                t = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
        End Select
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(message, t)
    End Sub
   
    Protected Overrides Function GetFilenameRender(fullname As String, fileExtension As String) As String
        Dim template As String = LTtemplateFile.Text
        If fileExtension.StartsWith(".") Then
            template = Replace(template, "#ico#", LTitemExtensionCssClass.Text & Replace(fileExtension, ".", ""))
        Else
            template = Replace(template, "#ico#", LTitemExtensionCssClass.Text)
        End If
        template = Replace(template, "#name#", fullname)
        Return template
    End Function
    Protected Overrides Function IsModalPage() As Boolean
        Return False
    End Function
    Protected Overrides Function IsExternalPage() As Boolean
        Return True
    End Function
    Protected Overrides Function GetHeader() As UC_RepositoryHeader
        Return CTRLheader
    End Function
#End Region

#Region "Internal"
    Private Sub ItemDownloadExternal_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Master.ShowDocType = True
    End Sub
#End Region

   
End Class