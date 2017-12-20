Imports System.Linq
Imports lm.Comol.Core.BaseModules.NoticeBoard.Presentation
Imports lm.Comol.Core.BaseModules.NoticeBoard.Domain
Public Class UC_NoticeboardRender
    Inherits BaseControl
    Implements IViewNoticeboardRenderControl

#Region "Context"
    Private _Presenter As NoticeboardRenderControlPresenter
    Public ReadOnly Property CurrentPresenter() As NoticeboardRenderControlPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New NoticeboardRenderControlPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property DisplayHistory As Boolean Implements IViewNoticeboardRenderControl.DisplayHistory
        Get
            Return ViewStateOrDefault("DisplayHistory", True)
        End Get
        Set(value As Boolean)
            ViewState("DisplayHistory") = value
        End Set
    End Property
    Public Property IsForManagement As Boolean Implements IViewNoticeboardRenderControl.IsForManagement
        Get
            Return ViewStateOrDefault("IsForManagement", False)
        End Get
        Set(value As Boolean)
            ViewState("IsForManagement") = value
        End Set
    End Property
    Public Property HistoryPageSize As Integer Implements IViewNoticeboardRenderControl.HistoryPageSize
        Get
            Return ViewStateOrDefault("HistoryPageSize", 5)
        End Get
        Set(value As Integer)
            ViewState("HistoryPageSize") = value
        End Set
    End Property
    Private Property ContainerIdCommunity As Integer Implements IViewNoticeboardRenderControl.ContainerIdCommunity
        Get
            Return ViewStateOrDefault("ContainerIdCommunity", -1)
        End Get
        Set(value As Integer)
            ViewState("ContainerIdCommunity") = value
        End Set
    End Property
  
    Private Property HasHistory As Boolean Implements IViewNoticeboardRenderControl.HasHistory
        Get
            Return ViewStateOrDefault("HasHistory", False)
        End Get
        Set(value As Boolean)
            ViewState("HasHistory") = value
        End Set
    End Property

    Private ReadOnly Property GetTranslatedRemovedUser As String Implements IViewNoticeboardRenderControl.GetTranslatedRemovedUser
        Get
            Return Resource.getValue("RemovedUserName")
        End Get
    End Property
    Private Property HistoryPageIndex As Integer Implements IViewNoticeboardRenderControl.HistoryPageIndex
        Get
            Return ViewStateOrDefault("HistoryPageIndex", 0)
        End Get
        Set(value As Integer)
            ViewState("HistoryPageIndex") = value
        End Set
    End Property
    Private Property HistoryPageCount As Integer Implements IViewNoticeboardRenderControl.HistoryPageCount
        Get
            Return ViewStateOrDefault("HistoryPageCount", 0)
        End Get
        Set(value As Integer)
            ViewState("HistoryPageCount") = value
        End Set
    End Property
    Private Property IdCurrentMessage As Long Implements IViewNoticeboardRenderControl.IdCurrentMessage
        Get
            Return ViewStateOrDefault("IdCurrentMessage", 0)
        End Get
        Set(value As Long)
            ViewState("IdCurrentMessage") = value
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
    Public Property RaiseEvents As Boolean
        Get
            Return ViewStateOrDefault("RaiseEvents", False)
        End Get
        Set(value As Boolean)
            ViewState("RaiseEvents") = value
        End Set
    End Property
    Public Event SelectedMessage(idMessage As Long)
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_noticeboard", "Modules", "Noticeboard")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            If IsForManagement Then
                LTnoticeboardTitle.Text = .getValue("LTnoticeboardTitle.IsForManagement")
            Else
                .setLiteral(LTnoticeboardTitle)
            End If

            .setLiteral(LTnoticeboardHistoryTitle)
            .setHyperLink(HYPeditNoticeboard, False, True)
            .setHyperLink(HYPprintNoticeboard, False, True)

            LNBiconPreviousMessages.Text = LTpreviousIconTemplate.Text
            LNBiconNextMessages.Text = LTnextIconTemplate.Text
            LNBiconPreviousMessages.ToolTip = String.Format(LTtextTemplate.Text, Resource.getValue("LNBpreviousMessages.ToolTip"))
            LNBiconNextMessages.ToolTip = String.Format(LTtextTemplate.Text, Resource.getValue("LNBnextMessages.ToolTip"))
            LNBpreviousMessages.ToolTip = String.Format(LTtextTemplate.Text, Resource.getValue("LNBpreviousMessages.ToolTip"))
            LNBnextMessages.ToolTip = String.Format(LTtextTemplate.Text, Resource.getValue(" LNBpreviousMessages.ToolTip"))
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(idCommunity As Integer, Optional idMessage As Long = 0) Implements IViewNoticeboardRenderControl.InitializeControl
        SetInternazionalizzazione()
        CurrentPresenter.InitView(idCommunity, Nothing, idMessage)
    End Sub
    Public Sub InitializeControl(idCommunity As Integer, permissions As ModuleNoticeboard, Optional idMessage As Long = 0) Implements IViewNoticeboardRenderControl.InitializeControl
        SetInternazionalizzazione()
        CurrentPresenter.InitView(idCommunity, permissions, idMessage)
    End Sub
    Private Sub LoadHistory(items As List(Of dtoHistoryItem), pageIndex As Integer, pageCount As Integer) Implements IViewNoticeboardRenderControl.LoadHistory
        DVcolumnHistory.Visible = DisplayHistory
        RPThistoryMessages.DataSource = items
        RPThistoryMessages.DataBind()
        HistoryPageCount = pageCount
        HistoryPageIndex = pageIndex

        LInextItems.Visible = False
        LIpreviousItems.Visible = False
        If pageCount > 1 Then
            Select Case pageIndex
                Case 0
                    LInextItems.Visible = True
                    LNBiconNextMessages.CommandArgument = pageIndex + 1
                    LNBnextMessages.CommandArgument = pageIndex + 1
                Case pageCount - 1
                    LIpreviousItems.Visible = True
                    LNBiconPreviousMessages.CommandArgument = pageIndex - 1
                    LNBpreviousMessages.CommandArgument = pageIndex - 1
                Case Else
                    LInextItems.Visible = True
                    LIpreviousItems.Visible = True
                    LNBiconNextMessages.CommandArgument = pageIndex + 1
                    LNBnextMessages.CommandArgument = pageIndex + 1
                    LNBiconPreviousMessages.CommandArgument = pageIndex - 1
                    LNBpreviousMessages.CommandArgument = pageIndex - 1
            End Select
        End If
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewNoticeboardRenderControl.DisplaySessionTimeout
        DVfooterCommands.Visible = False
        LInextItems.Visible = False
        LIpreviousItems.Visible = False

        For Each row As RepeaterItem In RPThistoryMessages.Items
            Dim oLinkButton As LinkButton = row.FindControl("LNBviewMessage")
            oLinkButton.Enabled = False
        Next
    End Sub
    Private Sub InitalizeCommands(allowPrint As Boolean, Optional editUrl As String = "") Implements IViewNoticeboardRenderControl.InitalizeCommands
        If String.IsNullOrEmpty(editUrl) Then
            HYPeditNoticeboard.Visible = False
        Else
            HYPeditNoticeboard.Visible = True
            Me.HYPeditNoticeboard.NavigateUrl = BaseUrl & editUrl
        End If
        HYPprintNoticeboard.Visible = allowPrint

        DVfooterCommands.Visible = allowPrint OrElse Not String.IsNullOrEmpty(editUrl)
        LBseparator.Visible = allowPrint AndAlso Not String.IsNullOrEmpty(editUrl)
    End Sub
    Private Sub LoadMessage(idMessage As Long, idCommunity As Integer) Implements IViewNoticeboardRenderControl.LoadMessage
        LTrenderNoticeboard.Text = String.Format(LTrenderNoticeboardTemplate.Text, BaseUrl + lm.Comol.Core.BaseModules.NoticeBoard.Domain.RootObject.DisplayMessage(idMessage, idCommunity))
    End Sub

#End Region

#Region "Internal"
    Private Sub RPThistoryMessages_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPThistoryMessages.ItemDataBound
        Dim dto As dtoHistoryItem = e.Item.DataItem
        Dim oLinkButton As LinkButton = e.Item.FindControl("LNBviewMessage")

        Dim modifiedOn As String = "//"
        If dto.ModifiedOn.HasValue Then
            modifiedOn = dto.ModifiedOn.Value.ToString(Resource.CultureInfo.DateTimeFormat.ShortDatePattern) & " " & dto.ModifiedOn.Value.ToString(Resource.CultureInfo.DateTimeFormat.ShortTimePattern)
        End If
        oLinkButton.Text = String.Format(LTitemTemplate.Text, dto.ModifiedBy, modifiedOn)

        Dim oControl As HtmlControl = e.Item.FindControl("LIitem")
        oControl.Attributes("class") = oControl.Attributes("class") & " " & GetItemCssClass(dto)
    End Sub
    Private Sub RPThistoryMessages_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPThistoryMessages.ItemCommand
        CurrentPresenter.LoadMessage(e.CommandArgument, ContainerIdCommunity)
        Dim oControl As HtmlControl = e.Item.FindControl("LIitem")
        If Not oControl.Attributes("class").Contains(LTcssClassActive.Text) Then
            oControl.Attributes("class") = oControl.Attributes("class") & " " & LTcssClassActive.Text

            For Each row As RepeaterItem In (From r As RepeaterItem In RPThistoryMessages.Items Where r.ItemIndex <> e.Item.ItemIndex)
                oControl = row.FindControl("LIitem")
                If oControl.Attributes("class").Contains(LTcssClassActive.Text) Then
                    oControl.Attributes("class") = Replace(oControl.Attributes("class"), LTcssClassActive.Text, "")
                    Exit For
                End If
            Next
        End If
        If RaiseEvents Then
            RaiseEvent SelectedMessage(CLng(e.CommandArgument))
        End If
    End Sub

    Public Function GetItemCssClass(ByVal item As dtoHistoryItem) As String
        Dim cssClass As String = GetItemCssClass(item.DisplayAs)
        If item.Selected Then
            cssClass &= " " & LTcssClassActive.Text
        End If
        If item.isDeleted Then
            cssClass &= " " & LTcssClassDeleted.Text
        End If
        Return cssClass
    End Function
    Public Function GetHistoryCssClass() As String
        If DisplayHistory AndAlso HasHistory Then
            Return LTcssClassHasHistory.Text
        Else
            Return LTcssClassNoHistory.Text
        End If
    End Function
    Public Function GetManagerCssClass() As String
        If IsForManagement Then
            Return LTcssClassManager.Text
        Else
            Return ""
        End If
    End Function
    Protected Function GetItemCssClass(ByVal d As lm.Comol.Core.DomainModel.ItemDisplayOrder) As String
        Dim cssClass As String = ""
        Select Case d
            Case lm.Comol.Core.DomainModel.ItemDisplayOrder.first, lm.Comol.Core.DomainModel.ItemDisplayOrder.last
                cssClass = " " & d.ToString
            Case lm.Comol.Core.DomainModel.ItemDisplayOrder.item
                cssClass = ""
            Case Else
                cssClass = " " & lm.Comol.Core.DomainModel.ItemDisplayOrder.first.ToString() & " " & lm.Comol.Core.DomainModel.ItemDisplayOrder.last.ToString()
        End Select
        Return cssClass
    End Function
    Private Sub LNBpreviousMessages_Click(sender As Object, e As EventArgs) Handles LNBpreviousMessages.Click, LNBiconPreviousMessages.Click, LNBiconNextMessages.Click, LNBnextMessages.Click
        CurrentPresenter.LoadHistoryItems(DirectCast(sender, LinkButton).CommandArgument, ContainerIdCommunity, IdCurrentMessage)
    End Sub
#End Region

End Class