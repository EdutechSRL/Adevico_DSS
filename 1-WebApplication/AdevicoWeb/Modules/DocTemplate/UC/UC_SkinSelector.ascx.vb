Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.Standard.Skin

Imports dt = lm.Comol.Core.DomainModel.DocTemplateVers

''' <summary>
''' 
''' </summary>
''' <remarks>
''' Questo controllo NON UTILIZZA MVP per i seguenti motivi:
''' 1. La maggior parte dei controlli utlizzati sono autonomi (selettore comunità) o non necessitano di logiche di business (controlli web)
''' 2. Gli unici dati che vengono recuperati, vanno ad utilizzare SKIN, che però NON sono nel lm.Comol.Core.BaseModule, ma in lm.Comol.Modules.Standard, creando più casini che altro...
''' </remarks>
Public Class UC_SkinSelector
    Inherits BaseControl

#Region "Context"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _SkinService As lm.Comol.Modules.Standard.Skin.Business.ServiceSkin

    Private ReadOnly Property ServiceSkinNew As lm.Comol.Modules.Standard.Skin.Business.ServiceSkin
        Get
            If IsNothing(_SkinService) Then
                _SkinService = New lm.Comol.Modules.Standard.Skin.Business.ServiceSkin(Me.CurrentContext)
            End If

            Return _SkinService
        End Get
    End Property
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
        End Get
    End Property
#End Region

#Region "Internal"

    Private CurrentTemplateHeader As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter
    Private CurrentTemplateFooter As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter

    Public ReadOnly Property SkinFullBaseUrl As String
        Get
            Return Me.PageUtility.ApplicationUrlBase
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            BindCommunities()
            Me.UCorgSel.BindOrganizations(0)
        End If
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_templateEdit", "Modules", "DocTemplates")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLiteral(LITskin_t)
            .setLiteral(LITheaderLogo_t)
            .setLiteral(LITheaderText_t)
            .setLiteral(LITfooterLogos_t)
            .setLiteral(LITfooterText)
            .setLiteral(LITnote_t)

            .setLabel(LBLcommunityID_t)
            .setLabel(LBLlanguageCode_t)
            .setLabel(LBLorganizationID_t)

            .setLinkButton(LKBundoCom, True, True, False, False)
            .setLinkButton(LKBload, True, True, False, False)
            .setLinkButton(LKBClearCache, True, True, False, False)
            .setLinkButton(LKBundoOrgn, True, True, False, False)
            .setLinkButton(LKBshowComSel, True, True, False, False)
            .setLinkButton(LKBshowOrgnSel, True, True, False, False)
        End With
    End Sub
#End Region

#Region "Internal"
    Public Function getTemplateHeader() As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter
        If IsNothing(CurrentTemplateHeader) Then
            GetTemplate()
        End If

        Return CurrentTemplateHeader
    End Function
    Public Function getTemplateFooter() As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter
        If IsNothing(CurrentTemplateFooter) Then
            GetTemplate()
        End If

        Return CurrentTemplateFooter
    End Function
    Private Function GetTemplate(Optional ByVal IsHTML As Boolean = False) As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template

        Dim ComId, OrgnId As Integer

        Try
            ComId = System.Convert.ToInt32(Me.UCcomSel.SelectedCommunitiesID(0))
        Catch ex As Exception
            ComId = -1
        End Try

        If ComId <= 0 Then
            Try
                OrgnId = Me.UCorgSel.CurrentOrganizationID
            Catch ex As Exception
                OrgnId = -1
            End Try
        Else
            Me.TXBorganizationID.Text = ""
            OrgnId = -1
        End If

        'EVENTUALMENTE! correggere gli URL passati nel caso ci siano problemi nel render HTML o nell'EXPORT!
        Dim BaseUrl As String = Me.SkinFullBaseUrl
        Dim SkinVirtualPath As String = SystemSettings.SkinSettings.SkinVirtualPath

        Dim LangCode As String = Me.DDL_TestLang.SelectedValue.ToString()

        'RIVEDERE!!!
        'Tale logica non mi piace, è ridondante, ma fare altrimenti = metter mano a tutti gli altri servizi che la utilizzano...
        'Inoltre NON impostare il font-size per il footer, rischia di avere footer diversi a seconda di altre impostazioni nel render del document...

        Dim FooterFontSize As Integer = SystemSettings.DocTemplateSettings.FooterFontSize

        If IsHTML Then
            FooterFontSize = -1
        End If

        Dim title As String = "Title"

        Try
            title = Resource.getValue("TemplateTitle")
        Catch ex As Exception

        End Try

        Dim Template As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template = _
            Me.ServiceSkinNew.GetTemplateCommunitySkin( _
                BaseUrl, _
                SkinVirtualPath, _
                Me.SystemSettings.DefaultLanguage.Codice, _
                LangCode, _
                title, _
                ComId, _
                OrgnId,
                0, _
                getConfigTemplate(), _
                FooterFontSize)

        If Not IsHTML Then
            CurrentTemplateHeader = Template.Header
            CurrentTemplateFooter = Template.Footer
        End If

        Return Template
    End Function
    Private Sub BindCommunities()
        Me.UCcomSel.SelectionMode = ListSelectionMode.Single
        Me.UCcomSel.isModalitaAmministrazione = True
        Me.UCcomSel.AllowMultipleOrganizationSelection = False
        Me.UCcomSel.AllowCommunityChangedEvent = True
        Me.UCcomSel.InitializeControl(-1)

    End Sub

    Private Function HasComIdUpdate(ByVal ComId As Integer) As Boolean
        Dim HasUpadate As Boolean = True
        Dim Id As Integer = -1
        Try
            Id = Me.ViewState("PreviousComId")
        Catch ex As Exception
            Id = -1
        End Try

        If Id = ComId Then
            HasUpadate = False
        Else
            Me.ViewState("PreviousComId") = ComId
        End If

        Return HasUpadate
    End Function
    Private Sub SetCommunityName(ByVal ComId As Integer)

        If (ComId > 0) Then
            Me.TXBcommunityID.Text = Me.UCcomSel.GetCommunityName(ComId)
        End If

    End Sub
    Public Function getConfigTemplate() As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template

        Dim ConfTemplate As New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template
        ConfTemplate.Header = New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter
        ConfTemplate.Footer = New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter

        'Header
        Dim ImgElement As New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementImage()
        Dim TxtElementH As New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementText()

        ImgElement.Path = Me.SkinFullBaseUrl & SystemSettings.SkinSettings.HeadLogo.Url
        ImgElement.Alignment = lm.Comol.Core.DomainModel.Helpers.Export.ElementAlignment.MiddleCenter

        TxtElementH.IsHTML = True

        TxtElementH.Text = SystemSettings.SkinSettings.HeadLogo.Alt

        If Not String.IsNullOrEmpty(TxtElementH.Text) Then
            TxtElementH.Text = "<h1>" & TxtElementH.Text & "</h1>"
        End If

        ConfTemplate.Header.Left = ImgElement
        ConfTemplate.Header.Right = TxtElementH
        ConfTemplate.Header.Center = Nothing

        'Footer

        Dim ImgElements As New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementImageMulti()
        ImgElements.Alignment = lm.Comol.Core.DomainModel.DocTemplateVers.ElementAlignment.MiddleCenter
        ImgElements.ImgElements = New List(Of lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementImage)()

        Dim TxtElementF As New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementText()

        For Each logo As Comol.Entity.Configuration.SkinSettings.Logo In SystemSettings.SkinSettings.FootLogos
            Dim ImgelementF As New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementImage()

            ImgelementF.Path = SkinFullBaseUrl & logo.Url
            ImgelementF.Alignment = lm.Comol.Core.DomainModel.Helpers.Export.ElementAlignment.MiddleCenter

            ImgElements.ImgElements.Add(ImgelementF)

        Next

        TxtElementF.IsHTML = True
        TxtElementF.Alignment = lm.Comol.Core.DomainModel.Helpers.Export.ElementAlignment.MiddleCenter
        TxtElementF.Text = SystemSettings.SkinSettings.FootText

        ConfTemplate.Footer.Left = ImgElements
        ConfTemplate.Footer.Center = Nothing
        ConfTemplate.Footer.Right = TxtElementF

        Return ConfTemplate

    End Function
#End Region

#Region "Handler"
    Private Sub LBKshowComSel_Click(sender As Object, e As System.EventArgs) Handles LKBshowComSel.Click
        If Me.PNLcommunity.Visible Then
            Me.PNLcommunity.Visible = False

            Dim ComId As Integer = 0

            Try
                ComId = Me.UCcomSel.SelectedCommunitiesID(0)
            Catch ex As Exception

            End Try

            SetCommunityName(ComId)
            Try
                Me.LKBshowComSel.Text = Resource.getValue("LKBshowComSel.text")
            Catch ex As Exception
                Me.LKBshowComSel.Text = "Show Selector"
            End Try

        Else
            Me.PNLcommunity.Visible = True

            Try
                Me.LKBshowComSel.Text = Resource.getValue("LKBshowComSel.hide")
            Catch ex As Exception
                Me.LKBshowComSel.Text = "#Hide Com Selector"
            End Try

        End If

    End Sub
    Private Sub UCcomSel_SelectedCommunityChanged(CommunityID As Integer) Handles UCcomSel.SelectedCommunityChanged
        If HasComIdUpdate(CommunityID) Then
            SetCommunityName(CommunityID)
            Me.PNLcommunity.Visible = False
            Try
                Me.LKBshowComSel.Text = Resource.getValue("LKBshowComSel.text")
            Catch ex As Exception
                Me.LKBshowComSel.Text = "Show Selector"
            End Try

        End If
    End Sub
    Private Sub LKBload_Click(sender As Object, e As System.EventArgs) Handles LKBload.Click
        Dim Template As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template = Me.GetTemplate(True)

        If Not IsNothing(Template) Then

            If Not IsNothing(Template.Header) Then

                If Not IsNothing(Template.Header.Left) Then
                    Dim logo As dt.Domain.DTO.ServiceExport.DTO_ElementImage = _
                        DirectCast(Template.Header.Left, dt.Domain.DTO.ServiceExport.DTO_ElementImage)
                    Me.IMGheaderLogo.ImageUrl = logo.Path

                    Me.LBLheaderLogoinfo.Text = "Path: " & logo.Path & " - Altre info..."
                End If

                If Not IsNothing(Template.Header.Right) Then
                    Dim text As dt.Domain.DTO.ServiceExport.DTO_ElementText = DirectCast(Template.Header.Right, dt.Domain.DTO.ServiceExport.DTO_ElementText)

                    Me.LBLheaderText.Text = text.Text
                End If

            End If

            If Not IsNothing(Template.Footer) Then
                If Not IsNothing(Template.Footer.Left) Then
                    Dim images As dt.Domain.DTO.ServiceExport.DTO_ElementImageMulti = _
                        DirectCast(Template.Footer.Left, dt.Domain.DTO.ServiceExport.DTO_ElementImageMulti)

                    Me.RPTfooterLogos.DataSource = images.ImgElements
                    Me.RPTfooterLogos.DataBind()

                End If

                If Not IsNothing(Template.Footer.Right) Then
                    Dim txt As dt.Domain.DTO.ServiceExport.DTO_ElementText = _
                        DirectCast(Template.Footer.Right, dt.Domain.DTO.ServiceExport.DTO_ElementText)


                    Me.LBLfooterText.Text = txt.Text
                End If
            End If


        End If


        Me.PNLpreview.Visible = True

    End Sub
    Private Sub RPTfooterLogos_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTfooterLogos.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim img As dt.Domain.DTO.ServiceExport.DTO_ElementImage = DirectCast(e.Item.DataItem, dt.Domain.DTO.ServiceExport.DTO_ElementImage)

            Dim IMGfooterLogos As System.Web.UI.WebControls.Image = e.Item.FindControl("IMGfooterLogos")

            If Not IsNothing(IMGfooterLogos) Then
                IMGfooterLogos.ImageUrl = img.Path
                IMGfooterLogos.AlternateText = img.Path
            End If

        End If
    End Sub
    
    Private Sub LKBshowOrgnSel_Click(sender As Object, e As System.EventArgs) Handles LKBshowOrgnSel.Click
        If Me.PNLorganization.Visible Then
            Me.PNLorganization.Visible = False
            Try
                Me.LKBshowOrgnSel.Text = Resource.getValue("LKBshowOrgnSel.text")
            Catch ex As Exception
                Me.LKBshowOrgnSel.Text = "Show Selector"
            End Try

        Else
            Me.PNLorganization.Visible = True
            Try
                Me.LKBshowOrgnSel.Text = Resource.getValue("LKBshowOrgnSel.hide")
            Catch ex As Exception
                Me.LKBshowOrgnSel.Text = "Hide Selector"
            End Try

        End If
    End Sub
    Private Sub LKBundoCom_Click(sender As Object, e As System.EventArgs) Handles LKBundoCom.Click
        Me.UCcomSel.InitializeControl(-1)
        Me.TXBcommunityID.Text = ""

        Me.PNLcommunity.Visible = False
        Try
            Me.LKBshowComSel.Text = Resource.getValue("LKBshowComSel.text")
        Catch ex As Exception
            Me.LKBshowComSel.Text = "Show Selector"
        End Try
    End Sub
    Private Sub LKBundoOrgn_Click(sender As Object, e As System.EventArgs) Handles LKBundoOrgn.Click

        Me.TXBorganizationID.Text = ""

        Me.PNLorganization.Visible = False

        Try
            Me.LKBshowOrgnSel.Text = Resource.getValue("LKBshowOrgnSel.text")
        Catch ex As Exception
            Me.LKBshowOrgnSel.Text = "Show Selector"
        End Try
    End Sub
    Private Sub UCorgSel_SelectedOrgnChanged(OrgId As Integer, OrgnName As String) Handles UCorgSel.SelectedOrgnChanged
        If OrgId > 0 Then


            Me.TXBorganizationID.Text = OrgnName 'LKBselOrgn.Text
        Else
            Me.TXBorganizationID.Text = ""
        End If

        Me.PNLorganization.Visible = False

        Try
            Me.LKBshowOrgnSel.Text = Resource.getValue("LKBshowOrgnSel.text")
        Catch ex As Exception
            Me.LKBshowOrgnSel.Text = "Show Selector"
        End Try
    End Sub
#End Region

End Class