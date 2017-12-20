Imports lm.Comol.Core.Dashboard.Domain
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Public Class UC_NoticeboardTile
    Inherits DBbaseControl
    Implements IViewNoticeboardTile

#Region "Context"
    Private _Presenter As NoticeboardTilePresenter
    Private ReadOnly Property CurrentPresenter() As NoticeboardTilePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New NoticeboardTilePresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property CurrentLayout As TileLayout Implements IViewNoticeboardTile.CurrentLayout
        Get
            Return ViewStateOrDefault("CurrentLayout", TileLayout.grid_4)
        End Get
        Set(value As TileLayout)
            ViewState("CurrentLayout") = value
        End Set
    End Property
#End Region

#Region "Internal"
    Public Property IsInitialized As Boolean
        Get
            Return ViewStateOrDefault("IsInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("IsInitialized") = value
        End Set
    End Property
    Private ReadOnly Property NoticeboardFilePath As String
        Get
            Dim baseFilePath As String = ""
            If Me.SystemSettings.File.Noticeboard.DrivePath = "" Then
                baseFilePath = Server.MapPath(Me.PageUtility.BaseUrl & Me.SystemSettings.File.Noticeboard.VirtualPath)
            Else
                baseFilePath = Me.SystemSettings.File.Noticeboard.DrivePath
            End If
            Return baseFilePath
        End Get
    End Property
    Private ReadOnly Property NoticeboardVirtualPath As String
        Get
            Return Me.SystemSettings.File.Noticeboard.VirtualPath
        End Get
    End Property
    Public Property IsPreview As Boolean
        Get
            Return ViewStateOrDefault("IsPreview", False)
        End Get
        Set(value As Boolean)
            ViewState("IsPreview") = value
        End Set
    End Property
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLiteral(LTnoticeboardTitle)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitalizeControl(layout As TileLayout, idCommunity As Integer) Implements IViewNoticeboardTile.InitalizeControl
        IsInitialized = True
        CurrentLayout = layout
        CurrentPresenter.InitView(idCommunity)
    End Sub

    Private Sub LoadMessage(message As lm.Comol.Core.BaseModules.NoticeBoard.Domain.liteHistoryItem, idCommunity As Integer, subscription As lm.Comol.Core.DomainModel.liteSubscriptionInfo) Implements IViewNoticeboardTile.LoadMessage
        LoadMessage(message, idCommunity)
        LoadMessageInfos(message, subscription)
    End Sub

    Private Sub LoadMessage(message As lm.Comol.Core.BaseModules.NoticeBoard.Domain.liteHistoryItem, p As lm.Comol.Core.DomainModel.litePerson) Implements IViewNoticeboardTile.LoadMessage
        LoadMessage(message, 0)
        LoadMessageInfos(message, p)
    End Sub

    Private Sub LoadNoPermissionsToSeeMessage() Implements IViewNoticeboardTile.LoadNoPermissionsToSeeMessage
        DisplaySessionTimeout()
        IMBthumbnail.ImageUrl = PageUtility.ApplicationUrlBase & LTdefaultImg.Text
    End Sub
    Private Sub LoadNoMessage() Implements IViewNoticeboardTile.LoadNoMessage
        IMBthumbnail.ImageUrl = PageUtility.ApplicationUrlBase & LTdefaultImg.Text
    End Sub

    Private Sub DisplaySessionTimeout() Implements IViewNoticeboardTile.DisplaySessionTimeout
        HYPnoticeboard.Visible = False
        HYPnoticeboardImage.Enabled = False
        DVmessage.Visible = False
    End Sub

#End Region

#Region "Internal"
    Private Sub LoadMessageInfos(message As lm.Comol.Core.BaseModules.NoticeBoard.Domain.liteHistoryItem, subscription As lm.Comol.Core.DomainModel.liteSubscriptionInfo)
        'Dim mTime As DateTime? = message.CreatedOn
        'If message.ModifiedOn.HasValue Then
        '    mTime = message.ModifiedOn
        'End If
        Dim mTime As DateTime? = message.DisplayDate
        Dim uTime As DateTime? = subscription.PreviousAccessOn
        If Not uTime.HasValue AndAlso subscription.LastAccessOn.HasValue Then
            uTime = subscription.LastAccessOn
        End If
        If IsNew(mTime, uTime) Then
            DVmessage.Attributes("class") = LTmessageInfoCssClass.Text & " " & LTisNewCssClass.Text
        ElseIf IsRecent(mTime, uTime) Then
            DVmessage.Attributes("class") = LTmessageInfoCssClass.Text & " " & LTrecentCssClass.Text
        End If
    End Sub
    Private Sub LoadMessageInfos(message As lm.Comol.Core.BaseModules.NoticeBoard.Domain.liteHistoryItem, p As lm.Comol.Core.DomainModel.litePerson)
        'Dim mTime As DateTime? = message.CreatedOn
        'If message.ModifiedOn.HasValue Then
        '    mTime = message.ModifiedOn
        'End If
        If IsNew(message.DisplayDate, p.PreviousAccess) Then
            DVmessage.Attributes("class") = LTmessageInfoCssClass.Text & " " & LTisNewCssClass.Text
        ElseIf IsRecent(message.DisplayDate, p.PreviousAccess) Then
            DVmessage.Attributes("class") = LTmessageInfoCssClass.Text & " " & LTrecentCssClass.Text
        End If
    End Sub
    Private Function IsRecent(ByVal d As DateTime?, ByVal userAccess As DateTime?) As Boolean
        Return Not d.HasValue OrElse Not userAccess.HasValue OrElse (d.HasValue AndAlso userAccess.HasValue AndAlso d.Value > userAccess.Value.AddDays(-7))
    End Function
    Private Function IsNew(ByVal d As DateTime?, ByVal userAccess As DateTime?) As Boolean
        Return Not d.HasValue OrElse Not userAccess.HasValue OrElse (d.HasValue AndAlso userAccess.HasValue AndAlso d.Value > userAccess)

    End Function
    Private Sub LoadMessage(message As lm.Comol.Core.BaseModules.NoticeBoard.Domain.liteHistoryItem, idCommunity As Integer)
        HYPnoticeboard.Visible = True

        HYPnoticeboard.Text = String.Format(LTexpandNoticeboardTemplate.Text, Resource.getValue("Noticeboard.expand"))
        HYPnoticeboard.NavigateUrl = BaseUrl + lm.Comol.Core.BaseModules.NoticeBoard.Domain.RootObject.ModalMessagePage(message.Id, idCommunity)

        HYPnoticeboardImage.NavigateUrl = BaseUrl + lm.Comol.Core.BaseModules.NoticeBoard.Domain.RootObject.ModalMessagePage(message.Id, idCommunity)
        If message.Image <> Guid.Empty AndAlso lm.Comol.Core.File.Exists.File(NoticeboardFilePath & idCommunity & "\" & message.Image.ToString & ".png") Then
            IMBthumbnail.ImageUrl = PageUtility.ApplicationUrlBase & NoticeboardVirtualPath & idCommunity & "/" & message.Image.ToString & ".png"
        Else
            IMBthumbnail.ImageUrl = PageUtility.ApplicationUrlBase & LTdefaultImg.Text
        End If
        DVmessage.Visible = True
        If message.CreatedOn.HasValue AndAlso message.CreatedOn.Value = message.DisplayDate Then
            LTmessage.Text = String.Format(Resource.getValue("LTmessageTileDashboard.CreatedOn"), GetMonthDayName(message.CreatedOn), message.CreatedOn.Value.Year)
        ElseIf message.ModifiedOn.HasValue Then
            LTmessage.Text = String.Format(Resource.getValue("LTmessageTileDashboard.ModifiedOn"), GetMonthDayName(message.DisplayDate), message.DisplayDate.Year)
        End If
    End Sub

    Private Function GetMonthDayName(ByVal d As DateTime?) As String
        If d.HasValue Then
            Return d.Value.ToString(Resource.CultureInfo.DateTimeFormat.MonthDayPattern)
        Else
            Return ""
        End If

    End Function
    Public Function GetItemCssClass() As String
        Return CurrentLayout.ToString
    End Function
#End Region

End Class