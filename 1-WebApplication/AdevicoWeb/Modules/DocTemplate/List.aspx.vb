Imports lm.Comol.Core.BaseModules.DocTemplate

Imports TemplateVers = lm.Comol.Core.DomainModel.DocTemplateVers

Public Class DocTemplateList
    Inherits PageBase
    Implements Presentation.IViewDocTemplateList


    'NOTE SVILUPPO
    ' Da rivedere:
    '   - Action
    '   - Permission a livello di master (gestiti internamente)

#Region "Context"
    Private _Presenter As Presentation.DocTemplateListPresenter
    Private ReadOnly Property CurrentPresenter() As Presentation.DocTemplateListPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New Presentation.DocTemplateListPresenter(Me.PageUtility.CurrentContext, Me)
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

    Public ReadOnly Property GetFooter As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter Implements lm.Comol.Core.BaseModules.DocTemplate.Presentation.IViewDocTemplateList.GetFooter
        Get
            Return Me.Master.getTemplateFooter()
        End Get
    End Property
    Public ReadOnly Property GetHeader As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter Implements lm.Comol.Core.BaseModules.DocTemplate.Presentation.IViewDocTemplateList.GetHeader
        Get
            Return Me.Master.getTemplateHeader("")
        End Get
    End Property

    Public ReadOnly Property TemplateBaseUrl As String Implements lm.Comol.Core.BaseModules.DocTemplate.Presentation.IViewDocTemplateList.TemplateBaseUrl
        Get
            Return Me.FullBaseUrl & MyBase.SystemSettings.DocTemplateSettings.BaseUrl
        End Get
    End Property

    Public ReadOnly Property Filter As TemplateVers.Domain.DTO.Management.TemplateFilter Implements lm.Comol.Core.BaseModules.DocTemplate.Presentation.IViewDocTemplateList.Filter
        Get
            Dim filtr As TemplateVers.Domain.DTO.Management.TemplateFilter = lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.TemplateFilter.ALL

            If (Not IsNothing(Request.Cookies("DocTemplate")) AndAlso Not IsNothing(Request.Cookies("DocTemplate")("Filter"))) Then
                Try
                    filtr = Request.Cookies("DocTemplate")("Filter")
                Catch ex As Exception

                End Try

            End If

            Return filtr
        End Get
    End Property
    Public ReadOnly Property OrderAscending As Boolean Implements lm.Comol.Core.BaseModules.DocTemplate.Presentation.IViewDocTemplateList.OrderAscending
        Get
            Dim Ascending As Boolean = True

            If (Not IsNothing(Request.Cookies("DocTemplate")) AndAlso Not IsNothing(Request.Cookies("DocTemplate")("OrderAscending"))) Then
                Try
                    Ascending = Request.Cookies("DocTemplate")("OrderAscending")
                Catch ex As Exception

                End Try

            End If
            Return Ascending
        End Get
    End Property
    Public ReadOnly Property OrderBy As TemplateVers.Domain.DTO.Management.TemplateOrderCol Implements lm.Comol.Core.BaseModules.DocTemplate.Presentation.IViewDocTemplateList.OrderBy
        Get
            Dim value As Integer = 1
            If (Not IsNothing(Request.Cookies("DocTemplate")) AndAlso Not IsNothing(Request.Cookies("DocTemplate")("OrderColumn"))) Then
                Try
                    value = Request.Cookies("DocTemplate")("OrderColumn")
                Catch ex As Exception
                End Try
            End If

            Dim OrderCol As TemplateVers.Domain.DTO.Management.TemplateOrderCol = lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.TemplateOrderCol.Name

            Try
                OrderCol = DirectCast(value, TemplateVers.Domain.DTO.Management.TemplateOrderCol)
            Catch ex As Exception

            End Try

            Return OrderCol
        End Get
    End Property
#End Region

#Region "Internal"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="IsDraft"></param>
    ''' <param name="IsActive"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Rivedere Stati e STILI (Nomi classi!)
    ''' </remarks>
    Public ReadOnly Property GetVersionCssClass(ByVal Version As TemplateVers.Domain.DTO.Management.DTO_ListTemplateVersion) As String
        Get
            If (Not IsNothing(Version)) Then
                If (Version.IsDraft) Then
                    Return "Draft"
                ElseIf (Not Version.IsActive) Then
                    Return "Deprecated"
                Else
                    Return "Active"
                End If
            End If
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="HasDraft"></param>
    ''' <param name="HasActive"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Rivedere Stati e STILI (Nomi classi!)
    ''' </remarks>
    Public ReadOnly Property GetTemplateCssClass(ByVal Templ As TemplateVers.Domain.DTO.Management.DTO_ListTemplate) As String
        Get
            Dim CSSClass As String = "template"

            If Not IsNothing(Templ) Then
                If (Templ.HasDraft) Then
                    CSSClass &= " HasDraft"
                End If

                If (Templ.HasActive) Then
                    CSSClass &= " HasActive"
                End If

            End If

            Return CSSClass
        End Get
    End Property

    Public ReadOnly Property GetTemplateStatusCssClass(ByVal Templ As TemplateVers.Domain.DTO.Management.DTO_ListTemplate) As String
        Get
            Dim CSSClass As String = "template"

            If Not IsNothing(Templ) Then
                If (Templ.HasDraft) Then
                    CSSClass &= " HasDraft"
                End If

                If (Templ.HasActive) Then
                    CSSClass &= " HasActive"
                End If

            End If

            Return CSSClass
        End Get
    End Property

    Public ReadOnly Property GetVersionStatusCssClass(ByVal Vers As TemplateVers.Domain.DTO.Management.DTO_ListTemplateVersion) As String
        Get
            Dim CSSClass As String = ""


            If Not IsNothing(Vers) Then
                If (Vers.IsDraft) Then
                    CSSClass &= " yellow"
                Else
                    If (Vers.IsActive) Then
                        CSSClass &= " green"
                    Else
                        CSSClass &= " red"
                    End If
                End If

            End If

            Return CSSClass
        End Get
    End Property

    Protected ReadOnly Property CookieName() As String
        Get
            Return "COMOL_PreviewTemplate"
        End Get
    End Property
    Protected ReadOnly Property DisplayMessageToken() As String
        Get
            ' TRADUZIONE DEL MESSAGGIO DA VISUALIZZARE
            Return Resource.getValue("DisplayToken.Message")
        End Get
    End Property
    Protected ReadOnly Property DisplayTitleToken() As String
        Get
            ' TRADUZIONE DEL MESSAGGIO DA VISUALIZZARE
            Return Resource.getValue("DisplayToken.Title")
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Me.Master.ShowDocType = True
        Me.LBLnoItems.Visible = False 'La nascondo, essendo utilizzata per mostrare vari messaggi...
    End Sub

#Region "Inherits"

    Public Overrides Sub BindDati()
        'If Not Page.IsPostBack Then
        '    SetDDLType()
        'End If

        Me.CurrentPresenter.InitView()
    End Sub

    Public Overrides Sub BindNoPermessi()
        ShowMessageToPage(Resource.getValue("Error.NoPermessi"))
        Me.RPTtemplate.Visible = False
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_templateList", "Modules", "DocTemplates")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        Me.HYPaddNew.Text = Resource.getValue("HYPaddNew.text")

        With Resource
            .setRadioButtonList(RBLtype, "-1")
            .setRadioButtonList(RBLtype, "1")
            .setRadioButtonList(RBLtype, "2")
            .setRadioButtonList(RBLtype, "3")

            '.setCheckBox(CBXadvEdit)

            .setLabel(LBLnoItems)

            .setLiteral(LTtitle_t)

            .setLabel(lblFilter_T)

        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)
        Me.LBLnoItems.Visible = True
        Me.LBLnoItems.Text = errorMessage
    End Sub

#End Region

#Region "Implements"

    Public Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements lm.Comol.Core.BaseModules.DocTemplate.Presentation.IViewDocTemplateList.DisplayNoPermission
        BindNoPermessi()
    End Sub

    Public Sub DisplaySessionTimeout() Implements lm.Comol.Core.BaseModules.DocTemplate.Presentation.IViewDocTemplateList.DisplaySessionTimeout
        Dim idCommunity As Integer = 0
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = lm.Comol.Core.DomainModel.DocTemplateVers.rootObject.List()
        'If idCommunity > 0 Then
        '    dto.IdCommunity = idCommunity
        'End If
        webPost.Redirect(dto)

        'ShowMessageToPage(Resource.getValue("Error.SessionTimeout"))
    End Sub

    Public Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As TemplateVers.ModuleDocTemplate.ActionType) Implements Presentation.IViewDocTemplateList.SendUserAction

    End Sub

    Public Sub SendUserAction(idCommunity As Integer, idModule As Integer, idTemplate As Long, action As TemplateVers.ModuleDocTemplate.ActionType) Implements Presentation.IViewDocTemplateList.SendUserAction

    End Sub
    Public Sub LoadTemplates(templates As System.Collections.Generic.IList(Of TemplateVers.Domain.DTO.Management.DTO_ListTemplate)) Implements Presentation.IViewDocTemplateList.LoadTemplates

        Me.SetDDLType(Me.Filter)

        Me.HYPaddNew.NavigateUrl = Me.GetAddUrl()

        If (Not IsNothing(templates) AndAlso templates.Count() > 0) Then
            Me.RPTtemplate.Visible = True
            Me.LBLnoItems.Visible = False
            Me.RPTtemplate.DataSource = templates
            Me.RPTtemplate.DataBind()
        Else
            Resource.setLabel(LBLnoItems)
            Me.LBLnoItems.Visible = True
            Me.RPTtemplate.Visible = False
        End If

    End Sub
#End Region

#Region "Internal"


    Private Function GetAddUrl(Optional ByVal IdTemplate As Int64 = 0, Optional ByVal IdVersion As Int64 = 0)
        Dim Url As String = "./Add.aspx"

        If (IdTemplate > 0) Then
            Url &= "?idTemplate=" & IdTemplate.ToString()
            If (IdVersion > 0) Then
                Url &= "&idVersion=" & IdVersion.ToString()
            End If
        End If

        Return Url
    End Function

    Private Function GetEditUrl(Optional ByVal IdTemplate As Int64 = 0, Optional ByVal IdVersion As Int64 = 0)
        Dim Url As String = "./Edit.aspx"

        If (IdTemplate > 0) Then
            Url &= "?idTemplate=" & IdTemplate.ToString()
            If (IdVersion > 0) Then
                Url &= "&idVersion=" & IdVersion.ToString()
            End If
        End If

        Return Url
    End Function

    Private Function GetEditSkinUrl(Optional ByVal IdTemplate As Int64 = 0, Optional ByVal IdVersion As Int64 = 0)
        Dim Url As String = "./EditSkin.aspx"

        If (IdTemplate > 0) Then
            Url &= "?idTemplate=" & IdTemplate.ToString()
            If (IdVersion > 0) Then
                Url &= "&idVersion=" & IdVersion.ToString()
            End If
        End If

        Return Url
    End Function

    Private Sub SetFilter(ByVal Filter As TemplateVers.Domain.DTO.Management.TemplateFilter)
        If (IsNothing(Filter)) Then
            Filter = lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.TemplateFilter.ALL
        End If

        Response.Cookies("DocTemplate")("Filter") = Filter
        Response.Cookies("DocTemplate").Expires = DateTime.Now.AddDays(15)


        'Me.CurrentPresenter.InitView()
    End Sub

    Private Sub SetOrderAscending(ByVal OrderASC As Boolean)
        If (IsNothing(OrderASC)) Then
            OrderASC = True
        End If
        Response.Cookies("DocTemplate")("OrderAscending") = OrderASC
        Response.Cookies("DocTemplate").Expires = DateTime.Now.AddDays(15)
        'Me.ViewState("OrderByAsc") = OrderASC
        'Me.CurrentPresenter.InitView()
    End Sub

    Private Sub SetOrderBy(ByVal OrderCol As TemplateVers.Domain.DTO.Management.TemplateOrderCol)
        If IsNothing(OrderCol) Then
            OrderCol = lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.TemplateOrderCol.Name
        End If
        Dim value As Integer = 1
        Try
            value = DirectCast(OrderCol, Integer)
        Catch ex As Exception
            value = 1
        End Try
        Response.Cookies("DocTemplate")("OrderColumn") = value
        Response.Cookies("DocTemplate").Expires = DateTime.Now.AddDays(15)
        'Me.ViewState("OrderColumn") = OrderASC
        'Me.CurrentPresenter.InitView()
    End Sub



#End Region

#Region "Handler"

    Private Sub RPTtemplate_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTtemplate.ItemDataBound

        '      H E A D E R

        If e.Item.ItemType = ListItemType.Header Then
            Dim LTpartecipant_t, LTassService_t, LTstatus_t, LTsavedOn_t, LTaction_t As Literal

            LTpartecipant_t = e.Item.FindControl("LTpartecipant_t")
            If Not IsNothing(LTpartecipant_t) Then
                LTpartecipant_t.Text = Resource.getValue("LTpartecipant_t.text")
            End If

            LTassService_t = e.Item.FindControl("LTassService_t")
            If Not IsNothing(LTassService_t) Then
                LTassService_t.Text = Resource.getValue("LTassService_t.text")
            End If

            LTstatus_t = e.Item.FindControl("LTstatus_t")
            If Not IsNothing(LTstatus_t) Then
                LTstatus_t.Text = Resource.getValue("LTstatus_t.text")
            End If

            LTsavedOn_t = e.Item.FindControl("LTsavedOn_t")
            If Not IsNothing(LTsavedOn_t) Then
                LTsavedOn_t.Text = Resource.getValue("LTsavedOn_t.text")
            End If

            LTaction_t = e.Item.FindControl("LTaction_t")
            If Not IsNothing(LTaction_t) Then
                LTaction_t.Text = Resource.getValue("LTaction_t.text")
            End If

            'Ordering
            Dim LNBpartecipantOrderUp, LNBpartecipantOrderDown, LNBsavedOnOrderUp, LNBsavedOnOrderDown As LinkButton
            LNBpartecipantOrderUp = e.Item.FindControl("LNBpartecipantOrderUp")
            If Not IsNothing(LNBpartecipantOrderUp) Then
                LNBpartecipantOrderUp.CommandName = "OrderBy.NameUp"
            End If

            LNBpartecipantOrderDown = e.Item.FindControl("LNBpartecipantOrderDown")
            If Not IsNothing(LNBpartecipantOrderDown) Then
                LNBpartecipantOrderDown.CommandName = "OrderBy.NameDown"
            End If

            LNBsavedOnOrderUp = e.Item.FindControl("LNBsavedOnOrderUp")
            If Not IsNothing(LNBsavedOnOrderUp) Then
                LNBsavedOnOrderUp.CommandName = "OrderBy.SaveOnUp"
            End If

            LNBsavedOnOrderDown = e.Item.FindControl("LNBsavedOnOrderDown")
            If Not IsNothing(LNBsavedOnOrderDown) Then
                LNBsavedOnOrderDown.CommandName = "OrderBy.SaveOnDown"
            End If




            '      I T E M S     (Template)
        ElseIf (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim template As TemplateVers.Domain.DTO.Management.DTO_ListTemplate = DirectCast(e.Item.DataItem, TemplateVers.Domain.DTO.Management.DTO_ListTemplate)

            If Not IsNothing(template) Then


                Dim CommandArgument As String = template.Id.ToString() & "_0"

                Dim LTtemplateName, LTtemplateServices, LTtemplateUpdateOn, LTstatus As Literal

                LTtemplateName = e.Item.FindControl("LTtemplateName")
                If Not IsNothing(LTtemplateName) Then
                    LTtemplateName.Text = template.Name
                End If


                LTtemplateServices = e.Item.FindControl("LTtemplateServices")
                If Not IsNothing(LTtemplateServices) Then


                    If Not IsNothing(template.Services) OrElse template.Services.Count() > 0 Then
                        LTtemplateServices.Text = ""
                        Dim IsFirst As Boolean = True
                        For Each serv As TemplateVers.Domain.DTO.Management.DTO_ListService In template.Services
                            If IsFirst Then
                                LTtemplateServices.Text &= serv.Name
                                IsFirst = False
                            Else
                                LTtemplateServices.Text &= " , " & serv.Name
                            End If
                        Next
                    Else
                        LTtemplateServices.Text = Resource.getValue("TempalteServices.System")
                    End If
                End If

                LTtemplateUpdateOn = e.Item.FindControl("LTtemplateUpdateOn")
                If Not IsNothing(LTtemplateUpdateOn) Then
                    LTtemplateUpdateOn.Text = template.LastModify.ToString()
                End If

                'LTstatus = e.Item.FindControl("LTstatus")
                'If Not IsNothing(LTstatus) Then
                '    If (template.HasActive AndAlso template.HasDefinitive) Then
                '        LTstatus.Text = Resource.getValue("Tempalte.Status.IsActive")
                '    ElseIf (template.HasDefinitive) Then
                '        LTstatus.Text = Resource.getValue("Tempalte.Status.IsDefinitive")
                '    ElseIf (template.HasDraft) Then
                '        LTstatus.Text = Resource.getValue("Tempalte.Status.IsDraft")
                '    Else
                '        LTstatus.Text = Resource.getValue("Tempalte.Status.none")
                '    End If

                'End If

                Dim LNBtemplatePDF, LNBtemplateRTF As LinkButton

                LNBtemplatePDF = e.Item.FindControl("LNBtemplatePDF")

                If Not IsNothing(LNBtemplatePDF) Then
                    Resource.setLinkButton(LNBtemplatePDF, True, True, False, False)
                    LNBtemplatePDF.CommandName = "Template.PDF"
                    LNBtemplatePDF.CommandArgument = CommandArgument
                    'AddHandler LNBtemplateExpPDF.Click, AddressOf LKBexport_Click
                End If

                LNBtemplateRTF = e.Item.FindControl("LNBtemplateRTF")
                If Not IsNothing(LNBtemplateRTF) Then
                    Resource.setLinkButton(LNBtemplateRTF, True, True, False, False)
                    LNBtemplateRTF.CommandName = "Template.RTF"
                    LNBtemplateRTF.CommandArgument = CommandArgument
                    'AddHandler LNBtemplateExpRTF.Click, AddressOf LKBexport_Click
                End If


                'Other commands (view)
                Dim HYPtemplatePreview As HyperLink
                HYPtemplatePreview = e.Item.FindControl("HYPtemplatePreview")
                If Not IsNothing(HYPtemplatePreview) Then
                    Resource.setHyperLink(HYPtemplatePreview, True, True, False, False)
                    HYPtemplatePreview.NavigateUrl = BaseUrl & lm.Comol.Core.DomainModel.DocTemplateVers.rootObject.PreviewTemplate(template.Id, True)
                End If


                Dim LNBtemplateDeleteLogical, LNBtemplateDeletePhisical, LNBtemplateAddVers, LNBtemplateRecover As LinkButton 'LNBtemplateCopy,
                Dim HypTemplateCopy As HyperLink

                LNBtemplateAddVers = e.Item.FindControl("LNBtemplateAddVers")
                If Not IsNothing(LNBtemplateAddVers) Then
                    If (template.Permissions.AllowNewVersion) Then
                        Resource.setLinkButton(LNBtemplateAddVers, True, True, False, False)
                        LNBtemplateAddVers.Visible = True
                        LNBtemplateAddVers.CommandName = "Template.AddVers"
                        LNBtemplateAddVers.CommandArgument = CommandArgument
                    Else
                        LNBtemplateAddVers.Visible = False
                    End If
                End If

                HypTemplateCopy = e.Item.FindControl("HypTemplateCopy")

                If Not IsNothing(HypTemplateCopy) Then
                    Resource.setHyperLink(HypTemplateCopy, True, True, False, False)
                    HypTemplateCopy.NavigateUrl = Me.GetAddUrl(template.Id)
                End If

                LNBtemplateDeleteLogical = e.Item.FindControl("LNBtemplateDeleteLogical")
                If Not IsNothing(LNBtemplateDeleteLogical) Then
                    If (template.Permissions.DeleteVirtual) Then
                        Resource.setLinkButton(LNBtemplateDeleteLogical, True, True, False, True)
                        LNBtemplateDeleteLogical.Visible = True
                        LNBtemplateDeleteLogical.CommandName = "Template.DelVirt"
                        LNBtemplateDeleteLogical.CommandArgument = CommandArgument
                    Else
                        LNBtemplateDeleteLogical.Visible = False
                    End If
                End If

                LNBtemplateDeletePhisical = e.Item.FindControl("LNBtemplateDeletePhisical")
                If Not IsNothing(LNBtemplateDeletePhisical) Then
                    If (template.Permissions.DeletePhisical) Then
                        Resource.setLinkButton(LNBtemplateDeletePhisical, True, True, False, True)
                        LNBtemplateDeletePhisical.Visible = True
                        LNBtemplateDeletePhisical.CommandName = "Template.DelPhis"
                        LNBtemplateDeletePhisical.CommandArgument = CommandArgument
                    Else
                        LNBtemplateDeletePhisical.Visible = False
                    End If
                End If

                LNBtemplateRecover = e.Item.FindControl("LNBtemplateRecover")
                If Not IsNothing(LNBtemplateRecover) Then
                    If (template.Permissions.UndeleteVirtual) Then
                        Resource.setLinkButton(LNBtemplateRecover, True, True, False, False)
                        LNBtemplateRecover.Visible = True
                        LNBtemplateRecover.CommandName = "Template.Recover"
                        LNBtemplateRecover.CommandArgument = CommandArgument
                    Else
                        LNBtemplateRecover.Visible = False
                    End If
                End If



                'Abilita/disabilita
                Dim LNBtemplateEnable, LNBtemplateDisable As LinkButton
                LNBtemplateEnable = e.Item.FindControl("LNBtemplateEnable")
                If Not IsNothing(LNBtemplateEnable) Then
                    If (template.Permissions.Activate) Then
                        Resource.setLinkButton(LNBtemplateEnable, True, True, False, False)
                        LNBtemplateEnable.Visible = True
                        LNBtemplateEnable.CommandName = "Template.Enable"
                        LNBtemplateEnable.CommandArgument = CommandArgument
                    Else
                        LNBtemplateEnable.Visible = False
                    End If
                End If

                LNBtemplateDisable = e.Item.FindControl("LNBtemplateDisable")
                If Not IsNothing(LNBtemplateDisable) Then
                    If (template.Permissions.DeActivate) Then
                        Resource.setLinkButton(LNBtemplateDisable, True, True, False, True)
                        LNBtemplateDisable.Visible = True
                        LNBtemplateDisable.CommandName = "Template.Disable"
                        LNBtemplateDisable.CommandArgument = CommandArgument
                    Else
                        LNBtemplateDisable.Visible = False
                    End If
                End If
            End If

            If Not IsNothing(template.TemplateVersions) Then

                '       S u b - I T E M S           (Verions)
                Dim RPTversions As Repeater = e.Item.FindControl("RPTversions")

                If Not IsNothing(RPTversions) Then
                    RPTversions.DataSource = template.TemplateVersions
                    AddHandler RPTversions.ItemDataBound, AddressOf RPTversions_ItemDataBound
                    'AddHandler RPTversions.ItemCommand, AddressOf RPTversions_ItemCommand
                    RPTversions.DataBind()

                End If
            End If

        End If

    End Sub

    Public Sub RPT_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs)
        Dim TemplateId As Int64 = 0
        Dim VersionId As Int64 = 0

        Dim Ids() As String = e.CommandArgument.ToString().Split("_")

        If Not IsNothing(Ids) Then
            Try
                TemplateId = System.Convert.ToInt64(Ids(0))
                VersionId = System.Convert.ToInt64(Ids(1))
            Catch ex As Exception

            End Try
        End If

        Select Case e.CommandName

            'TEMPLATE 
            Case "Template.PDF"
                Me.CurrentPresenter.ExportPDF(TemplateId, VersionId, Response, New HttpCookie(CookieName, HDNdownloadTokenValue.Value))
            Case "Template.RTF"
                Me.CurrentPresenter.ExportRTF(TemplateId, VersionId, Response, New HttpCookie(CookieName, HDNdownloadTokenValue.Value))
            Case "Template.AddVers"

                'Case "Template.Copy"
            Case "Template.DelVirt"
                Me.CurrentPresenter.TemplateVirtualDelete(TemplateId)
            Case "Template.DelPhis"
                Me.CurrentPresenter.TemplatePhysicalDelete(TemplateId)
            Case "Template.Recover"
                Me.CurrentPresenter.TemplateRecover(TemplateId)
            Case "Template.Enable"
                Me.CurrentPresenter.TemplateEnable(TemplateId)
            Case "Template.Disable"
                Me.CurrentPresenter.TemplateDisable(TemplateId)

                'VERSION
            Case "Version.PDF"
                Me.CurrentPresenter.ExportPDF(TemplateId, VersionId, Response, New HttpCookie(CookieName, HDNdownloadTokenValue.Value))
            Case "Version.RTF"
                Me.CurrentPresenter.ExportRTF(TemplateId, VersionId, Response, New HttpCookie(CookieName, HDNdownloadTokenValue.Value))
            Case "Version.DelVirt"
                Me.CurrentPresenter.VersionVirtuallDelete(TemplateId, VersionId)
            Case "Version.DelPhis"
                Me.CurrentPresenter.VersionPhysicalDelete(TemplateId, VersionId)
            Case "Version.Recover"
                Me.CurrentPresenter.VersionRecover(TemplateId, VersionId)
            Case "Version.Copy"
                Me.CurrentPresenter.VersionCopy(TemplateId, VersionId)
            Case "Version.Enable"
                Me.CurrentPresenter.VersionEnable(TemplateId, VersionId)
            Case "Version.Disable"
                Me.CurrentPresenter.VersionDisable(TemplateId, VersionId)

                'Filter
            Case "OrderBy.NameUp"
                Me.SetOrderBy(lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.TemplateOrderCol.Name)
                Me.SetOrderAscending(True)
                Me.CurrentPresenter.InitView()
            Case "OrderBy.NameDown"
                Me.SetOrderBy(lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.TemplateOrderCol.Name)
                Me.SetOrderAscending(False)
                Me.CurrentPresenter.InitView()
            Case "OrderBy.SaveOnUp"
                Me.SetOrderBy(lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.TemplateOrderCol.UpdatedOn)
                Me.SetOrderAscending(True)
                Me.CurrentPresenter.InitView()
            Case "OrderBy.SaveOnDown"
                Me.SetOrderBy(lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.TemplateOrderCol.UpdatedOn)
                Me.SetOrderAscending(False)
                Me.CurrentPresenter.InitView()
        End Select

    End Sub

    Private Sub RPTversions_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim version As TemplateVers.Domain.DTO.Management.DTO_ListTemplateVersion = DirectCast(e.Item.DataItem, TemplateVers.Domain.DTO.Management.DTO_ListTemplateVersion)


            If Not IsNothing(version) Then

                Dim CommandArgument As String = version.Template.Id.ToString() & "_" & version.Id.ToString()

                Dim LTversion, LTversionBy, LITversionStatus, LITversionUpdateOn As Literal

                LTversion = e.Item.FindControl("LTversion")
                If Not IsNothing(LTversion) Then
                    LTversion.Text = Resource.getValue("Version.Name").Replace("#Version#", version.Version.ToString())
                End If

                LTversionBy = e.Item.FindControl("LTversionBy")
                If Not IsNothing(LTversionBy) Then
                    LTversionBy.Text = version.LastModifiedBy
                End If

                LITversionStatus = e.Item.FindControl("LITversionStatus")
                If Not IsNothing(LITversionStatus) Then
                    If (version.IsDraft) Then
                        LITversionStatus.Text = Resource.getValue("Version.Status.Draft")
                    ElseIf (version.IsActive) Then
                        LITversionStatus.Text = Resource.getValue("Version.Status.Active")
                    Else
                        LITversionStatus.Text = Resource.getValue("Version.Status.None")
                    End If

                End If

                LITversionUpdateOn = e.Item.FindControl("LITversionUpdateOn")
                If Not IsNothing(LITversionUpdateOn) Then
                    LITversionUpdateOn.Text = version.LastModify.ToString()
                End If

                'EXPORT
                Dim LNBversionPDF, LNBversionRTF As LinkButton

                LNBversionPDF = e.Item.FindControl("LNBversionPDF")
                If Not IsNothing(LNBversionPDF) Then
                    LNBversionPDF.CommandName = "Version.PDF"
                    LNBversionPDF.CommandArgument = CommandArgument
                End If

                LNBversionRTF = e.Item.FindControl("LNBversionRTF")
                If Not IsNothing(LNBversionRTF) Then
                    LNBversionRTF.CommandName = "Version.RTF"
                    LNBversionRTF.CommandArgument = CommandArgument
                End If


                'Preview
                Dim HYPversionPreview As HyperLink
                HYPversionPreview = e.Item.FindControl("HYPversionPreview")
                If Not IsNothing(HYPversionPreview) Then
                    HYPversionPreview.NavigateUrl = BaseUrl & lm.Comol.Core.DomainModel.DocTemplateVers.rootObject.PreviewTemplate(version.Template.Id, version.Id, True)
                    'HYPversionPreview.Target = "_blank"
                End If

                'Commands

                Dim LNBversionDeleteLogical, LNBversionDeletePhisical, LNBversionRecover, LNBversionCopy As LinkButton ', LNBversionEdit

                LNBversionDeleteLogical = e.Item.FindControl("LNBversionDeleteLogical")
                If Not IsNothing(LNBversionDeleteLogical) Then
                    If (version.Permissions.DeleteVirtual) Then
                        Me.Resource.setLinkButton(LNBversionDeleteLogical, True, True, False, True)
                        LNBversionDeleteLogical.CommandName = "Version.DelVirt"
                        LNBversionDeleteLogical.CommandArgument = CommandArgument
                        LNBversionDeleteLogical.Visible = True
                    Else
                        LNBversionDeleteLogical.Visible = False
                    End If
                End If

                LNBversionDeletePhisical = e.Item.FindControl("LNBversionDeletePhisical")
                If Not IsNothing(LNBversionDeletePhisical) Then
                    If (version.Permissions.DeletePhisical) Then
                        Me.Resource.setLinkButton(LNBversionDeletePhisical, True, True, False, True)
                        LNBversionDeletePhisical.CommandName = "Version.DelPhis"
                        LNBversionDeletePhisical.CommandArgument = CommandArgument
                        LNBversionDeletePhisical.Visible = True
                    Else
                        LNBversionDeletePhisical.Visible = False
                    End If
                End If

                LNBversionRecover = e.Item.FindControl("LNBversionRecover")
                If Not IsNothing(LNBversionRecover) Then
                    If (version.Permissions.UndeleteVirtual) Then
                        Me.Resource.setLinkButton(LNBversionRecover, True, True, False, False)
                        LNBversionRecover.CommandName = "Version.Recover"
                        LNBversionRecover.CommandArgument = CommandArgument
                        LNBversionRecover.Visible = True
                    Else
                        LNBversionRecover.Visible = False
                    End If
                End If

                LNBversionCopy = e.Item.FindControl("LNBversionCopy")
                If Not IsNothing(LNBversionCopy) Then
                    If (version.Permissions.Copy) Then
                        LNBversionCopy.CommandName = "Version.Copy"
                        LNBversionCopy.CommandArgument = CommandArgument
                        LNBversionCopy.Visible = True
                    Else
                        LNBversionCopy.Visible = False
                    End If
                End If

                'HYPversionEdit
                Dim HYPversionEdit As HyperLink = e.Item.FindControl("HYPversionEdit")
                If Not IsNothing(HYPversionEdit) Then
                    If (version.Permissions.Edit AndAlso version.Deleted = lm.Comol.Core.DomainModel.BaseStatusDeleted.None) Then
                        Select Case version.Template.Type
                            Case lm.Comol.Core.DomainModel.DocTemplateVers.TemplateType.Standard
                                HYPversionEdit.NavigateUrl = GetEditUrl(version.Template.Id, version.Id)
                            Case lm.Comol.Core.DomainModel.DocTemplateVers.TemplateType.System
                                HYPversionEdit.NavigateUrl = GetEditUrl(version.Template.Id, version.Id)
                            Case lm.Comol.Core.DomainModel.DocTemplateVers.TemplateType.Skin
                                HYPversionEdit.NavigateUrl = GetEditSkinUrl(version.Template.Id, version.Id)
                        End Select

                        HYPversionEdit.Visible = True
                    Else
                        HYPversionEdit.Visible = False
                    End If
                End If

                Dim LNBversionEnable, LNBversionDisable As LinkButton
                LNBversionEnable = e.Item.FindControl("LNBversionEnable")
                If Not IsNothing(LNBversionEnable) Then
                    If (version.Permissions.Activate) Then
                        Resource.setLinkButton(LNBversionEnable, True, True, False, True)
                        LNBversionEnable.CommandName = "Version.Enable"
                        LNBversionEnable.CommandArgument = CommandArgument
                        LNBversionEnable.Visible = True
                    Else
                        LNBversionEnable.Visible = False
                    End If
                End If

                LNBversionDisable = e.Item.FindControl("LNBversionDisable")
                If Not IsNothing(LNBversionDisable) Then
                    If (version.Permissions.DeActivate) Then
                        Resource.setLinkButton(LNBversionDisable, True, True, False, True)
                        LNBversionDisable.CommandName = "Version.Disable"
                        LNBversionDisable.CommandArgument = CommandArgument
                        LNBversionDisable.Visible = True
                    Else
                        LNBversionDisable.Visible = False
                    End If
                End If
            End If

        End If
    End Sub

    Private Sub RBLtype_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RBLtype.SelectedIndexChanged
        Select Case RBLtype.SelectedValue
            Case "-1" 'Tutti
                Me.SetFilter(TemplateVers.Domain.DTO.Management.TemplateFilter.ALL)
            Case "1"  'Definitivi
                Me.SetFilter(TemplateVers.Domain.DTO.Management.TemplateFilter.Definitive)
            Case "2"  'In bozza
                Me.SetFilter(TemplateVers.Domain.DTO.Management.TemplateFilter.Draft)
            Case "3"  'Deleted
                Me.SetFilter(TemplateVers.Domain.DTO.Management.TemplateFilter.Deleted)
        End Select

        Me.CurrentPresenter.InitView()
    End Sub

    Private Sub SetDDLType(ByVal filter As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.TemplateFilter)
        Select Case filter
            Case lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.TemplateFilter.Definitive
                Me.RBLtype.SelectedValue = "1"
            Case lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.TemplateFilter.Deleted
                Me.RBLtype.SelectedValue = "3"
            Case lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.TemplateFilter.Draft
                Me.RBLtype.SelectedValue = "2"
            Case Else
                Me.RBLtype.SelectedValue = "-1"
        End Select
    End Sub
#End Region

    Public ReadOnly Property TemplateBasePath As String Implements lm.Comol.Core.BaseModules.DocTemplate.Presentation.IViewDocTemplateList.TemplateBasePath
        Get
            Return (MyBase.SystemSettings.DocTemplateSettings.BasePath).Replace("/", "\").Replace("\\", "\")
        End Get
    End Property

    Public ReadOnly Property TemplateTempPath As String Implements lm.Comol.Core.BaseModules.DocTemplate.Presentation.IViewDocTemplateList.TemplateTempPath
        Get
            If Not MyBase.SystemSettings.DocTemplateSettings.BasePath.EndsWith("\") Then
                MyBase.SystemSettings.DocTemplateSettings.BasePath &= "\"
            End If

            Return (MyBase.SystemSettings.DocTemplateSettings.BasePath & TemplateTempPath & MyBase.SystemSettings.DocTemplateSettings.BaseTemporaryFolder).Replace("/", "\").Replace("\\", "\")
        End Get
    End Property
End Class