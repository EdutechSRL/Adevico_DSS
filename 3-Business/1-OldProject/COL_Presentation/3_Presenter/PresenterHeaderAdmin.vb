'Imports COL_BusinessLogic_v2
'Imports COL_BusinessLogic_v2.Comunita
'Imports COL_BusinessLogic_v2.FileLayer
'Imports lm.Comol.Core.File

'Public Class PresenterHeaderAdmin
'	Inherits GenericPresenter

'	Private Shadows ReadOnly Property View() As IviewHeaderAdmin
'		Get
'			View = MyBase.view
'		End Get
'	End Property

'	Public Sub New(ByVal view As IviewBase)
'		MyBase.view = view
'	End Sub

'	Public Sub Initialize()
'		If Me.View.ShowLogo Then
'			Me.View.CaricaLogo(Me.Bind_Logo())
'		End If
'		Dim oListaHistory As HistoryElement
'		oListaHistory = New HistoryElement(0, 0, Me.View.Resource.getValue("home"), "", Me.View.TipoPersonaID, Nothing, False)
'		Me.View.CaricaHistory(oListaHistory)
'	End Sub

'#Region "Logo"
'	Private Function Bind_Logo() As LinkElement
'		Return Me.GetLogoIstituzione()
'	End Function
'	Private Function GetLogoIstituzione() As LinkElement
'		Dim iResponse As LinkElement = Nothing

'		Dim oIstituzione As New COL_Istituzione
'		Try
'			oIstituzione.Id = Me.View.IstituzioneID
'			oIstituzione.Estrai()

'			If oIstituzione.Logo <> "" Then
'                If Exists.File(Me.View.BaseUrlDrivePath & "images/Logo/" & oIstituzione.Logo) Then
'                    iResponse = New LinkElement
'                    iResponse.ImageUrl = Me.View.BaseUrl & "images/Logo/" & oIstituzione.Logo
'                    If oIstituzione.HomePage <> "" Then
'                        If InStr(oIstituzione.HomePage, "http://") > 0 Then
'                            iResponse.Url = oIstituzione.HomePage
'                        Else
'                            iResponse.Url = "http://" & oIstituzione.HomePage
'                        End If
'                    Else
'                        iResponse.Url = ""
'                    End If
'                End If
'			End If
'		Catch ex As Exception

'		End Try
'		Return iResponse
'	End Function
'#End Region

'#Region "Menu"
'	Public Sub RetrieveMenu()
'		Dim items As New GenericCollection(Of MenuElement)

'		items = COL_MenuLimbo.LazyGeneraMenu(Me.View.UtenteCorrente.TipoPersona.id, Me.View.LinguaID, Main.FiltroMenuLimbo.LimboForAdmin)
'		Me.View.GeneraMenu(items)
'	End Sub
'#End Region

'#Region "Post-IT"
'	Public Sub RetrievePostIT()
'		Me.SetPostITgenerale()
'	End Sub
'	Private Sub SetPostITgenerale()
'		If Me.View.ShowPostItSistema Then
'			Try
'				If Me.View.PostItSistema.id = -9999 Then
'					With Me.View.PopUpAvvisoGenerale
'						.ID = "generale0"
'						.DragDrop = True
'						.HideAfter = -1
'						.OffsetX = 230	'+ (i * 25)
'						.AutoShow = True
'						.Width = System.Web.UI.WebControls.Unit.Pixel(260)
'						.Height = System.Web.UI.WebControls.Unit.Pixel(110)

'						If Equals(Me.View.PostItSistema.dataScadenza, New Date) Then
'							.Visible = True
'							.Title = "! ATTENZIONE !"
'						Else
'							If Me.View.PostItSistema.dataScadenza <= Now() Then
'								.Visible = True
'								.Title = "ATTENZIONE (Scade:" & Me.View.PostItSistema.dataScadenza & ")"
'							End If
'						End If

'						.Text = Replace(Me.View.PostItSistema.testo, vbCrLf, "<br>")
'						If InStr(Me.View.PostItSistema.testo, "http://") <> 0 Then
'							.Text = Me.Link_Href(.Text)
'						End If
'						.Text = Me.GetEmoticon(.Text)
'						.Message = "<img src=./../images/persona.gif border=0 >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & "Visualizza tutto il testo &nbsp;&nbsp;&nbsp;&nbsp; <br/>" & .Text

'						.ColorStyle = Me.View.PostItSistema.Color
'						.Visible = True
'						.ActionType = EeekSoft.Web.PopupAction.MessageWindow
'					End With
'				Else
'					Me.View.PopUpAvvisoGenerale.Visible = False
'				End If
'			Catch ex As Exception
'				Me.View.PopUpAvvisoGenerale.Visible = False
'			End Try
'		Else
'			Me.View.PopUpAvvisoGenerale.Visible = False
'		End If
'	End Sub
'	Private Function GetEmoticon(ByVal testo As String) As String
'		Dim smile(22) As String
'		Dim smileimages(22) As String
'		smile(1) = ":-)"
'		smile(2) = ":D"
'		smile(3) = ":-O"
'		smile(4) = ":-p"
'		smile(5) = ";-)"
'		smile(6) = "(H)"
'		smile(7) = ":$"
'		smile(8) = "|-)"
'		smile(9) = ":("
'		smile(10) = ";-("
'		smile(11) = ":S"
'		smile(12) = ":@"
'		smile(13) = "(*)"
'		smile(14) = "(L)"
'		smile(15) = "(U)"
'		smile(16) = "(Y)"
'		smile(17) = "(N)"
'		smile(18) = "(pp)"
'		smile(19) = "8-|"
'		smile(20) = "(6)"
'		smile(21) = "(?)"

'		smileimages(1) = "smiley1.gif"
'		smileimages(2) = "smiley4.gif"
'		smileimages(3) = "smiley3.gif"
'		smileimages(4) = "smiley17.gif"
'		smileimages(5) = "smiley2.gif"
'		smileimages(6) = "smiley16.gif"
'		smileimages(7) = "smiley9.gif"
'		smileimages(8) = "smiley12.gif"
'		smileimages(9) = "smiley6.gif"
'		smileimages(10) = "smiley19.gif"
'		smileimages(11) = "smiley5.gif"
'		smileimages(12) = "smiley7.gif"
'		smileimages(13) = "smiley10.gif"
'		smileimages(14) = "smiley27.gif"
'		smileimages(15) = "smiley28.gif"
'		smileimages(16) = "smiley20.gif"
'		smileimages(17) = "smiley21.gif"
'		smileimages(18) = "smiley32.gif"
'		smileimages(19) = "smiley23.gif"
'		smileimages(20) = "smiley15.gif"
'		smileimages(21) = "smiley25.gif"

'		Dim i As Integer
'		For i = 1 To 21
'			testo = testo.Replace(smile(i), "<img src=""" & Me.View.BaseUrl & "images/forum/smile/" & smileimages(i) & """>")
'		Next

'		Return testo
'	End Function
'	Private Function Link_Href(ByVal testo As String) As String
'		Dim tmp, finish, i, n As Integer
'		Dim testo1, testo2 As String
'		testo1 = testo
'		While finish <> 1
'			tmp = InStr(testo1, "http://")
'			If tmp = 0 Then
'				finish = 1
'			Else
'				Mid(testo1, tmp, 1) = "l"
'				n = n + 1
'			End If
'		End While
'		Dim address(n - 1) As Integer
'		Dim linky(n - 1) As String
'		Dim txt(n + 1) As String
'		Dim c(n - 1) As Integer
'		tmp = 0
'		finish = 0
'		n = 0
'		testo2 = testo
'		While finish <> 1
'			tmp = InStr(testo2, "http://")
'			If tmp = 0 Then
'				finish = 1
'			Else
'				Mid(testo2, tmp, 1) = "l"
'				address(n) = tmp
'				n = n + 1
'			End If
'		End While
'		'For i = 0 To address.Length() - 1
'		'    Response.Write(address(i) & "<br>")
'		'Next
'		n = 0
'		Dim link, search, totalText, temp, temptxt As String
'		Dim v, l, u, s, r, t, g, ITemp As Integer

'		For i = 0 To address.Length() - 1
'			For l = 1 To Len(testo)
'				If address(i) <> 0 And l = address(i) Then
'					For v = address(i) To Len(testo)
'						If Mid(testo, v, 1) <> " " And u = 0 Then
'							link += Mid(testo, v, 1)
'							s = s + 1
'						End If
'						If Mid(testo, v, 1) = " " Or v = Len(testo) Then
'							u = 1
'							r = 1
'							linky(n) += "<a href='" & link & "' target=blank >" & link & "</a> "
'							c(n) = s
'							n += 1
'							Exit For
'						End If
'					Next
'					l = l + s

'				Else
'					If i = 0 And l < address(i) Then
'						txt(n) += Mid(testo, l, 1)
'					Else
'						If i > 0 Then
'							If l > ((address(i - 1) + c(i - 1)) - 1) And l < address(i) Then
'								txt(n) += Mid(testo, l, 1)
'							End If
'						End If
'					End If
'					If (i = address.Length - 1) And l > (address(i) - 1) And l <= Len(testo) Then
'						txt(n) += Mid(testo, l, 1)
'					End If
'				End If
'			Next
'			s = 0
'			u = 0
'			link = ""
'		Next
'		Dim TxtLink As String = ""
'		For i = 0 To linky.Length() - 1
'			TxtLink += txt(i) & linky(i)
'		Next
'		If txt(linky.Length()) <> Nothing Then
'			TxtLink += txt(linky.Length())
'		End If
'		Return TxtLink
'	End Function
'#End Region

'#Region "Cambio Comunita"
'	Public Sub GoToComunita(ByVal ComunitaID As Integer)
'        Me.View.CambiaComunitaFromHistory(ComunitaID)
'	End Sub
'#End Region

'End Class
