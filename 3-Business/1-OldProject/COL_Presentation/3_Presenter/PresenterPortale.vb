Imports COL_BusinessLogic_v2.Comunita
Imports Comol.Entity.Configuration

Public Class PresenterPortale
	Inherits GenericPresenter

	Public Sub New(ByVal view As IviewPortale)
		MyBase.view = view
	End Sub
	Private Shadows ReadOnly Property View() As IviewPortale
		Get
			View = MyBase.view
		End Get
	End Property
	Public Sub Init()
        '   Me.SetDisplay()
	End Sub

    Public Sub ChangeLanguage(ByVal LanguageID As Integer)
        Me.View.CurrentLanguageID = LanguageID
    End Sub

    'Private Sub SetDisplay()
    '	Dim oTelephoneNumber As String = ""
    '       Try
    '           Dim oMailLocalized As MailLocalized = Me.View.MailConfig
    '           IIf(Me.View.DefaultSetting.ShowTel, oTelephoneNumber = Me.View.DefaultSetting.TelephoneNumber, oTelephoneNumber = "")
    '           Me.View.SetDisplayNote(oMailLocalized.SubjectPrefix, oMailLocalized.SystemSender.Address, oMailLocalized.SystemSender.Address)
    '           Me.View.SetDisplaySegnalazione(oMailLocalized.SubjectPrefix, oMailLocalized.SystemSender.Address, oMailLocalized.SystemSender.Address, oTelephoneNumber, CreateWorkDayDisplay)
    '       Catch ex As Exception
    '           Console.WriteLine("ErrorE: " & ex.Message)
    '           If Not IsNothing(ex.InnerException) Then
    '               Console.WriteLine("ErrorE: " & ex.InnerException.Message)
    '           End If
    '       End Try

    'End Sub
End Class