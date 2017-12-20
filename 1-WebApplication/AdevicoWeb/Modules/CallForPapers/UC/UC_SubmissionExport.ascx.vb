Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Helpers.Export
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports System.Linq
Imports System.Collections.Generic

Public Class UC_SubmissionExport
    Inherits BaseControl
    Implements IViewSubmissionExport

#Region "Context"
    Private _Presenter As SubmissionExportPresenter
    Private ReadOnly Property CurrentPresenter() As SubmissionExportPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New SubmissionExportPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property CallType As CallForPaperType Implements IViewSubmissionExport.CallType
        Get
            Return ViewStateOrDefault("CallType", CallForPaperType.CallForBids)
        End Get
        Set(value As CallForPaperType)
            ViewState("CallType") = value
        End Set
    End Property
    Private Property IdRevision As Long Implements IViewSubmissionExport.IdRevision
        Get
            Return ViewStateOrDefault("IdRevision", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdRevision") = value
        End Set
    End Property
    Private Property IdSubmission As Long Implements IViewSubmissionExport.IdSubmission
        Get
            Return ViewStateOrDefault("IdSubmission", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdSubmission") = value
        End Set
    End Property
    Private Property isContainer As Boolean Implements IViewSubmissionExport.isContainer
        Get
            Return ViewStateOrDefault("isContainer", True)
        End Get
        Set(value As Boolean)
            ViewState("isContainer") = value
            Me.LTcontainerRenderO.Visible = value
            Me.LTcontainerRenderC.Visible = value
        End Set
    End Property

    Private Property isSubmissionOwner As Boolean Implements IViewSubmissionExport.isSubmissionOwner
        Get
            Return ViewStateOrDefault("isSubmissionOwner", False)
        End Get
        Set(value As Boolean)
            ViewState("isSubmissionOwner") = value
        End Set
    End Property
    Private Property RaiseContainerEvent As Boolean Implements IViewSubmissionExport.RaiseContainerEvent
        Get
            Return ViewStateOrDefault("RaiseContainerEvent", True)
        End Get
        Set(value As Boolean)
            ViewState("RaiseContainerEvent") = value
        End Set
    End Property
    Private Property SubmissionOwner As String Implements IViewSubmissionExport.SubmissionOwner
        Get
            Return ViewStateOrDefault("SubmissionOwner", "")
        End Get
        Set(value As String)
            ViewState("SubmissionOwner") = value
        End Set
    End Property
    Private Property SkinDetails As lm.Comol.Core.DomainModel.Helpers.ExternalPageContext Implements IViewSubmissionExport.SkinDetails
        Get
            Return ViewStateOrDefault("SkinDetails", New lm.Comol.Core.DomainModel.Helpers.ExternalPageContext())
        End Get
        Set(value As lm.Comol.Core.DomainModel.Helpers.ExternalPageContext)
            ViewState("SkinDetails") = value
        End Set
    End Property
    Private Property UserLanguageCode As String Implements IViewSubmissionExport.UserLanguageCode
        Get
            Return ViewStateOrDefault("UserLanguageCode", PageUtility.LinguaCode)
        End Get
        Set(value As String)
            ViewState("UserLanguageCode") = value
        End Set
    End Property
    Private Property IdUserSubmitter As Integer Implements IViewSubmissionExport.IdUserSubmitter
        Get
            Return ViewStateOrDefault("IdUserSubmitter", 0)
        End Get
        Set(value As Integer)
            ViewState("IdUserSubmitter") = value
        End Set
    End Property
    Private Property DefaultLanguageCode As String Implements IViewSubmissionExport.DefaultLanguageCode
        Get
            Return ViewStateOrDefault("DefaultLanguageCode", PageUtility.LinguaCode)
        End Get
        Set(value As String)
            ViewState("DefaultLanguageCode") = value
        End Set
    End Property
    Private Property AvailableTypes As List(Of lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType) Implements IViewSubmissionExport.AvailableTypes
        Get
            Return ViewStateOrDefault("AvailableTypes", New List(Of lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType))
        End Get
        Set(value As List(Of lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType))
            ViewState("AvailableTypes") = value
        End Set
    End Property
#End Region

#Region "Control"
    Private Property IdCallModule As Integer
        Get
            Return ViewStateOrDefault("IdCallModule", 0)
        End Get
        Set(value As Integer)
            ViewState("IdCallModule") = value
        End Set
    End Property
    Private Property IdCallCommunity As Integer
        Get
            Return ViewStateOrDefault("IdCallCommunity", 0)
        End Get
        Set(value As Integer)
            ViewState("IdCallCommunity") = value
        End Set
    End Property
    Public ReadOnly Property CookieName() As String
        Get
            Return "COMOL_submissionprintFileDownloadToken"
        End Get
    End Property

    Public Property WebOnlyRender() As Boolean
        Get
            Return ViewStateOrDefault("WebOnlyRender", False)
        End Get
        Set(value As Boolean)
            ViewState("WebOnlyRender") = value
        End Set
    End Property

    Public ReadOnly Property DisplayMessageToken(ByVal fileType As ExportFileType) As String
        Get
            Return Resource.getValue("DisplayMessageToken." & fileType.ToString)
        End Get
    End Property
    Protected ReadOnly Property DisplayTitleToken() As String
        Get
            Return Resource.getValue("DisplayMessageToken.Title")
        End Get
    End Property
    Public Event RefreshContainerEvent()
    Public Event GetHiddenIdentifierValueEvent(ByRef value As String)
    Public Event GetContainerTemplate(ByRef template As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template)
    Public Event GetConfigTemplate(ByRef template As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template)
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_CallSubmission", "Modules", "CallForPapers")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setHyperLink(HYPdownloadzip, True, True)
            .setHyperLink(HYPdownloadrtf, True, True)
            .setHyperLink(HTPdownloadpdf, True, True)
            .setHyperLink(HYPexportEvaluations, True, True)
            .setButton(BTNdownloadpdf, True, , , True)
            .setButton(BTNdownloadrtf, True, , , True)
            '.setButton(BTNdownloadzip, True, , , True)
            .setButton(BTNexportEvaluations, True, , , True)

        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(isOwner As Boolean, idUser As Integer, owner As String, idCall As Long, idSubmission As Long, idRevision As Long, idModule As Integer, idCommunity As Integer, callType As CallForPaperType) Implements IViewSubmissionExport.InitializeControl
        Me.InitializeControl(isOwner, idUser, owner, idCall, idSubmission, idRevision, idModule, idCommunity, callType, Nothing)
    End Sub
    Public Sub InitializeControl(isOwner As Boolean, idUser As Integer, owner As String, idCall As Long, idSubmission As Long, idRevision As Long, idModule As Integer, idCommunity As Integer, callType As CallForPaperType, loadTypes As List(Of lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType)) Implements IViewSubmissionExport.InitializeControl
        isSubmissionOwner = isOwner
        IdCallModule = idModule
        IdCallCommunity = idCommunity
        Me.CurrentPresenter.InitView(idUser, owner, idCall, idSubmission, idRevision, callType, loadTypes)
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission
        Me.MLVcontent.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
        Me.MLVcontent.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplayNone() Implements IViewSubmissionExport.DisplayNone
        Me.MLVcontent.SetActiveView(VIWempty)
    End Sub
    Private Sub LoadFiles(files As List(Of dtoSubmissionAttachment), _
                          availableTypes As List(Of ExportFileType)) _
                      Implements IViewSubmissionExport.LoadFiles

        Me.MLVcontent.SetActiveView(VIWcontrols)

        Dim buttons As Dictionary(Of ExportFileType, Button) = GetButtons()
        Dim hyperlinks As Dictionary(Of ExportFileType, HyperLink) = GetHyperLinks()
        Dim oButton As Button, oHyperlink As HyperLink

        For Each fileType As ExportFileType In _
            [Enum].GetValues(GetType(ExportFileType)).Cast(Of ExportFileType).ToList().Where(Function(f) f <> ExportFileType.none AndAlso f <> ExportFileType.xls AndAlso f <> ExportFileType.csv).ToList

            Try
                oButton = buttons(fileType)
            Catch ex As Exception
                oButton = New Button()
            End Try

            oHyperlink = hyperlinks(fileType)

            oButton.Visible = False
            oHyperlink.Visible = False

            If availableTypes.Contains(fileType) Then
                Dim dto As dtoSubmissionAttachment = files.Where(Function(f) f.Type = fileType).FirstOrDefault()


                ''ToDo: IMPORTANTE! Togliere la riga seguente x pubblicazioni!!!
                'dto = Nothing

                If (fileType <> ExportFileType.zip) AndAlso (IsNothing(dto) OrElse WebOnlyRender) Then
                    oButton.Visible = True
                Else
                    If IsNothing(dto) Then
                        oHyperlink.Visible = False
                    Else
                        oHyperlink.Visible = True
                        oHyperlink.NavigateUrl = lm.Comol.Core.FileRepository.Domain.RootObject.DownloadFromModule( _
                            PageUtility.ApplicationUrlBase, _
                            dto.Item.Id, _
                            0, _
                            dto.Item.DisplayName, _
                            lm.Comol.Core.FileRepository.Domain.DisplayMode.downloadOrPlay, _
                            PageUtility.UniqueGuidSession.ToString, _
                            IdCallModule, _
                            dto.ModuleLinkId, _
                            False)

                        ' lm.Comol.Core.BaseModules.Repository.Domain.RootObject.DownloadFileFromModule(dto.File.Id, PageUtility.CurrentContext.UserContext.CurrentUserID, PageUtility.LinguaCode, PageUtility.UniqueGuidSession, IdCallModule, IdCallCommunity, dto.ModuleLinkId, False)
                    End If
                End If
            End If
        Next
        'SPNseparator.Visible = (availableTypes.Contains(ExportFileType.zip) AndAlso (availableTypes.Contains(ExportFileType.rtf) OrElse availableTypes.Contains(ExportFileType.pdf)))
        SPNseparator.Visible = (availableTypes.Contains(ExportFileType.zip) AndAlso availableTypes.Contains(ExportFileType.pdf))
    End Sub
    Private Sub RefreshContainer() Implements IViewSubmissionExport.RefreshContainer
        RaiseEvent RefreshContainerEvent()
    End Sub
    Private Function GetTemplate(context As lm.Comol.Core.DomainModel.Helpers.ExternalPageContext, userLanguageCode As String, defaultLanguageCode As String) As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template Implements IViewSubmissionExport.GetTemplate
        Dim oService As New lm.Comol.Modules.Standard.Skin.Business.ServiceSkin(Me.PageUtility.CurrentContext)

        Dim configTemplate As New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template
        RaiseEvent GetConfigTemplate(configTemplate)

        'Puoi evitare anche l'IF ed usare direttamente questa:
        'Me.BaseUrl 
        Return oService.GetTemplateCommunitySkin(PageUtility.ApplicationUrlBase, SystemSettings.SkinSettings.SkinVirtualPath, Me.SystemSettings.DefaultLanguage.Codice, Me.PageUtility.CurrentUser.Lingua.Codice, "", context.Skin.IdCommunity, context.Skin.IdOrganization, context.Skin.IdSkin, configTemplate, SystemSettings.DocTemplateSettings.FooterFontSize)

        'If context.Skin.IdSkin = 0 Then
        '    Return oService.GetCommunityTemplate(context.Skin.IdCommunity, context.Skin.IdOrganization, defaultLanguageCode, userLanguageCode, "", PageUtility.ApplicationUrlBase, PageUtility.SystemSettings.SkinSettings.SkinVirtualPath)
        'Else
        '    Return oService.GetSkinTemplate(context.Skin.IdSkin, defaultLanguageCode, userLanguageCode, "", PageUtility.ApplicationUrlBase, SystemSettings.SkinSettings.SkinVirtualPath)
        'End If

        'Return oService.ge
    End Function
    Private Function GetTemplate() As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template Implements IViewSubmissionExport.GetTemplate
        Dim template As New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template
        RaiseEvent GetContainerTemplate(template)
        Return template
    End Function


#End Region

#Region "Control"
    Private Function GetButtons() As Dictionary(Of ExportFileType, Button)
        Dim items As New Dictionary(Of ExportFileType, Button)
        items.Add(ExportFileType.pdf, BTNdownloadpdf)
        'items.Add(ExportFileType.rtf, BTNdownloadrtf)
        'items.Add(ExportFileType.zip, BTNdownloadzip)
        items.Add(ExportFileType.xls, BTNexportEvaluations)
        items.Add(ExportFileType.xml, BTNexportEvaluations)
        Return items
    End Function
    Private Function GetHyperLinks() As Dictionary(Of ExportFileType, HyperLink)
        Dim items As New Dictionary(Of ExportFileType, HyperLink)
        items.Add(ExportFileType.pdf, HTPdownloadpdf)
        'items.Add(ExportFileType.rtf, HYPdownloadrtf)
        items.Add(ExportFileType.zip, HYPdownloadzip)
        items.Add(ExportFileType.xls, HYPexportEvaluations)
        items.Add(ExportFileType.xml, HYPexportEvaluations)
        Return items
    End Function
    'Private Sub BTNdownloadrtf_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNdownloadrtf.Click
    '    Response.Clear()
    '    Dim fileType As ExportFileType = ExportFileType.rtf
    '    Dim fileName As String = String.Format(Resource.getValue("SubmissionTranslations." & IIf(CallType = CallForPaperType.CallForBids, SubmissionTranslations.SubmissionCallFileName.ToString, SubmissionTranslations.SubmissionRequestFileName.ToString)), Me.SubmissionOwner)
    '    Try

    '        Dim baseFilePath As String = ""
    '        If Me.SystemSettings.File.Materiale.DrivePath = "" Then
    '            baseFilePath = Server.MapPath(Me.PageUtility.BaseUrl & Me.SystemSettings.File.Materiale.VirtualPath)
    '        Else
    '            baseFilePath = Me.SystemSettings.File.Materiale.DrivePath
    '        End If

    '        Dim translations As New Dictionary(Of SubmissionTranslations, String)
    '        For Each name As String In [Enum].GetNames(GetType(SubmissionTranslations))
    '            translations.Add([Enum].Parse(GetType(SubmissionTranslations), name), Me.Resource.getValue("SubmissionTranslations." & name))
    '        Next

    '        Dim value As String = ""
    '        RaiseEvent GetHiddenIdentifierValueEvent(value)

    '        Dim template As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template
    '        If isSubmissionOwner Then
    '            template = GetTemplate()
    '        Else
    '            template = GetTemplate(SkinDetails, UserLanguageCode, DefaultLanguageCode)
    '        End If



    '        Dim doc As iTextSharp.text.Document = CurrentPresenter.ExportToRtf(WebOnlyRender, IdSubmission, IdRevision, baseFilePath, fileName, translations, Response, New HttpCookie(CookieName, value), template)

    '    Catch de As Exception

    '    End Try
    '    Response.End()
    'End Sub
    Private Sub BTNdownloadpdf_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNdownloadpdf.Click
        Response.Clear()

        Dim fileType As ExportFileType = ExportFileType.pdf
        Dim fileName As String = String.Format(Resource.getValue("SubmissionTranslations." & IIf(CallType = CallForPaperType.CallForBids, SubmissionTranslations.SubmissionCallFileName.ToString, SubmissionTranslations.SubmissionRequestFileName.ToString)), Me.SubmissionOwner)

        Try

            Dim baseFilePath As String = ""
            If Me.SystemSettings.File.Materiale.DrivePath = "" Then
                baseFilePath = Server.MapPath(Me.PageUtility.BaseUrl & Me.SystemSettings.File.Materiale.VirtualPath)
            Else
                baseFilePath = Me.SystemSettings.File.Materiale.DrivePath
            End If

            Dim translations As New Dictionary(Of SubmissionTranslations, String)
            For Each name As String In [Enum].GetNames(GetType(SubmissionTranslations))
                translations.Add([Enum].Parse(GetType(SubmissionTranslations), name), Me.Resource.getValue("SubmissionTranslations." & name))
            Next

            Dim value As String = ""
            RaiseEvent GetHiddenIdentifierValueEvent(value)

            Dim template As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template
            If isSubmissionOwner Then
                template = GetTemplate()
            Else
                template = GetTemplate(SkinDetails, UserLanguageCode, DefaultLanguageCode)
            End If

            Dim dtImagePath As String = Me.SystemSettings.DocTemplateSettings.BasePath

            CurrentPresenter.ExportToPdf(
                WebOnlyRender,
                IdSubmission,
                IdRevision,
                baseFilePath,
                dtImagePath,
                fileName,
                translations,
                Response,
                New HttpCookie(CookieName, value),
                template)

        Catch de As Exception

        End Try
        Response.End()
    End Sub
    'Private Sub BTNdownloadzip_Click(sender As Object, e As System.EventArgs) Handles BTNdownloadzip.Click

    'End Sub
    Public Function GetControlScript(ByVal hiddenIdentifier As String)
        Dim s As String = ""
        s &= "var TokenHiddenFieldId = """ & hiddenIdentifier & """;" & vbCrLf
        s &= "var CookieName = """ & CookieName & """;" & vbCrLf
        s &= "var DisplayTitle =""" & DisplayTitleToken & """;" & vbCrLf
        s &= "var DisplayMessageZip = """ & DisplayMessageToken(ExportFileType.zip) & """;" & vbCrLf
        s &= "var DisplayMessagePdf = """ & DisplayMessageToken(ExportFileType.pdf) & """;" & vbCrLf
        's &= "var DisplayMessageRtf = """ & DisplayMessageToken(ExportFileType.rtf) & """;" & vbCrLf
        s &= "var DisplayMessageXls = """ & DisplayMessageToken(ExportFileType.xls) & """;" & vbCrLf
        s &= "var DisplayMessageCsv = """ & DisplayMessageToken(ExportFileType.csv) & """;" & vbCrLf & vbCrLf
        s &= "var DisplayMessageXml = """ & DisplayMessageToken(ExportFileType.xml) & """;" & vbCrLf & vbCrLf
        s &= "var fileDownloadCheckTimer;" & vbCrLf
        s &= "function blockUIForDownload(fileType) {" & vbCrLf
        s &= "    var token = new Date().getTime(); //use the current timestamp as the token value" & vbCrLf
        s &= "    var message = """";" & vbCrLf
        s &= "    if (fileType == 1)" & vbCrLf
        s &= "        message = DisplayMessageZip;" & vbCrLf
        s &= "    else if (fileType == 2)" & vbCrLf
        s &= "        message = DisplayMessagePdf;" & vbCrLf
        s &= "    else if (fileType == 3)" & vbCrLf
        s &= "        message = DisplayMessageRtf;" & vbCrLf
        s &= "    else if (fileType == 4)" & vbCrLf
        s &= "        message = DisplayMessageXls;" & vbCrLf
        s &= "    else if (fileType == 5)" & vbCrLf
        s &= "        message = DisplayMessageXml;" & vbCrLf
        s &= "    else if (fileType == 6)" & vbCrLf
        s &= "        message = DisplayMessageCsv;" & vbCrLf
        s &= "     $(""input[id='"" + TokenHiddenFieldId + ""']"").val(token);" & vbCrLf
        s &= "    $.blockUI({ message: "" '"" + message + ""'"", title: DisplayTitle, draggable: true, theme: true });" & vbCrLf
        s &= "    fileDownloadCheckTimer = window.setInterval(function () {" & vbCrLf
        s &= "        var cookieValue = $.cookie(CookieName);" & vbCrLf
        s &= "        if (cookieValue == token)" & vbCrLf
        s &= "            finishDownload();" & vbCrLf
        s &= "    }, 1000);" & vbCrLf
        s &= "}" & vbCrLf & vbCrLf

        s &= "function finishDownload() {" & vbCrLf
        s &= "window.clearInterval(fileDownloadCheckTimer);" & vbCrLf
        s &= "    $.cookie(CookieName, null); //clears this cookie value" & vbCrLf
        s &= "    $.unblockUI();" & vbCrLf
        s &= "}" & vbCrLf
        Return s
    End Function


    Public Sub Download()
        DownloadPdf()
    End Sub

    Private Sub DownloadPdf()
        Response.Clear()

        Dim fileType As ExportFileType = ExportFileType.pdf
        Dim fileName As String = String.Format(Resource.getValue("SubmissionTranslations." & IIf(CallType = CallForPaperType.CallForBids, SubmissionTranslations.SubmissionCallFileName.ToString, SubmissionTranslations.SubmissionRequestFileName.ToString)), Me.SubmissionOwner)

        Try

            Dim baseFilePath As String = ""
            If Me.SystemSettings.File.Materiale.DrivePath = "" Then
                baseFilePath = Server.MapPath(Me.PageUtility.BaseUrl & Me.SystemSettings.File.Materiale.VirtualPath)
            Else
                baseFilePath = Me.SystemSettings.File.Materiale.DrivePath
            End If

            Dim translations As New Dictionary(Of SubmissionTranslations, String)
            For Each name As String In [Enum].GetNames(GetType(SubmissionTranslations))
                translations.Add([Enum].Parse(GetType(SubmissionTranslations), name), Me.Resource.getValue("SubmissionTranslations." & name))
            Next

            Dim value As String = ""
            RaiseEvent GetHiddenIdentifierValueEvent(value)

            Dim template As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template
            If isSubmissionOwner Then
                template = GetTemplate()
            Else
                template = GetTemplate(SkinDetails, UserLanguageCode, DefaultLanguageCode)
            End If

            If (IsNothing(template) _
                OrElse IsNothing(template.Body) _
                OrElse String.IsNullOrEmpty(template.Body.Text)) Then

                template = GetTemplate(SkinDetails, UserLanguageCode, DefaultLanguageCode)
            End If

            Dim dtImagePath As String = Me.SystemSettings.DocTemplateSettings.BasePath

            CurrentPresenter.ExportToPdf(WebOnlyRender, IdSubmission, IdRevision, baseFilePath, dtImagePath, fileName, translations, Response, New HttpCookie(CookieName, value), template)
        Catch de As Exception

        End Try
        Response.End()
    End Sub
#End Region

    Public Sub ShowForDraft()
        Me.BTNdownloadpdf.Visible = False
        Me.HTPdownloadpdf.Visible = False
    End Sub
End Class