Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports System.Linq
Imports System.Collections.Generic

Public Class UC_SelectCallUsersForMessageService
    Inherits BaseControl
    Implements IViewSelectUsersForMessageService


#Region "Context"
    Private _Presenter As SelectUsersForMessageService
    Private ReadOnly Property CurrentPresenter() As SelectUsersForMessageService
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New SelectUsersForMessageService(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property isInitialized As Boolean Implements IViewSelectUsersForMessageService.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public Property WasGridInitialized As Boolean Implements IViewSelectUsersForMessageService.WasGridInitialized
        Get
            Return ViewStateOrDefault("WasGridInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("WasGridInitialized") = value
        End Set
    End Property

    Private Property IsInitializedForSubmitters As Boolean Implements IViewSelectUsersForMessageService.IsInitializedForSubmitters

        Get
            Return ViewStateOrDefault("IsInitializedForSubmitters", False)
        End Get
        Set(value As Boolean)
            ViewState("IsInitializedForSubmitters") = value
        End Set
    End Property
    Private Property IsInitializedForNoSubmitters As Boolean Implements IViewSelectUsersForMessageService.IsInitializedForNoSubmitters
        Get
            Return ViewStateOrDefault("IsInitializedForNoSubmitters", False)
        End Get
        Set(value As Boolean)
            ViewState("IsInitializedForNoSubmitters") = value
        End Set
    End Property
    Private Property SelectAll As Boolean Implements IViewSelectUsersForMessageService.SelectAll
        Get
            Return ViewStateOrDefault("SelectAll", False)
        End Get
        Set(value As Boolean)
            ViewState("SelectAll") = value
        End Set
    End Property
    Private Property HasUserValues As Boolean Implements IViewSelectUsersForMessageService.HasUserValues
        Get
            Return ViewStateOrDefault("HasUserValues", False)
        End Get
        Set(value As Boolean)
            ViewState("HasUserValues") = value
        End Set
    End Property
    Private Property CallType As lm.Comol.Modules.CallForPapers.Domain.CallForPaperType Implements IViewSelectUsersForMessageService.CallType
        Get
            Return Me.ViewStateOrDefault("CallType", lm.Comol.Modules.CallForPapers.Domain.CallForPaperType.CallForBids)
        End Get
        Set(value As lm.Comol.Modules.CallForPapers.Domain.CallForPaperType)
            Me.ViewState("CallType") = value
        End Set
    End Property
    Private Property FromPortal As Boolean Implements IViewSelectUsersForMessageService.FromPortal
        Get
            Return Me.ViewStateOrDefault("FromPortal", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("FromPortal") = value
        End Set
    End Property
    Public Property RaiseEvents As Boolean Implements IViewSelectUsersForMessageService.RaiseEvents
        Get
            Return Me.ViewStateOrDefault("RaiseEvents", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("RaiseEvents") = value
        End Set
    End Property
    Public Property RemoveUsers As List(Of Integer) Implements IViewSelectUsersForMessageService.RemoveUsers
        Get
            Return Me.ViewStateOrDefault("RemoveUsers", New List(Of Integer))
        End Get
        Set(value As List(Of Integer))
            Me.ViewState("RemoveUsers") = value
        End Set
    End Property
    Private Property UsersPageSize As Int32 Implements IViewSelectUsersForMessageService.UsersPageSize
        Get
            Return ViewStateOrDefault("UsersPageSize", 25)
        End Get
        Set(value As Int32)
            Me.ViewState("UsersPageSize") = value
        End Set
    End Property
    Private Property Pager As PagerBase Implements IViewSelectUsersForMessageService.Pager
        Get
            Return ViewStateOrDefault("CallsPager", New lm.Comol.Core.DomainModel.PagerBase With {.PageSize = UsersPageSize})
        End Get
        Set(value As PagerBase)
            Me.ViewState("CallsPager") = value
            Me.PGgrid.Pager = value
            Me.PGgrid.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
            'Me.DIVpageSize.Visible = (Not value.Count < Me.DefaultPageSize)
        End Set
    End Property
    Private Property AvailableColumns As List(Of ColumnMessageGrid) Implements IViewSelectUsersForMessageService.AvailableColumns
        Get
            Return Me.ViewStateOrDefault("AvailableColumns", New List(Of ColumnMessageGrid))
        End Get
        Set(value As List(Of ColumnMessageGrid))
            Me.ViewState("AvailableColumns") = value
        End Set
    End Property
    Private ReadOnly Property SelectedFilter As dtoUsersByMessageFilter Implements IViewSelectUsersForMessageService.SelectedFilter
        Get
            Dim oFilter As dtoUsersByMessageFilter = ViewStateOrDefault("CurrentFilter", New dtoUsersByMessageFilter() With {.Ascending = True, .OrderBy = SubmissionsOrder.ByUser})
            If (oFilter.StatusTranslations.Count = 0) Then
                For Each name As String In [Enum].GetNames(GetType(SubmissionFilterStatus))
                    oFilter.StatusTranslations.Add([Enum].Parse(GetType(SubmissionFilterStatus), name), Me.Resource.getValue("SubmissionFilterStatus." & name))
                Next
            End If
            If Me.DDLselectFrom.Items.Count > 0 Then
                oFilter.Access = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of AccessTypeFilter).GetByString(Me.DDLselectFrom.SelectedValue, AccessTypeFilter.NoSubmitters)
            Else
                oFilter.Access = AccessTypeFilter.NoSubmitters
            End If
            If Me.DDLsearchUserBy.Items.Count > 0 Then
                oFilter.SearchBy = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy).GetByString(Me.DDLsearchUserBy.SelectedValue, lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy.All)
            Else
                oFilter.SearchBy = lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy.All
            End If
            If oFilter.Access = AccessTypeFilter.Submitters Then
                If Me.DDLsubmitterTypes.Items.Count = 0 Then
                    oFilter.IdSubmitterType = -1
                Else
                    oFilter.IdSubmitterType = CLng(Me.DDLsubmitterTypes.SelectedValue)
                End If
                If Me.DDLsubmissionStatus.Items.Count > 0 Then
                    oFilter.Status = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SubmissionFilterStatus).GetByString(Me.DDLsubmissionStatus.SelectedValue, SubmissionFilterStatus.All)
                Else
                    oFilter.Status = SubmissionFilterStatus.All
                End If
            Else
                oFilter.IdSubmitterType = -1
                oFilter.Status = SubmissionFilterStatus.All
            End If
            oFilter.IdAgency = SelectedIdAgency
            oFilter.Ascending = Ascending
            oFilter.OrderBy = CurrentOrderBy
            oFilter.IdMessages = CTRLselectMessages.SelectedItems
            If Not String.IsNullOrEmpty(TXBsearchBy.Text) Then
                oFilter.Value = TXBsearchBy.Text.Trim
            End If
            oFilter.StartWith = CurrentStartWith
            Return oFilter
        End Get
    End Property
    Private Property CurrentFilter As dtoUsersByMessageFilter Implements IViewSelectUsersForMessageService.CurrentFilter
        Get
            Dim filter As dtoUsersByMessageFilter = ViewStateOrDefault("CurrentFilter", SelectedFilter)
            If (filter.StatusTranslations.Count = 0) Then
                For Each name As String In [Enum].GetNames(GetType(SubmissionFilterStatus))
                    filter.StatusTranslations.Add([Enum].Parse(GetType(SubmissionFilterStatus), name), Me.Resource.getValue("SubmissionFilterStatus." & name))
                Next
            End If
            Return filter
        End Get
        Set(value As dtoUsersByMessageFilter)
            Me.TXBsearchBy.Text = value.Value
            If DDLselectFrom.SelectedIndex > -1 AndAlso Me.DDLselectFrom.SelectedValue <> value.Access.ToString AndAlso Not IsNothing(Me.DDLsubmissionStatus.Items.FindByValue(value.Access.ToString)) Then
                DDLselectFrom.SelectedValue = value.Access.ToString
            End If
            If DDLsubmissionStatus.SelectedIndex > -1 AndAlso Me.DDLsubmissionStatus.SelectedValue <> value.Status.ToString AndAlso Not IsNothing(Me.DDLsubmissionStatus.Items.FindByValue(value.Status.ToString)) Then
                Me.DDLsubmissionStatus.SelectedValue = value.Status.ToString
            End If
            If DDLsubmitterTypes.SelectedIndex > -1 AndAlso Me.DDLsubmitterTypes.SelectedValue <> value.IdSubmitterType.ToString AndAlso Not IsNothing(Me.DDLsubmitterTypes.Items.FindByValue(value.IdSubmitterType.ToString)) Then
                Me.DDLsubmitterTypes.SelectedValue = value.IdSubmitterType
            End If
            SelectedIdAgency = value.IdAgency
            If DDLsearchUserBy.SelectedIndex > -1 AndAlso Me.DDLsearchUserBy.SelectedValue <> value.SearchBy.ToString AndAlso Not IsNothing(Me.DDLsearchUserBy.Items.FindByValue(value.SearchBy.ToString)) Then
                Me.DDLsearchUserBy.SelectedValue = value.SearchBy.ToString
            End If
            Me.ViewState("CurrentFilter") = value
        End Set
    End Property
    Private Property Ascending As Boolean Implements IViewSelectUsersForMessageService.Ascending
        Get
            Return ViewStateOrDefault("Ascending", True)
        End Get
        Set(value As Boolean)
            ViewState("Ascending") = value
        End Set
    End Property
    Private Property LoadedNoUsers As Boolean Implements IViewSelectUsersForMessageService.LoadedNoUsers
        Get
            Return ViewStateOrDefault("LoadedNoUsers", False)
        End Get
        Set(value As Boolean)
            ViewState("LoadedNoUsers") = value
        End Set
    End Property

    Private Property CurrentStartWith As String Implements IViewSelectUsersForMessageService.CurrentStartWith
        Get
            Return CTRLalphabetSelector.SelectedItem
        End Get
        Set(value As String)
            CTRLalphabetSelector.SelectedItem = value
        End Set
    End Property

    Private Property CurrentOrderBy As UserByMessagesOrder Implements IViewSelectUsersForMessageService.CurrentOrderBy
        Get
            Return ViewStateOrDefault("CurrentOrderBy", UserByMessagesOrder.ByUser)
        End Get
        Set(value As UserByMessagesOrder)
            Me.ViewState("CurrentOrderBy") = value
        End Set
    End Property
    Private Property SelectedSearchBy As lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy Implements IViewSelectUsersForMessageService.SelectedSearchBy
        Get
            If DDLsearchUserBy.SelectedIndex = -1 Then
                Return lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy).GetByString(Me.DDLsearchUserBy.SelectedValue, lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy.All)
            End If
        End Get
        Set(value As lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy)
            If DDLsearchUserBy.SelectedIndex > -1 AndAlso Me.DDLsearchUserBy.SelectedValue <> value.ToString AndAlso Not IsNothing(Me.DDLsearchUserBy.Items.FindByValue(value.ToString)) Then
                Me.DDLsearchUserBy.SelectedValue = value.ToString
            End If
        End Set
    End Property
    Private Property CurrentObject As lm.Comol.Core.DomainModel.ModuleObject Implements IViewSelectUsersForMessageService.CurrentObject
        Get
            If Not IsNothing(ViewState("CurrentModuleObject")) Then
                Try
                    Return DirectCast(ViewState("CurrentModuleObject"), ModuleObject)
                Catch ex As Exception
                    Return Nothing
                End Try
            Else
                Return Nothing
            End If
        End Get
        Set(value As ModuleObject)
            ViewState("CurrentModuleObject") = value
        End Set
    End Property
    Private Property SelectedIdAgency As Long Implements IViewSelectUsersForMessageService.SelectedIdAgency
        Get
            If DDLcommunityAgencies.SelectedIndex = -1 Then
                Return -1
            Else
                Return CLng(DDLcommunityAgencies.SelectedValue)
            End If
        End Get
        Set(value As Long)
            If DDLcommunityAgencies.Items.Count > 0 AndAlso Not IsNothing(DDLcommunityAgencies.Items.FindByValue(value)) Then
                Me.DDLcommunityAgencies.SelectedValue = value
            End If
        End Set
    End Property
    Private ReadOnly Property AnonymousUserTranslation As String Implements IViewSelectUsersForMessageService.AnonymousUserTranslation
        Get
            Return Resource.getValue("AnonymousUserTranslation")
        End Get
    End Property
    Private ReadOnly Property UnknownUserTranslation As String Implements IViewSelectUsersForMessageService.UnknownUserTranslation
        Get
            Return Resource.getValue("UnknownUser")
        End Get
    End Property

    Private Property SelectedItems As List(Of lm.Comol.Core.Mail.Messages.dtoModuleRecipientItem) Implements IViewSelectUsersForMessageService.SelectedItems
        Get
            Return ViewStateOrDefault("SelectedItems", New List(Of lm.Comol.Core.Mail.Messages.dtoModuleRecipientItem))
        End Get
        Set(value As List(Of lm.Comol.Core.Mail.Messages.dtoModuleRecipientItem))
            ViewState("SelectedItems") = value
        End Set
    End Property
    Private Property IdCommunityContainer As Integer Implements IViewSelectUsersForMessageService.IdCommunityContainer
        Get
            Return ViewStateOrDefault("IdCommunityContainer", 0)
        End Get
        Set(value As Integer)
            ViewState("IdCommunityContainer") = value
        End Set
    End Property
    Private Property IdModuleContainer As Integer Implements IViewSelectUsersForMessageService.IdModuleContainer
        Get
            Return ViewStateOrDefault("IdModuleContainer", 0)
        End Get
        Set(value As Integer)
            ViewState("IdModuleContainer") = value
        End Set
    End Property
    Private Property CodeModuleContainer As String Implements IViewSelectUsersForMessageService.CodeModuleContainer
        Get
            Return ViewStateOrDefault("CodeModuleContainer", "")
        End Get
        Set(value As String)
            ViewState("CodeModuleContainer") = value
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgrid.Pager = Pager
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EditCall", "Modules", "CallForPapers")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBselectUsersFrom_t)
            .setLabel(LBselectMessages_t)
            .setLinkButton(LNBselectMessages, False, True)
            .setLabel(LBsearchUserByFilter_t)
            .setLabel(LBcommunityAgencyFilter_t)
            .setLabel(LBsubmissionStatusFilter_t)
            .setLabel(LBsubmissionTypeFilter_t)
            .setButton(BTNapplyFilters, True)
            .setButton(BTNcloseMessageSelectorWindow, True)
            DVdialogMessages.Attributes.Add("title", Resource.getValue("DVdialogMessages.Title"))
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(type As CallForPaperType, fromPortal As Boolean, idCommunity As Integer, obj As ModuleObject, idTemplate As Long, idVersion As Long, isTemplateCompliant As Boolean, Optional translation As List(Of lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation) = Nothing) Implements IViewSelectUsersForMessageService.InitializeControl
        LBselectMessages.Text = Me.Resource.getValue("LBselectMessages.0")
        Me.CurrentPresenter.InitView(type, RemoveUsers, fromPortal, idCommunity, obj, idTemplate, idVersion, isTemplateCompliant, translation)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewSelectUsersForMessageService.DisplaySessionTimeout
        'Me.RPTitems.DataSource = 
        Me.BTNapplyFilters.Enabled = False
        Me.CTRLalphabetSelector.RaiseSelectionEvent = False
        Me.LNBselectMessages.Enabled = False
    End Sub

#Region "Filters"
    Private Sub InitializeMessagesSelector(fromPortal As Boolean, idCommunity As Integer, modulecode As String, idModule As Integer, obj As ModuleObject) Implements IViewSelectUsersForMessageService.InitializeMessagesSelector
        CTRLselectMessages.InitializeControl(fromPortal, idCommunity, modulecode, idModule, obj)
    End Sub
    Private Sub InitializeWordSelector(availableWords As List(Of String)) Implements IViewSelectUsersForMessageService.InitializeWordSelector
        Me.DVletters.Visible = True
        Me.CTRLalphabetSelector.InitializeControl(availableWords)
    End Sub
    Private Sub InitializeWordSelector(availableWords As List(Of String), activeWord As String) Implements IViewSelectUsersForMessageService.InitializeWordSelector
        Me.DVletters.Visible = True
        Me.CTRLalphabetSelector.InitializeControl(availableWords, activeWord)
    End Sub
    Private Sub LoadSearchProfilesBy(list As List(Of lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy), defaultSearch As lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy) Implements IViewSelectUsersForMessageService.LoadSearchProfilesBy
        Dim translations As List(Of TranslatedItem(Of String)) = (From s In list Select New TranslatedItem(Of String) With {.Id = s.ToString, .Translation = Me.Resource.getValue("SearchProfilesBy." & s.ToString)}).ToList

        Me.DDLsearchUserBy.DataSource = translations
        Me.DDLsearchUserBy.DataValueField = "Id"
        Me.DDLsearchUserBy.DataTextField = "Translation"
        Me.DDLsearchUserBy.DataBind()

        Me.SelectedSearchBy = defaultSearch
    End Sub
    Private Sub LoadAgencies(items As Dictionary(Of Long, String), idDefaultAgency As Long) Implements IViewSelectUsersForMessageService.LoadAgencies
        Me.DDLcommunityAgencies.Items.Clear()
        Me.DDLcommunityAgencies.DataSource = items
        Me.DDLcommunityAgencies.DataValueField = "Key"
        Me.DDLcommunityAgencies.DataTextField = "Value"
        Me.DDLcommunityAgencies.DataBind()

        If items.Count = 0 Then
            Me.DDLcommunityAgencies.Items.Insert(0, New ListItem(Me.Resource.getValue("agencies.ignorefilter"), -1))
        Else
            If items.Count > 1 Then
                Me.DDLcommunityAgencies.Items.Insert(0, New ListItem(Me.Resource.getValue("agencies.all"), -2))
            End If
            Me.DDLcommunityAgencies.Items.Insert(0, New ListItem(Me.Resource.getValue("agencies.noemployeee"), -3))
            Me.DDLcommunityAgencies.Items.Insert(0, New ListItem(Me.Resource.getValue("agencies.ignorefilter"), -1))
        End If
        Me.LBcommunityAgencyFilter_t.Visible = True
        Me.DDLcommunityAgencies.Visible = True
    End Sub
    Private Sub LoadAvailableStatus(items As List(Of lm.Comol.Modules.CallForPapers.Domain.SubmissionFilterStatus), selected As lm.Comol.Modules.CallForPapers.Domain.SubmissionFilterStatus) Implements IViewSelectUsersForMessageService.LoadAvailableStatus
        Me.DDLsubmissionStatus.Items.Clear()

        Dim types As New Dictionary(Of String, String)
        For Each name As SubmissionFilterStatus In items
            types.Add(name.ToString, Me.Resource.getValue("SubmissionFilterStatus." & name.ToString))
        Next
        Me.DDLsubmissionStatus.DataSource = types.OrderBy(Function(t) t.Value).ToDictionary(Function(t) t.Key, Function(t) t.Value)
        Me.DDLsubmissionStatus.DataTextField = "Value"
        Me.DDLsubmissionStatus.DataValueField = "Key"
        Me.DDLsubmissionStatus.DataBind()

        If types.Count > 0 Then
            Me.DDLsubmissionStatus.Items.Insert(0, New ListItem(Me.Resource.getValue("SubmissionFilterStatus." & SubmissionFilterStatus.All.ToString), SubmissionFilterStatus.All.ToString))
        End If
        If types.ContainsKey(selected) Then
            Me.DDLsubmissionStatus.SelectedValue = selected
        End If
    End Sub
    Private Sub LoadSubmitterstype(submitters As Dictionary(Of Long, String), dSubmitter As Long) Implements IViewSelectUsersForMessageService.LoadSubmittersType
        Me.DDLsubmitterTypes.DataSource = submitters
        Me.DDLsubmitterTypes.DataTextField = "Value"
        Me.DDLsubmitterTypes.DataValueField = "Key"
        Me.DDLsubmitterTypes.DataBind()
        If submitters.Count > 0 Then
            Me.DDLsubmitterTypes.Items.Insert(0, New ListItem(Me.Resource.getValue("DDLsubmitterTypes.-1"), -1))
        End If
        If submitters.ContainsKey(dSubmitter) Then
            Me.DDLsubmitterTypes.SelectedValue = dSubmitter
        End If
    End Sub
    Private Sub UnLoadAgencies() Implements IViewSelectUsersForMessageService.UnLoadAgencies
        Me.DDLcommunityAgencies.Visible = False
        Me.LBcommunityAgencyFilter_t.Visible = False
    End Sub
    Private Sub DisplayNoSubmittersFilter() Implements IViewSelectUsersForMessageService.DisplayNoSubmittersFilter
        Me.MLVfilters.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplaySubmittersFilter() Implements IViewSelectUsersForMessageService.DisplaySubmittersFilter
        Me.MLVfilters.SetActiveView(VIWparticipantFilters)
    End Sub
    Private Sub LoadAccessType(types As List(Of AccessTypeFilter), selected As AccessTypeFilter) Implements IViewSelectUsersForMessageService.LoadAccessType
        Dim translations As List(Of TranslatedItem(Of String)) = (From s In types Select New TranslatedItem(Of String) With {.Id = s.ToString, .Translation = Me.Resource.getValue("AccessTypeFilter." & CallType.ToString & "." & s.ToString)}).ToList

        Me.DDLselectFrom.DataSource = translations
        Me.DDLselectFrom.DataValueField = "Id"
        Me.DDLselectFrom.DataTextField = "Translation"
        Me.DDLselectFrom.DataBind()

        Me.DDLselectFrom.SelectedValue = selected.ToString
    End Sub
    Public Function GetSelectedRecipients() As List(Of lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient) Implements IViewSelectUsersForMessageService.GetSelectedRecipients
        Return Me.CurrentPresenter.GetSelectedRecipients()
    End Function
#End Region

    Private Sub DisplayNoUsersFound() Implements IViewSelectUsersForMessageService.DisplayNoUsersFound
        Me.RPTitems.DataSource = New List(Of dtoCallMessageRecipient)
        Me.RPTitems.DataBind()
    End Sub

    Private Sub LoadUsers(recipients As List(Of dtoCallMessageRecipient)) Implements IViewSelectUsersForMessageService.LoadUsers
        Me.RPTitems.DataSource = recipients
        Me.RPTitems.DataBind()
    End Sub
    Private Function GetCurrentSelectedItems() As List(Of dtoSelectItem(Of lm.Comol.Core.Mail.Messages.dtoModuleRecipientItem)) Implements IViewSelectUsersForMessageService.GetCurrentSelectedItems
        Dim items As New List(Of dtoSelectItem(Of lm.Comol.Core.Mail.Messages.dtoModuleRecipientItem))

        For Each row As RepeaterItem In Me.RPTitems.Items
            Dim oCheck As HtmlInputCheckBox = row.FindControl("CBXuser")
            Dim item As New lm.Comol.Core.Mail.Messages.dtoModuleRecipientItem
            Dim oLiteral As Literal = row.FindControl("LTidPerson")
            item.IdPerson = CInt(oLiteral.Text)
            oLiteral = row.FindControl("LTidUserModule")
            item.IdUserModule = CLng(oLiteral.Text)
            oLiteral = row.FindControl("LTidModuleObject")
            item.IdModuleObject = CLng(oLiteral.Text)
            Dim oLabel As Label = row.FindControl("LBmailAddress")
            item.MailAddress = oLabel.Text
            items.Add(New dtoSelectItem(Of lm.Comol.Core.Mail.Messages.dtoModuleRecipientItem)() With {.Id = item, .Selected = oCheck.Checked})
        Next
        Return items
    End Function
#End Region

#Region "Control"
    Public ReadOnly Property isColumnVisible(ByVal column As ColumnMessageGrid) As Boolean
        Get
            Return AvailableColumns.Contains(column)
        End Get
    End Property
    Public ReadOnly Property RowItemCss(ByVal item As dtoCallMessageRecipient) As String
        Get
            If item.IsSubmitter Then
                Return "moduleobjectuser"
            ElseIf item.IsInternal Then
                Return "internaluser"
            Else
                Return "externaluser"
            End If
        End Get
    End Property

    Private Sub BTNapplyFilters_Click(sender As Object, e As System.EventArgs) Handles BTNapplyFilters.Click
        Dim dto As dtoUsersByMessageFilter = SelectedFilter
        Me.CurrentFilter = dto
        Me.CurrentPresenter.LoadItems(dto, 0, UsersPageSize, False)
    End Sub
    Private Sub DDLselectFrom_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLselectFrom.SelectedIndexChanged
        Me.CurrentPresenter.ChangeAccessFilter(lm.Comol.Core.DomainModel.Helpers.EnumParser(Of AccessTypeFilter).GetByString(Me.DDLselectFrom.SelectedValue, AccessTypeFilter.NoSubmitters))
    End Sub
    Private Sub CTRLalphabetSelector_SelectItem(letter As String) Handles CTRLalphabetSelector.SelectItem
        Dim dto As dtoUsersByMessageFilter = Me.SelectedFilter
        dto.StartWith = letter
        Me.CurrentStartWith = letter
        Me.CurrentFilter = dto
        Me.CurrentPresenter.LoadItems(dto, 0, UsersPageSize, False)
    End Sub
    Private Sub RPTitems_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTitems.ItemCommand
        Select Case e.CommandName
            Case "orderby"
                Dim filter As dtoUsersByMessageFilter = CurrentFilter
                Ascending = e.CommandArgument.contains("True")
                filter.Ascending = e.CommandArgument.contains("True")
                filter.OrderBy = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of UserByMessagesOrder).GetByString(e.CommandArgument.replace("." & filter.Ascending.ToString, ""), UserByMessagesOrder.ByUser)
                Me.CurrentPresenter.LoadItems(filter, Me.Pager.PageIndex, Me.Pager.PageSize, False)
            Case "all"
                Me.CurrentPresenter.EditItemsSelection(True)
            Case "none"
                Me.CurrentPresenter.EditItemsSelection(False)
        End Select
    End Sub

    Private Sub RPTitems_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTitems.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As dtoCallMessageRecipient = DirectCast(e.Item.DataItem, dtoCallMessageRecipient)
            Dim oCheck As HtmlInputCheckBox = e.Item.FindControl("CBXuser")

            oCheck.Checked = SelectAll OrElse SelectedItems.Where(Function(i) i.IdPerson = dto.IdPerson AndAlso i.IdUserModule = dto.IdUserModule AndAlso i.IdModuleObject = dto.IdModuleObject).Any()
            Dim oLabel As Label = e.Item.FindControl("LBstatus")
            If dto.IsSubmitter Then
                oLabel.CssClass = "icon status "
                Dim oLiteral As Literal = e.Item.FindControl("LTsubStatus")

                ''If revStatus = RevisionStatus.None OrElse revStatus = RevisionStatus.Approved Then
                oLabel.CssClass &= "submission " & dto.Status.ToString.ToLower
                oLabel.ToolTip = Resource.getValue("SubmissionStatus." & dto.Status.ToString())
                oLiteral.Text = Resource.getValue("SubmissionStatus." & dto.Status.ToString())
                oLabel.Visible = True
                oLiteral.Visible = True
            End If

            'Dim ModifiedBy As String

            'ModifiedBy = dto.UserDisplayName
            'oLiteral.Text = ModifiedBy

            'oLiteral = e.Item.FindControl("LTtemplateType")
            'oLiteral.Text = Resource.getValue("TemplateType.Translations." & dto.Type.ToString)

            ''oLiteral = e.Item.FindControl("LTmoduleName")
            ''oLiteral.Text = ModuleFileStatistics


            'Dim olabel As Label = e.Item.FindControl("LBlastEditOn")
            'If dto.ModifiedOn.HasValue Then
            '    olabel.Text = FormatDateTime(dto.ModifiedOn.Value, DateFormat.ShortDate) & " " & FormatDateTime(dto.ModifiedOn.Value, DateFormat.ShortTime)
            '    olabel.ToolTip = dto.ModifiedByName
            'Else
            '    olabel.Text = "&nbsp;"
            'End If

            'Dim cContext As lm.Comol.Core.TemplateMessages.Domain.dtoModuleContext = ContainerContext
            Dim oHyperlink As HyperLink = e.Item.FindControl("HYPdisplayMessages")
            oHyperlink.Visible = (dto.MessageNumber > 0)
            oHyperlink.NavigateUrl = PageUtility.ApplicationUrlBase() & lm.Comol.Core.Mail.Messages.RootObject.ViewMessage(dto.IdPerson, dto.IdUserModule, PageUtility.GetUrlEncoded(dto.MailAddress), CodeModuleContainer, IdCommunityContainer, IdModuleContainer, CurrentObject)
            Me.Resource.setHyperLink(oHyperlink, True, True)
            'oHyperlink.Visible = dto.Permission.AllowEdit
            'oHyperlink.NavigateUrl = BaseUrl & lm.Comol.Core.TemplateMessages.RootObject.EditByStep(CurrentSessionId, dto.Type, dto.OwnerInfo, WizardTemplateStep.Settings, cContext.IdCommunity, cContext.ModuleCode, cContext.ModulePermissions, cContext.BackUrl, dto.Id, dto.IdLastVersion)

            'oHyperlink = e.Item.FindControl("HYPtemplatePreview")
            'Me.Resource.setHyperLink(oHyperlink, True, True)
            'oHyperlink.Visible = dto.Permission.AllowUse
            'oHyperlink.NavigateUrl = BaseUrl & lm.Comol.Core.TemplateMessages.RootObject.EditByStep(CurrentSessionId, dto.Type, dto.OwnerInfo, WizardTemplateStep.Settings, cContext.IdCommunity, cContext.ModuleCode, cContext.ModulePermissions, cContext.BackUrl, dto.Id, dto.IdLastVersion, True)

            'oHyperlink = e.Item.FindControl("HYPtemplateEditPermissions")
            'Me.Resource.setHyperLink(oHyperlink, True, True)
            'oHyperlink.Visible = dto.Permission.AllowChangePermission
            'oHyperlink.NavigateUrl = BaseUrl & lm.Comol.Core.TemplateMessages.RootObject.EditByStep(CurrentSessionId, dto.Type, dto.OwnerInfo, WizardTemplateStep.Permission, cContext.IdCommunity, cContext.ModuleCode, cContext.ModulePermissions, cContext.BackUrl, dto.Id, dto.IdLastVersion)


            'Dim oLinkbutton As LinkButton = e.Item.FindControl("LNBtemplateDelete")
            'Resource.setLinkButton(oLinkbutton, True, True)
            'oLinkbutton.Visible = dto.Permission.AllowDelete

            'oLinkbutton = e.Item.FindControl("LNBtemplateVirtualDelete")
            'Resource.setLinkButton(oLinkbutton, True, True)
            'oLinkbutton.Visible = dto.Permission.AllowVirtualDelete

            'oLinkbutton = e.Item.FindControl("LNBtemplateRecover")
            'oLinkbutton.Visible = dto.Permission.AllowUnDelete
            'Resource.setLinkButton(oLinkbutton, True, True)

            'oLinkbutton = e.Item.FindControl("LNBtemplateclone")
            'oLinkbutton.Visible = dto.Permission.AllowClone
            'Resource.setLinkButton(oLinkbutton, True, True)

            'oLinkbutton = e.Item.FindControl("LNBtemplateNewVersion")
            'oLinkbutton.Visible = dto.Permission.AllowNewVersion
            'Resource.setLinkButton(oLinkbutton, True, True)

            'Dim allowSendMail As Boolean = (dto.Permission.AllowUse AndAlso RaiseTemplateSelection AndAlso dto.Versions.Where(Function(v) v.Id = dto.IdLastVersion AndAlso v.Status = TemplateStatus.Active).Any())

            'oLinkbutton = e.Item.FindControl("LNBsendMail")
            'oLinkbutton.Visible = allowSendMail
            'Resource.setLinkButton(oLinkbutton, True, True)

            'oLiteral.Visible = Not (allowSendMail OrElse dto.Permission.AllowClone OrElse dto.Permission.AllowDelete OrElse dto.Permission.AllowEdit OrElse dto.Permission.AllowUnDelete OrElse dto.Permission.AllowUse OrElse dto.Permission.AllowVirtualDelete)

        ElseIf e.Item.ItemType = ListItemType.Footer Then
            Dim oTableItem As HtmlControl = e.Item.FindControl("TRempty")
            oTableItem.Visible = (RPTitems.Items.Count = 0)
            If (RPTitems.Items.Count = 0) Then
                Dim oTableCell As HtmlTableCell = e.Item.FindControl("TDemptyItems")

                oTableCell.ColSpan = 4 + AvailableColumns.Where(Function(c) c = ColumnMessageGrid.AgencyName OrElse c = ColumnMessageGrid.SubmissionStatus OrElse c = ColumnMessageGrid.SubmissionType).Count

                Dim oLabel As Label = e.Item.FindControl("LBemptyItems")
                If LoadedNoUsers Then
                    oLabel.Text = Resource.getValue("NoUsersForSelection")
                Else
                    Resource.setLabel(oLabel)
                End If

            End If

            If RPTitems.Items.Count < 2 Then
                Dim oLinkButton As LinkButton
                For Each name As String In [Enum].GetNames(GetType(UserByMessagesOrder))
                    oLinkButton = RPTitems.Controls(0).FindControl("LNBorder" & name & "Up")
                    If Not IsNothing(oLinkButton) Then
                        oLinkButton.Visible = False
                    End If
                    oLinkButton = RPTitems.Controls(0).FindControl("LNBorder" & name & "Down")
                    If Not IsNothing(oLinkButton) Then
                        oLinkButton.Visible = False
                    End If
                Next
            ElseIf RPTitems.Items.Count > 2 AndAlso RPTitems.Controls(0).FindControl("LNBorder" & UserByMessagesOrder.ByUser.ToString & "Up").Visible = False Then
                Dim oLinkButton As LinkButton
                For Each name As String In [Enum].GetNames(GetType(UserByMessagesOrder))
                    oLinkButton = RPTitems.Controls(0).FindControl("LNBorder" & name & "Up")
                    If Not IsNothing(oLinkButton) Then
                        oLinkButton.Visible = True
                    End If
                    oLinkButton = RPTitems.Controls(0).FindControl("LNBorder" & name & "Down")
                    If Not IsNothing(oLinkButton) Then
                        oLinkButton.Visible = True
                    End If
                Next
            End If
        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oLinkbutton As LinkButton = e.Item.FindControl("LNBselectRecipientsIntoAllPages")
            Resource.setLinkButton(oLinkbutton, True, True)

            oLinkbutton = e.Item.FindControl("LNBunselectRecipientsIntoAllPages")
            Resource.setLinkButton(oLinkbutton, True, True)

            Dim oControl As HtmlControl = e.Item.FindControl("DVselectorAll")
            oControl.Visible = (Pager.Count > UsersPageSize)

            oControl = e.Item.FindControl("DVselectorNone")
            oControl.Visible = (Pager.Count > UsersPageSize)

            Dim oLabel As Label = e.Item.FindControl("LBusernameHeader_t")
            Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBuserAgencyNameHeader_t")
            Resource.setLabel(oLabel)

            Dim oLiteral As Literal = e.Item.FindControl("LTsubPartecipantType_t")
            Resource.setLiteral(oLiteral)

            oLiteral = e.Item.FindControl("LTsubStatus_t")
            Resource.setLiteral(oLiteral)

            oLiteral = e.Item.FindControl("LTmessages_t")
            Resource.setLiteral(oLiteral)

            oLiteral = e.Item.FindControl("LTuserMessageActions_t")
            Resource.setLiteral(oLiteral)

            For Each name As String In [Enum].GetNames(GetType(UserByMessagesOrder))
                oLinkbutton = e.Item.FindControl("LNBorder" & name & "Up")
                If Not IsNothing(oLinkbutton) Then
                    oLinkbutton.ToolTip = Resource.getValue("UserByMessagesOrder.Ascending") & Resource.getValue("UserByMessagesOrder." & name)
                End If
                oLinkbutton = e.Item.FindControl("LNBorder" & name & "Down")
                If Not IsNothing(oLinkbutton) Then
                    oLinkbutton.ToolTip = Resource.getValue("UserByMessagesOrder.Descending") & Resource.getValue("UserByMessagesOrder." & name)
                End If
            Next
        End If
    End Sub
    Private Sub PGgrid_OnPageSelected() Handles PGgrid.OnPageSelected
        Me.CurrentPresenter.LoadItems(CurrentFilter, Me.Pager.PageIndex, UsersPageSize, False)
    End Sub
    Private Sub LNBselectMessages_Click(sender As Object, e As System.EventArgs) Handles LNBselectMessages.Click
        Me.DVdialogMessages.Visible = True
    End Sub
    Private Sub BTNcloseMessageSelectorWindow_Click(sender As Object, e As System.EventArgs) Handles BTNcloseMessageSelectorWindow.Click
        Dim count As Integer = Me.CTRLselectMessages.SelectedItems.Count
        Me.DVdialogMessages.Visible = False
        If count <= 1 Then
            LBselectMessages.Text = Me.Resource.getValue("LBselectMessages." & count.ToString)
        Else
            LBselectMessages.Text = String.Format(Me.Resource.getValue("LBselectMessages.n"), count)
        End If
    End Sub
#End Region

End Class