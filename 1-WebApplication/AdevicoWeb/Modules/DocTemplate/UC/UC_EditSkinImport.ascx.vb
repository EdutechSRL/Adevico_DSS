Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.Standard.Skin

Public Class UC_EditSkinImport
    Inherits BaseControl

#Region "Context"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _SkinService As lm.Comol.Modules.Standard.Skin.Business.ServiceSkin

    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Private ReadOnly Property ServiceSkinNew As lm.Comol.Modules.Standard.Skin.Business.ServiceSkin
        Get
            If IsNothing(_SkinService) Then
                _SkinService = New lm.Comol.Modules.Standard.Skin.Business.ServiceSkin(Me.CurrentContext)
            End If

            Return _SkinService
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
    Public Event Close()
    Public Event Confirm(ByVal Elements As List(Of ImportedElement))

    Private _BaseUrl As String
    Private _Elements As List(Of ImportedElement)
    Private _id As Long = 0

    'Private Function GetCurrentOrganizationId() As Integer
    '    Dim Organization_Id As Integer = 0

    '    If Me.CurrentContext.UserContext.CurrentCommunityID > 0 Then
    '        Organization_Id = PageUtility.ComunitaCorrente.GetOrganizzazioneID() '<- questo funziona
    '        'Me.CurrentContext.UserContext.CurrentCommunityOrganizationID '
    '    Else
    '        'Non funziona nessuno dei due...
    '        'Organization_Id = PageUtility.CurrentUser.ORGNDefault_id 'Me.CurrentContext.UserContext.UserDefaultOrganizationId '
    '        Organization_Id = PageUtility.UserDefaultIdOrganization
    '    End If
    '    Return Organization_Id
    'End Function
    Public ReadOnly Property SkinBaseUrl As String
        Get
            If String.IsNullOrEmpty(_BaseUrl) Then
                _BaseUrl = PageUtility.BaseUrl
            End If

            Return _BaseUrl
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Not Page.IsPostBack Then
        '    Me.InitView()
        'End If
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_templateEdit", "Modules", "DocTemplates")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLinkButton(LKBundob, True, True, False, False)
            .setLinkButton(LKBundot, True, True, False, False)
            '.setLinkButton(LKBcomSelector, True, True, False, False)
            '.setLinkButton(LKBorgnSelector, True, True, False, False)
            .setLinkButton(LKBnextt, True, True, False, False)
            .setLinkButton(LKBnextb, True, True, False, False)
            .setLinkButton(LKBbackt, True, True, False, False)
            .setLinkButton(LKBbackb, True, True, False, False)
            .setLinkButton(LKBconfirmt, True, True, False, False)
            .setLinkButton(LKBconfirmb, True, True, False, False)
            '.setRadioButtonList(Me.RBLsource, "0")
            '.setRadioButtonList(Me.RBLsource, "1")
            '.setRadioButtonList(Me.RBLsource, "2")
            '.setRadioButtonList(Me.RBLsource, "3")
        End With
    End Sub
#End Region

#Region "Internal"
    Public Sub InitView()

        ShowByStep(ImportStep.SourceType)
        'LinkButton (Nav)
        'Me.LKBcomSelector.Visible = False
        'Me.LKBorgnSelector.Visible = False
        'Me.LKBback.Visible = False
        'Me.LKBconfirm.Visible = False
        'Me.LKBnext.Visible = True

        ''Panel (Nav)
        'Me.PNLmain.Visible = True
        'Me.PNLselCom.Visible = False
        'Me.PNLselOrgn.Visible = False
        'Me.PNLelSelector.Visible = False

        'Bind RadioButtonList
        Me.BindRBLSource()

        'Organization selector (Init)
        Me.UCorgSelector.BindOrganizations(0)

        'Community Selector (Init)
        Me.UCcomSelector.SelectionMode = ListSelectionMode.Single
        Me.UCcomSelector.isModalitaAmministrazione = True
        Me.UCcomSelector.AllowMultipleOrganizationSelection = False
        Me.UCcomSelector.AllowCommunityChangedEvent = True
        Me.UCcomSelector.InitializeControl(-1)

    End Sub
    Private Function GetId() As Long
        _id += 1
        Return _id
    End Function
    Private Sub Bindelements(ByVal IsForPreview As Boolean)
        Dim Elements As New List(Of ImportedElement)
        Dim Template As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template = GetTemplate(True)

        If Not IsNothing(Template) Then
            If Not IsNothing(Template.Header) Then
                If Not IsNothing(Template.Header.Left) Then
                    Template.Header.Left.Id = Me.GetId()

                    Elements.Add( _
                        New ImportedElement With { _
                            .Element = Template.Header.Left, _
                            .SourcePosition = lm.Comol.Core.DomainModel.DocTemplateVers.ElementPosition.HeaderLeft, _
                            .ElementPlacing = Placing.HeaderLeft})
                End If
            End If

            If Not IsNothing(Template.Footer) Then

                If Not IsNothing(Template.Footer.Left) Then

                    If TypeOf Template.Footer.Left Is lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementImageMulti Then
                        Dim DTO_eim As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementImageMulti = DirectCast(Template.Footer.Left, lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementImageMulti)

                        Dim i As Integer = 0

                        For Each img As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementImage In DTO_eim.ImgElements
                            Dim impElement As ImportedElement = New ImportedElement()
                            img.Id = GetId()

                            impElement.Element = img
                            impElement.SourcePosition = lm.Comol.Core.DomainModel.DocTemplateVers.ElementPosition.FooterLeft

                            If (i = 0) Then
                                impElement.ElementPlacing = Placing.FooterLeft
                            ElseIf (i = 1) Then
                                impElement.ElementPlacing = Placing.FooterCenter
                            Else
                                impElement.ElementPlacing = Placing.Signature
                            End If

                            i += 1
                            Elements.Add(impElement)
                        Next

                    Else
                        Template.Footer.Left.Id = Me.GetId()
                        Elements.Add( _
                        New ImportedElement With { _
                            .Element = Template.Footer.Left, _
                            .SourcePosition = lm.Comol.Core.DomainModel.DocTemplateVers.ElementPosition.FooterLeft, _
                            .ElementPlacing = Placing.FooterLeft})
                    End If

                End If

                If Not IsNothing(Template.Footer.Right) Then
                    Template.Footer.Right.Id = Me.GetId()
                    Elements.Add( _
                        New ImportedElement With { _
                            .Element = Template.Footer.Right, _
                            .SourcePosition = lm.Comol.Core.DomainModel.DocTemplateVers.ElementPosition.FooterRight, _
                            .ElementPlacing = Placing.FooterRight})
                End If

            End If

            'Bind Repeater
            If (IsForPreview) Then
                Me.RPTfotElm.DataSource = Elements
                Me.RPTfotElm.DataBind()
            End If

        End If

        If Not IsForPreview Then
            _Elements = Elements
        End If

    End Sub
    Private Function GetImageHtml(ByVal ImgElement As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementImage) As String
        Dim html As String = ""
        html &= "<img src=""" & ImgElement.Path & """ alt=""" & ImgElement.Path & """ />"

        Return html
    End Function
    Private Function GetTextHtml(ByVal TxtElement As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementText) As String
        Dim html As String = ""
        If TxtElement.IsHTML Then
            html = TxtElement.Text
        Else
            html = "<p>" & TxtElement.Text & "</p>"
        End If

        Return html
    End Function
    Private Sub BindDDLPosition(ByRef DDLnewPosition As DropDownList, ByVal DefPosition As Placing)
        DDLnewPosition.Items.Clear()

        Dim items As Integer()
        Try
            items = System.Enum.GetValues(GetType(Placing))
        Catch ex As Exception

        End Try

        For Each itm As Integer In items
            Dim li As New System.Web.UI.WebControls.ListItem
            li.Value = itm
            li.Text = Resource.getValue("NewPlacing." & itm.ToString)
            If itm = DefPosition Then
                li.Selected = True
            End If
            DDLnewPosition.Items.Add(li)
        Next

    End Sub
    Private Function GetTemplate(ByVal IsForPreview As Boolean) As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template

        Dim ComId, OrgnId As Integer

        Dim Fontsize As Integer = -1

        If (IsForPreview = False) Then
            Fontsize = SystemSettings.DocTemplateSettings.FooterFontSize
        End If


        Select Case Me.RBLsource.SelectedValue
            Case "0"    'Portal
                ComId = 0
                OrgnId = 0

            Case "1"    'Current
                ComId = Me.CurrentContext.UserContext.CurrentCommunityID
                OrgnId = PageUtility.GetSkinIdOrganization

            Case "2"    'Organization
                ComId = 0
                OrgnId = Me.UCorgSelector.CurrentOrganizationID

            Case "3"    'Community
                Try
                    ComId = System.Convert.ToInt32(Me.UCcomSelector.SelectedCommunitiesID(0))
                Catch ex As Exception
                    ComId = -1
                End Try

        End Select

        'EVENTUALMENTE! correggere gli URL passati nel caso ci siano problemi nel render HTML o nell'EXPORT!
        'Dim VirPath As String = Me.BaseUrl & Me.SystemSettings.SkinSettings.SkinVirtualPath
        'Dim BaseUrl As String = "~/" 'Me.SkinFullBaseUrl
        'Dim SkinVirtualPath As String = SystemSettings.SkinSettings.SkinVirtualPath

        Dim LangCode As String = Me.PageUtility.CurrentUser.Lingua.Codice


        'RIVEDERE!!!
        'Tale logica non mi piace, è ridondante, ma fare altrimenti = metter mano a tutti gli altri servizi che la utilizzano...
        'Inoltre NON impostare il font-size per il footer, rischia di avere footer diversi a seconda di altre impostazioni nel render del document...

        Dim FooterFontSize As Integer = SystemSettings.DocTemplateSettings.FooterFontSize
        FooterFontSize = -1

        Dim Template As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template = _
            Me.ServiceSkinNew.GetTemplateCommunitySkin( _
                Me.SkinBaseUrl, _
                SystemSettings.SkinSettings.SkinVirtualPath, _
                Me.SystemSettings.DefaultLanguage.Codice, _
                Me.PageUtility.CurrentUser.Lingua.Codice, _
                "", _
                ComId, _
                OrgnId, _
                0, _
                getConfigTemplate(), _
                Fontsize)

        Return Template
    End Function
    Public Function getConfigTemplate() As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template

        Dim ConfTemplate As New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template
        ConfTemplate.Header = New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter
        ConfTemplate.Footer = New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter

        'Header
        Dim ImgElement As New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementImage()
        Dim TxtElementH As New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementText()

        ImgElement.Path = Me.PageUtility.ApplicationUrlBase & SystemSettings.SkinSettings.HeadLogo.Url
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

            ImgelementF.Path = SkinBaseUrl & logo.Url
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

    Private Sub Import() 'LKBconfirm_Click(sender As Object, e As System.EventArgs) Handles LKBconfirm.Click
        Dim OldElements As New List(Of ImportedElement)
        Dim OutElements As New List(Of ImportedElement)

        Me.Bindelements(False)

        If Not IsNothing(_Elements) Then
            OldElements = _Elements
        End If

        For Each item As RepeaterItem In Me.RPTfotElm.Items
            If item.ItemType = ListItemType.Item OrElse item.ItemType = ListItemType.AlternatingItem Then
                Dim HIDid As HiddenField = item.FindControl("HIDid")
                Dim CbxSel As CheckBox = item.FindControl("CbxSel")
                Dim DDLnewPosition As DropDownList = item.FindControl("DDLnewPosition")

                If Not IsNothing(HIDid) _
                    AndAlso Not IsNothing(CbxSel) _
                    AndAlso Not IsNothing(DDLnewPosition) _
                    AndAlso Not (DDLnewPosition.SelectedValue = Placing.none) Then

                    If CbxSel.Checked Then
                        Try
                            Dim id As Int64 = System.Convert.ToInt64(HIDid.Value)
                            Dim elm As New ImportedElement()

                            Dim DTOelm As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Element = _
                                (From el As ImportedElement In OldElements Where (el.Element.Id = id) Select el.Element).FirstOrDefault()

                            elm.Element = DTOelm
                            elm.ElementPlacing = DDLnewPosition.SelectedValue

                            OutElements.Add(elm)

                        Catch ex As Exception

                        End Try


                    End If
                End If

            End If

        Next

        If Not IsNothing(OutElements) Then
            RaiseEvent Confirm(OutElements)
        Else
            RaiseEvent Close()
        End If

        Me.InitView()

    End Sub
#End Region

#Region "Handler"
    Private Sub LKBnextb_Click(sender As Object, e As System.EventArgs) Handles LKBnextb.Click
        NextStep()
    End Sub

    Private Sub LKBnextt_Click(sender As Object, e As System.EventArgs) Handles LKBnextt.Click
        NextStep()
    End Sub


    Private Sub LKBbackb_Click(sender As Object, e As System.EventArgs) Handles LKBbackb.Click
        BackStep()
    End Sub

    Private Sub LKBbackt_Click(sender As Object, e As System.EventArgs) Handles LKBbackt.Click
        BackStep()
    End Sub

    Private Sub LKBconfirmb_Click(sender As Object, e As System.EventArgs) Handles LKBconfirmb.Click
        Import()
    End Sub

    Private Sub LKBconfirmt_Click(sender As Object, e As System.EventArgs) Handles LKBconfirmt.Click
        Import()
    End Sub


    Private Sub LKBundob_Click(sender As Object, e As System.EventArgs) Handles LKBundob.Click
        RaiseEvent Close()
    End Sub

    Private Sub LKBundot_Click(sender As Object, e As System.EventArgs) Handles LKBundot.Click
        RaiseEvent Close()
    End Sub
    'Private Sub UCorgSelector_SelectedOrgnChanged(OrgId As Integer, OrgnName As String) Handles UCorgSelector.SelectedOrgnChanged
    '    If (OrgId > 0) Then
    '        Me.TXBsourceName.Text = OrgnName
    '    End If
    'End Sub
    'Private Sub UCcomSelector_SelectedCommunityChanged(CommunityID As Integer) Handles UCcomSelector.SelectedCommunityChanged
    '    If (CommunityID > 0) Then
    '        Me.TXBsourceName.Text = Me.UCcomSelector.GetCommunityName(CommunityID)
    '    End If
    'End Sub
    'Private Sub RBLsource_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RBLsource.SelectedIndexChanged
    '    Me.LKBcomSelector.Visible = False
    '    Me.LKBorgnSelector.Visible = False
    '    Select Case RBLsource.SelectedValue
    '        Case "2"    'Organization
    '            Me.LKBorgnSelector.Visible = True
    '        Case "3"    'Community
    '            Me.LKBcomSelector.Visible = True
    '        Case Else       '0      Current
    '            '            1      Portal
    '            Me.PNLselOrgn.Visible = False
    '            Me.LKBorgnSelector.Text = Resource.getValue("LKBorgnSelector.text")
    '            Me.PNLselCom.Visible = False
    '            Me.LKBcomSelector.Text = Resource.getValue("LKBcomSelector.text")
    '    End Select
    'End Sub
    'Private Sub LKBorgnSelector_Click(sender As Object, e As System.EventArgs) Handles LKBorgnSelector.Click
    '    If (Me.PNLselOrgn.Visible) Then
    '        Me.PNLselOrgn.Visible = False
    '        Me.LKBorgnSelector.Text = Resource.getValue("LKBorgnSelector.text")
    '    Else
    '        Me.PNLselOrgn.Visible = True
    '        Me.LKBorgnSelector.Text = Resource.getValue("LKBorgnSelector.hide")
    '    End If
    'End Sub
    'Private Sub LKBcomSelector_Click(sender As Object, e As System.EventArgs) Handles LKBcomSelector.Click
    '    If (Me.PNLselCom.Visible) Then
    '        Me.PNLselCom.Visible = False
    '        Me.LKBcomSelector.Text = Resource.getValue("LKBcomSelector.text")
    '    Else
    '        Me.PNLselCom.Visible = True
    '        Me.LKBcomSelector.Text = Resource.getValue("LKBcomSelector.hide")
    '    End If
    'End Sub
    'Private Sub LKBnext_Click(sender As Object, e As System.EventArgs) Handles LKBnext.Click
    '    'Me.PNLmain.Visible = False
    '    'Me.PNLselOrgn.Visible = False
    '    'Me.PNLselCom.Visible = False
    '    'Me.PNLelSelector.Visible = True

    '    'Me.LKBnext.Visible = False
    '    'Me.LKBback.Visible = True
    '    'Me.LKBconfirm.Visible = True


    '    Me.NextStep()
    'End Sub
    'Private Sub LKBback_Click(sender As Object, e As System.EventArgs) Handles LKBback.Click
    '    'Me.PNLmain.Visible = True
    '    'Me.PNLselOrgn.Visible = False
    '    'Me.PNLselCom.Visible = False
    '    'Me.PNLelSelector.Visible = False

    '    'Me.LKBnext.Visible = True
    '    'Me.LKBback.Visible = False
    '    'Me.LKBconfirm.Visible = False
    '    Me.BackStep()
    'End Sub

    'Private Sub LKBundo_Click(sender As Object, e As System.EventArgs) Handles LKBundo.Click
    '    RaiseEvent Close()
    'End Sub
    Private Sub RPTfotElm_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTfotElm.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim Elm As ImportedElement = e.Item.DataItem

            Dim CbxSel As CheckBox = e.Item.FindControl("CbxSel")
            If Not IsNothing(CbxSel) Then
                CbxSel.Text = ""
                CbxSel.Checked = True
            End If

            Dim DDLnewPosition As DropDownList = e.Item.FindControl("DDLnewPosition")
            If Not IsNothing(DDLnewPosition) Then
                BindDDLPosition(DDLnewPosition, Elm.ElementPlacing)
            End If

            Dim LITpreview As Literal = e.Item.FindControl("LITpreview")
            If Not IsNothing("LITpreview") Then
                If TypeOf Elm.Element Is lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementImage Then
                    LITpreview.Text = _
                        GetImageHtml(DirectCast(Elm.Element,  _
                        lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementImage))
                ElseIf TypeOf Elm.Element Is lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementText Then
                    LITpreview.Text = _
                        GetTextHtml(DirectCast(Elm.Element,  _
                        lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementText))
                Else
                    LITpreview.Text = ""
                End If
            End If

            Dim HIDid As HiddenField = e.Item.FindControl("HIDid")
            If Not IsNothing(HIDid) Then
                HIDid.Value = Elm.Element.Id
            End If

        End If
    End Sub
#End Region

    <Serializable()> Public Class ImportedElement
        Public Element As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Element
        Public SourcePosition As lm.Comol.Core.DomainModel.DocTemplateVers.ElementPosition
        Public ElementPlacing As Placing
    End Class
    Public Enum Placing As Integer
        HeaderLeft = 10
        HeaderCenter = 11
        HeaderRight = 12

        'Body = 20

        FooterLeft = 30
        FooterCenter = 31
        FooterRight = 32

        Signature = 40

        none = 50
    End Enum

#Region "Navigation"
    Private Enum ImportStep As Integer
        SourceType = 0
        ComSelect = 10
        OrgnSelect = 11
        Element = 20
    End Enum

    Private Sub ShowByStep(ByVal CurStep As ImportStep)
        Me.PNLmain.Visible = False
        Me.PNLselCom.Visible = False
        Me.PNLselOrgn.Visible = False
        Me.PNLelSelector.Visible = False

        Me.LKBbackt.Visible = True
        Me.LKBbackb.Visible = True
        Me.LKBconfirmt.Visible = False
        Me.LKBconfirmb.Visible = False
        Me.LKBnextt.Visible = True
        Me.LKBnextb.Visible = True

        Select Case CurStep
            Case ImportStep.SourceType
                Me.PNLmain.Visible = True
                Me.LKBbackt.Visible = False
                Me.LKBbackb.Visible = False
                LITstepTitle_t.Text = Resource.getValue("Step.0.text")
                LITstepDescription_t.Text = Resource.getValue("Step.0.description")

            Case ImportStep.ComSelect
                Me.PNLselCom.Visible = True
                LITstepTitle_t.Text = Resource.getValue("Step.2.text")
                LITstepDescription_t.Text = Resource.getValue("Step.2.description")

            Case ImportStep.OrgnSelect
                Me.PNLselOrgn.Visible = True
                LITstepTitle_t.Text = Resource.getValue("Step.1.text")
                LITstepDescription_t.Text = Resource.getValue("Step.1.description")

            Case ImportStep.Element
                Me.PNLelSelector.Visible = True
                Me.LKBconfirmt.Visible = True
                Me.LKBconfirmb.Visible = True
                Me.LKBnextt.Visible = False
                Me.LKBnextb.Visible = False
                LITstepTitle_t.Text = Resource.getValue("Step.3.text")
                LITstepDescription_t.Text = Resource.getValue("Step.3.description")

        End Select
    End Sub

    Private Sub NextStep()
        If (Me.PNLmain.Visible) Then
            If Me.RBLsource.SelectedValue = "2" Then
                ShowByStep(ImportStep.OrgnSelect)
            ElseIf Me.RBLsource.SelectedValue = "3" Then
                ShowByStep(ImportStep.ComSelect)
            Else
                Me.Bindelements(True)
                ShowByStep(ImportStep.Element)
            End If

        Else
            Me.Bindelements(True)
            ShowByStep(ImportStep.Element)
        End If
    End Sub

    Private Sub BackStep()
        If Me.PNLelSelector.Visible Then
            If Me.RBLsource.SelectedValue = "2" Then
                ShowByStep(ImportStep.OrgnSelect)
            ElseIf Me.RBLsource.SelectedValue = "3" Then
                ShowByStep(ImportStep.ComSelect)
            Else
                ShowByStep(ImportStep.SourceType)
            End If

        Else
            ShowByStep(ImportStep.SourceType)
        End If
    End Sub
#End Region

    Private Sub UCorgSelector_SelectedOrgnChanged(OrgId As Integer, OrgnName As String) Handles UCorgSelector.SelectedOrgnChanged
        NextStep()
    End Sub

    Private Sub UCcomSelector_SelectedCommunityChanged(CommunityID As Integer) Handles UCcomSelector.SelectedCommunityChanged
        If CommunityID > 0 Then
            NextStep()
        End If
    End Sub

    ''' <summary>
    ''' Carico i dati nella radiobutton con i 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindRBLSource()
        Me.RBLsource.Items.Clear()

        Dim LiElement As ListItem

        Me.RBLsource.Items.Add(New ListItem(String.Format("<dl class=""options""><dt>{0}</dt><dd>{1}</dd></dl>", Resource.getValue("RBLsource.0.text"), Resource.getValue("RBLsource.0.description")), 0))
        Me.RBLsource.Items.Add(New ListItem(String.Format("<dl class=""options""><dt>{0}</dt><dd>{1}</dd></dl>", Resource.getValue("RBLsource.1.text"), Resource.getValue("RBLsource.1.description")), 1))
        Me.RBLsource.Items.Add(New ListItem(String.Format("<dl class=""options""><dt>{0}</dt><dd>{1}</dd></dl>", Resource.getValue("RBLsource.2.text"), Resource.getValue("RBLsource.2.description")), 2))
        Me.RBLsource.Items.Add(New ListItem(String.Format("<dl class=""options""><dt>{0}</dt><dd>{1}</dd></dl>", Resource.getValue("RBLsource.3.text"), Resource.getValue("RBLsource.3.description")), 3))



        ' <asp:ListItem Value="0" Text="#Current"></asp:ListItem>
        '<asp:ListItem Value="1" Text="#Portal"></asp:ListItem>
        '<asp:ListItem Value="2" Text="#Organization"></asp:ListItem>
        '<asp:ListItem Value="3" Text="#Community"></asp:ListItem>


        



        'New ListItem(String.Format("<dl><dt>{0}</dt><dd>{1}</dd></dl>", submitter.Name, submitter.Description), submitter.Id))
    End Sub


End Class