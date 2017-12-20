Imports lm.Comol.Core.DomainModel.Helpers.Export
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports System.Linq
Imports System.Collections.Generic
Imports lm.Comol.Modules.CallForPapers.Presentation.Base.Controls
Imports lm.Comol.Modules.CallForPapers.Presentation.Base.Controls.IView

Public Class UC_PrintDraft
    Inherits BaseControl
    Implements IViewPrintDraft
    
    Public Event UpdateSettingsForEdit(ByRef settings As CallPrintSettings)

    Public Event PrePrintDraft()

#Region "Context"
    Private _Presenter As PrintDraftPresenter
    Private ReadOnly Property CurrentPresenter() As PrintDraftPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New PrintDraftPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EditCall", "Modules", "CallForPapers")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        'With Resource
        '    .setLinkButton(Me.LKBprint, True, False, False)
        'End With
        SetButtonText(ButtonTextType.draft)
    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get

        End Get
    End Property

    Public Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission

    End Sub

    Public Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout

    End Sub
#End Region

    'Public Property SaveLink As LinkButton
    '    Get
    '        Return Me.LKBprint
    '    End Get
    '    Set(value As LinkButton)
    '        Me.LKBprint = value
    '    End Set
    'End Property

    Public Property PrintButton As Button
        Get
            Return Me.BTNprint
        End Get
        Set(value As Button)
            Me.BTNprint = value
        End Set
    End Property


#Region "Implements"
    Public Property IdCall As Long Implements IViewPrintDraft.IdCall
        Get
            Return ViewStateOrDefault("IdCall", 0)
        End Get
        Set(value As Long)
            ViewState("IdCall") = value
        End Set
    End Property

    Private Property CallType As CallForPaperType Implements IViewPrintDraft.CallType
        Get
            Return ViewStateOrDefault("CallType", CallForPaperType.CallForBids)
        End Get
        Set(value As CallForPaperType)
            ViewState("CallType") = value
        End Set
    End Property

    Private Property CallName As String Implements IViewPrintDraft.CallName
        Get
            Return ViewStateOrDefault("CallName", "")
        End Get
        Set(value As String)
            ViewState("CallName") = value
        End Set
    End Property
    Public Property SubmissionType As SubmitterType Implements IViewPrintDraft.SubmissionType
        Get
            Return ViewStateOrDefault(Of SubmitterType)("IdSubmissionType", Nothing)
        End Get
        Set(value As SubmitterType)
            ViewState("IdSubmissionType") = value
        End Set
    End Property

    Public Property IdRevision As Long Implements IViewPrintDraft.IdRevision
        Get
            Return ViewStateOrDefault(Of Long)("IdRevision", Nothing)
        End Get
        Set(value As Long)
            ViewState("IdRevision") = value
        End Set
    End Property
    Public Property IdSubmission As Long Implements IViewPrintDraft.IdSubmission
        Get
            Return ViewStateOrDefault(Of Long)("IdSubmission", Nothing)
        End Get
        Set(value As Long)
            ViewState("IdSubmission") = value
        End Set
    End Property
    Public Sub UpdateSettings(ByRef settings As CallPrintSettings) Implements IViewPrintDraft.UpdateSettings
        RaiseEvent UpdateSettingsForEdit(settings)
    End Sub

    'Public Sub InitControl(ByVal callId As Int64, ByVal submissionTypeId As Int64, ByVal revisionId As Long, ByVal submissionId As Long)
    '    Me.CurrentPresenter.Initialize(callId, submissionTypeId, revisionId, submissionId)
    'End Sub

    Public Sub InitializeUC(idCall As Long, submissionTypeId As Long, idRevision As Long, idSubmission As Long)

        CurrentPresenter.Initialize(idCall, submissionTypeId, idRevision, idSubmission)

    End Sub



#End Region

    Private Sub LKBprint_Click(sender As Object, e As EventArgs) Handles BTNprint.Click 'LKBprint.Click
        If (CurrentPresenter.CanPrint) Then

            RaiseEvent PrePrintDraft()

            Response.Clear()

            'Dim fileType As ExportFileType = ExportFileType.pdf
            Dim fileName As String = ""

            Try
                fileName = String.Format(Resource.getValue("SubmissionTranslations." & IIf(CallType = CallForPaperType.CallForBids, SubmissionTranslations.SubmissionCallFileName.ToString, SubmissionTranslations.SubmissionRequestFileName.ToString)), "")
            Catch ex As Exception
                fileName = String.Format("{0}{1}.{2}", CallType.ToString(), "_Draft", ".pdf")
            End Try

            Try

                Dim translations As New Dictionary(Of SubmissionTranslations, String)

                For Each name As String In [Enum].GetNames(GetType(SubmissionTranslations))
                    translations.Add([Enum].Parse(GetType(SubmissionTranslations), name), Me.Resource.getValue("SubmissionTranslations." & name))
                Next

                Dim value As String = ""
                'RaiseEvent GetHiddenIdentifierValueEvent(value)

                Dim template As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template

                'template = GetTemplate(Presenter.GetSkinDetails(Me.CallId))

                If (IsNothing(template) _
                    OrElse IsNothing(template.Body) _
                    OrElse String.IsNullOrEmpty(template.Body.Text)) Then

                    template = GetTemplate(CurrentPresenter.GetSkinDetails(Me.IdCall))
                End If

                Dim dtImagePath As String = Me.SystemSettings.DocTemplateSettings.BasePath

                CurrentPresenter.ExportDraftToPdf(Me.SystemSettings.DocTemplateSettings.BasePath, fileName, translations, Response, New HttpCookie(CookieName, value), template)

            Catch de As Exception

            End Try
            Response.Flush()

        End If
    End Sub

    Private Function GetTemplate( _
                                context As lm.Comol.Core.DomainModel.Helpers.ExternalPageContext) _
                            As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template
        'Implements IViewSubmissionExport.GetTemplate

        Dim oService As New lm.Comol.Modules.Standard.Skin.Business.ServiceSkin(Me.PageUtility.CurrentContext)

        Dim configTemplate As New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template
        'RaiseEvent GetConfigTemplate(configTemplate)

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

    Public ReadOnly Property CookieName() As String
        Get
            Return "COMOL_submissionprintFileDownloadToken"
        End Get
    End Property


    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        'Me.LKBprint.Visible = CurrentPresenter.CanPrint
        'Me.LKBprint.Enabled = CurrentPresenter.CanPrint
        Me.BTNprint.Visible = CurrentPresenter.CanPrint
        Me.BTNprint.Enabled = CurrentPresenter.CanPrint
    End Sub

    'Public Sub SetButtonText(text As String, tooltip As String)
    '    If Not String.IsNullOrEmpty(text) Then
    '        Me.LKBprint.Text = text
    '    End If
    '    If Not String.IsNullOrEmpty(tooltip) Then
    '        Me.LKBprint.ToolTip = tooltip
    '    End If
    'End Sub

    Public Enum ButtonTextType
        draft
        empytdraft
        compiledDraft
    End Enum

    Public Sub SetButtonText(ByVal type As ButtonTextType)
        'Resource.setLinkButtonToValue(LKBprint, type.ToString(), True, True, False, False)



        Resource.setButton(BTNprint, True, False, False, True)

        BTNprint.Text = Resource.getValue(String.Format("BTNprint.text.{0}", type.ToString()))


    End Sub

    Public Property ButtonCssClass As String
        Get
            'Return LKBprint.CssClass
            Return BTNprint.CssClass
        End Get
        Set(value As String)
            'LKBprint.CssClass = value
            BTNprint.CssClass = value
        End Set
    End Property
End Class