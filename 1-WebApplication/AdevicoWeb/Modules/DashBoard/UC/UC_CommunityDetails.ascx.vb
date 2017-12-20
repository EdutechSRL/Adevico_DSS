Imports lm.Comol.Core.BaseModules.Dashboard.Presentation

Public Class UC_CommunityDetails
    Inherits DBbaseControl
    Implements IViewCommunityDetails

#Region "Context"
    Private _Presenter As CommunityDetailsPresenter
    Private ReadOnly Property CurrentPresenter() As CommunityDetailsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CommunityDetailsPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLiteral(LTcommunityInfoTitle)
            .setLiteral(LTcommunityOwnerTitle)
            .setLabel(LBcommunityCreatedBy_t)
            .setLabel(LBcommunityTagsTitle)
            .setLabel(LBcommunityTagsTitle)
            .setLiteral(LTenrollmentsDetailsTitle)
            .setLiteral(LTenrollmentsDetailsCount_t)
            .setLiteral(LTconstraintsTitle)
            .setLiteral(LTdescriptionTile)
            .setLiteral(LTextraInfoTile)
            .setLiteral(LTdetailsCommunityType_t)
            .setLiteral(LTdetailsStatus_t)
            .setLiteral(LTdetailsEnrollments_t)
            .setLiteral(LTdetailsCreatedOn_t)
            .setLiteral(LTdetailsClosedOn_t)
            .setLiteral(LTdetailsAvailableSeats_t)
            .setLabel(LBdetailsAvailableSeats)
            .setLiteral(LTenrollments_t)
            .setLabel(LBenrollments)
            .setLiteral(LTmaxEnrollments_t)
            .setLabel(LBmaxEnrollments)
            .setLiteral(LTspecialAccess_t)
            .setLiteral(LTenrollmentWindow_t)

            .setLiteral(LTdetailsCourseYear_t)
            .setLiteral(LTdetailsCourseCode_t)
            .setLiteral(LTdetailsCourseTimespan_t)
            .setLiteral(LTotherDetailsTitle)
    
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(community As lm.Comol.Core.DomainModel.liteCommunityInfo) Implements IViewCommunityDetails.InitializeControl
        IMGavatar.ImageUrl = PageUtility.ApplicationUrlBase & LTdefaultImg.Text
        CurrentPresenter.InitView(community)
    End Sub
    Private Sub LoadUserInfo(responsible As lm.Comol.Core.DomainModel.litePerson, creator As lm.Comol.Core.DomainModel.litePerson) Implements IViewCommunityDetails.LoadUserInfo
        If Not IsNothing(responsible) Then
            LBownerName.Text = responsible.SurnameAndName
            LBownerMail.Text = "<a href=mailto:" & responsible.Mail & ">" & responsible.Mail & "</a>"
            SetPhotoPath(responsible)
            If Not String.IsNullOrEmpty(responsible.OfficeHours) Then
                LBownerName.ToolTip = String.Format(Resource.getValue("OfficeHours"), responsible.OfficeHours)
                IMGavatar.ToolTip = LBownerName.Text & " - " & LBownerName.ToolTip
            End If
        Else
            LBownerMail.Visible = False
            LBownerName.Text = Resource.getValue("UnknownResponsible")
        End If
        If Not IsNothing(creator) AndAlso IsNothing(responsible) Then
            DVcreator.Visible = True
            LBcommunityCreatedBy.Text = creator.SurnameAndName
            LBcommunityCreatedByMail.Text = "<a href=mailto:" & creator.Mail & ">" & creator.Mail & "</a>"
            SetPhotoPath(creator)
        Else
            DVcreator.Visible = False
        End If
    End Sub
    Private Sub LoadTags(tags As List(Of String), idCommunityType As Integer) Implements IViewCommunityDetails.LoadTags
        LBtagCommunityType.Visible = False
        If idCommunityType = lm.Comol.Core.DomainModel.CommunityTypeStandard.Organization Then
            LBtagCommunityType.Visible = True
            LBtagCommunityType.Text = Resource.getValue("LBtagCommunityType." & lm.Comol.Core.DomainModel.CommunityTypeStandard.Organization.ToString)
        End If
        RPTtags.DataSource = tags
        RPTtags.DataBind()
    End Sub
    Private Sub LoadEnrollmentsInfo(community As lm.Comol.Core.DomainModel.liteCommunityInfo, items As List(Of lm.Comol.Core.BaseModules.Dashboard.Domain.dtoEnrollmentsDetailInfo), count As Long, waiting As Long) Implements IViewCommunityDetails.LoadEnrollmentsInfo
        TRenrollmentWindow.Visible = community.SubscriptionEndOn.HasValue OrElse community.SubscriptionStartOn.HasValue
        If community.SubscriptionStartOn.HasValue Then
            CTRLstart.Visible = True
            CTRLstart.InitializeControl(Resource.getValue("CTRLstart.StartEnrollments"), GetDateTimeString(community.SubscriptionStartOn.Value, "//", True))
        End If
        If community.SubscriptionEndOn.HasValue Then
            CTRLend.Visible = True
            CTRLend.InitializeControl(Resource.getValue("CTRLend.EndEnrollments"), GetDateTimeString(community.SubscriptionEndOn.Value, "//", True))
        End If

        ' Display available seats
        TRseatsAvailable.Visible = (community.MaxUsersWithDefaultRole > 0)
        If community.MaxUsersWithDefaultRole > 0 Then
            Dim totalSeats As Long = community.MaxUsersWithDefaultRole + community.MaxOverDefaultSubscriptionsAllowed
            Dim dSubscriptions As Long = items.Where(Function(i) i.IsDefault).Select(Function(i) i.Count).FirstOrDefault()
            Dim availableSeats As Long = totalSeats - dSubscriptions
            If totalSeats >= availableSeats AndAlso availableSeats > 0 Then
                LBseatsNumber.Text = availableSeats.ToString
            Else
                LBseatsNumber.Text = 0
            End If

            Dim bItems As New List(Of StackedBarItem)
            If totalSeats = dSubscriptions Then
                bItems.Add(New StackedBarItem() With {.CssClass = LTbusySeatsCssClass.Text, .Title = String.Format(Resource.getValue("ProgressTooltip.Seats.Busy"), totalSeats, "{0}"), .Value = 100})
            ElseIf availableSeats > 0 Then
                bItems.Add(New StackedBarItem() With {.CssClass = LTbusySeatsCssClass.Text, .Title = String.Format(Resource.getValue("ProgressTooltip.Seats.Busy"), dSubscriptions, "{0}"), .Value = Percentual(dSubscriptions, totalSeats)})
                bItems.Add(New StackedBarItem() With {.CssClass = LTavailableSeatsCssClass.Text, .Title = String.Format(Resource.getValue("ProgressTooltip.Seats.Available"), availableSeats, "{0}"), .Value = 100 - bItems(0).Value})

            Else
                bItems.Add(New StackedBarItem() With {.CssClass = LTbusySeatsCssClass.Text, .Title = String.Format(Resource.getValue("ProgressTooltip.Seats.Busy"), totalSeats, "{0}"), .Value = Percentual(totalSeats, dSubscriptions)})
                bItems.Add(New StackedBarItem() With {.CssClass = LTotherSeatsCssClass.Text, .Title = String.Format(Resource.getValue("ProgressTooltip.Seats.Over"), dSubscriptions - totalSeats, "{0}"), .Value = 100 - bItems(0).Value})
            End If
            CTRLprogressBar.InitializeControl(bItems)
            LBmaxEnrollments.Visible = False
            CTRLlimitedSeats.Visible = True
            CTRLlimitedSeats.InitializeControl(Resource.getValue("LBmaxEnrollments.MaxUsersWithDefaultRole.First"), community.MaxUsersWithDefaultRole, Resource.getValue("LBmaxEnrollments.MaxUsersWithDefaultRole.Third"))
            CTRLextraSeats.Visible = (community.MaxOverDefaultSubscriptionsAllowed > 0)
            If community.MaxOverDefaultSubscriptionsAllowed > 0 Then
                CTRLextraSeats.InitializeControl(Resource.getValue("LBmaxEnrollments.MaxOverDefaultSubscriptionsAllowed"), community.MaxOverDefaultSubscriptionsAllowed)
            End If
        Else
            LBmaxEnrollments.Visible = True
            CTRLextraSeats.Visible = False
            CTRLlimitedSeats.Visible = False
        End If
        LBenrollmentsNumber.Text = count

        ' load enrollments details
        LBenrollmentsDetailsCount.Text = count
        RPTenrollmentsInfo.DataSource = items
        RPTenrollmentsInfo.DataBind()
    End Sub
    Private Sub LoadConstraints(constraints As List(Of lm.Comol.Core.DomainModel.dtoCommunityConstraint)) Implements IViewCommunityDetails.LoadConstraints
        DVconstraints.Visible = constraints.Any()
        CTRLconstraintsEnrolledTo.Visible = False
        CTRLconstraintsNotEnrolledTo.Visible = False
        If constraints.Where(Function(c) c.Type = lm.Comol.Core.DomainModel.ConstraintType.EnrolledTo AndAlso Not c.IsReverse).Any() Then
            CTRLconstraintsEnrolledTo.Visible = True
            CTRLconstraintsEnrolledTo.Message = Resource.getValue("Message.ConstraintType." & lm.Comol.Core.DomainModel.ConstraintType.EnrolledTo.ToString)
            CTRLconstraintsEnrolledTo.InitializeControl(constraints.Where(Function(c) c.Type = lm.Comol.Core.DomainModel.ConstraintType.EnrolledTo AndAlso Not c.IsReverse).ToList())
        End If
        If constraints.Where(Function(c) c.Type = lm.Comol.Core.DomainModel.ConstraintType.NotEnrolledTo AndAlso Not c.IsReverse).Any() Then
            CTRLconstraintsNotEnrolledTo.Visible = True
            CTRLconstraintsNotEnrolledTo.Message = Resource.getValue("Message.ConstraintType." & lm.Comol.Core.DomainModel.ConstraintType.NotEnrolledTo.ToString)
            CTRLconstraintsNotEnrolledTo.InitializeControl(constraints.Where(Function(c) c.Type = lm.Comol.Core.DomainModel.ConstraintType.NotEnrolledTo AndAlso Not c.IsReverse).ToList())
        End If
        If constraints.Where(Function(c) c.Type = lm.Comol.Core.DomainModel.ConstraintType.NotEnrolledTo AndAlso c.IsReverse).Any() Then
            CTRLconstraintsEnrollingUnavailableFor.Visible = True
            CTRLconstraintsEnrollingUnavailableFor.Message = Resource.getValue("Message.EnrollingUnavailableFor")
            CTRLconstraintsEnrollingUnavailableFor.InitializeControl(constraints.Where(Function(c) c.Type = lm.Comol.Core.DomainModel.ConstraintType.NotEnrolledTo AndAlso c.IsReverse).ToList())
        End If
    End Sub
    Private Sub LoadDetails(community As lm.Comol.Core.DomainModel.liteCommunityInfo, type As String, description As String) Implements IViewCommunityDetails.LoadDetails
        SetBaseInfo(community, type, description, False)
    End Sub

    Public Sub SendUserAction(idCommunity As Integer, idModule As Integer, idActionCommunity As Integer, action As lm.Comol.Core.Dashboard.Domain.ModuleDashboard.ActionType) Implements IViewCommunityDetails.SendUserAction

    End Sub
#End Region

#Region "Internal"
    Private Function Percentual(value As Long, tot As Long) As Int32
        If tot > 0 Then
            Dim d As Double = value / tot * 100
            Return Math.Floor(d)
        Else
            Return 0
        End If
    End Function
    Private Sub SetBaseInfo(community As lm.Comol.Core.DomainModel.liteCommunityInfo, type As String, description As String, showDetails As Boolean)
        LTdetailsCommunityType.Text = type
        If community.isClosedByAdministrator Then
            LTdetailsStatus.Text = Resource.getValue("LTdetailsStatus.isClosedByAdministrator")
        ElseIf community.isArchived Then
            LTdetailsStatus.Text = Resource.getValue("LTdetailsStatus.isArchived." & community.ConfirmSubscription.ToString)
        Else
            LTdetailsStatus.Text = Resource.getValue("LTdetailsStatus." & community.ConfirmSubscription.ToString)
        End If

        LTdetailsEnrollments.Text = Resource.getValue("LTdetailsEnrollments." & community.AllowSubscription.ToString & "." & community.AllowUnsubscribe.ToString)
        LBdetailsCreatedOn.Text = GetDateTimeString(community.CreatedOn, "//")
        If community.ClosedOn.HasValue Then
            LBdetailsClosedOn.Text = GetDateTimeString(community.ClosedOn, "//")
            TRclosedOn.Visible = True
        Else
            TRclosedOn.Visible = False
        End If

        TRspecialAccess.Visible = community.AllowPublicAccess OrElse community.AlwaysAllowAccessToCopyCenter
        If community.AllowPublicAccess OrElse community.AlwaysAllowAccessToCopyCenter Then
            LTspecialAccess.Text = Resource.getValue("LTspecialAccess." & community.AllowPublicAccess.ToString & "." & community.AlwaysAllowAccessToCopyCenter.ToString)
        End If

        DVdescription.Visible = Not String.IsNullOrEmpty(description)
        LTdescription.Text = description

        DVotherDetails.Visible = showDetails
        If showDetails Then
            If DVenrollmentsDetails.Attributes("class").Contains(LTnoOtherDetailsCssClass.Text) Then
                DVenrollmentsDetails.Attributes("class") = Replace(DVenrollmentsDetails.Attributes("class"), LTnoOtherDetailsCssClass.Text, LTotherDetailsCssClass.Text)
            ElseIf Not DVenrollmentsDetails.Attributes("class").Contains(LTotherDetailsCssClass.Text) Then
                DVenrollmentsDetails.Attributes("class") &= " " & LTotherDetailsCssClass.Text
            End If
        Else
            If DVenrollmentsDetails.Attributes("class").Contains(LTotherDetailsCssClass.Text) Then
                DVenrollmentsDetails.Attributes("class") = Replace(DVenrollmentsDetails.Attributes("class"), LTotherDetailsCssClass.Text, LTnoOtherDetailsCssClass.Text)
            ElseIf Not DVenrollmentsDetails.Attributes("class").Contains(LTnoOtherDetailsCssClass.Text) Then
                DVenrollmentsDetails.Attributes("class") &= " " & LTnoOtherDetailsCssClass.Text
            End If
        End If
    End Sub
    Private Sub SetPhotoPath(p As lm.Comol.Core.DomainModel.litePerson)
        Dim path As String = ""
        If Not IsNothing(p) Then
            path = p.PhotoPath
        End If
        If String.IsNullOrEmpty(path) OrElse Not lm.Comol.Core.File.Exists.File(PageUtility.ProfilePath & p.Id & "/" & path) Then
            IMGavatar.ImageUrl = PageUtility.ApplicationUrlBase & LTdefaultImg.Text
        Else
            IMGavatar.ImageUrl = PageUtility.ApplicationUrlBase & "profili/" & p.Id & "/" & path
        End If
    End Sub
    Private Sub RPTenrollmentsInfo_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTenrollmentsInfo.ItemDataBound
        Dim oControl As HtmlGenericControl = e.Item.FindControl("SPNwaiting")
        Dim dto As lm.Comol.Core.BaseModules.Dashboard.Domain.dtoEnrollmentsDetailInfo = e.Item.DataItem
        oControl.Visible = (dto.Waiting > 0)
        If dto.Waiting > 0 Then
            Dim oLabel As Label = e.Item.FindControl("LBwaitingRoleOpen")
            Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBwaitingRoleClose")
            Resource.setLabel(oLabel)
        End If

    End Sub

    Private Sub AddToDetails(items As List(Of dtoDetail), titleKey As String, value As String)
        If Not String.IsNullOrEmpty(value) Then
            items.Add(New dtoDetail() With {.Title = Resource.getValue(titleKey), .Value = value})
        End If
    End Sub
    Private Sub AddToDetails(items As List(Of dtoDetail), titleKey As String, value As Integer)
        If value > 0 Then
            items.Add(New dtoDetail() With {.Title = Resource.getValue(titleKey), .Value = value})
        End If
    End Sub
    Public Class dtoDetail
        Public Title As String
        Public Value As String
    End Class
#End Region

End Class