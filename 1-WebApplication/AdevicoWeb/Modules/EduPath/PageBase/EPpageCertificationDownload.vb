Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Imports lm.Comol.Modules.EduPath.Domain
Imports lm.Comol.Modules.EduPath.Presentation

Public MustInherit Class EPpageCertificationDownload
    Inherits EPpageBaseCertification
    Implements IViewCertificationDownload


#Region "Context"
    Private _Presenter As CertificationDownloadPresenter
    Public ReadOnly Property CurrentPresenter() As CertificationDownloadPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CertificationDownloadPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"

#Region "Preload"
    Protected Friend ReadOnly Property PreloadUniqueId As Guid Implements IViewCertificationDownload.PreloadUniqueId
        Get
            Return GetGuidFromQueryString("uniqueId", Guid.Empty)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadRestore As Boolean Implements IViewCertificationDownload.PreloadRestore
        Get
            Return GetBooleanFromQueryString("restore", False)
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadReferenceTime As Long Implements IViewCertificationDownload.PreloadReferenceTime
        Get
            Return GetLongFromQueryString("rt", DateTime.Now.Ticks)
        End Get
    End Property

#End Region

    Private Property ForUserId As Int32 Implements IViewCertificationDownload.ForUserId
        Get
            Return ViewStateOrDefault("ForUserId", 0)
        End Get
        Set(value As Int32)
            ViewState("ForUserId") = value
        End Set
    End Property
    Private Property IdCommunityContainer As Int32 Implements IViewCertificationDownload.IdCommunityContainer
        Get
            Return ViewStateOrDefault("IdCommunityContainer", 0)
        End Get
        Set(value As Int32)
            ViewState("IdCommunityContainer") = value
        End Set
    End Property
    Private Property IdPath As Long Implements IViewCertificationDownload.IdPath
        Get
            Return ViewStateOrDefault("IdPath", 0)
        End Get
        Set(value As Long)
            ViewState("IdPath") = value
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Internal"
    Private ReadOnly Property TemplateBaseUrl As String
        Get
            Return Me.Request.Url.AbsoluteUri.Replace( _
             Me.Request.Url.PathAndQuery, "") & Me.BaseUrl & MyBase.SystemSettings.DocTemplateSettings.BaseUrl
        End Get
    End Property
    Private ReadOnly Property GetLongFromQueryString(key As String, dValue As Long) As Long
        Get
            Dim idItem As Long = dValue
            If Not String.IsNullOrEmpty(Request.QueryString(key)) Then
                Long.TryParse(Request.QueryString(key), idItem)
            End If
            Return idItem
        End Get
    End Property
    Private ReadOnly Property GetIntFromQueryString(key As String, dValue As Integer) As Long
        Get
            Dim result As Integer = dValue
            If Not String.IsNullOrEmpty(Request.QueryString(key)) Then
                Integer.TryParse(Request.QueryString(key), result)
            End If
            Return result
        End Get
    End Property
    Private ReadOnly Property GetGuidFromQueryString(key As String, dValue As Guid) As Guid
        Get
            Dim idItem As Guid = dValue
            If Not String.IsNullOrEmpty(Request.QueryString(key)) Then
                Guid.TryParse(Request.QueryString(key), idItem)
            End If
            Return idItem
        End Get
    End Property
    Private ReadOnly Property GetBooleanFromQueryString(key As String, dValue As Boolean) As Boolean
        Get
            Dim idItem As Boolean = dValue
            If Not String.IsNullOrEmpty(Request.QueryString(key)) Then
                Boolean.TryParse(Request.QueryString(key), idItem)
            End If
            Return idItem
        End Get
    End Property
    Private ReadOnly Property CertificationFilePath As String
        Get
            Dim baseFilePath As String = ""
            If Me.SystemSettings.File.UserCertifications.DrivePath = "" Then
                baseFilePath = Server.MapPath(Me.PageUtility.BaseUrl & Me.SystemSettings.File.UserCertifications.VirtualPath)
            Else
                baseFilePath = Me.SystemSettings.File.UserCertifications.DrivePath
            End If
            Return baseFilePath
        End Get
    End Property
#End Region

#Region "Inherits"
    Public Overrides Sub BindDati()
        If PreloadRestore Then
            CurrentPresenter.InitView(IsOnModalWindow, PreloadTime, PreloadTimeValidity, PreloadMac, PreloadIdPath, PreloadIdActivity, PreloadIdSubactivity, PreloadIdUser)
        Else
            CurrentPresenter.InitView(IsOnModalWindow, PreloadIdPath, PreloadIdActivity, PreloadIdSubactivity, PreloadUniqueId, PreloadIdUser)
        End If
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Certification", "EduPath")
    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

#End Region

#Region "Implements"

    Private Function GetQuizInfos(idItems As List(Of Long), idUser As Integer, isEvaluable As Boolean) As List(Of dtoQuizInfo) Implements IViewCertificationDownload.GetQuizInfos
        Dim items As New List(Of dtoQuizInfo)
        For Each idQuestionaire As Integer In idItems
            Dim q As COL_Questionario.LazyQuestionnaire = ServiceQS.GetItem(Of COL_Questionario.LazyQuestionnaire)(idQuestionaire)
            If Not IsNothing(q) Then
                Dim evaluable As Boolean = False
                evaluable = (isEvaluable AndAlso q.IdType <> COL_Questionario.QuestionnaireType.Poll AndAlso q.IdType <> COL_Questionario.QuestionnaireType.QuestionLibrary AndAlso q.IdType <> COL_Questionario.QuestionnaireType.Model) _
                    OrElse (Not isEvaluable AndAlso q.IdType = COL_Questionario.QuestionnaireType.RandomMultipleAttempts)
                items.Add(New dtoQuizInfo() With {.IdQuestionnaire = q.Id, .Evaluable = evaluable, .EvaluationScale = q.EvaluationScale, .MinScore = q.MinScore, .Name = ServiceQS.GetItemName(q.Id, PageUtility.LinguaID), .Attempts = (From a In ServiceQS.GetQuestionnaireAttempts(q.Id, idUser, 0) Select New dtoQuizAttemptInfo() With {.Id = a.Id, .IdQuestionnaire = a.IdQuestionnnaire, .CorrectAnswers = a.CorrectAnswers, .Completed = a.CompletedOn.HasValue, .CompletedOn = a.CompletedOn, .QuestionsCount = a.QuestionsCount, .QuestionsSkipped = a.QuestionsSkipped, .RelativeScore = a.RelativeScore, .Score = a.Score, .UngradedAnswers = a.UngradedAnswers, .WrongAnswers = a.WrongAnswers, .Passed = a.RelativeScore >= q.MinScore}).ToList()})
            End If
        Next
        Return items
    End Function

    Private Sub GenerateAndDownload(idPath As Long, idSubActivity As Long, allowEmptyPlaceholders As Boolean, idUser As Integer, fileName As String, type As lm.Comol.Core.Certifications.CertificationType, saveFile As Boolean, saveAction As Boolean) Implements IViewCertificationDownload.GenerateAndDownload
        GenerateCertificationFile(idPath, idSubActivity, allowEmptyPlaceholders, idUser, fileName, type, saveFile, saveAction, False)
    End Sub

    Private Sub DownloadCertification(allowEmptyPlaceholders As Boolean, idUser As Integer, fileName As String, type As lm.Comol.Core.Certifications.CertificationType, saveFile As Boolean, saveAction As Boolean, uniqueId As Guid, extension As String) Implements IViewCertificationDownload.DownloadCertification
        GenerateCertificationFile(IdPath, PreloadIdSubactivity, allowEmptyPlaceholders, idUser, fileName, type, saveFile, saveAction, True, uniqueId.ToString, extension)
    End Sub
    Private Function GetDefaulFileName() As String Implements IViewCertificationDownload.GetDefaulFileName
        Return Resource.getValue("Certification.FileName")
    End Function
#End Region

#Region "Internal"
    Private Function GenerateCertificationFile(idPath As Long, idSubActivity As Long, allowEmptyPlaceholders As Boolean, idUser As Integer, webFileName As String, ByVal certType As lm.Comol.Core.Certifications.CertificationType, ByVal saveFile As Boolean, ByVal saveAction As Boolean, ByVal onlyDownload As Boolean, Optional ByVal fileUniqueId As String = "", Optional ByVal fileExtension As String = "") As lm.Comol.Core.Certifications.CertificationError
        Dim result As lm.Comol.Core.Certifications.CertificationError = lm.Comol.Core.Certifications.CertificationError.None

        Try
            Dim isValidFileName As Boolean = False
            Dim storedFileName As String = Me.CertificationFilePath
            If Not String.IsNullOrEmpty(storedFileName) AndAlso Not String.IsNullOrEmpty(fileUniqueId) AndAlso Not fileUniqueId = Guid.Empty.ToString Then
                storedFileName &= IIf(storedFileName.EndsWith("/") OrElse storedFileName.EndsWith("\"), "", "\") & idUser.ToString() & "\" & fileUniqueId & ".cer"
                isValidFileName = True
            End If
            If Not String.IsNullOrEmpty(storedFileName) AndAlso Not String.IsNullOrEmpty(fileUniqueId) AndAlso lm.Comol.Core.File.Exists.File(storedFileName) Then
                If String.IsNullOrEmpty(fileExtension) Then
                    fileExtension = "." & Helpers.Export.ExportFileType.pdf.ToString().ToLower
                End If
                DownloadExistingFile(webFileName, storedFileName, fileExtension)
            Else


                Dim fileName As String = ""
                Dim err As lm.Comol.Core.Certifications.CertificationError = SaveCertificationFile(webFileName, certType, allowEmptyPlaceholders, idUser, fileName)
                If String.IsNullOrEmpty(fileName) Then
                    Response.AppendCookie(GetDownloadCookie())
                    DisplayMessage(Resource.getValue("IViewCertificationDownload.CertificationError." & err.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
                Else
                    If String.IsNullOrEmpty(fileExtension) Then
                        fileExtension = "." & Helpers.Export.ExportFileType.pdf.ToString().ToLower
                    End If
                    If Not webFileName.EndsWith(fileExtension) Then
                        webFileName &= fileExtension
                    End If
                    Dim contentType As String = Response.ContentType
                    Response.BufferOutput = False
                    Response.ClearHeaders()
                    Response.ClearContent()
                    Response.AddHeader("Content-Disposition", "attachment; filename=" & webFileName)
                    Response.ContentType = "application/pdf"


                    Response.AppendCookie(GetDownloadCookie())

                    Dim chunkSize As Integer = 64
                    Dim offset As Integer = 0, read As Integer = 0

                    DisplayMessage(Resource.getValue("IViewCertificationDownload.NotDownloadExistingFile"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)

                    If Not lm.Comol.Core.File.TransmitFactory.TransmitFile(fileName, Response) = lm.Comol.Core.File.FileMessage.Read Then
                        'RaiseEvent ItemActionResult(lm.Comol.Core.Certifications.CertificationError.TransmittingFile, saveFile, True)
                        Response.ContentType = contentType
                        Response.Headers.Remove("Content-Disposition")
                    ElseIf (saveAction) Then
                        CurrentPresenter.ExecuteAction(idPath, idSubActivity, StatusStatistic.CompletedPassed)
                    End If

                    Context.ApplicationInstance.CompleteRequest()
                End If
            End If
        Catch ex As Exception
            '  NotifyError(PageUtility.SystemSettings, ex)
            result = lm.Comol.Core.Certifications.CertificationError.Unknown
        End Try
        Return result
    End Function

    Private Function SaveCertificationFile(ByVal webFileName As String, ByVal crType As lm.Comol.Core.Certifications.CertificationType, ByVal allowEmptyPlaceHolders As Boolean, ByVal idUser As Integer, ByRef filename As String, Optional template As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template = Nothing) As lm.Comol.Core.Certifications.CertificationError
        Dim result As lm.Comol.Core.Certifications.CertificationError = lm.Comol.Core.Certifications.CertificationError.None
        Dim allowSave As Boolean = True
        Try
            Dim baseFilePath As String = Me.CertificationFilePath
            Dim language As lm.Comol.Core.DomainModel.Language = CurrentPresenter.GetUserLanguage(idUser)
            If Not String.IsNullOrEmpty(baseFilePath) Then
                baseFilePath &= IIf(baseFilePath.EndsWith("/") OrElse baseFilePath.EndsWith("\"), "", "\") & idUser.ToString() & "\"
                If Not lm.Comol.Core.File.Exists.Directory(baseFilePath) Then
                    allowSave = lm.Comol.Core.File.Create.Directory(baseFilePath)
                End If
                If allowSave Then
                    If IsNothing(template) Then
                        template = CurrentPresenter.FillDataIntoTemplate(PreloadIdSubactivity, TemplateBaseUrl, PageUtility.SystemSettings.Presenter.PortalDisplay.LocalizeIstanceName(language.Id), result)
                    End If


                    If result = lm.Comol.Core.Certifications.CertificationError.None OrElse (allowEmptyPlaceHolders AndAlso (result = lm.Comol.Core.Certifications.CertificationError.EmptyTemplateItem OrElse result = lm.Comol.Core.Certifications.CertificationError.EmptyTemplateItems)) Then
                        Dim idFile As Guid = Guid.NewGuid

                        ''ToDo: Export
                        result = lm.Comol.Core.Certifications.CertificationError.SavingFile

                    End If
                Else
                    result = lm.Comol.Core.Certifications.CertificationError.RepositoryError
                End If
            Else
                result = lm.Comol.Core.Certifications.CertificationError.RepositoryError
            End If
        Catch ex As Exception
            result = lm.Comol.Core.Certifications.CertificationError.Unknown
        End Try
        Return result
    End Function
    Private Sub DownloadExistingFile(webFileName As String, storedFileName As String, fileExtension As String)
        Dim contentType As String = Response.ContentType
        Dim cFilename As String = webFileName & fileExtension.ToLower
        Response.AddHeader("Content-Disposition", "attachment; filename=" & cFilename)
        Select Case (fileExtension.ToLower)
            Case ".pdf"
                Response.ContentType = "application/pdf"
            Case ".rtf"
                Response.ContentType = "application/rtf"
        End Select

        Dim query As String = Request.Url.Query
        If query.StartsWith("?") Then
            query = query.Remove(0, 1)
        End If
        Dim cookie As HttpCookie = New HttpCookie("fileDownload", query)
        cookie.Expires = Now.AddMinutes(5)
        Response.AppendCookie(cookie)
        Dim chunkSize As Integer = 64
        Dim offset As Integer = 0, read As Integer = 0
        DisplayMessage(Resource.getValue("IViewCertificationDownload.NotDownloadExistingFile.Download"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
        If Not lm.Comol.Core.File.TransmitFactory.TransmitFile(storedFileName, Response) = lm.Comol.Core.File.FileMessage.Read Then
            'RaiseEvent ItemActionResult(lm.Comol.Core.Certifications.CertificationError.TransmittingFile, saveRequired, True)
            Response.ContentType = contentType
            Response.Headers.Remove("Content-Disposition")
        End If
        Context.ApplicationInstance.CompleteRequest()
        'If Response.IsClientConnected Then
        '    Response.End()
        'End If
    End Sub
    Private Sub RestoreCertificate(allowEmptyPlaceholders As Boolean, idUser As Integer, webFileName As String, type As lm.Comol.Core.Certifications.CertificationType, saveFile As Boolean) Implements IViewCertificationDownload.RestoreCertificate
        Dim fileName As String = ""
        Dim err As lm.Comol.Core.Certifications.CertificationError = SaveCertificationFile(webFileName, type, allowEmptyPlaceholders, idUser, fileName)
        If String.IsNullOrEmpty(fileName) Then
            Response.AppendCookie(GetDownloadCookie())
            DisplayMessage(Resource.getValue("IViewCertificationDownload.CertificationError." & err.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
        Else
            '    Dim contentType As String = Response.ContentType
            '    Dim cFilename As String = CurrentPresenter.ReplaceInvalidFileName(webFileName) & "." & Helpers.Export.ExportFileType.pdf.ToString().ToLower
            '    Response.AddHeader("Content-Disposition", "attachment; filename=" & cFilename)
            '    Response.ContentType = "application/pdf"

            '    Response.AppendCookie(New HttpCookie(CookieName, cookieValue))
            '    Dim chunkSize As Integer = 64
            '    Dim offset As Integer = 0, read As Integer = 0

            '    If Not lm.Comol.Core.File.TransmitFactory.TransmitFile(fileName, Response) = lm.Comol.Core.File.FileMessage.Read Then
            '        RaiseEvent ItemActionResult(lm.Comol.Core.Certifications.CertificationError.TransmittingFile, item.SaveCertificate, True)
            '        Response.ContentType = contentType
            '        Response.Headers.Remove("Content-Disposition")
            '    End If
            '    Context.ApplicationInstance.CompleteRequest()
            Dim contentType As String = Response.ContentType
            Response.BufferOutput = False
            Response.ClearHeaders()
            Response.ClearContent()
            Response.AddHeader("Content-Disposition", "attachment; filename=" & webFileName)
            Response.ContentType = "application/pdf"


            Response.AppendCookie(GetDownloadCookie())

            Dim chunkSize As Integer = 64
            Dim offset As Integer = 0, read As Integer = 0

            DisplayMessage(Resource.getValue("IViewCertificationDownload.NotDownloadExistingFile.Restore"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)

            If Not lm.Comol.Core.File.TransmitFactory.TransmitFile(fileName, Response) = lm.Comol.Core.File.FileMessage.Read Then
                'RaiseEvent ItemActionResult(lm.Comol.Core.Certifications.CertificationError.TransmittingFile, saveFile, True)
                Response.ContentType = contentType
                Response.Headers.Remove("Content-Disposition")
            End If

            Context.ApplicationInstance.CompleteRequest()
        End If

    End Sub
#End Region

   
End Class