Imports lm.Comol.Core.BaseModules.DocTemplate                       'Presentation
Imports TemplateVers = lm.Comol.Core.DomainModel.DocTemplateVers    'Domain
Imports lm.Comol.Core.DomainModel.Helpers.Export

Public Class Edit
    Inherits PageBase
    Implements Presentation.IViewDocTemplateEdit

    'NOTE SVILUPPO
    ' Da rivedere:
    '   - Action
    '   - Permission a livello di master (gestiti internamente)

#Region "Context"
    Private _Presenter As Presentation.DocTemplateEditPresenter
    Private ReadOnly Property CurrentPresenter() As Presentation.DocTemplateEditPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New Presentation.DocTemplateEditPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"

    Public Property TemplateId As Long Implements Presentation.IViewDocTemplateEdit.TemplateId
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

    Public Property VersionId As Long Implements Presentation.IViewDocTemplateEdit.VersionId
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

    Public ReadOnly Property PreloadedView As Presentation.ViewEditTemplateElement Implements Presentation.IViewDocTemplateEdit.PreloadedView
        Get

        End Get
    End Property

    Public ReadOnly Property SaveOnChangeView As Boolean Implements Presentation.IViewDocTemplateEdit.SaveOnChangeView
        Get
            Return False
        End Get
    End Property

    Public Property CurrentView As Presentation.ViewEditTemplateElement Implements Presentation.IViewDocTemplateEdit.CurrentView
        Get
            Return CType(Me.CTRLsteps.CurrentStepId, Presentation.ViewEditTemplateElement)
        End Get
        Set(value As Presentation.ViewEditTemplateElement)
            'If (value = Presentation.ViewEditTemplateElement.none) Then
            '    value = Presentation.ViewEditTemplateElement.Property
            'End If

            Me.CTRLsteps.BindData(value, CType([Enum].GetValues(GetType(Presentation.ViewEditTemplateElement)), Integer()))

            Select Case value
                Case Presentation.ViewEditTemplateElement.Header
                    Me.MLVtemplatePart.SetActiveView(Me.VWheader)
                Case Presentation.ViewEditTemplateElement.Body
                    Me.MLVtemplatePart.SetActiveView(Me.VWBody)
                Case Presentation.ViewEditTemplateElement.Footer
                    Me.MLVtemplatePart.SetActiveView(Me.VWfooter)
                Case Presentation.ViewEditTemplateElement.Signature
                    Me.MLVtemplatePart.SetActiveView(Me.VWsignatures)
                Case Else
                    Me.MLVtemplatePart.SetActiveView(Me.VWproperty)
            End Select
        End Set
    End Property

    Public ReadOnly Property IsAdvancedEdit As Boolean Implements Presentation.IViewDocTemplateEdit.IsAdvancedEdit
        Get
            If (Not String.IsNullOrEmpty(Request.QueryString("IsAdv"))) Then
                'Se ho una richiesta ESPLICITA via Querystring, imposto quella
                If (Request.QueryString("IsAdv") = "1") Then
                    CookieAdvEdit = True
                ElseIf (Request.QueryString("IsAdv") = "0") Then
                    CookieAdvEdit = False
                End If

            End If

            Return CookieAdvEdit
        End Get
    End Property
    Public ReadOnly Property TemplateBaseUrl As String Implements Presentation.IViewDocTemplateEdit.TemplateBaseUrl
        Get
            Return Me.FullBaseUrl & MyBase.SystemSettings.DocTemplateSettings.BaseUrl
        End Get
    End Property
    Public ReadOnly Property PreloadBackUrl As String Implements Presentation.IViewDocTemplateEdit.PreloadBackUrl
        Get
            Return Request.QueryString("BackUrl")
        End Get
    End Property

    Public ReadOnly Property TemplateBasePath As String Implements Presentation.IViewDocTemplateEdit.TemplateBasePath
        Get
            Return (MyBase.SystemSettings.DocTemplateSettings.BasePath).Replace("/", "\").Replace("\\", "\")
        End Get
    End Property
    Public ReadOnly Property TemplateTempPath As String Implements Presentation.IViewDocTemplateEdit.TemplateTempPath
        Get
            If Not MyBase.SystemSettings.DocTemplateSettings.BasePath.EndsWith("\") Then
                MyBase.SystemSettings.DocTemplateSettings.BasePath &= "\"
            End If

            Return (MyBase.SystemSettings.DocTemplateSettings.BasePath & TemplateTempPath & MyBase.SystemSettings.DocTemplateSettings.BaseTemporaryFolder).Replace("/", "\").Replace("\\", "\")
        End Get
    End Property
    Public ReadOnly Property TemplateBaseTempUrl As String Implements Presentation.IViewDocTemplateEdit.TemplateBaseTempUrl
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

#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides Sub BindDati()
        Me.CurrentPresenter.InitView()
        Me.CurrentPresenter.ChangePage(Presentation.ViewEditTemplateElement.Property)
    End Sub
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get

        End Get
    End Property
#End Region

#Region "Internal"
    Public ReadOnly Property FullBaseUrl() As String
        Get
            Return Me.BaseUrl
        End Get
    End Property
    Private Property CookieAdvEdit As Boolean
        Get
            Dim AdvEdit As Boolean = False

            If (Not IsNothing(Request.Cookies("DocTemplate")) AndAlso Not IsNothing(Request.Cookies("DocTemplate")("AdvanceEdit"))) Then
                Try
                    AdvEdit = Request.Cookies("DocTemplate")("AdvanceEdit")
                Catch ex As Exception

                End Try

            End If
            Return AdvEdit
        End Get
        Set(value As Boolean)
            Response.Cookies("DocTemplate")("AdvanceEdit") = value
            Response.Cookies("DocTemplate").Expires = DateTime.Now.AddDays(15)
        End Set
    End Property
    Public ReadOnly Property BaseUrl() As String
        Get
            Return Me.PageUtility.ApplicationUrlBase
        End Get
    End Property
    Private ReadOnly Property BaseSkinPath As String
        Get
            Return MyBase.SystemSettings.SkinSettings.SkinVirtualPath
        End Get
    End Property
    Private ReadOnly Property FullSkinPath As String
        Get
            Return MyBase.SystemSettings.SkinSettings.SkinPhisicalPath
        End Get
    End Property
    Public ReadOnly Property CookieName() As String
        Get
            Return "COMOL_DocTemplate"
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
            ' TRADUZIONE DEL MESSAGGIO DA VISUALIZZARE
            Dim Msg As String = ""
            Try
                Msg = Resource.getValue("DisplayToken.Title")
            Catch ex As Exception
            End Try

            Return Msg

        End Get
    End Property
    Public ReadOnly Property EditorClientId As String
        Get
            Return Me.UCbody.EditorClientId
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
    Public Overrides Sub BindNoPermessi()
    End Sub
    Public Overrides Sub RegistraAccessoPagina()
    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_templateEdit", "Modules", "DocTemplates")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setButton(BTNaddUp)

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
        LBLerror.Text = errorMessage
        LBLerror.Visible = True
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

#End Region

#Region "Implements"
    Public Sub SetCurrentVersion(Version As TemplateVers.Domain.DTO.Management.DTO_EditTemplateVersion) Implements Presentation.IViewDocTemplateEdit.SetCurrentVersion

        If Not IsNothing(Version) Then

            'Reset Id
            Me.TemplateId = Version.IdTemplate
            Me.VersionId = Version.Id

            PLHexport.Visible = True

            'LKBexportPDF.Visible = True
            'LKBexportRTF.Visible = True

            Dim ImgBasePath As String = TemplateVers.Business.ImageHelper.GetImageUrl("", Me.TemplateBaseUrl, Version.IdTemplate, Version.Id)

            'Settings
            If Not IsNothing(Version.Setting) AndAlso Not IsNothing(Version.Setting.Data) Then
                Me.UCsettings.SetTemplteVersion(Version.IdTemplate, Version.Id)
                Me.UCsettings.CurrentSettings = Version.Setting.Data
                Me.UCsettings.TemplateName = Version.TemplateName
                Me.UCsettings.BindPrevVersion(Version.Setting.PreviousVersion)
            End If

            'Header
            Me.UCheader.TemplateImageBasePath = ImgBasePath
            Me.UCheader.SetElement(Version.HeaderLeft.Data, Version.HeaderCenter.Data, Version.HeaderRight.Data, Version.IdTemplate, Version.Id)
            Me.UCheader.SetRevision(Version.HeaderLeft.PreviousVersion, Version.HeaderCenter.PreviousVersion, Version.HeaderRight.PreviousVersion)

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

            'Footer
            Me.UCfooter.TemplateImageBasePath = ImgBasePath
            Me.UCfooter.SetElement(Version.FooterLeft.Data, Version.FooterCenter.Data, Version.FooterRight.Data, Version.IdTemplate, Version.Id)
            Me.UCfooter.SetRevision(Version.FooterLeft.PreviousVersion, Version.FooterCenter.PreviousVersion, Version.FooterRight.PreviousVersion)

            'Signatures
            Me.UCsignatures.SetTemplteVersion(Version.IdTemplate, Version.Id)
            Me.UCsignatures.Signatures = Version.Signatures
            Me.UCsignatures.BindPrevVersion(Version.SignaturesPrevious)

            'Version
            InitLink()
        End If
    End Sub
    Public Sub ShowError(EditError As TemplateVers.Domain.DTO.Management.VersionEditError) Implements Presentation.IViewDocTemplateEdit.ShowError
        Me.ShowMessageToPage(Me.Resource.getValue("ErrorMsg_" & EditError.ToString()))
    End Sub

    Public Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements lm.Comol.Core.BaseModules.DocTemplate.Presentation.IViewDocTemplateEdit.DisplayNoPermission
        ShowError(lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.VersionEditError.NoPremission)
        'Me.ShowMessageToPage(Resource.getValue("ErrorMsg_NoPremission"))
        PnlData.Visible = False
        PLHsecButton.Visible = False
    End Sub
    Public Sub DisplaySessionTimeout() Implements lm.Comol.Core.BaseModules.DocTemplate.Presentation.IViewDocTemplateEdit.DisplaySessionTimeout
        Me.ShowMessageToPage(Resource.getValue("SessionTimeOut"))
        PnlData.Visible = False
        PLHsecButton.Visible = False
    End Sub
    Public Sub DisplayNoVersion() Implements lm.Comol.Core.BaseModules.DocTemplate.Presentation.IViewDocTemplateEdit.DisplayNoVersion
        ShowError(lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.VersionEditError.NotFound)
        PnlData.Visible = False
        PLHsecButton.Visible = False
    End Sub



    Public Sub SendUserAction(idCommunity As Integer, idModule As Integer, idTemplate As Long, action As lm.Comol.Core.DomainModel.DocTemplateVers.ModuleDocTemplate.ActionType) Implements Presentation.IViewDocTemplateEdit.SendUserAction
    End Sub
    Private Sub SetBackUrl(backUrl As String) Implements Presentation.IViewDocTemplateEdit.SetBackUrl
        Me.HYPbackUrl.Visible = Not String.IsNullOrEmpty(backUrl)
        Me.HYPbackUrl.NavigateUrl = BaseUrl & backUrl
    End Sub
    Private Sub SetBackToListUrl(url As String) Implements Presentation.IViewDocTemplateEdit.SetBackToListUrl
        Me.HYPlist.Visible = Not String.IsNullOrEmpty(url)
        Me.HYPlist.NavigateUrl = BaseUrl & url
    End Sub
    Private Sub SetPreviewUrl(url As String) Implements Presentation.IViewDocTemplateEdit.SetPreviewUrl
    End Sub

    Public Sub LoadAvailableServices(usedItems As System.Collections.Generic.IList(Of lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.DTO_EditItem(Of lm.Comol.Core.DomainModel.DocTemplateVers.ServiceContent))) Implements Presentation.IViewDocTemplateEdit.LoadAvailableServices
        Me.UCbody.LoadAvailableService(usedItems)
    End Sub
    Public Function GetCurrentModules() As System.Collections.Generic.IList(Of lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.DTO_EditItem(Of lm.Comol.Core.DomainModel.DocTemplateVers.ServiceContent)) Implements Presentation.IViewDocTemplateEdit.GetCurrentModules
        Return UCbody.GetCurrentModules()
    End Function
    Public Function GetCurrentVersion() As TemplateVers.Domain.DTO.Management.DTO_EditTemplateVersion Implements Presentation.IViewDocTemplateEdit.GetCurrentVersion
        Dim Version As New TemplateVers.Domain.DTO.Management.DTO_EditTemplateVersion
        Version.Id = Me.VersionId
        Version.IdTemplate = Me.TemplateId

        Version.Error = lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.VersionEditError.none

        'Settings
        Version.Setting = New TemplateVers.Domain.DTO.Management.DTO_EditItem(Of TemplateVers.Settings)()
        Version.Setting.Data = UCsettings.CurrentSettings

        'Header
        Version.HeaderLeft = New TemplateVers.Domain.DTO.Management.DTO_EditItem(Of TemplateVers.PageElement)()
        Version.HeaderLeft.Data = Me.UCheader.LeftElement
        Version.HeaderLeft.Data.Position = lm.Comol.Core.DomainModel.DocTemplateVers.ElementPosition.HeaderLeft

        Version.HeaderCenter = New TemplateVers.Domain.DTO.Management.DTO_EditItem(Of TemplateVers.PageElement)()
        Version.HeaderCenter.Data = Me.UCheader.CenterElement
        Version.HeaderCenter.Data.Position = lm.Comol.Core.DomainModel.DocTemplateVers.ElementPosition.HeaderCenter

        Version.HeaderRight = New TemplateVers.Domain.DTO.Management.DTO_EditItem(Of TemplateVers.PageElement)()
        Version.HeaderRight.Data = Me.UCheader.RightElement
        Version.HeaderRight.Data.Position = lm.Comol.Core.DomainModel.DocTemplateVers.ElementPosition.HeaderRight

        'Body
        Version.Body = New TemplateVers.Domain.DTO.Management.DTO_EditItem(Of TemplateVers.ElementText)()
        Version.Body.Data = New TemplateVers.ElementText
        Version.Body.Data.IsHTML = Me.UCbody.IsHtml
        Version.Body.Data.Text = Me.UCbody.CurrentText
        Version.Body.Data.Position = lm.Comol.Core.DomainModel.DocTemplateVers.ElementPosition.Body

        'Footer
        Version.FooterLeft = New TemplateVers.Domain.DTO.Management.DTO_EditItem(Of TemplateVers.PageElement)()
        Version.FooterLeft.Data = Me.UCfooter.LeftElement
        Version.FooterLeft.Data.Position = lm.Comol.Core.DomainModel.DocTemplateVers.ElementPosition.FooterLeft

        Version.FooterCenter = New TemplateVers.Domain.DTO.Management.DTO_EditItem(Of TemplateVers.PageElement)()
        Version.FooterCenter.Data = Me.UCfooter.CenterElement
        Version.FooterCenter.Data.Position = lm.Comol.Core.DomainModel.DocTemplateVers.ElementPosition.FooterCenter

        Version.FooterRight = New TemplateVers.Domain.DTO.Management.DTO_EditItem(Of TemplateVers.PageElement)()
        Version.FooterRight.Data = Me.UCfooter.RightElement
        Version.FooterRight.Data.Position = lm.Comol.Core.DomainModel.DocTemplateVers.ElementPosition.FooterRight

        'Signatures
        Version.Signatures = Me.UCsignatures.Signatures
        If IsNothing(Version.Signatures) Then
            Version.Signatures = New List(Of TemplateVers.Domain.DTO.Management.DTO_EditItem(Of TemplateVers.Signature))()
        End If

        Return Version
    End Function
#End Region

#Region "Internal"
    Public Sub InitLink()
        Dim Url As String = "./Edit.aspx"

        Url &= "?idTemplate=" & Me.TemplateId.ToString()
        If (Me.VersionId > 0) Then
            Url &= "&idVersion=" & Me.VersionId.ToString()
        End If

        Me.HYPGoToSimple.NavigateUrl = Url & "&IsAdv=0"
        Me.HYPGoToAdvance.NavigateUrl = Url & "&IsAdv=1"

    End Sub
    Private Function ConvertSignature(ByVal Source As TemplateVers.Domain.DTO.ServiceExport.DTO_Element)
        Dim out As New TemplateVers.Signature

        If TypeOf Source Is TemplateVers.Domain.DTO.ServiceExport.DTO_ElementImage Then
            Dim imgIn As TemplateVers.Domain.DTO.ServiceExport.DTO_ElementImage = DirectCast(Source, TemplateVers.Domain.DTO.ServiceExport.DTO_ElementImage)
            With out
                .HasImage = True
                .IsHTML = False
                .Text = ""
                .HasPDFPositioning = False
                .Height = imgIn.Height
                .PagePlacingMask = 1 'ALL
                .PagePlacingRange = ""
                .Path = CopyImage(imgIn.Path)
                '.Placing = 
                .Position = lm.Comol.Core.DomainModel.DocTemplateVers.SignaturePosition.left
                .Width = imgIn.Width
            End With


        ElseIf TypeOf Source Is TemplateVers.Domain.DTO.ServiceExport.DTO_ElementText Then
            Dim txtIn As TemplateVers.Domain.DTO.ServiceExport.DTO_ElementText = DirectCast(Source, TemplateVers.Domain.DTO.ServiceExport.DTO_ElementText)
            With out
                .HasImage = False
                .IsHTML = txtIn.IsHTML
                .Text = txtIn.Text
                .HasPDFPositioning = False
                .Height = 0
                .PagePlacingMask = 1 'ALL
                .PagePlacingRange = ""
                .Path = ""
                '.Placing = 
                .Position = lm.Comol.Core.DomainModel.DocTemplateVers.SignaturePosition.left
                .Width = 0
            End With
        End If

        Return out
    End Function
    Private Function ConvertElement(ByVal Source As TemplateVers.Domain.DTO.ServiceExport.DTO_Element, ByVal Position As TemplateVers.ElementPosition) As TemplateVers.PageElement
        Dim out As TemplateVers.PageElement

        If TypeOf Source Is TemplateVers.Domain.DTO.ServiceExport.DTO_ElementImage Then
            Dim imgIn As TemplateVers.Domain.DTO.ServiceExport.DTO_ElementImage = DirectCast(Source, TemplateVers.Domain.DTO.ServiceExport.DTO_ElementImage)
            Dim imgOut As New TemplateVers.ElementImage

            With imgOut
                .Alignment = imgIn.Alignment
                .Height = imgIn.Height
                .Path = CopyImage(imgIn.Path)
                .Position = Position
                .Width = imgIn.Width
            End With

            out = imgOut
        ElseIf TypeOf Source Is TemplateVers.Domain.DTO.ServiceExport.DTO_ElementText Then
            Dim txtOut As New TemplateVers.ElementText
            Dim txtIn As TemplateVers.Domain.DTO.ServiceExport.DTO_ElementText = DirectCast(Source, TemplateVers.Domain.DTO.ServiceExport.DTO_ElementText)

            With txtOut
                .Alignment = txtIn.Alignment
                .IsHTML = txtIn.IsHTML
                .Position = Position
                .Text = txtIn.Text
            End With

            out = txtOut

        End If

        Return out

    End Function
    Private Function CopyImage(ByVal Source As String) As String
        'ATTENZIONE!!!
        'NON posso sapere A PRIORI SE la sorgente deriva da configurazione o da SKIN!
        Source = Server.MapPath(Source)
        Return Me.CurrentPresenter.CopySkin(Source)
    End Function
#End Region

#Region "Handler"
    Private Sub CTRLsteps_SetCurrentView(View As lm.Comol.Core.BaseModules.DocTemplate.Presentation.ViewEditTemplateElement) Handles CTRLsteps.SetCurrentView
        Me.CurrentPresenter.ChangePage(View)
    End Sub
    Private Sub LKBsave_Click(sender As Object, e As System.EventArgs) Handles LKBsave.Click
        Me.CurrentPresenter.UpdateCurrentData()
    End Sub
    Private Sub LKBundo_Click(sender As Object, e As System.EventArgs) Handles LKBundo.Click
        Me.CurrentPresenter.InitView()
    End Sub
    Private Sub LKBexportPDF_Click(sender As Object, e As System.EventArgs) Handles LKBexportPDF.Click

        Dim fileName As String

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
            fileName = "Preview"
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
    Private Sub UCSkinImport_Close() Handles UCSkinImport.Close
        Me.PnlModal.Visible = False
    End Sub
    Private Sub UCSkinImport_Confirm(Elements As System.Collections.Generic.List(Of UC_EditSkinImport.ImportedElement)) Handles UCSkinImport.Confirm

        For Each element As UC_EditSkinImport.ImportedElement In Elements
            If Not IsNothing(element.Element) Then
                Select Case element.ElementPlacing
                    Case UC_EditSkinImport.Placing.HeaderLeft
                        Me.UCheader.SetLeftElemt( _
                            ConvertElement(element.Element, lm.Comol.Core.DomainModel.DocTemplateVers.ElementPosition.HeaderLeft), _
                            Me.TemplateId, Me.VersionId)
                    Case (UC_EditSkinImport.Placing.HeaderCenter)
                        Me.UCheader.SetCenterElemt( _
                            ConvertElement(element.Element, lm.Comol.Core.DomainModel.DocTemplateVers.ElementPosition.HeaderCenter), _
                            Me.TemplateId, Me.VersionId)
                    Case UC_EditSkinImport.Placing.HeaderRight
                        Me.UCheader.SetRightElemt( _
                            ConvertElement(element.Element, lm.Comol.Core.DomainModel.DocTemplateVers.ElementPosition.HeaderRight), _
                            Me.TemplateId, Me.VersionId)

                    Case UC_EditSkinImport.Placing.FooterLeft
                        Me.UCfooter.SetLeftElemt( _
                            ConvertElement(element.Element, lm.Comol.Core.DomainModel.DocTemplateVers.ElementPosition.FooterLeft), _
                            Me.TemplateId, Me.VersionId)
                    Case UC_EditSkinImport.Placing.FooterCenter
                        Me.UCfooter.SetCenterElemt( _
                            ConvertElement(element.Element, lm.Comol.Core.DomainModel.DocTemplateVers.ElementPosition.FooterCenter), _
                            Me.TemplateId, Me.VersionId)
                    Case UC_EditSkinImport.Placing.FooterRight
                        Me.UCfooter.SetRightElemt( _
                            ConvertElement(element.Element, lm.Comol.Core.DomainModel.DocTemplateVers.ElementPosition.FooterRight), _
                            Me.TemplateId, Me.VersionId)

                    Case UC_EditSkinImport.Placing.Signature
                        Me.UCsignatures.AddRecSignature( _
                            New TemplateVers.Domain.DTO.Management.DTO_EditItem(Of TemplateVers.Signature) _
                            With {.Data = ConvertSignature(element.Element)} _
                            )
                End Select

            End If
        Next

        Me.PnlModal.Visible = False
    End Sub
    Private Sub BTNaddUp_Click(sender As Object, e As System.EventArgs) Handles BTNaddUp.Click
        Me.UCSkinImport.InitView()
        Me.PnlModal.Visible = True

    End Sub



#Region "Delete"

    Private Sub UCbody_DeletePrevBody(Id As Long) Handles UCbody.DeletePrevBody
        Me.CurrentPresenter.PageElementDeletePrev(Me.TemplateId, Me.VersionId, Id)
    End Sub
    Private Sub UCfooter_DeleteOldElement(Id As Long) Handles UCfooter.DeleteOldElement
        Me.CurrentPresenter.PageElementDeletePrev(Me.TemplateId, Me.VersionId, Id)
    End Sub
    Private Sub UCheader_DeleteOldElement(Id As Long) Handles UCheader.DeleteOldElement
        Me.CurrentPresenter.PageElementDeletePrev(Me.TemplateId, Me.VersionId, Id)
    End Sub

    Private Sub UCsettings_DeletePrevSetting(Id As Long) Handles UCsettings.DeletePrevSetting
        Me.CurrentPresenter.SettingsDeletePrev(Id)
    End Sub

    Private Sub UCsignatures_DeletePrevSignature(Id As Long) Handles UCsignatures.DeletePrevSignature
        Me.CurrentPresenter.SignatureDeletePrev(Me.TemplateId, Me.VersionId, Id)
    End Sub

#End Region

#Region "Delete Selected"

    Private Sub UCheader_DeleteOldElements(Ids As System.Collections.Generic.IList(Of Long)) Handles UCheader.DeleteOldElements
        Me.CurrentPresenter.PageElementDeletePrevs(Me.TemplateId, Me.VersionId, Ids)
    End Sub

    Private Sub UCbody_DeletePrevBodies(Ids As System.Collections.Generic.IList(Of Long)) Handles UCbody.DeletePrevBodies
        Me.CurrentPresenter.PageElementDeletePrevs(Me.TemplateId, Me.VersionId, Ids)
    End Sub

    Private Sub UCfooter_DeleteOldElements(Ids As System.Collections.Generic.IList(Of Long)) Handles UCfooter.DeleteOldElements
        Me.CurrentPresenter.PageElementDeletePrevs(Me.TemplateId, Me.VersionId, Ids)
    End Sub

    Private Sub UCsettings_DeletePrevSettings(Ids As System.Collections.Generic.IList(Of Long)) Handles UCsettings.DeletePrevSettings
        Me.CurrentPresenter.SettingsDeletePrevs(Ids)
    End Sub

    Private Sub UCsignatures_DeletePrevSignatures(Ids As System.Collections.Generic.IList(Of Long)) Handles UCsignatures.DeletePrevSignatures
        Me.CurrentPresenter.SignatureDeletePrevs(Me.TemplateId, Me.VersionId, Ids)
    End Sub


#End Region

#Region "Recover"
    Public Sub RecoverSettings(Settings As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.DTO_EditItem(Of lm.Comol.Core.DomainModel.DocTemplateVers.Settings)) Implements Presentation.IViewDocTemplateEdit.RecoverSettings
        Me.UCsettings.CurrentSettings = Settings.Data
        Me.UCsettings.BindPrevVersion(Settings.PreviousVersion)
    End Sub
    Public Sub RecoverPageElement(Element As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.DTO_EditItem(Of lm.Comol.Core.DomainModel.DocTemplateVers.PageElement)) Implements Presentation.IViewDocTemplateEdit.RecoverPageElement
        If Not IsNothing(Element) _
           AndAlso Not IsNothing(Element.Data) Then

            Select Case Element.Data.Position
                Case lm.Comol.Core.DomainModel.DocTemplateVers.ElementPosition.HeaderLeft
                    UCheader.SetLeftElemt(Element.Data, Me.TemplateId, Me.VersionId)
                    UCheader.SetRevisionLeft(Element.PreviousVersion)
                Case lm.Comol.Core.DomainModel.DocTemplateVers.ElementPosition.HeaderCenter
                    UCheader.SetCenterElemt(Element.Data, Me.TemplateId, Me.VersionId)
                    UCheader.SetRevisionCenter(Element.Data)
                Case lm.Comol.Core.DomainModel.DocTemplateVers.ElementPosition.HeaderRight
                    UCheader.SetRightElemt(Element.Data, Me.TemplateId, Me.VersionId)
                    UCheader.SetRevisionRight(Element.PreviousVersion)

                Case lm.Comol.Core.DomainModel.DocTemplateVers.ElementPosition.Body
                    If TypeOf Element.Data Is lm.Comol.Core.DomainModel.DocTemplateVers.ElementText Then
                        Dim TxtEl As lm.Comol.Core.DomainModel.DocTemplateVers.ElementText = DirectCast(Element.Data, lm.Comol.Core.DomainModel.DocTemplateVers.ElementText)

                        If Not IsNothing(TxtEl) Then
                            Me.UCbody.CurrentText = TxtEl.Text
                            Me.UCbody.IsHtml = TxtEl.IsHTML
                            Me.UCbody.BindPrevVersion(Element.PreviousVersion)
                        End If
                    End If

                Case lm.Comol.Core.DomainModel.DocTemplateVers.ElementPosition.FooterLeft
                    UCfooter.SetLeftElemt(Element.Data, Me.TemplateId, Me.VersionId)
                    UCfooter.SetRevisionLeft(Element.PreviousVersion)
                Case lm.Comol.Core.DomainModel.DocTemplateVers.ElementPosition.FooterCenter
                    UCfooter.SetCenterElemt(Element.Data, Me.TemplateId, Me.VersionId)
                    UCfooter.SetRevisionCenter(Element.PreviousVersion)
                Case lm.Comol.Core.DomainModel.DocTemplateVers.ElementPosition.FooterRight
                    UCfooter.SetRightElemt(Element.Data, Me.TemplateId, Me.VersionId)
                    UCfooter.SetRevisionRight(Element.PreviousVersion)
            End Select

        End If
    End Sub
    Public Sub RecoverSignature(Element As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.DTO_EditItem(Of lm.Comol.Core.DomainModel.DocTemplateVers.Signature)) Implements Presentation.IViewDocTemplateEdit.RecoverSignature
        Me.UCsignatures.AddRecSignature(Element)
    End Sub

    Private Sub UCsettings_RecoverPrevSetting(Id As Long) Handles UCsettings.RecoverPrevSetting
        Me.CurrentPresenter.SettingsRecoverPrev(Id)
    End Sub
    Private Sub UCheader_RecoverLeft(Id As Long) Handles UCheader.RecoverLeft
        Me.CurrentPresenter.PageElementRecoverPrev(Id)
    End Sub
    Private Sub UCheader_RecoverCenter(Id As Long) Handles UCheader.RecoverCenter
        Me.CurrentPresenter.PageElementRecoverPrev(Id)
    End Sub
    Private Sub UCheader_RecoverRight(Id As Long) Handles UCheader.RecoverRight
        Me.CurrentPresenter.PageElementRecoverPrev(Id)
    End Sub
    Private Sub UCbody_RecoverPrevbody(Id As Long) Handles UCbody.RecoverPrevbody
        Me.CurrentPresenter.PageElementRecoverPrev(Id)
    End Sub
    Private Sub UCfooter_RecoverLeft(Id As Long) Handles UCfooter.RecoverLeft
        Me.CurrentPresenter.PageElementRecoverPrev(Id)
    End Sub
    Private Sub UCfooter_RecoverCenter(Id As Long) Handles UCfooter.RecoverCenter
        Me.CurrentPresenter.PageElementRecoverPrev(Id)
    End Sub
    Private Sub UCfooter_RecoverRight(Id As Long) Handles UCfooter.RecoverRight
        Me.CurrentPresenter.PageElementRecoverPrev(Id)
    End Sub
    Private Sub UCsignatures_RecoverPrevSignature(Id As Long) Handles UCsignatures.RecoverPrevSignature
        Me.CurrentPresenter.SignatureRecoverPrev(Id)
    End Sub


#End Region

#End Region


End Class