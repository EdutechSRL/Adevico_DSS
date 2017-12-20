Public Partial Class UC_TextEditor
    Inherits BaseControlSession
	Implements IviewEditor


	Public ReadOnly Property CurrentCommunity() As Comol.Entity.Community Implements PresentationLayer.IviewEditor.CurrentCommunity
		Get
			If Me.ComunitaCorrenteID = 0 Then
				Return Nothing
			Else
				Return New Community(Me.ComunitaCorrente.Id, Me.ComunitaCorrente.Nome, Me.ComunitaCorrente.IdPadre)
			End If
		End Get
	End Property
	Public ReadOnly Property CurrentUser() As Comol.Entity.Person Implements PresentationLayer.IviewEditor.CurrentUser
		Get
			'Return New Person(1)
			If MyBase.UtenteCorrente Is Nothing Then
				Return Nothing
			Else
				Return New Person(MyBase.UtenteCorrente.ID, MyBase.UtenteCorrente.Nome, MyBase.UtenteCorrente.Cognome)
			End If
		End Get
	End Property
	Public ReadOnly Property UserLanguage() As Comol.Entity.Lingua Implements PresentationLayer.IviewEditor.UserLanguage
		Get
			Return MyBase.UserSessionLanguage
		End Get
	End Property


#Region "Editor properties"
	Public Property HTML() As String Implements PresentationLayer.IviewEditor.HTML
		Get
			Return Me.TXBanteprima.Text
		End Get
		Set(ByVal value As String)
			If Me.EditorMaxChar > 0 Then
				If Len(value) > Me.EditorMaxChar Then
					Me.LBcharUsed.Text = 0
					Me.TXBanteprima.Text = Left(value, Me.EditorMaxChar)
				Else
					Me.TXBanteprima.Text = value
					Me.LBcharUsed.Text = Me.EditorMaxChar - Len(value)
				End If
			Else
				Me.TXBanteprima.Text = value
			End If
		End Set
	End Property
	Public ReadOnly Property Text() As String Implements PresentationLayer.IviewEditor.Text
		Get
			Return IIf(Me.TXBanteprima.Text = "", "", HTMLHelpers.StripHTML(Me.TXBanteprima.Text))
		End Get
	End Property
	Public Property ShowScrollingSpeed() As Boolean Implements PresentationLayer.IviewEditor.ShowScrollingSpeed
		Get
			ShowScrollingSpeed = (Me.DIVmenuScrolling.Style("display") = "block")
		End Get
		Set(ByVal value As Boolean)
			If value Then
				Me.DIVmenuScrolling.Style("display") = "block"
			Else
				Me.DIVmenuScrolling.Style("display") = "none"
			End If
		End Set
	End Property
	Public Property AutoScrollingSpeed() As ScrollingSpeed Implements PresentationLayer.IviewEditor.AutoScrollingSpeed
		Get
			If Me.DDLscorrimentoHTML.SelectedValue = -1 Then
				Return ScrollingSpeed.Slow
			Else
				Return Me.DDLscorrimentoHTML.SelectedValue
			End If
		End Get
		Set(ByVal value As ScrollingSpeed)
			Try
				Me.DDLscorrimentoHTML.SelectedValue = value
			Catch ex As Exception
				Me.DDLscorrimentoHTML.SelectedValue = ScrollingSpeed.Slow
			End Try
		End Set
	End Property
	Public Property EditorEnabled() As Boolean Implements PresentationLayer.IviewEditor.EditorEnabled
		Get
			EditorEnabled = Me.TXBanteprima.Enabled
		End Get
		Set(ByVal value As Boolean)
			Me.TXBanteprima.Enabled = value
			Me.DDLFont.Enabled = value
			Me.DDLFontSize.Enabled = value
			Me.DDLscorrimentoHTML.Enabled = value
			Me.IMBcenter.Enabled = value
			Me.IMBcolorB.Enabled = value
			Me.IMBcolorBlu.Enabled = value
			Me.IMBcolorG.Enabled = value
			Me.IMBcolorN.Enabled = value
			Me.IMBcolorR.Enabled = value
			Me.IMBcolorV.Enabled = value
			Me.IMBGrassetto.Enabled = value
			Me.IMBitalic.Enabled = value
			Me.IMBleft.Enabled = value
			Me.IMBright.Enabled = value
			Me.IMBright.Enabled = value
			Me.IMBsottolineato.Enabled = value
			Me.IMBsottolineato.Enabled = value
		End Set
	End Property
	Public Property EditorColumnsNumber() As Integer
		Get
			Return Me.TXBanteprima.Columns
		End Get
		Set(ByVal value As Integer)
			Me.TXBanteprima.Columns = value
		End Set
	End Property
	Public Property EditorRowNumber() As Integer
		Get
			EditorRowNumber = Me.TXBanteprima.Rows
		End Get
		Set(ByVal value As Integer)
			Me.TXBanteprima.Rows = value
		End Set
	End Property

	Public Property EditorMaxChar() As Long Implements PresentationLayer.IviewEditor.EditorMaxChar
		Get
			Try
				Return DirectCast(Me.ViewState("EditorMaxChar"), Long)
			Catch ex As Exception

			End Try
		End Get
		Set(ByVal value As Long)
			If value = 0 Then
				Me.DIVmaxChar.Attributes.Add("display", "none")
			Else
				Me.DIVmaxChar.Attributes.Add("display", "block")

				If Len(Me.TXBanteprima.Text) > value Then
					Me.LBcharUsed.Text = 0
					Me.TXBanteprima.Text = Left(Me.TXBanteprima.Text, value)
				Else
					Me.LBcharUsed.Text = value - Len(Me.TXBanteprima.Text)
				End If
			End If
			Me.ViewState("EditorMaxChar") = value
			Me.LTmaxChar.Text = value
		End Set
	End Property
	Public Property ImagesPaths() As String() Implements PresentationLayer.IviewEditor.ImagesPaths
		Get

		End Get
		Set(ByVal value As String())

		End Set
	End Property
	Public WriteOnly Property AllowPreview() As Boolean Implements PresentationLayer.IviewEditor.AllowPreview
		Set(ByVal value As Boolean)
			Me.ViewState("AllowPreview") = value
		End Set
	End Property
	Public Property FontNames() As String Implements PresentationLayer.IviewEditor.FontNames
		Get

		End Get
		Set(ByVal value As String)

		End Set
	End Property

	Public Property FontSizes() As String Implements PresentationLayer.IviewEditor.FontSizes
		Get

		End Get
		Set(ByVal value As String)

		End Set
	End Property

	Public Property ShowAddSmartTag() As Boolean Implements PresentationLayer.IviewEditor.ShowAddSmartTag
		Get
			If Me.ViewState("ShowAddSmartTag") = "" Then
				Return False
			Else
				Return Me.ViewState("ShowAddSmartTag")
			End If
		End Get
		Set(ByVal value As Boolean)
			Me.ViewState("ShowAddSmartTag") = value
		End Set
	End Property
	Public WriteOnly Property ShowAdvancedControlsImage() As Boolean Implements PresentationLayer.IviewEditor.ShowAddImage
		Set(ByVal value As Boolean)

		End Set
	End Property
#End Region

#Region "Editor"
	Public Property EditorAlign() As String
		Get
			EditorAlign = Me.DIVeditorHTML.Attributes("align")
		End Get
		Set(ByVal value As String)
			Me.DIVeditorHTML.Attributes("align") = value
		End Set
	End Property
	Public Property Alignment() As String
		Get
			Alignment = Me.HDNalign.Value
		End Get
		Set(ByVal value As String)
			If value = "" Then
				value = "left"
			End If
			Me.HDNalign.Value = value
			Me.TXBanteprima.Style.Item("textalign") = value
			Me.TXBanteprima.Style.Item("text-align") = value
			Me.TXBanteprima.Style.Item("align") = value
			Me.IMBleft.ImageUrl = Me.BaseUrl & "images/Editors/left.gif"
			Me.IMBcenter.ImageUrl = Me.BaseUrl & "images/Editors/centrato.gif"
			Me.IMBright.ImageUrl = Me.BaseUrl & "images/Editors/right.gif"
			Select Case value
				Case "left"
					Me.IMBleft.ImageUrl = Me.BaseUrl & "images/Editors/leftO.gif"
				Case "center"
					Me.IMBcenter.ImageUrl = Me.BaseUrl & "images/Editors/centratoO.gif"
				Case "right"
					Me.IMBright.ImageUrl = Me.BaseUrl & "images/Editors/rightO.gif"
			End Select
		End Set
	End Property
	Public Property BackGround() As String
		Get
			BackGround = Me.HDNbackground.Value
		End Get
		Set(ByVal value As String)
			If value = "" Then
				value = "white"
			End If
			Me.HDNbackground.Value = value
			Me.TXBanteprima.Style.Item("background-Color") = value
			Me.TXBanteprima.Style.Item("backgroundColor") = value
		End Set
	End Property
	Public Property FontColor() As String
		Get
			FontColor = Me.HDNcolor.Value
		End Get
		Set(ByVal value As String)
			If value = "" Then
				value = "black"
			End If
			Me.HDNcolor.Value = value
			Me.TXBanteprima.Style("color") = value
		End Set
	End Property
	Public Property FontFace() As String
		Get
			FontFace = Me.DDLFont.SelectedValue
		End Get
		Set(ByVal value As String)
			If value = "" Then
				value = "Verdana"
			End If
			Me.TXBanteprima.Style("fontFamily") = value
			Try
				Me.DDLFont.SelectedValue = value
			Catch ex As Exception

			End Try
		End Set
	End Property
	Public Property FontSize() As Integer
		Get
			FontSize = DDLFontSize.SelectedValue
		End Get
		Set(ByVal value As Integer)
			If value = 0 Then
				value = 2
			End If
			Me.DDLFontSize.SelectedValue = value
			setSizeEditorHTML(value)
		End Set
	End Property
	Private Sub setSizeEditorHTML(ByVal Size As Integer)
		Select Case Size
			Case 1
				Me.TXBanteprima.Style.Add("fontSize", "8pt")
			Case 2
				Me.TXBanteprima.Style.Add("fontSize", "10pt")
			Case 3
				Me.TXBanteprima.Style.Add("fontSize", "12pt")
			Case 4
				Me.TXBanteprima.Style.Add("fontSize", "14pt")
			Case 5
				Me.TXBanteprima.Style.Add("fontSize", "18pt")
			Case 6
				Me.TXBanteprima.Style.Add("fontSize", "24pt")
			Case 7
				Me.TXBanteprima.Style.Add("fontSize", "36pt")
		End Select
	End Sub
	Public Property DisabledTags() As String Implements PresentationLayer.IviewEditor.DisabledTags
		Get

		End Get
		Set(ByVal value As String)

		End Set
	End Property
	Public Sub SetAdvancedTools(ByVal oList As System.Collections.Generic.List(Of Comol.Entity.SmartTag)) Implements PresentationLayer.IviewEditor.SetAdvancedTools

	End Sub
#End Region

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Me.Page.IsPostBack = False Then
			Me.SetInternazionalizzazione()
			Me.BindDati()
		End If
	End Sub

	Public Overrides Sub BindDati()
		Dim insertCode, InsertText As String
		insertCode = Me.Resource.getValue("insertCode")
		InsertText = Me.Resource.getValue("InsertText")

		insertCode = Replace(insertCode, "'", "\'")
		InsertText = Replace(InsertText, "'", "\'")
		Me.DDLFont.Attributes.Add("onchange", "AggiornaFont();")
		Me.DDLFontSize.Attributes.Add("onchange", "AggiornaFont();")
		Me.IMBGrassetto.Attributes.Add("onClick", "addTag('b','" & Replace(InsertText, "#code#", "b") & "') ;return false;")
		Me.IMBitalic.Attributes.Add("onClick", "addTag('i','" & Replace(InsertText, "#code#", "b") & "');return false;")
		Me.IMBsottolineato.Attributes.Add("onClick", "addTag('u','" & Replace(InsertText, "#code#", "b") & "');return false;")

		Me.IMGunderColoreB.Attributes.Add("onClick", "AggiornaFontSfondo('white');return false;")
		Me.IMGunderColoreN.Attributes.Add("onClick", "AggiornaFontSfondo('black');return false;")
		Me.IMBcolorN.Attributes.Add("onClick", "AggiornaFontColor('black');return false;")
		Me.IMBcolorG.Attributes.Add("onClick", "AggiornaFontColor('#666666');return false;")
		Me.IMBcolorB.Attributes.Add("onClick", "AggiornaFontColor('white');return false;")
		Me.IMBcolorR.Attributes.Add("onClick", "AggiornaFontColor('red');return false;")
		Me.IMBcolorV.Attributes.Add("onClick", "AggiornaFontColor('#009966');return false;")
		Me.IMBcolorBlu.Attributes.Add("onClick", "AggiornaFontColor('#3366FF');return false;")

		Me.IMBleft.Attributes.Add("onClick", "AggiornaAlign('left');return false;")
		Me.IMBright.Attributes.Add("onClick", "AggiornaAlign('right');return false;")
		Me.IMBcenter.Attributes.Add("onClick", "AggiornaAlign('center');return false;")
		If Me.EditorMaxChar > 0 Then
			Me.TXBanteprima.Attributes.Add("onkeypress", "CharNumber();")
			Me.TXBanteprima.Attributes.Add("onKeyDown", "return CharNumber();")
			Me.TXBanteprima.Attributes.Add("onKeyUp", "return CharNumber();")
			Me.TXBanteprima.Attributes.Add("onBlur", "return CharNumber();")
			Me.TXBanteprima.Attributes.Add("onFocus", "return CharNumber();")
		End If
	End Sub

	Public Overrides Sub SetCultureSettings()
		MyBase.SetCulture("pg_UC_Editor", "UC")
	End Sub
	Public Overrides Sub SetInternazionalizzazione()
		With MyBase.Resource
			.setImageButton(Me.IMBGrassetto, False, True, True)
			.setImageButton(Me.IMBitalic, False, True, True)
			.setImageButton(Me.IMBsottolineato, False, True, True)
			.setImageButton(Me.IMBleft, False, True, True)
			.setImageButton(Me.IMBcenter, False, True, True)
			.setImageButton(Me.IMBright, False, True, True)
			.setImage(Me.IMGcolor, True)
			.setImageButton(Me.IMBcolorN, False, True, True)
			.setImageButton(Me.IMBcolorG, False, True, True)
			.setImageButton(Me.IMBcolorB, False, True, True)
			.setImageButton(Me.IMBcolorR, False, True, True)
			.setImageButton(Me.IMBcolorV, False, True, True)
			.setImageButton(Me.IMBcolorBlu, False, True, True)
			.setImage(Me.IMGsfondo, True)
			.setImageButton(Me.IMGunderColoreB, False, True, True)
			.setImageButton(Me.IMGunderColoreN, False, True, True)
			Dim oValueList As New List(Of String)
			For Each oItem As ListItem In Me.DDLscorrimentoHTML.Items
				oValueList.Add(oItem.Value)
			Next
			.TranslateDropDownList(Me.DDLscorrimentoHTML, oValueList)
			.TranslateDropDownList(Me.DDLFont, Nothing)
			.TranslateDropDownList(Me.DDLFontSize, Nothing)
			.setLabel(Me.LBcharDescription)
		End With
	End Sub

	Public ReadOnly Property CustomDialogScript() As String Implements PresentationLayer.IviewEditor.CustomDialogScript
		Get

		End Get
	End Property


	Public WriteOnly Property ShowAdvancedControlsDocument() As Boolean Implements PresentationLayer.IviewEditor.ShowAddDocument
		Set(ByVal value As Boolean)

		End Set
	End Property
End Class