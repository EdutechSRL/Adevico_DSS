Imports lm.Comol.Modules.CallForPapers.Advanced.dto
Imports lm.Comol.Modules.CallForPapers.Advanced.Presentation.iView
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports Adv = lm.Comol.Modules.CallForPapers.Advanced

Public Class Uc_AdvComments
    Inherits BaseControl
    Implements Adv.Presentation.iView.iViewUcAdvComments

    ''' <summary>
    ''' Inizializzazione controllo
    ''' </summary>
    ''' <param name="CommissionId">Id commissione</param>
    ''' <param name="SubmissionId">Id Sottomissione</param>
    Public Sub InitUc(ByVal CommissionId As Long, ByVal SubmissionId As Long)

        If CommissionId = 0 Then
            Me.RPTcomments.Visible = False
        Else
            Me.CurrentPresenter.InitView(CommissionId, SubmissionId)

        End If

    End Sub


#Region "Context"
    Private _Presenter As Adv.Presentation.AdvUcCommentsPresenter
    Private ReadOnly Property CurrentPresenter() As Adv.Presentation.AdvUcCommentsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New Adv.Presentation.AdvUcCommentsPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()

    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub



#Region "BaseView"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property

    Public Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout

    End Sub

    Public Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission

    End Sub

#End Region


    ''' <summary>
    ''' Inizializzazione view
    ''' </summary>
    ''' <param name="comments">Elenco commenti</param>
    Public Sub BindView(comments As IList(Of dtoAdvComment)) Implements iViewUcAdvComments.BindView

        If Not IsNothing(comments) OrElse Not comments.Any() Then
            Me.RPTcomments.Visible = True
            Me.RPTcomments.DataSource = comments
            Me.RPTcomments.DataBind()
        Else
            Me.RPTcomments.Visible = False
        End If


    End Sub

    Private Sub RPTcomments_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTcomments.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim Comment As dtoAdvComment = e.Item.DataItem

            If Not IsNothing(Comment) Then

                Dim LT As Literal = e.Item.FindControl("LTstatus")

                If Not IsNothing(LT) Then
                    Dim css As String = If((Comment.isDraft), "draft", "confirm")
                    Dim text As String = If((Comment.isDraft), "Bozza", "Confermata")

                    LT.Text = String.Format(LT.Text, css, text)
                End If

                Dim LB As Label = e.Item.FindControl("LBLmember")
                If Not IsNothing(LB) Then
                    LB.Text = Comment.MemberName
                End If

                LB = e.Item.FindControl("LBLdate")
                If Not IsNothing(LB) Then
                    If IsNothing(Comment.SaveOn) Then
                        LB.Visible = False
                    Else
                        LB.Visible = True
                        LB.Text = Comment.SaveOn
                    End If

                End If

                LB = e.Item.FindControl("LBLcriteria")
                If Not IsNothing(LB) Then
                    If Comment.IsCriteria Then
                        LB.Visible = True
                        LB.Text = Comment.CriteriaName
                    Else
                        LB.Visible = False
                    End If
                End If

                LB = e.Item.FindControl("LBLcomment")
                If Not IsNothing(LB) Then
                    LB.Text = Comment.Comment
                End If

            End If

        End If
    End Sub
End Class