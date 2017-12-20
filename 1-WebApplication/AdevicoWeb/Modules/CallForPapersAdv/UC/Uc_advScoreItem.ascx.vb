Public Class Uc_advScoreItem
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub


    Public Sub InitUC(
                   score As String, minScore As String,
                   boolCount As Integer, boolTot As Integer,
                   success As Boolean,
                   showScore As Boolean)
        ',
        '           ShowBoolean As Boolean,
        '           Shownumeric As Boolean)

        If showScore Then

            If (String.IsNullOrEmpty(minScore)) Then
                LTscoreNum.Text = String.Format(LTscoreNum.Text, score, "")
            Else
                LTscoreNum.Text = String.Format(LTscoreNum.Text, score, "/" & minScore)
            End If
        Else
            LTscoreNum.Text = ""
        End If

        If (boolTot <= 0) Then
            LTboolCount.Text = "" 'String.Format(LTboolCount.Text, boolCount, "")
        Else
            LTboolCount.Text = String.Format(LTboolCount.Text, boolCount, "/" & boolTot)
        End If

        If (success) Then
            LTscoreSuccess.Text = String.Format(LTscoreSuccess.Text, "success", "Criteri superati")
        Else
            LTscoreSuccess.Text = String.Format(LTscoreSuccess.Text, "fail", "Criteri non superati")
        End If

        'Me.PHscoreBoolean.Visible = ShowBoolean
        'Me.PHscoreNumeric.Visible = Shownumeric
    End Sub

    Public Sub InitUCStep(
                   score As String,
                   boolRating As Boolean,
                   success As Boolean,
                   RatingText As String,
                   showScore As Boolean)
        ',
        'ShowBoolean As Boolean,
        'Shownumeric As Boolean)

        InitUC(score, "", 0, -1, success, showScore) ', "") ', ShowBoolean, Shownumeric)

        LTboolRating.Visible = True
        LTboolCount.Visible = False

        If (boolRating) Then
            LTboolRating.Text = String.Format(LTscoreSuccess.Text, "success", String.Format("{0}: {1}", RatingText, "Criteri binari marcati"))
        Else
            LTboolRating.Text = String.Format(LTscoreSuccess.Text, "fail", String.Format("{0}: {1}", RatingText, "Criteri binari non marcati"))
        End If


    End Sub


End Class