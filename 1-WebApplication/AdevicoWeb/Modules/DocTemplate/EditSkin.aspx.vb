Imports lm.Comol.Core.BaseModules.DocTemplate                       'Presentation
Imports TemplateVers = lm.Comol.Core.DomainModel.DocTemplateVers    'Domain

Public Class EditSkin1
    Inherits PageBase
    Implements Presentation.IVIewDocTemplateEditSkin

#Region "Context"
    Private _Presenter As Presentation.DocTemplateEditSkinPresenter
    Private ReadOnly Property CurrentPresenter() As Presentation.DocTemplateEditSkinPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New Presentation.DocTemplateEditSkinPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
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
        End Get
    End Property
#End Region

#Region "Implements"
    Public ReadOnly Property IsAdvancedEdit As Boolean Implements Presentation.IVIewDocTemplateEditSkin.IsAdvancedEdit
        Get
            Return True
        End Get
    End Property
    Public ReadOnly Property PreloadBackUrl As String Implements Presentation.IVIewDocTemplateEditSkin.PreloadBackUrl
        Get
            Return Request.QueryString("BackUrl")
        End Get
    End Property
    Public ReadOnly Property PreloadedView As lm.Comol.Core.BaseModules.DocTemplate.Presentation.ViewEditTemplateSkinElement Implements Presentation.IVIewDocTemplateEditSkin.PreloadedView
        Get
        End Get
    End Property
    Public ReadOnly Property SaveOnChangeView As Boolean Implements Presentation.IVIewDocTemplateEditSkin.SaveOnChangeView
        Get
            Return False
        End Get
    End Property
    Public ReadOnly Property TempalteBasePath As String Implements Presentation.IVIewDocTemplateEditSkin.TempalteBasePath
        Get
            Return (MyBase.SystemSettings.DocTemplateSettings.BasePath).Replace("/", "\").Replace("\\", "\") '& TempalteTempPath
        End Get
    End Property
    Public ReadOnly Property TempalteTempPath As String Implements Presentation.IVIewDocTemplateEditSkin.TempalteTempPath
        Get
            If Not MyBase.SystemSettings.DocTemplateSettings.BasePath.EndsWith("\") Then
                MyBase.SystemSettings.DocTemplateSettings.BasePath &= "\"
            End If

            Return (MyBase.SystemSettings.DocTemplateSettings.BasePath & TempalteTempPath & MyBase.SystemSettings.DocTemplateSettings.BaseTemporaryFolder).Replace("/", "\").Replace("\\", "\")
        End Get
    End Property
    Public ReadOnly Property TemplateBaseTempUrl As String Implements Presentation.IVIewDocTemplateEditSkin.TemplateBaseTempUrl
        Get
            Dim TempUrl As String = Me.FullBaseUrl

            If Not TempUrl.EndsWith("/") Then
                TempUrl &= "/"
            End If

            TempUrl &= MyBase.SystemSettings.DocTemplateSettings.BaseUrl.Replace("//", "/")

            If Not TempUrl.EndsWith("/") Then
                TempUrl &= "/"
            End If

            TempUrl = (TempUrl & MyBase.SystemSettings.DocTemplateSettings.BaseTemporaryFolder.Replace("\\", "\")).Replace("\", "/")

            Return TempUrl

        End Get
    End Property
    Public ReadOnly Property TemplateBaseUrl As String Implements Presentation.IVIewDocTemplateEditSkin.TemplateBaseUrl
        Get
            Return Me.FullBaseUrl & MyBase.SystemSettings.DocTemplateSettings.BaseUrl
        End Get
    End Property
    Public Property TemplateId As Long Implements Presentation.IVIewDocTemplateEditSkin.TemplateId
        Get
            Dim TmplId As Int64 = 0

            Try
                TmplId = System.Convert.ToInt64(ViewState("idTemplate"))
            Catch ex As Exception
            End Try

            If (TmplId <= 0 AndAlso Not String.IsNullOrEmpty(Request.QueryString("idTemplate"))) Then
                Try
                    TmplId = System.Convert.ToInt64(Request.QueryString("idTemplate"))
                Catch ex As Exception
                    TmplId = 0
                End Try
            End If

            Return TmplId
        End Get
        Set(value As Long)
            ViewState("idTemplate") = value
        End Set
    End Property
    Public Property VersionId As Long Implements Presentation.IVIewDocTemplateEditSkin.VersionId
        Get
            Dim VersId As Int64 = 0
            Try
                VersId = System.Convert.ToInt64(ViewState("idVersion"))
            Catch ex As Exception
            End Try
            If (VersId <= 0 AndAlso Not String.IsNullOrEmpty(Request.QueryString("idVersion"))) Then
                Try
                    VersId = System.Convert.ToInt64(Request.QueryString("idVersion"))
                Catch ex As Exception
                    VersId = 0
                End Try
            End If

            Return VersId
        End Get
        Set(value As Long)
            ViewState("idVersion") = value
        End Set
    End Property
    Public Property CurrentView As Presentation.ViewEditTemplateSkinElement Implements Presentation.IVIewDocTemplateEditSkin.CurrentView
        Get
            Return CType(Me.CTRLsteps.CurrentStepId, lm.Comol.Core.BaseModules.DocTemplate.Presentation.ViewEditTemplateSkinElement)
        End Get
        Set(value As lm.Comol.Core.BaseModules.DocTemplate.Presentation.ViewEditTemplateSkinElement)

            Me.CTRLsteps.BindData(value, CType([Enum].GetValues(GetType(Presentation.ViewEditTemplateSkinElement)), Integer()))

            Select Case value
                Case Presentation.ViewEditTemplateElement.Body
                    Me.MLVtemplatePart.SetActiveView(Me.VWbody)
                Case Presentation.ViewEditTemplateElement.Signature
                    Me.MLVtemplatePart.SetActiveView(Me.VWsignatures)
                Case Presentation.ViewEditTemplateSkinElement.Property
                    Me.MLVtemplatePart.SetActiveView(Me.VWproperty)
                Case Else
                    Me.MLVtemplatePart.SetActiveView(Me.VWskin)
            End Select
        End Set
    End Property
    Public ReadOnly Property GetCurrentHeader As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter Implements Presentation.IVIewDocTemplateEditSkin.GetCurrentHeader
        Get
            Return Me.UCskin.getTemplateHeader()
        End Get
    End Property
    Public ReadOnly Property GetCurrentFooter As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter Implements Presentation.IVIewDocTemplateEditSkin.GetCurrentFooter
        Get
            Return Me.UCskin.getTemplateFooter()
        End Get
    End Property
    Public Sub RecoverSignature(Element As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.DTO_EditItem(Of lm.Comol.Core.DomainModel.DocTemplateVers.Signature)) Implements Presentation.IVIewDocTemplateEditSkin.RecoverSignature
        Me.UCsignatures.AddRecSignature(Element)
    End Sub
#End Region

#Region "Internal"
    Public ReadOnly Property CookieName() As String
        Get
            Return "COMOL_EditTemplateSkin"
        End Get
    End Property
    Public ReadOnly Property DisplayMessageToken() As String
        Get
            ' TRADUZIONE DEL MESSAGGIO DA VISUALIZZARE
            Dim Msg As String = "Wait..."
            Try
                Msg = Resource.getValue("DisplayToken.Message")
            Catch ex As Exception
            End Try

            Return Msg
        End Get
    End Property
    Public ReadOnly Property DisplayTitleToken() As String
        Get
            ' TRADUZIONE DEL TITOLO DEL MESSAGGIO DA VISUALIZZARE
            Dim Msg As String = ""
            Try
                Msg = Resource.getValue("DisplayToken.Title")
            Catch ex As Exception
            End Try

            Return Msg

        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Master.ShowDocType = True

        If (Me.IsAdvancedEdit) Then
            Me.HYPGoToAdvance.Visible = False
            Me.HYPGoToSimple.Visible = True
        Else
            Me.HYPGoToAdvance.Visible = True
            Me.HYPGoToSimple.Visible = False
        End If
    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.CurrentPresenter.InitView()
        Me.CurrentPresenter.ChangePage(Presentation.ViewEditTemplateSkinElement.Property)
    End Sub
    Public Overrides Sub BindNoPermessi()
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()
    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_templateEdit", "Modules", "DocTemplates")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLinkButton(Me.LKBexportRTF, True, True, False, False)
            .setLinkButton(Me.LKBexportPDF, True, True, False, False)

            .setHyperLink(Me.HYPbackUrl, True, True)
            .setHyperLink(Me.HYPlist, True, True)

            .setLinkButton(Me.LKBundo, True, True, False, False)
            .setLinkButton(Me.LKBsave, True, True, False, False)

            .setHyperLink(Me.HYPGoToAdvance, True, True, False, True)
            .setHyperLink(Me.HYPGoToSimple, True, True, False, True)

            .setLiteral(LTtitle_t)
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)
    End Sub
#End Region

#Region "Implements"
    Public Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements Presentation.IVIewDocTemplateEditSkin.DisplayNoPermission
    End Sub
    Public Sub DisplaySessionTimeout() Implements Presentation.IVIewDocTemplateEditSkin.DisplaySessionTimeout
        Me.ShowMessageToPage("SessionTimeOut")
    End Sub
    Public Function GetCurrentModules() As System.Collections.Generic.IList(Of lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.DTO_EditItem(Of lm.Comol.Core.DomainModel.DocTemplateVers.ServiceContent)) Implements Presentation.IVIewDocTemplateEditSkin.GetCurrentModules
    End Function
    Public Function GetCurrentVersion() As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.DTO_EditTemplateVersion Implements Presentation.IVIewDocTemplateEditSkin.GetCurrentVersion
        Dim Version As New TemplateVers.Domain.DTO.Management.DTO_EditTemplateVersion
        Version.Id = Me.VersionId
        Version.IdTemplate = Me.TemplateId

        Version.Error = lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.VersionEditError.none

        'Settings
        Version.Setting = New TemplateVers.Domain.DTO.Management.DTO_EditItem(Of TemplateVers.Settings)()
        Version.Setting.Data = UCsettings.CurrentSettings

        Version.HeaderLeft = Nothing
        Version.HeaderCenter = Nothing
        Version.HeaderRight = Nothing

        Version.FooterLeft = Nothing
        Version.FooterCenter = Nothing
        Version.FooterRight = Nothing

        'Body
        Version.Body = New TemplateVers.Domain.DTO.Management.DTO_EditItem(Of TemplateVers.ElementText)()
        Version.Body.Data = New TemplateVers.ElementText
        Version.Body.Data.IsHTML = Me.UCbody.IsHtml
        Version.Body.Data.Text = Me.UCbody.CurrentText
        Version.Body.Data.Position = lm.Comol.Core.DomainModel.DocTemplateVers.ElementPosition.Body

        'Signatures
        Version.Signatures = Me.UCsignatures.Signatures
        If IsNothing(Version.Signatures) Then
            Version.Signatures = New List(Of TemplateVers.Domain.DTO.Management.DTO_EditItem(Of TemplateVers.Signature))()
        End If

        Return Version
    End Function
    Public Sub LoadAvailableServices(usedItems As System.Collections.Generic.IList(Of lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.DTO_EditItem(Of lm.Comol.Core.DomainModel.DocTemplateVers.ServiceContent))) Implements Presentation.IVIewDocTemplateEditSkin.LoadAvailableServices
        Me.UCbody.LoadAvailableService(usedItems)
    End Sub
    Public Sub SendUserAction(idCommunity As Integer, idModule As Integer, idTemplate As Long, action As lm.Comol.Core.DomainModel.DocTemplateVers.ModuleDocTemplate.ActionType) Implements Presentation.IVIewDocTemplateEditSkin.SendUserAction
    End Sub
    Public Sub SetBackToListUrl(url As String) Implements Presentation.IVIewDocTemplateEditSkin.SetBackToListUrl
        Me.HYPlist.Visible = Not String.IsNullOrEmpty(url)
        Me.HYPlist.NavigateUrl = BaseUrl & url
    End Sub
    Public Sub SetBackUrl(url As String) Implements Presentation.IVIewDocTemplateEditSkin.SetBackUrl
        Me.HYPbackUrl.Visible = Not String.IsNullOrEmpty(url)
        Me.HYPbackUrl.NavigateUrl = BaseUrl & url
    End Sub
    Public Sub SetCurrentVersion(Version As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.DTO_EditTemplateVersion) Implements Presentation.IVIewDocTemplateEditSkin.SetCurrentVersion

        If Not IsNothing(Version) Then

            'Reset Id
            Me.TemplateId = Version.IdTemplate
            Me.VersionId = Version.Id

            LKBexportPDF.Visible = True
            LKBexportRTF.Visible = True

            Dim ImgBasePath As String = TemplateVers.Business.ImageHelper.GetImageUrl("", Me.TemplateBaseUrl, Version.IdTemplate, Version.Id)

            'Settings
            If Not IsNothing(Version.Setting) AndAlso Not IsNothing(Version.Setting.Data) Then
                Me.UCsettings.SetTemplteVersion(Version.IdTemplate, Version.Id)
                'Me.UCsettings.TemplateImageBasePath = ImgBasePath
                Me.UCsettings.CurrentSettings = Version.Setting.Data
                Me.UCsettings.TemplateName = Version.TemplateName
                Me.UCsettings.BindPrevVersion(Version.Setting.PreviousVersion)
            End If

            'Body
            If Not IsNothing(Version.Body) AndAlso Not IsNothing(Version.Body.Data) Then
                Me.UCbody.CurrentText = Version.Body.Data.Text
                Me.UCbody.IsHtml = Version.Body.Data.IsHTML
                Me.UCbody.BindPrevVersion(Version.Body.PreviousVersion)
                Me.UCbody.Code = Version.Body.Data.Id
            Else
                Me.UCbody.CurrentText = "-1"
                Me.UCbody.IsHtml = True
                Me.UCbody.BindPrevVersion(Nothing)
            End If

            'Signatures
            'Me.UCsignatures.TemplateImageBasePath = ImgBasePath
            Me.UCsignatures.SetTemplteVersion(Version.IdTemplate, Version.Id)

            Me.UCsignatures.Signatures = Version.Signatures
            Me.UCsignatures.BindPrevVersion(Version.SignaturesPrevious)

            InitLink()
        End If
    End Sub
    Public Sub SetPreviewUrl(url As String) Implements Presentation.IVIewDocTemplateEditSkin.SetPreviewUrl
    End Sub
    Public Sub ShowError(EditError As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.VersionEditError) Implements lm.Comol.Core.BaseModules.DocTemplate.Presentation.IVIewDocTemplateEditSkin.ShowError
        Me.ShowMessageToPage(Me.Resource.getValue("ErrorMsg_" & EditError.ToString()))
    End Sub
    Public Sub RecoverSettings(Settings As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.DTO_EditItem(Of lm.Comol.Core.DomainModel.DocTemplateVers.Settings)) Implements Presentation.IVIewDocTemplateEditSkin.RecoverSettings
        Me.UCsettings.CurrentSettings = Settings.Data
        Me.UCsettings.BindPrevVersion(Settings.PreviousVersion)
    End Sub
    Public Sub RecoverPageElement(Element As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.DTO_EditItem(Of lm.Comol.Core.DomainModel.DocTemplateVers.PageElement)) Implements Presentation.IVIewDocTemplateEditSkin.RecoverPageElement
        If Not IsNothing(Element) _
            AndAlso Not IsNothing(Element.Data) _
            AndAlso Element.Data.Position = lm.Comol.Core.DomainModel.DocTemplateVers.ElementPosition.Body _
            AndAlso TypeOf Element.Data Is lm.Comol.Core.DomainModel.DocTemplateVers.ElementText Then

            Dim TxtEl As lm.Comol.Core.DomainModel.DocTemplateVers.ElementText = DirectCast(Element.Data, lm.Comol.Core.DomainModel.DocTemplateVers.ElementText)

            If Not IsNothing(TxtEl) Then
                Me.UCbody.CurrentText = TxtEl.Text
                Me.UCbody.IsHtml = TxtEl.IsHTML
                Me.UCbody.BindPrevVersion(Element.PreviousVersion)
            End If

        End If
    End Sub
#End Region

#Region "Internal"
    Public Sub InitLink()
        Dim Url As String = "./Edit.aspx"
        Url &= "?idTemplate=" & Me.TemplateId.ToString()
        If (Me.VersionId > 0) Then
            Url &= "&idVersion=" & Me.VersionId.ToString()
        End If
        Me.HYPGoToSimple.NavigateUrl = Url
        Me.HYPGoToAdvance.NavigateUrl = Url & "&IsAdv=1"
    End Sub
#End Region

#Region "Handler"
    Private Sub LKBexportPDF_Click(sender As Object, e As System.EventArgs) Handles LKBexportPDF.Click
        Dim fileName As String '= "test"

        Try
            fileName = String.Format(Resource.getValue("TemplateFile.Name"))
        Catch ex As Exception

        End Try

        If String.IsNullOrEmpty(fileName) Then
            fileName = "Preview"
        End If

        Try
            CurrentPresenter.ExportToPdf(fileName, Response, New HttpCookie(CookieName, HDNdownloadTokenValue.Value))

        Catch ex As Exception
        End Try
    End Sub
    Private Sub LKBexportRTF_Click(sender As Object, e As System.EventArgs) Handles LKBexportRTF.Click
        Response.Clear()

        Dim fileName As String '= "test"

        Try
            fileName = String.Format(Resource.getValue("TemplateFile.Name"))
        Catch ex As Exception

        End Try

        If String.IsNullOrEmpty(fileName) Then
            fileName = "Preview"
        End If

        Try
            CurrentPresenter.ExportToRtf(fileName, Response, New HttpCookie(CookieName, HDNdownloadTokenValue.Value))
        Catch ex As Exception
        End Try
        Response.End()
    End Sub
    Private Sub CTRLsteps_SetCurrentView(View As lm.Comol.Core.BaseModules.DocTemplate.Presentation.ViewEditTemplateElement) Handles CTRLsteps.SetCurrentView
        Me.CurrentPresenter.ChangePage(View)
    End Sub
    Private Sub LKBsave_Click(sender As Object, e As System.EventArgs) Handles LKBsave.Click
        Me.CurrentPresenter.UpdateCurrentData()
    End Sub
    Private Sub LKBundo_Click(sender As Object, e As System.EventArgs) Handles LKBundo.Click
        Me.CurrentPresenter.InitView()
    End Sub
    Private Sub UCbody_DeletePrevBody(Id As Long) Handles UCbody.DeletePrevBody
        Me.CurrentPresenter.PageElementDeletePrev(Me.TemplateId, Me.VersionId, Id)
    End Sub
    Private Sub UCsettings_DeletePrevSetting(Id As Long) Handles UCsettings.DeletePrevSetting
        Me.CurrentPresenter.SettingsDeletePrev(Id)
    End Sub
    Private Sub UCsignatures_DeletePrevSignature(Id As Long) Handles UCsignatures.DeletePrevSignature
        Me.CurrentPresenter.SignatureDeletePrev(Me.TemplateId, Me.VersionId, Id)
    End Sub
    Private Sub UCbody_DeletePrevBodies(Ids As System.Collections.Generic.IList(Of Long)) Handles UCbody.DeletePrevBodies
        Me.CurrentPresenter.PageElementDeletePrevs(Me.TemplateId, Me.VersionId, Ids)
    End Sub
    Private Sub UCsettings_DeletePrevSettings(Ids As System.Collections.Generic.IList(Of Long)) Handles UCsettings.DeletePrevSettings
        Me.CurrentPresenter.SettingsDeletePrevs(Ids)
    End Sub
    Private Sub UCsignatures_DeletePrevSignatures(Ids As System.Collections.Generic.IList(Of Long)) Handles UCsignatures.DeletePrevSignatures
        Me.CurrentPresenter.SignatureDeletePrevs(Me.TemplateId, Me.VersionId, Ids)
    End Sub
    Private Sub UCsettings_RecoverPrevSetting(Id As Long) Handles UCsettings.RecoverPrevSetting
        Me.CurrentPresenter.SettingsRecoverPrev(Id)
    End Sub
    Private Sub UCbody_RecoverPrevbody(Id As Long) Handles UCbody.RecoverPrevbody
        Me.CurrentPresenter.PageElementRecoverPrev(Id)
    End Sub
    Private Sub UCsignatures_RecoverPrevSignature(Id As Long) Handles UCsignatures.RecoverPrevSignature
        Me.CurrentPresenter.SignatureRecoverPrev(Id)
    End Sub
#End Region

End Class