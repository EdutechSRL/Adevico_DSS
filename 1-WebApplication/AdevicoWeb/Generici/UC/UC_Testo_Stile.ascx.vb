Public Partial Class UC_Testo_Stile
    Inherits BaseControlSession


	Public Property FontFace() As String
		Get
			FontFace = Me.DDLfontFace.SelectedValue
		End Get
		Set(ByVal value As String)
			Try
				If value = String.Empty Or value = "" Then
					Me.DDLfontFace.SelectedIndex = 0
				Else
					Me.DDLfontFace.SelectedValue = value
				End If
			Catch ex As Exception
				Me.DDLfontFace.SelectedIndex = 0
			End Try
		End Set
	End Property
	Public Property FontSize() As String
		Get
			FontSize = Me.DDLfontSize.SelectedValue
		End Get
		Set(ByVal value As String)
			Try
				If value = String.Empty Or value = "" Then
					Me.DDLfontSize.SelectedIndex = 0
				Else
					Me.DDLfontSize.SelectedValue = value
				End If
			Catch ex As Exception
				Me.DDLfontSize.SelectedIndex = 0
			End Try
		End Set
	End Property
	Public Property FontColor() As String
		Get
			FontColor = Me.DDLfontColor.SelectedValue
		End Get
		Set(ByVal value As String)
			Try
				If value = String.Empty Or value = "" Then
					Me.DDLfontColor.SelectedIndex = 0
				Else
					Me.DDLfontColor.SelectedValue = value
				End If
			Catch ex As Exception
				Me.DDLfontColor.SelectedIndex = 0
			End Try
		End Set
	End Property
	Public Property isBold() As Boolean
		Get
			isBold = Me.CBXbold.Checked
		End Get
		Set(ByVal value As Boolean)
			Me.CBXbold.Checked = value
		End Set
	End Property
	Public Property isItalic() As Boolean
		Get
			isItalic = Me.CBXitalic.Checked
		End Get
		Set(ByVal value As Boolean)
			Me.CBXitalic.Checked = value
		End Set
	End Property
	Public Property Testo() As String
		Get
			Testo = Me.TXBcampo.Text
		End Get
		Set(ByVal value As String)
			Me.TXBcampo.Text = value
		End Set
	End Property
	Public Property isRequired() As Boolean
		Get
			isRequired = Me.RFVcampo.Visible
		End Get
		Set(ByVal value As Boolean)
			Me.RFVcampo.Visible = value
			If value Then
				Me.TXBcampo.CssClass = "Testo_campoSmall_obbligatorio"
			Else
				Me.TXBcampo.CssClass = "Testo_campoSmall"
			End If
		End Set
	End Property

	Public Property MaxLength() As Integer
		Get
			MaxLength = Me.TXBcampo.MaxLength
		End Get
		Set(ByVal value As Integer)

		End Set
	End Property
	Public Property TextRows() As Integer
		Get
			TextRows = Me.TXBcampo.Rows
		End Get
		Set(ByVal value As Integer)
			If value = 1 Then
				Me.TXBcampo.Rows = 1
				Me.TXBcampo.TextMode = TextBoxMode.SingleLine
			Else
				Me.TXBcampo.TextMode = TextBoxMode.MultiLine
				Me.TXBcampo.Rows = value
			End If
		End Set
	End Property
	Public Property TextColumns() As Integer
		Get
			TextColumns = Me.TXBcampo.Columns
		End Get
		Set(ByVal value As Integer)
			Me.TXBcampo.Columns = value
		End Set
	End Property

	Public Property ShowStile() As Boolean
		Get
			If Me.DVstile.Style("display") = "block" Then
				ShowStile = True
			Else
				ShowStile = False
			End If
		End Get
		Set(ByVal value As Boolean)
			If value Then
				Me.DVstile.Style("display") = "block"
			Else
				Me.DVstile.Style("display") = "none"
			End If
		End Set
	End Property
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Me.TextRows > 0 Then
			Me.TXBcampo.Attributes.Add("onkeypress", "return(LimitText(this," & Me.MaxLength & "));")
		End If
	End Sub

	Public Overrides Sub BindDati()

	End Sub

	Public Overrides Sub SetCultureSettings()
		MyBase.SetCulture("pg_Cover", "Generici")
	End Sub

	Public Overrides Sub SetInternazionalizzazione()
		With MyBase.Resource
			.setLabel(Me.LBfontFace)
			.setLabel(Me.LBfontSize)
			.setLabel(Me.LBfontColor)
			.setCheckBox(Me.CBXbold)
			.setCheckBox(Me.CBXitalic)
		End With
	End Sub
End Class