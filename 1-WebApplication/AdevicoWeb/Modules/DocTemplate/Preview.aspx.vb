Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.DocTemplate
Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.DocTemplate.Presentation
Imports lm.Comol.Core.DomainModel.DocTemplate
Imports TemplateVers = lm.Comol.Core.DomainModel.DocTemplateVers
Imports lm.Comol.Core.DomainModel.Helpers.Export

Public Class TemplatePreview
    Inherits PageBase
    Implements IViewDocTemplatePreview

#Region "Context"
    Private _Presenter As DocTemplatePreviewPresenter
    Private ReadOnly Property CurrentPresenter() As DocTemplatePreviewPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New DocTemplatePreviewPresenter(Me.PageUtility.CurrentContext, Me)
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
            Return False
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property PreloadIdTemplate As Long Implements IViewDocTemplatePreview.PreloadIdTemplate
        Get
            If IsNumeric(Request.QueryString("idTemplate")) Then
                Return CLng(Request.QueryString("idTemplate"))
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property PreloadIdVersion As Long Implements lm.Comol.Core.BaseModules.DocTemplate.Presentation.IViewDocTemplatePreview.PreloadIdVersion
        Get
            If IsNumeric(Request.QueryString("idVersion")) Then
                Return CLng(Request.QueryString("idVersion"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadFromList As Boolean Implements IViewDocTemplatePreview.PreloadFromList
        Get
            If String.IsNullOrEmpty(Request.QueryString("fromList")) Then
                Return False
            Else
                Return (Request.QueryString("fromList").ToLower = "true")
            End If
        End Get
    End Property
    Private ReadOnly Property TemplateBaseUrl As String Implements IViewDocTemplatePreview.TemplateBaseUrl
        Get
            Return Me.FullBaseUrl & MyBase.SystemSettings.DocTemplateSettings.BaseUrl
        End Get
    End Property

    Private Property CurrentIdTemplate As Long Implements IViewDocTemplatePreview.CurrentIdTemplate
        Get
            Return ViewStateOrDefault("CurrentIdTemplate", 0)
        End Get
        Set(value As Long)
            ViewState("CurrentIdTemplate") = value
        End Set
    End Property
    Private Property CurrentIdVersion As Long Implements IViewDocTemplatePreview.CurrentIdVersion
        Get
            Return ViewStateOrDefault("CurrentIdVersion", 0)
        End Get
        Set(value As Long)
            ViewState("CurrentIdVersion") = value
        End Set
    End Property


#End Region

#Region "Internal"
    ''' <summary>
    ''' Nome del cookie utilizzato per l'attivazione del pop-up di download
    ''' </summary>
    ''' <returns>Nome del cookie</returns>
    ''' <remarks>Dev'essere univoco a livello di piattaforma!</remarks>
    Protected ReadOnly Property CookieName() As String
        Get
            Return "COMOL_PreviewTemplate"
        End Get
    End Property
    ''' <summary>
    ''' Per la localizzazione del messaggio visualizzato nel pop-up di download.
    ''' </summary>
    ''' <returns>Messaggio internazionalizzato.</returns>
    Protected ReadOnly Property DisplayMessageToken() As String
        Get
            ' TRADUZIONE DEL MESSAGGIO DA VISUALIZZARE
            Return Resource.getValue("DisplayToken.Message")
        End Get
    End Property
    ''' <summary>
    ''' Per la localizzazione del titolo del messaggio visualizzato nel pop-up di download.
    ''' </summary>
    ''' <returns>Titolo internazionalizzato</returns>
    Protected ReadOnly Property DisplayTitleToken() As String
        Get
            ' TRADUZIONE DEL MESSAGGIO DA VISUALIZZARE
            Return Resource.getValue("DisplayToken.Title")
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Master.ShowDocType = True
    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.CurrentPresenter.InitView()
    End Sub
    Public Overrides Sub BindNoPermessi()
        MLVtemplate.SetActiveView(VIWempty)
        Me.LBmessage.Text = Resource.getValue("ServiceNopermission." & IIf(PreloadIdTemplate = 0, "PreviewAdd", "PreviewEdit"))
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_templatePreview", "Modules", "DocTemplates")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource

            Me.Master.ServiceTitle = .getValue("serviceTitle." & IIf(PreloadIdTemplate = 0, "PreviewAdd", "PreviewEdit"))
            Me.Master.ServiceTitleToolTip = .getValue("serviceTitle." & IIf(PreloadIdTemplate = 0, "PreviewAdd", "PreviewEdit"))
            Me.Master.ServiceNopermission = .getValue("ServiceNopermission." & IIf(PreloadIdTemplate = 0, "PreviewAdd", "PreviewEdit"))

            .setLinkButton(Me.LKBexportRTF, True, True, False, False)
            .setLinkButton(Me.LKBexportPDF, True, True, False, False)

            '.setHyperLink(Me.HYPbackUrl, True, True)

            '.setHyperLink(Me.HYPedit, True, True)
            '.setHyperLink(Me.HYPlist, True, True)
            'BindBackUrl()
        End With
    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"

    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewDocTemplatePreview.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Core.DomainModel.DocTemplateVers.ModuleDocTemplate.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub

    Private Sub DisplaySessionTimeout() Implements IViewDocTemplatePreview.DisplaySessionTimeout
        Dim idCommunity As Integer = 0
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.NewWindow

        If Me.PreloadFromList Then
            dto.DestinationUrl = lm.Comol.Core.DomainModel.DocTemplateVers.rootObject.PreviewTemplate(PreloadIdTemplate, PreloadIdVersion, True)
        Else
            dto.DestinationUrl = lm.Comol.Core.DomainModel.DocTemplateVers.rootObject.PreviewTemplate(PreloadIdTemplate, PreloadIdVersion, False)
        End If

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub

    Private Sub DisplayUnknownTemplate() Implements lm.Comol.Core.BaseModules.DocTemplate.Presentation.IViewDocTemplatePreview.DisplayUnknownTemplate

    End Sub

    Public Sub LoadTemplate(template As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template) Implements lm.Comol.Core.BaseModules.DocTemplate.Presentation.IViewDocTemplatePreview.LoadTemplate
        LKBexportPDF.Visible = True
        LKBexportRTF.Visible = True

        If Not IsNothing(template) Then
            Dim cols As Integer

            'Per test
            Dim ServiceId As Int64 = 1

            If (template.UseSkinHeaderFooter) Then
                template.Header = Master.getTemplateHeader
                template.Footer = Master.getTemplateFooter
            End If

            cols = GetCol(template.Header)

            If cols > 0 Then

                Dim cssClass As String = "ItemCol" + cols.ToString()

                If Not IsNothing(template.Header.Left) Then
                    DIVheaderLeft.Attributes.Add("class", cssClass)
                    LITitemHeaderLeft.Text = GetHTMLString(template.Header.Left)
                    DIVheaderLeft.Visible = True
                Else
                    DIVheaderLeft.Visible = False
                End If

                If Not IsNothing(template.Header.Center) Then
                    DIVheaderCenter.Attributes.Add("class", cssClass)
                    LITitemHeaderCenter.Text = GetHTMLString(template.Header.Center)
                    DIVheaderCenter.Visible = True
                Else
                    DIVheaderCenter.Visible = False
                End If

                If Not IsNothing(template.Header.Right) Then
                    DIVheaderRight.Attributes.Add("class", cssClass)
                    LITitemHeaderRight.Text = GetHTMLString(template.Header.Right)
                    DIVheaderRight.Visible = True
                Else
                    DIVheaderRight.Visible = False
                End If

            Else
                DIVheaderLeft.Visible = False
                DIVheaderCenter.Visible = False
                DIVheaderRight.Visible = False
            End If


            If Not IsNothing(template.Body) Then
                Me.LITitemBody.Text = GetHTMLString(template.Body) '.Text
            Else
                Me.LITitemBody.Text = "no element"
            End If


            cols = GetCol(template.Footer)

            If cols > 0 Then

                Dim cssClass As String = "ItemCol" + cols.ToString()

                If Not IsNothing(template.Footer.Left) Then
                    DIVfooterLeft.Attributes.Add("class", cssClass)
                    LITitemFooterLeft.Text = GetHTMLString(template.Footer.Left)
                    DIVfooterLeft.Visible = True
                Else
                    DIVfooterLeft.Visible = False
                End If

                If Not IsNothing(template.Footer.Center) Then
                    DIVfooterCenter.Attributes.Add("class", cssClass)
                    LITitemFooterCenter.Text = GetHTMLString(template.Footer.Center)
                    DIVfooterCenter.Visible = True
                Else
                    DIVfooterCenter.Visible = False
                End If

                If Not IsNothing(template.Footer.Right) Then
                    DIVfooterRight.Attributes.Add("class", cssClass)
                    LITitemFooterRight.Text = GetHTMLString(template.Footer.Right)
                    DIVfooterRight.Visible = True
                Else
                    DIVfooterRight.Visible = False
                End If

            Else
                DIVfooterLeft.Visible = False
                DIVfooterCenter.Visible = False
                DIVfooterRight.Visible = False
            End If

            If Not IsNothing(template.Signatures) AndAlso template.Signatures.Count() > 0 Then
                PNLsignatures.Visible = True

                Dim ULLeftContent As String = ""
                Dim ULCenterContent As String = ""
                Dim ULRightContent As String = ""
                'Dim Leftcol, Centercol, Rightcol As Boolean
                'Leftcol = False
                'Centercol = False
                'Rightcol = False

                For Each sign As TemplateVers.Domain.DTO.ServiceExport.DTO_Signature In template.Signatures
                    Dim contentHTML As String = ""

                    contentHTML = "<li>" + vbCrLf
                    contentHTML += sign.Text + vbCrLf

                    If (sign.HasImage) Then
                        Dim Style As String = """"
                        If (sign.Width > 0 AndAlso sign.Height > 0) Then
                            Style = "Style=""width: " + sign.Width.ToString().Replace(",", ".") + _
                            "px; height: " + sign.Height.ToString().Replace(",", ".") + "px;"""

                        End If

                        contentHTML += "<br/>" + vbCrLf + "<img " + Style + " src=""" + sign.Path + """ alt=""" + sign.Id.ToString() + """/>"
                    End If

                    contentHTML += "</li>"

                    Select Case sign.Position
                        Case DocTemplateVers.SignaturePosition.left
                            'Leftcol = True
                            ULLeftContent += contentHTML

                        Case DocTemplateVers.SignaturePosition.center
                            'Centercol = True
                            ULCenterContent += contentHTML

                        Case DocTemplateVers.SignaturePosition.right
                            'Rightcol = True
                            ULRightContent += contentHTML
                    End Select

                Next

                'Dim sgncols As Integer = If(Leftcol, 1, 0) + If(Centercol, 1, 0) + If(Rightcol, 1, 0)
                LITsignatures.Text = ""
                If Not ULLeftContent = "" Then
                    LITsignatures.Text &= "<td><ul>" + ULLeftContent + "</ul></td>"
                Else
                    LITsignatures.Text &= "<td>&nbsp;</td>"
                End If



                If Not ULLeftContent = "" Then
                    LITsignatures.Text &= "<td><ul>" + ULCenterContent + "</ul></td>"
                Else
                    LITsignatures.Text &= "<td>&nbsp;</td>"
                End If


                If Not ULRightContent = "" Then
                    LITsignatures.Text &= "<td><ul>" + ULRightContent + "</ul></td>"
                Else
                    LITsignatures.Text &= "<td>&nbsp;</td>"
                End If

                If Not LITsignatures.Text = "" Then
                    LITsignatures.Text = "<table style=""width:100%"">" + LITsignatures.Text + "</table>"
                End If
            Else
                PNLsignatures.Visible = False
            End If

            Me.MLVtemplate.SetActiveView(VIWtemplate)
        Else
            DIVheaderLeft.Visible = False
            DIVheaderCenter.Visible = False
            DIVheaderRight.Visible = False

            DIVfooterLeft.Visible = False
            DIVfooterCenter.Visible = False
            DIVfooterRight.Visible = False

            Me.MLVtemplate.SetActiveView(VIWempty)
            Me.LBmessage.Text = "#No template!"
        End If


    End Sub

    Private Function GetCol(ByVal PageItem As TemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter) As Integer
        If IsNothing(PageItem) Then
            Return 0
        End If

        Dim cols As Integer = 0

        If Not IsNothing(PageItem.Left) Then
            cols += 1
        End If

        If Not IsNothing(PageItem.Right) Then
            cols += 1
        End If

        If Not IsNothing(PageItem.Center) Then
            cols += 1
        End If

        Return cols

    End Function

    Private Function GetHTMLString(ByVal element As TemplateVers.Domain.DTO.ServiceExport.DTO_Element)

        Dim HTML As String = ""

        If IsNothing(element) Then
            Return HTML
        ElseIf TypeOf element Is TemplateVers.Domain.DTO.ServiceExport.DTO_ElementText Then
            Dim txtel As TemplateVers.Domain.DTO.ServiceExport.DTO_ElementText = DirectCast(element, TemplateVers.Domain.DTO.ServiceExport.DTO_ElementText)
            HTML = txtel.Text

        ElseIf TypeOf element Is TemplateVers.Domain.DTO.ServiceExport.DTO_ElementImage Then

            Dim imgel As TemplateVers.Domain.DTO.ServiceExport.DTO_ElementImage = DirectCast(element, TemplateVers.Domain.DTO.ServiceExport.DTO_ElementImage)

            Dim Style As String = ""

            If (imgel.Width > 0 AndAlso imgel.Height > 0) Then
                Style = "Style=""width: " + imgel.Width.ToString().Replace(",", ".") + _
                "px; height: " + imgel.Height.ToString().Replace(",", ".") + "px;"""

            End If

            HTML = "<img " + Style + " src=""" & imgel.Path & """ alt=""Image""/>"

        ElseIf TypeOf element Is TemplateVers.Domain.DTO.ServiceExport.DTO_ElementImageMulti Then
            'Dim HTML As String = ""
            Dim mimgel As TemplateVers.Domain.DTO.ServiceExport.DTO_ElementImageMulti = DirectCast(element, TemplateVers.Domain.DTO.ServiceExport.DTO_ElementImageMulti)

            For Each imgel As TemplateVers.Domain.DTO.ServiceExport.DTO_ElementImage In mimgel.ImgElements
                HTML &= "<img src=""" & imgel.Path & """ alt=""MultiImage""/>"
            Next
            'Return HTML
        End If

        Dim cssContainer As String = ""

        Select Case element.Alignment
            Case DocTemplateVers.ElementAlignment.BottomLeft
                cssContainer = "style=""text-align:left; vertical-align:bottom;"""
            Case DocTemplateVers.ElementAlignment.BottomCenter
                cssContainer = "style=""text-align:center; vertical-align:bottom;"""
            Case DocTemplateVers.ElementAlignment.BottomRight
                cssContainer = "style=""text-align:right; vertical-align:bottom;"""
            Case DocTemplateVers.ElementAlignment.MiddleLeft
                cssContainer = "style=""text-align:left; vertical-align:middle;"""
            Case DocTemplateVers.ElementAlignment.MiddleCenter
                cssContainer = "style=""text-align:center; vertical-align:middle;"""
            Case DocTemplateVers.ElementAlignment.MiddleRight
                cssContainer = "style=""text-align:right; vertical-align:middle;"""
            Case DocTemplateVers.ElementAlignment.TopLeft
                cssContainer = "style=""text-align:left; vertical-align:top;"""
            Case DocTemplateVers.ElementAlignment.TopCenter
                cssContainer = "style=""text-align:center; vertical-align:top;"""
            Case DocTemplateVers.ElementAlignment.TopRight
                cssContainer = "style=""text-align:right; vertical-align:top;"""
        End Select

        If Not cssContainer = "" Then
            HTML = "<p " & cssContainer & ">" & HTML & "</p>"
        End If

        'Me.Lit_Template.Text &= HTML
        Return HTML
    End Function

    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idTemplate As Long, action As lm.Comol.Core.DomainModel.DocTemplateVers.ModuleDocTemplate.ActionType) Implements lm.Comol.Core.BaseModules.DocTemplate.Presentation.IViewDocTemplatePreview.SendUserAction
        ' Me.PageUtility.AddActionToModule(idCommunity, idModule, action, , InteractionType.UserWithLearningObject)
    End Sub
    Public Function GetSysFooter() As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter Implements lm.Comol.Core.BaseModules.DocTemplate.Presentation.IViewDocTemplatePreview.GetSysFooter
        Return Master.getTemplateFooter()
    End Function
    Public Function GetSysHeader() As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter Implements lm.Comol.Core.BaseModules.DocTemplate.Presentation.IViewDocTemplatePreview.GetSysHeader
        Return Master.getTemplateHeader()
    End Function
#End Region

#Region "Handler"
    Private Sub TemplateList_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Me.Master.ShowDocType = True
    End Sub
    Private Sub LKBexportPDF_Click(sender As Object, e As System.EventArgs) Handles LKBexportPDF.Click

        Dim fileName As String = "test"

        Try
            fileName = String.Format(Resource.getValue("TemplateFile.Name"))
        Catch ex As Exception
        End Try

        Try
            CurrentPresenter.ExportToPdf(fileName, Response, New HttpCookie(CookieName, HDNdownloadTokenValue.Value))

        Catch ex As Exception
        End Try

    End Sub
    Private Sub LKBexportRTF_Click(sender As Object, e As System.EventArgs) Handles LKBexportRTF.Click
        Response.Clear()

        Dim fileName As String = "test"

        Try
            fileName = String.Format(Resource.getValue("TemplateFile.Name"))
        Catch ex As Exception
        End Try

        Try
            CurrentPresenter.ExportToRtf(fileName, Response, New HttpCookie(CookieName, HDNdownloadTokenValue.Value))
        Catch ex As Exception
        End Try
        Response.End()
    End Sub
#End Region

End Class