Public Class HTTPhandler_IconFile
    Implements System.Web.IHttpHandler
    Implements System.Web.SessionState.IRequiresSessionState
    Implements iHTTPhandler_comunita


    Public ReadOnly Property IsReusable() As Boolean Implements System.Web.IHttpHandler.IsReusable
        Get
            Return True
        End Get
    End Property

    Public Sub ProcessRequest(ByVal context As System.Web.HttpContext) Implements System.Web.IHttpHandler.ProcessRequest
        Dim oHTTPhandler As New HTTPhandler_Utility(context)

        oHTTPhandler.ClearHTTPcontext()
        If Me.isIconFileHandler(oHTTPhandler) Then
            Me.DefineFileToDownload(oHTTPhandler)
        Else
            Me.SendToErrorPage(oHTTPhandler)
        End If
    End Sub

    Public Function isIconFileHandler(ByVal oHandler As HTTPhandler_Utility) As Boolean Implements iHTTPhandler_comunita.isQueryStringCorrect
        Return oHandler.HttpContext.Request.Path.EndsWith(oHandler.SystemSettings.Extension.ExtensionToShow)
    End Function

    Public Sub DefineFileToDownload(ByVal oHandler As HTTPhandler_Utility) Implements iHTTPhandler_comunita.DefineFileToDownload
        Dim RequestUrlBase As String = oHandler.HttpContext.Request.Url.AbsoluteUri
        Dim IconName As String = RequestUrlBase.Substring(RequestUrlBase.LastIndexOf("/") + 1)
        IconName = "." & Replace(IconName, oHandler.SystemSettings.Extension.ExtensionToShow, "")
        Dim IconFileName As String = ""

        IconFileName = oHandler.BaseUrlDrivePath & oHandler.SystemSettings.Extension.FindIconImage(IconName)

        oHandler.ClearHTTPcontext()
        oHandler.DownloadFile(0, IconFileName)
    End Sub


    Private Sub SendToErrorPage(ByVal oHandler As HTTPhandler_Utility)
        oHandler.DownloadErrorMessage(ErrorSettings.ErrorType.None, True)
    End Sub
End Class