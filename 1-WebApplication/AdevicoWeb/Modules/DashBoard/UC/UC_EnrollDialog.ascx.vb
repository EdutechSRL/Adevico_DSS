Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.Dashboard.Domain
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Public Class UC_EnrollDialog
    Inherits DBbaseControl
    Implements IViewConfirmEnrollTo

#Region "Implements"
#Region "Settings"
    Public Property isInitialized As Boolean Implements IViewConfirmEnrollTo.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public Property DisplayDescription As Boolean Implements IViewConfirmEnrollTo.DisplayDescription
        Get
            Return ViewStateOrDefault("DisplayDescription", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayDescription") = value
            Me.DVdescription.Visible = value
        End Set
    End Property
    Private Property UniqueIdentifier As Guid Implements IViewConfirmEnrollTo.UniqueIdentifier
        Get
            Return ViewStateOrDefault("UniqueIdentifier", Guid.Empty)
        End Get
        Set(value As Guid)
            ViewState("UniqueIdentifier") = value
        End Set
    End Property
    Private Property AllowEnroll As Boolean Implements IViewConfirmEnrollTo.AllowEnroll
        Get
            Return ViewStateOrDefault("AllowEnroll", True)
        End Get
        Set(value As Boolean)
            BTNapplyEnrollTo.Visible = value
            ViewState("AllowEnroll") = value
        End Set
    End Property
  
    Private Property CurrentItems As List(Of lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunityInfoForEnroll) Implements IViewConfirmEnrollTo.CurrentItems
        Get
            If Not IsNothing(Session("dtoCommunityInfoForEnroll_" & UniqueIdentifier.ToString)) AndAlso TypeOf (Session("dtoCommunityInfoForEnroll_" & UniqueIdentifier.ToString)) Is List(Of lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunityInfoForEnroll) Then
                Return DirectCast(Session("dtoCommunityInfoForEnroll_" & UniqueIdentifier.ToString), List(Of lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunityInfoForEnroll))
            Else
                Return New List(Of lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunityInfoForEnroll)
            End If
        End Get
        Set(value As List(Of lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunityInfoForEnroll))
            Session("dtoCommunityInfoForEnroll_" & UniqueIdentifier.ToString) = value
        End Set
    End Property

#End Region
#End Region

#Region "Internal"
    Public Event CloseWindow()
    Public Event EnrollToCommunities(items As List(Of lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunityInfoForEnroll))
    Public Event EnrollToCommunity(item As lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunityInfoForEnroll)
    Public Property IsMulti As Boolean
        Get
            Return ViewStateOrDefault("IsMulti", True)
        End Get
        Set(value As Boolean)
            ViewState("IsMulti") = value
        End Set
    End Property
    Public ReadOnly Property DialogIdentifier As String
        Get
            Return LTcssClassDialog.Text
        End Get
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

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setButton(BTNapplyEnrollTo, True)
            .setHyperLink(HYPcloseEnrollToConfirmDialog, False, True)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(item As lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunityInfoForEnroll, Optional description As String = "") Implements IViewConfirmEnrollTo.InitializeControl
        Dim enrollingCommunities As New List(Of lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunityInfoForEnroll)()
        enrollingCommunities.Add(item)
        InitializeControl(New List(Of lm.Comol.Core.BaseModules.CommunityManagement.dtoEnrollment), New List(Of lm.Comol.Core.BaseModules.CommunityManagement.dtoEnrollment), enrollingCommunities, description)
    End Sub
    Public Sub InitializeControl(enrolledItems As List(Of lm.Comol.Core.BaseModules.CommunityManagement.dtoEnrollment), notEnrolledItems As List(Of lm.Comol.Core.BaseModules.CommunityManagement.dtoEnrollment), enrollingCommunities As List(Of lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunityInfoForEnroll), Optional description As String = "") Implements IViewConfirmEnrollTo.InitializeControl
        If Not String.IsNullOrEmpty(description) AndAlso DisplayDescription Then
            DVdescription.Visible = True
            LBdescription.Text = description
        End If

        If Not enrolledItems.Any() AndAlso Not notEnrolledItems.Any() Then
            IsMulti = enrollingCommunities.Count > 1
        Else
            IsMulti = enrollingCommunities.Count > 1 OrElse (enrolledItems.GroupBy(Function(e) e.Status).Select(Function(e) e.Key).Count > 1) OrElse (enrolledItems.Any() AndAlso notEnrolledItems.Any()) OrElse (enrolledItems.Any() AndAlso enrollingCommunities.Any()) OrElse (notEnrolledItems.Any() AndAlso enrollingCommunities.Any())
        End If
        CurrentItems = enrollingCommunities
        InitializeMessage(enrolledItems, notEnrolledItems, enrollingCommunities)

    End Sub
    Public Sub ClearSession() Implements IViewConfirmEnrollTo.ClearSession
        Session.Remove("dtoCommunityInfoForEnroll_" & UniqueIdentifier.ToString)
    End Sub
#End Region

#Region "Internal"
    Private Sub InitializeMessage(enrolledItems As List(Of lm.Comol.Core.BaseModules.CommunityManagement.dtoEnrollment), notEnrolledItems As List(Of lm.Comol.Core.BaseModules.CommunityManagement.dtoEnrollment), enrollingCommunities As List(Of lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunityInfoForEnroll))
        If Not IsMulti AndAlso enrollingCommunities.Count = 1 Then
            SetSingleMessage(enrollingCommunities)
        ElseIf enrolledItems.Any() OrElse notEnrolledItems.Any() OrElse enrollingCommunities.Any Then
            Dim message As String = ""
            DVnoWarning.Visible = enrolledItems.Any()
            If enrolledItems.Any() Then
                message = "IViewConfirmEnrollTo.DVnoWarning."
                Select Case enrolledItems.Count
                    Case 1
                        message &= "1"
                    Case Else
                        message &= "n"
                End Select
                CTRLnoWarningMessage.InitializeControl(Resource.getValue(message), enrolledItems.Count, enrolledItems)
            End If
            DVerror.Visible = notEnrolledItems.Any()
            If notEnrolledItems.Any() Then
                message = "IViewConfirmEnrollTo.DVerror."
                Select Case notEnrolledItems.Count
                    Case 1
                        message &= "1"
                    Case Else
                        message &= "n"
                End Select
                CTRLerrorMessage.InitializeControl(Resource.getValue(message), notEnrolledItems.Count, notEnrolledItems)
            End If

            DVunableToUnsubscribe.Visible = enrollingCommunities.Where(Function(c) Not c.AllowUnsubscribe AndAlso Not c.HasConstraints).Any()
            If enrollingCommunities.Where(Function(c) Not c.AllowUnsubscribe AndAlso Not c.HasConstraints).Any() Then
                message = "IViewConfirmEnrollTo.DVunableToUnsubscribe."
                Select Case enrollingCommunities.Where(Function(c) Not c.AllowUnsubscribe AndAlso Not c.HasConstraints).Count
                    Case 1
                        message &= "1"
                    Case Else
                        message &= "n"
                End Select
                CTRLunableToUnsubscribeMessage.InitializeControl(Resource.getValue(message), enrollingCommunities.Where(Function(c) Not c.AllowUnsubscribe AndAlso Not c.HasConstraints).Count, enrollingCommunities.Where(Function(c) Not c.AllowUnsubscribe AndAlso Not c.HasConstraints).ToList())
            End If
            DVconflicts.Visible = enrollingCommunities.Where(Function(c) c.HasConstraints).Any()
            If enrollingCommunities.Where(Function(c) c.HasConstraints).Any() Then
                message = "IViewConfirmEnrollTo.DVconflicts."
                Select Case enrollingCommunities.Where(Function(c) c.HasConstraints).Count
                    Case 1
                        message &= "1"
                    Case Else
                        message &= "n"
                End Select
                CTRLconflicts.InitializeControl(Resource.getValue(message), enrollingCommunities.Where(Function(c) c.HasConstraints).Count, enrollingCommunities.Where(Function(c) c.HasConstraints).ToList())
            End If

            BTNapplyEnrollTo.Visible = enrollingCommunities.Any
        Else
            BTNapplyEnrollTo.Visible = False
        End If
    End Sub
    Private Sub SetSingleMessage(items As List(Of lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunityInfoForEnroll))
        Dim item As lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunityInfoForEnroll = items.FirstOrDefault()
        Dim message As String = ""
        If item.HasConstraints AndAlso item.Constraints.Where(Function(c) c.Type = lm.Comol.Core.DomainModel.ConstraintType.NotEnrolledTo).Any() Then
            DVconflicts.Visible = True

            Select Case item.Constraints.Where(Function(c) c.Type = lm.Comol.Core.DomainModel.ConstraintType.NotEnrolledTo).Count
                Case 1
                    message = Resource.getValue("IViewConfirmEnrollTo.Constraints.1")
                Case Else
                    message = String.Format(Resource.getValue("IViewConfirmEnrollTo.Constraints.n"), item.Constraints.Where(Function(c) c.Type = lm.Comol.Core.DomainModel.ConstraintType.NotEnrolledTo).Count)
            End Select
            CTRLconflicts.InitializeControlForConstraints(message, item.Constraints.Where(Function(c) c.Type = lm.Comol.Core.DomainModel.ConstraintType.NotEnrolledTo).ToList())
        Else
            DVconflicts.Visible = False
        End If
        DVunableToUnsubscribe.Visible = Not item.AllowUnsubscribe
        If Not item.AllowUnsubscribe Then
            CTRLunableToUnsubscribeMessage.InitializeControl(Resource.getValue("IViewConfirmEnrollTo.UnableToUnsubscribeMessage"))
        End If
    End Sub
    Protected Function GetDialogTitle() As String
        Return Resource.getValue("IViewConfirmEnrollTo.GetDialogTitle")
    End Function
    Protected Function CssClassDialog() As String
        Return IIf(CurrentItems.Count > 1, LTcssClassMulti.Text, LTcssClassSingle.Text)
    End Function
    Private Sub BTNapplyEnrollTo_Click(sender As Object, e As System.EventArgs) Handles BTNapplyEnrollTo.Click
        If IsMulti Then
            Dim idCommunites As List(Of Integer) = CTRLunableToUnsubscribeMessage.GetSelectedItems()
            RaiseEvent EnrollToCommunities(CurrentItems.Where(Function(c) c.HasConstraints OrElse (Not c.HasConstraints AndAlso c.ConfirmSubscription AndAlso idCommunites.Contains(c.Id))).ToList())
        Else
            RaiseEvent EnrollToCommunities(CurrentItems)
        End If
        ClearSession()
        isInitialized = False
        UniqueIdentifier = Guid.Empty
    End Sub

#End Region



    

   
   
End Class