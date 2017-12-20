Public Class UC_ConstraintsDetails
    Inherits DBbaseControl

#Region "Internal"
    Private _ContainerCssClass As String
    Private _MessageCssClass As String
    Public Property ContainerCssClass As String
        Get
            Return _ContainerCssClass
        End Get
        Set(value As String)
            _ContainerCssClass = value
        End Set
    End Property
    Public Property MessageCssClass As String
        Get
            Return _MessageCssClass
        End Get
        Set(value As String)
            _MessageCssClass = value
        End Set
    End Property
    Public Property Message As String
        Get
            Return LTmessage.text
        End Get
        Set(value As String)
            LTmessage.text = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region
  

#Region "Internal"
    Public Sub InitializeControl(constraints As List(Of lm.Comol.Core.DomainModel.dtoCommunityConstraint))
        RPTconstraints.DataSource = constraints
        RPTconstraints.DataBind()
    End Sub
    Private Sub RPTconstraints_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTconstraints.ItemDataBound
        Dim item As lm.Comol.Core.DomainModel.dtoCommunityConstraint = e.Item.DataItem

        Dim oLiteral As Literal = e.Item.FindControl("LTconstraintReason")
        oLiteral.Text = Resource.getValue("LTconstraintReason.ConstraintType." & item.Type.ToString)
        oLiteral.Text = String.Format(oLiteral.Text, item.Name)
        oLiteral = e.Item.FindControl("LTconstraintStatus")
        Select Case item.Type
            Case lm.Comol.Core.DomainModel.ConstraintType.EnrolledTo
                If item.IsRespected Then
                    oLiteral.Text = Resource.getValue("LTconstraintStatus.ConstraintType." & item.Type.ToString & "." & item.IsRespected.ToString)
                End If
            Case lm.Comol.Core.DomainModel.ConstraintType.NotEnrolledTo
                If Not item.IsRespected Then
                    oLiteral.Text = Resource.getValue("LTconstraintStatus.ConstraintType." & item.Type.ToString & "." & item.IsRespected.ToString)
                End If
        End Select

        Resource.setLiteral(oLiteral)
    End Sub
    Public Function GetCssClass(constraint As lm.Comol.Core.DomainModel.dtoCommunityConstraint)
        Dim cssClass As String = ""
        If constraint.IsRespected AndAlso constraint.Type = lm.Comol.Core.DomainModel.ConstraintType.EnrolledTo Then
            cssClass = LTcssClassPassed.Text
        ElseIf Not constraint.IsRespected Then
            If constraint.Type = lm.Comol.Core.DomainModel.ConstraintType.EnrolledTo Then
                cssClass = LTcssClassWaiting.Text
            Else
                cssClass = LTcssClassNotPassed.Text
            End If
        Else
            cssClass = LTcssClassWaiting.Text
        End If
        Return cssClass
    End Function
#End Region

   
End Class