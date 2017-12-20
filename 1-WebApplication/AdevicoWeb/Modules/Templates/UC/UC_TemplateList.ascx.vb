Imports lm.Comol.Core.BaseModules.TemplateMessages.Presentation
Imports lm.Comol.Core.TemplateMessages.Domain
Imports lm.ActionDataContract
Imports System.Linq

Public Class UC_TemplatesList
    Inherits BaseControl
    Implements lm.Comol.Core.BaseModules.TemplateMessages.Presentation.IViewCommonTemplateList

#Region "Context"
    Private _Presenter As CommonListPresenter
    Private ReadOnly Property CurrentPresenter() As CommonListPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CommonListPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property AllowAdd As Boolean Implements IViewCommonTemplateList.AllowAdd
        Get
            Return ViewStateOrDefault("AllowAdd", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowAdd") = value
        End Set
    End Property
    Private Property AvailableTypes As List(Of lm.Comol.Core.TemplateMessages.Domain.TemplateType) Implements IViewCommonTemplateList.AvailableTypes
        Get
            Return ViewStateOrDefault("AvailableTypes", New List(Of lm.Comol.Core.TemplateMessages.Domain.TemplateType))
        End Get
        Set(value As List(Of lm.Comol.Core.TemplateMessages.Domain.TemplateType))
            ViewState("AvailableTypes") = value
        End Set
    End Property
    Private Property AvailableDisplay As List(Of TemplateDisplay) Implements IViewCommonTemplateList.AvailableDisplay
        Get
            Return ViewStateOrDefault("AvailableDisplay", New List(Of TemplateDisplay))
        End Get
        Set(value As List(Of TemplateDisplay))
            ViewState("AvailableDisplay") = value
        End Set
    End Property
    Public Property CurrentFilters As dtoBaseFilters Implements IViewCommonTemplateList.CurrentFilters
        Get
            Dim filters As dtoBaseFilters = ViewState("CurrentFilters")
            filters.OrderBy = CurrentOrderBy
            filters.Ascending = CurrentAscending
            filters.TemplateDisplay = CurrentDisplay
            filters.TemplateType = CurrentType
            filters.Status = TemplateStatus.Active
            filters.SearchForName = Me.TXBtemplateName.Text

            Return filters
        End Get
        Set(value As dtoBaseFilters)
            ViewState("CurrentFilters") = value
            Me.TXBtemplateName.Text = value.SearchForName

        End Set
    End Property
    Private Property CurrentType As lm.Comol.Core.TemplateMessages.Domain.TemplateType Implements IViewCommonTemplateList.CurrentType
        Get
            Return ViewStateOrDefault("CurrentType", lm.Comol.Core.TemplateMessages.Domain.TemplateType.User)
        End Get
        Set(value As lm.Comol.Core.TemplateMessages.Domain.TemplateType)
            ViewState("CurrentType") = value
        End Set
    End Property
    Private Property Pager As lm.Comol.Core.DomainModel.PagerBase Implements IViewCommonTemplateList.Pager
        Get
            Return ViewStateOrDefault("Pager", New lm.Comol.Core.DomainModel.PagerBase)
        End Get
        Set(ByVal value As lm.Comol.Core.DomainModel.PagerBase)
            Me.ViewState("Pager") = value
            Me.PGgridBottom.Pager = value
            Me.DVpagerBottom.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
        End Set
    End Property
    Public Property RaiseApplyFiltersEvent As Boolean Implements IViewCommonTemplateList.RaiseApplyFiltersEvent
        Get
            Return ViewStateOrDefault("RaiseApplyFiltersEvent", False)
        End Get
        Set(value As Boolean)
            ViewState("RaiseApplyFiltersEvent") = value
        End Set
    End Property
    Public Property RaisePageChangedEvent As Boolean Implements IViewCommonTemplateList.RaisePageChangedEvent
        Get
            Return ViewStateOrDefault("RaisePageChangedEvent", False)
        End Get
        Set(value As Boolean)
            ViewState("RaisePageChangedEvent") = value
        End Set
    End Property
    Public Property RaiseSessionTimeoutEvent As Boolean Implements IViewCommonTemplateList.RaiseSessionTimeoutEvent
        Get
            Return ViewStateOrDefault("RaiseSessionTimeoutEvent", False)
        End Get
        Set(value As Boolean)
            ViewState("RaiseSessionTimeoutEvent") = value
        End Set
    End Property
    Private Property ContainerContext As lm.Comol.Core.TemplateMessages.Domain.dtoModuleContext Implements IViewCommonTemplateList.ContainerContext
        Get
            Return ViewStateOrDefault("ContainerContext", New lm.Comol.Core.TemplateMessages.Domain.dtoModuleContext)
        End Get
        Set(value As lm.Comol.Core.TemplateMessages.Domain.dtoModuleContext)
            ViewState("ContainerContext") = value
        End Set
    End Property
    Public Property CurrentPageSize As Integer Implements IViewCommonTemplateList.CurrentPageSize
        Get
            Return ViewStateOrDefault("CurrentPageSize", 25)
        End Get
        Set(value As Integer)
            ViewState("CurrentPageSize") = value
        End Set
    End Property
    Public Property SendTemplateActions As Boolean Implements IViewCommonTemplateList.SendTemplateActions
        Get
            Return ViewStateOrDefault("SendTemplateActions", False)
        End Get
        Set(value As Boolean)
            ViewState("SendTemplateActions") = value
        End Set
    End Property
    Private Property CurrentAscending As Boolean Implements IViewCommonTemplateList.CurrentAscending
        Get
            Return ViewStateOrDefault("CurrentAscending", True)
        End Get
        Set(value As Boolean)
            ViewState("CurrentAscending") = value
        End Set
    End Property
    Private Property CurrentOrderBy As TemplateOrder Implements IViewCommonTemplateList.CurrentOrderBy
        Get
            Return ViewStateOrDefault("CurrentOrderBy", TemplateOrder.ByName)
        End Get
        Set(value As TemplateOrder)
            ViewState("CurrentOrderBy") = value
        End Set
    End Property
    Public Property CurrentDisplay As TemplateDisplay Implements IViewCommonTemplateList.CurrentDisplay
        Get
            If Me.RBLdisplayType.SelectedIndex = -1 Then
                Return TemplateDisplay.All
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TemplateDisplay).GetByString(Me.RBLdisplayType.SelectedValue, TemplateDisplay.OnlyVisible)
            End If
        End Get
        Set(value As TemplateDisplay)
            If Not IsNothing(Me.RBLdisplayType.Items.FindByValue(value.ToString)) Then
                Me.RBLdisplayType.SelectedValue = value.ToString
            End If
        End Set
    End Property
    Private Property CurrentSessionId As Guid Implements IViewCommonTemplateList.CurrentSessionId
        Get
            Return ViewStateOrDefault("CurrentSessionId", Guid.Empty)
        End Get
        Set(value As Guid)
            ViewState("CurrentSessionId") = value
        End Set
    End Property
    Private ReadOnly Property UnknownUserName As String Implements IViewCommonTemplateList.UnknownUserName
        Get
            Return Resource.getValue("UnknownUserProfile")
        End Get
    End Property
    Private Property OpenIdTemplate As Long Implements IViewCommonTemplateList.OpenIdTemplate
        Get
            Return ViewStateOrDefault("OpenIdTemplate", CLng(0))
        End Get
        Set(value As Long)
            ViewState("OpenIdTemplate") = value
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Internal"
    Public Property RaiseTemplateSelection As Boolean
        Get
            Return ViewStateOrDefault("RaiseTemplateSelection", False)
        End Get
        Set(value As Boolean)
            ViewState("RaiseTemplateSelection") = value
        End Set
    End Property
    Public Event ApplyFiltersEvent(filters As dtoBaseFilters)
    Public Event PageChangedEvent(filters As dtoBaseFilters, pageIndex As Integer, pageSize As Integer)
    Public Event SelectedTemplate(idTemplate As Long, idversion As Long)
    Public Event SessionTimeout()
    Public ReadOnly Property GetRevisionCssClass(display As ItemDisplayAs) As String
        Get
            Select Case display
                Case ItemDisplayAs.first, ItemDisplayAs.last
                    Return "the" & display.ToString
                Case ItemDisplayAs.item
                    Return ""
                Case Else
                    Return "the" & ItemDisplayAs.first.ToString & " the" & ItemDisplayAs.last.ToString
            End Select
        End Get
    End Property
    Public ReadOnly Property GetOpenCssClass(idTemplate As Long) As String
        Get
            Return IIf(idTemplate = OpenIdTemplate, "expanded", "")
        End Get
    End Property


  
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgridBottom.Pager = Me.Pager
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Templates", "Modules", "Templates")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setHyperLink(HYPaddTemplate, False, True)
            .setHyperLink(HYPaddPersonalTemplate, False, True)
            .setHyperLink(HYPaddObjectTemplate, False, True)

            .setLabel(LBtemplateNameFilter_t)
            .setLiteral(LTpageBottom)
            .setButton(BTNfilterTemplates, True)
            .setLabel(LBtemplateDisplayFilter_t)
        End With
    End Sub
#End Region

#Region "implements"
    Private Sub DisplaySessionTimeout() Implements IViewCommonTemplateList.DisplaySessionTimeout
        MLVlist.SetActiveView(VIWempty)
        If RaiseSessionTimeoutEvent Then
            RaiseEvent SessionTimeout()
        End If
    End Sub
#Region "Display Messages"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As lm.Comol.Core.TemplateMessages.ModuleTemplateMessages.ActionType) Implements IViewCommonTemplateList.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, , InteractionType.UserWithLearningObject)
    End Sub

    Private Sub DisplayMessage(name As String, action As lm.Comol.Core.BaseModules.TemplateMessages.Domain.ListAction, obj As lm.Comol.Core.TemplateMessages.ModuleTemplateMessages.ObjectType, completed As Boolean) Implements IViewCommonTemplateList.DisplayMessage
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(String.Format(Resource.getValue(obj.ToString & "." & action.ToString & "." & completed.ToString), name), IIf(completed, lm.Comol.Core.DomainModel.Helpers.MessageType.success, lm.Comol.Core.DomainModel.Helpers.MessageType.error))
    End Sub
#End Region

    Public Sub InitializeControl(mContext As lm.Comol.Core.TemplateMessages.Domain.dtoModuleContext, filter As dtoBaseFilters, availableType As List(Of TemplateType), availableDisplay As List(Of TemplateDisplay), sessionId As Guid, Optional idTemplate As Long = 0, Optional displayAdd As Boolean = False, Optional addUrl As String = "", Optional addPersonalUrl As String = "", Optional addObjectUrl As String = "", Optional title As String = "") Implements IViewCommonTemplateList.InitializeControl
        OpenIdTemplate = idTemplate
        InitializeControl(sessionId, displayAdd, addUrl, title)
        CurrentPresenter.InitView(mContext, filter, AvailableTypes, availableDisplay, displayAdd, addUrl, addPersonalUrl, addObjectUrl)
    End Sub
    Private Sub InitializeControl(sessionId As Guid, Optional displayAdd As Boolean = False, Optional addUrl As String = "", Optional title As String = "")
        Me.CurrentSessionId = sessionId
        Me.DVtitle.Visible = (displayAdd AndAlso Not String.IsNullOrEmpty(addUrl)) OrElse Not String.IsNullOrEmpty(title)
        LTtitle.Text = title
    End Sub

    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewCommonTemplateList.DisplayNoPermission
        MLVlist.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplayNoPermission() Implements IViewCommonTemplateList.DisplayNoPermission
        MLVlist.SetActiveView(VIWempty)
    End Sub
    Private Sub SetAddUrl(url As String) Implements IViewCommonTemplateList.SetAddUrl
        HYPaddTemplate.Visible = Not String.IsNullOrEmpty(url)
        HYPaddTemplate.NavigateUrl = BaseUrl & url
    End Sub
    Private Sub SetAddUrl(url As String, personalUrl As String, addObjectUrl As String) Implements IViewCommonTemplateList.SetAddUrl
        HYPaddTemplate.Visible = Not String.IsNullOrEmpty(url)
        HYPaddTemplate.NavigateUrl = BaseUrl & url

        HYPaddPersonalTemplate.Visible = Not String.IsNullOrEmpty(personalUrl)
        HYPaddPersonalTemplate.NavigateUrl = BaseUrl & personalUrl
        HYPaddObjectTemplate.Visible = Not String.IsNullOrEmpty(addObjectUrl)
        HYPaddObjectTemplate.NavigateUrl = BaseUrl & addObjectUrl
        Dim items As Integer = IIf(String.IsNullOrEmpty(url), 0, 1)
        items += IIf(String.IsNullOrEmpty(personalUrl), 0, 1)
        items += IIf(String.IsNullOrEmpty(addObjectUrl), 0, 1)
        DVaddButtons.Visible = (items > 0)
        If items = 1 Then
            DVaddButtons.Attributes("class") = DVaddButtons.Attributes("class").Replace("enabled", "")
        End If

    End Sub
    Private Sub LoadTemplates(templates As List(Of dtoDisplayTemplateDefinition)) Implements IViewCommonTemplateList.LoadTemplates
        Me.CTRLmessages.Visible = False

        Me.RPTtemplates.DataSource = templates
        Me.RPTtemplates.DataBind()

        If templates.Count > 0 Then
            ParseRepeater()
        End If
        Me.MLVlist.SetActiveView(VIWlist)
    End Sub
    Private Sub LoadNoTemplatesFound() Implements IViewCommonTemplateList.LoadNoTemplatesFound
        LoadTemplates(New List(Of dtoDisplayTemplateDefinition))
    End Sub
    Private Sub LoadTemplateDisplay(types As List(Of TemplateDisplay), Optional selected As TemplateDisplay = TemplateDisplay.None) Implements IViewCommonTemplateList.LoadTemplateDisplay
        Me.RBLdisplayType.Items.Clear()
        If types.Contains(TemplateDisplay.All) Then
            Me.RBLdisplayType.Items.Add(New ListItem(Resource.getValue("RBLdisplayType." & TemplateDisplay.All.ToString), TemplateDisplay.All.ToString))
        End If
        If types.Contains(TemplateDisplay.OnlyVisible) Then
            Me.RBLdisplayType.Items.Add(New ListItem(Resource.getValue("DDLdisplayType." & TemplateDisplay.OnlyVisible.ToString), TemplateDisplay.OnlyVisible.ToString))
        End If
        If types.Contains(TemplateDisplay.Deleted) Then
            Me.RBLdisplayType.Items.Add(New ListItem(Resource.getValue("DDLdisplayType." & TemplateDisplay.Deleted.ToString), TemplateDisplay.Deleted.ToString))
        End If
        Me.CurrentDisplay = selected
        DVdisplayType.Visible = (RBLdisplayType.Items.Count > 1)
    End Sub
    Private Sub ReloadPageAndFilters(filters As lm.Comol.Core.TemplateMessages.Domain.dtoBaseFilters, types As List(Of TemplateDisplay), Optional selected As TemplateDisplay = TemplateDisplay.None) Implements IViewCommonTemplateList.ReloadPageAndFilters
        LoadTemplateDisplay(types, selected)
        CurrentFilters = filters
        If RaiseApplyFiltersEvent Then
            RaiseEvent ApplyFiltersEvent(filters)
        End If
    End Sub
#End Region

#Region "Internal"
    Private Sub RPTtemplates_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTtemplates.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As dtoDisplayTemplateDefinition = DirectCast(e.Item.DataItem, dtoDisplayTemplateDefinition)

            Dim oLiteral As Literal = e.Item.FindControl("LTlastEditBy")
            Dim ModifiedBy As String

            ModifiedBy = dto.UserDisplayName
            oLiteral.Text = ModifiedBy

            oLiteral = e.Item.FindControl("LTtemplateType")
            oLiteral.Text = Resource.getValue("TemplateType.Translations." & dto.Type.ToString)

            'oLiteral = e.Item.FindControl("LTmoduleName")
            'oLiteral.Text = ModuleFileStatistics


            Dim olabel As Label = e.Item.FindControl("LBlastEditOn")
            If dto.ModifiedOn.HasValue Then
                olabel.Text = FormatDateTime(dto.ModifiedOn.Value, DateFormat.ShortDate) & " " & FormatDateTime(dto.ModifiedOn.Value, DateFormat.ShortTime)
                olabel.ToolTip = dto.ModifiedByName
            Else
                olabel.Text = "&nbsp;"
            End If

            Dim cContext As lm.Comol.Core.TemplateMessages.Domain.dtoModuleContext = ContainerContext
            Dim oHyperlink As HyperLink = e.Item.FindControl("HYPtemplateEdit")
            oLiteral = e.Item.FindControl("LTemptyActions")

            Me.Resource.setHyperLink(oHyperlink, True, True)
            oHyperlink.Visible = dto.Permission.AllowEdit
            oHyperlink.NavigateUrl = BaseUrl & lm.Comol.Core.TemplateMessages.RootObject.EditByStep(CurrentSessionId, dto.Type, dto.OwnerInfo, WizardTemplateStep.Settings, cContext.IdCommunity, cContext.ModuleCode, cContext.ModulePermissions, cContext.BackUrl, dto.Id, dto.IdLastVersion)

            oHyperlink = e.Item.FindControl("HYPtemplatePreview")
            Me.Resource.setHyperLink(oHyperlink, True, True)
            oHyperlink.Visible = dto.Permission.AllowUse
            oHyperlink.NavigateUrl = BaseUrl & lm.Comol.Core.TemplateMessages.RootObject.EditByStep(CurrentSessionId, dto.Type, dto.OwnerInfo, WizardTemplateStep.Settings, cContext.IdCommunity, cContext.ModuleCode, cContext.ModulePermissions, cContext.BackUrl, dto.Id, dto.IdLastVersion, True)

            oHyperlink = e.Item.FindControl("HYPtemplateEditPermissions")
            Me.Resource.setHyperLink(oHyperlink, True, True)
            oHyperlink.Visible = dto.Permission.AllowChangePermission
            oHyperlink.NavigateUrl = BaseUrl & lm.Comol.Core.TemplateMessages.RootObject.EditByStep(CurrentSessionId, dto.Type, dto.OwnerInfo, WizardTemplateStep.Permission, cContext.IdCommunity, cContext.ModuleCode, cContext.ModulePermissions, cContext.BackUrl, dto.Id, dto.IdLastVersion)


            Dim oLinkbutton As LinkButton = e.Item.FindControl("LNBtemplateDelete")
            Resource.setLinkButton(oLinkbutton, True, True)
            oLinkbutton.Visible = dto.Permission.AllowDelete

            oLinkbutton = e.Item.FindControl("LNBtemplateVirtualDelete")
            Resource.setLinkButton(oLinkbutton, True, True)
            oLinkbutton.Visible = dto.Permission.AllowVirtualDelete

            oLinkbutton = e.Item.FindControl("LNBtemplateRecover")
            oLinkbutton.Visible = dto.Permission.AllowUnDelete
            Resource.setLinkButton(oLinkbutton, True, True)

            oLinkbutton = e.Item.FindControl("LNBtemplateclone")
            oLinkbutton.Visible = dto.Permission.AllowClone
            Resource.setLinkButton(oLinkbutton, True, True)

            oLinkbutton = e.Item.FindControl("LNBtemplateNewVersion")
            oLinkbutton.Visible = dto.Permission.AllowNewVersion
            Resource.setLinkButton(oLinkbutton, True, True)

            Dim allowSendMail As Boolean = (dto.Permission.AllowUse AndAlso RaiseTemplateSelection AndAlso dto.Versions.Where(Function(v) v.Id = dto.IdLastVersion AndAlso v.Status = TemplateStatus.Active).Any())

            oLinkbutton = e.Item.FindControl("LNBsendMail")
            oLinkbutton.Visible = allowSendMail
            Resource.setLinkButton(oLinkbutton, True, True)

            oLiteral.Visible = Not (allowSendMail OrElse dto.Permission.AllowClone OrElse dto.Permission.AllowDelete OrElse dto.Permission.AllowEdit OrElse dto.Permission.AllowUnDelete OrElse dto.Permission.AllowUse OrElse dto.Permission.AllowVirtualDelete)

        ElseIf e.Item.ItemType = ListItemType.Footer Then
            Dim oTableItem As HtmlControl = e.Item.FindControl("TRempty")
            oTableItem.Visible = (RPTtemplates.Items.Count = 0)
            If (RPTtemplates.Items.Count = 0) Then
                Dim oTableCell As HtmlTableCell = e.Item.FindControl("TDemptyItems")

                '  oTableCell.ColSpan = 3 + AvailableColumns.Where(Function(c) c = ProfileColumn.agency OrElse c = ProfileColumn.mail OrElse c = ProfileColumn.status OrElse c = ProfileColumn.type OrElse c = ProfileColumn.companyName).Count

                Dim oLabel As Label = e.Item.FindControl("LBnoTemplates")
                Resource.setLabel(oLabel)
            End If

            If RPTtemplates.Items.Count < 2 Then
                Dim oLinkButton As LinkButton
                For Each name As String In [Enum].GetNames(GetType(TemplateOrder))
                    oLinkButton = RPTtemplates.Controls(0).FindControl("LNBorder" & name & "Up")
                    If Not IsNothing(oLinkButton) Then
                        oLinkButton.Visible = False
                    End If
                    oLinkButton = RPTtemplates.Controls(0).FindControl("LNBorder" & name & "Down")
                    If Not IsNothing(oLinkButton) Then
                        oLinkButton.Visible = False
                    End If
                Next
            ElseIf RPTtemplates.Items.Count > 2 AndAlso RPTtemplates.Controls(0).FindControl("LNBorder" & TemplateOrder.ByName.ToString & "Up").Visible = False Then
                Dim oLinkButton As LinkButton
                For Each name As String In [Enum].GetNames(GetType(TemplateOrder))
                    oLinkButton = RPTtemplates.Controls(0).FindControl("LNBorder" & name & "Up")
                    If Not IsNothing(oLinkButton) Then
                        oLinkButton.Visible = True
                    End If
                    oLinkButton = RPTtemplates.Controls(0).FindControl("LNBorder" & name & "Down")
                    If Not IsNothing(oLinkButton) Then
                        oLinkButton.Visible = True
                    End If
                Next
            End If
        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oLiteral As Literal = e.Item.FindControl("LTtemplateName_t")
            Resource.setLiteral(oLiteral)

            oLiteral = e.Item.FindControl("LTtemplateType_t")
            Resource.setLiteral(oLiteral)

            oLiteral = e.Item.FindControl("LTcreatedBy_t")
            Resource.setLiteral(oLiteral)

            oLiteral = e.Item.FindControl("LTlastEditOn_t")
            Resource.setLiteral(oLiteral)

            oLiteral = e.Item.FindControl("LTactions_t")
            Resource.setLiteral(oLiteral)

            Dim oLinkButton As LinkButton
            For Each name As String In [Enum].GetNames(GetType(TemplateOrder))
                oLinkButton = e.Item.FindControl("LNBorder" & name & "Up")
                If Not IsNothing(oLinkButton) Then
                    oLinkButton.ToolTip = Resource.getValue("TemplateOrder.Ascending") & Resource.getValue("TemplateOrder." & name)
                End If
                oLinkButton = e.Item.FindControl("LNBorder" & name & "Down")
                If Not IsNothing(oLinkButton) Then
                    oLinkButton.ToolTip = Resource.getValue("TemplateOrder.Descending") & Resource.getValue("TemplateOrder." & name)
                End If
            Next
        End If
    End Sub
    Protected Sub RPTrevisions_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        Dim dto As dtoDisplayTemplateVersion = DirectCast(e.Item.DataItem, dtoDisplayTemplateVersion)
        Dim oLiteral As Literal = e.Item.FindControl("LTrevisionName")

        oLiteral.Text = String.Format(Resource.getValue("VersionName"), dto.Number)
        oLiteral.Text &= " (" & Resource.getValue("TemplateStatus.Translations." & dto.Status.ToString) & ")."

        Dim olabel As Label = e.Item.FindControl("LBlastEditOn")
        If dto.ModifiedOn.HasValue Then
            olabel.Text = FormatDateTime(dto.ModifiedOn.Value, DateFormat.ShortDate) & " " & FormatDateTime(dto.ModifiedOn.Value, DateFormat.ShortTime)
            olabel.ToolTip = dto.ModifiedByName
        Else
            olabel.Text = "&nbsp;"
        End If


        Dim cContext As lm.Comol.Core.TemplateMessages.Domain.dtoModuleContext = ContainerContext
        Dim oHyperlink As HyperLink = e.Item.FindControl("HYPversionEdit")
        oLiteral = e.Item.FindControl("LTemptyActions")

        Me.Resource.setHyperLink(oHyperlink, True, True)
        oHyperlink.Visible = dto.Permission.AllowEdit
        oHyperlink.NavigateUrl = BaseUrl & lm.Comol.Core.TemplateMessages.RootObject.EditByStep(CurrentSessionId, dto.Template.Type, dto.Template.OwnerInfo, WizardTemplateStep.Settings, cContext.IdCommunity, cContext.ModuleCode, cContext.ModulePermissions, cContext.BackUrl, dto.IdTemplate, dto.Id)

        oHyperlink = e.Item.FindControl("HYPversionPreview")
        Me.Resource.setHyperLink(oHyperlink, True, True)
        oHyperlink.Visible = dto.Permission.AllowUse
        oHyperlink.NavigateUrl = BaseUrl & lm.Comol.Core.TemplateMessages.RootObject.EditByStep(CurrentSessionId, dto.Template.Type, dto.Template.OwnerInfo, WizardTemplateStep.Settings, cContext.IdCommunity, cContext.ModuleCode, cContext.ModulePermissions, cContext.BackUrl, dto.IdTemplate, dto.Id, True)

        oHyperlink = e.Item.FindControl("HYPversionEditPermissions")
        Me.Resource.setHyperLink(oHyperlink, True, True)
        oHyperlink.Visible = dto.Permission.AllowChangePermission
        oHyperlink.NavigateUrl = BaseUrl & lm.Comol.Core.TemplateMessages.RootObject.EditByStep(CurrentSessionId, dto.Template.Type, dto.Template.OwnerInfo, WizardTemplateStep.Permission, cContext.IdCommunity, cContext.ModuleCode, cContext.ModulePermissions, cContext.BackUrl, dto.IdTemplate, dto.Id)


        Dim oLinkbutton As LinkButton = e.Item.FindControl("LNBversionDelete")
        Resource.setLinkButton(oLinkbutton, True, True)
        oLinkbutton.Visible = dto.Permission.AllowDelete

        oLinkbutton = e.Item.FindControl("LNBversionVirtualDelete")
        Resource.setLinkButton(oLinkbutton, True, True)
        oLinkbutton.Visible = dto.Permission.AllowVirtualDelete

        oLinkbutton = e.Item.FindControl("LNBversionRecover")
        oLinkbutton.Visible = dto.Permission.AllowUnDelete
        Resource.setLinkButton(oLinkbutton, True, True)

        oLinkbutton = e.Item.FindControl("LNBversionClone")
        oLinkbutton.Visible = dto.Permission.AllowClone
        Resource.setLinkButton(oLinkbutton, True, True)

        Dim allowSendMail As Boolean = (dto.Permission.AllowUse AndAlso RaiseTemplateSelection AndAlso dto.Status = TemplateStatus.Active)
        oLinkbutton = e.Item.FindControl("LNBsendMail")
        oLinkbutton.Visible = allowSendMail
        Resource.setLinkButton(oLinkbutton, True, True)

        oLiteral.Visible = Not (allowSendMail OrElse dto.Permission.AllowClone OrElse dto.Permission.AllowDelete OrElse dto.Permission.AllowEdit OrElse dto.Permission.AllowUnDelete OrElse dto.Permission.AllowUse OrElse dto.Permission.AllowVirtualDelete)


    End Sub

    Private Sub RPTsubmissions_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTtemplates.ItemCommand
        Select Case e.CommandName
            Case "orderby"
                Dim filters As dtoBaseFilters = CurrentFilters
                filters.Ascending = e.CommandArgument.contains("True")
                filters.OrderBy = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TemplateOrder).GetByString(e.CommandArgument.replace("." & filters.Ascending.ToString, ""), TemplateOrder.ByName)
                If (RaiseApplyFiltersEvent) Then
                    RaiseEvent ApplyFiltersEvent(filters)
                End If
                Me.CurrentPresenter.LoadTemplates(ContainerContext, filters, Me.Pager.PageIndex, Me.Pager.PageSize)
            Case "delete", "virtualdelete", "recover", "clone", "new", "sendmail"
                Dim oLiteral As Literal = e.Item.FindControl("LTtemplateName")
                Dim name As String = oLiteral.Text
                Dim idTemplate As Long = 0
                If IsNumeric(e.CommandArgument) Then
                    idTemplate = CLng(e.CommandArgument)
                End If

                If idTemplate > 0 Then
                    Select Case e.CommandName
                        Case "delete"
                            Me.CurrentPresenter.PhisicalDeleteTemplate(idTemplate, name, ContainerContext, CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
                        Case "virtualdelete"
                            Me.CurrentPresenter.VirtualDeleteTemplate(idTemplate, name, True, ContainerContext, CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
                        Case "recover"
                            Me.CurrentPresenter.VirtualDeleteTemplate(idTemplate, name, False, ContainerContext, CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
                        Case "clone"
                            Me.CurrentPresenter.CloneTemplate(idTemplate, name, Resource.getValue("TemplateClone"), ContainerContext, CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
                        Case "new"
                            Me.CurrentPresenter.AddNewVersion(idTemplate, name, ContainerContext, CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
                        Case "sendmail"
                            RaiseEvent SelectedTemplate(idTemplate, 0)
                    End Select
                Else
                    Me.CurrentPresenter.LoadTemplates(ContainerContext, CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
                End If
        End Select
    End Sub
    Protected Sub RPTrevisions_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs)

        Dim oLiteral As Literal = e.Item.FindControl("LTtemplateName")
        Dim name As String = oLiteral.Text
        Dim idTemplate As Long = CLng(e.CommandArgument.ToString.Split(",")(1))
        Dim idVersion As Long = CLng(e.CommandArgument.ToString.Split(",")(0))


        If idVersion > 0 Then
            Select Case e.CommandName
                Case "delete"
                    Me.CurrentPresenter.PhisicalDeleteVersion(idTemplate, idVersion, name, ContainerContext, CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
                Case "virtualdelete"
                    Me.CurrentPresenter.VirtualDeleteVersion(idTemplate, idVersion, name, True, ContainerContext, CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
                Case "recover"
                    Me.CurrentPresenter.VirtualDeleteVersion(idTemplate, idVersion, name, False, ContainerContext, CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
                    'Case "clone"
                    '    Me.CurrentPresenter.CloneTemplate(idTemplate, name, Resource.getValue("TemplateClone"), ContainerContext, CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
                Case "sendmail"
                    RaiseEvent SelectedTemplate(idTemplate, idVersion)
            End Select
        Else
            Me.CurrentPresenter.LoadTemplates(ContainerContext, CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
        End If
    End Sub

    Private Sub RBLdisplayType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RBLdisplayType.SelectedIndexChanged
        Dim filters As dtoBaseFilters = CurrentFilters
        filters.PageIndex = Me.Pager.PageIndex
        filters.PageSize = Me.Pager.PageSize
        If (RaiseApplyFiltersEvent) Then
            RaiseEvent ApplyFiltersEvent(filters)
        End If
        Me.CTRLmessages.Visible = False
        Me.CurrentPresenter.LoadTemplates(ContainerContext, filters, 0, Me.Pager.PageSize)
    End Sub

    Private Sub BTNfilterTemplates_Click(sender As Object, e As System.EventArgs) Handles BTNfilterTemplates.Click
        Dim filters As dtoBaseFilters = CurrentFilters
        filters.PageIndex = Me.Pager.PageIndex
        filters.PageSize = Me.Pager.PageSize
        If (RaiseApplyFiltersEvent) Then
            RaiseEvent ApplyFiltersEvent(filters)
        End If
        Me.CTRLmessages.Visible = False
        Me.CurrentPresenter.LoadTemplates(ContainerContext, filters, 0, Me.Pager.PageSize)
    End Sub

    Private Sub PGgridBottom_OnPageSelected() Handles PGgridBottom.OnPageSelected
        Dim filters As dtoBaseFilters = CurrentFilters
        filters.PageIndex = Me.Pager.PageIndex
        filters.PageSize = Me.Pager.PageSize
        If (RaiseApplyFiltersEvent) OrElse RaisePageChangedEvent Then
            RaiseEvent PageChangedEvent(filters, filters.PageIndex, filters.PageSize)
        End If
        Me.CTRLmessages.Visible = False
        Me.CurrentPresenter.LoadTemplates(ContainerContext, filters, Me.Pager.PageIndex, Me.Pager.PageSize)
    End Sub

    Private Sub ParseRepeater()
        For Each rowTemplate As RepeaterItem In RPTtemplates.Items
            Dim creatorName As String = DirectCast(rowTemplate.FindControl("LTlastEditBy"), Literal).Text
            Dim repeater As Repeater = DirectCast(rowTemplate.FindControl("RPTrevisions"), Repeater)

            Dim oLiteral As Literal
            Dim revModifiedBy As String = ""
            If repeater.Items.Count = 1 Then
                oLiteral = DirectCast(repeater.Items(0).FindControl("LTdisplayName"), Literal)
                revModifiedBy = oLiteral.Text
                If revModifiedBy = creatorName Then
                    oLiteral.Text = ""
                End If
            ElseIf repeater.Items.Count > 1 Then
                For i As Integer = repeater.Items.Count - 1 To 0 Step -1
                    Dim oLiteralOtherRow As Literal
                    Dim rowRevision As RepeaterItem = repeater.Items(i)
                    oLiteral = DirectCast(repeater.Items(i).FindControl("LTdisplayName"), Literal)
                    revModifiedBy = oLiteral.Text
                    If i > 0 Then
                        oLiteralOtherRow = DirectCast(repeater.Items(i - 1).FindControl("LTdisplayName"), Literal)
                        If revModifiedBy = oLiteralOtherRow.Text Then
                            oLiteral.Text = ""
                        End If
                    Else
                        oLiteralOtherRow = DirectCast(repeater.Items(i + 1).FindControl("LTdisplayName"), Literal)
                        If (revModifiedBy = oLiteralOtherRow.Text OrElse String.IsNullOrEmpty(oLiteralOtherRow.Text)) AndAlso creatorName = revModifiedBy Then
                            oLiteral.Text = ""
                        End If
                    End If
                Next
            End If
        Next
    End Sub
#End Region

End Class