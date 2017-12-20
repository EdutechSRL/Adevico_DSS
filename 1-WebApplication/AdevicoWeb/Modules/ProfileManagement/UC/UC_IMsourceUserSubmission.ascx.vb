Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation
Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Modules.CallForPapers.Domain

Public Class UC_IMsourceUserSubmission
    Inherits BaseControl
    Implements IViewCallFinder


#Region "Context"
    Private _Presenter As CallFinderPresenter
    Private ReadOnly Property CurrentPresenter() As CallFinderPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CallFinderPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private WriteOnly Property AllowPreview As Boolean Implements IViewCallFinder.AllowPreview
        Set(value As Boolean)
            Me.SPNpreview.Visible = value
        End Set
    End Property
    Public Property AvailableColumns As List(Of ProfileColumnComparer(Of String)) Implements IViewCallFinder.AvailableColumns
        Get
            Return ViewStateOrDefault("AvailableColumns", New List(Of ProfileColumnComparer(Of String)))
        End Get
        Set(value As List(Of ProfileColumnComparer(Of String)))
            ViewState("AvailableColumns") = value
        End Set
    End Property
    Public Property StandardAvailableColumns As List(Of ExternalColumnComparer(Of String, String)) Implements IViewCallFinder.StandardAvailableColumns
        Get
            Return ViewStateOrDefault("AvailableColumns", New List(Of ExternalColumnComparer(Of String, String)))
        End Get
        Set(value As List(Of ExternalColumnComparer(Of String, String)))
            ViewState("AvailableColumns") = value
        End Set
    End Property
    Public Property CurrentIdItem As Long Implements IViewCallFinder.CurrentIdItem
        Get
            Return ViewStateOrDefault("CurrentIdItem", 0)
        End Get
        Set(value As Long)
            ViewState("CurrentIdItem") = value
        End Set
    End Property
    Private Property FromDate As Date? Implements IViewCallFinder.FromDate
        Get
            Return Me.RDPstartDay.SelectedDate
        End Get
        Set(value As Date?)
            Me.RDPstartDay.SelectedDate = value
        End Set
    End Property
    Public Property FromIdCommunity As Integer Implements IViewCallFinder.FromIdCommunity
        Get
            Return Me.ViewStateOrDefault("FromIdCommunity", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("FromIdCommunity") = value
        End Set
    End Property
    Public Property isInitialized As Boolean Implements IViewCallFinder.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public Property isValid As Boolean Implements IViewCallFinder.isValid
        Get
            Return ViewStateOrDefault("isValid", False)
        End Get
        Set(value As Boolean)
            ViewState("isValid") = value
        End Set
    End Property
    Public Property AlsoAnonymousSubmissions As Boolean Implements IViewCallFinder.AlsoAnonymousSubmissions
        Get
            Return ViewStateOrDefault("AlsoAnonymousSubmissions", False)
        End Get
        Set(value As Boolean)
            ViewState("AlsoAnonymousSubmissions") = value
        End Set
    End Property

    Public Property OnlyAnonymousSubmissions As Boolean Implements IViewCallFinder.OnlyAnonymousSubmissions
        Get
            Return ViewStateOrDefault("OnlyAnonymousSubmissions", True)
        End Get
        Set(value As Boolean)
            ViewState("OnlyAnonymousSubmissions") = value
        End Set
    End Property
    Public Property ItemType As lm.Comol.Modules.CallForPapers.Domain.CallForPaperType Implements IViewCallFinder.ItemType
        Get
            Return Me.ViewStateOrDefault("ItemType", lm.Comol.Modules.CallForPapers.Domain.CallForPaperType.CallForBids)
        End Get
        Set(value As lm.Comol.Modules.CallForPapers.Domain.CallForPaperType)
            Me.ViewState("ItemType") = value
        End Set
    End Property
    Public Property Range As SearchRange Implements IViewCallFinder.Range
        Get
            Return Me.ViewStateOrDefault("OnlyFromPortal", SearchRange.Portal)
        End Get
        Set(value As SearchRange)
            Me.ViewState("Range") = value
        End Set
    End Property
    Private Property CurrentSubmitterId As Long Implements IViewCallFinder.CurrentSubmitterId
        Get
            If Me.DDLsubmitters.SelectedIndex < 0 Then
                Return CLng(0)
            Else
                Return Me.DDLsubmitters.SelectedValue
            End If
        End Get
        Set(value As Long)
            Me.DDLsubmitters.SelectedValue = value
        End Set
    End Property
    Private Property SelectedStatus As lm.Comol.Modules.CallForPapers.Domain.SubmissionStatus Implements IViewCallFinder.SelectedStatus
        Get
            If Not IsNothing(Me.RBLfilterRFM.SelectedItem) Then
                Return Me.RBLfilterRFM.SelectedValue
            Else
                Return SubmissionStatus.submitted
            End If
        End Get
        Set(ByVal value As lm.Comol.Modules.CallForPapers.Domain.SubmissionStatus)
            Try
                Me.RBLfilterRFM.SelectedValue = value
            Catch ex As Exception
            End Try
        End Set
    End Property
    Private WriteOnly Property AllowViewList As Boolean Implements IViewCallFinder.AllowViewList
        Set(value As Boolean)
            Me.BTNreturn.Visible = value
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False 'controllare: copiato da UC_IMsourceCSV.
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProfilesImport", "Modules", "ProfileManagement")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setButton(BTNpreviewCSV, True)
            .setButton(BTNreturn, True)

            .setLabel(LBnameRFM)
            .setLabel(LBstartDate)
            .setLabel(LBsubmittedItems)
            .setLabel(LBwaitingItems)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl() Implements lm.Comol.Modules.CallForPapers.Presentation.IViewCallFinder.InitializeControl
        isValid = False
        isInitialized = False
        Me.CurrentPresenter.InitView(ItemType, Range, FromIdCommunity)
    End Sub
    Public Sub InitializeControl(type As CallForPaperType, sRange As SearchRange, idCommunity As Integer) Implements IViewCallFinder.InitializeControl
        isValid = False
        isInitialized = False
        ItemType = type
        Range = sRange
        FromIdCommunity = idCommunity
        Me.CurrentPresenter.InitView(type, sRange, idCommunity)
    End Sub
    Public Sub InitializeControl(idItem As Long) Implements IViewCallFinder.InitializeControl
        isValid = False
        isInitialized = False
        Me.CurrentPresenter.SelectCall(idItem)
    End Sub
    Private Sub DisplaySelectionError() Implements IViewCallFinder.DisplaySelectionError
        'da implementare
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewCallFinder.DisplaySessionTimeout
        Me.MLVcontrolData.SetActiveView(VIWempty)
    End Sub
    Public Function GetStandardAvailableColumns() As List(Of lm.Comol.Core.DomainModel.Helpers.ExternalColumn) Implements IViewCallFinder.GetStandardAvailableColumns
        Return Me.CurrentPresenter.GetStandardAvailableColumns()
    End Function
    Public Function GetAvailableColumns() As List(Of ProfileColumnComparer(Of String)) Implements IViewCallFinder.GetAvailableColumns
        Return Me.CurrentPresenter.GetAvailableColumns()
    End Function
    'Private Function GetItemSubmissions(columns As List(Of lm.Comol.Core.DomainModel.Helpers.ExternalColumnComparer(Of String, String))) As lm.Comol.Core.DomainModel.Helpers.ExternalResource Implements lm.Comol.Modules.CallForPapers.Presentation.IViewCallFinder.GetItemSubmissions

    'End Function
    Public Function GetItemSubmissions(columns As List(Of ProfileColumnComparer(Of String))) As ProfileExternalResource Implements IViewCallFinder.GetItemSubmissions
        Return CurrentPresenter.GetItemSubmissions(columns)
    End Function

    Public Sub LoadAvailableStatus(status As System.Collections.Generic.List(Of lm.Comol.Modules.CallForPapers.Domain.SubmissionStatus)) Implements IViewCallFinder.LoadAvailableStatus
        Me.RBLfilterRFM.Items.Clear()
        For Each oStatus As SubmissionStatus In status
            Dim oItem As New ListItem
            oItem.Text = Me.Resource.getValue("RBLfilterRFM." & oStatus) 'setRadioButtonList(Me.RBLfilterRFM, oStatus) 'getValue("RBLfilterRFM." & oStatus)
            oItem.Value = oStatus
            Me.RBLfilterRFM.Items.Add(oItem)
        Next
    End Sub

    Public Sub LoadCalls(calls As System.Collections.Generic.List(Of lm.Comol.Modules.CallForPapers.Domain.dtoCallToFind)) Implements IViewCallFinder.LoadCalls
        Me.RPTlistRFM.DataSource = calls
        Me.RPTlistRFM.DataBind()
        Me.MLVcontrolData.SetActiveView(VIWlist)
    End Sub

    Public Sub LoadCallInfo(dtoCall As lm.Comol.Modules.CallForPapers.Domain.dtoCallToFind) Implements IViewCallFinder.LoadCallInfo
        If Not IsNothing(dtoCall) Then
            Me.LTNameRFM.Text = dtoCall.Name
            Me.LTstartDate.Text = dtoCall.StartDate.ToString("dd/MM/yyyy")
            Me.LTsubmittedItems.Text = dtoCall.SubmittedItems.ToString()
            Me.LTwaitingItems.Text = dtoCall.WaitingItems.ToString()
            Me.MLVcontrolData.SetActiveView(VIWselectedItem)
        End If
    End Sub
    Public Sub SetMinMaxDateInTimePicker(oDto As dtoCallToFind)
        If (Me.RDPstartDay.SelectedDate.HasValue) Then
            Me.RDPstartDay.MinDate = oDto.CreatedOn
            Me.RDPstartDay.MaxDate = oDto.EndDate
        End If
    End Sub



    Public Sub PreviewRows(submissions As lm.Comol.Core.DomainModel.Helpers.ExternalResource) Implements lm.Comol.Modules.CallForPapers.Presentation.IViewCallFinder.PreviewRows
        If Not IsNothing(submissions) Then
            Dim list As New List(Of ExternalResource)
            list.Add(submissions)
            Me.RPTpreview.DataSource = list
            Me.RPTpreview.DataBind()
            Me.RPTpreview.Visible = True
            SPNpreviewTable.Visible = True
        Else
            Me.RPTpreview.Visible = False
            SPNpreviewTable.Visible = False
        End If
    End Sub

    Public Function RetrieveItemSubmissions() As ProfileExternalResource Implements IViewCallFinder.RetrieveItemSubmissions
        Return Me.CurrentPresenter.GetContent() 'Controllare
    End Function




    Public Sub LoadSubmitterType(submitters As Dictionary(Of Long, String)) Implements lm.Comol.Modules.CallForPapers.Presentation.IViewCallFinder.LoadSubmitterType
        Me.DDLsubmitters.Items.Clear()
        Me.DDLsubmitters.Visible = False
        For Each kvp As KeyValuePair(Of Long, String) In submitters
            Dim oItem As New ListItem
            oItem.Text = kvp.Value
            oItem.Value = kvp.Key
            Me.DDLsubmitters.Items.Add(oItem)
        Next
        If (submitters.Count > 1) Then
            Me.DDLsubmitters.Visible = True
        End If
    End Sub

#End Region

    Private Sub BTNpreviewCSV_Click(sender As Object, e As System.EventArgs) Handles BTNpreviewCSV.Click
        Me.CurrentPresenter.Preview()
    End Sub

    Private Sub BTNreturn_Click(sender As Object, e As System.EventArgs) Handles BTNreturn.Click
        Me.CurrentPresenter.ChangeCall(ItemType, Range, FromIdCommunity)
        Me.MLVcontrolData.SetActiveView(VIWlist)
        Me.RPTpreview.Visible = False
        SPNpreviewTable.Visible = False
    End Sub
    Private Sub RBLfilterRFM_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RBLfilterRFM.SelectedIndexChanged
        If Me.RPTpreview.Visible = True Then
            Me.CurrentPresenter.Preview()
        End If
    End Sub
    Private Sub DDLsubmitters_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDLsubmitters.SelectedIndexChanged
        If Me.RPTpreview.Visible = True Then
            Me.CurrentPresenter.Preview()  'controlla
        End If
    End Sub

    Private Sub RDPstartDay_SelectedDateChanged(sender As Object, e As EventArgs) Handles RDPstartDay.SelectedDateChanged
        If Me.RPTpreview.Visible = True Then
            Me.CurrentPresenter.Preview()  'controlla
        End If
    End Sub

    Protected Sub RPTheader_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim column As ExternalColumn = DirectCast(e.Item.DataItem, ExternalColumn)
            Dim oLiteral As Literal = e.Item.FindControl("LTcolumn")

            If IsNothing(column.Name) Then
                oLiteral.Text = String.Format(Me.Resource.getValue("Column"), column.Number.ToString)
            Else
                oLiteral.Text = column.Name
            End If
        End If
    End Sub

    Protected Sub RPTitems_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As ExternalCell = DirectCast(e.Item.DataItem, ExternalCell)
            Dim oLiteral As Literal = e.Item.FindControl("LTitem")

            If item.Value.Contains(lm.Comol.Core.DomainModel.Helpers.ImportCostants.MultipleItemsSeparator) Then
                Dim div As HtmlControl = e.Item.FindControl("DVmultiple")
                div.Visible = True
                Dim oControl As UCtoolTip = e.Item.FindControl("CTRLtoolTip")
                Dim myDelims As String() = New String() {lm.Comol.Core.DomainModel.Helpers.ImportCostants.MultipleItemsSeparator}
                Dim values As List(Of String) = item.Value.Split(myDelims, StringSplitOptions.RemoveEmptyEntries).ToList
                oControl.InitializeControl(Me.Resource.getValue("Details"), values)
                Dim oDisplay As Literal = e.Item.FindControl("LTdisplayName")
                oDisplay.Text = values.Count.ToString & " " & item.Column.Name
                oLiteral.Visible = False
            ElseIf Not IsNothing(item.Value) AndAlso (item.Value <> "") AndAlso item.Value <> "True" AndAlso item.Value <> "False" Then
                oLiteral.Text = item.Value.ToString()   'IIf(String.IsNullOrEmpty(item), " ", item)
            ElseIf (item.Value = "True" OrElse "False") Then
                oLiteral.Text = Me.Resource.getValue(item.Value.ToString())
            Else
                oLiteral.Text = "-"
            End If
        End If
    End Sub
    Protected Sub RPTlistRFM_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Header Then

            Dim oLTheaderSelect As Literal = e.Item.FindControl("LTheaderSelect")
            Dim oLTheaderName As Literal = e.Item.FindControl("LTheaderName")
            Dim oLTheaderDate As Literal = e.Item.FindControl("LTheaderDate")
            Dim oLTheaderSubmittedQuestions As Literal = e.Item.FindControl("LTheaderSubmittedQuestions")
            Dim oLTheaderIncompletedQuestions As Literal = e.Item.FindControl("LTheaderIncompletedQuestions")
            Try
                Me.Resource.setLiteral(oLTheaderName)
                Me.Resource.setLiteral(oLTheaderDate)
                Me.Resource.setLiteral(oLTheaderSubmittedQuestions)
                Me.Resource.setLiteral(oLTheaderIncompletedQuestions)
                Me.Resource.setLiteral(oLTheaderSelect)
            Catch ex As Exception
            End Try

        ElseIf e.Item.ItemType = ListItemType.Item Or ListItemType.AlternatingItem Then
            Dim oDto As dtoCallToFind = e.Item.DataItem
            If Not IsNothing(e.Item.DataItem) Then
                Dim oLTname As Literal = e.Item.FindControl("LTname")
                If Not IsNothing(oLTname) Then
                    oLTname.Text = oDto.Name.ToString()
                End If

                Dim oLTdate As Literal = e.Item.FindControl("LTdate")
                If Not IsNothing(oLTdate) Then
                    oLTdate.Text = oDto.StartDate.ToString("dd/MM/yy")
                End If

                Dim oLTsubmittedQuestions As Literal = e.Item.FindControl("LTsubmittedQuestions")
                If Not IsNothing(oLTsubmittedQuestions) Then
                    oLTsubmittedQuestions.Text = oDto.SubmittedItems.ToString()
                End If

                Dim oLTincompletedQuestions As Literal = e.Item.FindControl("LTincompletedQuestions")
                If Not IsNothing(oLTincompletedQuestions) Then
                    oLTincompletedQuestions.Text = oDto.WaitingItems.ToString()
                End If

                Dim oLNBselectRFM As LinkButton = e.Item.FindControl("LNBselectRFM")
                If Not IsNothing(oLNBselectRFM) Then
                    Me.Resource.setLinkButton(oLNBselectRFM, False, True) 'False, True, , True)
                    oLNBselectRFM.CommandArgument = oDto.Id.ToString()
                    oLNBselectRFM.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
                    oLNBselectRFM.Text = String.Format(oLNBselectRFM.Text, Me.BaseUrl & "images/Grid/s.gif", oLNBselectRFM.ToolTip)
                End If
            End If
        End If
    End Sub

    Public Sub RPTlistRFM_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTlistRFM.ItemCommand
        Try
            Me.RDPstartDay.Clear()
            Me.CurrentPresenter.SelectCall(CType(e.CommandArgument, Long))
        Catch ex As Exception
        End Try
    End Sub

End Class