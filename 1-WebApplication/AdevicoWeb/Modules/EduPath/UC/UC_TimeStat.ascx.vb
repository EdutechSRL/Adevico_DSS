Imports lm.Comol.Modules.EduPath.Domain
Imports lm.Comol.Modules.EduPath.BusinessLogic

Public Class UC_TimeStat
    Inherits BaseControlSession

    Public Event DateSelected(ByVal dateToView As DateTime)
    Public Event ViewCertifiedStat()


    'Private _isCertifiedView As Boolean
    'Private Property isCertifiedView As Boolean
    '    Get
    '        Return _isCertifiedView
    '    End Get
    '    Set(value As Boolean)
    '        _isCertifiedView = value
    '    End Set
    'End Property

    Private ReadOnly Property isOverEndDate As Boolean
        Get
            If EndDate Is Nothing Then
                Return False
            End If
            Return DateTime.Now > EndDate
        End Get
    End Property

    Private _endDate As DateTime?
    Private Property EndDate As DateTime?
        Get
            Return _endDate
        End Get
        Set(value As DateTime?)
            If value Is Nothing Then
                _endDate = DateTime.MaxValue
            Else
                _endDate = value
            End If

        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub


    Public Sub InitView(ByVal selectedDate As DateTime, ByVal endDateStr As String, ByVal overTimeStr As String, ByVal viewDateSelector As Boolean, ByVal isManageMode As Boolean, ByVal endDate As DateTime?)
        SetInternazionalizzazione()
        Me.EndDate = endDate

        If isManageMode Then


            If Not isOverEndDate Then
                'nascondere le cose da vedere se sono prima della end date
                'LKBviewCertified.CssClass = "hidden"
                ' DIVselect.Style("Display") = "None"
                LKBviewCertified.Visible = False
            End If

            'LKBviewNow.CssClass = "hidden"
            LKBviewNow.Visible = False
            If endDateStr.Length = 0 Then
                'nascondi tutto
                DIVendDate.Style("Display") = "None"
                ' DIVselect.Style("Display") = "None"

            Else
                'nascondo selettore
                Resource.setLabel(LBviewStat)
                Me.LBendDate.Text = Me.Resource.getValue("LBendDate.text") & endDateStr

                'INVERTIRE COMMENTI CON CODICE IN CASO DI ABILITAZIONE DELL'OVERTIME
                DIVoverTime.Style("Display") = "None"
                'If overTimeStr.Length = 0 Then
                '    'nascondo overtime
                '    DIVoverTime.Style("Display") = "None"

                'Else
                '    Me.LBoverTime.Text = Me.Resource.getValue("LBoverTime.text") & overTimeStr

                'End If
                'FINE INVERSIONE
            End If

            If viewDateSelector Then
                'view all
                TXBhour.Text = selectedDate.Hour
                TXBminuts.Text = selectedDate.Minute

                TXBhour.NumberFormat.DecimalDigits = 0
                TXBminuts.NumberFormat.DecimalDigits = 0

                RDPviewStat.SelectedDate = selectedDate
                Me.Resource.setLabel(LBviewStat)
            Else

                DIVoverTime.Style("Display") = "None"

            End If

        Else

            If endDateStr.Length = 0 Then
                'nascondi tutto
                DIVendDate.Style("Display") = "None"

            Else
                'nascondo selettore
                Me.LBendDate.Text = Me.Resource.getValue("LBendDate.text") & endDateStr

            End If

            If Not isOverEndDate Then
                'nascondere le cose da vedere se sono prima della end date
                'LKBviewCertified.CssClass = "hidden"
                ' LKBviewNow.CssClass = "hidden"
                LKBviewNow.Visible = False
                LKBviewCertified.Visible = False
            End If

            DIVselect.Style("Display") = "None"
            DIVoverTime.Style("Display") = "None"

        End If

    End Sub


    Public Function GetSelectedDate() As DateTime
        Dim retDate As New DateTime

        If Not RDPviewStat.SelectedDate Is Nothing Then

            retDate = RDPviewStat.SelectedDate

            If Not TXBhour.Text Is Nothing Then
                retDate = retDate.AddHours(TXBhour.Text)
            End If

            If Not TXBminuts.Text Is Nothing Then
                retDate = retDate.AddMinutes(TXBminuts.Text)
            End If
        Else
            retDate = DateTime.Now
        End If

        Return retDate

    End Function


    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_HelpEpRole", "EduPath")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLinkButton(LKBviewCertified, False, True)
            .setLinkButton(LKBviewNow, False, True)
            .setLinkButton(LKBviewStat, False, True)
        End With
    End Sub

    Private Sub LKBviewStat_Click(sender As Object, e As System.EventArgs) Handles LKBviewStat.Click
       
            RaiseEvent DateSelected(GetSelectedDate)
     
    End Sub


    Private Sub LKBviewCertified_Click(sender As Object, e As System.EventArgs) Handles LKBviewCertified.Click
        RaiseEvent ViewCertifiedStat()
    End Sub

    Private Sub LKBviewNow_Click(sender As Object, e As System.EventArgs) Handles LKBviewNow.Click
        RaiseEvent DateSelected(DateTime.Now)
    End Sub
End Class