Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Presentation.Call

Imports lm.Comol.Core.DomainModel.Helpers.Export

Public Class UC_PrintSettings
    Inherits BaseControl
    Implements IViewCallPrintSettings


    Public Event GetConfigTemplate(ByRef template As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template)


#Region "Context"
    Private _Presenter As CallPrintSettingsPresenter
    Private ReadOnly Property Presenter() As CallPrintSettingsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CallPrintSettingsPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("uc_PrintSettings", "Modules", "CallForPapers")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBtemplate_t)
            .setLabel(LBunselected_t)
            .setLabel(LBlayout_t)
            .setLabel(LBhighlightMandatory)
            .setLabel(LBLSectionTitle_t)
            .setLabel(LBLsectionDesc_t)
            .setLabel(LBLFieldTitle_t)
            .setLabel(LBLFieldDesc_t)
            .setLabel(LBLFieldEntry_t)

            .setCheckBox(CBXmandatory)


            .setDropDownList(DDLlayout, "0")
            .setDropDownList(DDLlayout, "1")
            .setDropDownList(DDLlayout, "19")
            .setDropDownList(DDLlayout, "28")
            .setDropDownList(DDLlayout, "37")
            .setDropDownList(DDLlayout, "46")
            .setDropDownList(DDLlayout, "55")
            .setDropDownList(DDLlayout, "64")
            .setDropDownList(DDLlayout, "73")
            .setDropDownList(DDLlayout, "82")
            .setDropDownList(DDLlayout, "91")

            .setRadioButtonList(RBLhiddenFields, "0")
            .setRadioButtonList(RBLhiddenFields, "1")


            .setCheckBox(CBXpermitDraft)
            .setLabel(LBprintDraft_t)
            .setLabel(LBwatermark_t)
        End With

        CTRL_SectionTitleFont.SetInternazionalizzazione("SectionTitle", Resource)
        CTRL_SectionDescFont.SetInternazionalizzazione("SectionDesc", Resource)
        CTRL_FieldTitle.SetInternazionalizzazione("FieldTitle", Resource)
        CTRL_FieldDesc.SetInternazionalizzazione("FieldDesc", Resource)
        CTRL_FieldEntry.SetInternazionalizzazione("FieldEntry", Resource)



    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get

        End Get
    End Property

#Region "Inizializzazione"


    Public Sub InitControl(ByVal callForPeaperId As Int64)
        CallId = callForPeaperId
        Presenter.InitView(callForPeaperId)
    End Sub



    Private Property CallId
        Get
            Try
                Return System.Convert.ToInt64(Me.HDFcallId.Value)
            Catch ex As Exception

            End Try
            Return -1
        End Get
        Set(value)
            Me.HDFcallId.Value = value
        End Set
    End Property



    Public Sub Initialize(ByVal settings As CallPrintSettings, ByVal moduleId As Long) Implements lm.Comol.Modules.CallForPapers.Presentation.IViewCallPrintSettings.Initialize

        With settings
            'Selettore Template
            Me.CTRLtemplate.isInAjaxPanel = False
            Me.CTRLtemplate.EnabledSelectedIndexChanged = False

            '            Me.CTRLtemplate.AllowSelect = Not isReadOnly
            Me.CTRLtemplate.InitializeControl(.TemplateId, .VersionId, moduleId) 'ServiceEP.ServiceModuleID(), oOwner)

            RBLhiddenFields.SelectedValue = .UnselectFields
            DDLlayout.SelectedValue = .Layout
            CBXmandatory.Checked = .ShowMandatory

            CTRL_SectionTitleFont.FontSettings = .SectionTitle
            CTRL_SectionDescFont.FontSettings = .SectionDescription

            CTRL_FieldTitle.FontSettings = .FieldTitle
            CTRL_FieldDesc.FontSettings = .FieldDescription
            CTRL_FieldEntry.FontSettings = .FieldContent

            CBXpermitDraft.Checked = .AllowPrintDraft
            TXBwatermarkText.Text = .DraftWaterMark

        End With

        CTRL_PrintDraft.InitializeUC(CallId, 0, 0, 0)

    End Sub


    Public Sub UpdateSettings(ByRef settings As CallPrintSettings) Implements IViewCallPrintSettings.UpdateSettings
        'Implements lm.Comol.Modules.CallForPapers.Presentation.IViewCallPrintSettings.GetSettings
        If IsNothing(settings) Then
            settings = New CallPrintSettings()
        End If

        Dim version As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_sTemplateVersion = CTRLtemplate.SelectedItem

        With settings
            .TemplateId = version.IdTemplate
            .VersionId = version.Id
            .CallId = Me.CallId
            .UnselectFields = RBLhiddenFields.SelectedValue
            .Layout = DDLlayout.SelectedValue
            .ShowMandatory = CBXmandatory.Checked
            .SectionTitle = CTRL_SectionTitleFont.FontSettings
            .SectionDescription = CTRL_SectionDescFont.FontSettings

            .FieldTitle = CTRL_FieldTitle.FontSettings
            .FieldDescription = CTRL_FieldDesc.FontSettings
            .FieldContent = CTRL_FieldEntry.FontSettings
            .DraftWaterMark = TXBwatermarkText.Text
            .AllowPrintDraft = CBXpermitDraft.Checked
        End With
    End Sub

    Public WriteOnly Property IsReadOnly As Boolean
        Set(value As Boolean)

            Me.CTRLtemplate.AllowSelect = Not value

            Me.RBLhiddenFields.Enabled = Not value
            Me.DDLlayout.Enabled = Not value

            Me.CTRL_SectionTitleFont.IsReadonly = value
            Me.CTRL_SectionDescFont.IsReadonly = value

            Me.CTRL_FieldTitle.IsReadonly = value
            Me.CTRL_FieldDesc.IsReadonly = value
            Me.CTRL_FieldEntry.IsReadonly = value

        End Set
    End Property
#End Region

    Public Sub SaveSettings()
        Me.Presenter.SaveSetting()
        CTRL_PrintDraft.InitializeUC(CallId, 0, 0, 0)
    End Sub


#Region "PrintDraft"

    'Private Sub LKBprintDraft_Click(sender As Object, e As EventArgs) Handles LKBprintDraft.Click
    '    Response.Clear()

    '    'Dim fileType As ExportFileType = ExportFileType.pdf
    '    Dim fileName As String = ""

    '    Try
    '        fileName = String.Format(Resource.getValue("SubmissionTranslations." & IIf(CallType = CallForPaperType.CallForBids, SubmissionTranslations.SubmissionCallFileName.ToString, SubmissionTranslations.SubmissionRequestFileName.ToString)), "")
    '    Catch ex As Exception
    '        fileName = String.Format("{0}{1}.{2}", CallType.ToString(), "_Draft", ".pdf")
    '    End Try

    '    Try

    '        'Dim baseFilePath As String = ""
    '        'If Me.SystemSettings.File.Materiale.DrivePath = "" Then
    '        '    baseFilePath = Server.MapPath(Me.PageUtility.BaseUrl & Me.SystemSettings.File.Materiale.VirtualPath)
    '        'Else
    '        '    baseFilePath = Me.SystemSettings.File.Materiale.DrivePath
    '        'End If

    '        Dim translations As New Dictionary(Of SubmissionTranslations, String)

    '        For Each name As String In [Enum].GetNames(GetType(SubmissionTranslations))
    '            translations.Add([Enum].Parse(GetType(SubmissionTranslations), name), Me.Resource.getValue("SubmissionTranslations." & name))
    '        Next

    '        Dim value As String = ""
    '        'RaiseEvent GetHiddenIdentifierValueEvent(value)

    '        Dim template As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template

    '        'If isSubmissionOwner Then
    '        '    template = GetTemplate()
    '        'Else
    '        template = GetTemplate(Presenter.GetSkinDetails(Me.CallId))
    '        'End If

    '        If (IsNothing(template) _
    '            OrElse IsNothing(template.Body) _
    '            OrElse String.IsNullOrEmpty(template.Body.Text)) Then

    '            template = GetTemplate(Presenter.GetSkinDetails(Me.CallId))
    '        End If

    '        Dim dtImagePath As String = Me.SystemSettings.DocTemplateSettings.BasePath

    '        Presenter.ExportDraftToPdf(Me.CallId, Me.SystemSettings.DocTemplateSettings.BasePath, fileName, translations, Response, New HttpCookie(CookieName, value), template, 0)
    '        'CurrentPresenter.ExportToPdf(WebOnlyRender, 0, 0, baseFilePath, dtImagePath, fileName, translations, Response, New HttpCookie(CookieName, value), template)
    '    Catch de As Exception

    '    End Try
    '    Response.End()
    'End Sub

    'Private Property CallType As CallForPaperType 'Implements IViewSubmissionExport.CallType
    '    Get
    '        Return ViewStateOrDefault("CallType", CallForPaperType.CallForBids)
    '    End Get
    '    Set(value As CallForPaperType)
    '        ViewState("CallType") = value
    '    End Set
    'End Property

    'Private Function GetTemplate( _
    '                            context As lm.Comol.Core.DomainModel.Helpers.ExternalPageContext) _
    '                        As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template
    '    'Implements IViewSubmissionExport.GetTemplate

    '    Dim oService As New lm.Comol.Modules.Standard.Skin.Business.ServiceSkin(Me.PageUtility.CurrentContext)

    '    Dim configTemplate As New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template
    '    RaiseEvent GetConfigTemplate(configTemplate)

    '    'Puoi evitare anche l'IF ed usare direttamente questa:
    '    'Me.BaseUrl 
    '    Return oService.GetTemplateCommunitySkin(PageUtility.ApplicationUrlBase, SystemSettings.SkinSettings.SkinVirtualPath, Me.SystemSettings.DefaultLanguage.Codice, Me.PageUtility.CurrentUser.Lingua.Codice, "", context.Skin.IdCommunity, context.Skin.IdOrganization, context.Skin.IdSkin, configTemplate, SystemSettings.DocTemplateSettings.FooterFontSize)

    '    'If context.Skin.IdSkin = 0 Then
    '    '    Return oService.GetCommunityTemplate(context.Skin.IdCommunity, context.Skin.IdOrganization, defaultLanguageCode, userLanguageCode, "", PageUtility.ApplicationUrlBase, PageUtility.SystemSettings.SkinSettings.SkinVirtualPath)
    '    'Else
    '    '    Return oService.GetSkinTemplate(context.Skin.IdSkin, defaultLanguageCode, userLanguageCode, "", PageUtility.ApplicationUrlBase, SystemSettings.SkinSettings.SkinVirtualPath)
    '    'End If

    '    'Return oService.ge
    'End Function

    ''Private Property SkinDetails As lm.Comol.Core.DomainModel.Helpers.ExternalPageContext 'Implements IViewSubmissionExport.SkinDetails
    ''    Get
    ''        Return ViewStateOrDefault("SkinDetails", New lm.Comol.Core.DomainModel.Helpers.ExternalPageContext())
    ''    End Get
    ''    Set(value As lm.Comol.Core.DomainModel.Helpers.ExternalPageContext)
    ''        ViewState("SkinDetails") = value
    ''    End Set
    ''End Property


    'Public ReadOnly Property CookieName() As String
    '    Get
    '        Return "COMOL_submissionprintFileDownloadToken"
    '    End Get
    'End Property
#End Region

    Private Sub CTRL_PrintDraft_UpdateSettingsForEdit(ByRef settings As CallPrintSettings) Handles CTRL_PrintDraft.UpdateSettingsForEdit
        Me.UpdateSettings(settings)
    End Sub

End Class