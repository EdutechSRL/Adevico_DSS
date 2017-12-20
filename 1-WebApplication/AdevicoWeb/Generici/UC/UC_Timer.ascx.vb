Public Partial Class UC_Timer
    Inherits BaseControlSession
	Private _AutoUpdateAction As Boolean

	Public Property AutoUpdateAction() As Boolean
		Get
			Return _AutoUpdateAction
		End Get
		Set(ByVal value As Boolean)
			_AutoUpdateAction = value
		End Set
	End Property
	Public Event UpdateAction()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

	Protected Sub TMSessione_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles TMSessione.Tick
		If AutoUpdateAction Then
			RaiseEvent UpdateAction()
		End If
		'If isFirstRun Then
		'    TMSessione.Interval = RootObject.tickMassimo
		'    startTime = Now
		'    isFirstRun = False
		'    'If Not (MLVquestionari.ActiveViewIndex = 0) Then
		'    '    TMSessione.Enabled = False
		'    '    TMDurata.Enabled = False
		'    'End If
		'End If
		'If DateDiff("n", startTime, Now) > RootObject.vitaSessione_max Then
		'    Session("isSessioneScaduta") = True
		'    Response.Redirect(RootObject.compileUrlUI_short)
		'End If
	End Sub

    Public Sub Init()
        Dim Intervallo As Integer = Me.PageUtility.SystemSettings.Presenter.AjaxTimer
        Me.TMSessione.Interval = Intervallo
    End Sub

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetCultureSettings()

    End Sub

    Public Overrides Sub SetInternazionalizzazione()

	End Sub

End Class