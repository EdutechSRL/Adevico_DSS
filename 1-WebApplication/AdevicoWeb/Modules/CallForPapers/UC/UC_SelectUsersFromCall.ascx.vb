Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic

Public Class UC_SelectUsersFromCall
    Inherits BaseControl
    Implements IViewSelectUsersFromCall

#Region "Context"
    Private _Presenter As SelectUsersFromCallPresenter
    Private ReadOnly Property CurrentPresenter() As SelectUsersFromCallPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New SelectUsersFromCallPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property SelectedIdCall As Long Implements IViewSelectUsersFromCall.SelectedIdCall
        Get
            Return ViewStateOrDefault("SelectedIdCall", 0)
        End Get
        Set(value As Long)
            ViewState("SelectedIdCall") = value
        End Set
    End Property
    'Private Property FromDate As Date? Implements IViewSelectUsersFromCall.FromDate
    '    Get
    '        Return Me.RDPstartDay.SelectedDate
    '    End Get
    '    Set(value As Date?)
    '        Me.RDPstartDay.SelectedDate = value
    '    End Set
    'End Property
    Public Property FromCommunities As List(Of Integer) Implements IViewSelectUsersFromCall.FromCommunities
        Get
            Return Me.ViewStateOrDefault("FromCommunities", New List(Of Integer))
        End Get
        Set(value As List(Of Integer))
            Me.ViewState("FromCommunities") = value
        End Set
    End Property
    Public Property isInitialized As Boolean Implements IViewSelectUsersFromCall.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public Property CallsType As lm.Comol.Modules.CallForPapers.Domain.CallForPaperType Implements IViewSelectUsersFromCall.CallsType
        Get
            Return Me.ViewStateOrDefault("CallsType", lm.Comol.Modules.CallForPapers.Domain.CallForPaperType.CallForBids)
        End Get
        Set(value As lm.Comol.Modules.CallForPapers.Domain.CallForPaperType)
            Me.ViewState("CallsType") = value

            Resource.setLabel_To_Value(LBemptyCallSelectorDescription, "LBemptyCallSelectorDescription." & value.ToString)
            Resource.setLabel_To_Value(LBcallSelectorDescription, "LBcallSelectorDescription." & value.ToString)
            Resource.setLabel_To_Value(LBcallname_t, "LBcallname_t." & value.ToString)

        End Set
    End Property
    Public Property FromPortal As Boolean Implements IViewSelectUsersFromCall.FromPortal
        Get
            Return Me.ViewStateOrDefault("FromPortal", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("FromPortal") = value
        End Set
    End Property
    Public Property RaiseEvents As Boolean Implements IViewSelectUsersFromCall.RaiseEvents
        Get
            Return Me.ViewStateOrDefault("RaiseEvents", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("RaiseEvents") = value
        End Set
    End Property
    Public Property RemoveUsers As List(Of Integer) Implements IViewSelectUsersFromCall.RemoveUsers
        Get
            Return Me.ViewStateOrDefault("RemoveUsers", New List(Of Integer))
        End Get
        Set(value As List(Of Integer))
            Me.ViewState("RemoveUsers") = value
        End Set
    End Property
    Private Property CallsPageSize As Int32 Implements IViewSelectUsersFromCall.CallsPageSize
        Get
            Return ViewStateOrDefault("CallsPageSize", 25)
        End Get
        Set(value As Int32)
            Me.ViewState("CallsPageSize") = value
        End Set
    End Property
    Private Property CallsPager As PagerBase Implements IViewSelectUsersFromCall.CallsPager
        Get
            Return ViewStateOrDefault("CallsPager", New lm.Comol.Core.DomainModel.PagerBase With {.PageSize = CallsPageSize})
        End Get
        Set(value As PagerBase)
            Me.ViewState("CallsPager") = value
            Me.PGcallsPager.Pager = value
            Me.PGcallsPager.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
            'Me.DIVpageSize.Visible = (Not value.Count < Me.DefaultPageSize)
        End Set
    End Property

#Region "Submissions"
    Private ReadOnly Property UnknownCommunityName As String Implements IViewSelectUsersFromCall.UnknownCommunityName
        Get
            Return Me.Resource.getValue("UnknownCommunity")
        End Get
    End Property
    Private ReadOnly Property Portalname As String Implements IViewSelectUsersFromCall.PortalName
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property SelectedFilter As dtoFilterSubmissions Implements IViewSelectUsersFromCall.SelectedFilter
        Get
            Dim oFilter As dtoFilterSubmissions = ViewStateOrDefault("CurrentFilter", New dtoFilterSubmissions() With {.Ascending = True, .OrderBy = SubmissionsOrder.ByUser})
            If (oFilter.TranslationsSubmission.Count = 0) Then
                For Each name As String In [Enum].GetNames(GetType(SubmissionFilterStatus))
                    oFilter.TranslationsSubmission.Add([Enum].Parse(GetType(SubmissionFilterStatus), name), Me.Resource.getValue("SubmissionFilterStatus." & name))
                Next
            End If
            If Me.DDLfilterSubmitterTypes.Items.Count = 0 Then
                oFilter.IdSubmitterType = -1
            Else
                oFilter.IdSubmitterType = CLng(Me.DDLfilterSubmitterTypes.SelectedValue)
            End If
            If Me.DDLfilterStatus.Items.Count > 0 Then
                oFilter.Status = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SubmissionFilterStatus).GetByString(Me.DDLfilterStatus.SelectedValue, SubmissionFilterStatus.All)
            Else
                oFilter.Status = SubmissionFilterStatus.All
            End If
            If Not String.IsNullOrEmpty(TXBsubmitterName.Text) Then
                oFilter.SearchForName = TXBsubmitterName.Text.Trim
            End If

            Return oFilter
        End Get
    End Property
    Private Property CurrentFilter As dtoFilterSubmissions Implements IViewSelectUsersFromCall.CurrentFilter
        Get
            Dim filter As dtoFilterSubmissions = ViewStateOrDefault("CurrentFilter", SelectedFilter)
            If (filter.TranslationsSubmission.Count = 0) Then
                For Each name As String In [Enum].GetNames(GetType(SubmissionFilterStatus))
                    filter.TranslationsSubmission.Add([Enum].Parse(GetType(SubmissionFilterStatus), name), Me.Resource.getValue("SubmissionFilterStatus." & name))
                Next
            End If

            Return filter
        End Get
        Set(value As dtoFilterSubmissions)
            Me.TXBsubmitterName.Text = value.SearchForName
            If DDLfilterStatus.SelectedIndex > -1 AndAlso Me.DDLfilterStatus.SelectedValue <> value.Status.ToString AndAlso Not IsNothing(Me.DDLfilterStatus.Items.FindByValue(value.Status.ToString)) Then
                Me.DDLfilterStatus.SelectedValue = value.Status.ToString
            End If
            If DDLfilterSubmitterTypes.SelectedIndex > -1 AndAlso Me.DDLfilterSubmitterTypes.SelectedValue <> value.IdSubmitterType.ToString AndAlso Not IsNothing(Me.DDLfilterSubmitterTypes.Items.FindByValue(value.IdSubmitterType.ToString)) Then
                Me.DDLfilterSubmitterTypes.SelectedValue = value.IdSubmitterType
            End If
            Me.ViewState("CurrentFilter") = value
        End Set
    End Property
    Private Property SelectAllSubmissions As Boolean Implements IViewSelectUsersFromCall.SelectAllSubmissions
        Get
            Return ViewStateOrDefault("SelectAllSubmissions", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("SelectAllSubmissions") = value
        End Set
    End Property
    Private Property SelectedSubmissions As List(Of Long) Implements IViewSelectUsersFromCall.SelectedSubmissions
        Get
            Return ViewStateOrDefault("SelectedSubmissions", New List(Of Long))
        End Get
        Set(value As List(Of Long))
            Me.ViewState("SelectedSubmissions") = value
        End Set
    End Property

    Private Property SubmissionsPageSize As Int32 Implements IViewSelectUsersFromCall.SubmissionsPageSize
        Get
            Return ViewStateOrDefault("SubmissionsPageSize", 25)
        End Get
        Set(value As Int32)
            Me.ViewState("SubmissionsPageSize") = value
        End Set
    End Property
    Private Property SubmissionsPager As PagerBase Implements IViewSelectUsersFromCall.SubmissionsPager
        Get
            Return ViewStateOrDefault("SubmissionsPager", New lm.Comol.Core.DomainModel.PagerBase With {.PageSize = SubmissionsPageSize})
        End Get
        Set(value As PagerBase)
            Me.ViewState("SubmissionsPager") = value
            Me.PGsubmissionsPager.Pager = value
            Me.PGsubmissionsPager.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
            'Me.DIVpageSize.Visible = (Not value.Count < Me.DefaultPageSize)
        End Set
    End Property
#End Region

#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region
    Public Event CloseWindow()
    Public Event SelectedIdUsers(idUsers As List(Of Integer))

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGcallsPager.Pager = CallsPager
        Me.PGsubmissionsPager.Pager = SubmissionsPager
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EditCall", "Modules", "CallForPapers")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setButton(BTNemptyCloseCallSelector, True)
            .setButton(BTNcloseCallSelector, True)
            .setButton(BTNundoCallSelection, True)
            .setLabel(LBsubmissionsSelectorDescription)
            .setButton(BTNchangeSelectedCall, True)
            .setLabel(LBfilterSubmissionsStatus_t)
            .setLabel(LBfilterSubmissionsType_t)
            .setLabel(LBfilterSubmitterName)
            .setButton(BTNapplySubmissionsFilters, True)
            .setButton(BTNcloseCallSumbissionsSelector, True)
            .setButton(BTNaddSubmissions, True)
            .setLabel(LBnoCallSubmissions)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(type As CallForPaperType, fromPortal As Boolean, fromCommunities As List(Of Integer), removeUsers As List(Of Integer)) Implements IViewSelectUsersFromCall.InitializeControl
        CurrentPresenter.InitView(type, fromPortal, fromCommunities, removeUsers)
    End Sub
   
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission
        Me.MLVsubmissions.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
        Me.MLVsubmissions.SetActiveView(VIWempty)
    End Sub

#Region "Calls view"
    Private Sub LoadCalls(calls As List(Of lm.Comol.Modules.CallForPapers.Domain.dtoCallInfo)) Implements IViewSelectUsersFromCall.LoadCalls
        Me.MLVsubmissions.SetActiveView(VIWselectCall)
        Me.RPTcalls.DataSource = calls
        Me.RPTcalls.DataBind()
    End Sub
    Private Sub DisplayNoCallsAvailable() Implements IViewSelectUsersFromCall.DisplayNoCallsAvailable
        Me.MLVsubmissions.SetActiveView(VIWempty)
    End Sub
    Private Sub LoadAvailableCalls() Implements IViewSelectUsersFromCall.LoadAvailableCalls
        Me.MLVsubmissions.SetActiveView(VIWselectCall)
    End Sub
#End Region

#Region "Submissions view"
    Private Sub DisplayCallSubmissions(callName As String) Implements IViewSelectUsersFromCall.DisplayCallSubmissions
        Me.MLVsubmissions.SetActiveView(VIWselectItems)
        Me.LBcallname.Text = callName
    End Sub
    Private Sub LoadAvailableStatus(items As List(Of SubmissionFilterStatus), selected As SubmissionFilterStatus) Implements IViewSelectUsersFromCall.LoadAvailableStatus
        Me.DDLfilterStatus.Items.Clear()

        Dim types As New Dictionary(Of String, String)
        For Each name As SubmissionFilterStatus In items
            types.Add(name.ToString, Me.Resource.getValue("SubmissionFilterStatus." & name.ToString))
        Next
        Me.DDLfilterStatus.DataSource = types.OrderBy(Function(t) t.Value).ToDictionary(Function(t) t.Key, Function(t) t.Value)
        Me.DDLfilterStatus.DataTextField = "Value"
        Me.DDLfilterStatus.DataValueField = "Key"
        Me.DDLfilterStatus.DataBind()

        If types.Count > 1 Then
            Me.DDLfilterStatus.Items.Insert(0, New ListItem(Me.Resource.getValue("SubmissionFilterStatus." & SubmissionFilterStatus.All.ToString), SubmissionFilterStatus.All.ToString))
        End If
        If types.ContainsKey(selected) Then
            Me.DDLfilterStatus.SelectedValue = selected
        End If
    End Sub
    Private Sub LoadSubmitterstype(submitters As Dictionary(Of Long, String), dSubmitter As Long) Implements IViewSelectUsersFromCall.LoadSubmitterstype
        Me.DDLfilterSubmitterTypes.DataSource = submitters
        Me.DDLfilterSubmitterTypes.DataTextField = "Value"
        Me.DDLfilterSubmitterTypes.DataValueField = "Key"
        Me.DDLfilterSubmitterTypes.DataBind()

        If submitters.Count > 1 Then
            Me.DDLfilterSubmitterTypes.Items.Insert(0, New ListItem(Me.Resource.getValue("DDLfilterSubmitterTypes.-1"), -1))
        End If
        If submitters.ContainsKey(dSubmitter) Then
            Me.DDLfilterSubmitterTypes.SelectedValue = dSubmitter
        End If
    End Sub
    Private Function GetCurrentSelectedItems() As List(Of dtoSelectItem(Of Long)) Implements IViewSelectUsersFromCall.GetCurrentSelectedItems
        Dim items As New List(Of dtoSelectItem(Of Long))

        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTcallSubmissions.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim oCheck As HtmlInputCheckBox = row.FindControl("CBXsubmission")
            Dim oLiteral As Literal = row.FindControl("LTidSubmission")
            items.Add(New dtoSelectItem(Of Long)() With {.Id = CLng(oLiteral.Text), .Selected = oCheck.Checked})

        Next
        Return items
    End Function
    Private Sub DisplayNoSubmissionsAvailable() Implements lm.Comol.Modules.CallForPapers.Presentation.IViewSelectUsersFromCall.DisplayNoSubmissionsAvailable
        LBnoCallSubmissions.Visible = True
        RPTcallSubmissions.Visible = False
        BTNaddSubmissions.Enabled = False
    End Sub
    Private Sub LoadSubmissions(submissions As List(Of dtoSubmissionDisplayItem)) Implements IViewSelectUsersFromCall.LoadSubmissions
        LBnoCallSubmissions.Visible = False
        RPTcallSubmissions.Visible = True
        Me.RPTcallSubmissions.DataSource = submissions
        Me.RPTcallSubmissions.DataBind()
        BTNaddSubmissions.Enabled = True
    End Sub
#End Region

#End Region

#Region "Empty View"
    Private Sub BTNcloseCallSelector_Click(sender As Object, e As System.EventArgs) Handles BTNcloseCallSelector.Click, BTNcloseCallSumbissionsSelector.Click, BTNemptyCloseCallSelector.Click
        If RaiseEvents Then
            RaiseEvent CloseWindow()
        End If
    End Sub
#End Region

#Region "Call View"
    Private Sub RPTcalls_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTcalls.ItemCommand
        Dim idCall As Long = CLng(e.CommandArgument)
        SelectAllSubmissions = False
        Me.SelectedSubmissions = New List(Of Long)
        Me.TXBsubmitterName.Text = ""
        Me.CurrentPresenter.SelectCall(idCall)
    End Sub
    Private Sub RPTcalls_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcalls.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            Dim oLiteral As Literal = e.Item.FindControl("LTcallNameTitle")
            Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTsubmissionsCountTitle")
            Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTsubscriptionsClosedOnTitle")
            Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTselectTitle")
            Resource.setLiteral(oLiteral)
        ElseIf e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oLinkbutton As LinkButton = e.Item.FindControl("LNBselectCall")
            Resource.setLinkButton(oLinkbutton, True, True)

            Dim oLiteral As Literal = e.Item.FindControl("LTsubscriptionsClosedOn")
            Dim item As dtoCallInfo = DirectCast(e.Item.DataItem, dtoCallInfo)

            If item.EndDate.HasValue Then
                oLiteral.Text = FormatDateTime(item.EndDate.Value, DateFormat.ShortDate) & " " & item.EndDate.Value.ToString("HH:mm:ss")
            Else
                oLiteral.Text = "-"
            End If
        End If
    End Sub
    Private Sub PGcallsPager_OnPageSelected() Handles PGcallsPager.OnPageSelected
        Me.CurrentPresenter.LoadCalls(PGcallsPager.Pager.PageIndex, CallsPageSize)
    End Sub
#End Region
#Region "Submissions View"
    Private Sub BTNchangeSelectedCall_Click(sender As Object, e As System.EventArgs) Handles BTNchangeSelectedCall.Click
        Me.CurrentPresenter.ChangeCall()
    End Sub
    Private Sub BTNapplySubmissionsFilters_Click(sender As Object, e As System.EventArgs) Handles BTNapplySubmissionsFilters.Click
        SelectAllSubmissions = False
        Me.TXBsubmitterName.Text = ""
        Me.SelectedSubmissions = New List(Of Long)
        Me.CurrentPresenter.LoadSubmissions(SelectedFilter, 0, SubmissionsPageSize)
    End Sub
    Private Sub BTNaddSubmissions_Click(sender As Object, e As System.EventArgs) Handles BTNaddSubmissions.Click
        If RaiseEvents Then
            RaiseEvent SelectedIdUsers(CurrentPresenter.GetSelectedUsers())
        End If
    End Sub
    Private Sub RPTcallSubmissions_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTcallSubmissions.ItemCommand
        Select Case e.CommandName
            Case "all"
                Me.CurrentPresenter.EditItemsSelection(True)
            Case "none"
                Me.CurrentPresenter.EditItemsSelection(False)
        End Select
    End Sub
    Private Sub RPTcallSubmissions_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcallSubmissions.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            Dim oLiteral As Literal = e.Item.FindControl("LTsubmissionTitle")
            Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTsubmissionTypeTitle")
            Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTsubmissionEvaluationTitle")
            Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTsubmittedOnTitle")
            Resource.setLiteral(oLiteral)

            Dim oLinkbutton As LinkButton = e.Item.FindControl("LNBselectItemsIntoAllPages")
            Resource.setLinkButton(oLinkbutton, True, True)

            oLinkbutton = e.Item.FindControl("LNBunselectItemsIntoAllPages")
            Resource.setLinkButton(oLinkbutton, True, True)

            Dim oControl As HtmlControl = e.Item.FindControl("DVselectorAll")
            oControl.Visible = (SubmissionsPager.Count > SubmissionsPageSize)

            oControl = e.Item.FindControl("DVselectorNone")
            oControl.Visible = (SubmissionsPager.Count > SubmissionsPageSize)
        ElseIf e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oLabel As Label = e.Item.FindControl("LBmark")
            Dim item As dtoSubmissionDisplayItem = DirectCast(e.Item.DataItem, dtoSubmissionDisplayItem)
            If item.HasEvaluations Then
                oLabel.Text = GetEvaluatedValue(item.SumRating)
            End If

            oLabel = e.Item.FindControl("LBlastActionOn")
            If item.LastActionOn.HasValue Then
                oLabel.Text = FormatDateTime(item.LastActionOn.Value, DateFormat.ShortDate) & " " & item.LastActionOn.Value.ToString("HH:mm:ss")
                oLabel.ToolTip = Resource.getValue("LastActionOn." & item.LastActionStatus.ToString)
            End If

        End If
    End Sub
    Private Function GetEvaluatedValue(sum As Double) As String
        Dim fractional As Double = sum - Math.Floor(sum)
        If (fractional = 0) Then
            Return String.Format("{0:N0}", sum)
        Else
            Return String.Format("{0:N2}", sum)
        End If
    End Function
    Private Sub PGsubmissionsPager_OnPageSelected() Handles PGsubmissionsPager.OnPageSelected
        Me.CurrentPresenter.LoadSubmissions(CurrentFilter, Me.PGsubmissionsPager.Pager.PageIndex, SubmissionsPageSize)
    End Sub
#End Region

End Class